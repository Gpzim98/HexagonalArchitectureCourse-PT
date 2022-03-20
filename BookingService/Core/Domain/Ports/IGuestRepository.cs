using Domain.Entities;

namespace Domain.Ports
{
    public interface IGuestRepository
    {
        Task<Guest> Get(int Id);
        Task<int> Save(Guest guest);
    }
}
