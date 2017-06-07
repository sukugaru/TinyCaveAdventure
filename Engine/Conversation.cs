using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using CustomExtensions;
using System.Reflection;

// 6/6/17 - Using the new AddMoveType to add "parkour" to the player in the Parkour Manual conversation

namespace Engine
{
    [DataContractAttribute(IsReference = true)]
    [KnownType("DerivedTypes")]
    public abstract class Conversation
    // Conversations can be coded pretty much however, just as long as they provide the
    // following three methods that the UI can use.
    // flow goes like this:
    //   0) AtBeginning() is called right at the beginning of the conversation.
    //   1) UI calls GetDialogue and displays the result
    //   2) UI calls DetermineResponses, displays them to the player, and waits for player
    //      to select one
    //   3) UI calls HandleResponse to handle the response.  This can return an OutMessage
    //      as well.
    // A conversation in this system doesn't have to be with an NPC - it can be used for any
    // part of the game where a menu of options are provided.  For example, using a computer,
    // or a vending machine.  "Buy Coke", "Buy Sprite", "Insert Coin", etc.
    //
    // Highly suggested to handle AtBeginning() in the way demonstrated below.
    //
    // You don't have to implement EndConversation() as the UI doesn't call it.  You'll need
    // to make sure bStarted is set back to false, and CurrentConversation to null in some
    // way though.
    //
    // ISSUE:  HandleResponse seems to get called twice by the UI, where it should be called once.
    //         if HandleResponse spits out an OutMessage but doesn't end the conversation, then
    //         you get that OutMessage multiple times.
    //         It means this is probably all too complicated and needs a rethink.
    {
        [DataMember()]
        protected bool bStarted;

        public Conversation()
        {
            bStarted = false;
        }
        public virtual bool AtBeginning()
        {
            if (bStarted == false)
            {
                return true;
            }
            return false;
        }

        public abstract void GetDialogue(ref string OutMessage);

        public abstract void DetermineResponses(ref List<string> ResponseList);

        public virtual void HandleResponse(string Response, ref string OutMessage)
        {
            bStarted = true;
        }

        public virtual void EndConversation()
        {
            World._player.CurrentConversation = null;
            bStarted = false;
        }

        private static Type[] DerivedTypes()
        {
            return typeof(Conversation).GetDerivedTypes(Assembly.GetExecutingAssembly()).ToArray();
        }

    }

    [DataContractAttribute(IsReference=true)]
    public class CrazyGuyConversation : Conversation
    {
        [DataMember()]
        public int iNode { get; set; }

        [DataMember()]
        public bool bInConversation { get; set; }

        [DataMember()]
        public int iTimesTalked { get; set; }

        [DataMember()]
        public bool bHiFlag { get; set; }

        public CrazyGuyConversation() : base()
        {
            iNode = 1;
            bInConversation = false;
            iTimesTalked = 0;
            bHiFlag = false;
        }

        public override void GetDialogue(ref string OutMessage)
        {
            if (iNode == 1)
            {
                OutMessage += "\"Hi!\" he says, his eyes only slightly vibrating.\n";
            }

            if (iNode == 2)
            {
                OutMessage += "\"Hi!\"\n";
            }

            if (iNode == 3)
            {
                OutMessage += "\"Oh, okay!\"\n";
                iNode = 1;
            }

            if (iNode == 4)
            {
                OutMessage += "\"Oh... you haven't killed yourself yet I see,\" he says.\n";

            }

            if (iNode == 5)
            {
                OutMessage += "\"So they are!  With XXium!  But you'll do less damage with " +
                    "them bound, and perhaps you won't kill yourself.  It'd be nice if someone " +
                    "didn't go off and kill themselves for once.\"\n\n" +
                    "\"What's XXium?\" you ask.\n\n" +
                    "\"It's the toughest material known in the Tiny Cave!";
                iNode = 8;
            }

            if (iNode == 6)
            {
                OutMessage += "\"Excellent, excellent... oh!  You have to remember!  Beware " +
                    "the lobster!!!";
                World._player.bHasLobsterClue = true;
                iNode = 8;
            }

            if (iNode == 7)
            {
                OutMessage += "\"It's a front!  She's a greedy sneaky conniving swearword!";
                iNode = 8;
            }

            if (iNode == 8)
            {
                OutMessage += "  So!  How's it hanging?\" he continues.  \"Hopefully not limply and dead!  Don't want you killing yourself!\"\n";
            }

        }

