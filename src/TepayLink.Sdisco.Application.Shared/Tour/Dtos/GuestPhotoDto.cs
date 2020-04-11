using System.Collections.Generic;

namespace TepayLink.Sdisco.Tour.Dtos
{
    public class GuestPhotoDto
    {
        public int TotalItem { get; set; }
        public List<PhotoTag> Tags { get; set; }
        public  List<PhotoDto> Photos { get; set; }
    }

    public class PhotoTag
    {
        public  string Tag { get; set; }
        public  int PhotoCount { get; set; }
       
    }
}