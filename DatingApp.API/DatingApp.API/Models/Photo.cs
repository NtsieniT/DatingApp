using System;

namespace DatingApp.API.Models
{
    public class Photo
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }
        public DateTime DateAdded { get; set; }
        public bool IsMain { get; set; }
        public string  PublicId { get; set; }

        // Adding user model in the photo class , entity framework adds/creates
        // a cascade delete in the migrations instead of restricted delete.
        // This means that if a user is deleted, then photos collection are also deleted.
        public User User { get; set; }
        public int UserId { get; set; }
    }
}