        public override void DetermineResponses(ref List<string> ResponseList)
        {
            ResponseList.Clear();
            if ((iNode == 1))
            {
                if (World._crazyGuy.bKnockedPlayerDown)
                {
                    ResponseList.Add("What the hell, man?  You were trying to kill me!");
                }
                ResponseList.Add("What do you mean, I'm trying to kill myself?");
                ResponseList.Add("Hi.");
            }

            if (iNode == 2)
            {
                ResponseList.Add("Hi.");
                if (bHiFlag)
                {
                    ResponseList.Add("Okay, that's enough.");
                }
                bHiFlag = true;
            }

            if ( (iNode == 4) || (iNode == 8))
            {
                if (World._player.bTiedUp)
                {
                    ResponseList.Add("Can you help with my hands?  They're tied up.");
                }
                ResponseList.Add("Of course not.  Not planning to.");
                if (World._sageGrotto.bVisited)
                {
                    ResponseList.Add("No, not going to.  What do you know about the sage?");
                }
                ResponseList.Add("Okay, I'm off.");
            }

            
        }

        public override void HandleResponse(string Response, ref string OutMessage)
        {
            bStarted = true;

            if (Response == "What the hell, man?  You were trying to kill me!")
            {
                OutMessage += "\"Oh, no, I'm trying to save you!  From yourself!  But it's " +
                    "clearly not working.  So follow me!\"\n";
                OutMessage += "\nHe squeezes through the crack in the wall.  Just before he " +
                    "vanishes from sight he turns and beckons to you to follow him.\n";
                World._crazyGuy.bFollowingThroughMaze = true;
                EndConversation();
            }

            if (Response == "What do you mean, I'm trying to kill myself?")
            {
                OutMessage += "\"Lots of people come, and nobody leaves.  The best way to " +
                    "stay alive is to not struggle!  Struggling is bad.  It's never worked " +
                    "and then you fall and land on the table.  But you're struggling anyway, " +
                    "though I'm trying to save you.  Which is clearly not working.  " +
                    "So follow me!\"\n";
                OutMessage += "\nHe squeezes through the crack in the wall.  Just before he " +
                    "vanishes from sight he turns and beckons to you to follow him.\n";

                World._crazyGuy.bFollowingThroughMaze = true;
                EndConversation();
            }

            if (Response == "Hi.")
            {
                iNode = 2;
            }

            if (Response == "Okay, that's enough.")
            {
                iNode = 3;
            }

            if (Response == "Can you help with my hands?  They're tied up.")
            {
                iNode = 5;
            }

            if (Response == "Of course not.  Not planning to.")
            {
                iNode = 6;
            }

            if (Response == "No, not going to.  What do you know about the sage?")
            {
                iNode = 7;
            }

            if (Response == "Okay, I'm off.")
            {
                OutMessage += "\"Okay!  Good to know you're still alive!\"\n";
                EndConversation();
            }

        }

        public override void EndConversation()
        {
            
            if ( (iNode >= 1) || (iNode <= 3) )
            {
                World._highLedge.Remove(World._crazyGuy);
                World._crazyGuy.sDefiniteName = "the crazy old guy";
                World._crazyGuy.sIndefiniteName = "the crazy old guy";
                iNode = 4;
            }
            World._player.CurrentConversation = null;
            bStarted = false;
        }


    }


    [DataContractAttribute(IsReference=true)]
    public class VendingMachineConversation : Conversation
    {
        public VendingMachineConversation()
            : base()
        {

        }

        public override void GetDialogue(ref string OutMessage)
        {
            OutMessage += "A sign on the machine says \"Welcome to TinyCave Vending(tm)!\"." +
                "  Inexplicably the vending machine is plugged into an electrical outlet and " +
                "is working.\n\n" +
                "Your options are:\n" +
                "1. XXium Saw (cost: Lemonade)\n" +
                "2. Paper and Stationery Kit (cost: 1 tin of tinned fish)\n" +
                "3. Parkour Manual (cost: 1 chunk of stalagmite + 1 chunk of stalactite)\n" +
                "4. Fizzy Drink (cost: 1 chunk of stalagmite)\n";
        }

