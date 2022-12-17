using Application.Booking.Commands;
using Application.Ports;
using Domain.Booking.Ports;
using Domain.Entities;
using Domain.Ports;
using Domain.Room.Ports;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ApplicationTests
{
    public class CreateBookingCommandHandlerTests
    {
        private CreateBookingCommandHandler GetCommandMock(
            Mock<IRoomRepository>    roomRepository    = null,
            Mock<IGuestRepository>   guestRepository   = null,
            Mock<IBookingRepository> bookingRepository = null
        )
        {
            var _bookingRepository = bookingRepository ?? new Mock<IBookingRepository>();
            var _guestRepository   = guestRepository   ?? new Mock<IGuestRepository>();
            var _roomRepository    = roomRepository    ?? new Mock<IRoomRepository>();

            return new CreateBookingCommandHandler(
                _bookingRepository.Object,
                _roomRepository.Object,
                _guestRepository.Object
            );
        }
        [Test]
        public async Task Should_Not_CreateBooking_If_Room_Is_Missing()
        {
            var command = new CreateBookingCommand
            {
                BookingDto = new Application.Booking.Dtos.BookingDto
                {
                    // RoomId= 1,
                    GuestId = 1,
                    Start = DateTime.Now,
                    End = DateTime.Now.AddDays(2),
                }
            };

            var fakeGuest = new Guest
            {
                Id = command.BookingDto.GuestId,
                DocumentId = new Domain.ValueObjects.PersonId
                {
                    DocumentType = Domain.Enums.DocumentType.Passport,
                    IdNumber = "abc1234"
                },
                Email = "a@a.com",
                Name = "Fake Guest",
                Surname = "Fake Guest Surname"
            };

            var guestRepository = new Mock<IGuestRepository>();
            guestRepository.Setup(x => x.Get(command.BookingDto.GuestId))
                .Returns(Task.FromResult(fakeGuest));

            var fakeRoom = new Room
            {
                Id = command.BookingDto.RoomId,
                InMaintenance = false,
                Price = new Domain.ValueObjects.Price
                {
                    Currency = Domain.Enums.AcceptedCurrencies.Dollar,
                    Value = 100
                },
                Name = "Fake Room 01",
                Level = 1,
            };

            var fakeBooking = new Booking
            {
                Id = 1
            };

            var bookingRepository = new Mock<IBookingRepository>();
            bookingRepository.Setup(x => x.CreateBooking(It.IsAny<Booking>()))
                .Returns(Task.FromResult(fakeBooking));

            var handler = GetCommandMock(null, guestRepository, bookingRepository);
            var resp = await handler.Handle(command, CancellationToken.None);

            Assert.NotNull(resp);
            Assert.False(resp.Success);
            Assert.AreEqual(resp.ErrorCode, Application.Responses.ErrorCodes.BOOKING_MISSING_REQUIRED_INFORMATION);
            Assert.AreEqual(resp.Message, "Room is a required information");    
        }

        [Test]
        public async Task Should_Not_CreateBooking_If_StartDateIsMissing()
        {
            var command = new CreateBookingCommand
            {
                BookingDto = new Application.Booking.Dtos.BookingDto
                {
                    RoomId= 1,
                    GuestId = 1,
                    //Start = DateTime.Now,
                    End = DateTime.Now.AddDays(2),
                }
            };

            var fakeGuest = new Guest
            {
                Id = command.BookingDto.GuestId,
                DocumentId = new Domain.ValueObjects.PersonId
                {
                    DocumentType = Domain.Enums.DocumentType.Passport,
                    IdNumber = "abc1234"
                },
                Email = "a@a.com",
                Name = "Fake Guest",
                Surname = "Fake Guest Surname"
            };

            var guestRepository = new Mock<IGuestRepository>();
            guestRepository.Setup(x => x.Get(command.BookingDto.GuestId))
                .Returns(Task.FromResult(fakeGuest));

            var fakeRoom = new Room
            {
                Id = command.BookingDto.RoomId,
                InMaintenance = false,
                Price = new Domain.ValueObjects.Price
                {
                    Currency = Domain.Enums.AcceptedCurrencies.Dollar,
                    Value = 100
                },
                Name = "Fake Room 01",
                Level = 1,
            };

            var fakeBooking = new Booking
            {
                Id = 1
            };

            var bookingRepository = new Mock<IBookingRepository>();
            bookingRepository.Setup(x => x.CreateBooking(It.IsAny<Booking>()))
                .Returns(Task.FromResult(fakeBooking));

            var handler = GetCommandMock(null, guestRepository, bookingRepository);
            var resp = await handler.Handle(command, CancellationToken.None);

            Assert.NotNull(resp);
            Assert.False(resp.Success);
            Assert.AreEqual(resp.ErrorCode, Application.Responses.ErrorCodes.BOOKING_MISSING_REQUIRED_INFORMATION);
            Assert.AreEqual(resp.Message, "Start is a required information");
        }

        [Test]
        public async Task Should_CreateBooking()
        {
            var command = new CreateBookingCommand
            {
                BookingDto = new Application.Booking.Dtos.BookingDto
                {
                    RoomId = 1,
                    GuestId = 1,
                    Start = DateTime.Now,
                    End = DateTime.Now.AddDays(2),
                }
            };

            var fakeGuest = new Guest
            {
                Id = command.BookingDto.GuestId,
                DocumentId = new Domain.ValueObjects.PersonId
                {
                    DocumentType = Domain.Enums.DocumentType.Passport,
                    IdNumber = "abc1234"
                },
                Email = "a@a.com",
                Name = "Fake Guest",
                Surname = "Fake Guest Surname"
            };

            var guestRepository = new Mock<IGuestRepository>();
            guestRepository.Setup(x => x.Get(command.BookingDto.GuestId))
                .Returns(Task.FromResult(fakeGuest));

            var fakeRoom = new Room
            {
                Id = command.BookingDto.RoomId,
                InMaintenance = false,
                Price = new Domain.ValueObjects.Price
                {
                    Currency = Domain.Enums.AcceptedCurrencies.Dollar,
                    Value = 100
                },
                Name = "Fake Room 01",
                Level = 1,
            };

            var roomRepository = new Mock<IRoomRepository>();
            roomRepository.Setup(x => x.GetAggregate(command.BookingDto.RoomId))
                .Returns(Task.FromResult(fakeRoom));

            var fakeBooking = new Booking
            {
                Id = 1,
                Room = fakeRoom,
                Guest= fakeGuest,
         
            };  

            var bookingRepoMock = new Mock<IBookingRepository>();
            bookingRepoMock.Setup(x => x.CreateBooking(It.IsAny<Booking>()))
                .Returns(Task.FromResult(fakeBooking));
            //bookingRepository.Setup(x => x.Save)

            var handler = GetCommandMock(roomRepository, guestRepository, bookingRepoMock);
            var resp = await handler.Handle(command, CancellationToken.None);

            Assert.NotNull(resp);
            Assert.True(resp.Success);
            Assert.NotNull(resp.Data);
            Assert.AreEqual(resp.Data.Id, command.BookingDto.Id);
        }
    }
}
