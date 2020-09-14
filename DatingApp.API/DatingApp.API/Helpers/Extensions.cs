using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatingApp.API.Helpers
{
    //extensions do not need to create a new instance hence we add static key 
    public static class Extensions
    {
        /*This class will be responsible for writing general extentions for all other extensions 
         * we need
         */

        //AddApplicationError overrides response hence we add 'this' before HttpResponse to override response
        public static void AddApplicationError(this HttpResponse response, string message)
        {
            // 
            response.Headers.Add("Application-Error", message);

            // CORS Headers for angular to get proper access control header
            response.Headers.Add("Access-Control-Expose-Headers", "Application-Error");

            // To allow cross origin between application eg angular from any origin
            response.Headers.Add("Access-Control-Allow-Origin", "*");
        }

        // Creating an extension method to calcilate date
        public static int CalculateAge(this DateTime dateTime)
        {
            var age = DateTime.Today.Year - dateTime.Year;
            //this checks if the person has had their birthday or not
            if (dateTime.AddYears(age) > DateTime.Today)
            {
                age--;
            }
            return age;
        }
    }
}
