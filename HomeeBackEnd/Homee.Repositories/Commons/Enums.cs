using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homee.BusinessLayer.Commons
{
    public enum AccountRole
    {
        Free,//buy subscription then be premium
        Premium, 
        Administrator
    }
    public enum PlaceDirection
    {
        North,
        NorthEast,
        East,
        Southeast,
        South,
        Southwest,
        West,
        NorthWest
    }
    public enum PostStatus
    {
        None,
        Premium
    }
    public enum InteriorStatus
    {
        Empty, Basic, Full
    }
}
