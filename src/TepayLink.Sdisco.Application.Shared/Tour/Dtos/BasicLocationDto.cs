using System;
using System.Collections.Generic;
using System.Text;

namespace TepayLink.Sdisco.Tour.Dtos
{
   public class BasicLocationDto
    {
        /// <summary>
        /// Id
        /// </summary>
        public  long Id { get; set; }
        /// <summary>
        /// Tên
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Địa chỉ
        /// </summary>
        public string Addess { get; set; }
        /// <summary>
        /// Lat
        /// </summary>
        public float Lat { get; set; }
        /// <summary>
        /// Long
        /// </summary>
        public float Long { get; set; }
        /// <summary>
        /// Khoảng các
        /// </summary>
        public float Distance { get; set; }
        /// <summary>
        /// Ghi chú
        /// </summary>
        public string Description { get; set; }
    }
}
