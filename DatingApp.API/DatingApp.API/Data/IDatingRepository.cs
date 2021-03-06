﻿using DatingApp.API.Helpers;
using DatingApp.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatingApp.API.Data
{
    public interface IDatingRepository
    {
        /*
         * T is generic
         * entity is the parameter
         * and retriction is for Where T is a class
         */
        void Add<T>(T entity) where T : class;
        void Delete<T>(T entity) where T : class;
        Task<bool> SaveAll();//save all if there are more then 1 data to save
        Task<PagedList<User>> GetUsers(UserParams userParams); // Gets all users
        Task<User> GetUser(int id); // Get an individual user
        Task<Photo> GetPhoto(int id);
        Task<Photo> GetMainPhotoForUser(int userId);
    }
}
