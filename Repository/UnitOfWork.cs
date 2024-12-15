using Microsoft.EntityFrameworkCore;
using OnlineLibraryAspNet.Interfice;

namespace OnlineLibraryAspNet.Repository
{
    public class UnitOfWork :IDisposable, IUnitOfWork
    {
        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        private bool disposed = false;

        // Resurslar bilan ishlash uchun boshqa kodlar

        public void Dispose()
        {
            // Agar allaqachon Dispose qilinmagan bo'lsa
            if (!disposed)
            {
                // Resurslarni tozalash va yig'ish
                // Masalan, ma'lumotlar bazasi konteksini tozalash
                //if (AppContext != null)
                //{
                //    dbContext.Dispose();
                //}

                // Resurslarni qo'shimcha tozalash

                disposed = true;
            }

            // Manba tozalashni aniq ko'rsatish uchun
            GC.SuppressFinalize(this);
        }
    }
}
