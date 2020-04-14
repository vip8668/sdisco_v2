using System.Collections.Generic;

namespace TepayLink.Sdisco.Search.Dto
{
    public class SearchCategoryDto
    {
        public string Type { get; set; }
        
        public string Name { get; set; }
        public string Icon { get; set; }

        public static List<SearchCategoryDto> InitList()
        {
            return new List<SearchCategoryDto>
            {
                //todo chỗ này xem lại config lại đường dẫn
                new SearchCategoryDto
                {
                    Type = "Tour",
                    Icon = $"{AppConsts.Domain}/images/sdisco/tour.png",
                    Name = "Tour"
                },
                new SearchCategoryDto
                {
                    Type = "activity",
                    Icon =  $"{AppConsts.Domain}/images/sdisco/thing.png",
                    Name = "Activity"
                },
                new SearchCategoryDto
                {
                    Type = "Destinations",
                    Icon =  $"{AppConsts.Domain}/images/sdisco/destination.png",
                    Name = "Destinations"
                },
                new SearchCategoryDto
                {
                    Type = "tripplan",
                    Icon =  $"{AppConsts.Domain}/images/sdisco/plan.png",
                    Name = "Trip Plan"
                },
                new SearchCategoryDto
                {
                    Type = "thingtobuy",
                    Icon =  $"{AppConsts.Domain}/images/sdisco/thing.png",
                    Name = "Things to buy"
                }
            };
        }
    }
}