
using TepayLink.Sdisco.Products;
using TepayLink.Sdisco.Utils;

namespace TepayLink.Sdisco.Tour.Dtos
{
    public class BasicItemDto
    {
        /// <summary>
        /// Id
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// Tên 
        /// </summary>
        public  string Name { get; set; }
        public string Slug =>  this.Name.GenerateSlug(); 
        public string ThumbImage { get; set; }
        public  string Icon { get; set; }
       
        public  ProductTypeEnum ItemType { get; set; }
    }
}