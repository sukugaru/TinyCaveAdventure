using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CustomExtensions;
using System.Runtime.Serialization;
using System.Reflection;

// 18/6/2017 - Enhancement 8 - Added HasMoveType() method
//
// 7/6/2017 - Bug 6 - Renaming the Object class to Item.
// 
// 6/6/2017 - Enhancement 7 - Have methods to add and remove movement types
//
// 24/5/2017 - Bug 9 - TieUp() didn't actually set bTiedUp!

namespace Engine
{
    [DataContractAttribute(IsReference=true)]    
    public class Player : HasInventory
    {
        [DataMember()] public Location CurrentLocation { get; set; }
        [DataMember()] public Conversation CurrentConversation { get; set; }
        // If CurrentConversation is set then the UI will switch to conversation mode.
        // If null will be in regular mode.  (list of actions, objects, etc.)

        [DataMember()] public TextSequence CurrentTextSequence { get; set; }
        // Similarly, if CurrentTextSequence is set then the UI will switch to Text 
        // Sequence mode.

        [DataMember()] public string sState { get; set; }

        // Bodyparts and worn items don't count towards your carry limit.
        // if 0 then infinite.
        [DataMember()] public int iCarrySize { get; set; }
        [DataMember()] public string sCantCarryMoreMsg { get; set; }

        // Restriction system
        // This is for all the standard actions.
        // If an action cannot be performed, then sCantXMsg is given to the player.
        // If an action can be performed on an item anyway:
        //  * The most common action that ignores this system is usually Use (and Use With)
        //    So objects have a bCanUseAnyway attribute.
        //  * Other items that ignore this restriction system will need coding in the individual
        //    actions... unless a lot of items start ignoring the restrictions!  :)
        [DataMember()] public bool bCanMove { get; set; }
        [DataMember()] public string sCantMoveMsg { get; set; }

        // Figuring out a new movement system...  
        // If a pathway has "x" in a direction, then the player needs "x" in sMoveTypes to
        // go in that directory
        // if a pathway is "" then that's standard movement
        // if sMoveTypes is "" or "none" then the player can't move at all
        // Something like that.
        [DataMember()]
        public string sMoveTypes { get; set; }

        [DataMember()] public bool bCanTake { get; set; }
        [DataMember()] public string sCantTakeMsg { get; set; }

        [DataMember()] public bool bCanDrop { get; set; }
        [DataMember()] public string sCantDropMsg { get; set; }

        [DataMember()] public bool bCanPutIn { get; set; }
        [DataMember()] public string sCantPutInMsg { get; set; }

        [DataMember()] public bool bCanGetOut { get; set; }
        [DataMember()] public string sCantGetOutMsg { get; set; }

        [DataMember()] public bool bCanWear { get; set; }
        [DataMember()] public string sCantWearMsg { get; set; }

        [DataMember()] public bool bCanRemove { get; set; }
        [DataMember()] public string sCantRemoveMsg { get; set; }

        [DataMember()] public bool bCanTalk { get; set; }
        [DataMember()] public string sCantTalkMsg { get; set; }

        [DataMember()] public bool bCanUse { get; set; }
        [DataMember()] public string sCantUseMsg { get; set; }


        // Other attributes
        [DataMember()] public bool bTiedUp { get; set; }
        [DataMember()] public bool bTiedUpTraining { get; set; }        // Idea that was never used
        [DataMember()] public bool bReceivedSagesClue { get; set; }
        [DataMember()] public bool bUsedSagesClue { get; set; }
        [DataMember()] public bool bKnowAdditionalLocations { get; set; }
        [DataMember()] public bool bCanParkour { get; set; }
        [DataMember()] public int iParkourTrainingLevel { get; set; }
        [DataMember()] public int iSore { get; set; }
        [DataMember()] public bool bCanLeave { get; set; }
        [DataMember()] public bool bSacrificed { get; set; }
        [DataMember()] public bool bBeingSacrificed { get; set; }
        [DataMember()] public bool bHasLobsterClue { get; set; }
        [DataMember()] public bool bLiftedCurse { get; set; }


