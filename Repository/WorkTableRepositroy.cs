using OnlineLibraryAspNet.Context;
using OnlineLibraryAspNet.IRepository;
using OnlineLibraryAspNet.Models;

namespace OnlineLibraryAspNet.Repository
{
    public class WorkTableRepositroy:GenericRepository<WorkTable>,IWorkTableRepository
    {
        private readonly AppDbContext _appDbContext;
        public WorkTableRepositroy(AppDbContext appDbContext):base(appDbContext)
        {
            _appDbContext = appDbContext;
        }


    }
}
