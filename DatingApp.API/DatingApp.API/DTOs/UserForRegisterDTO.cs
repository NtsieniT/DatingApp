﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DatingApp.API.DTOs
{
    //Used to map domain models to simpler objects that get returened or displayed by the view
    public class UserForRegisterDTO
    {
        [Required]
        public string Username { get; set; }
        
        [Required]
        [StringLength(8,MinimumLength =4,ErrorMessage ="Password must be at least 4 and 8 characters.")]
        public string Password { get; set; }
    }
}
