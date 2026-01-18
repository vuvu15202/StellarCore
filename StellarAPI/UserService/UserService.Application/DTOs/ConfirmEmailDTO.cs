using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;
namespace UserService.Application.DTOs
{
    public class ConfirmEmailDTO
    {
        [Required(ErrorMessage = "User ID is required.")]
        public Guid UserId { get; set; }

        [Required(ErrorMessage = "Confirmation token is required.")]
        public string Token { get; set; } = null!;
    }
}
