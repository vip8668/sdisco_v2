using System;
using System.Collections.Generic;

namespace TepayLink.Sdisco.TripPlanManager.Dto
{
    public class SearhTourItemInputDto
    {
        /// <summary>
        /// Ngày tìm kiếm
        /// </summary>
        public  DateTime Date { get; set; }
        /// <summary>
        /// Danh sách Id danh mục cần tìm kiếm
        /// </summary>
        public List<int> CategoryIds { get; set; }
        /// <summary>
        /// Từ khóa
        /// </summary>
        public string Keyword { get; set; }
    }
}