using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// 7/6/2017 - Bug 6 - Renaming the Object class to Item.

namespace Engine
{

    public class Use_Action : Action
    {
        public Use_Action()
        {
            sName = "Use";
            sProtoCmdLine = "Use [item1]";
            iNumArgs = 1;
        }

        public override void DoAction(Item i, Item item2, bool Suppress, ref string OutMessage, ref bool bSuccess)
        {
            bSuccess = false;

            // Sanity checks
            if (i == null)
            {
                OutMessage += "Please select something to use.\n";
                return;
            }

            // Is the action restricted?
            if ( (World._player.bCanUse == false) &&
                 (i.bUsableAnyway == false)
                )
            {
                OutMessage += World._player.sCantUseMsg.Replace("[item]", i.sDefiniteName) + "\n";
                return;
            }

            /* Superceded by restriction system
            if ( (World._player.bTiedUp) &&
                 (i.bUsableWhileTiedUp == false)
                )
            {
                OutMessage += "You can't use the " + i.sName + " with your hands tied " +
                    "up like this.\n";
                return;
            } */

            // GetItemToUse (take if you don't have it, remove it if you're wearing it)
            // type stuff is done in specific item handler code.
            // GetItemToUse is a public method in the UseWith_Action class.


            // We are now past the sanity checks, so now to use the item!

            // First, any generic stuff
            // If you Use a wearable item, the game interprets this as put on/take off
            if (i.bWearable)
            {
                if (i.bWorn)
                {
                    World._Remove.DoAction(i, null, false, ref OutMessage, ref bSuccess);
                }
                else
                {
                    World._Wear.DoAction(i, null, false, ref OutMessage, ref bSuccess);
                }
            }

            // If I put in openable containers, then similarly, Use would map to Open/Close
            // At the moment, though, the Look At Item action implies opening and closing, so 
            // maybe I won't ever actually implement Open/Close.


            // Now for Item specific actions.  Most of these are separate procedures.
            // Sometimes multiple items will call the same procedure.

            if (i == World._MazeBook)
            {
                UseMazeBook(i, item2, false, ref OutMessage, ref bSuccess);
            }

            if (i == World._head)
            {
                UseHead(i, item2, false, ref OutMessage, ref bSuccess);
            }


            if (i == World._XXiumSaw)
            {
                UseXXiumSaw(i, item2, false, ref OutMessage, ref bSuccess);
            }

            if (i == World._map)
            {
                UseMap(i, item2, false, ref OutMessage, ref bSuccess);

            }

            // Item specific actions, without separate procedures.
            // This is really simple noddy stuff.  If it's anything more complicated than
            // a line or two, put it in a separate method.

            if (i == World._vendingMachine)
            {
                OutMessage += "While you might be tied up, it's not too hard to use the vending " +
                    "machine.\n";
                World._player.CurrentConversation = World._vendingMachineConversation;
            }

            if (i == World._WallMap)
            {
                World._wallMapConversation.sMode = "WallMap";
                World._player.CurrentConversation = World._wallMapConversation;
            }

            if (i == World._parkourManual)
            {
                World._player.CurrentConversation = World._parkourManualConversation;
            }

            if (i == World._tinnedFish)
            {
                OutMessage += "You don't have a can opener.\n";
            }

            if (i == World._fizzyDrink)
            {
                OutMessage += "Refreshing!\n";
                World._player.Remove(World._fizzyDrink);
            }

            bSuccess = true;
        }

        private void UseHead(Item i, Item item2, bool Suppress, ref string OutMessage, ref bool bSuccess)
        {
            OutMessage += "You think about things.\n";
        }

        private void UseMazeBook(Item i, Item item2, bool Suppress, ref string OutMessage, ref bool bSuccess)
        {
            World._UseWith.GetToUse(i, ref OutMessage, ref bSuccess);
            if (bSuccess)
            {
                if (World._player.bTiedUp == false)
                {
                    OutMessage += "You open the book and start reading.\n";
                }
                else
                {
                    OutMessage += "With your arms tied up it's quite awkward, but you can " +
                        "still read the book and turns the pages.\n";
                }
                World._player.CurrentTextSequence = World._mazeBookSequence;
            }
            else
            {
                OutMessage += "You couldn't get the book to read it.\n";
                bSuccess = false;
            }
        }

        private void UseXXiumSaw(Item i, Item item2, bool Suppress, ref string OutMessage, ref bool bSuccess)
        {
            World._UseWith.GetToUse(i, ref OutMessage, ref bSuccess);
            if (bSuccess)
            {
                if (World._player.bTiedUp == false)
                { }
                else
                {
                    OutMessage += "With your arms tied up it's quite awkward, but you " +
                        "manage to use the saw to cut through your bonds and finally, " +
                        "*finally* free your hands.\n";
                    World._player.bTiedUp = false;
                    World._player.FreeHands();
//                    World._player.iCarrySize = 0;
                    World._highLedge.DownLoc = World._stalagmiteCave;
                    World._stalagmiteCave.UpLoc = World._highLedge;
                    World._stalagmiteCave.bSuppressGoMsg = true;
                    bSuccess = true;
                }
            }
            else
            {
                OutMessage += "You couldn't get the saw to use it.\n";
            }
        }

        private void UseMap(Item i, Item item2, bool Suppress, ref string OutMessage, ref bool bSuccess)
        {
            if (World.MazeLocations.HasPlayer() == false)
            {
                OutMessage += "You have a look at the map and admire your handiwork.\n";
            }
            else
            {
                World._wallMapConversation.sMode = "OwnMap";
                World._player.CurrentConversation = World._wallMapConversation;
            }

        }


    }



}
