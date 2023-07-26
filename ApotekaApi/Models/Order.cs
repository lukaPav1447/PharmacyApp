namespace ApotekaApi.Models
{
    public partial class Order
    {
        public Order()
        {
            Orderitems = new HashSet<Orderitem>();
        }

        public int Orderid { get; set; }
        public DateTime Orderdate { get; set; }
        public decimal Totalamount { get; set; }

        public virtual ICollection<Orderitem> Orderitems { get; set; }
    }
}