        public override void DetermineResponses(ref List<string> ResponseList)
        {
            ResponseList.Clear();
            if ( (World._player.HasItem(World._lemonWater)) )
            {
                ResponseList.Add("XXium Saw");
            }
            if (World._player.HasItem(World._tinnedFish))
            {
                ResponseList.Add("Paper and Stationery Kit");
            }
            if ( (World._player.HasItem(World._chunkOfStalactite)) &&
                 (World._player.HasItem(World._chunkOfStalagmite))
                )
            {
                ResponseList.Add("Parkour Manual");
            }
            if (World._player.HasItem(World._chunkOfStalagmite))
            {
                ResponseList.Add("Fizzy Drink");
            }

            ResponseList.Add("Leave");
        }

        public override void HandleResponse(string Response, ref string OutMessage)
        {
            bStarted = true;
            
            if (Response == "Parkour Manual")
            {
                World._player.Remove(World._chunkOfStalagmite);
                World._player.Remove(World._chunkOfStalactite);
                World._player.Add(World._parkourManual);
                World._chunkOfStalactite.hiOwner = null;
                World._chunkOfStalagmite.hiOwner = null;
                OutMessage += "You deposit the chunks of stalagmite and stalactite into the " +
                    "payment slot and press the button for the Parkour Manual.  " +
                    "With a whirring and a clunk, the manual is dispensed.\n";
                EndConversation();


            } 
            if (Response == "XXium Saw")
            {
                World._player.Remove(World._lemonWater);
                World._player.Add(World._XXiumSaw);
                World._lemonWater.hiOwner = null;
                OutMessage += "You put the bottle of lemony water into the payment slot and " +
                    "press the button for the XXium Saw.  There is a whirring as the machine " +
                    "seems to think about it for a moment, then with a DING it accepts the " +
                    "bottle of lemony water.  With a SHING of audible sharpness and a clunk, " +
                    "the XXium Saw is dispensed.\n";
                EndConversation();
            }
            
            if (Response == "Paper and Stationery Kit")
            {
                World._player.Remove(World._tinnedFish);
                World._tinnedFish.hiOwner = null;
                World._player.Add(World._PaperAndStationeryKit);
                OutMessage += "With some difficulty, you manage to deposit the tin of fish " +
                    "into the payment slot and press the button for the Paper and " +
                    "Stationery Kit.  With a whirring and a clunk, the staionery kit is " +
                    "dispensed.\n";
                EndConversation();
            }

            if (Response == "Fizzy Drink")
            {
                World._player.Remove(World._chunkOfStalagmite);
                World._chunkOfStalagmite.hiOwner = null;
                World._player.Add(World._fizzyDrink);
                OutMessage += "You deposit the chunk of stalagmite into the payment slot, " +
                    "and press the button for the Fizzy Drink.  With a whirring and a clunk, " +
                    "the fizzy drink is dispensed.\n";
                EndConversation();
            }

            if (Response == "Leave")
            {
                OutMessage += "You leave the vending machine.\n";
                EndConversation();
            }
        }

        public override void EndConversation()
        {
            World._player.CurrentConversation = null;
            bStarted = false;
        }

    }

    [DataContractAttribute(IsReference=true)]
    public class WallMapConversation : Conversation
    // Used for the wall map (only has maze entrance and high ledge)
    // and the player-made map (can get added to)
    {
        [DataMember()]
        public string sMode { get; set; }           // if "ownmap" is the handcrafted portable map

        public WallMapConversation()
            : base()
        {

        }

        public override void GetDialogue(ref string OutMessage)
        {
            OutMessage += "Where do you want to go?\n";
        }

        public override void DetermineResponses(ref List<string> ResponseList)
        {
            ResponseList.Clear();
            ResponseList.Add("Go to Maze Entrance");
            ResponseList.Add("Go to High Ledge");
            if ((sMode == "OwnMap") && (World._player.bKnowAdditionalLocations) )
            {
                ResponseList.Add("Holy Basket");
                ResponseList.Add("Abandoned Shrine");

            }
            ResponseList.Add("Stay here");
        }

        public override void HandleResponse(string Response, ref string OutMessage)
        {
            bStarted = true;
            bool bSuccess = false;

            if (Response == "Go to Maze Entrance")
            {
                // OutMessage += "You walk through the twisty little tunnels and end up " +
                //    "back at the Maze Entrance.\n";
                // The _mazeEntrance.PreMove already does something like that.
                World.MoveTo(World._mazeEntrance, null, true, ref OutMessage, ref bSuccess);
                EndConversation();
            }

            if (Response == "Go to High Ledge")
            {
                OutMessage += "You walk through the twisty little tunnels and they get " +
                    "*very* narrow and you have to really squeeze through the last bits, " +
                    "and you end up back at the High Ledge.\n";
                World.MoveTo(World._highLedge, null, true, ref OutMessage, ref bSuccess);
                EndConversation();
            }

            if (Response == "Holy Basket")
            {
                OutMessage += "You walk through the twisty little tunnels until you get " +
                    "to the \"Holy Basket\" site.\n";
                World.MoveTo(World._holyBasketSite, null, true, ref OutMessage, ref bSuccess);
                EndConversation();
            }

            if (Response == "Abandoned Shrine")
            {
                OutMessage += "You walk through the twisty little tunnels until you get " +
                    "to the abandoned shrine.\n";
                World.MoveTo(World._abandonedShrineSite, null, true, ref OutMessage, ref bSuccess);
                EndConversation();
            }


            if (Response == "Stay here")
            {
                OutMessage += "You decide to stay stuck and wandering around mostly " +
                    "directionlessly in this very confusing maze.\n";
                EndConversation();
            }
        }

        public override void EndConversation()
        {
            World._player.CurrentConversation = null;
            bStarted = false;
        }

    }

