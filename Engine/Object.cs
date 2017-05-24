using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CustomExtensions;
using System.Runtime.Serialization;
using System.Reflection;

// 24/5/217 - Enhancements 1+5 : Adding Size.  Adding sizes to all objects.

namespace Engine
{
    // Size enum
    public enum Size { Tiny, Small, Medium, Large, NA };
    // Tiny, Small, Medium - all carriable, and to do with containers having sizes
    // Large - cannot be carried
    // NA - Size is n/a as this is not a carriable item (e.g. scenery, NPCS, etc.)
    // Might be some overlap with Large and NA!

    [DataContractAttribute(IsReference=true)]
    public class Object : HasInventory
    {
        [DataMember()] public string sDescription { get; set; }
        [DataMember()] public string sDefiniteName { get; set; }         // "The X" or "Your X"
        [DataMember()] public string sIndefiniteName { get; set; }       // "A/An X" or "Your X"


        [DataMember()] public bool bTakeable { get; set; }
        [DataMember()] public bool bDroppable { get; set; }
        [DataMember()] public bool bWearable { get; set; }
        [DataMember()] public bool bWorn { get; set; }
        [DataMember()] public bool bContainer { get; set; }
        // public string bInContainerText { get; set; }    // Something that was never used.
        // public Object oContainerObject { get; set;  }   // Old bit of code, should be removable.
        [DataMember()] public HasInventory hiOwner { get; set; }
        [DataMember()] public bool bDiscoveredContents { get; set; }
        [DataMember()] public bool bLocked { get; set; }               // Not blocked, but boolean-is it locked?
        [DataMember()] public bool bLockable { get; set; }             // Again, this is boolean-is it lockable?
        [DataMember()] public bool bUsableWhileTiedUp { get; set; }

        [DataMember()] public bool bUsableAnyway { get; set; }         // For restriction system v2
                                                                       // To replace bUsableWhileTiedUp
        [DataMember()] public bool bOpenable { get; set; }             // Might implement open/close actions
        [DataMember()] public bool bOpen { get; set; }

        [DataMember()] public bool bCanTalkTo { get; set; }
        [DataMember()] public bool bNPC { get; set; }
        [DataMember()] public bool bBodypart { get; set; }
        [DataMember()] public string sClothingType { get; set; }
        [DataMember()] public bool bStaysInMaze { get; set; }

        public Size sSize { get; set; }                                // Size of object.
        public Size sContainerSize { get; set; }                       // If object is a container, then this is the
                                                                       // largest item size that can fit in container.
        public int iContainerCapacity { get; set; }                    // if 0 then container has infinite space.


        public Object()
        // Default constructor sets everything to blank and to false
        {
            sDescription = "";
            sIndefiniteName = "";
            sDefiniteName = "";
            bTakeable = false;
            bDroppable = false;
            bWearable = false;
            bWorn = false;
            bContainer = false;
            // bInContainerText = "";
            // oContainerObject = null;
            hiOwner = null;
            bDiscoveredContents = false;
            bLocked = false;
            bLockable = false;
            bUsableWhileTiedUp = false;
            bUsableAnyway = false;
            bCanTalkTo = false;
            bNPC = false;
            bBodypart = false;
            sClothingType = "";
            bStaysInMaze = false;
            bUsableAnyway = false;
            sSize = Size.NA;
        }

        public virtual void Use(ref string OutMessage, ref bool bSuccess)
        { }

        public override string ToString()
        {
            // Has a few bits and pieces here, to show if an object is worn, inside another
            // object, or is containing another object.
            string s = "";

//            if ((hiOwner != null) && (hiOwner.GetType() == typeof(Engine.Object)))
            if ((hiOwner != null) && (hiOwner is Engine.Object))

            {
                s = "   ";
            }

            s = s + sIndefiniteName.CapitaliseBeginning();

            if ((bWearable) && (bWorn))
            {
                s = s + " (worn)";
            }

            if (bLocked)
            {
                s = s + " (locked)";
            }

            if (bLockable && (bLocked == false))
            {
                s = s + " (unlocked)";
            }

            if ((bContainer) && (Inventory.Count > 0) && bDiscoveredContents && (bLocked == false))
            {
                s = s + ", which contains:";
            }
            return s;
        }


    }

