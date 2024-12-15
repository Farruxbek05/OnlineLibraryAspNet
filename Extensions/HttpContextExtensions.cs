namespace OnlineLibraryAspNet.Extensions
{
    public static class HttpContextExtensions
    {
        public static string GetClientIpAddress(this HttpContext context)
        {
            // X-Forwarded-For sarlavhasidan IP manzilini olish
            var forwardedFor = context.Request.Headers["X-Forwarded-For"].FirstOrDefault();
            if (!string.IsNullOrEmpty(forwardedFor))
            {
                // IP manzillarni vergul bilan ajratib, birinchi IP manzilini qaytaradi
                return forwardedFor.Split(',').FirstOrDefault()?.Trim();
            }

            // Agar X-Forwarded-For mavjud bo'lmasa, remote IP ni olish
            return context.Connection.RemoteIpAddress?.ToString();
        }
    }
}
