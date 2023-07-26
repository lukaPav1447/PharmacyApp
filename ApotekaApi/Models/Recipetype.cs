using System;
using System.Collections.Generic;

namespace ApotekaApi.Models
{
    public partial class Recipetype
    {
        public Recipetype()
        {
            Products = new HashSet<Product>();
        }

        public int Recipetypeid { get; set; }
        public string Name { get; set; } = null!;
        public decimal Pricemodifier { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}
