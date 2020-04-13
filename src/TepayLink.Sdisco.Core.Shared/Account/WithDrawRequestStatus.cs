using System;
using System.Collections.Generic;
using System.Text;

namespace TepayLink.Sdisco.Account
{
    public enum WithDrawRequestStatus : byte
    {

        Init = 0,
        Confirm = 1,
        Reject = 2,
        Cancel = 3,
    }

    public enum TransactionType : byte
    {
        Booking = 1,
        WithDraw = 2,

        Affiliate = 3,
        CancelWithDraw = 4,

    }
}
