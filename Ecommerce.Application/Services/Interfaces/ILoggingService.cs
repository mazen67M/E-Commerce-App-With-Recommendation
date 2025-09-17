using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Services.Interfaces
{
    public interface ILoggingService
    {
        void LogInfo(string message, object data = null);
        void LogWarning(string message, object data = null);
        void LogError(Exception ex, string message = null, object data = null);
        void LogAudit(string action, string userId, object data = null);
    }
}
