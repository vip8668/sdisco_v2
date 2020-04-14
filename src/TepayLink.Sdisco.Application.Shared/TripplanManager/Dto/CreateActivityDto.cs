using System;
using System.Collections.Generic;


namespace TepayLink.Sdisco.TripPlanManager.Dto
{
    public class CreateActivityDto:CreateTourItemBasicDto
    {
        public long ActivityId { get; set; }

       // public TourItemTypeEnum Type { get; set; }
    }
}