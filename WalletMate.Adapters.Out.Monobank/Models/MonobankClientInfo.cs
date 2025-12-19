using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace WalletMate.Adapters.Out.Monobank.Models;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
public class MonobankClientInfo
{
    public required string ClientId { get; set; }
    public required string Name { get; set; }
    public required List<MonobankAccount> Accounts { get; set; }
}