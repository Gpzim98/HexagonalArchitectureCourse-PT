using Application.Booking.Ports;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Booking.Commands
{
    public class CreateBookingCommandHandler : IRequestHandler<CreateBookingCommand, BookingResponse>
    {
        IBookingManager _bookingManager;
        public CreateBookingCommandHandler(IBookingManager bookingManager)
        {
            _bookingManager = bookingManager;
        }
        public Task<BookingResponse> Handle(CreateBookingCommand request, CancellationToken cancellationToken)
        {
            return _bookingManager.CreateBooking(request.BookingDto);
        }
    }
}
