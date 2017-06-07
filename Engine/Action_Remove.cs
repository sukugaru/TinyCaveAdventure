using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// 7/6/2017 - Bug 6 - Renaming the Object class to Item.
// 
// 19/5/2017 - Articles Project - Making sure definite and indefinite articles are properly used.

namespace Engine
{

    public class Remove_Action : Action
    {
        public Remove_Action()
        {
            sName = "Remove";
            sProtoCmdLine = "Remove [item1]";
            iNumArgs = 1;
        }

        public override void DoAction(Item i, Item iFrom, bool Suppress, ref string OutMessage, ref bool bSuccess)
        {
            Item i2;

            bSuccess = false;

            // Sanity checks
            if (i == null)
            {
                OutMessage += "Please select something to take off.\n";
                return;
            }

            // Is the action restricted?
            if (World._player.bCanRemove == false)
            {
                OutMessage += World._player.sCantRemoveMsg.Replace("[item]", i.sDefiniteName) + "\n";
                return;
            }


            if (i.bWearable == false)
            {
                OutMessage += "That is not a wearable item.\n";
                return;
            }

            if (i.bWorn == false)
            {
                OutMessage += "You are not wearing " + i.sDefiniteName + ".\n";
                return;
            }

            // Item specific behaviours

            if ( ( (i == World._tribalCostume) || (i == World._tribalHeadgear) ) &&
                 (World._player.CurrentLocation == World._tribalCavern)
                )
            {
                OutMessage += "That would be a *spectacularly* bad idea.\n";
                return;
            }

            if (i == World._head)
            {
                OutMessage += "Don't be silly, you can't remove your head!\n";
                return;
            }

            // Default behaviour
            i.bWorn = false;
            if (Suppress == false)
                OutMessage += "You remove " + i.sDefiniteName + ".\n";
            bSuccess = true;
        }
    }

}
