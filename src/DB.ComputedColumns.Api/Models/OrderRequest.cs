namespace DB.TemporalTable.Api.Models
{
    public record OrderRequest(string Product, string Description, decimal UnitPrice, int Quantity);
}
