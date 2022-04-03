using Application.Payment.Dtos;
using Application.Responses;

namespace Application.Payment.Responses
{
    public class PaymentResponse : Response
    {
        public PaymentStateDto Data { get; set; }
    }
}
