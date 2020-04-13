using System;
using System.Collections.Generic;
using System.Text;

namespace TepayLink.Sdisco.Authorization.Users.Profile.Dto
{
    public class GetRankingAndPointDto
    {

        public string Ranking { get; set; }
        public float Rating { get; set; }
        public float Point { get; set; }
    }

    public class GetNotifycationSettingDto
    {
        /// <summary>
        /// Key Setting
        /// notification.sms : sms
        /// notification.app : app
        /// notification.email : email
        /// </summary>
        public string SettingKey { get; set; }
    }
    public class UpdateNotificationSettingInput
    {
        // <summary>
        /// Key Setting
        /// notification.sms : sms
        /// notification.app : app
        /// notification.email : email
        /// </summary>
        public string SettingKey { get; set; }
        public bool Value { get; set; }
    }
}
