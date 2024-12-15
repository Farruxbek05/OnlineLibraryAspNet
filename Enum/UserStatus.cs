using System.ComponentModel;

namespace OnlineLibraryAspNet.Enum
{
    public enum UserStatus
    {
        [Description("User tasdiqlanmoqda")]
        Pending,
        [Description("User aktiv")]
        Active,
        [Description("User blocklangan")]
        Blocked
    }
}
