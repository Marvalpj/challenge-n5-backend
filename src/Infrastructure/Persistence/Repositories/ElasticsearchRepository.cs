using Application.Interface;
using Confluent.Kafka;
using Domain.Entities;
using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.Nodes;
using ErrorOr;
using Infrastructure.Persistence.Documents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Repositories
{
    public class ElasticsearchRepository : IElasticsearchRepository
    {
        private readonly ElasticsearchClient _elasticClient;
        private readonly string _defaultIndex;

        public ElasticsearchRepository(ElasticClientService elasticClientService)
        {
            _defaultIndex = elasticClientService.DefaultIndex;
            _elasticClient = elasticClientService.Client;
            CreateIndexIfNotExistsAsync(_defaultIndex).GetAwaiter().GetResult();
        }

        public async Task CreateIndexIfNotExistsAsync(string indexName)
        {
            if(!_elasticClient.Indices.Exists(indexName).Exists)
                await _elasticClient.Indices.CreateAsync(indexName);
        }

        public async Task<IEnumerable<Permission>> GetallAsync()
        {
            var response = await _elasticClient.SearchAsync<SimplifiedPermission>(s => s.Index(_defaultIndex));

            if (response.IsValidResponse)
            {
                var doc = response.Documents.ToList();
                
                return doc.Select(permission => new Permission(
                    permission.Id,
                    permission.NameEmployee,
                    permission.LastNameEmployee,
                    permission.PermissionTypeId,
                    permission.Date
                ));
            }

            return null;
        }

        public async Task<Permission> GetByIdAsync(string key)
        {
            var response = await _elasticClient.GetAsync<SimplifiedPermission>(key, idx => idx.Index(_defaultIndex));

            if (response.IsValidResponse)
            {
                SimplifiedPermission permission = response.Source;

                return new Permission(
                    permission.Id,
                    permission.NameEmployee,
                    permission.LastNameEmployee,
                    permission.PermissionTypeId,
                    permission.Date
                );

            }
            return null;
        }

        public async Task<bool> Index(Permission permission)
        {
            SimplifiedPermission permissionDocument = new SimplifiedPermission(
                permission.Id,
                permission.NameEmployee,
                permission.LastNameEmployee,
                permission.PermissionTypeId,
                permission.Date
            );

            var response = await _elasticClient.IndexAsync(permissionDocument);

            return response.IsValidResponse;
        }

        public async Task<bool> UpdateIndex(Permission permission)
        {
            SimplifiedPermission permissionDocument = new SimplifiedPermission(
               permission.Id,
               permission.NameEmployee,
               permission.LastNameEmployee,
               permission.PermissionTypeId,
               permission.Date
           );

            var response = await _elasticClient.UpdateAsync<SimplifiedPermission, SimplifiedPermission>(_defaultIndex, permissionDocument.Id.ToString(), u => u
            .Doc(permissionDocument));


            return response.IsValidResponse;    
        }
    }
}