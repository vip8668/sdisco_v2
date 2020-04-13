using System;
using System.Collections.Generic;
using System.Text;

namespace TepayLink.Sdisco.Common.Dto
{
    public class CountryOutputDto
    {
        /// <summary>
        /// Id
        /// </summary>
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
        /// Icon
        /// </summary>

        public string Icon { get; set; }

        public bool IsDisabled { get; set; }
    }
}
