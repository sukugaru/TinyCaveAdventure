using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{

    public class Inventory_Action : Action
    // Not really used for the graphical UI, but would be used if an old-style parser
    // plus single window UI is implemented.
    {
        private List<Engine.Object> DisplayList;

        public Inventory_Action()
        {
            sName = "Inventory";
            sProtoCmdLine = "Inventory";
            iNumArgs = 0;
            DisplayList = new List<Engine.Object>();
        }

        public override void DoAction(Engine.Object item1, Engine.Object item2, bool Suppress, ref string OutMessage, ref bool bSuccess)
        {
            OutMessage += "You have: \n";

            World._player.DetermineDisplayList(ref DisplayList);

            foreach (var i in DisplayList)
            {
                OutMessage += i.ToString() + "\n";
            }

            if (World._player.Inventory.Count == 0)
            {
                OutMessage += "   Nothing.\n";
            }
        }
    }

}
