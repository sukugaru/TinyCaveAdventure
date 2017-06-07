using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// 7/6/2017 - Bug 6 - Renaming the Object class to Item.
// 
// 19/5/2017 - Articles Project - Making sure definite and indefinite articles are properly used.
//             Making changes in DoAction so that sDefiniteName gets used.

namespace Engine
{

    public class Wear_Action : Action
    {
        public Wear_Action()
        {
            sName = "Wear";
            sProtoCmdLine = "Wear [item1]";
            iNumArgs = 1;
        }

        public override void DoAction(Item i, Item iFrom, bool Suppress, ref string OutMessage, ref bool bSuccess)
        {
            Item i2;
            Item CurrentlyWearing;
            string PutMessage = ""; // Use this for custom 'putting it on' messages.

            bSuccess = false;
            if (i == null)
            {
                OutMessage += "You need to select something to wear.\n";
                return;
            }

            if (i.hiOwner != World._player)
            {
                OutMessage += "(Taking it first...)\n";
                World._Take.DoAction(i, null, false, ref OutMessage, ref bSuccess);
                //                Take(i, FromLoc, FromObj, ref bSuccess);
                if (bSuccess == false)
                {
                    OutMessage += "Because you couldn't take it, you couldn't wear it.\n";
                    return;
                }
            }

            // Is the action restricted?
            if (World._player.bCanWear == false)
            {
                OutMessage += World._player.sCantWearMsg.Replace("[item]", i.sDefiniteName) + "\n";
                return;
            }

            /* Superceded by restriction system 
            if (World._player.bTiedUp)
            {
                OutMessage += "You can't put anything on, or take anything off, with your " +
                    "arms tied up.\n";
                return;
            } */

            // Check if player is already wearing something in the given slot
            // Will need some item specific behaviour - e.g. you can put a hat on over a wig,
            // but not the other way around.  Will need to figure this out later.
            CurrentlyWearing = World._player.Inventory.Find(x => (x.sClothingType == i.sClothingType) &&
                                                     (x.bWorn) &&
                                                     (x != i));

            if (CurrentlyWearing != null)
            {
                OutMessage += "You can't put " + i.sDefiniteName + " on over the top of " + CurrentlyWearing.sDefiniteName + ".\n";
                return;
            }


            // Item specific behaviours
            // Wig example again
            // if (i == World._wig)
            //{
            //    if (i.bWorn == false)
            //    {
            //        i2 = World._player.Inventory.Find(x => x == World._hat);
            //        if (i2 != null)
            //        {
            //            if (i2.bWorn == true)
            //            {
            //                OutMessage += "You can't put the wig on over the top of the hat.\n";
            //                return;
            //            }
            //        }
            //    }
            //
            //   World._player.Inventory.Remove(World._baldHead);
            //}
            if (i == World._lostGemNecklace)
            {
                OutMessage += "Given its history that's probably a VERY BAD IDEA.\n";
                return;
            }

            // Default behaviour
            if (i.bWearable == false)
            {
                OutMessage += "That is not a wearable item.\n";
                return;
            }
            if (i.bWorn)
            {
                OutMessage += "You are already wearing " + i.sDefiniteName + ".\n";
                return;
            }
            else
            {
                i.bWorn = true;
                if (Suppress == false)
                {
                    if (PutMessage != "")
                    {
                        OutMessage += PutMessage;
                    }
                    else
                    {
                        OutMessage += "You put " + i.sDefiniteName + " on.\n";
                    }
                }
                bSuccess = true;
            }

        }
    }

}
