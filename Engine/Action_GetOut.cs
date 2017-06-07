using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using CustomExtensions;

// 7/6/2017 - Bug 6 - Renaming the Object class to Item.
// 
// 19/5/2017 - Articles Project - Making sure definite and indefinite articles are properly used.
//             Making changes in DoAction so that sDefiniteName gets used.


namespace Engine
{
    [DataContractAttribute(IsReference = true)]
    public class GetOut_Action : Action
    {
        public GetOut_Action()
        {
            sName = "Get out of";
            sProtoCmdLine = "Get [item1] out of [item2]";
            iNumArgs = 2;
        }

        public override void DoAction(Item i, Item iFrom, bool Suppress, ref string OutMessage, ref bool bSuccess)
        {
            bSuccess = false;
            string s;

            // Sanity checks
            if ((i == null) || (iFrom == null))
            {
                OutMessage += "Please select something and what to get it out of.\n";
                return;
            }

            if (iFrom.bContainer == false)
            {
                OutMessage += iFrom.sDefiniteName.CapitaliseBeginning() + " is not something you can put things into or get things out of.\n";
                return;
            }

            // Is the action restricted?
            if (World._player.bCanGetOut == false)
            {
                s = World._player.sCantGetOutMsg.Replace("[item1]", i.sDefiniteName);
                s = s.Replace("[item2]", iFrom.sDefiniteName) + "\n";
                OutMessage += s;
                return;
            }


            if (iFrom.bLocked)
            {
                OutMessage += iFrom.sDefiniteName.CapitaliseBeginning() + " is locked.";
                return;
            }

            if (i.hiOwner != iFrom)
            {
                OutMessage += "That's not in " + iFrom.sDefiniteName + ".\n";
                return;
            }

            if (i.bTakeable == false)
            {
                OutMessage += "That isn't something you can pick up.\n";
                return;
            }

            // Item-specific code
            // Would something interfere with taking item1 out of itemFrom?
            // Or does something specific need to happen if item X is taken out of item Y?

            // When you take the sachet of lemon flavouring, update the description of the
            // basket.
            if ((i == World._sachet) && (iFrom == World._holyBasket))
            {
                World._holyBasket.sDescription = "There is a very old picnic basket here.  " +
                    "It is almost rotted through and fallen into pieces.  In its awful " +
                    "murky depths there is a split teabag and the shards of a broken mug.";
            }

            // Perform Get Out
            // Need to examine oContainerObject code and make sure I can get rid of it all
            // i.oContainerObject = null;
            iFrom.Remove(i);
            World._player.Add(i);

            bSuccess = true;
            if (Suppress == false)
            {
                OutMessage += "You get " + i.sDefiniteName + " out of " + iFrom.sDefiniteName + ".\n";
            }
        }

    }

}
