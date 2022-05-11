using Microsoft.AspNetCore.Identity;
using System;

namespace HealthApp.API.Models
{
    public class User : IdentityUser
    {
        public DateTimeOffset? DateCreated { get; set; }

        public DateTimeOffset? LastLogin { get; set; }
    }
}
