using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace WalletMate.Adapters.Out.Monobank.Models;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
public class MonobankAccount
{
    public required string Id { get; set; }
    public long Balance { get; set; }
    public required string Type { get; set; }
    public required int CurrencyCode { get; set; }
    public required string Iban { get; set; }
}