    [DataContractAttribute(IsReference=true)]
    public class RecipeObject : Object
    {
        public RecipeObject() : base()
        {
            sName = "Recipe";
            sDescription = "This is a recipe for the chocolatiest cake you've ever heard of, " +
                "using ingredients you didn't think existed anymore.  Perhaps they don't.  This " +
                "recipe is written in slightly archaic language, on very very old and " +
                "very very yellowed paper.  It feels incredibly fragile, as if it should crumble " +
                "at your touch, yet it resolutely stays intact.\n\n" +
                "On the back of the recipe is a diary entry, written by Dotty, age 8, talking " +
                "about her wonderful her mother was and how she'll miss her and here, she'll " +
                "write down her favourite " +
                "of all her mother's fantastic and wonderful recipes, before she forgets it.";
            sIndefiniteName = "a recipe";
            sDefiniteName = "the recipe";
            bTakeable = true;
            bDroppable = true;
            sSize = Size.Small;
        }
    }

    [DataContractAttribute(IsReference = true)]
    public class DaisObject : Object
    {
        public DaisObject() : base()
        {
            sName = "Dais";
            sDescription = "This is a raised platform, consisting of a large, flat, smoothed " +
                "stone, resting on top of a bed of well crafted pubbles, themselves in a " +
                "hollowed-out basin.  The platform is angled down slightly, to better show off " + 
                "anything on top of it.  The whole thing hass obviously been made with a lot " +
                "of care.";
            sIndefiniteName = "a dais";
            sDefiniteName = "the dais";
            bContainer = true;
            bDiscoveredContents = true;
            Add(World._recipe);
            sSize = Size.NA;
            iContainerCapacity = 1;
            sContainerSize = Size.Medium;
        }

        public override void Use(ref string OutMessage, ref bool bSuccess)
        {
            OutMessage += "No matter how you move the platform about, it rolls back to the same " +
                "position, angled slightly to show off anything on top of it.\n\n" +
                "\"Nifty innit?\" asks a nearby villager.  \"Spent *ages* making sure it did " +
                "that.\"\n\n" +
                "\"Well done,\" you tell him, and he beams a look of incredible pride.\n";
            bSuccess = true;
        }
    }

    [DataContractAttribute(IsReference = true)]
    public class BedsObject : Object
    {
        public BedsObject() : base()
        {
            sName = "Beds";
            sDescription = "Well, not exactly beds, but bed-like.  These 'beds' are threadbare " +
                "old fur pelts, looking very worn, arrayed around the various firepits.";
            sDefiniteName = "the beds";
            sIndefiniteName = "beds";
            sSize = Size.NA;
        }

        public override void Use(ref string OutMessage, ref bool bSuccess)
        {
            OutMessage += "\"How can you want to sleep?\" asks a nearby villager.  \"There's " +
                "a party on!\"\n";
            bSuccess = true;
        }

    }

    [DataContractAttribute(IsReference = true)]
    public class FirePitsObject : Object
    {
        public FirePitsObject() : base()
        {
            sName = "Fire pits";
            sDescription = "Various fire pits, spread around the edge of the central hole in " +
                "this cavern.  As the tribe is in full partying mode, the fires are all bright " +
                "and hot.";
            sIndefiniteName = "fire pits";
            sDefiniteName = "the fire pits";
            sSize = Size.NA;
        }

        public override void Use(ref string OutMessage, ref bool bSuccess)
        {
            OutMessage += "You'd just burn yourself.  Not a good look, especially for the god " +
                "Wunkahpshoo-gar.\n";
            bSuccess = true;
        }

    }

    [DataContractAttribute(IsReference = true)]
    public class FoodStoresObject : Object
    {
        public FoodStoresObject() : base()
        {
            sName = "Food stores";
            sDescription = "Over in one corner of the cave, where frigid air seems to seep up " +
                "through the rocks, is the tribe's food stores.  There's not a lot.  Some " +
                "animals, recently caught.  Some fungus and mushrooms.  And a whole heap of " +
                "tinned fish and bottled water.\n";
            sIndefiniteName = "food stores";
            sDefiniteName = "the food stores";
            sSize = Size.NA;
        }

        public override void Use(ref string OutMessage, ref bool bSuccess)
        {
            OutMessage += "You'd really better leave the foodstores alone.  They're already " +
                "down.  To take more of the village's food away would just be cruel.\n";
            bSuccess = true;
        }

    }

    [DataContractAttribute(IsReference = true)]
    public class LeaderObject : Object
    {
        public LeaderObject() : base()
        {
            sName = "Leader";
            sDescription = "The leader is a very large man, wearing what looks like the best " +
                "'crockery armor' of everyone except yourself, the least worst furs, and is " +
                "wearing a crown made of bent spoons.";
            sIndefiniteName = "a leader";
            sDefiniteName = "the leader";
            bNPC = true;
            bCanTalkTo = true;
        }

