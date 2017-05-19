using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

// 19/5/2017 - Articles Project - Making sure definite and indefinite articles are properly used.
//             Making changes in DoAction so that sDefiniteName gets used, for both dropped object
//             and the containing object (if applicable).

namespace Engine
{
    [DataContractAttribute(IsReference=true)]
    public class Drop_Action : Action
    {
        public Drop_Action()
        {
            sName = "Drop";
            sProtoCmdLine = "Drop [item1]";
            iNumArgs = 1;
        }

        public override void DoAction(Engine.Object i, Engine.Object item2, bool Suppress, ref string OutMessage, ref bool bSuccess)
        {
            // This code is has a bunch of messages being built up and can be confusing.

            // The basic idea is:
            //  * There are messages from sanity checks (e.g. "That isn't droppable") and these
            //    go into _message, and _proceed is set to false.
            //  * You might want to overwrite those with item specific messages, (e.g. "Don't
            //    be silly, dropping your head would kill you.") _proceed remains false.

            // If sanity checks failed, update OutMessage and leave.
            // After this point you don't really see _message anymore.

            // If we get past the sanity checks OK, then you have a drop message.
            //  * Most of the time this is just "you drop [item]".
            //  * Some items have a specific drop message.

            // Finally once we're past all that, drop the item.


            string _message = "";           // Sanity Check message
            bool _proceed = true;           // If sanity checks were successful

            string _DropMessage = "";       // "Drop the item" message
            Engine.Object ContainerObject;

            bSuccess = false;

            // Is the command complete?
            if (i == null)
            {
                OutMessage += "You haven't selected anything to drop.\n";
                return;
            }

            // Is the action restricted?
            if (World._player.bCanDrop == false)
            {
                OutMessage += World._player.sCantDropMsg.Replace("[item]", i.sDefiniteName) + "\n";
                return;
            }


            // Other sanity checks
            // _message is used mainly in the Sanity Checks.

            if (i.bDroppable == false)
            {
                _message = "That is not something you can drop.\n";
                _proceed = false;
            }

            if (World._player.HasItem(i) == false)
            {
                _message = "You don't have that item.\n";
                _proceed = false;
            }

            // Item specific sanity check messages, overwriting _message.
            // For example, rather than a dry "You can't drop Your Head" message, a
            // "It's far too firmly attached to your neck, besides, you like it where it is"
            // message would be more interesting and funnier.

            // Head
            if (i == World._head)
            {
                _message = "Don't be silly, you can't drop your head!\n";
                _proceed = false;
            }


            // Now we're past the sanity checks.
            // If an error has been detected, update OutMessage and leave.
            if (_proceed == false)
            {
                OutMessage += _message;
                return;
            }


            // Now time to process the drop.

            // Take it off if being worn
            if (i.bWorn)
            {
                OutMessage += "(Removing it first...)\n";
                World._Remove.DoAction(i, item2, false, ref OutMessage, ref bSuccess);

                if (bSuccess == false)
                {
                    OutMessage += "Because you couldn't remove " + i.sName + " you can't drop it.\n";
                    return;
                }
            }

            // Take it out of what it's in if it's in a container you have
            if (i.hiOwner.GetType() == typeof(Engine.Object))
            {
                // Need to cast the container to Engine.Object so we can use sDefiniteName
                ContainerObject = (Engine.Object)i.hiOwner;

                // OutMessage += "(Getting it out from " + i.hiOwner.sName + " first...)\n";
                // World._GetOutOf.DoAction(i, (Engine.Object)i.hiOwner, false, ref OutMessage, ref bSuccess);
                OutMessage += "(Getting it out from " + ContainerObject.sDefiniteName + " first...)\n";
                World._GetOutOf.DoAction(i, ContainerObject, false, ref OutMessage, ref bSuccess);

                if (bSuccess == false)
                {
                    // OutMessage += "Because you couldn't get " + i.sName + " out of " + i.hiOwner.sName + " you can't drop it.\n";
                    OutMessage += "Because you couldn't get " + ContainerObject.sDefiniteName + " out of " + i.hiOwner.sName + " you can't drop it.\n";
                    return;
                }
            }

            // Generic drop message
            if (_DropMessage == "")
            {
                _DropMessage = "You drop " + i.sDefiniteName + ".\n";
            }

            // Item specific drop messages (and overwriting generic drop message)
            // e.g. A computer or phone might get a custom _DropMessage, so that you're putting it gently down.


            // Finally drop the object (ie move it out of player's inventory and into current
            // location's inventory
            if (Suppress == false)
            {
                OutMessage += _DropMessage;
            }

            World._player.Remove(i);
            World._player.CurrentLocation.Add(i);
            bSuccess = true;
        }
    }

}
