using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRTLib
{
    public class MRTLink
    {
        public string routeID;
        public MRTExit to;
        public MRTLink(string routeID, MRTExit to)
        {
            this.routeID = routeID;
            this.to = to;
        }
    }
}
