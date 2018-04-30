using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Microsoft.AspNetCore.Identity;

namespace DoorBash.Persistence
{
    public class User : IdentityUser
    {
        [Required]
        public String FullName { get; set; }
    }
}
