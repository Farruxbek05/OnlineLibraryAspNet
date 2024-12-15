using Microsoft.AspNetCore.Mvc;
using OnlineLibraryAspNet.DTOs;
using OnlineLibraryAspNet.Interfice;
using OnlineLibraryAspNet.Models;
using OnlineLibraryAspNet.Service;

namespace OnlineLibraryAspNet.Controllers
{
        [ApiController]
        [Route("[controller]")]
    public class CustomerController:ControllerBase
    {
        private readonly IAppService appService;

        public CustomerController(IAppService appService)
        {
            this.appService = appService;
        }
        [HttpGet("GetAllCustomers")]
        public async Task<IActionResult>GetAllCustomers()
        {
            var res=await appService.GetAllBooks();
            return res==null ? NotFound(res) : Ok(res);
        }
        [HttpGet("GetCustomer")]
        public async Task<IActionResult>GetCustomer(int id)
        {
            var res= await appService.GetCustomer(id);
            return res==null ? NotFound(res) : Ok(res) ;
        }
        [HttpPost("CreateCustomer")]
        public async Task<IActionResult>CreateCustomer(CustomerDto dto)
        {
            var res = await appService.CreateCustomer(dto);
            return res == null ? NotFound(res) : Ok(res);
        }
        [HttpPut("UpdateCustomer")]
        public async Task<IActionResult>UpdateCustomer(Customer customer)
        {
            var res= await appService.UpdateCustomer(customer);
            return res == null ? NotFound(res) : Ok(res);
        }
        [HttpDelete("DeleteCustomer")]
        public async Task DeleteCustomer(int id)
        {
            await appService.DeleteCustomer(id);                                            
        }
    }
}
