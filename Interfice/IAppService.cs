using OnlineLibraryAspNet.DTOs;
using OnlineLibraryAspNet.Models;

namespace OnlineLibraryAspNet.Interfice
{
    public interface IAppService
    {
        Task<IEnumerable<Author>> GetAllAuthor();
        Task<Author> GetAuthor(int id);
        Task<Author> CreateAuthor(AuthorDto author);
        Task<Author> UpdateAuthor(Author author);
        Task DeleteAuthor(int id);

        Task<IEnumerable<Books>> GetAllBooks();
        Task<Books> GetBook(int id);
        Task<Books> CreateBook(BooksDto booksDto);
        Task<Books> UpdateBook(Books books);
        Task DeleteBook(int id);

        Task<IEnumerable<Customer>> GetAllCustomers();
        Task<Customer> GetCustomer(int id);
        Task<Customer> CreateCustomer(CustomerDto customer);
        Task<Customer> UpdateCustomer(Customer customer);
        Task DeleteCustomer(int id);

        Task<IEnumerable<WorkTable>> GetAllworkTable();
        Task<WorkTable> GetWorkTable(int id);
        Task<WorkTable> CreateWorkTable(WorkTableDto workTable);
        Task<WorkTable> UpdateWorkTable(WorkTable workTable);
        Task DeleteWorkTable(int id);

    }
}
