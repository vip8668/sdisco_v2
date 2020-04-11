using System;
using System.Collections.Generic;
using System.Text;

namespace TepayLink.Sdisco.Tour.Dtos
{
  public  class PhotoDto
    {
        /// <summary>
        /// Url
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// Title
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// Tag
        /// </summary>
        public  string Tag { get; set; }
        /// <summary>
        /// Order
        /// </summary>
        public int Order { get; set; }
    }
}
