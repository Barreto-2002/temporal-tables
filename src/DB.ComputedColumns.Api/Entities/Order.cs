namespace DB.TemporalTable.Api.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public string Product { get; set; }
        public string Description { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public string CreatedBy { get; set; }
    }
}