        public void TalkTo(ref string OutMessage)
        {
            OutMessage += "\"Ah, Wunkahpshoo-gar, so good of you to finally come!\" says the " +
                "leader.  \"You must know, we follow the ways as best we can, but the curse " +
                "you placed on us makes it harder every year, every decade, and every century!  " +
                "The game in the maze tunnels grows less every year, the water harder to find, " +
                "the machine of food more intractable, and yet we still aren't allowed to " +
                "leave the cave!  Please don't tell me to sacrifice the " +
                "sage or the crazy hermit, they're both pretty helpful.\"\n\n";
            
            if (World._caveEntrance.bTriedExiting == false)
            {
                OutMessage += "Curse?  What curse?  You'll have to investigate.  " +
                    "\"I'll get back to you on that,\" you say.  \"And, uh, hold off on " + 
                    "the sacrifices.\"\n\n";
            }
            else
            {
                if (World._caveEntrance.NorthwestLoc == World._blocked)
                {
                    OutMessage += "You have no idea how to even break the curse.  " +
                        "\"I'll have to get back to you on that,\" you say.  \"And, uh, " +
                        "hold off on the sacrifices.\"\n\n";
                }
                else
                {
                    OutMessage += "You have a pretty good idea of what you need to do.  " +
                        "\"I'm getting close.  I'll get back to you soon,\" you say.  " +
                        "\"And, uh, hold off on the sacrifices.\"\n\n";
                }

            }

            OutMessage += "\"Really?  But they've always worked before.  But okay, we'll " +
                "wait.  For now.\"\n";
        }
    }

    [DataContractAttribute(IsReference = true)]
    public class Villagers : Object
    {
        public Villagers() : base()
        {
            sName = "Villagers";
            sDescription = "The villagers are in full-on partying mode, now that their god " +
                "Wunkahpshoo-gar has taken mortal form and is walking amongst them.";
            sIndefiniteName = "villagers";
            sDefiniteName = "the villagers";

            bCanTalkTo = true;
            bNPC = true;
        }

        public void TalkTo(ref string OutMessage)
        {
            Random random = new Random();
            int j;

            j = random.Next(1, 5);
            switch (j)
            {
                case 1:
                    OutMessage += "\"Hey there Wunkahpshoo-gar!  Thanks for visiting us at " +
                        "last!\"\n";
                    break;

                case 2:
                    OutMessage += "\"Wunkhapshoo-gar, when shall you deliver us from your " +
                        "curse?  When will we have done enough penance in this cave, to be " +
                        "allowed to go free?\"\n";
                    break;

                case 3:
                    OutMessage += "\"I drunk I'm think!\"\n";
                    break;

                case 4:
                    OutMessage += "\"Hi!  Welcome to Tuhercaavervillatuherrcipea!\"\n";
                    break;

                case 5:
                    OutMessage += "\"I'm selling these fine leather jackets.\"\n";
                    break;  

                default:
                    break;
            }
        }

    }

    [DataContractAttribute(IsReference = true)]
    public class Lobster : Object
    {
        public Lobster() : base()
        {
            sName = "Giant Enemy Lobster";
            sDescription = "It's a giant enemy lobster, lurking just outside the cave's " +
                "entrance.  Even if you were allowed to leave, which you aren't, you can't get " +
                "too close to the lobster anyway, for it would hit you in a weak spot for " +
                "massive damage.";
            sIndefiniteName = "a giant enemy lobster";
            sDefiniteName = "the giant enemy lobster";
        }
    }

    [DataContractAttribute(IsReference = true)]
    public class CrazyOldGuy : Object
    {
        [DataMember()] public int iLedgePosition { get; set; }
        [DataMember()] public bool bKnockedPlayerDown { get; set; }
        [DataMember()] public bool bDispensedFood { get; set; }
        [DataMember()] public int iDirection { get; set; }             // 1 for away from the edge
                                                                       // -1 for heading back to the edge
        [DataMember()] public bool bFollowingThroughMaze { get; set; }
        [DataMember()] public bool bFollowedThroughMaze { get; set; }

        public CrazyOldGuy() : base()
        {
            sName = "Crazy Guy";
            sDescription = "He's a crazy looking old man, with wild hair and a wild beard and " +
                "wild eyes, and his smile seems a bit fixed and somewhat deranged.  His right " +
                "arm is heavily bandaged, and it looks seriously swollen under the bandage.";
            sDefiniteName = "the crazy guy";
            sIndefiniteName = "a crazy guy";        // Gets set to "the crazy guy" later, after
                                                    // talking to him.  Ah, English!

            bCanTalkTo = true;
            bNPC = true;

            iLedgePosition = 1;
            iDirection = -1;
            bKnockedPlayerDown = false;

            bFollowedThroughMaze = false;
            bFollowingThroughMaze = false;

            // Objects (and NPCs) are initialised before Locations, so can't set his location
            // in his constructor.
            // His start position is World._highLedge.
        }
    }

