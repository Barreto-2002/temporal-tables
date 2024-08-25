namespace DB.TemporalTable.Api.Models
{
    public record OrderResponse(int Id, string Name, string Description, decimal UnitPrice, int Quantity, decimal? RowTotal,DateTime PeriodStart, DateTime PeriodEnd);
}
