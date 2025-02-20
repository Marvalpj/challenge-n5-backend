using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interface
{
    public interface IKafkaProducer
    {
        Task ProduceMessage(string topic, string operationType, string content = "");
    }
}