    [DataContractAttribute(IsReference = true)]
    public class QuestGiver : Object
    {
        [DataMember()] public bool bTalkedTo { get; set; }
        [DataMember()] public bool bCompletedFetchQuest { get; set; }
        [DataMember()] public bool bGivenCakeQuest { get; set; }
        [DataMember()] public bool bCompletedCakeQuest { get; set; }

        public QuestGiver() : base()
        {
            sName = "House Owner";
            sDescription = "The house owner is a woman of indeterminate possibly-middling-age, " +
                "in a bumblebee-patterned dress, with brightly coloured tights, and with her " +
                "dark hair both tied back and " +
                "falling all over the place.  Around her neck she wears a gold necklace, and " +
                "just under her collarbones is a vivdly green gemstone.";
            sIndefiniteName = "a house owner";
            sDefiniteName = "the house owner";

            bNPC = true;
            bCanTalkTo = true;

            bTalkedTo = false;
            bCompletedFetchQuest = false;
            bGivenCakeQuest = false;
            bCompletedCakeQuest = false;

        }

        public void TalkTo(ref string OutMessage)
        {
            if (bCompletedFetchQuest == false)
            {
                if (bTalkedTo == false)
                {
                    bTalkedTo = true;
                    OutMessage += "\"So, an adventurous sort, eh?  Willing to take quests and " +
                        "and go off to strange and possibly dangerous locations, in the off " +
                        "chance of finding treasure and interesting things?  You're just who " +
                        "I need to see!  I got a quest for ya.  There was a pretty big storm, " +
                        "and it blew my washing all over the forest.  And then on top of that " +
                        "I'd left my mother's ring in a pocket in the washing somewhere!  Be " +
                        "a dear and go find that too, thanks.\"\n";
                    if ( ( World._player.HasItem(World._ring) == true ) &&
                         ( World._player.HasItem(World._stormBlownClothes) == true ) &&
                         ( World._player.HasItem(World._stormBlownPants) == true )
                        )
                    {
                        OutMessage += "\nAs you already have everything she wants, you hand " +
                            "the wayward washing and the ring over to the woman.\n\n" +
                            "\"Oh wow, I knew I could count on you!  You'd already found it " +
                            "all already!";
                        bCompletedFetchQuest = true;
                        World._player.Remove(World._ring);
                        World._player.Remove(World._stormBlownClothes);
                        World._player.Remove(World._stormBlownPants);

                    }
                }
                else
                {
                    if ((World._player.HasItem(World._ring) == true) &&
                         (World._player.HasItem(World._stormBlownClothes) == true) &&
                         (World._player.HasItem(World._stormBlownPants) == true)
                        )
                    {
                        OutMessage += "\"So, you found all my stuff yet?\"\n\n" +
                            "As you now have everything she wants, you hand " +
                            "the wayward washing and the ring over to the woman.\n\n" +
                            "\"Oh wow, I knew I could count on you!  You've found it all!";

                        bCompletedFetchQuest = true;
                        World._player.Remove(World._ring);
                        World._player.Remove(World._stormBlownClothes);
                        World._player.Remove(World._stormBlownPants);

                    }
                    else
                    {
                        OutMessage += "\"You still haven't gone and found all my washing and my " +
                            "ring yet.  Shoo, go off and find it.\"\n";
                    }
                }
            }
            
            if (bCompletedFetchQuest)
            {
                if  (bGivenCakeQuest == false) 
                {
                    OutMessage += "  So, hey.  Thanks for that!  And I've got another " +
                        "quest for ya.  A much more " +
                        "interesting quest.  Somewhere nearby is *the* Tiny Cave, where once " +
                        "I went looking for a fortune in treasure and gold.  Didn't get " +
                        "squat, and I dropped my mother's favourite cake recipe somewhere in the " +
                        "place.  I've asked a bunch of people to go find it for me, though " +
                        "some of them what entered ain't never been seen again.  Others say " +
                        "that magic and weird stuff works in the cave.  Anyway, you're an " +
                        "adventurous sort, do be a dear and go find my cake recipe for me, " +
                        "willya?  You can find the cave entrance in the mountains to the " +
                        "south.\"\n";
                    bGivenCakeQuest = true;

                    World._ImpassableMountains.sDescription = "To the south there are " +
                        "impassable mountains.  A pathway leads to a forest to the north, " +
                        "and to the southeast is the entrance to the infamous Tiny Cave.  " +
                        "There are stunning valley views off to the west.";

                    World._ImpassableMountains.SoutheastLoc = World._caveEntrance;

                }
                else
                {
                    if (bCompletedCakeQuest == false)
                    {
                        OutMessage += "\"So what are you waiting for?  Go find my recipe " +
                            "for me.\"\n";
                    }
                    else
                    {
                        World._player.CurrentConversation = World._questGiverFinalConversation;
                    }
               
                }
            }

        }
    

    
    }

