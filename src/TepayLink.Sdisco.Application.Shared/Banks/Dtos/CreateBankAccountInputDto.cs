using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TepayLink.Sdisco.Banks.Dtos
{
    public class CreateBankAccountInputDto
    {
        /// <summary>
        /// Id 
        /// Nếu tạo mới không cần truyền
        /// 
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Bank Id
        /// </summary>
        [Required]
        public int BankId { get; set; }
        /// <summary>
        /// Id branch
        /// </summary>
        [Required]
        public int BranchId { get; set; }
        /// <summary>
        /// SỐ tài khoản
        /// </summary>
        [Required]
        public string AccountNo { get; set; }
        /// <summary>
        /// Tên tài khoản
        /// </summary>
        [Required]
        public string AccountName { get; set; }
    }
}
