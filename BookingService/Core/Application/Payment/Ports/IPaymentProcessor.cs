using Application.Payment.Responses;

namespace Application.Payment
{
    public interface IPaymentProcessor
    {
        Task<PaymentResponse> CapturePayment(string paymentIntention);
    }
}
