using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MojDom.Core.Entities
{
    public class MaterialUsed
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }  // maknuli expression, EF će računati
        public int TaskCompletionId { get; set; }
        public TaskCompletion TaskCompletion { get; set; } = null!;
    }
}