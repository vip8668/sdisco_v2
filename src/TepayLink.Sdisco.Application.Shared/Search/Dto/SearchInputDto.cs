using System;
using System.Collections.Generic;

using TepayLink.Sdisco.Dto;

namespace TepayLink.Sdisco.Search.Dto
{
    public class DestinationSearchDto : PagedInputDto
    {
        public static string TYPE_ACTIVITY = "Activity";
        public static string TYPE_TRIPPLAN = "TripPlan";
        public static string TYPE_TOUR = "Tour";
        public static string TYPE_LOCAL_PARTNER = "LocalPartner";
        public static string TYPE_PLACE = "Place";
        public static string TYPE_TRAVEL_GUIDE = "TraveGuide";
        public static string TYPE_MOBILE_DATA = "MobileData";
        public static string TYPE_CurrencyExchange = "CurrencyExchange";
        public static string TYPE_LocalGuideBook = "LocalGuideBook";
        public static string TYPE_Showticket = "Showticket";
        public static string TYPE_BikeRentail = "BikeRentail";

        public static string TYPE_THING_TO_BUY = "Thingtobuy";

        //Activity,Tour,TripPlan,LocalPartner,Place,Travel Guide,MobileData,CurrencyExchange,LocalGuideBook,Showticket,BikeRentail
        public string Type { get; set; }
        public string KeyWord { get; set; }
        public List<int> TourCategories { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public int TripLength { get; set; }
        public long FromPrice { get; set; }
        public long ToPrice { get; set; }

        public int Guest { get; set; }
    }

    public class GuestInfo
    {
        public int Adult { get; set; }
        public int Children { get; set; }
        public int Infants { get; set; }
    }

    public class SearchDto : PagedInputDto
    {
        public static string TYPE_ACTIVITY = "Activity";
        public static string TYPE_TRIPPLAN = "TripPlan";
        public static string TYPE_TOUR = "Tour";
        public static string TYPE_LOCAL_PARTNER = "LocalPartner";
        public static string TYPE_PLACE = "Place";

        public static string TYPE_TRAVEL_GUIDE = "Thingtobuy";

        //Activity,Tour,TripPlan,LocalPartner,Place,Travel Guide,MobileData,CurrencyExchange,LocalGuideBook,Showticket,BikeRentail
        public string Type { get; set; }
        public string KeyWord { get; set; }
        public List<int> TourCategories { get; set; }
        public DateTime Date { get; set; }
        public int TripLength { get; set; }
        public long Price { get; set; }
    }
}