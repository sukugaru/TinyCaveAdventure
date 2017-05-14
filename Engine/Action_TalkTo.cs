using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{

    public class TalkTo_Action : Action
    // Could be refined by creating an NPC class
    {
        public TalkTo_Action()
        {
            sName = "Talk to";
            sProtoCmdLine = "Talk to [item1]";
            iNumArgs = 1;
        }

        public override void DoAction(Engine.Object item1, Engine.Object item2, bool Suppress, ref string OutMessage, ref bool bSuccess)
        {
            bSuccess = false;

            if (item1 == null)
            {
                OutMessage = "Please select someone to talk to.\n";
                return;
            }

            // Is the action restricted?
            if (World._player.bCanTalk == false)
            {
                OutMessage += World._player.sCantTalkMsg.Replace("[item]", item1.sDefiniteName) + "\n";
                return;
            }


            // Even if/when I create an NPC class, the following bit should probably stay the
            // same, maybe you could have NPCs you can't talk to.
            if (item1.bCanTalkTo == false)
            {
                OutMessage = "You cannot talk to " + item1.sName + ".\n";
                return;
            }

            // If/when I ever get around to creating an NPC class, the following bits
            // would just become item1.TalkTo(ref OutMessage)
            if (item1 == World._questGiver)
            {
                World._questGiver.TalkTo(ref OutMessage);
            }

            if (item1 == World._sage)
            {
                World._sage.TalkTo(ref OutMessage);
            }

            if (item1 == World._crazyGuy)
            {
                World._player.CurrentConversation = World._crazyGuyConversation;
            }

            if (item1 == World._villagers)
            {
                World._villagers.TalkTo(ref OutMessage);
            }

            if (item1 == World._leader)
            {
                World._leader.TalkTo(ref OutMessage);
            }
        }

    }

}
