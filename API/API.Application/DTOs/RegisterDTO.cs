using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.Application.DTOs
{
    public class RegisterDTO
    {
        [Required]
        public string? Name { get; set; }
        [Required]
        public string? Surname { get; set; }
        [Required]
        public string? Username { get; set; }        
        [Required]
        public string? Email { get; set; }
        [Required]
        [StringLength(6, MinimumLength = 4)]
        [RegularExpression("[a-zA-Z]+")]
        public string? Password { get; set; }
    }
}