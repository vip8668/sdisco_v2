using System;
using System.Collections.Generic;
using System.Text;

namespace TepayLink.Sdisco.Common.Dto
{
    public class CurrencyOutputDto
    {
        public int Id { get; set; }
        /// <summary>
        /// Tên
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Tên hiển thị
        /// </summary>
        public string DisplayName { get; set; }
        /// <summary>
        /// Ký hiệu
        /// </summary>
        public string CurrencySign { get; set; }


    }
}
