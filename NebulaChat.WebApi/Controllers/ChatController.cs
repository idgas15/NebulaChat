using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NebulaChat.Core.Dto;
using NebulaChat.Core.Models;
using NebulaChat.Services;
using NebulaChat.WebApi.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NebulaChat.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ChatController : ControllerBase
    {
        private readonly IMessageService messageService;
        private readonly INotifyService notifyService;
        private readonly IMapper mapper;

        public ChatController(INotifyService notifyService, IMessageService messageService, IMapper mapper)
        {
            this.notifyService = notifyService;
            this.messageService = messageService;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var messages = mapper.Map<IEnumerable<SendMessageNotification>>(await messageService.GetMessagesAsync());
            return Ok(messages);
        }
        [HttpPost]
        public async Task<IActionResult> AddMessage([FromBody]CreateMessageRequest messageRequest)
        {
            var message = mapper.Map<Message>(messageRequest);
            await messageService.CreateMessageAsync(message);
            return Ok();
        }
    }
}
