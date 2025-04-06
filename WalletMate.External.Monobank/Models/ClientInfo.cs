using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace WalletMate.External.Monobank.Models;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
public class ClientInfo
{
    public required string ClientId { get; set; }
    public required string Name { get; set; }
    public required List<Account> Accounts { get; set; }
}