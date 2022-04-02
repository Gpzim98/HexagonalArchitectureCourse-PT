using Domain.Booking.Exceptions;
using Domain.Booking.Ports;
using Domain.Enums;
using Action = Domain.Enums.Action;

namespace Domain.Entities
{
    public class Booking
    {
        public Booking()
        {
            this.Status = Status.Created;
            this.PlacedAt = DateTime.UtcNow;
        }
        public int Id { get; set; }
        public DateTime PlacedAt { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public Room Room { get; set; }
        public Guest Guest { get; set; }
        public Status Status { get; set; }

        public void ChangeState(Action action)
        {
            this.Status = (this.Status, action) switch
            {
                (Status.Created,  Action.Pay)     => Status.Paid,
                (Status.Created,  Action.Cancel)  => Status.Canceled,
                (Status.Paid,     Action.Finish)  => Status.Finished,
                (Status.Paid,     Action.Refound) => Status.Refounded,
                (Status.Canceled, Action.Reopen)  => Status.Created,
                _ => this.Status
            };
        }

        public bool IsValid()
        {
            try
            {
                this.ValidateState();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private void ValidateState()
        {
            if (this.PlacedAt == default(DateTime))
            {
                throw new PlacedAtIsARequiredInformationException();
            }

            if (this.Start == default(DateTime))
            {
                throw new StartDateTimeIsRequiredException();
            }

            if (this.End == default(DateTime))
            {
                throw new EndDateTimeIsRequiredException();
            }

            if (this.Room == null)
            {
                throw new RoomIsRequiredException();
            }

            if (this.Guest == null)
            {
                throw new GuestIsRequiredException();
            }
        }

        public async Task Save(IBookingRepository bookingRepository)
        {
            this.ValidateState();

            this.Guest.IsValid();

            if (!this.Room.CanBeBooked())
            {
                throw new RoomCannotBeBookedException();
            }

            if (this.Id == 0)
            {
                var resp = await bookingRepository.CreateBooking(this);
                this.Id = resp.Id;
            }
            else
            { 
            
            }
        }
    }
}
