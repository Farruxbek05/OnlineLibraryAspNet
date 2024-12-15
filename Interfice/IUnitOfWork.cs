namespace OnlineLibraryAspNet.Interfice
{
    public interface IUnitOfWork:IDisposable
    {
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default); // Barcha o'zgarishlarni saqlash
    }
}
