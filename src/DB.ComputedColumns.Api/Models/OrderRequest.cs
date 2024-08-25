namespace DB.TemporalTable.Api.Models
{
    public record OrderRequest(string Name, string Description, decimal UnitPrice, int Quantity);
}
