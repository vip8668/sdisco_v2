using System;
using System.Collections.Generic;
using System.Text;

namespace TepayLink.Sdisco.TripPlanManager.Dto
{
    public class CreateHotelDto:CreateTourItemBasicDto
    {
        public long HotelId { get; set; }
        /// <summary>
        /// Ngày đang chọn
        /// </summary>
        /// <summary>
        /// Điah chỉ
        /// </summary>
        public string Address { get; set; }
        public int Star { get; set; }

        /// <summary>
        /// Danh sách phòng
        /// </summary>
        public List<CreateRoomDto> Rooms { get; set; }
       
    }

    public class CreateRoomDto
    {
        public int RoomId { get; set; }
        /// <summary>
        /// Tên phòng
        /// </summary>
        public string RoomName { get; set; }
        /// <summary>
        /// Giá phòng
        /// </summary>
        public decimal Price { get; set; }
        public decimal CostPrice { get; set; }
        public bool Selected { get; set; }
    }
}
