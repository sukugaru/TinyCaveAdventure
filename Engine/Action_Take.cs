using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{

    public class Take_Action : Action
    {
        public Take_Action()
        {
            sName = "Take";
            sProtoCmdLine = "Take [item1]";
            iNumArgs = 1;
        }

        public override void DoAction(Engine.Object i, Engine.Object iFrom, bool Suppress, ref string OutMessage, ref bool bSuccess)
        {
            Engine.Object i2;
            bSuccess = false;

            // Sanity Checks

            if (i == null)
            {
                OutMessage += "Please select something to take.\n";
                return;
            }

            // Is the action restricted?
            if (World._player.bCanTake == false)
            {
                OutMessage += World._player.sCantTakeMsg.Replace("[item]", i.sDefiniteName) + "\n";
                return;
            }


            // When tied up, the player can only hold one thing at a time
            // Indicated by _player.iCarrySize, which is 1 when tied up.
            // (This is temporarily relaxed to 2 when using UseWith)
            if ( (World._player.carrying() >= World._player.iCarrySize) &&
                 (World._player.iCarrySize > 0)
                )
            {
                if (World._player.bTiedUp)
                {
                    OutMessage += "As you are tied up, you " +
                        "can only carry one thing at a time, and very awkwardly.\n";
                    return;
                }
                OutMessage += "You're already juggling as many things as you can carry.\n";
            }

            // You need to be on the ground in the Stalagmite Cave to take anything
            if ( (World._player.CurrentLocation == World._stalagmiteCave) && 
                 (World._player.sState != "")
                )
            {
                OutMessage += "You're not picking anything up here while you're jumping " +
                    "around the place.\n";
                return;
            }

            // Use Get Out action if the object is in another object
            if ((i.hiOwner != null) &&
                 (i.hiOwner.GetType() == typeof(Engine.Object))
               )
            {
                //                GetOut(i, (Engine.Object)i.hiOwner, ref bSuccess);
                World._GetOutOf.DoAction(i, (Engine.Object)i.hiOwner, false, ref OutMessage, ref bSuccess);
                return;
            }

            // Item specific handling for the Treasure item.  It goes here to avoid the
            // bTakeable check
            if (i == World._treasure)
            {
                World._player.CurrentConversation = World._treasureConversation;
                return;
            }

            // Is it actually something you can take?
            // If bTakeable is false then it tends to be some sort of scenery item.
            if (i.bTakeable == false)
            {
                OutMessage += "That isn't something you can pick up.\n";
                return;
            }
            if (i.hiOwner == World._player)
            {
                OutMessage += "You already have it.\n";
                return;

            }

            // Item specific behaviours

            // After taking both tribal headgear and costume, change description of the stone
            // statue.
            if ( (i == World._tribalCostume) && 
                 (i.hiOwner == World._abandonedShrineSite)
                )
            {
                if (World._tribalHeadgear.hiOwner != World._abandonedShrineSite)
                {
                    World._abandonedShrine.sDescription = "This is an old abandoned shrine, " +
                        "adorned with a stone statue.";
                }
            }

            if ( (i == World._tribalHeadgear) &&
                 (i.hiOwner == World._abandonedShrineSite)
                )
            {
                if (World._tribalCostume.hiOwner != World._abandonedShrineSite)
                {
                    World._abandonedShrine.sDescription = "This is an old abandoned shrine, " +
                        "adorned with a stone statue.";
                }

            }

            // Taking the lost gem
            if (i == World._lostGemNecklace)
            {
                OutMessage += "You take the necklace, being careful to disturb Dotty as " +
                    "little as possible.\n";
                Suppress = true;
                World._questGiver.sDescription = "The House Owner and Quest Giver is here, though " +
                    "at this point it's obvious " +
                    "there's a lot more to her than a simple Quest Giver owning a white house.  " +
                    "Let's call her The Witch.  Or better yet, let's call her by her name, Dotty.\n\n" +
                    "Dotty is sprawled ungainly in the middle of the ruins of her house, very " +
                    "very still.";
            }



            // Default behaviour
            // (Remove from owner's inventory and put into player's inventory)
            if (Suppress == false)
            {
                OutMessage += "You take " + i.sName + "\n";
            }
            i.hiOwner.Remove(i);
            World._player.Add(i);
            bSuccess = true;

        }
    }

}
