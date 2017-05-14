using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public override void DoAction(Engine.Object i, Engine.Object iFrom, bool Suppress, ref string OutMessage, ref bool bSuccess)
        {
            Engine.Object i2;

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

            /* Superceded by restrictions system 
            if (World._player.bTiedUp)
            {
                OutMessage += "You can't put anything on, or take anything off, with your " +
                    "arms tied up.\n";
                return;
            } */

            if (i.bWearable == false)
            {
                OutMessage += "That is not a wearable item.\n";
                return;
            }

            if (i.bWorn == false)
            {
                OutMessage += "You are not wearing " + i.sName + "\n";
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
                OutMessage += "You remove " + i.sName + "\n";
            bSuccess = true;
        }
    }

}
