using System;
using System.Collections.Generic;
using System.Text;

namespace TepayLink.Sdisco.Banks.Dtos
{
    public class BankBranchDto
    {
        public int Id { get; set; }
        /// <summary>
        /// Tên chi nhánh
        /// </summary>
        public string BranchName { get; set; }
        /// <summary>
        /// Order
        /// </summary>
        public int Order { get; set; }

    }
}