        public void DetermineActions(ref List<Engine.Action> ActionList)
        {
            // It's possible that you might have an action specific to a location
            // Or that you might be in a state that has a different set of actions
            // e.g. sState == "Happy" and actions are "Whee!", "Laugh", and "Jump"
            ActionList.Clear();

            if ( (sState == "") ||
                 (sState.StartsWith("On "))
                )
            {
                // Inventory and LookAtLocation not needed by graphical UI
                // ActionList.Add(World._Inventory);
                // ActionList.Add(World._LookAtLocation);

                // report hiOwner was a debug action
                // ActionList.Add(World._Report_hiOwner);

                ActionList.Add(World._LookAtItem);
                ActionList.Add(World._Wear);
                ActionList.Add(World._Remove);
                ActionList.Add(World._Take);
                ActionList.Add(World._Drop);
                ActionList.Add(World._Use);
                ActionList.Add(World._UseWith);
                ActionList.Add(World._PutInto);
                ActionList.Add(World._GetOutOf);
                ActionList.Add(World._TalkTo);
                ActionList.Add(World._Wait);
            }

            // Add any location-specific locations
            foreach (var a in CurrentLocation.LocationActions)
            {
                ActionList.Add(a);
            }

            // Other special actions

            // The jumping bit at the beginning
            if (CurrentLocation == World._stalagmiteCave)
            {
                if (sState == "")
                {
                    World._JumpUp.SetTarget(World._stalagmiteBase, "up");
                    ActionList.Add(World._JumpUp);
                }

                if (sState == "On Stalagmite base")
                {
                    World._JumpUp.SetTarget(World._boulder, "up");
                    World._JumpUp.oFrom = World._stalagmiteBase;
                    ActionList.Add(World._JumpUp);

                    World._JumpDown.SetTarget(null, "down");
                    World._JumpDown.oFrom = World._stalagmiteBase;
                    ActionList.Add(World._JumpDown);
                }

                if (sState == "On Boulder")
                {
                    World._JumpUp.SetTarget(World._ledge, "up");
                    World._JumpUp.oFrom = World._boulder;
                    ActionList.Add(World._JumpUp);

                    World._JumpDown.SetTarget(World._stalagmiteBase, "down");
                    World._JumpDown.oFrom = World._boulder;
                    ActionList.Add(World._JumpDown);
                }

                if (sState == "On Ledge")
                {
                    World._JumpUp.SetTarget(World._precariousPlatform, "up");
                    World._JumpUp.oFrom = World._ledge;
                    ActionList.Add(World._JumpUp);

                    World._JumpDown.SetTarget(World._boulder, "down");
                    World._JumpDown.oFrom = World._ledge;
                    ActionList.Add(World._JumpDown);
                }
                
                if (sState == "On Precarious Platform")
                {
                    World._JumpUp.SetTarget(null, "up");
                    World._JumpUp.oFrom = World._precariousPlatform;
                    ActionList.Add(World._JumpUp);

                    World._JumpDown.SetTarget(World._ledge, "down");
                    World._JumpDown.oFrom = World._precariousPlatform;
                    ActionList.Add(World._JumpDown);
                }
                
            }
            else if (CurrentLocation == World._highLedge)
            {
                if (sState == "")
                {
                    World._JumpDown.SetTarget(World._precariousPlatform, "down");
                    World._JumpDown.oFrom = null;
                    ActionList.Add(World._JumpDown);
                }
            }

        }


        public string getDiagnosis()
        {
            bool bWearingCostume = false;
            bool bWearingHeaddress = false;
            string s = "";
            Item i;

            if (iSore == 1)
            {
                s = "You are a completely healthy and normal cave adventurer.";
            }
            else
            {
                s = "You are a completely normal cave adventurer.";
            }
            
            if (bTiedUp)
            {
                s += "  You are tied up, with your hands bound quite tightly";
                if (bTiedUpTraining)
                {
                    s += ", but you've learned how to do things anyway";
                }
                s += ".";
            }
            bWearingCostume = ((HasItem(World._tribalCostume) && World._tribalCostume.bWorn));
            bWearingHeaddress = ((HasItem(World._tribalHeadgear) && World._tribalHeadgear.bWorn));

            if (bWearingCostume && bWearingHeaddress)
            {
                s += "  You are wearing a bizarre crockery-based tribal costume and headdress.";
            }
            if (bWearingCostume && (bWearingHeaddress == false))
            {
                s += "  You are wearing a bizarre crockery-based tribal costume.";
            }
            if ((bWearingCostume == false) && bWearingHeaddress)
            {
                s += "  You are wearing a bizarre crockery-based tribal headdress.";
            }

            // Soreness level
            if (iSore > 10)
            {
                s += "  You are feeling very sore.";
            }
            if ((iSore > 5) && (iSore <= 10))
            {
                s += "  You are feeling sore.";
            }
            if ((iSore > 2) && (iSore <= 5))
            {
                s += "  You're feeling a bit scraped, but not too bad.";
            }
            if (iSore == 2)
            {
                s += "  You're feeling as good as new.";
            }


            if (sState != "")
            {
                s += "\n(" + sState + ")";
            }
            return s;
        }

