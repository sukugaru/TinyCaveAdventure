using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{

    public class LookAtItem_Action : Action
    {

        public LookAtItem_Action()
        {
            iNumArgs = 1;
            sProtoCmdLine = "Look at [item1]";
            sName = "Look at";
        }

        public override void DoAction(Engine.Object i, Engine.Object Item2, bool Suppress, ref string OutMessage, ref bool bSuccess)
        {
            OutMessage = "";
            bSuccess = false;

            if (i == null)
            {
                OutMessage += "You need to select something to look at.\n";
                return;
            }

            // If you want to override sDescription in some way, you can do that here
            // Though why not just modify sDescription?
            if (i == World._head)
            {
                if (World._player.HasItem(World._tribalHeadgear) && (World._tribalHeadgear.bWorn))
                {
                    OutMessage += "This is your head, currently covered by the Tribal Headgear.\n";
                }
                else
                {
                    OutMessage += "This is your head, still firmly attached to your neck.\n";
                }
                return;
            }


            OutMessage += i.sDescription;


            if (i.bWorn)
            {
                OutMessage += "  Currently being worn.";
            }

            // Other item-specific text might be needed.


            // Generic text - adding info about containers and what's in them.
            OutMessage += "\n";

            if (i.bContainer)
            {
                if (i.bLocked == false)
                {
                    i.bDiscoveredContents = true;
                    OutMessage += "It contains:\n";
                    foreach (var item in i.Inventory)
                    {
                        OutMessage += "   " + item.sName + "\n";
                    }
                    if (i.Inventory.Count == 0)
                    {
                        OutMessage += "   Nothing.\n";
                    }
                }
                else
                {
                    OutMessage += "It's locked.\n";
                    i.bDiscoveredContents = false;
                }
            }
            bSuccess = true;
        }
    }

}
