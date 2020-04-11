using System.Collections.Generic;

namespace TepayLink.Sdisco.Tour.Dtos
{
    public class TourItemDetailDto : BasicTourItemDto
    {
        public bool InstantBook { get; set; }
        public List<PhotoDto> Images { get; set; }
        public string Overview { get; set; }
        public  string Language { get; set; }
        public BasicHostUserInfo HostUser { get; set; }
       
        public  string Policies { get; set; }
        
        
    }
}