    [DataContractAttribute(IsReference = true)]
    public class Sage : Object
    {
        [DataMember()] public bool bClueGiven { get; set; }
        [DataMember()] public bool bClueUsed { get; set; }
        [DataMember()] public bool bGreedClueUsed { get; set; }
        [DataMember()] public bool bGreedClueGiven { get; set; }
        private List<string> ResponseOptions { get; set; }

        public Sage() : base()
        {
            sName = "Wise old sage";
            sDescription = "This oldish woman is bald, dressed in a simple robe and is meditating, serenely sitting down in an uncomfortable looking position, and her eyes are closed.";
            sIndefiniteName = "a sage";
            sDefiniteName = "the wise old sage";

            bCanTalkTo = true;
            bNPC = true;
            ResponseOptions = new List<String>();
        }


        public void TalkTo(ref string OutMessage)
        {
            Random random = new Random();
            int j;

            // Now that we've talked to the sage, set IndefiniteName to "the sage".
            // Ah, English.
            if (sIndefiniteName == "a sage")
            {
                sIndefiniteName = "the wise old sage";
            }

            
            // Context-specific responses
            if ( World._lobster.hiOwner != null )
            {
                // That is, the lobster now exists somewhere, so it's been unleashed
                OutMessage += "The sage looks at you glumly.  \"I see the Lobster has once " +
                    "again been unleashed.  Beware it, " +
                    "lest it hit you in a weak spot for massive damage.\"\n";
                return;
            }

            if ( (World._treasureConversation.bSomethingStrange) &&
                 (bGreedClueUsed == false)
                )
            {
                OutMessage += "The sage opens her eyes as you approach, and studies you " +
                    "thoughtfully.  \"Greed dooms not only the self, but contains the potential " +
                    "to doom others as well,\" she says.  \"Embrace nothingness. Only then " +
                    "can you empty your mind of greed and material thought, and continue on " +
                    "the path to understanding and transcendence.\"\n";
                World._tinyLedge.DownLoc = World._treasureCave;
                bGreedClueGiven = true;
                return;

            }
            if ( (World._player.bUsedSagesClue == false) || (bClueUsed == false) )
            {
                OutMessage += "The sage opens her eyes as you approach, and studies you " +
                    "thoughtfully.  \"Up hill and mountain, and down dale, and through " +
                    "storm, thick and clearing, and even up in the trees, you shall find " +
                    "your path,\" she says.\n";
                World._player.bReceivedSagesClue = true;
                bClueGiven = true;
                return;

            }

            // General responses
            ResponseOptions.Clear();

            if (World._player.bTiedUp)
            {
                ResponseOptions.Add("The sage opens her eyes as you approach, and studies you " +
                    "thoughtfully.  \"Bound you may be, but your path is not shackled,\" " + 
                    "she finally says.\n");
            }

            if (World._player.iSore > 1)
            {
                ResponseOptions.Add("The sage opens her eyes as you approach, and studies you " +
                    "thoughtfully.  \"Bear the pain, for it, too, shall pass,\" she says.\n");
            }

            ResponseOptions.Add("The sage opens her eyes as you approach, and studies you " +
                "thoughtfully.  \"In crockery, too, the path shall lie, but only for a new " +
                "voice.\"\n");

            ResponseOptions.Add("The sage opens her eyes as you approach, and studies you " +
                "thoughtfully.  \"A popular pastime is the promulgation of the prevarication " +
                "of... Uh... Pie.  I like pie,\" she says.\n");

            ResponseOptions.Add("The sage opens her eyes as you approach, and studies you " +
                "thoughtfully.  \"Beware the lobster,\" she says solemnly.\n");

            ResponseOptions.Add("The sage opens her eyes as you approach, and studies you " +
                "thoughtfully.  \"Not all is as it seems, except when it is,\" she finally " +
                "says.\n");

            j = random.Next(0, (ResponseOptions.Count - 1));
            OutMessage += ResponseOptions[j];

        }
    }
    

}