    [DataContractAttribute(IsReference=true)]
    public class ParkourManualConversation : Conversation
    {
        public ParkourManualConversation() : base()
        {

        }

        public override void GetDialogue(ref string OutMessage)
        {
            if (World._player.iParkourTrainingLevel == 0)
            {
                OutMessage += "How to become an amazing Le Parkour athlete, a comprehensive " +
                    "training manual.\n\n" +
                    "We could either do this the easy gamey way, where by using an in-game " +
                    "item you compress months and months of physical training into a one-" +
                    "second button click, or the hard way by making you click on that item " +
                    "300 times, followed by an extensive 'cut-scene' showing your character's " +
                    "slow progress to mastery.\n";
            }

            if ( (World._player.iParkourTrainingLevel > 0) &&
                 (World._player.iParkourTrainingLevel < 300)
                )
            {
                OutMessage += "You train and train and train and your training level is " +
                    "now at " + World._player.iParkourTrainingLevel + ", of a potential " +
                    "300.  Keep at it!\n";
            }

            if (World._player.iParkourTrainingLevel == 300)
            {
                OutMessage += "Congratulations, you made it!  You have become an amazing " +
                    "Le Parkour athlete!  You just aren't a proper adventuer of caves and " +
                    "such if you can't Le Parkour all over everything.\n";
            }
        }

        public override void DetermineResponses(ref List<string> ResponseList)
        {
            ResponseList.Clear();
            if (World._player.iParkourTrainingLevel < 300)
            {
                ResponseList.Add("I'm feeling hardcore!  I'm going to click on this option 300 times!");
                if (World._player.iParkourTrainingLevel > 10)
                {
                    ResponseList.Add("Oh my god all this clicking is so boring.");
                }
                if (World._player.iParkourTrainingLevel == 0)
                {
                    ResponseList.Add("No thanks, I'd like to click on this option just once.");
                }
                ResponseList.Add("I'd like to take a break.");
            }
            else
            {
                ResponseList.Add("Leave.");
            }
        }

        public override void HandleResponse(string Response, ref string OutMessage)
        {
            if (bStarted == false)
            {
                bStarted = true;
            }

            if (Response == "I'm feeling hardcore!  I'm going to click on this option 300 times!")
            {
                World._player.iParkourTrainingLevel++;

                //if (World._player.iParkourTrainingLevel < 300)
                //{
//                    OutMessage += "You train and train and train and your training level is " +
//                        "now at " + World._player.iParkourTrainingLevel + ", of a potential " +
//                        "300.  Keep at it!\n";
                //}
                if (World._player.iParkourTrainingLevel == 300)
                {
                    OutMessage += "Congratulations and oh my god.  You reached maximum " +
                        "training level by clicking on this option 300 times!\n";
                    World._player.bCanParkour = true;
//                    World._player.sMoveTypes += ",parkour";
                    World._player.AddMoveType("parkour");
                    World._centralCavern.SouthLoc = World._treasureCave;
                    World._treasureCave.NorthLoc = World._centralCavern;
                    EndConversation();

                }
            }

            if ((Response == "No thanks, I'd like to click on this option just once.") ||
                (Response == "Oh my god all this clicking is so boring.")
                )
            {
                OutMessage += "Okay, let's skip past the training sequence and put your " +
                    "training level at 300 straight away.\n";
                World._player.iParkourTrainingLevel = 300;
                World._player.bCanParkour = true;
                World._player.sMoveTypes += ",parkour";
                World._centralCavern.SouthLoc = World._treasureCave;
                World._treasureCave.NorthLoc = World._centralCavern;

                EndConversation();
            }

            if ( (Response == "I'd like to take a break.") ||
                 (Response == "Leave.")
                )
            {
                OutMessage += "Sure thing.\n";
                EndConversation();
            }
        }

