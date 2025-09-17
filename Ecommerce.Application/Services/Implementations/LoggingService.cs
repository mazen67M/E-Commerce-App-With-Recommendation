using Ecommerce.Application.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Services.Implementations
{
    public class LoggingService : ILoggingService
    {
        public void LogAudit(string action, string userId, object data = null)
        {
            throw new NotImplementedException();
        }

        public void LogError(Exception ex, string message = null, object data = null)
        {
            throw new NotImplementedException();
        }

        public void LogInfo(string message, object data = null)
        {
            throw new NotImplementedException();
        }

        public void LogWarning(string message, object data = null)
        {
            throw new NotImplementedException();
        }
    }
}
