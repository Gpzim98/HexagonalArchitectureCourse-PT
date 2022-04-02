using Domain.Exceptions;
using Domain.ValueObjects;
using Domain.UtilsTools;
using Domain.Ports;

namespace Domain.Entities
{
    public class Guest
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public PersonId DocumentId { get; set; }

        private void ValidateState()
        {
            if (DocumentId == null ||
                string.IsNullOrEmpty(DocumentId.IdNumber) ||
                DocumentId.IdNumber.Length <= 3 ||
                DocumentId.DocumentType == 0)
            {
                throw new InvalidPersonDocumentIdException();
            }

            if (string.IsNullOrEmpty(Name)     || 
                string.IsNullOrEmpty(Surname)  || 
                string.IsNullOrEmpty(Email))
            {
                throw new MissingRequiredInformation();
            }

            if (Utils.ValidateEmail(this.Email) == false)
            { 
                throw new InvalidEmailException();
            }
        }

        public bool IsValid()
        {
            this.ValidateState();
            return true;
        }

        public async Task Save(IGuestRepository guestRepository)
        {
            this.ValidateState();

            if (this.Id == 0)
            {
                this.Id = await guestRepository.Create(this);
            }
            else
            {
                //await guestRepository.Update(this);
            }
        }
    }
}
