using System;
using System.Collections.Generic;
using TepayLink.Sdisco.Dto;
using TepayLink.Sdisco.Products;

namespace TepayLink.Sdisco.Booking.Dtos
{
    public class MyBookingInputDto : PagedInputDto
    {
        /// <summary>
        /// Ngày
        /// </summary>
        public int Day { get; set; }
        /// <summary>
        /// Tháng
        /// </summary>
        public int Month { get; set; }
        /// <summary>
        /// Năm
        /// </summary>
        public int Year { get; set; }
    }
    public class BasicDateDto
    {
        public ProductTypeEnum ItemType { get; set; }
        public int Day { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
    }

    public class MyBookingInput1Dto : PagedInputDto
    {
        /// <summary>
        /// Ngày
        /// </summary>
        public List<int> Days { get; set; }
        /// <summary>
        /// Tháng
        /// </summary>
        public int Month { get; set; }
        /// <summary>
        /// Năm
        /// </summary>
        public int Year { get; set; }
    }
}