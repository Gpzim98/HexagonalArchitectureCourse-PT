using Application.Responses;
using Application.Room;
using Application.Room.Request;
using Domain.Entities;
using Domain.Room.Ports;
using Moq;
using NUnit.Framework;
using System.Threading.Tasks;
using System.Collections.Generic;
using Domain.Users;

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

            var userRepoMock = new Mock<IUserRepository>();

            var fakeUser = new User();
            userRepoMock.Setup(x => x.Get(It.IsAny<string>())).Returns(Task.FromResult(fakeUser));

            var manager = new RoomManager(repoMock.Object, userRepoMock.Object);
            var res = await manager.GetRoom(roomId);

            Assert.True(res.Success);
            Assert.NotNull(res.Data);
        }

        [Test]
        public async Task Should_Not_Create_Room_If_User_IsNot_Manager()
        {
            var repoMock = new Mock<IRoomRepository>();

            var request = new CreateRoomRequest()
            {
                UserName = "usernamehere"
            };

            var userRepoMock = new Mock<IUserRepository>();

            var fakeUser = new User()
            {
                Id= 1,
                Name= request.UserName,
                Roles = new List<string>() { "notamanager" }
            };

            userRepoMock.Setup(x => x.Get(request.UserName)).Returns(Task.FromResult(fakeUser));

            var manager = new RoomManager(repoMock.Object, userRepoMock.Object);
            var res = await manager.CreateRoom(request);

            Assert.False(res.Success);
            Assert.AreEqual(res.ErrorCode, ErrorCodes.ROOM_INVALID_PERMISSION);
            Assert.AreEqual(res.Message, "User does not have permission to perform this action");
        }

        [Test]
        public async Task Should_Create_Room_When_User_Is_Manager()
        {
            var repoMock = new Mock<IRoomRepository>();

            var request = new CreateRoomRequest()
            {
                UserName = "usernamehere",
                Data = new Application.Room.Dtos.RoomDto
                {
                    Id = 1,
                    InMaintenance = true,
                    Level = 1,
                    Name = "Room test",
                    Price = 100,
                    Currency = Domain.Enums.AcceptedCurrencies.Dollar
                }
            };

            var userRepoMock = new Mock<IUserRepository>();

            var fakeUser = new User()
            {
                Id = 1,
                Name = request.UserName,
                Roles = new List<string>() { "manager", "admin", "finance" }
            };

            userRepoMock.Setup(x => x.Get(request.UserName)).Returns(Task.FromResult(fakeUser));

            var manager = new RoomManager(repoMock.Object, userRepoMock.Object);
            var res = await manager.CreateRoom(request);

            Assert.True(res.Success);
            Assert.NotNull(res.Data);
        }

        [Test]
        public async Task Should_Return_ProperResonse_WhenRoom_NotFound()
        {
            var roomId = 1;
            var repoMock = new Mock<IRoomRepository>();

            var fakeUser = new User();
            var userRepoMock = new Mock<IUserRepository>();
            userRepoMock.Setup(x => x.Get(It.IsAny<string>())).Returns(Task.FromResult(fakeUser));

            var manager = new RoomManager(repoMock.Object, userRepoMock.Object);
            var res = await manager.GetRoom(roomId);

            Assert.False(res.Success);
            Assert.AreEqual(res.ErrorCode, ErrorCodes.ROOM_NOT_FOUND);
            Assert.AreEqual(res.Message, "Room not found");
        }
    }
}
