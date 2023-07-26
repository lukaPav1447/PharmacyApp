using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Apoteka.Models
{
    [Table("recipetypes")]
    public partial class Recipetype
    {
        public Recipetype()
        {
            Products = new HashSet<Product>();
        }

        [Key]
        [Column("recipetypeid")]
        public int Recipetypeid { get; set; }
        [Column("name")]
        [StringLength(50)]
        public string Name { get; set; } = null!;
        [Column("pricemodifier")]
        [Precision(10, 2)]
        public decimal Pricemodifier { get; set; }

        [InverseProperty("Recipetype")]
        public virtual ICollection<Product> Products { get; set; }
    }
}
