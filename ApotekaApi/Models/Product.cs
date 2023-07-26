using System.ComponentModel.DataAnnotations;

namespace ApotekaApi.Models
{
    public partial class Product
    {
        public Product()
        {
            Orderitems = new HashSet<Orderitem>();
        }
        [Key]
        public int Productid { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public decimal Baseprice { get; set; }
        public int Quantity { get; set; }
        public int? Categoryid { get; set; }
        public int? Recipetypeid { get; set; }

        public virtual Category? Category { get; set; }
        public virtual Recipetype? Recipetype { get; set; }
        public virtual ICollection<Orderitem> Orderitems { get; set; }
    }
}
