using Application.Room.Dtos;
using Application.Room.Queries;
using Domain.Entities;
using Domain.Room.Ports;
using Moq;
using NUnit.Framework;
using System.Threading;
using System.Threading.Tasks;

namespace ApplicationTests
{
    public class GetRoomQueryHandlerTests
    {
        [Test]
        public async Task Should_Return_Room()
        {
            var query = new GetRoomQuery { Id = 1 };

            var repoMock = new Mock<IRoomRepository>();
            var fakeRoom = new Room() { Id = 1 };
            repoMock.Setup(x => x.Get(query.Id)).Returns(Task.FromResult(fakeRoom));

            var handler = new GetRoomQueryHandler(repoMock.Object);
            var res = await handler.Handle(query, CancellationToken.None);

            Assert.True(res.Success);
            Assert.NotNull(res.Data);
        }

        [Test]
        public async Task Should_Return_ProperError_Message_WhenRoom_NotFound()
        {
            var query = new GetRoomQuery { Id = 1 };

            var repoMock = new Mock<IRoomRepository>();

            var handler = new GetRoomQueryHandler(repoMock.Object);
            var res = await handler.Handle(query, CancellationToken.None);

            Assert.False(res.Success);
            Assert.AreEqual(res.ErrorCode, Application.Responses.ErrorCodes.ROOM_NOT_FOUND);
            Assert.AreEqual(res.Message, "Could not find a Room with the given Id");
        }
    }
}
