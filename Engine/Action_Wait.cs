using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public override void DoAction(Engine.Object i, Engine.Object item2, bool Suppress, ref string OutMessage, ref bool bSuccess)
        {
            OutMessage += "You wait a turn.\n";
            bSuccess = true;
        }

    }
}
