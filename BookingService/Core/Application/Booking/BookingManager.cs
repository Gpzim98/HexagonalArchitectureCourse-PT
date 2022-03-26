using Application.Booking.Dtos;
using Application.Booking.Ports;
using Domain.Booking.Ports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Booking
{
    public class BookingManager : IBookingManager
    {
        private readonly IBookingRepository _bookingRepository;
        public BookingManager(IBookingRepository bookingRepository)
        { 
            _bookingRepository = bookingRepository;
        }
        public Task<BookingDto> CreateBooking(BookingDto bookingDto)
        {
            var booking = BookingDto.MapToEntity(bookingDto);

            booking.Save(_bookingRepository);

            _bookingRepository.CreateBooking(booking)
        }

        public Task<BookingDto> GetBooking(int id)
        {
            throw new NotImplementedException();
        }
    }
}
