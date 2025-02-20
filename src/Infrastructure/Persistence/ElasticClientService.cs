using Elastic.Clients.Elasticsearch;
using Elastic.Transport;
using Microsoft.Extensions.Configuration;
using System.Text.Json.Serialization;

namespace Infrastructure.Persistence
{
    public class ElasticClientService
    {
        private readonly ElasticsearchClient _client;
        private readonly string _defaultIndex;
        public ElasticsearchClient Client => _client;
        public string DefaultIndex => _defaultIndex;

        public ElasticClientService(IConfiguration configuration)
        {
            _defaultIndex = configuration["Elasticsearch:defaultIndex"];

            var settings = new ElasticsearchClientSettings(new Uri(configuration["Elasticsearch:uri"]))
                .Authentication(new BasicAuthentication(configuration["Elasticsearch:username"], configuration["Elasticsearch:password"]))
                .DefaultIndex(_defaultIndex);


            _client = new ElasticsearchClient(settings);
        }
    }
}
