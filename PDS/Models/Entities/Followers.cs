using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PDS.Models.Entities
{
    public class Followers
    {
        public Int32 idFollower { get; set; }

        public Int32 idFollowing { get; set; }
    }
}