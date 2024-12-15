using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineLibraryAspNet.DTOs;
using OnlineLibraryAspNet.Interfice;
using OnlineLibraryAspNet.Models;
using OnlineLibraryAspNet.Service;

namespace OnlineLibraryAspNet.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthorController:ControllerBase
    {
        private readonly IAppService appService;

        public AuthorController(IAppService appService)
        {
            this.appService = appService;
        }
        [HttpGet("GetAllAuthor")]
        public async Task<IActionResult> GetAllAuthor()
        {
            var re = await appService.GetAllAuthor();
            return re == null ? NotFound() : Ok(re);
        }
        [HttpGet("GetAuthor")]
        public async Task<IActionResult>GetAuthor(int id)
        {
            var re= await appService.GetAuthor(id);
            return re == null ? NotFound() : Ok(re);
        }
        [HttpPost("CreateAuthor")]
        public async Task<IActionResult>CreateAuthor(AuthorDto authorDto)
        {
            var res = await appService.CreateAuthor( authorDto);
            return res== null ? NotFound() : Ok(res);

        }
        [HttpPut("UpdateAuthor")]
        public async Task<IActionResult>UpdateAuthor(Author author)
        {
            var res= await appService.UpdateAuthor(author);
            return res == null ? NotFound() : Ok(res);
        }
        [HttpDelete("DeleteAuthor")]
        public async Task DeleteAuthor(int id)
        {
             await appService.DeleteAuthor(id);
        
        }




    }
}
