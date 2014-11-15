using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRTLib
{
    public class MRTExit
    {
        public string name;
        public List<MRTLink> links = new List<MRTLink>();
        public MRTExit(string name)
        {
            this.name = name;
        }

        public void AddLink(string routeID, MRTExit to)
        {
            MRTLink link = new MRTLink(routeID, to);
            this.links.Add(link);
        }
    }
}
