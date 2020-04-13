using System;
using SDisco.Tour;

namespace SDisco.Affiliate.Dto
{
    public class GetChartInputDto
    {
        /// <summary>
        /// Tháng từ
        /// </summary>
        public int FromMonth { get; set; }
        
        /// <summary>
        /// Năm từ
        /// </summary>
        public int FromYear { get; set; }
        
        
        /// <summary>
        /// Tháng tới
        /// </summary>
        public int ToMonth { get; set; }
        
        /// <summary>
        ///năm tới
        /// </summary>
        public int ToYear { get; set; }
        

        

        /// <summary>
        /// Loại
        /// Shared=1,
        ///Coppy=2,
        ///   Booking=3,
        /// Click=4
        /// </summary>
        public int Type { get; set; }
    }
}