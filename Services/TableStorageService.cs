using Microsoft.Azure.Cosmos.Table;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;

public class TableStorageService
{
    private readonly CloudTableClient _tableClient;
    private readonly CloudTable _customerTable;

    public TableStorageService(IConfiguration configuration)
    {
        var storageAccount = CloudStorageAccount.Parse(configuration.GetConnectionString("AzureStorage"));
        _tableClient = storageAccount.CreateCloudTableClient(new TableClientConfiguration());
        _customerTable = _tableClient.GetTableReference("CustomerProfile");
        _customerTable.CreateIfNotExists();
    }

    public async Task InsertCustomerProfileAsync(CustomerProfile profile)
    {
        var insertOperation = TableOperation.Insert(profile);
        await _customerTable.ExecuteAsync(insertOperation);
    }

    public async Task<CustomerProfile> GetCustomerProfileAsync(string partitionKey, string rowKey)
    {
        var retrieveOperation = TableOperation.Retrieve<CustomerProfile>(partitionKey, rowKey);
        var result = await _customerTable.ExecuteAsync(retrieveOperation);
        return result.Result as CustomerProfile;
    }

    public async Task<List<CustomerProfile>> GetAllCustomerProfilesAsync()
    {
        var query = new TableQuery<CustomerProfile>();
        var result = new List<CustomerProfile>();
        TableContinuationToken token = null;

        do
        {
            var queryResult = await _customerTable.ExecuteQuerySegmentedAsync(query, token);
            result.AddRange(queryResult.Results);
            token = queryResult.ContinuationToken;
        } while (token != null);

        return result;
    }

    public async Task UpdateCustomerProfileAsync(CustomerProfile profile)
    {
        var updateOperation = TableOperation.Replace(profile);
        await _customerTable.ExecuteAsync(updateOperation);
    }

    public async Task DeleteCustomerProfileAsync(string partitionKey, string rowKey)
    {
        var retrieveOperation = TableOperation.Retrieve<CustomerProfile>(partitionKey, rowKey);
        var result = await _customerTable.ExecuteAsync(retrieveOperation);
        var deleteEntity = result.Result as CustomerProfile;

        if (deleteEntity != null)
        {
            var deleteOperation = TableOperation.Delete(deleteEntity);
            await _customerTable.ExecuteAsync(deleteOperation);
        }
    }
}
