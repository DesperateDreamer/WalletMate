using System.Text.Json.Serialization;

namespace WalletMate.BLL.Shared.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum TransactionSortOption
{
    Date,
    Amount,
    Currency
}