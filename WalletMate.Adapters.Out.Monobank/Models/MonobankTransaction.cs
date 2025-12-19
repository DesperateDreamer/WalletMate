using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace WalletMate.Adapters.Out.Monobank.Models;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
public class MonobankTransaction
{
    public required string Id { get; set; }
    public required long Time { get; set; }
    public required string Description { get; set; }
    public required int Mcc { get; set; }
    public required long Amount { get; set; }
    public required int CurrencyCode { get; set; }
    public required long Balance { get; set; }
    public required long CashbackAmount { get; set; }
    public string? Comment { get; set; }
}