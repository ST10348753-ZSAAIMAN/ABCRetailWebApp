using Microsoft.Azure.Cosmos.Table;

public class CustomerProfile : TableEntity
{
    // Constructor that uses lastName as PartitionKey and email as RowKey to ensure uniqueness.
    public CustomerProfile(string lastName, string email)
    {
        PartitionKey = lastName;
        RowKey = email; // Using email to ensure uniqueness
    }

    // Default constructor for TableEntity serialization
    public CustomerProfile() { }

    public string Email { get; set; }
    public string PhoneNumber { get; set; }
}
