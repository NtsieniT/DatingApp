using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatingApp.API.Helpers
{
    // This class is used to store user parameters
    public class UserParams
    {
        private const int MaxPageSize = 50; // Used to set limit

        public int PageNumber { get; set; } = 1; // give a default value to always return the first page
        
        private int pageSize = 10;

        public int PageSize
        {
            get { return pageSize; }
            set { pageSize = (value > MaxPageSize) ? MaxPageSize : value; }
        }

        // Below poperties are used for filtering
        public int UserId { get; set; }
        public string Gender { get; set; }
        public int MinAge { get; set; } = 18;
        public int MaxAge { get; set; } = 99;


        //for ordering
        public string OrderBy { get; set; }

    }
}
