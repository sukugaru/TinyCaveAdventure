using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// 7/6/2017 - Bug 6 - Renaming the Object class to Item.
// 
// 19/5/2017 - Articles Project - Making sure definite and indefinite articles are properly used.
//             A few minor changes in GetToUse().

namespace Engine
{

    public class UseWith_Action : Action
    // Complicated!
        // Basically goes like this:
        //  * DoAction is a wrapper method.  It temporarily sets carrysize to 2 if tied up, calls
        //    WrappedDoAction to attempt the UseWith, then handles the "You cannot use those items
        //    together" error message after coming back from WrappedDoAction.
        //  * WrappedDoAction calls GetToUse to get the items so as to use them.
        //  * WrappedDoAction does some basic sanity checks, then based on the two items calls
        //    a further method to handle those two items.
        // It's a bit fiddly and probably needs some fixing.
    {
        public UseWith_Action()
        {
            sName = "Use with";
            sProtoCmdLine = "Use [item1] with [item2]";
            iNumArgs = 2;
        }

        public override void DoAction(Item item1, Item item2, bool Suppress, ref string OutMessage, ref bool bSuccess)
        {
            bSuccess = false;
            int OldCarrySize = World._player.iCarrySize;
            string tempMessage = "";
            bool bDoneSomething = false;

//            if ((World._player.bTiedUp))
            if (World._player.iCarrySize < 2)
            {
                World._player.iCarrySize = 2; // Setting to 2 for this method
            }


            // This is a wrapper method - most of the action is in WrappedDoAction
            // Things have been done this way so that this wrapper method can do all
            // the fiddling with the carry size, and a "Hey that didn't make sense"
            // message.
            WrappedDoAction(item1, item2, ref tempMessage, ref bDoneSomething, ref bSuccess);

            if ((bSuccess) && (bDoneSomething == false))
            {
                tempMessage += "While that command may have sounded perfectly OK in your head, " +
                              "it doesn't make sense to this game.  Sorry!\n";
            }
            OutMessage += tempMessage;

            World._player.iCarrySize = OldCarrySize;

        }

        public void GetToUse(Item i, ref string OutMessage, ref bool bSuccess)
        {
            if ((i.hiOwner != World._player) && (i.bTakeable == true))
            {
                OutMessage += "(Taking " + i.sDefiniteName + " first...)\n";
                World._Take.DoAction(i, null, false, ref OutMessage, ref bSuccess);
                if (bSuccess == false)
                {
                    OutMessage += "You could not get " + i.sDefiniteName + " to use it.\n";
                    return;
                }
            }
            if ((i.bWorn) && (i.bTakeable = true))
            {
                OutMessage += "(Removing " + i.sDefiniteName + " first...)\n";
                World._Remove.DoAction(i, null, false, ref OutMessage, ref bSuccess);
                if (bSuccess == false)
                {
                    OutMessage += "You could not get " + i.sDefiniteName +  " to use it.\n";
                    return;
                }
            }
            bSuccess = true;

        }

        public void WrappedDoAction(Item item1, Item item2, ref string OutMessage, ref bool bDoneSomething, ref bool bSuccess)
        {
            bSuccess = false;
            bDoneSomething = false;

            // Sanity checks
            if ((item1 == null) || (item2 == null))
            {
                OutMessage += "Please select something to use and something to use it with.\n";
                return;
            }

            // Can we get the items to use them?
            GetToUse(item1, ref OutMessage, ref bSuccess);
            if (bSuccess == false)
            {
                return;
            }

            GetToUse(item2, ref OutMessage, ref bSuccess);
            if (bSuccess == false)
            {
                return;
            }

            // Yes, if you're tied up, this can leave you holding two items.
            // Rather than try to figure out which one to drop, I figure to just leave them
            // carried by the player.


            // Restriction check

            if (World._player.bCanUse == false)
            {
                if ((item1.bUsableAnyway == false) && (item2.bUsableAnyway == false))
                {
                    OutMessage += World._player.sCantUseMsg.Replace("[item]",
                        "either " + (item1.sDefiniteName + " or " + item2.sDefiniteName)
                        ) + "\n";
                    return;
                }
                if (item1.bUsableAnyway == false)
                {
                    OutMessage += World._player.sCantUseMsg.Replace("[item]",
                        item1.sDefiniteName
                        ) + "\n";
                    return;
                }
                if (item2.bUsableAnyway == false)
                {
                    OutMessage += World._player.sCantUseMsg.Replace("[item]",
                        item2.sDefiniteName
                        ) + "\n";
                    return;
                }
                bSuccess = false;
            }

            /* Superceded by restriction system
            // Tied-up check
            
            if (World._player.bTiedUp)
            {
                if ((item1.bUsableWhileTiedUp == false) && (item2.bUsableWhileTiedUp == false))
                {
                    OutMessage += "You can't use either " + item1.sName + " or " +
                                  item2.sName + " while tied up.\n";
                    return;
                }
                if (item1.bUsableWhileTiedUp == false)
                {
                    OutMessage += "You can't use " + item1.sName + " while tied up.\n";
                    return;
                }
                if (item2.bUsableWhileTiedUp == false)
                {
                    OutMessage += "You can't use " + item2.sName + " while tied up.\n";
                    return;
                }

            } */
             

            // That should be it with sanity checks.  On to specific items!
            // The way this works that an item, or a group of items, can trigger a private 
            // method inside the UseWith_Action class.
            // Because the action to use is based on two items, this seems like the best way,
            // rather than trying to fiddle with a massive amount of polymorphism.

            if (((item1 == World._PaperAndStationeryKit) && (item2 == World._WallMap)) ||
                 ((item2 == World._WallMap) && (item1 == World._PaperAndStationeryKit))
               )
            {
                UseKitAndMap(ref OutMessage, ref bSuccess);
                if (bSuccess == false)
                {
                    return;
                }
                bDoneSomething = true;
            }


            if (((item1 == World._map) && (item2 == World._abstractDesigns)) ||
                ((item2 == World._abstractDesigns) && (item1 == World._map))
               )
            {
                UseDesignsAndMap(ref OutMessage, ref bSuccess);
                if (bSuccess == false)
                {
                    return;
                }
                bDoneSomething = true;
            }

            if ( ((item2 == World._bottledWater) && (item1 == World._sachet)) ||
                 ((item1 == World._bottledWater) && (item2 == World._sachet))
               )
            {
                UseWaterAndSachet(ref OutMessage, ref bSuccess);
                if (bSuccess == false)
                {
                    return;
                }
                bDoneSomething = true;
            }

            if ( ((item2 == World._abstractDesigns) && (item1 == World._PaperAndStationeryKit)) ||
                 ((item1 == World._abstractDesigns) && (item2 == World._PaperAndStationeryKit))
               )
            {
                UseKitAndSlopeDesigns(ref OutMessage, ref bSuccess);
                if (bSuccess == false)
                {
                    return;
                }
                bDoneSomething = true;
            }

            if ( ((item2 == World._XXiumSaw) && (item1 == World._stalagmiteBase)) ||
                 ((item1 == World._XXiumSaw) && (item2 == World._stalagmiteBase))
               )
            {
                UseSawAndStgmiteBase(ref OutMessage, ref bSuccess);
                if (bSuccess == false)
                {
                    return;
                }
                bDoneSomething = true;
            }

            if ( ((item2 == World._fizzyDrink) && (item1 == World._lobster)) ||
                 ((item1 == World._fizzyDrink) && (item2 == World._lobster))
               )
            {
                UseDrinkAndLobster(ref OutMessage, ref bSuccess);
                if (bSuccess == false)
                {
                    return;
                }
                bDoneSomething = true;
            }

            if ( ((item2 == World._lostGemNecklace) && (item1 == World._treasure)) ||
                 ((item1 == World._lostGemNecklace) && (item2 == World._treasure))
               )
            {
                UseNecklaceAndTreasure(ref OutMessage, ref bSuccess);
                if (bSuccess == false)
                {
                    return;
                }
                bDoneSomething = true;
            }


            // At the end!
            bSuccess = true;
        }

        private void UseKitAndSlopeDesigns(ref string OutMessage, ref bool bSuccess)
        {
            OutMessage += "Unfortunately the designs in this passage are only a partial " +
                "map.  You'll need to copy from a much more extensive map to make your own.\n";
        }

        private void UseKitAndMap(ref string OutMessage, ref bool bSuccess)
        {
            if (World._player.HasItem(World._map) == false)
            {
                if (World._player.bTiedUp)
                {
                    OutMessage += "It's quite awkward with your hands still tied up, but " +
                        "you use the stationery kit to make your own map of the maze.  You " +
                        "have to use all the paper to do it, and you use up a couple of the " +
                        "pens as well.\n";
                }
                else
                {
                    OutMessage += "You use the stationery kit to make your own map of the maze.  You " +
                        "have to use all the paper to do it, and you use up a couple of the " +
                        "pens as well.\n";
                }
                World._player.Add(World._map);
                World._player.Remove(World._PaperAndStationeryKit);
                

            }
            else
            {
                OutMessage += "You've already made yourself a map of the maze.\n";
                return;
            }
            bSuccess = true;

        }

        private void UseDesignsAndMap(ref string OutMessage, ref bool bSuccess)
        {
            if (World._player.bKnowAdditionalLocations == false)
            {
                    OutMessage += "You compare your map to the designs on the walls of the " +
                        "passage, and are able to fill in a couple more points of interest.\n";
                    World._player.bKnowAdditionalLocations = true;

            }
            else
            {
                OutMessage += "You've already noted down the extra points of interest on " +
                    "your map.\n";
                return;
            }
            bSuccess = true;

        }

        private void UseWaterAndSachet(ref string OutMessage, ref bool bSuccess)
        {
            OutMessage += "You pour the contents of the sachet into the bottle of water and " +
                "shake thoroughly.\n";
            World._player.Remove(World._bottledWater);
            World._player.Remove(World._sachet);
            World._bottledWater.hiOwner = null;
            World._sachet.hiOwner = null;
            World._player.Add(World._lemonWater);
        }

        private void UseDrinkAndLobster(ref string OutMessage, ref bool bSuccess)
        {
            OutMessage += "You shake the bottle like mad for several minutes.  The lobster " +
                "flails at you madly all the while, unable to reach you through the cave " +
                "entrance, which is much too small for it.  You keep on shaking the bottle.  " +
                "The lobster keeps on flailing, too maddened to think of any strategy for " +
                "getting at you.  You give the bottle a few more shakes, just for luck, then " +
                "point it at the lobster and pull the lid off and shoot.\n\n" +
                "The jet of soft drink hits the Giant Enemy Lobster square in its weak spot " +
                "for massive damage.  It slumps, defeated, glowing " +
                "slightly, then a bit more, then a LOT more -\n\n" +
                "The entranceway of the cave shields you as there is a sudden massive " +
                "magical blowback, hurtling northwards, there is a massive explosion off in " +
                "the distance, and suddenly the compulsion forcing you to stay in the cave " +
                "is gone.\n\n" +
                "\"You... you defeated the guardian and the witch,\" a voice says in your " +
                "mind.  \"Your way is clear.\"\n";
            World._caveEntrance.NorthwestLoc = World._ImpassableMountains;
            World._player.Remove(World._fizzyDrink);
            World._caveEntrance.Remove(World._lobster);
            World._ImpassableMountains.Add(World._lobster);
            World.MakeRuins();
        }


        private void UseSawAndStgmiteBase(ref string OutMessage, ref bool bSuccess)
        {
            if (World._player.sState.StartsWith("On "))
            {
                OutMessage += "You need to get down to the ground before doing that.\n";
                bSuccess = false;
                return;
            }
            if (World._chunkOfStalagmite.hiOwner != null)
            {
                OutMessage += "You already have a chunk of stalagmite, somewhere.  By the " + 
                    "power vested in the XXium Saw by game logic, it doesn't work.  You need to " +
                    "get the chunk of stalagmite you have somewhere else instead.\n";
                bSuccess = false;

            }
            else
            {
                OutMessage += "Using the XXium saw, you easily slice off a small chunk of " +
                    "stalagmite.\n";
                World._player.Add(World._chunkOfStalagmite);
                bSuccess = true;
            }
        }

        

        private void UseNecklaceAndTreasure(ref string OutMessage, ref bool bSuccess)
        {
            OutMessage += "You return the necklace to the treasure hoard.\n\n" +
                "\"At last,\" the voice says in your head.  \"Together again!  All affected " +
                "by the curse, yourself included, are now completely free of its effects!  You " +
                "may go tell them the good news.  Thank you for all your efforts.\"\n";
            World._player.bLiftedCurse = true;
            World._player.CurrentTextSequence = World._endSequence;
        }



    }

}
