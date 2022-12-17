using Application.Responses;
using Application.Room.Commands;
using Application.Room.Dtos;
using Application.Room.Ports;
using Application.Room.Request;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class RoomController : ControllerBase
    {
        private readonly ILogger<GuestsController> _logger;
        private readonly IRoomManager _roomManager;
        private readonly IMediator _mediator;

        public RoomController(
            ILogger<GuestsController> logger,
            IRoomManager roomManager,
            IMediator mediator)
        {
            _logger = logger;
            _roomManager = roomManager;
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<ActionResult<RoomDto>> Post(RoomDto room)
        {
            var request = new CreateRoomCommand
            {
                RoomDto = room
            };

            var res = await _mediator.Send(request);

            if (res.Success) return Created("", res.Data);

            else if (res.ErrorCode == ErrorCodes.ROOM_MISSING_REQUIRED_INFORMATION)
            {
                return BadRequest(res);
            }
            else if (res.ErrorCode == ErrorCodes.ROOM_COULD_NOT_STORE_DATA)
            {
                return BadRequest(res);
            }

            _logger.LogError("Response with unknown ErrorCode Returned", res);
            return BadRequest(500);
        }

        //[HttpGet]
        //public async Task<ActionResult<GuestDTO>> Get(int guestId)
        //{
        //    var res = await _guestManager.GetGuest(guestId);

        //    if (res.Success) return Ok(res.Data);

        //    return NotFound(res);
        //}
    }
}
