using Application.Responses;
using Application.Room;
using Domain.Entities;
using Domain.Room.Ports;
using Moq;
using NUnit.Framework;
using System.Threading.Tasks;

namespace ApplicationTests
{
    public class RoomManagerTests
    {
        [Test]
        public async Task Should_Return_Room()
        {
            var roomId = 1;
            var repoMock = new Mock<IRoomRepository>();
            var fakeRoom = new Room()
            {
                Id = roomId,
                Price = new Domain.ValueObjects.Price 
                { 
                    Currency = Domain.Enums.AcceptedCurrencies.Bitcoin, 
                    Value = 123
                }
            };

            repoMock.Setup(x => x.Get(roomId)).Returns(Task.FromResult(fakeRoom));

            var manager = new RoomManager(repoMock.Object);
            var res = await manager.GetRoom(roomId);

            Assert.True(res.Success);
            Assert.NotNull(res.Data);
        }

        [Test]
        public async Task Should_Return_ProperResonse_WhenRoom_NotFound()
        {
            var roomId = 1;
            var repoMock = new Mock<IRoomRepository>();

            var manager = new RoomManager(repoMock.Object);
            var res = await manager.GetRoom(roomId);

            Assert.False(res.Success);
            Assert.AreEqual(res.ErrorCode, ErrorCodes.ROOM_NOT_FOUND);
            Assert.AreEqual(res.Message, "Room not found");
        }
    }
}
