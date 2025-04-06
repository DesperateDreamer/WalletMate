using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace WalletMate.External.Monobank.Models;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
public class Transaction
{
    public required string Id { get; set; }
    public required string Time { get; set; }
    public required string Description { get; set; }
    public required string Mcc { get; set; }
    public required long Amount { get; set; }
    public required string Currency { get; set; }
    public required long Balance { get; set; }
    public required long CashbackAmount { get; set; }
    public string? Comment { get; set; }
}