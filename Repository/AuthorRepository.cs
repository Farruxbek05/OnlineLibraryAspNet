using OnlineLibraryAspNet.Context;
using OnlineLibraryAspNet.IRepository;
using OnlineLibraryAspNet.Models;

namespace OnlineLibraryAspNet.Repository
{
    public class AuthorRepository : GenericRepository<Author>, IAuthorRepository
    {
        private readonly AppDbContext _context;
        public AuthorRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
