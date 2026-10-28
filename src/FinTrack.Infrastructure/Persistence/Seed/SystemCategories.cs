using FinTrack.Domain.Entities;

namespace FinTrack.Infrastructure.Persistence.Seed;

internal static class SystemCategories
{
    public static readonly Category[] All =
    [
        Create("019a2a00-0000-7000-8000-000000000001", "Market", "shopping_cart"),
        Create("019a2a00-0000-7000-8000-000000000002", "Restoran & Kafe", "restaurant"),
        Create("019a2a00-0000-7000-8000-000000000003", "Ulaşım", "directions_bus"),
        Create("019a2a00-0000-7000-8000-000000000004", "Faturalar", "receipt_long"),
        Create("019a2a00-0000-7000-8000-000000000005", "Kira", "home"),
        Create("019a2a00-0000-7000-8000-000000000006", "Sağlık", "local_hospital"),
        Create("019a2a00-0000-7000-8000-000000000007", "Eğlence", "movie"),
        Create("019a2a00-0000-7000-8000-000000000008", "Giyim", "checkroom"),
        Create("019a2a00-0000-7000-8000-000000000009", "Eğitim", "school"),
        Create("019a2a00-0000-7000-8000-00000000000a", "Abonelikler", "subscriptions"),
        Create("019a2a00-0000-7000-8000-00000000000b", "Gelir", "payments"),
        Create("019a2a00-0000-7000-8000-00000000000c", "Transfer", "swap_horiz"),
        Create("019a2a00-0000-7000-8000-00000000000d", "Diğer", "more_horiz"),
    ];

    private static Category Create(string id, string name, string icon) =>
        new() { Id = Guid.Parse(id), Name = name, Icon = icon };
}
