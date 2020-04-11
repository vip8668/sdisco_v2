using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace TepayLink.Sdisco.Tour.Dtos
{
    public class FeeConfigDto : Entity<long>
    {

        public long HostUserId { get; set; }
        public double FeePercent { get; set; }
        public decimal ServiceFee(decimal price) => (decimal)Math.Round(((double)price * FeePercent / 100), 0);



    }
}
