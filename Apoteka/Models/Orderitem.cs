using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Apoteka.Models
{
    [Table("orderitems")]
    public partial class Orderitem
    {
        [Key]
        [Column("orderitemid")]
        public int Orderitemid { get; set; }
        [Column("orderid")]
        public int Orderid { get; set; }
        [Column("productid")]
        public int Productid { get; set; }
        [Column("quantity")]
        public int Quantity { get; set; }
        [Column("price")]
        [Precision(10, 2)]
        public decimal Price { get; set; }

        [ForeignKey("Orderid")]
        [InverseProperty("Orderitems")]
        public virtual Order Order { get; set; } = null!;
        [ForeignKey("Productid")]
        [InverseProperty("Orderitems")]
        public virtual Product Product { get; set; } = null!;
    }
}
