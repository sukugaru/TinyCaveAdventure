using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{

    public class LocationGroup
    // This class lets you group a number of locations together.
    // A group of locations could be, for example an "InPublic" list and you can see
    // if the player is in the InPublic list.
    // Another example, you can set a group of locations as "InApartment" and see if an
    // item is in the apartment.
    {
        public List<Location> LocationList;

        public LocationGroup()
        {
            LocationList = new List<Location>();
        }

        public LocationGroup(List<Location> InList)
        {
            LocationList = InList;
        }

        public void Set(List<Location> InList)
        {
            LocationList = InList;
        }

        public bool HasItem(Object SearchItem)
        {
            bool returnValue = false;

            foreach (var l in LocationList)
            {
                if (l.HasItem(SearchItem))
                {
                    returnValue = true;
                }
            }

            return returnValue;
        }

        public bool HasPlayer()
        {
            bool returnValue = false;

            foreach (var l in LocationList)
            {
                if (World._player.CurrentLocation == l)
                {
                    returnValue = true;
                }
            }

            return returnValue;
        }


    }

}
