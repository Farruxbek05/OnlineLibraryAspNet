using Microsoft.AspNetCore.Mvc;
using Npgsql.EntityFrameworkCore.PostgreSQL.Storage.Internal.Mapping;
using OnlineLibraryAspNet.DTOs;
using OnlineLibraryAspNet.Interfice;
using OnlineLibraryAspNet.Models;
using OnlineLibraryAspNet.Service;

namespace OnlineLibraryAspNet.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WorkTableController:ControllerBase
    {
        private readonly IAppService appService;

        public WorkTableController(IAppService appService)
        {
            this.appService = appService;
        }
        [HttpGet("GetAllWorkTables")]
        public async Task<IActionResult>GetAllWorkTAbles()
        {
            var res = await appService.GetAllworkTable();
            return res ==null ? NotFound() : Ok(res);
        }
        [HttpGet("GetWorkTable")]
        public async Task<IActionResult>GetWorkTable(int id)
        {
            var res=await appService.GetWorkTable(id);
            return  res == null ? NotFound() : Ok(res); 
        }
        [HttpPost("CreateWorkTable")]
        public async Task<IActionResult>CreateWorkTable(WorkTableDto workTableDto)
        {
            var res = await appService.CreateWorkTable(workTableDto);
            return res==null ? NotFound() : Ok(res);    
        }
        [HttpPut("UpdateWorkTable")]
        public async Task<IActionResult>UpdateWorkTable(WorkTable workTable)
        {
            var res= await appService.UpdateWorkTable(workTable);
            return res == null ? NotFound() : Ok(res);
        }
        [HttpDelete("DeleteWorkTable")]
        public async Task DeleteWorkTable(int id)
        {
            await appService.DeleteWorkTable(id);
        }
    }
}
