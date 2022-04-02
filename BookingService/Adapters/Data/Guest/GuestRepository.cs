using Domain.Ports;
using Microsoft.EntityFrameworkCore;

namespace Data.Guest
{
    public class GuestRepository : IGuestRepository
    {
        private readonly HotelDbContext _hotelDbContext;
        public GuestRepository(HotelDbContext hotelDbContext)
        { 
            _hotelDbContext = hotelDbContext;
        }
        public async Task<int> Create(Domain.Entities.Guest guest)
        {
            _hotelDbContext.Guests.Add(guest);
            await _hotelDbContext.SaveChangesAsync();
            return guest.Id;
        }

        public Task<Domain.Entities.Guest> Get(int Id)
        {
            return _hotelDbContext.Guests.Where(g => g.Id == Id).FirstOrDefaultAsync();
        }
    }
}
