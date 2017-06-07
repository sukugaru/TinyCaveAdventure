using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// 7/6/2017 - Bug 6 - Renaming the Object class to Item.

namespace Engine
{

    public class LookAtLocation_Action : Action
    // Not really used for the graphical UI, but would be used if an old-style parser
    // plus single window UI is implemented.  This outputs the current location's
    // description.
    {
        public LookAtLocation_Action()
        {
            sName = "Look at location";
            sProtoCmdLine = "Look at location";
            iNumArgs = 0;
        }

        public override void DoAction(Item item1, Item item2, bool Suppress, ref string OutMessage, ref bool bSuccess)
        {
            string s;

            OutMessage += World._player.CurrentLocation.sName + "\n";
            OutMessage += World._player.CurrentLocation.sDescription + "\n";

            s = "";
            s = World._player.CurrentLocation.Exits();
            if (s != "") { s += "\n"; }
            OutMessage += s;

            s = "";
            s = World._player.CurrentLocation.BlockedExits();
            if (s != "") { s += "\n"; }
            OutMessage += s;

            s = "";
            s = World._player.CurrentLocation.LockedExits();
            if (s != "") { s += "\n"; }
            OutMessage += s;
        }
    }

}
