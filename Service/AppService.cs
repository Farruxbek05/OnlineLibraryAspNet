using Microsoft.AspNetCore.Mvc;
using OnlineLibraryAspNet.DTOs;
using OnlineLibraryAspNet.Interfice;
using OnlineLibraryAspNet.IRepository;
using OnlineLibraryAspNet.Models;

namespace OnlineLibraryAspNet.Service
{
   
    public class AppService:IAppService
    {
        private readonly IAuthorRepository _authorRepository;
        private readonly IBooksRepository _booksRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IWorkTableRepository _workTableRepository;
        public AppService(IAuthorRepository authorRepository, IBooksRepository booksRepository, ICustomerRepository customerRepository, IWorkTableRepository workTableRepository)
        {
            _authorRepository = authorRepository;
            _booksRepository = booksRepository;
            _customerRepository = customerRepository;
            _workTableRepository = workTableRepository;
        }
        #region Author
        public async Task<IEnumerable<Author>> GetAllAuthor()
        {
            return await _authorRepository.GetAll(null);
        }
        public async Task<Author>GetAuthor(int id)
        {
            return (await _authorRepository.GetAll(x => x.Id == id)).ToList().FirstOrDefault();
        }
        public async Task<Author>CreateAuthor(AuthorDto author)
        {
            Author author1 = new Author()
            {
                FirstName = author.FirstName,
                LastName = author.LastName,
                Age = author.Age,
            };
            return await _authorRepository.CreateAsync(author1);
        }
        public async Task<Author>UpdateAuthor(Author author)
        {
            var res= await _authorRepository.GetAll(x=>x.Id == author.Id);
            Author author1= res.FirstOrDefault();
            author1.FirstName = author.FirstName;
            author1.LastName = author.LastName;
            author1.Age = author.Age;
            return await _authorRepository.UpdateAsync(author1);    
        }
        public async Task DeleteAuthor(int id)
        {
            var res=await _authorRepository.GetAll(x=>x.Id == id);
            Author author= res.FirstOrDefault();
            await _authorRepository.DeleteAsync(author); 
        }
        #endregion
        #region Books

        public async Task<IEnumerable<Books>>GetAllBooks()
        {
            return await _booksRepository.GetAll(null);
        }
        public async Task<Books>GetBook(int id)
        {
            return (await _booksRepository.GetAll(x=>x.Id==id)).ToList().FirstOrDefault();
        }
        public async Task<Books>CreateBook(BooksDto booksDto)
        {
            Books books = new Books()
            {
                Jenre = booksDto.Jenre,
                AuthorId = booksDto.AuthorId,
                Name = booksDto.Name,
                Price = booksDto.Price,

            };
            return await _booksRepository.CreateAsync(books);
        }
        public async Task<Books>UpdateBook(Books books)
        {
            var res=await _booksRepository.GetAll(x=>x.Id!=books.Id);
            Books book= res.FirstOrDefault();
            book.Jenre = books.Jenre;
            book.AuthorId = books.AuthorId;
            book.Name = books.Name;
            book.Price = books.Price;
            return await _booksRepository.UpdateAsync(books);   
        }
        public async Task DeleteBook(int id)
        {
            var res=await _booksRepository.GetAll(x=>x.Id== id);
            Books books= res.FirstOrDefault();
             await _booksRepository.DeleteAsync(books);
        }
        #endregion
        #region Customer
        public async Task<IEnumerable<Customer>>GetAllCustomers()
        {
            return await _customerRepository.GetAll(null);
        }
        public async Task<Customer>GetCustomer(int id)
        {
             return (await _customerRepository.GetAll(x=> x.Id== id)).ToList().FirstOrDefault();
        }
        public async Task<Customer>CreateCustomer(CustomerDto customer)
        {
            Customer customer2 = new Customer()
            {
                Enail = customer.Enail,
                Name = customer.Name,
                PhoneNumber = customer.PhoneNumber,
            };
            return await _customerRepository.CreateAsync(customer2);
        }
        public async Task<Customer>UpdateCustomer(Customer customer)
        {
            var res= await _customerRepository.GetAll(x=>x.Id == customer.Id);
            Customer customer2 = res.FirstOrDefault();
            customer2.PhoneNumber= customer.PhoneNumber;
            customer2.Name= customer.Name;
            customer2.Enail= customer.Enail;
            return await _customerRepository.UpdateAsync(customer2);        
        }
        public async Task DeleteCustomer(int id)
        {
            var res=await _customerRepository.GetAll(x=>x.Id== id);
            Customer customer = res.FirstOrDefault();
            await _customerRepository.DeleteAsync(customer);
        }
        #endregion
        #region WorkTable
        public async Task<IEnumerable<WorkTable>>GetAllworkTable()
        {
            return await _workTableRepository.GetAll(null);
        }
        public async Task<WorkTable>GetWorkTable(int id)
        {
            return (await _workTableRepository.GetAll(x=>x.Id== id)).ToList().FirstOrDefault();
        }
        public async Task<WorkTable>CreateWorkTable(WorkTableDto workTable)
        {
            WorkTable workTable1 = new WorkTable()
            {
                Time = workTable.Time,
                BookId = workTable.BookId,
                CustomerId = workTable.CustomerId,
            };
            return await _workTableRepository.CreateAsync(workTable1);
        }
        public async Task<WorkTable>UpdateWorkTable(WorkTable workTable)
        {
            var res= await _workTableRepository.GetAll(x=> x.Id== workTable.Id);
            WorkTable workTable2 = res.FirstOrDefault();
            workTable2.Time = workTable.Time;
            workTable2.CustomerId = workTable.CustomerId;
            workTable2.BookId = workTable.BookId;
            return await _workTableRepository.UpdateAsync(workTable2);
        }
        public async Task DeleteWorkTable(int id)
        {
            var res= await _workTableRepository.GetAll(x=>x.Id == id);
            WorkTable workTable = res.FirstOrDefault();
            await _workTableRepository.DeleteAsync(workTable);
        }

        #endregion
    }
}
