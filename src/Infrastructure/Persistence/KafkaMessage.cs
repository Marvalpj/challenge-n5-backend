using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence
{
    public class KafkaMessage
    {
        public Guid Id { get; set; }
        public string NameOperation { get; set; }
        public string Content { get; set; }
    }
}
