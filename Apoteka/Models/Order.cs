using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Apoteka.Models
{
    [Table("orders")]
    public partial class Order
    {
        public Order()
        {
            Orderitems = new HashSet<Orderitem>();
        }

        [Key]
        [Column("orderid")]
        public int Orderid { get; set; }
        [Column("orderdate")]
        public DateTime Orderdate { get; set; }
        [Column("totalamount")]
        [Precision(10, 2)]
        public decimal Totalamount { get; set; }

        [InverseProperty("Order")]
        public virtual ICollection<Orderitem> Orderitems { get; set; }
    }
}
