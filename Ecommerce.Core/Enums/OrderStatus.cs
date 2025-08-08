using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Core.Enums
{
    public enum OrderStatus
    {
        Pending,     // طلب معلق
        Confirmed,   // تمت الموافقة
        Shipped,     // تم الشحن
        Delivered,   // تم التسليم
        Cancelled,   // تم الإلغاء
        Returned     // تم الإرجاع
    }
}
