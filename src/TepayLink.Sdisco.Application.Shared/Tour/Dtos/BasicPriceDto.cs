namespace TepayLink.Sdisco.Tour.Dtos
{
    public class BasicPriceDto
    {
        public  long ItemId { get; set; }
        //  public decimal NumberPeople { get; set; }
        /// <summary>
        /// Giá cũ
        /// </summary>
        public decimal OldPrice { get; set; }

        /// <summary>
        /// Giá hiện tại
        /// </summary>
        public decimal Price { get; set; }
        
        /// <summary>
        /// Giá nhập
        /// </summary>
        public decimal CostPrice { get; set; }

        /// <summary>
        /// Giá vé ( áp dụng cho Tour/Trip detail)
        /// </summary>
        public decimal Ticket { get; set; }

        /// <summary>
        /// Giá phòng ks ( áp dụng cho Tour/Trip detail)
        /// </summary>
        public decimal Hotel { get; set; }

        public decimal ServiceFee { get; set; }
        public decimal Total { get; set; }
    }
}