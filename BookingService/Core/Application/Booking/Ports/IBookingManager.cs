using Application.Booking.Dtos;
using Application.Payment.Responses;

namespace Application.Booking.Ports
{
    public interface IBookingManager
    {
        Task<BookingResponse> CreateBooking(BookingDto booking);
        Task<PaymentResponse> PayForABooking(PaymentRequestDto paymentRequestDto);
        Task<BookingDto> GetBooking(int id);
    }
}
