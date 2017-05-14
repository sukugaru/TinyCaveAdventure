using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;


namespace Engine
{
    [DataContractAttribute(IsReference=true)]
    public class JumpTo_Action : Action
    // Used to jump from object to object in the stalagmite cave.
    {
        public Engine.Object oTarget { get; set; }
        public Engine.Object oFrom { get; set; }

        public JumpTo_Action()
        {
            sName = "Jump To X";
            sProtoCmdLine = "Jump to X";
            iNumArgs = 0;
        }

        public void SetTarget(Engine.Object poTarget, string sDirection)
        {
            if (poTarget != null)
            {
                oTarget = poTarget;
                sName = "Jump to " + poTarget.sName;
                sProtoCmdLine = "Jump " + sDirection + " to " + poTarget.sName;
            }
            else
            {
                oTarget = poTarget;
                sName = "Jump " + sDirection;
                sProtoCmdLine = "Jump " + sDirection;
            }
        }

        public override void DoAction(Engine.Object i, Engine.Object item2, bool Suppress, ref string OutMessage, ref bool bSuccess)
        {
            bSuccess = false;
            if (oTarget != null)
            {
                // Restriction check
                // Base this on the player's bCanMove
                if (World._player.bCanMove == false)
                {
                    OutMessage += World._player.sCantMoveMsg + "\n";

                }


                OutMessage += "You jump to the " + oTarget.sName;

                if ((oFrom != null) && 
                    (World._player.CurrentLocation == World._stalagmiteCave)
                    )
                {
                    OutMessage += " from the " + oFrom.sName;
                }

                // Jumping from High Ledge to the precarious platform
                if ((oFrom == null) &&
                    (World._player.CurrentLocation == World._highLedge)
                    )
                {
                    OutMessage += " from the high ledge";
                    World.MoveTo(World._stalagmiteCave, null, true, ref OutMessage, ref bSuccess);
                    if (bSuccess)
                    {
                        // World._player.sState = "";
                    }
                    else
                    {
                        OutMessage += ", but for some reason it doesn't work!  You have a " +
                            "new quest - you must inform the programmer at once!\n";
                    }
                }
                
                // Landing and making noise
                if (oTarget == World._ledge)
                {
                    OutMessage += ".  As you land, you crash into the side of the rockface " +
                        "and disturb a bunch of pebbles, which clatter to the ground far " +
                        "below.";
                }
                else if (oTarget == World._precariousPlatform)
                {
                    OutMessage += ".  As you land, the platform wobbles and grinds " +
                        "against its precarious perch, making an enormous clatter.";
                }
                else
                {
                    OutMessage += ".  As you land, you make quite some noise.";
                }

                // From the PP you can make it to the High Ledge
                if (oTarget == World._precariousPlatform)
                {
                    OutMessage += "  Above you, just within jumping reach, is the top of the " +
                        "rock face.";
                }
                OutMessage += "\n";

                // You always make noise, so if the Crazy Old Guy is patrolling, make him
                // hurry back.
                if (World._highLedge.bVisited == false)
                {
                    OutMessage += "\nYou hear a voice from above.  \"Hey!  What's all that " +
                        "noise?\" it shouts.\n";
                    World._crazyGuy.iDirection = -1;
                }

                // Finally, set the player's state.
                World._player.sState = "On " + oTarget.sName;
            }

            // End points
            if (oTarget == null)
            {
                // Jumping down 
                if (oFrom == World._stalagmiteBase)
                {
                    OutMessage += "You jump down to the rocky floor of the Stalagmite Cave.\n";
                    World._player.sState = "";
                }

                if (oFrom == World._precariousPlatform)
                {
                    World.MoveTo(World._highLedge, null, true, ref OutMessage, ref bSuccess);
                    if (bSuccess)
                    {
                        OutMessage += "You jump up, just making it to the top of the rockface.\n";
                        World._player.sState = "";
                    }
                    else
                    {
                        OutMessage += "You leap for the top of the rockface, and for some " +
                            "unknown reason you don't make it!  You have a new quest - you " +
                            "must inform the programmer at once!\n";
                    }


                }

            }

            bSuccess = true;
        }

    }
}
