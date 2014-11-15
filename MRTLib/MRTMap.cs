using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRTLib
{
    public class MRTMap
    {
        static MRTMap sharedMap;
        static public MRTMap SharedMap
        {
            get
            {
                if (sharedMap == null)
                {
                    sharedMap = new MRTMap();
                }
                return sharedMap;
            }
        }

        private MRTMap()
        {
            this.Load();
        }

        public Dictionary<string, MRTExit> exits = new Dictionary<string, MRTExit>();
        private void Load()
        {
            string data = MRTData.data;
            foreach (string line in data.Split('\r'))
            {
                string[] components = line.Split(',');
                if (components.Length == 3)
                {
                    string routeID = components[0];
                    string from = components[1];
                    string to = components[2];

                    if (!this.exits.ContainsKey(from))
                    {
                        this.exits.Add(from, new MRTExit(from));
                    }

                    if (!this.exits.ContainsKey(to))
                    {
                        this.exits.Add(to, new MRTExit(to));
                    }
                    MRTExit fromExit = this.exits[from];
                    MRTExit toExit = this.exits[to];
                    fromExit.AddLink(routeID, toExit);
                    toExit.AddLink(routeID, fromExit);
                }
            }
        }

        public MRTRoute[] FindRoutes(string from, string to)
        {
            if (from == null)
            {
                throw new Exception("from is null");
            }
            else if (to == null)
            {
                throw new Exception("to is null");
            }
            else if (from == to)
            {
                throw new Exception("Begin and destination should be different.");
            }

            MRTExit fromExit = this.exits[from];
            MRTExit toExit = this.exits[to];
            if (fromExit == null)
            {
                throw new Exception("Unable to find this exit as begin.");
            }
            else if (toExit == null)
            {
                throw new Exception("Unable to find this exit as destination.");
            }
            MRTRouteFinder finder = new MRTRouteFinder(fromExit, toExit);
            return finder.routes;
        }
    }

    class MRTRouteFinder
    {
        private Stack<MRTLink> visitedLinks = new Stack<MRTLink>();
        private Stack<MRTExit> visitedExits = new Stack<MRTExit>();
        private MRTExit fromExit;
        private MRTExit toExit;
        private List<MRTRoute> foundRoutes = new List<MRTRoute>();
        public MRTRoute[] routes
        {
            get {
                return this.foundRoutes.ToArray();  
            }
        }

        void travelLinksOfExit(MRTExit exit)
        {
            MRTLink[] links = exit.links.ToArray();
            foreach (MRTLink link in links) {
                if (link.to == this.toExit)
                {
                    visitedLinks.Push(link);
                    List<MRTLink> copy = this.visitedLinks.ToList();
                    copy.Reverse();
                    foundRoutes.Add(new MRTRoute(this.fromExit, copy.ToArray()));
                    visitedLinks.Pop();
                }
                else if (!visitedExits.Contains(link.to))
                {
                    visitedExits.Push(exit);
                    visitedLinks.Push(link);
                    travelLinksOfExit(link.to);
                    visitedExits.Pop();
                    visitedLinks.Pop();
                }
            }
        }

        public MRTRouteFinder(MRTExit fromExit, MRTExit toExit)
        {
            this.fromExit = fromExit;
            this.toExit = toExit;
            this.travelLinksOfExit(fromExit);
            this.foundRoutes.Sort((a, b) => a.CompareTo(b));
        }        
    }
}
