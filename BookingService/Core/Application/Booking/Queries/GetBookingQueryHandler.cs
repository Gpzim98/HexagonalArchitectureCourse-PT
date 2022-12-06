using Application.Booking.Dtos;
using Domain.Booking.Ports;
using MediatR;

namespace Application.Booking.Queries
{
    public class GetBookingQueryHandler : IRequestHandler<GetBookingQuery, BookingResponse>
    {
        public readonly IBookingRepository _bookingRepository;
        public GetBookingQueryHandler(IBookingRepository bookingRepository)
        {
            _bookingRepository = bookingRepository;
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
