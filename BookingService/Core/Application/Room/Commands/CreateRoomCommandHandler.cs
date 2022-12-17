using Application.Responses;
using Application.Room.Dtos;
using Application.Room.Responses;
using Domain.Room.Exceptions;
using Domain.Room.Ports;
using MediatR;


namespace Application.Room.Commands
{
    public class CreateRoomCommandHandler : IRequestHandler<CreateRoomCommand, RoomResponse>
    {
        public readonly IRoomRepository _roomRepository;
        public CreateRoomCommandHandler(IRoomRepository roomRepository) 
        {
            _roomRepository= roomRepository;
        }
        public async Task<RoomResponse> Handle(CreateRoomCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var room = RoomDto.MapToEntity(request.RoomDto);

                await room.Save(_roomRepository);
                request.RoomDto.Id = room.Id;

                return new RoomResponse
                {
                    Success = true,
                    Data = request.RoomDto,
                };
            }
            catch(InvalidRoomPriceException)
            {
                return new RoomResponse
                {
                    Success = false,
                    ErrorCode = ErrorCodes.ROOM_MISSING_REQUIRED_INFORMATION,
                    Message = "Room price is invalid"
                };
            }
            catch (InvalidRoomDataException)
            {
                return new RoomResponse
                {
                    Success = false,
                    ErrorCode = ErrorCodes.ROOM_MISSING_REQUIRED_INFORMATION,
                    Message = "Missing required information passed"
                };
            }
            catch (Exception)
            {
                return new RoomResponse
                {
                    Success = false,
                    ErrorCode = ErrorCodes.ROOM_COULD_NOT_STORE_DATA,
                    Message = "There was an error when saving to DB"
                };
            }
        }
    }
}
