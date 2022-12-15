using Application.Booking.Dtos;
using Application.Payment;
using Domain.Booking.Ports;
using Domain.Ports;
using Domain.Room.Ports;
using MediatR;

namespace Application.Booking.Queries
{
    public class GetBookingQueryHandler : IRequestHandler<GetBookingQuery, BookingResponse>
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IRoomRepository _roomRepository;
        private readonly IGuestRepository _guestRepository;
        private readonly IPaymentProcessorFactory _paymentProcessorFactory;
        public GetBookingQueryHandler(IBookingRepository bookingRepository,
            IRoomRepository roomRepository,
            IGuestRepository guestRepository,
            IPaymentProcessorFactory paymentProcessorFactory)
        {
            _roomRepository = roomRepository;
            _guestRepository = guestRepository;
            _bookingRepository = bookingRepository;
            _paymentProcessorFactory = paymentProcessorFactory;
        }
        public async Task<BookingResponse> Handle(GetBookingQuery request, CancellationToken cancellationToken)
        {
            var booking = await _bookingRepository.Get(request.Id);
            var bookingDto = BookingDto.MapToDto(booking);
            return new BookingResponse
            {
                Success = true,
                Data = bookingDto
            };
        }
    }
}
