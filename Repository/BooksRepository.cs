using OnlineLibraryAspNet.Context;
using OnlineLibraryAspNet.IRepository;
using OnlineLibraryAspNet.Models;

namespace OnlineLibraryAspNet.Repository
{
    public class BooksRepository : GenericRepository<Books>, IBooksRepository
    {
        private readonly AppDbContext _context;
        public BooksRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
