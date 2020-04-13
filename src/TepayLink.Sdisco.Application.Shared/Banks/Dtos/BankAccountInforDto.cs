using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using TepayLink.Sdisco.AdminConfig;

namespace TepayLink.Sdisco.Banks.Dtos
{
    public class BankAccountInforDto : Entity<long>
    {

        public int BranchId { get; set; }
        public int BankId { get; set; }
        public string BankName { get; set; }
        public string BranchName { get; set; }
        public string AccountNo { get; set; }
        public string AccountName { get; set; }
        public BankTypeEnum Type { get; set; }
        public string Logo { get; set; }
        public string Image { get; set; }
        public string ExpiredDate { get; set; }


    }
}
