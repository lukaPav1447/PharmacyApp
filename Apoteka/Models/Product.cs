using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Apoteka.Models
{
    [Table("products")]
    public partial class Product
    {
        public Product()
        {
            Orderitems = new HashSet<Orderitem>();
        }

        [Key]
        [Column("productid")]
        public int Productid { get; set; }
        [Column("name")]
        [StringLength(100)]
        [Required]
        public string Name { get; set; } = null!;
        [Column("description")]
        [Required]
        public string? Description { get; set; }
        [Column("baseprice")]
        [Precision(10, 2)]
        [Required]
        public decimal Baseprice { get; set; }
        [Column("quantity")]
        [Required]
        public int Quantity { get; set; }
        [Column("categoryid")]
        [Required]
        public int? Categoryid { get; set; }
        [Column("recipetypeid")]
        [Required]
        public int? Recipetypeid { get; set; }

        [ForeignKey("Categoryid")]
        [InverseProperty("Products")]
        public virtual Category? Category { get; set; }
        [ForeignKey("Recipetypeid")]
        [InverseProperty("Products")]
        public virtual Recipetype? Recipetype { get; set; }
        [InverseProperty("Product")]
        public virtual ICollection<Orderitem> Orderitems { get; set; }
    }
}
