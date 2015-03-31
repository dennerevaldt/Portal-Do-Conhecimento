using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PDS.Models.Entities
{
    public class Followers
    {
        public Int64 idFollower { get; set; }
        public Int64 idFollowing { get; set; }
    }
}