using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using OnlineLibraryAspNet.Service;
using OnlineLibraryAspNet.Models;
using OnlineLibraryAspNet.DTOs;
using OnlineLibraryAspNet.Interfice;

namespace OnlineLibraryAspNet.Controllers
{
    [ApiController]
    [Route("lib/[controller]")]
    public class BooksController:ControllerBase
    {
        private readonly IAppService appService;

        public BooksController(IAppService appService)
        {
            this.appService = appService;
        }
        [HttpGet("GetAllBooks")]
        public async Task<IActionResult>GetAllBooks()
        {
            var re = await appService.GetAllBooks();
            return re == null ? NotFound() : Ok(re);
            await appService.GetAllAuthor();

        }
        [HttpGet("GetBook")]
        public async Task<IActionResult>GetBook(int id)
        {
            var re=await appService.GetBook(id);
            return re == null ? NotFound() : Ok(re);
            await appService.GetAllAuthor();
        }
        [HttpPost("CreateBook")]
        public async Task<IActionResult>CreateBook(BooksDto booksDto)
        {
            var res = await appService.CreateBook(booksDto);
            return res==null ? NotFound() : Ok(res);
        }
        [HttpPut("UpdateBook")]
        public async Task<IActionResult>UpdateBook(Books books)
        {
            var res= await appService.UpdateBook(books);
            return res==null ? NotFound() : Ok(res) ;
        }
        [HttpDelete("DeleteBook")]
        public async Task DeleteBook(int id)
        {
            await  appService.DeleteBook(id);
            
        }
    }
}
