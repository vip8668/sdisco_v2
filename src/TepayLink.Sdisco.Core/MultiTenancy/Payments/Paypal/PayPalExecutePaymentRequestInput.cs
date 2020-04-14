namespace TepayLink.Sdisco.MultiTenancy.Payments.Paypal
{
    public class PayPalCaptureOrderRequestInput
    {
        public string OrderId { get; set; }

        public PayPalCaptureOrderRequestInput(string orderId)
        {
            OrderId = orderId;
        }
    }

    public class PayPalExecutePaymentRequestInput
    {
        public string PaymentId { get; set; }

        public string PayerId { get; set; }

        public PayPalExecutePaymentRequestInput(string paymentId, string payerId)
        {
            PaymentId = paymentId;
            PayerId = payerId;
        }
    }
}