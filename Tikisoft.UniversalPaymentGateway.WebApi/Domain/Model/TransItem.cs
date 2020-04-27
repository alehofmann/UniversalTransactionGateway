using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TikiSoft.UniversalPaymentGateway.Domain.Model
{
    public class TransItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long TransItemId { get; set; }

        [Required] 
        public int Quantity { get; set; }

        public string Description { get; set; }
        
        [Required]  
        public decimal UnitPrice { get; set; }

        public TransactionRequest TransDef { get; set; }

        public long TransDefId { get; set; }
    }
}
