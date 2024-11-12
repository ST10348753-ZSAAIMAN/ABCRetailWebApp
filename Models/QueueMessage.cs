public class QueueMessage
{
    public string? MessageId { get; set; }  // Make MessageId nullable
    public string? PopReceipt { get; set; } // Make PopReceipt nullable
    public string? Content { get; set; }    // Make Content nullable
}
