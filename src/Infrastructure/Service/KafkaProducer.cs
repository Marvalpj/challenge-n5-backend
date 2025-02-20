using Application.Interface;
using Confluent.Kafka;
using Infrastructure.Persistence;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Service
{
    public class KafkaProducer : IKafkaProducer
    {
        private readonly IProducer<Null, string> producer;
        public KafkaProducer(string bootstrapServers)
        {
            var config = new ProducerConfig
            {
                BootstrapServers = bootstrapServers,
                EnableIdempotence = true,
                MessageSendMaxRetries = 2
            };

            producer = new ProducerBuilder<Null, string>(config).Build();
        }
        public async Task ProduceMessage(string topic, string operationType, string content = "")
        {
            try
            {
                KafkaMessage kafkaMessage = new KafkaMessage()
                {
                    Id = Guid.NewGuid(),
                    NameOperation = operationType,
                    Content = content
                };

                await producer.ProduceAsync(topic, new Message<Null, string> { Value = JsonConvert.SerializeObject(kafkaMessage) });
            }
            catch (ProduceException<Null, string> ex)
            {
                Console.WriteLine($"Error al enviar el mensaje to kafka: {ex.Error.Reason}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error inesperado: {ex.Message}");
            }
        }
    }
}
