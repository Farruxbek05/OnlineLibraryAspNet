using OnlineLibraryAspNet.Context;
using OnlineLibraryAspNet.IRepository;
using OnlineLibraryAspNet.Models;

namespace OnlineLibraryAspNet.Repository
{
    public class CutomerRepository:GenericRepository<Customer>,ICustomerRepository
    {
        private readonly AppDbContext _appDbContext;

        public CutomerRepository(AppDbContext context) : base(context)
        {
            _appDbContext = context;
        }
    }
}
