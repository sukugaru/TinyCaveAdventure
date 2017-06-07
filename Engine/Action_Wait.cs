using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// 7/6/2017 - Bug 6 - Renaming the Object class to Item.

namespace Engine
{
    public class Wait_Action : Action
    // Sometimes you just need to do nothing for a turn.
    {
        public Wait_Action()
        {
            sName = "Wait";
            sProtoCmdLine = "Wait a turn";
            iNumArgs = 0;
        }

        public override void DoAction(Item i, Item item2, bool Suppress, ref string OutMessage, ref bool bSuccess)
        {
            OutMessage += "You wait a turn.\n";
            bSuccess = true;
        }

    }
}
