using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Engine
{
    [DataContractAttribute(IsReference = true)]
    public class Direction
    {
        [DataMember()]
        public string sName { get; set; }

        public Direction(string InDirName)
        {
            sName = InDirName;
        }

        public override string ToString()
        {
            return sName;
        }

        public Location TargetLocation(Location InLocation)
        // If you move in this Direction from InLocation, where do you end up?
        // Not assuming that InLocation is _player.CurrentLocation.  Because reasons.  You
        // might at one point want to determine what location lies in a Direction from
        // another location.  Maybe?
        // Could be useful for NPC movements I guess.
        {
            Pathway p;

            if (InLocation.Pathways.Count == 0)
            {
                switch (sName)
                {
                    case "North":
                        return InLocation.NorthLoc;
                    case "Northeast":
                        return InLocation.NortheastLoc;
                    case "East":
                        return InLocation.EastLoc;
                    case "Southeast":
                        return InLocation.SoutheastLoc;
                    case "South":
                        return InLocation.SouthLoc;
                    case "Southwest":
                        return InLocation.SouthwestLoc;
                    case "West":
                        return InLocation.WestLoc;
                    case "Northwest":
                        return InLocation.NorthwestLoc;
                    case "Up":
                        return InLocation.UpLoc;
                    case "Down":
                        return InLocation.DownLoc;
                    default:
                        return null;
                }
            }
            else
            {
                p = InLocation.Pathways.Find(x => (x.dir == this));
                if (p != null)
                { 
                    return p.TargetLocation; 
                }
                else
                {
                    return null;
                }

            }
        }

        /*
         * I considered putting Move here in the Direction class, and after doing
         * Direction.TargetLocation(location) you'd do Direction.Move(one of the locations, OutMessage)
         * It seemed complicated and not obvious, and you didn't get anything useful from Move
         * being a method in Direction.
         * Move is instead the static MoveTo method in the World class.

         * */
    }

}
