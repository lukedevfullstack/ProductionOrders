namespace ProductionOrdersSequor.Domain.Entities;

public class Production
{
    public long ID { get; private set; }
    public string Email { get; private set; }
    public string Order { get; private set; }
    public DateTime Date { get; private set; }
    public decimal Quantity { get; private set; }
    public string MaterialCode { get; private set; }
    public decimal CycleTime { get; private set; }

    public Production(string email, string order, DateTime date, decimal quantity, string materialCode, decimal cycleTime)
    {
        Email = email;
        Order = order;
        Date = date;
        Quantity = quantity;
        MaterialCode = materialCode;
        CycleTime = cycleTime;
    }
}