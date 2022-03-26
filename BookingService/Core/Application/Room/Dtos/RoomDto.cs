using Domain.Enums;
using Domain.Entities;

namespace Application.Room.Dtos
{
    public class RoomDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Level { get; set; }
        public bool InMaintenance { get; set; }
        public decimal Price { get; set; }
        public AcceptedCurrencies Currency { get; set; }

        public static Domain.Entities.Room MapToEntity(RoomDto dto)
        {
            return new Domain.Entities.Room
            {
                Id = dto.Id,
                Name = dto.Name,
                Level = dto.Level,
                InMaintenance = dto.InMaintenance,
                Price = new Domain.ValueObjects.Price { Currency = dto.Currency, Value = dto.Price }
            };
        }
    }
}
