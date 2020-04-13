using System;
using Abp.Application.Services.Dto;

namespace SDisco.Affiliate.Dto
{
    public class PayoutOutputDto
    {
        public PayoutOutputDto()
        {
            
        }
        
        public DateTime Date { get; set; }
        public string TripTitle { get; set; }
        public decimal Point { get; set; }
        public decimal Money { get; set; }
    }

    public class PayoutDto
    {
        public decimal TotalMoney { get; set; }
        public PagedResultDto<PayoutOutputDto> PayoutList { get; set; }
    }

    public class PayoutHistoryDto
    {
        public PayoutHistoryDto()
        {
            
        }
        

        public decimal Amount { get; set; }
        public string Reason { get; set; }
        public DateTime Date { get; set; }
    }
}