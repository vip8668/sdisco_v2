namespace SDisco.Affiliate.Dto
{
    public class GetCommissionOutputDto
    {
        /// <summary>
        /// Số điểm share
        /// </summary>
        public decimal SharePoint { get; set; }
        /// <summary>
        /// Số điểm copy
        /// </summary>
        public decimal CoppyTripPoint { get; set; }
        public decimal ClickPoint { get; set; }
        /// <summary>
        /// Tổng tiền share booking
        /// </summary>
        public decimal Booked { get; set; }
        /// <summary>
        /// Lastpayment
        /// </summary>
        public decimal LastPayment { get; set; }
        /// <summary>
        /// Total
        /// </summary>
        public  decimal Total { get; set; }
        
    }
}