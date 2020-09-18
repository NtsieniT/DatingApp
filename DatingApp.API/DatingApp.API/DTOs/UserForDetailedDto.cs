﻿using DatingApp.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatingApp.API.DTOs
{
    // Class returns users full detail information together with their photos
    public class UserForDetailedDto
    {
        public int Id { get; set; }
        public string Username { get; set; }
       
        public int Age { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string KnownAs { get; set; }
        public DateTime Created { get; set; }
        public DateTime LastActive { get; set; }
        public string Introduction { get; set; }
        public string Interests { get; set; }
        public string LookingFor { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string PhotoUrl { get; set; }
        public ICollection<PhotosForDetailedDto> Photos { get; set; } //Returns filtered d=infor for photos
    }
}
