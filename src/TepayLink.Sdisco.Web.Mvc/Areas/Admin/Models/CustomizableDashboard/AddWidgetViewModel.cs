﻿using System.Collections.Generic;
using TepayLink.Sdisco.DashboardCustomization.Dto;

namespace TepayLink.Sdisco.Web.Areas.Admin.Models.CustomizableDashboard
{
    public class AddWidgetViewModel
    {
        public List<WidgetOutput> Widgets { get; set; }

        public string DashboardName { get; set; }

        public string PageId { get; set; }
    }
}
