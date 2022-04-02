using Domain.Room.Ports;
using Microsoft.EntityFrameworkCore;

namespace Data.Room
{
    public class RoomRepository : IRoomRepository
    {
        private readonly HotelDbContext _hotelDbContext;
        public RoomRepository(HotelDbContext hotelDbContext)
        { 
            _hotelDbContext = hotelDbContext;
        }
        public async Task<int> Create(Domain.Entities.Room room)
        {
            _hotelDbContext.Rooms.Add(room);
            await _hotelDbContext.SaveChangesAsync();
            return room.Id;
        }

        public Task<Domain.Entities.Room> Get(int Id)
        {
            return _hotelDbContext.Rooms
                .Where(g => g.Id == Id).FirstAsync();
        }

        public Task<Domain.Entities.Room> GetAggregate(int Id)
        {
            return _hotelDbContext.Rooms
                .Include(r => r.Bookings)
                .Where(g => g.Id == Id).FirstAsync();
        }
    }
}
