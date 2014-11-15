using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRTLib
{
    public class MRTRoute
    {
        public MRTExit from;
        public MRTLink[] links;
        public string Description = "";
        public int StationCount
        {
            get
            {
                return this.links.Count();
            }
        }
        public int TransionCount = 0;

        public MRTRoute(MRTExit from, MRTLink[] links)
        {
            this.from = from;
            this.links = links;
            this.Description = from.name;
            string lastRouteID = this.links[0].routeID;
            if (this.links.Count() < 2)
            {
                this.Description += string.Format(" -{0}-> {1}", this.links.Last().routeID, this.links.Last().to.name);
                return;
            }
            for (int x = 1; x < this.links.Count(); x++)
            {
                MRTLink link = this.links[x];
                if (link.routeID != lastRouteID)
                {
                    this.Description += string.Format(" -{0}-> {1}", lastRouteID, this.links[x - 1].to.name);
                    TransionCount++;
                }
                lastRouteID = link.routeID;
            }
            
            this.Description += string.Format(" -{0}-> {1}", this.links.Last().routeID, this.links.Last().to.name);
        }

        public int CompareTo(MRTRoute anotherRoute)
        {
            if (anotherRoute.StationCount > this.StationCount)
            {
                return -1;
            }
            else if (anotherRoute.StationCount < this.StationCount)
            {
                return 1;
            }
            if (anotherRoute.TransionCount > this.TransionCount)
            {
                return -1;
            }
            else if (anotherRoute.TransionCount < this.TransionCount)
            {
                return 1;
            }
            return 0;
        }
    }
}