        public override void EndConversation()
        {
            World._player.CurrentConversation = null;
            bStarted = false;
        }

    }

    [DataContractAttribute(IsReference = true)]
    public class TreasureConversation : Conversation
    {
        [DataMember()]
        public bool bAllThisStuff { get; set; }

        [DataMember()]
        public bool bSomethingSmall { get; set; }
        
        [DataMember()]
        public bool bMassiveHoard { get; set; }

        [DataMember()]
        public bool bSomethingGoingOn { get; set; }

        [DataMember()]
        public bool bSomethingStrange { get; set; }

        public TreasureConversation() : base()
        {
            bAllThisStuff = false;
            bSomethingSmall = false;
            bMassiveHoard = false;
            bSomethingGoingOn = false;
            bSomethingStrange = false;

        }
        public override void GetDialogue(ref string OutMessage)
        {
            if (World._tinyLedge.bNothingnessLeap)
            {
                OutMessage += "\"How curious...\" says a voice in your head.  \"The old " +
                    "adventuress has been trying for decades to achieve what you just did in " +
                    "less than a day.\n\n" +
                    "\"You will need to depart this place, but only to return what once was " +
                    "taken.  You have leave to do so.  Beware, few have reached this point, and " +
                    "none have succeeded.  Once, one almost made it, but he returned in failure, " +
                    "full of ravings, his mind gone.\"\n";
                World._player.bCanLeave = true;
                World._caveEntrance.NorthwestLoc = World._ImpassableMountains;
            }
            else
            if (bSomethingStrange == false)
            {
                OutMessage += "You feel you shouldn't take anything; besides, you can't carry all " +
                    "that much stuff when you're parkouring around.\n";
            }
            else
            {
                OutMessage += "You feel you shouldn't take anything from the hoard; in " +
                    "particular, there is a vividly green gemstone that looks stunning and " +
                    "just fine where it is.\n";
            }

        }

        public override void DetermineResponses(ref List<string> ResponseList)
        {
            ResponseList.Clear();
            if (World._tinyLedge.bNothingnessLeap)
            {
                ResponseList.Add("I shall.");
            }
            else
            {
                ResponseList.Add("But I've been parkouring around with all this stuff already.");
                ResponseList.Add("How about I just take something small?");
                if (bAllThisStuff && bSomethingSmall)
                {
                    ResponseList.Add("It's a massive treasure hoard, in a cave, surely there's something I'm allowed to take.");
                }
                if (bMassiveHoard)
                {
                    ResponseList.Add("Hey, something's going on.");
                }

                if (bSomethingGoingOn)
                {
                    ResponseList.Add("Is there anything here that seems particularly strange?");
                }

                ResponseList.Add("Leave.");
            }
            
        }

        public override void HandleResponse(string Response, ref string OutMessage)
        {
            if (bStarted == false)
            {
                bStarted = true;
            }

            if (Response == "But I've been parkouring around with all this stuff already.")
            {
                bAllThisStuff = true;
            }

            if (Response == "How about I just take something small?")
            {
                bSomethingSmall = true;
            }

            if (Response == "It's a massive treasure hoard, in a cave, surely there's something I'm allowed to take.")
            {
                bMassiveHoard = true;
            }


            if (Response == "Hey, something's going on.")
            {
                bSomethingGoingOn = true;
            }

            if (Response == "Leave.")
            {
                OutMessage += "Sure thing.\n";
                EndConversation();
            }

            if (Response == "I shall.")
            {
                OutMessage += "\"Go.  Return what was taken.\"\n";
                EndConversation();
            }

            if (Response == "Is there anything here that seems particularly strange?")
            {
                bSomethingStrange = true;
            }

        }

        public override void EndConversation()
        {
            World._player.CurrentConversation = null;
            bStarted = false;
        }


    }



    [DataContractAttribute(IsReference=true)]
    public class QuestGiverFinalConversation : Conversation
    {
        [DataMember()]
        public int iNode { get; set; }

        public QuestGiverFinalConversation() : base()
        {
            iNode = 1;
        }

