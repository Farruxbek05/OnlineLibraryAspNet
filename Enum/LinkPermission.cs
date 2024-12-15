using System.ComponentModel;

namespace OnlineLibraryAspNet.Enum
{
    public enum LinkPermission
    {
        [Description("Barcha Uchun ochiq")]
        Public = 0,
        [Description("Faqat Tizimga kirgan foydalanuvchilar uchun ochiq")]
        Private = 1,
        [Description("Faqat fayl egasi uchun ochiq")]
        Internal = 2
    }
}
