using Application.Booking.Dtos;
using Application.Booking.Ports;
using Application.Responses;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BookingController : ControllerBase
    {
        private readonly IBookingManager _bookingManager;
        private readonly ILogger<BookingController> _logger;

        public BookingController(
            IBookingManager bookingManager,
            ILogger<BookingController> logger)
        {
            _bookingManager = bookingManager;
            _logger = logger;
        }

        [HttpPost]
        public async Task<ActionResult<BookingDto>> Post(BookingDto booking)
        {
            var res = await _bookingManager.CreateBooking(booking);

            if (res.Success) return Created("", res.Data);

            else if (res.ErrorCode == ErrorCodes.BOOKING_MISSING_REQUIRED_INFORMATION)
            {
                return BadRequest(res);
            }
            else if (res.ErrorCode == ErrorCodes.BOOKING_COULD_NOT_STORE_DATA)
            {
                return BadRequest(res);
            }
            else if (res.ErrorCode == ErrorCodes.BOOKING_ROOM_CANNOT_BE_BOOKED)
            {
                return BadRequest(res);
            }

            _logger.LogError("Response with unknown ErrorCode Returned", res);
            return BadRequest(500);
        }
    }
}