        public int carrying()
        {
            int count = 0;

            foreach (var i in Inventory)
            {
                // Carryable item that is not being worn
                if (((i.bDroppable) && (i.bTakeable)) &&
                     (i.bWorn == false)
                   )
                {
                    count++;
                }
            }

            return count;

        }

        public virtual void AddMoveType(string addType)
        // 18/6/2017 - Enhancement 8 - Doing it all in lowercase
        //
        // Add a movement type to a location's pathway in a specified direction.
        {
            string s = sMoveTypes.ToLower();
            addType = addType.ToLower();

            if (s.Contains(addType) == false)
            {
                s += "," + addType;
                s = s.Trim(',');
                s = s.Replace(",,", ",");
                sMoveTypes = s;
            }
        }

        public virtual void RemoveMoveType(string removeType)
        // 18/6/2017 - Enhancement 8 - Doing it all in lowercase
        //
        // Remove a movement type from a location's pathway in a specified direction.
        {
            string s = sMoveTypes.ToLower();
            removeType = removeType.ToLower();

            if (s.Contains(removeType))
            {
                s = s.Replace(removeType, "");
                s = s.Trim(',');
                s = s.Replace(",,", ",");
                sMoveTypes = s;
            }
        }

        public Boolean HasMoveType(string pMoveType)
        // See if the player has a movement type
        {
            string sInType = pMoveType.ToLower();
            string sPlayerMoveTypes = sMoveTypes.ToLower();

            if (sPlayerMoveTypes.IndexOf(sInType) == -1)
            {
                return false;
            }

            return true;

        }


        public void TieUp()
        // When tied up the player is somewhat restricted in what they can do
        // 24/5/2017 - Bug 9 - Didn't actually set bTiedUp!
        {
            bTiedUp = true;

            bCanMove = true;
            bCanTake = true;
            bCanDrop = true;
            bCanPutIn = true;
            bCanGetOut = true;
            bCanTalk = true;

            iCarrySize = 1;
            sCantCarryMoreMsg = "With your hands tied up, you can only carry one " +
                "thing at a time, and only very awkwardly.";

            bCanWear = false;
            bCanRemove = false;
            sCantWearMsg = "You can't put anything on or take anything off with your hands tied up.";
            sCantRemoveMsg = "You can't put anything on or take anything off with your hands tied up.";

            bCanUse = false;
            sCantUseMsg = "You can't use [item] with your hands tied up.";

            World._head.bUsableAnyway = true;
            World._vendingMachine.bUsableAnyway = true;
            World._MazeBook.bUsableAnyway = true;
            World._bottledWater.bUsableAnyway = true;
            World._WallMap.bUsableAnyway = true;
            World._PaperAndStationeryKit.bUsableAnyway = true;
            World._XXiumSaw.bUsableAnyway = true;
            World._map.bUsableAnyway = true;
            World._abstractDesigns.bUsableAnyway = true;
            World._sachet.bUsableAnyway = true;

        }

        public void FreeHands()
        // No more restrictions
        {
            bCanMove = true;
            bCanTake = true;
            bCanDrop = true;
            bCanPutIn = true;
            bCanGetOut = true;
            bCanTalk = true;
            bCanWear = true;
            bCanRemove = true;
            bCanUse = true;

            iCarrySize = 0;
            sMoveTypes += ",climb";

        }


    }

}
