﻿using System.Collections.Generic;

namespace TepayLink.Sdisco.Web.Common
{
    public static class WebConsts
    {
        public const string SwaggerUiEndPoint = "/swagger";
        public const string HangfireDashboardEndPoint = "/hangfire";

        public static bool SwaggerUiEnabled = true;
        public static bool HangfireDashboardEnabled = false;

        public static List<string> ReCaptchaIgnoreWhiteList = new List<string>
        {
            SdiscoConsts.AbpApiClientUserAgent
        };

        public static class GraphQL
        {
            public const string PlaygroundEndPoint = "/ui/playground";
            public const string EndPoint = "/graphql";

            public static bool PlaygroundEnabled = false;
            public static bool Enabled = false;
        }
    }
}
