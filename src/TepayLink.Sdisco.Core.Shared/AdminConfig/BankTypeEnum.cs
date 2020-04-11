using System;
using System.Collections.Generic;
using System.Text;

namespace TepayLink.Sdisco.AdminConfig
{
    public enum BankTypeEnum
    {
        /// <summary>
      /// Thẻ quốc tế
      /// </summary>
        AtmCard = 2,
        /// <summary>
        /// Thẻ nội địa
        /// </summary>
        CreditCard = 1,
        /// <summary>
        /// Paypal
        /// </summary>
        Paypal = 3,
        VTC = 4
    }
}
