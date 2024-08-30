using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

public class BlobStorageService
{
    private readonly BlobServiceClient _blobServiceClient;
    private readonly BlobContainerClient _blobContainerClient;

    public BlobStorageService(IConfiguration configuration)
    {
        _blobServiceClient = new BlobServiceClient(configuration.GetConnectionString("AzureStorage"));
        _blobContainerClient = _blobServiceClient.GetBlobContainerClient("multimedia");
        _blobContainerClient.CreateIfNotExists();
    }

    public async Task UploadBlobAsync(string blobName, Stream stream)
    {
        var blobClient = _blobContainerClient.GetBlobClient(blobName);
        await blobClient.UploadAsync(stream, true);
    }

    public async Task<IEnumerable<BlobItem>> ListBlobsAsync()
    {
        var blobs = new List<BlobItem>();
        await foreach (var blobItem in _blobContainerClient.GetBlobsAsync())
        {
            blobs.Add(new BlobItem { BlobName = blobItem.Name, BlobUrl = _blobContainerClient.GetBlobClient(blobItem.Name).Uri.ToString() });
        }
        return blobs;
    }

    public async Task<Stream> DownloadBlobAsync(string blobName)
    {
        var blobClient = _blobContainerClient.GetBlobClient(blobName);
        BlobDownloadInfo download = await blobClient.DownloadAsync();
        return download.Content;
    }

    public async Task DeleteBlobAsync(string blobName)
    {
        var blobClient = _blobContainerClient.GetBlobClient(blobName);
        await blobClient.DeleteIfExistsAsync();
    }

    public async Task<bool> BlobExistsAsync(string blobName)
    {
        var blobClient = _blobContainerClient.GetBlobClient(blobName);
        var response = await blobClient.ExistsAsync();
        return response.Value;
    }
}
