namespace ProductionOrdersSequor.Domain.Entities;

public class User
{
    public string Email { get; private set; }
    public string Name { get; private set; }
    public DateTime InitialDate { get; private set; }
    public DateTime EndDate { get; private set; }

    public User(string email, string name, DateTime initialDate, DateTime endDate)
    {
        Email = email;
        Name = name;
        InitialDate = initialDate;
        EndDate = endDate;
    }

    public bool IsActiveAt(DateTime date)
    {
        return date >= InitialDate && date <= EndDate;
    }
}
