namespace DB.TemporalTable.Api.Models
{
    public record OrderResponse(int Id, string Product, string Description, decimal UnitPrice, int Quantity, string CreatedBy, DateTime PeriodStart, DateTime PeriodEnd);
}
