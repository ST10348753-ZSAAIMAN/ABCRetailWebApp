using Microsoft.Azure.Cosmos.Table;

public class CustomerProfile : TableEntity
{
    public CustomerProfile(string lastName, string firstName)
    {
        PartitionKey = lastName;
        RowKey = firstName;
    }

    public CustomerProfile() { }

    public string Email { get; set; }
    public string PhoneNumber { get; set; }
}
