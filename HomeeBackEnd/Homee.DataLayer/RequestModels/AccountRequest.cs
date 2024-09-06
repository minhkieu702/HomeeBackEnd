﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Homee.DataLayer.RequestModels
{
    public class AccountRequest
    {
        public string Email { get; set; } = null!;

        public string Password { get; set; } = null!;

        public string ImageUrl { get; set; } = null!;

        public string Name { get; set; } = null!;

        public string Phone { get; set; } = null!;

        public string CitizenId { get; set; } = null!;
        
        public int Role { get; set; }

        public DateTime? BirthDay { get; set; }

    }
}
