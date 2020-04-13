using System;

namespace SDisco.Affiliate.Dto
{
    public class CommissionChartOutputDto
    {
        /// <summary>
        /// Tháng
        /// </summary>
        public  int Month { get; set; }
        /// <summary>
        /// Nam
        /// </summary>
        public  int Year { get; set; }
        /// <summary>
        /// Giá trị
        /// </summary>
        public  decimal Value { get; set; }
        
    }
}