using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using Abp.Domain.Entities;

using TepayLink.Sdisco.Tour.Dtos;
using TepayLink.Sdisco.Utils;

namespace SDisco.Home.Dto
{
    public class BasicTourDto : Entity<long>
    {
        /// <summary>
        /// Tiêu đề
        /// </summary>
        public string Title { get; set; }
        
        public string Slug
        {
            get { return this.Title.GenerateSlug(); }
        }

        /// <summary>
        /// Id Ngôn ngữ
        /// </summary>
        public int OfferLanguageId { get; set; }

        /// <summary>
        /// Ngôn ngữ
        /// </summary>
        public string LanguageOffer { get; set; }

        /// <summary>
        /// Danh mục
        /// </summary>
        public int CategoryId { get; set; }

        /// <summary>
        /// Tên danh mục
        /// </summary>
        public string CategoryName { get; set; }

        /// <summary>
        /// Id Địa điểm
        /// </summary>
        public long PlaceId { get; set; }

        /// <summary>
        /// Tên địa điểm
        /// </summary>
        public string PlaceName { get; set; }

        //public decimal StartPrice { get; set; }
        /// <summary>
        /// Giá
        /// </summary>
        public BasicPriceDto Price
        {
            get
            {
                if (AvaiableTimes == null)
                    return null;
                var avaiable = AvaiableTimes.OrderBy(p => p.Price.Price).FirstOrDefault();
                return avaiable != null ? avaiable.Price : new BasicPriceDto
                {
                    Price=0
                };
            }
        }

        /// <summary>
        /// Hotdeal
        /// </summary>
        public bool IsHotDeal { get; set; }

        /// <summary>
        /// Tổng đã bán
        /// </summary>
        public int SoldCount { get; set; }

        /// <summary>
        /// Số ngày của chuyến đi
        /// </summary>
        public int TripLength { get; set; }


        public int Order { get; set; }

        //todo nho bo cho nay ra 
        public List<PhotoDto> ThumbImages
        {
            get;
            set;
//            get
//            {
//                return new List<PhotoDto>
//                {
//                    new PhotoDto
//                    {
//                        Url =
//                            "https://images.unsplash.com/photo-1531804055935-76f44d7c3621?ixlib=rb-1.2.1&ixid=eyJhcHBfaWQiOjExMjU4fQ&w=1000&q=80"
//                    },
//
//                    new PhotoDto
//                    {
//                        Url =
//                            "https://cdn57.androidauthority.net/wp-content/uploads/2018/10/android-authority-xiaomi-photo-competition-11.jpg"
//                    },
//                };
//            }
//            set { }
        }

        /// <summary>
        /// Trending
        /// </summary>
        public bool IsTrending { get; set; }

        /// <summary>
        /// Số lượt thích
        /// </summary>
        public int CountLike { get; set; }

        /// <summary>
        /// Bán tốt nhất
        /// </summary>

        public bool BestSaller { get; set; }

        public List<AvaiableTimeDto> AvaiableTimes { get; set; }

        /// <summary>
        /// Avaiable date
        /// </summary>
        //  public DateTime AvaiableTime { get; set; }
        /// <summary>
        /// Reivew
        /// </summary>
        public ReviewSummaryDto Review { get; set; }

        /// <summary>
        /// Yêu thích
        /// </summary>
        public bool IsLove { get; set; }

        /// <summary>
        /// SỐ lượt coppy
        /// </summary>

        public int CoppyCount { get; set; }

        /// <summary>
        /// Số lượt Share
        /// </summary>
        public int ShareCount { get; set; }

        public int ViewCount { get; set; }
    }
}