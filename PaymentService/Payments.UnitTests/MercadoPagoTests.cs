using Application.Booking.Dtos;
using Application.Payment;
using Application.Responses;
using NUnit.Framework;
using Payment.Application;
using System.Threading.Tasks;

namespace PaymentService.UnitTests
{
    public class MercadoPagoTests
    {
        [Test]
        public void ShouldReturn_MercadoPagoAdapter_Provider()
        {
            var factory = new PaymentProcessorFactory();

            var provider = factory.GetPaymentProcessor(SupportedPaymentProviders.MercadoPago);

            Assert.AreEqual(provider.GetType(), typeof(MercadoPagoAdapter));
        }

        [Test]
        public async Task Should_FailWhenPaymentIntentionStringIsInvalid()
        {
            var factory = new PaymentProcessorFactory();

            var provider = factory.GetPaymentProcessor(SupportedPaymentProviders.MercadoPago);

            var res = await provider.CapturePayment("");

            Assert.False(res.Success);
            Assert.AreEqual(res.ErrorCode, ErrorCodes.PAYMENT_INVALID_PAYMENT_INTENTION);
            Assert.AreEqual(res.Message, "The selected payment intention is invalid");
        }

        [Test]
        public async Task Should_SuccessfullyProcessPayment()
        {
            var factory = new PaymentProcessorFactory();

            var provider = factory.GetPaymentProcessor(SupportedPaymentProviders.MercadoPago);

            var res = await provider.CapturePayment("https://mercadopago.com.br/asdf");

            Assert.IsTrue(res.Success);
            Assert.AreEqual(res.Message, "Payment successfully processed");
            Assert.NotNull(res.Data);
            Assert.NotNull(res.Data.CreatedDate);
            Assert.NotNull(res.Data.PaymentId);
        }
    }
}