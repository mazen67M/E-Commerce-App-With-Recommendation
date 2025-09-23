using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.DTOs
{
    public class ValidationResultDto
    {
        public bool IsValid { get; set; }
        public List<string> Errors { get; set; } = new(); public static ValidationResultDto Success => new ValidationResultDto { IsValid = true }; public static ValidationResultDto Fail(IEnumerable<string> errors)
        {
            return new ValidationResultDto
            {
                IsValid = false,
                Errors = errors.ToList()
            };
        }
    }
}
