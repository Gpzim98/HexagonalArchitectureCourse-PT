using Application.Room.Responses;
using MediatR;

namespace Application.Room.Queries
{
    public class GetRoomQuery : IRequest<RoomResponse>
    {
        public int Id { get; set; }
    }
}
