using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Application.DTOs
{
    public class UserDTO
    {
        public string? Username { get; set; }
        public string? Token { get; set; }

        public float BTCAmount { get; set; }
        public float USDAmount { get; set; }
    }
}