using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{

    public class PutInto_Action : Action
    {
        public PutInto_Action()
        {
            sName = "Put into";
            sProtoCmdLine = "Put [item1] into [item2]";
            iNumArgs = 2;
        }

        public override void DoAction(Engine.Object i, Engine.Object iTo, bool Suppress, ref string OutMessage, ref bool bSuccess)
        {
            Engine.Object i2;
            bSuccess = false;
            string PutMessage = "";         // Used if a specific 'put item into other item' message
                                            // is needed.
            string s;

            // Sanity checks


            if ((i == null) && (iTo != null))
            {
                if (iTo.bNPC)
                {
                    OutMessage += "You can't do that.\n";
                }
                else
                {
                    OutMessage += "Please select something to put into " + iTo.sName + ".\n";
                }
                return;
            }

            if ((i != null) && (iTo == null))
            {
                OutMessage += "Please select something to put " + i.sName + " into.\n";
                return;
            }

            if ((i == null) && (iTo == null))
            {
                OutMessage += "Please select something, and something to put it into.\n";
                return;
            }

            if (iTo.bNPC)
            {
                OutMessage += "You can't do that.\n";
                return;
            }

            if (iTo.bContainer == false)
            {
                OutMessage += iTo.sName + " is not something you can put things into.\n";
                return;
            }

            // Is the action restricted?
            if (World._player.bCanPutIn == false)
            {
                s = World._player.sCantGetOutMsg.Replace("[item1]", i.sDefiniteName);
                s = s.Replace("[item2]", iTo.sDefiniteName) + "\n";
                OutMessage += s;
                return;
            }


            if (iTo.bLocked)
            {
                OutMessage += iTo.sName + " is locked.\n";
                return;
            }

            if (i.bContainer)
            {
                OutMessage += "Due to game constraints, you aren't allowed to put a container inside another container.  Sorry!  " +
                              "Just imagine if you had something inside a carry bag, which was inside a " +
                              "box, which itself was inside a bigger box.  Pretty complicated, right?\n";
                return;
            }

            /* Superceded by restrictions code
            // If you're tied up you can barely do anything
            if (World._player.bTiedUp)
            {
                OutMessage += "You aren't putting anything into " + iTo.sName + " while tied up like this.\n";
                return;

            } */

            // Some items can't be dropped or taken, like e.g. scenery items
            if ((i.bTakeable == false) || (i.bDroppable == false))
            {
                OutMessage += "That isn't something you can pick up and drop.\n";
                return;
            }
                        
            // Taking it first... check
            if (i.hiOwner != World._player)
            {
                OutMessage += "(Taking it first...)\n";
                World._Take.DoAction(i, null, false, ref OutMessage, ref bSuccess);

                if (bSuccess == false)
                {
                    OutMessage += "You couldn't get it to put it into " + iTo.sName + ".\n";
                    return;
                }

            }

            // Removing it first... check
            if (i.bWorn)
            {
                OutMessage += "(Removing it first...)\n";
                World._Remove.DoAction(i, null, false, ref OutMessage, ref bSuccess);

                if (bSuccess == false)
                {
                    OutMessage += "You couldn't take it off first to put it into " + iTo.sName + ".\n";
                    return;
                }
            }

            // Okay, past all the general sanity checks!

            // Item-specific handling
            // you might put a specific message for putting X into Y (assign to message)
            // or you might have some item-specific logic to occur


            // Finally, perform the action.
            if (Suppress == false)
            {
                if (PutMessage == "")
                {
                    OutMessage += "You put " + i.sName + " into " + iTo.sName + ".\n";
                }
                else
                {
                    OutMessage += PutMessage;
                }
            }

            // Remove from player and put into target object.
            // If the target object's contents are not yet known, then make them discovered.
            // The oContainer should be removable - make sure nothing is using it for logic
            // i.oContainerObject = iTo;
            World._player.Remove(i);
            iTo.Add(i);
            iTo.bDiscoveredContents = true;
            
            bSuccess = true;
        }
    }

}
