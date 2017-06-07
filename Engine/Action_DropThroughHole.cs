using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

// 7/6/2017 - Bug 6 - Renaming the Object class to Item.
// 
// 19/5/2017 - Articles Project - Making sure definite and indefinite articles are properly used.
//             Making changes in DoAction so that sDefiniteName gets used.

namespace Engine
{
    [DataContractAttribute(IsReference=true)]
    public class DropThroughHole_Action : Action
    // This action is specific to the tribal cave
    {
        public DropThroughHole_Action()
        {
            sName = "Drop item through hole";
            sProtoCmdLine = "Drop [item1] through the cavern floor hole";
            iNumArgs = 1;
        }

        public override void DoAction(Item i, Item item2, bool Suppress, ref string OutMessage, ref bool bSuccess)
        {
            bSuccess = false;

            // Borrowing _Drop for this, as it already has a lot of sanity checks around
            // dropping something.  However, suppressing the success "dropped" message,
            // so we can use a "you drop it through the hole" message instead.

            World._Drop.DoAction(i, item2, true, ref OutMessage, ref bSuccess);
            
            if (bSuccess)
            {
                    World._tribalCavern.Remove(i);
                    World._stalagmiteCave.Add(i);
                    OutMessage += "You drop " + i.sDefiniteName + " through the hole, and it falls " +
                        "into the gloom below.\n";
                    bSuccess = true;
            }
            else
            {
                OutMessage += "You could not drop " + i.sDefiniteName + " through the hole.\n";
                return;
            }
        }
    }

}
