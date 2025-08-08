using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Core.Enums
{
    public enum PaymentStatus
    {
        Pending,     // في الانتظار
        Paid,        // مدفوع
        Failed,      // فشل الدفع
        Refunded,    // تم الاسترداد
        PartiallyRefunded // استرداد جزئي
    }
}
