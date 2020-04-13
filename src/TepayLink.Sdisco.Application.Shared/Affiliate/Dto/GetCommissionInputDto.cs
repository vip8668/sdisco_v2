using System;

namespace SDisco.Affiliate.Dto
{
    public class GetCommissionInputDto
    {
        /// <summary>
        /// Thời gian từ
        /// </summary>
        public DateTime FromDate { get; set; }
        /// <summary>
        /// Thời gian tới
        /// </summary>
        public  DateTime ToDate { get; set; }
    }
}