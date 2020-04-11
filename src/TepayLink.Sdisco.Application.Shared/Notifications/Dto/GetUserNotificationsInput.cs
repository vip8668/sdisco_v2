using System;
using Abp.Notifications;
using TepayLink.Sdisco.Dto;

namespace TepayLink.Sdisco.Notifications.Dto
{
    public class GetUserNotificationsInput : PagedInputDto
    {
        public UserNotificationState? State { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }
    }
}