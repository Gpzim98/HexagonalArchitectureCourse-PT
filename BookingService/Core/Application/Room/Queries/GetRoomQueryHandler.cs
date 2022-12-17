using Application.Room.Dtos;
using Application.Room.Responses;
using Domain.Room.Ports;
using MediatR;

namespace Application.Room.Queries
{
    public class GetRoomQueryHandler : IRequestHandler<GetRoomQuery, RoomResponse>
    {
        private readonly IRoomRepository _roomRepository;
        public GetRoomQueryHandler(IRoomRepository roomRepository) 
        {
            _roomRepository= roomRepository;
        }
        public async Task<RoomResponse> Handle(GetRoomQuery request, CancellationToken cancellationToken)
        {
            var room = await _roomRepository.Get(request.Id);

            if (room == null)
            {
                return new RoomResponse
                {
                    Success = false,
                    ErrorCode = Application.Responses.ErrorCodes.ROOM_NOT_FOUND,
                    Message = "Could not find a Room with the given Id"
                };
            }

            return new RoomResponse
            {
                Data = RoomDto.MapToDto(room),
                Success = true
            };
        }
    }
}
