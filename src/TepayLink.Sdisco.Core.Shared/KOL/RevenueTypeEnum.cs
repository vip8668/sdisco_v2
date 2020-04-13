using System;
using System.Collections.Generic;
using System.Text;

namespace TepayLink.Sdisco.KOL
{
    public enum RevenueTypeEnum : byte
    {
        /// <summary>
        /// Share
        /// </summary>
        Shared = 1,
        /// <summary>
        /// coppy
        /// </summary>
        Coppy = 2,
        /// <summary>
        /// booking
        /// </summary>
        Booking = 3,
        /// <summary>
        /// Click
        /// </summary>
        Click = 4

    }
    public enum RevenueStatusEnum : byte
    {
        Init = 0,
        Approve = 2,
        Payout = 3,

    }
}
