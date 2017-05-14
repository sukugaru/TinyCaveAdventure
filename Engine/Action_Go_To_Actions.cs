using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    // Not used in Tiny Cave Adventure
    // Actions specifically for going to a location are handled in a menu-like interface when
    // using various maps.

    /*
    public class GoToX_Action : Action
    {
        public GoToX_Action()
        {
            sName = "Go To X";
            sProtoCmdLine = "Go to X";
            iNumArgs = 0;
        }

        public override void DoAction(Engine.Object i, Engine.Object item2, bool Suppress, ref string OutMessage, ref bool bSuccess)
        {
            bSuccess = false;
            World.MoveTo(World.X, true, ref OutMessage, ref bSuccess);
            if (bSuccess)
            {
                OutMessage += "Some sort of special message about how you're getting to X.\n";
            }
        }

    }
    */

    
}