        public override void GetDialogue(ref string OutMessage)
        {
            if (iNode == 1)
            {
                OutMessage += "\"OHMYGODOHMYGODOHMYGOD!\" she cries.  \"You did it!  You got " +
                    "Mum's recipe back!  Thankyouthankyouthankyou!  I knew you could do it!\"\n";
            }

            if (iNode == 2)
            {
                OutMessage += "Her expression sours.  \"The necklace thing again?  Oh, come on, " +
                    "it was just one little thing in a massive hoard!  Surely I'm allowed to " +
                    "take one little thing from massive treasure hoards in caves?\"\n";
            }

            if (iNode == 3)
            {
                OutMessage += "\"A curse?\"\n\n" +
                    "\"No-one in the cave can get out.\"\n\n" +
                    "\"Oh!  That explains why I could never get back in!  But still, you got " +
                    "out and brought my recipe back so all's well!\"\n\n" +
                    "\"The cave curse is kinda still going, and it wants your necklace " +
                    "back,\" you hopefully say.\n\n" +
                    "\"Eh, whatever.  Heeeey!  I know!  I just recently got this delivery of " +
                    "some amazing-tasting seafood.  Here, have some of that, if you like!\"  " +
                    "She hopefully holds something that smells vaguely seafoody towards you, " +
                    "wrapped in newspaper.\n";
            }
        }

        public override void DetermineResponses(ref List<string> ResponseList)
        {
            ResponseList.Clear();
            if (iNode == 1)
            {
                ResponseList.Add("So, uh, where did you get the necklace from?");
                ResponseList.Add("The cave kinda wants your necklace back.");
                ResponseList.Add("I'm selling these fine gold + green gem necklaces.");
            }

            if (iNode == 2)
            {
                ResponseList.Add("You, uh, kinda created a curse on the cave by doing that.");
            }

            if (iNode == 3)
            {
                if (World._player.bHasLobsterClue)
                {
                    ResponseList.Add("REMEMBER!  BEWARE THE LOBSTER!!!");
                }
                ResponseList.Add("But...");
            }

        }

        public override void HandleResponse(string Response, ref string OutMessage)
        {
            bool bSuccess = true;

            if (bStarted == false)
            {
                bStarted = true;
            }

            if (iNode == 1)
            {
                iNode = 2;
            }
            else if (iNode == 2)
            {
                iNode = 3;
            }

            if (Response == "REMEMBER!  BEWARE THE LOBSTER!!!")
            {
                OutMessage += "\"No thanks!\" you say, rejecting her offer.\n\n" +
                    "\"What?  NO!  NO NO NO!  It'll come after me now!\"\n\n" +
                    "\"What'll come after you?\"\n\n" +
                    "A Giant Enemy Lobster appears.  The House Owner has a moment " +
                    "to scream, then the lobster hits her in a " +
                    "weak spot for massive damage.  There is a massive amount of magical " +
                    "blowback and an enormous explosion.\n\n" +
                    "You regain consciousness up in the branches of a tree.\n";
                World.MoveTo(World._UpATree, null, true, ref OutMessage, ref bSuccess);
                World._BackDoor.Add(World._lobster);
                World._crazyGuy.hiOwner.Remove(World._crazyGuy);
                World._crazyGuy.hiOwner = null;

                World.MakeRuins();
                EndConversation();
            }

            if (Response == "But...")
            {
                OutMessage += "You hesitate just a little too long.\n\n" +
                    "\"It's coming for you now,\" the House Owner says solemnly.\n\n" +
                    "\"What's coming after me?\"\n\n" +
                    "A Giant Enemy Lobster appears and tries to hit you in a weak spot for " +
                    "massive damage.  Using your newly-acquired parkour skills, you run like " +
                    "the wind, bouncing from tree to tree, until finally you come to a stop " +
                    "inside the Cave Entrance.  Outside, the Giant Enemy Lobster is trying " +
                    "reach you and hit you for massive damage, but is a bit too big.\n\n" +
                    "A voice sounds in your head.  \"You came so close... yet you also " +
                    "failed.  You shall not be allowed to leave.\"\n";
                World.MoveTo(World._caveEntrance, null, true, ref OutMessage, ref bSuccess);
                World._caveEntrance.NorthwestLoc = World._blocked;
                World._caveEntrance.Add(World._lobster);
                World._crazyGuy.hiOwner.Remove(World._crazyGuy);
                World._crazyGuy.hiOwner = null;
                EndConversation();

            }
        }

        public override void EndConversation()
        {
            World._player.CurrentConversation = null;
            bStarted = false;
        }

    }
}
