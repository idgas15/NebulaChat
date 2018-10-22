using Microsoft.AspNetCore.Mvc;
using NebulaChat.WebApi.Services;
using System.Threading.Tasks;

namespace NebulaChat.WebApi.Controllers
{
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly IMessageService messageService;
        public DashboardController(IMessageService messageService)
        {
            this.messageService = messageService;
        }
        // GET: api/Dashboard
        [HttpGet("api/dashboard/topten")]
        public async Task<IActionResult> GetTopTen()
        {
            var topTen = await messageService.GetTopTenChatters();
            return Ok(topTen);
        }
    }
}
