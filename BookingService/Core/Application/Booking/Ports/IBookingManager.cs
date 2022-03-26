using Application.Booking.Dtos;

namespace Application.Booking.Ports
{
    public interface IBookingManager
    {
        Task<BookingDto> CreateBooking(BookingDto booking);
        Task<BookingDto> GetBooking(int id);
    }
}
