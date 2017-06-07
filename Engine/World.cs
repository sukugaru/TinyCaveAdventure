using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CustomExtensions;
using System.Runtime.Serialization;
using System.IO;

// 7/6/2017 - Bug 6 - Renaming the Object class to Item.
// 
// 6/6/2017 - Enhancement 7 - PostAction method - use the new AddMoveType and RemoveMoveType to add and
//                            remove "climb" from the player.
//                            Commenting out various debuggery type things in PostAction
//
// 6/6/2017 - Bug 10 - The Pathway system wasn't working if locked or blocked is in the
//                     MovementTypes.  Also fixing up some formatting.
//
// 24/5/2017 - Enhancements 1+5 : Adding Size.  Adding a Size to all object definitions, and capacity to
//             all container definitions.
//           - Enhancements 1+5 : In PostAction, add and remove climb based on whether the player is
//             tied up or carrying anything.
//
// 22/5/2017 - Bug 3 - Changes to MoveTo() to improve the messaging to the player if the
// player does not have the required movement types.



namespace Engine
{
    [DataContractAttribute(IsReference = true)]    
    public static class World
    {
        // Player
        [DataMember()] public static Player _player;

        // Beginning Text
        // (If you want to start with a text sequence instead, set _player.CurrentTextSequence
        // at the end of the World() constructor.)
        public static string _BeginningText;

        // Actions - standard actions
        public static Inventory_Action _Inventory;
        public static LookAtLocation_Action _LookAtLocation;
        public static LookAtItem_Action _LookAtItem;
        public static Remove_Action _Remove;
        public static Wear_Action _Wear;
        public static Drop_Action _Drop;
        public static Take_Action _Take;
        public static Use_Action _Use;
        public static PutInto_Action _PutInto;
        public static GetOut_Action _GetOutOf;
        public static UseWith_Action _UseWith;
        public static TalkTo_Action _TalkTo;
        public static Wait_Action _Wait;
        public static DropThroughHole_Action _DropThroughHole;

        public static Engine.Action _Report_hiOwner;

        // Actions - new actions
        public static JumpTo_Action _JumpUp;
        public static JumpTo_Action _JumpDown;

        // Directions
        public static Direction _north;
        public static Direction _northeast;
        public static Direction _east;
        public static Direction _southeast;
        public static Direction _south;
        public static Direction _southwest;
        public static Direction _west;
        public static Direction _northwest;
        public static Direction _up;
        public static Direction _down;

        public static List<Direction> AllDirections;
        //        public static List<Direction> AllDirections = new List<Direction>();

        // Objects
        [DataMember()] public static Item _head;

        [DataMember()] public static Item _mailbox;
        [DataMember()] public static Item _leaflet;
        [DataMember()] public static Item _washingLine;
        [DataMember()] public static Item _nest;
        [DataMember()] public static Item _stormBlownClothes;
        [DataMember()] public static Item _stormBlownPants;
        [DataMember()] public static Item _ring;

        [DataMember()] public static Item _stalagmiteBase;
        [DataMember()] public static Item _boulder;
        [DataMember()] public static Item _ledge;
        [DataMember()] public static Item _precariousPlatform;
        [DataMember()] public static Item _chunkOfStalagmite;
        [DataMember()] public static Item _chunkOfStalactite;
        [DataMember()] public static Item _tinnedFish;
        [DataMember()] public static Item _bottledWater;

        [DataMember()] public static Item _vendingMachine;
        [DataMember()] public static Item _Note;
        [DataMember()] public static Item _ReturnsBox;
        [DataMember()] public static Item _MazeBook;
        [DataMember()] public static Item _WallMap;
        [DataMember()] public static Item _PaperAndStationeryKit;
        [DataMember()] public static Item _XXiumSaw;
        [DataMember()] public static Item _map;
        [DataMember()] public static Item _abstractDesigns;
        [DataMember()] public static Item _holyBasket;
        [DataMember()] public static Item _sachet;
        [DataMember()] public static Item _abandonedShrine;
        [DataMember()] public static Item _tribalCostume;
        [DataMember()] public static Item _tribalHeadgear;
        [DataMember()] public static Item _lemonWater;
        [DataMember()] public static Item _parkourManual;
        [DataMember()] public static Item _treasure;
        [DataMember()] public static Item _fizzyDrink;
        [DataMember()] public static Lobster _lobster = new Lobster();
        [DataMember()] public static Item _lostGemNecklace;

        [DataMember()] public static RecipeObject _recipe = new RecipeObject();
        [DataMember()] public static DaisObject _dais = new DaisObject();
        [DataMember()] public static BedsObject _beds = new BedsObject();
        [DataMember()] public static FirePitsObject _firePits = new FirePitsObject();
        [DataMember()] public static FoodStoresObject _foodStores = new FoodStoresObject();

        public static Item _CarryBag;


        // We'll just call NPCs Objects for now

        [DataMember()] public static QuestGiver _questGiver;
        [DataMember()] public static Sage _sage;
        [DataMember()] public static CrazyOldGuy _crazyGuy;

        [DataMember()] public static Villagers _villagers = new Villagers();
        [DataMember()] public static LeaderObject _leader = new LeaderObject();

        // Location Groups
        [DataMember()] public static LocationGroup TestLocationGroup;
        [DataMember()] public static LocationGroup MazeLocations;
        [DataMember()] public static LocationGroup AllLocations = new LocationGroup();

        // Locations

        [DataMember()] public static CaveEntrance_Location _caveEntrance;
        [DataMember()] public static Location _stalagmiteCave;
        [DataMember()] public static Location _highLedge;
        [DataMember()] public static Location _mazeEntrance;
        [DataMember()] public static maze_Location _maze;
        [DataMember()] public static CentralCavern_Location _centralCavern;
        [DataMember()] public static SageGrotto_Location _sageGrotto;
        [DataMember()] public static specialMaze_Location _specialMazeLocation;
        [DataMember()] public static SlopingPassage_Location _slopingPassage;
        [DataMember()] public static HolyBasket_Location _holyBasketSite;
        [DataMember()] public static AbandonedShrine_Location _abandonedShrineSite;
        [DataMember()] public static TreasureCave_Location _treasureCave;
        [DataMember()] public static TribalCavern_Location _tribalCavern;
        [DataMember()] public static TinyLedge_Location _tinyLedge;

        [DataMember()] public static Location _FrontDoor;
        [DataMember()] public static Location _WestOfHouse;
        [DataMember()] public static Location _EastOfHouse;
        [DataMember()] public static Location _BackDoor;
        [DataMember()] public static Location _Forest1;
        [DataMember()] public static Location _Forest2;
        [DataMember()] public static Location _ForestClearing;
        [DataMember()] public static Location _UpATree;
        [DataMember()] public static Location _ForestPath;
        [DataMember()] public static Location _ImpassableMountains;



        // Special 'locked' and 'blocked' locations.  If a location.direction is either of
        // these, it means you can't go in that direction because the way is either locked, or
        // blocked in some way.
        [DataMember()] public static Location _locked;
        [DataMember()] public static Location _blocked;


        // Conversations

        [DataMember()] public static CrazyGuyConversation _crazyGuyConversation;
        [DataMember()] public static VendingMachineConversation _vendingMachineConversation;
        [DataMember()] public static WallMapConversation _wallMapConversation;
        [DataMember()] public static ParkourManualConversation _parkourManualConversation;
        [DataMember()] public static TreasureConversation _treasureConversation;
        [DataMember()] public static QuestGiverFinalConversation _questGiverFinalConversation;

        // Text Sequences

        public static TextSequence _startTextSequence;
        public static TextSequence _mazeBookSequence;
        public static TribeVistSequence _tribeFirstVisitSequence;
        public static EndingSequence _endSequence;

        static World()
        {
            populateActions();
            populateDirections();
            populateObjects();
            populateLocations();
            populatePrologueLocations();
            populateLocationGroups();
            populateConversations();
            populateTextSequences();
            initialisePlayer();

            // Either assign _BeginningText or set the current text sequence to an introductory
            // text sequence.

            //  _BeginningText = "This is the beginning of a grand, well, tiny, cave adventure!\n";
            _player.CurrentTextSequence = _startTextSequence;
        }

        public static void populateActions()
        {
            _Inventory = new Engine.Inventory_Action();
            _LookAtLocation = new Engine.LookAtLocation_Action();
            _LookAtItem = new Engine.LookAtItem_Action();
            _Wear = new Engine.Wear_Action();
            _Remove = new Engine.Remove_Action();
            _Drop = new Engine.Drop_Action();
            _Take = new Engine.Take_Action();
            _Use = new Engine.Use_Action();
            _PutInto = new Engine.PutInto_Action();
            _GetOutOf = new Engine.GetOut_Action();
            _UseWith = new Engine.UseWith_Action();
            _TalkTo = new Engine.TalkTo_Action();
            _Wait = new Engine.Wait_Action();

            _JumpDown = new JumpTo_Action();
            _JumpUp = new JumpTo_Action();
            _DropThroughHole = new DropThroughHole_Action();


            _Report_hiOwner = new Engine.Action();
            _Report_hiOwner.sName = "Report hiOwner";
            _Report_hiOwner.sProtoCmdLine = "Report hiOwner of [item1]";
            _Report_hiOwner.iNumArgs = 1;

        }

        public static void populateDirections()
        {
            _north = new Direction("North");
            _northeast = new Direction("Northeast");
            _east = new Direction("East");
            _southeast = new Direction("Southeast");
            _south = new Direction("South");
            _southwest = new Direction("Southwest");
            _west = new Direction("West");
            _northwest = new Direction("Northwest");
            _up = new Direction("Up");
            _down = new Direction("Down");

            AllDirections = new List<Direction>();

            AllDirections.Clear();
            AllDirections.Add(_north);
            AllDirections.Add(_northeast);
            AllDirections.Add(_east);
            AllDirections.Add(_southeast);
            AllDirections.Add(_south);
            AllDirections.Add(_southwest);
            AllDirections.Add(_west);
            AllDirections.Add(_northwest);
            AllDirections.Add(_up);
            AllDirections.Add(_down);
        }

        public static void populateObjects()
        {
            _head = new Item();
            _head.sName = "Head";
            _head.sDescription = "It's your head.";
            _head.sDefiniteName = "your head";
            _head.sIndefiniteName = "your head";
            _head.bBodypart = true;
            _head.bUsableWhileTiedUp = true;
            _head.bUsableAnyway = true;

            _leaflet = new Item();
            _leaflet.sName = "Leaflet";
            _leaflet.sDescription = "The leaflet has a simple message on it:  \"Beware of mail thieves!\"";
            _leaflet.sIndefiniteName = "a leaflet";
            _leaflet.sDefiniteName = "the leaflet";
            _leaflet.bTakeable = true;
            _leaflet.bDroppable = true;
            _leaflet.sSize = Size.Small;

            _mailbox = new Item();
            _mailbox.sName = "Mailbox";
            _mailbox.sDescription = "This is a mailbox.  There is no house number on it, instead, there is a name: \"The Forest Clearing House\".";
            _mailbox.sDefiniteName = "the mailbox";
            _mailbox.sIndefiniteName = "a mailbox";
            _mailbox.bContainer = true;
            _mailbox.Add(_leaflet);
            _mailbox.sSize = Size.Medium;
            _mailbox.iContainerCapacity = 20; // Totally arbitrary!  
            _mailbox.sContainerSize = Size.Medium;

            _washingLine = new Item();
            _washingLine.sName = "Washing Lines";
            _washingLine.sDescription = "Washing lines are strung up between handy tree branches.  There is nothing on them.";
            _washingLine.sIndefiniteName = "washing lines";
            _washingLine.sDefiniteName = "the washing lines";
            _washingLine.sSize = Size.NA;

            _nest = new Item();
            _nest.sName = "Bird's nest";
            _nest.sDescription = "The nest has been expertly woven together by its owner, has stunning forest views, and is even padded with grass, for the bird with exacting requirements.";
            _nest.sDefiniteName = "the bird's nest";
            _nest.sIndefiniteName = "a bird's nest";
            _nest.bContainer = true;
            _nest.bDiscoveredContents = true;
            _nest.sSize = Size.Small;
            _nest.sContainerSize = Size.Small;
            _nest.iContainerCapacity = 5; // Mostly arbitrary.  You can't fit that many things in a bird's nest.

            _ring = new Item();
            _ring.sName = "Ring";
            _ring.sDescription = "This is a really fancy looking gold ring.  There is an inscription on the inside but it's archaic, ancient, and incomprehensible.";
            _ring.sDefiniteName = "the ring";
            _ring.sIndefiniteName = "a ring";
            _ring.bTakeable = true;
            _ring.bDroppable = true;
            _ring.sSize = Size.Tiny;

            _stormBlownPants = new Item();
            _stormBlownPants.sName = "Storm-blown pants";
            _stormBlownPants.sDescription = "These are some pants, blown away during a violent storm and looking rather worse for wear.";
            _stormBlownPants.sDefiniteName = "the storm-blown pants";
            _stormBlownPants.sIndefiniteName = "some storm-blown pants";
            _stormBlownPants.bTakeable = true;
            _stormBlownPants.bDroppable = true;
            _stormBlownPants.sSize = Size.Medium;

            _stormBlownClothes = new Item();
            _stormBlownClothes.sName = "Storm-blown clothes";
            _stormBlownClothes.sDescription = "These are a lot of clothes, blown away during a violent storm and looking rather worse for wear.";
            _stormBlownClothes.sDefiniteName = "the storm-blown clothes";
            _stormBlownClothes.sIndefiniteName = "some storm-blown clothes";
            _stormBlownClothes.bTakeable = true;
            _stormBlownClothes.bDroppable = true;
            _stormBlownClothes.sSize = Size.Medium;

            _vendingMachine = new Item();
            _vendingMachine.sName = "Vending Machine";
            _vendingMachine.sDescription = "Inexplicably, in the middle of this tiny cave you're in, there is a vending machine.";
            _vendingMachine.sIndefiniteName = "a vending machine";
            _vendingMachine.sDefiniteName = "the vending machine";
            _vendingMachine.bUsableWhileTiedUp = true;
            _vendingMachine.bUsableAnyway = true;
            _vendingMachine.sSize = Size.NA;

            _chunkOfStalagmite = new Item();
            _chunkOfStalagmite.sName = "Chunk of Stalagmite";
            _chunkOfStalagmite.sDescription = "A piece of stalagmite, broken off.";
            _chunkOfStalagmite.sIndefiniteName = "a chunk of stalagmite";
            _chunkOfStalagmite.sDefiniteName = "the chunk of stalagmite";
            _chunkOfStalagmite.bTakeable = true;
            _chunkOfStalagmite.bDroppable = true;
            _chunkOfStalagmite.sSize = Size.Tiny;

            _chunkOfStalactite = new Item();
            _chunkOfStalactite.sName = "Chunk of Stalactite";
            _chunkOfStalactite.sDescription = "A piece of stalactite, broken off.";
            _chunkOfStalactite.sIndefiniteName = "a chunk of stalactite";
            _chunkOfStalactite.sDefiniteName = "the chunk of stalactite";
            _chunkOfStalactite.bTakeable = true;
            _chunkOfStalactite.bDroppable = true;
            _chunkOfStalactite.sSize = Size.Tiny;

            _Note = new Item();
            _Note.sName = "Note";
            _Note.sDescription = "The note says, \"RETURNS BOX\nWe noticed you left something in the maze, so we are returning your things to you.\n - The Managament\"";
            _Note.sDefiniteName = "the note";
            _Note.sIndefiniteName = "a note";
            _Note.sSize = Size.Small;

            _ReturnsBox = new Item();
            _ReturnsBox.sName = "box";
            _ReturnsBox.sDescription = "There is a large box here, half as large as you.";
            _ReturnsBox.sDefiniteName = "the box";
            _ReturnsBox.sIndefiniteName = "a box";
            _ReturnsBox.bContainer = true;
            _ReturnsBox.bDiscoveredContents = false;
            _ReturnsBox.Inventory.Clear();
            _ReturnsBox.sSize = Size.NA;
            _ReturnsBox.iContainerCapacity = 0;
            _ReturnsBox.sContainerSize = Size.Medium;

            _MazeBook = new Item();
            _MazeBook.sName = "Tattered book";
            _MazeBook.sDescription = "The title of this book is \"A history of adventures\".";
            _MazeBook.sDefiniteName = "the tattered book";
            _MazeBook.sIndefiniteName = "a tattered book";
            _MazeBook.bTakeable = true;
            _MazeBook.bDroppable = true;
            _MazeBook.bUsableWhileTiedUp = true;
            _MazeBook.bUsableAnyway = true;
            _MazeBook.sSize = Size.Medium;

            _stalagmiteBase = new Item();
            _stalagmiteBase.sName = "Stalagmite base";
            _stalagmiteBase.sDescription = "This is the base of a large stalagmite, broken " +
                "off and reaching up to about waist height.";
            _stalagmiteBase.sIndefiniteName = "a stalagmite base";
            _stalagmiteBase.sDefiniteName = "the stalagmite base";
            _stalagmiteBase.sSize = Size.NA;

            _boulder = new Item();
            _boulder.sName = "Boulder";
            _boulder.sDescription = "This is a large boulder, much larger than you.  It looks " +
                "like it used to be part of a massive stalagmite that broke off;  it is only " +
                "a short distance from the base of a broken-off stalagmite.";
            _boulder.sIndefiniteName = "a boulder";
            _boulder.sDefiniteName = "the boulder";
            _boulder.sSize = Size.NA;

            _ledge = new Item();
            _ledge.sName = "Ledge";
            _ledge.sDescription = "Partway up the massive rockface you can see where a large " +
                "chunk of the wall has been gouged out.  This is making a natural ledge in " +
                "the wall.";
            _ledge.sDefiniteName = "the ledge";
            _ledge.sIndefiniteName = "a ledge";
            _ledge.sSize = Size.NA;

            _precariousPlatform = new Item();
            _precariousPlatform.sName = "Precarious Platform";
            _precariousPlatform.sDescription = "A large chunk of rock, probably from a broken-" +
                "off stalactite, is balancing precariously on top of a number of stalagmites.";
            _precariousPlatform.sDefiniteName = "the precarious platform";
            _precariousPlatform.sIndefiniteName = "a precarious platform";
            _precariousPlatform.sSize = Size.NA;

            _tinnedFish = new Item();
            _tinnedFish.sName = "Tinned fish";
            _tinnedFish.sDescription = "A small tin of mashed-up fish.  The tin is a vivid " +
                "red, and says it is the finest tinned herring in the entire Tiny Cave.  " +
                "\"Now with added tomato flavour!\" says the label.";
            _tinnedFish.sDefiniteName = "the tinned fish";
            _tinnedFish.sIndefiniteName = "a tin of fish";
            _tinnedFish.bTakeable = true;
            _tinnedFish.bDroppable = true;
            _tinnedFish.sSize = Size.Tiny;

            _bottledWater = new Item();
            _bottledWater.sName = "Bottled water";
            _bottledWater.sDescription = "The label says this water is from the purest " +
                "mountain river snowmelt, and bottled with care by water technicians in " +
                "flowing white robes.  \"Flowing... like water!\"";
            _bottledWater.sDefiniteName = "the bottled water";
            _bottledWater.sIndefiniteName = "a bottle of water";
            _bottledWater.bTakeable = true;
            _bottledWater.bDroppable = true;
            _bottledWater.bUsableWhileTiedUp = true;
            _bottledWater.bUsableAnyway = true;
            _bottledWater.sSize = Size.Small;


            _WallMap = new Item();
            _WallMap.sName = "Map";
            _WallMap.sDescription = "At first this just looks like some sort of abstract art, " +
                "drawn on the side of the wall.  As you study it closer, you realise it's " +
                "actually a map of the maze!  You can see how to get from here back to the " +
                "Maze Entrance, and back to the High Ledge.";
            _WallMap.sDefiniteName = "the map";
            _WallMap.sIndefiniteName = "a map";
            _WallMap.bUsableWhileTiedUp = true;
            _WallMap.bUsableAnyway = true;
            _WallMap.bStaysInMaze = true;
            _WallMap.sSize = Size.Small;

            _PaperAndStationeryKit = new Item();
            _PaperAndStationeryKit.sName = "Paper and Stationery Kit";
            _PaperAndStationeryKit.sDescription = "This is a collection of 10 sheets of paper " +
                "and a set of pens, pencils, an eraser, a straightedge and a compass.";
            _PaperAndStationeryKit.sDefiniteName = "the paper and stationery kit";
            _PaperAndStationeryKit.sIndefiniteName = "some paper and a stationery kit";
            _PaperAndStationeryKit.bUsableWhileTiedUp = true;
            _PaperAndStationeryKit.bUsableAnyway = true;
            _PaperAndStationeryKit.bTakeable = true;
            _PaperAndStationeryKit.bDroppable = true;
            _PaperAndStationeryKit.sSize = Size.Small;


            _XXiumSaw = new Item();
            _XXiumSaw.sName = "XXium Saw";
            _XXiumSaw.sDescription = "This is a saw made of the hardest material known in the " +
                "entire Tiny Cave.";
            _XXiumSaw.sIndefiniteName = "an XXium saw";
            _XXiumSaw.sDefiniteName = "the XXium saw";
            _XXiumSaw.bUsableWhileTiedUp = true;
            _XXiumSaw.bUsableAnyway = true;
            _XXiumSaw.bTakeable = true;
            _XXiumSaw.bDroppable = true;
            _XXiumSaw.sSize = Size.Medium;

            _map = new Item();
            _map.sName = "Maze Map";
            _map.sDescription = "It might look like an abstract drawing, but this is actually " +
                "a map of the Tiny Cave's maze of passages, along with a pen to make any " +
                "adjustments or additions.";
            _map.sDefiniteName = "your maze map";
            _map.sIndefiniteName = "your maze map";
            _map.bTakeable = true;
            _map.bDroppable = true;
            _map.bUsableWhileTiedUp = true;
            _map.bUsableAnyway = true;
            _map.sSize = Size.Small;

            _abstractDesigns = new Item();
            _abstractDesigns.sName = "Strange abstract designs";
            _abstractDesigns.sDescription = "These are strange abstract designs drawn all " +
                "over the walls of the passage.";
            _abstractDesigns.sDefiniteName = "the strange abstract designs";
            _abstractDesigns.sIndefiniteName = "some strange abstract designs";
            _abstractDesigns.bUsableWhileTiedUp = true;
            _abstractDesigns.bUsableAnyway = true;
            _abstractDesigns.sSize = Size.NA;

            _sachet = new Item();
            _sachet.sName = "Sachet of Lemon Flavouring";
            _sachet.sDescription = "This is an old sachet of lemon flavouring.  A quick sniff " +
                "reveals it is still potent.";
            _sachet.sDefiniteName = "the sachet";
            _sachet.sIndefiniteName = "a sachet";
            _sachet.bTakeable = true;
            _sachet.bDroppable = true;
            _sachet.bUsableWhileTiedUp = true;
            _sachet.bUsableAnyway = true;
            _sachet.bStaysInMaze = true;
            _sachet.sSize = Size.Tiny;

            _holyBasket = new Item();
            _holyBasket.sName = "Very old basket";
            _holyBasket.sDescription = "There is a very old picnic basket here.  It is " +
                "almost rotted through and fallen into pieces.  In its awful murky depths " +
                "there is a split teabag, the shards of a broken mug, and a sachet of " +
                "lemon flavouring.";
            _holyBasket.sDefiniteName = "the basket";
            _holyBasket.sIndefiniteName = "a basket";
            _holyBasket.bStaysInMaze = true;
            _holyBasket.bContainer = true;
            _holyBasket.bDiscoveredContents = true;
            _holyBasket.Add(_sachet);
            _holyBasket.sSize = Size.NA;

            _abandonedShrine = new Item();
            _abandonedShrine.sName = "Abandoned Shrine";
            _abandonedShrine.sDescription = "This is an old abandoned shrine, adorned with a " +
                "stone statue wearing tribal gear.";
            _abandonedShrine.sDefiniteName = "the abandoned shrine";
            _abandonedShrine.sIndefiniteName = "an abandoned shrine";
            _abandonedShrine.bStaysInMaze = true;
            _abandonedShrine.sSize = Size.NA;

            _tribalCostume = new Item();
            _tribalCostume.sName = "Tribal Gear";
            _tribalCostume.sDescription = "This is the oddest tribal gear you've ever seen, " +
                "adorned as it is with teaspoons and shards of crockery.";
            _tribalCostume.sDefiniteName = "the tribal gear";
            _tribalCostume.sIndefiniteName = "some tribal gear";
            _tribalCostume.bTakeable = true;
            _tribalCostume.bDroppable = true;
            _tribalCostume.bWearable = true;
            _tribalCostume.bWorn = false;
            _tribalCostume.sClothingType = "costume";
            _tribalCostume.bStaysInMaze = true;
            _tribalCostume.sSize = Size.NA;

            _tribalHeadgear = new Item();
            _tribalHeadgear.sName = "Tribal Headgear";
            _tribalHeadgear.sDescription = "This is the oddest headgear you've ever seen.  " +
                "The face is a mask, made of the shards of broken plates, and the crown is " +
                "adorned with forks and small shards of crockery.  The inside of the " +
                "headgear appears to be the old remains of a red and white checked blanket.";
            _tribalHeadgear.sDefiniteName = "the tribal headgear";
            _tribalHeadgear.sIndefiniteName = "some tribal headgear";
            _tribalHeadgear.bTakeable = true;
            _tribalHeadgear.bDroppable = true;
            _tribalHeadgear.bWearable = true;
            _tribalHeadgear.bWorn = false;
            _tribalHeadgear.sClothingType = "head";
            _tribalHeadgear.bStaysInMaze = true;
            _tribalHeadgear.sSize = Size.Medium;

            _lemonWater = new Item();
            _lemonWater.sName = "Lemony Water";
            _lemonWater.sDescription = "You're not game to actually drink it, but this now " +
                "smells very very lemony.";
            _lemonWater.sDefiniteName = "the lemon water";
            _lemonWater.sIndefiniteName = "a bottle of lemon water";
            _lemonWater.bTakeable = true;
            _lemonWater.bDroppable = true;
            _lemonWater.sSize = Size.Small;

            _parkourManual = new Item();
            _parkourManual.sName = "Parkour Manual";
            _parkourManual.sDescription = "This book gives some quite detailed instructions " +
                "and drills on how to become, if not a parkour master, at least proficient " +
                "at it.";
            _parkourManual.sDefiniteName = "the parkour manual";
            _parkourManual.sIndefiniteName = "a parkour manual";
            _parkourManual.bTakeable = true;
            _parkourManual.bDroppable = true;
            _parkourManual.sSize = Size.Medium;

            _treasure = new Item();
            _treasure.sName = "Treasure";
            _treasure.sDescription = "The fabled treasure hoard of the Tiny Cave.  It is more " +
                "massive then ever you dreamed in your own wild imaginings!";
            _treasure.sDefiniteName = "The Tiny Cave Treasure";
            _treasure.sIndefiniteName = "The Tiny Cave Treasure";
            _treasure.sSize = Size.NA;

            _fizzyDrink = new Item();
            _fizzyDrink.sName = "Fizzy Drink";
            _fizzyDrink.sDescription = "A bottle of fizzy soft drink.";
            _fizzyDrink.sDefiniteName = "the fizzy drink";
            _fizzyDrink.sIndefiniteName = "a bottle of fizzy soft drink.";
            _fizzyDrink.bTakeable = true;
            _fizzyDrink.bDroppable = true;
            _fizzyDrink.sSize = Size.Small;

            _lostGemNecklace = new Item();
            _lostGemNecklace.sName = "Lost Necklace";
            _lostGemNecklace.sDescription = "A gold necklace, with a vividly green gemstone set " +
                "in it.";
            _lostGemNecklace.bTakeable = true;
            _lostGemNecklace.bDroppable = true;
            _lostGemNecklace.bWearable = true;
            _lostGemNecklace.sDefiniteName = "the Lost Necklace";
            _lostGemNecklace.sIndefiniteName = "the Lost Necklace";
            _lostGemNecklace.sSize = Size.Tiny;

            // 24/5/2017 - Enhancements 1 and 5 - This used to be the Infinite Carrybag, used in
            // testing.  Turning it into a regular carrybag for Enhancements 1 + 5.
            _CarryBag = new Item();
            _CarryBag.sName = "Carrybag";
            _CarryBag.sDefiniteName = "the carrybag";
            _CarryBag.sIndefiniteName = "a carrybag";
            _CarryBag.sDescription = "A bag with a long strap, easily carried over one shoulder.  It looks old and threadbare, but still usable.";
            _CarryBag.bTakeable = true;
            _CarryBag.bDroppable = true;
            _CarryBag.bWearable = true;
            _CarryBag.bContainer = true;
            _CarryBag.sSize = Size.Small;
            _CarryBag.sContainerSize = Size.Medium;
            _CarryBag.iContainerCapacity = 0;


            // NPCs
            _crazyGuy = new CrazyOldGuy();
            _sage = new Sage();
            _questGiver = new QuestGiver();
        }

        public static void populateLocations()
        {
            // Create locations, fill with objects, and link them together

            // Special utility locations
            _locked = new Location();
            _locked.sName = "Utility Locked Location";
            _locked.sDescription = "You should never be able to get here!  Something's gone wrong!";
            _blocked = new Location();
            _blocked.sName = "Utility Blocked Location";
            _blocked.sDescription = "You should never be able to get here!  Something's gone wrong!";


            // Normal locations
            _stalagmiteCave = new stalagmiteCave_Location();
            _highLedge = new highLedge_Location();

            _maze = new maze_Location();
            /*            _maze.sName = "Maze";
                        _maze.sDescription = "This is a maze of twisty little tunnels and passages, all alike.";
                        _maze.bDiscovered = false;
                        */
            _mazeEntrance = new mazeEntrance_Location();
            /*            _mazeEntrance.sName = "Maze Entrance";
                        _mazeEntrance.sDescription = "To the west is a maze, one of the most dreaded puzzles in all Adventuredom!";
                        _mazeEntrance.sFirstVisitText = "In a corner you can see an old discarded book.";
                        _mazeEntrance.bDiscovered = false;
                        _mazeEntrance.Add(_ReturnsBox);
                        _mazeEntrance.Add(_MazeBook);
                        */
            _caveEntrance = new CaveEntrance_Location();
            _centralCavern = new CentralCavern_Location();
            _sageGrotto = new SageGrotto_Location();
            _specialMazeLocation = new specialMaze_Location();
            _slopingPassage = new SlopingPassage_Location();
            _holyBasketSite = new HolyBasket_Location();
            _abandonedShrineSite = new AbandonedShrine_Location();
            _treasureCave = new TreasureCave_Location();
            _tribalCavern = new TribalCavern_Location();
            _tinyLedge = new TinyLedge_Location();

            // Link locations
            /*
            _highLedge.LinkLocation();
            _caveEntrance.LinkLocation();
            _centralCavern.LinkLocation();
            _sageGrotto.LinkLocation();
            _specialMazeLocation.LinkLocation();
            _slopingPassage.LinkLocation();
            _holyBasketSite.LinkLocation();
            _abandonedShrineSite.LinkLocation();
            _treasureCave.LinkLocation();
            _tribalCavern.LinkLocation();
            _tinyLedge.LinkLocation();


            _mazeEntrance.LinkLocation();
            _maze.LinkLocation();
            */
            foreach (Location l in AllLocations.LocationList)
            {
                l.LinkLocation();
            }

            /*
                        _mazeEntrance.WestLoc = _maze;
                        _mazeEntrance.EastLoc = _centralCavern;
                        */
            /*
            _maze.NorthLoc = _maze;
            _maze.SouthLoc = _maze;
            _maze.EastLoc = _maze;
            _maze.WestLoc = _maze;
            _maze.NortheastLoc = _maze;
            _maze.SoutheastLoc = _maze;
            _maze.SouthwestLoc = _maze;
            _maze.NorthwestLoc = _maze;
            _maze.UpLoc = _maze;
            _maze.DownLoc = _maze;
            */

            // nothing yet...

            // Special actions that lead to locations

            // e.g. Go to X actions
            // nothing yet
            // e.g. World._streetOutsideBuilding.LocationActions.Add(World._GoToStarkStore);



        }

        public static void populatePrologueLocations()
        {
            _FrontDoor = new frontDoor_Location();
            _WestOfHouse = new WestOfHouse_Location();
            _EastOfHouse = new EastOfHouse_Location();
            _BackDoor = new BackDoor_Location();

            _ForestPath = new ForestPath_Location();
            _Forest1 = new Forest1_Location();
            _Forest2 = new Forest2_Location();
            _ForestClearing = new ForestClearing_Location();

            _ImpassableMountains = new ImpassableMountains_Location();
            _UpATree = new UpATree_Location();

            _FrontDoor.LinkLocation();
            _WestOfHouse.LinkLocation();
            _EastOfHouse.LinkLocation();
            _BackDoor.LinkLocation();
            _ForestPath.LinkLocation();
            _ForestClearing.LinkLocation();
            _UpATree.LinkLocation();
            _Forest1.LinkLocation();
            _Forest2.LinkLocation();
            _ImpassableMountains.LinkLocation();



            /*
            _FrontDoor.sName = "Front of house";
            _FrontDoor.sDescription = "You are standing in an open field south of a white " +
                "house, with a boarded up front door.  There is a mailbox here.\n\n";
            _FrontDoor.bDiscovered = true;
            */

        }

        public static void populateLocationGroups()
        {
            // Location Groups

            List<Location> MazeList = new List<Location>();
            MazeList.Add(_maze);
            MazeList.Add(_mazeEntrance);
            MazeList.Add(_highLedge);
            MazeList.Add(_specialMazeLocation);
            MazeList.Add(_holyBasketSite);
            MazeList.Add(_abandonedShrineSite);
            MazeLocations = new LocationGroup(MazeList);
        }

        public static void populateConversations()
        {
            _crazyGuyConversation = new CrazyGuyConversation();
            _vendingMachineConversation = new VendingMachineConversation();
            _wallMapConversation = new WallMapConversation();
            _parkourManualConversation = new ParkourManualConversation();
            _treasureConversation = new TreasureConversation();
            _questGiverFinalConversation = new QuestGiverFinalConversation();
        }

        public static void populateTextSequences()
        {
            // populating individual text sequences in their own procedures.  Because these
            // things can be pretty big!
            populateStartTextSequence();
            populateMazeBookSequence();
            populateTribeVisitSequence();
            populateEndSequence();
        }

        private static void populateStartTextSequence()
        {
            string s;
            List<string> sList;

            sList = new List<string>();

            s = "The story is well known - the Wizard of the Tiny Cave was, once upon a time, " +
                "just a man, whose ambition far outstripped his " +
                "ability.  He wanted to make something of himself, and live a life that was " +
                "big and sprawling and action-packed.  " +
                "He wanted to create things, and leave behind an amazing story or two.  " +
                "He was big on ideas, but short on, well, any of the things " +
                "he needed to make such things.\n\n" +
                "Filled with despair and disappointment and despondence and other words " +
                "beginning with d, he gave up on his ideas and went into finance and " +
                "accounting.\n\n" +
                "And there his boring story would have ended, were it not for the fact that " +
                "he went " +
                "off the deep end, went into the country and mountains, and found a tiny " +
                "little cave.  He filled it with all sorts of dangers, oddities, and the " +
                "occasional Very Valuable Thing.\n";
            sList.Add(s);

            s = "You are not in that cave.  Not yet.  You'll get to it in a bit.\n";
            sList.Add(s);

            s = "You are instead standing in an open " +
                "field south of a white house, with a boarded up front door.  There is a " +
                "mailbox here.\n";
            sList.Add(s);

            _startTextSequence = new TextSequence(sList);
        }

        private static void populateMazeBookSequence()
        {
            string s;
            List<string> sList;

            sList = new List<string>();

            s = "When opened, the tattered book naturally falls open to one particular, " +
                "very dogeared page.  It is a section on mazes.\n";
            sList.Add(s);

            s = "Ah, the maze.  Is there any sort of puzzle more frustrating and aggravating " +
                "in adventures?  Mention to a seasoned adventure player the phrase, 'You are " +
                "in a series of twisty little passages, all alike', and watch their face " +
                "twitch, and their expression sour.\n\n" +
                "Mazes, by their nature, are confusing, complicated messes, put into " +
                "adventure games to confuse and enrage, and lengthen the game " +
                "as the player attempts to understand the nature of the maze and how to get " +
                "through it.\n";
            sList.Add(s);

            s = "Imagine this - you are in Maze Location 1, and you can go north and east.  " +
                "Go north, and you come to Maze Location 2; go east, and you come to Maze " +
                "Location 3.  So far, simple.\n\n" +
                "Here comes the horrible bit.  In Maze Location 2, you can go south and up.  " +
                "But going south does not go to Maze Location 1.  Oh no.  Instead, you end up " +
                "in Maze Location 4.  And going up will take you to Maze Location 3.\n\n" +
                "And it gets worse.  In Maze Location 3, you can go west and down.  Going " +
                "down just puts you back in Maze Location 3.  Go west, and you go to Maze " +
                "Location 4.\n\n" +
                "(If you can follow all that, have a cookie.  You deserve it.)\n";
            sList.Add(s);

            s = "One way to map such a confusing mess is to go through the maze, and drop " +
                "an item in each location, to help identify where you are in the maze, and " +
                "build up a map.\n\n" +
                "But even as early as Zork, developers were wise to this trick, and " +
                "introduced a thief NPC, who would steal things you dropped in its maze.\n\n" +
                "If a maze can't be mapped and defeated by dropping items in each location, " +
                "then it is highly likely that getting through the maze will need a " +
                "different approach.  Perhaps there's an item in the game world that will " +
                "help you through it.\n";
            sList.Add(s);

            s = "This book proudly brought to you by Bludgeoning the Fourth Wall (tm) Hint " +
                "Productions.\n";
            sList.Add(s);

            _mazeBookSequence = new TextSequence(sList);
        }

        private static void populateTribeVisitSequence()
        {
            string s;
            List<string> sList;

            sList = new List<string>();

            s = "At the west end of the Sloping Passage, there are a couple of people, " +
                "dressed in a very bizarre style.  They're wearing what you might call " +
                "\"tribal-ish\" clothes, with a hodgepodge of furs and teeth and claws and " +
                "loincloths and such.  But in addition to all the archetypical tribal-ish " +
                "gear, they're wearing bits of smashed-up crockery and twisted forks and " +
                "bent spoons.  For weapons, they're wielding kitchen knives that look " +
                "quite incredibly sharp.\n";
            sList.Add(s);

            s = "As you approach, in your purloined shrine outfit, their eyes light up " +
                "and they start talking excitedly, though you can't understand a word they're " +
                "saying.\n\n" +
                "After a few moments, they seem to recognise that you can't understand " +
                "them.  Rather than getting suspicious, one of them turns, and beckons you " +
                "to come with her.  You follow her into a large cavern.\n";
            sList.Add(s);

            s = "In the middle of the large cavern is a hole.  You look through it and can " +
                "see a lot of sharp looking stalagmites below.\n\n" +
                "But the main obvious feature of this cavern is that it's filled with " +
                "people.  Most of them are dressed in what you would think of 'typical " +
                "tribal-ish' gear, with furs and teeth and claws, and not very much.  But " +
                "some, who look like guards, or more important figures, are wearing the " +
                "'smashed-up crockery armor' look.\n\n" +
                "When the people see you all chatter stops immediately - they stare at you " +
                "openmouthed.  Some of them drop to their knees and start kowtowing.\n";
            sList.Add(s);

            s = "The guard takes you to one of the more important looking figures and talks " +
                "at length.  The important-looking figure is a large fat man with a headband " +
                "made of spoons twisted together, which have been polished to an amazing " +
                "shine.  He nods, and then dismisses the guard with a wave.  She goes back " +
                "to the cavern entrance.\n\n" +
                "The man turns to you, and gets out an old, yellowing piece of paper, " +
                "studies it carefully, and says, in halting pronunciation, \"Wun Kahp Shoo-" +
                "gar?\"\n";
            sList.Add(s);

            s = "He sees your expression and looks happy and launches into a stream of " +
                "gibberish, none of which you understand.  When he realises you aren't " +
                "understanding him, he looks at you quizzically, and consults his paper " +
                "again.  \"Too Stikk But-tah?  Hahf kahp mllk?  Phoower Aigs?\"\n\n" +
                "\"One cup sugar?\" you ask.  \"Two sticks of butter?  Half a cup of milk? " +
                "Four eggs?  What is this?\"\n\n" +
                "\"Ah, so even the gods must modernise,\" the man says sadly.  \"But I " +
                "suppose it makes sense.  After all, we don't actually use 'wunhahfkahpphaloower'" +
                "or 'tuhereekahpcuhohcualahtte' anymore.  So, welcome to your humble servants, " +
                "Wunkahpshoogar!\"  He turns to the crowd and shouts.  \"The god walks " +
                "amongst us!\"\n\n" +
                "A great party ensues.\n";
            sList.Add(s);

            //postmove of tribal location
            // if first visited, change description to include massive party

            _tribeFirstVisitSequence = new TribeVistSequence(sList);

        }

        private static void populateEndSequence()
        {
            string s;
            List<string> sList;

            sList = new List<string>();

            s = "You go to the sage first.  She nods, calmly and serenely accepting the news." +
                "  \"Thank you for all your efforts.  But I shall not return to my old life.  " +
                "I feel much calmer now.  To the one who cursed us here... I feel... regret.  " +
                "I also, regretfully, feel anger.\n\n" +
                "\"Besides, I'm also a bit too old now to go jumping and climbing around old " +
                "caves and tombs and whatnot.\"\n";
            sList.Add(s);

            s = "You consider how best to tell the Crockery Tribe.  How would they handle the " +
                "truth, to learn that their gods were never real?  That they were stuck in the " +
                "cave for so long because of Dotty's mistake?\n\n" +
                "You go to them in costume and spin them a story of god fighting god for their " +
                "freedom, and you give them the Recipe - their Holy Words - back to the " +
                "Leader.\n\n" +
                "\"I think perhaps we no longer need those Holy Words, Wunkahpshoo-gar.  They " +
                "have caused us far too much trouble.\"\n\n" +
                "\"That's probably for the best,\" you say diplomatically.\n";
            sList.Add(s);

            s = "They were the easy ones.\n";
            sList.Add(s);

            s = "You take Dotty's cake recipe back to her.  All she wanted was a treasured " +
                "memento of a mother long gone, of her childhood, back again.  But it was " +
                "denied to her, for so incredibly long, and in her anger and desperation she " +
                "caused so much anguish and despair for so many " +
                "people in the Tiny Cave.\n\n" +
                "\"I'm not sure what, exactly, you deserve, Dotty.  But one thing you should " +
                "definitely have is your mother's recipe back,\" you say.  You press the recipe " +
                "into her hands.\n";
            sList.Add(s);

            s = "You don't even know the Crazy Old Guy's name, or anything you can do for him.\n\n" +
                "\"I see now you were only trying to help, to warn me... thank you.\"\n";
            sList.Add(s);

            s = "\"Oorgh... oh, wow, my mind feels so clear now!  You know what it's like to be " +
                "affected by two mind-affecting enchantments at the same time?  For 50 years?  " +
                "It's murder on the old brain is what it is.\"\n\n" +
                "\"You...you're alive?\"\n\n" +
                "\"Of course I'm alive, I'm talking to you right now, aren't I?  Help me up, " +
                "goshdarnit!  My back is murder after a Lobster " +
                "transformation.  You got any idea what it's like to turn into a giant enemy " +
                "lobster when you're over 70?  It hits your joints in the weak spots for " +
                "massive damage.\"\n\n" +
                "He gets up and decides not to.  \"And get me some goshdurned pants too.\"\n";
            sList.Add(s);

            s = "YOU HAVE FINISHED.\n\n" +
                "Thanks for playing!  Your score is 4 shcmaltzy scenes out of 4.\n";
            sList.Add(s);


            _endSequence = new EndingSequence(sList);

        }

        public static void initialisePlayer()
        {
            string tempstring = "";

            _player = new Player();
            _player.sState = "";
            _player.sName = "player";

            // Restrictions
            _player.bCanMove = true;
            _player.bCanTake = true;
            _player.bCanDrop = true;
            _player.bCanWear = true;
            _player.bCanRemove = true;
            _player.bCanPutIn = true;
            _player.bCanGetOut = true;
            _player.bCanTalk = true;
            _player.bCanUse = true;

            _player.sMoveTypes = "standard";
            //_player.sMoveTypes = "";
            //_player.sCantMoveMsg = "Sorry, you're not being allowed to move right now.";


            // Other things
            _player.CurrentConversation = null;
            _player.Inventory.Clear();
            _player.Add(_head);

            _player.bReceivedSagesClue = false;
            _player.bUsedSagesClue = false;
            _player.bCanParkour = false;
            _player.iParkourTrainingLevel = 0;
            _player.bCanLeave = false;
            _player.bHasLobsterClue = false;


            // Normal start

            _player.CurrentLocation = _FrontDoor;
            _player.bTiedUp = false;
            _player.iCarrySize = 0;


            // debug code, to set up various testing states

            // _player.CurrentLocation = _highLedge;
            // _player.Add(World._CarryBag);

            //_player.CurrentLocation = _holyBasketSite;
            //_centralCavern.Add(_chunkOfStalactite);
            //_centralCavern.Add(_chunkOfStalagmite);
            //_player.bTiedUp = true;
            //_player.TieUp();
            //_player.sMoveTypes += ",parkour";

            //_BackDoor.sName = "North of ruins";
            //_BackDoor.sDescription = "You are standing in an open field, to the north of the " +
            //    "smoking ruins of a white house.  It is mostly charred wood and ash now.  " +
            //    "There used to be a garden here - the flowers and plants are scattered all over.";
            //_BackDoor.SouthLoc = _FrontDoor;

            //_questGiver.sName = "Dotty";
            //_questGiver.sIndefiniteName = "Dotty";
            //_questGiver.sDefiniteName = "Dotty";
            //_questGiver.sDescription = "The House Owner and Quest Giver is here, though she " +
            //    "at this point it's obvious " +
            //   "there's a lot more to her than a simple Quest Giver owning a white house.  " +
            //    "Let's call her The Witch.  Or better yet, let's call her by her name, Dotty.\n\n" +
            //    "Dotty is sprawled ungainly in the middle of the ruins of her house, very " +
            //    "very still.  Around her neck is a gold necklace with a vivid green gemstone.";
            //_questGiver.bCanTalkTo = false;
            //_BackDoor.Add(_lostGemNecklace);

            // World._centralCavern.AddMoveType(World._west, "locked");
            // World._centralCavern.AddMoveType(World._north, "blocked");

            //_player.CurrentLocation = _centralCavern;
            //_player.Add(World._parkourManual);
            //_player.sMoveTypes += ",parkour, climb";

            //_player.CurrentLocation = _abandonedShrineSite;
            //_abandonedShrineSite.Add(_ReturnsBox);
            //_abandonedShrineSite.Add(_infiniteCarryBag);

            // Figuring out new Pathways system
            // _player.CurrentLocation = _centralCavern;
            // _player.sMoveTypes = "standard";
            // _centralCavern.SetPathway("parkour", World._south, World._treasureCave);
            
            //_player.CurrentLocation = _stalagmiteCave;
            //_player.sMoveTypes = "standard";
            


            // Past prologue
            
            //_player.bTiedUp = true; // This is for when you first get to the cave
            //_player.iCarrySize = 1; // You can only carry one thing at a time
            //                        // set to 0 for infinite carry space
            //_player.CurrentLocation = _stalagmiteCave;
            

            // Central Cavern
            /*
            _player.bTiedUp = true;
            _player.iCarrySize = 1;
            _player.CurrentLocation = _centralCavern;
            _highLedge.Remove(_crazyGuy);
            _centralCavern.Add(_crazyGuy);
            _crazyGuyConversation.iNode = 4;
            _mazeEntrance.bDiscovered = true;
            _mazeEntrance.bVisited = true;
            _mazeEntrance.PostMove(_highLedge, null, ref tempstring);
            _stalagmiteCave.Add(World._tinnedFish);
            _stalagmiteCave.Add(World._bottledWater);
            World._highLedge.NorthLoc = World._maze;

            _player.Add(World._chunkOfStalagmite);
            _player.Add(World._chunkOfStalactite);
            _player.Add(World._tinnedFish);
            _player.bTiedUp = false;
            _player.iCarrySize = 0;
            _player.Add(World._bottledWater);
            // _player.Add(World._sachet);
            _player.bKnowAdditionalLocations = true;
            _player.Add(_map);
            _caveEntrance.bEnteredCave = true;
            
            _questGiver.bCompletedFetchQuest = true;
            _questGiver.bGivenCakeQuest = true;
            _player.Add(_recipe);
            _player.bHasLobsterClue = true;
            _player.CurrentLocation = _BackDoor;
            _player.Add(_XXiumSaw);
            _ImpassableMountains.SoutheastLoc = _caveEntrance;
            _player.bCanParkour = true;
            _centralCavern.SouthLoc = _treasureCave;
            _treasureCave.NorthLoc = _centralCavern;
            */
            /*

           _player.CurrentLocation = _sageGrotto;
           _sage.bGreedClueGiven = true;
           _sage.bGreedClueUsed = true;
           _sage.bClueGiven = true;
           _sage.bClueUsed = true;
           _player.bUsedSagesClue = true;
           _player.bTiedUp = true;
           _player.iSore = 15;
            * */

            // Testing restriction system
            /*
            _player.bCanUse = false;
//            _player.sCantUseMsg = "You aren't being allowed to use anything by some mysterious force.";
            _player.sCantUseMsg = "You aren't being allowed to use [item] by some mysterious force.";
            _FrontDoor.Add(_vendingMachine);
            _vendingMachine.bUsableAnyway = true;
            _head.bUsableAnyway = true;

            _player.bCanTake = true;
            _player.sCantTakeMsg = "You can't pick up [item] because of reasons.";

            _player.bCanDrop = false;
            _player.sCantDropMsg = "You aren't being allowed to drop [item] by some mysterious force.";

            _player.bCanMove = true;
            _player.sCantMoveMsg = "You can't move anywhere.";
            _player.bCanGetOut = true;
            _player.sCantGetOutMsg = "Some mysterious force is preventing you from getting [item1] from [item2].";

            _player.bCanPutIn = true;
            _player.sCantGetOutMsg = "Some mysterious force is preventing you from putting [item1] into [item2].";
            _FrontDoor.Add(_tribalHeadgear);
            _player.bCanRemove = true;
            _player.sCantRemoveMsg = "Some mysterious force is preventing you from removing [item].";
            _player.bCanWear = true;
            _player.sCantWearMsg = "Some mysterious force is preventing you from putting [item] on.";

            _player.bCanTalk = true;
            _player.sCantTalkMsg = "Some mysterious force is preventing you from talking to [item].";
            */
        }

        // 6/6/2017 - Bug 10 - The Pathway system wasn't working if locked or blocked is in the
        //                     MovementTypes.  Also fixing up some formatting.

        public static void MoveTo(Location TargetLocation, Direction dir, bool Suppress, ref String OutMessage, ref bool bSuccess)
        // Moving in direction dir to TargetLocation
        // Suppress = if true, do not put out the "You go to X" message.
        // OutMessage = what message to display
        // bSuccess = was the move successful or not?
        {
            Pathway pathToMove;
            string s;
            string HasTypes;
            string[] FullNeededTypes; // includes locked and blocked

            bool bProceed = true;
            // string sNope;

            string sNope2;
            int lastCommaIndex;
            int commaCount;


            // =========================================================================
            // Pre-Move checks

            // Is the action restricted?
            if (World._player.bCanMove == false)
            {
                OutMessage += World._player.sCantMoveMsg + "\n";
                return;
            }
            
            if ( (World._player.sMoveTypes == "") ||
                 (World._player.sMoveTypes == "none")
                )
            {
                OutMessage += World._player.sCantMoveMsg + "\n";
                return;
            }

            // 22/5/2017 - Bug 3 - Changes to MoveTo() to improve the messaging to the player if the
            // player does not have the required movement types.
            //
            // The new Pathway stuff.
            // The first if is a version switch - ie don't bother doing the pathway stuff if the
            // current location's Pathways aren't implemented.

            if (World._player.CurrentLocation.Pathways.Count > 0)
            {
                pathToMove = World._player.CurrentLocation.Pathways.Find(x => (x.dir == dir));

                if (pathToMove != null)
                {
                    // Determine the Movement types you need for the movement
                    s = pathToMove.sMovementTypes.ToLower();
                    if (s == "")
                    {
                        s = "standard";
                    }
                    s = s.Replace(" ", "");
                    FullNeededTypes = s.Split(',');

                    // Determine the Movement types you actually have
                    HasTypes = World._player.sMoveTypes.ToLower();
                    HasTypes = HasTypes.Replace(" ", "");

                    // Because I'll probably forget and create lists of movement types both with spaces
                    // after commas, and without spaces after commas, I strip spaces from s and hasTypes.

                    // For each required movement type, see if the player has that movement type
                    // (Basically, is it in HasTypes?)
                    // If not, then disallow the movement
                    // At the same time, sNope2 is being built up as a list of movement types that
                    // the player needs, that will be displayed to the player later. 

                    // sNope = "";
                    bProceed = true;
                    sNope2 = "";

                    foreach (string RequiredType in FullNeededTypes)
                    {
                        if (HasTypes.IndexOf(RequiredType) == -1)
                        {
                            bProceed = false;
                            if ((RequiredType != "blocked") && (RequiredType != "locked"))
                            {
                                sNope2 += ", " + RequiredType;
                            }
                        }
                    }

                    // if sNope2 has stuff in it - that is, the player isn't allowed to go
                    // that way - fix up the commas and "ands" in sNope2 to make it more
                    // natural sounding.
                    if (sNope2 != "")
                    {
                        sNope2 = sNope2.Substring(2);

                        commaCount = 0;
                        commaCount = sNope2.Count(c => c == ',');

                        // if there is one comma, replace it with "and"
                        if (commaCount == 1)
                        {
                            sNope2 = sNope2.Replace(", ", " and ");
                        }

                        // if there's more than one comma, find the location of the last ", ",
                        // and replace it with ", and ".

                        if (commaCount > 1)
                        {
                            lastCommaIndex = sNope2.LastIndexOf(',');
                            sNope2 = sNope2.Substring(0, (lastCommaIndex + 1)) + " and " +
                                     sNope2.Substring((lastCommaIndex + 2));
                        }
                    }

                    // If the movement isn't allowed, put the standard message into OutMessage.
                    if (bProceed == false)
                    {
                        if ((s.IndexOf("blocked") != -1) || 
                            (FullNeededTypes.Contains("blocked"))
                            )
                        {
                            OutMessage += "The way is blocked.\n";
                        }
                        else if ( (s.IndexOf("locked") != -1) ||
                                  (FullNeededTypes.Contains("locked"))
                                )
                        {
                            OutMessage += "The way is locked.\n";
                        }
                        else
                        {
                            OutMessage += "You need to be able to " + sNope2 + " to go that way.\n";
                        }
                        // However don't return yet, we still need to call the location's PreMove()
                        // method.  This may give a custom OutMessage.
                    }
                }
            }

            // Run location-specific code to check if the move is okay.
            // This might override the standard "The way is locked", "The way is blocked", or
            // "You need to be able to X to go that way" text.
            bSuccess = false;
            _player.CurrentLocation.PreMove(TargetLocation, ref OutMessage, ref bSuccess);
            if (bSuccess == false)
            {
                return;
            }

            // if after all the Pathway stuff, bProceed is still false, return
            if (bProceed == false)
            {
                return;
            }

            // Standard locked and blocked messages.
            // (Old movement system)

            if (TargetLocation == _locked)
            {
                OutMessage += "The way is locked.\n";
                bSuccess = false;
                return;
            }
            if (TargetLocation == _blocked)
            {
                OutMessage += "The way is blocked.\n";
                bSuccess = false;
                return;
            }

            // =========================================================================
            // Actually move

            // Generic message, OK you're moving to the new location.
            if ((Suppress == false) && (TargetLocation.bSuppressGoMsg == false))
            {
                OutMessage += "You go to " + TargetLocation.sName + ".\n";
            }

            World._player.CurrentLocation = TargetLocation;
            TargetLocation.bDiscovered = true;
            bSuccess = true;

            // =========================================================================
            // Post-Move stuff

            // If you want to fire off some specific code when you move to a location,
            // but before bVisited is true, then use location.PostMove();
            TargetLocation.PostMove(_player.CurrentLocation, dir, ref OutMessage);

            // If this is the first visit, output the sFirstVisitText (if there is any)
            // Can sometimes be handy, especially if you want to draw the attention to a
            // detail, but don't want to bludgeon the player with it every time they enter
            // a location.

            if (TargetLocation.bVisited == false)
            {
                TargetLocation.bVisited = true;
                if (TargetLocation.sFirstVisitText != "")
                {
                    OutMessage += "\n" + TargetLocation.sFirstVisitText + "\n";
                }
            }

        }

        public static void PreAction(ref string OutMessage)
        // Anything that should happen before the player's turn should go here
        {
            // e.g. a condition might deteriorate
            // in here you would do condition--; if condition < 0 then condition = 0
            // regular action might increase it
            // then in postaction if the condition gets to X you do something

            // another example: if a guard is going to turn around every 5 turns you
            // can keep the counter here.

            // if lots of location-specific checks start showing up here, then that'll be
            // a case for a Location.PreAction method.
        }

        public static void PostAction(ref string OutMessage)
        // Anything that should happen after the player's turn should go here
        // Note that this occurs after the action has completed, whether it was
        // successful or not, and its OutMessage is independent from the regular
        // action's OutMessage.
        // 24/5/2017 - Enhancements 1+5 - Add and remove climb based on whether the player is tied up
        //             or carrying anything.
        {
            // e.g. if a guard is going to turn around you can do that here

            // another example - if the player is in a LocationGroup you might want to do
            // something.  If the player is in Tribal Village and not wearing the Tribal
            // mask, check for it here and if so then toss them out.


            // Another example - debug messages...
            /*
            if (_stalagmiteBase == null)
            {
               OutMessage += "Stalagmite base is null\n";
            }
            else
            {
                OutMessage += "Stalagmite base is not null.  hiOwner is " +
                    _stalagmiteBase.hiOwner.sName + "\n";
            }
             * */
            /*
            OutMessage += "AllLocations has: ";
            foreach(Location l in AllLocations.LocationList)
            {
                OutMessage += l.sName + ", ";
            }
            OutMessage += "that's it.\n";
            */
            /*
            OutMessage += "hiOwner of recipe is " + World._recipe.hiOwner.ToString() +
                "and toString does this: \"" + World._recipe.ToString() + "\"\n";
            */


            // Player starts off in the cave feeling quite raw and sore, due to being hit and
            // dumped into the stalagmite cave.  Later on they'll get dumped into the s'mite 
            // cave again.  Reduce iSore to 1 over time.
            if (_player.iSore > 1)
            {
                _player.iSore--;
            }

            // Check for tribal gear
            if ((World._tribalCostume.bWorn) && (World._tribalHeadgear.bWorn))
            {
                // Open up the way to the tribal village
                World._slopingPassage.WestLoc = World._tribalCavern;
            }
            else
            {
                //Close the way to the tribal village
                World._slopingPassage.WestLoc = World._blocked;
            }

            /*
            OutMessage += "Test:" +
                "blahfoobar".IndexOf("blah") + " " +
                "blahfoobar".IndexOf("foo") + " " +

                "blahfoobar".IndexOf("bar") + " " +
                "blahfoobar".IndexOf("why of course") + " " +
                "\n";
             */ 

            // Example of using restriction system
            // Based on a particular state you can set if the player can perform various actions
            // Though given the length of this it'd be better to put it into a TieUp method
            // on the player.
            // Check on TiedUp status
            /*
            if (_player.bTiedUp)
            {
                _player.bCanMove = true;
                _player.bCanTake = true;
                _player.bCanDrop = true;
                _player.bCanPutIn = true;
                _player.bCanGetOut = true;
                _player.bCanTalk = true;

                _player.iCarrySize = 1;
                _player.sCantCarryMoreMsg = "With your hands tied up, you can only carry one " +
                    "thing at a time, and only very awkwardly.";

                _player.bCanWear = false;
                _player.bCanRemove = false;
                _player.sCantWearMsg = "You can't put anything on or take anything off with your hands tied up.";
                _player.sCantRemoveMsg = "You can't put anything on or take anything off with your hands tied up.";

                _player.bCanUse = false;
                _player.sCantUseMsg = "You can't use [item] with your hands tied up.";

                _head.bUsableAnyway = true;
                _vendingMachine.bUsableAnyway = true;
                _MazeBook.bUsableAnyway = true;
                _bottledWater.bUsableAnyway = true;
                _WallMap.bUsableAnyway = true;
                _PaperAndStationeryKit.bUsableAnyway = true;
                _XXiumSaw.bUsableAnyway = true;
                _map.bUsableAnyway = true;
                _abstractDesigns.bUsableAnyway = true;
                _sachet.bUsableAnyway = true;

            }
            else
            {
                _player.bCanMove = true;
                _player.bCanTake = true;
                _player.bCanDrop = true;
                _player.bCanPutIn = true;
                _player.bCanGetOut = true;
                _player.bCanTalk = true;
                _player.bCanWear = true;
                _player.bCanRemove = true;
                _player.bCanUse = true;

                _player.iCarrySize = 0;
            }
            */

            // Check on status of main cake quest
            if (World._player.HasItem(World._recipe))
            {
                World._questGiver.bCompletedCakeQuest = true;
            }
            else
            {
                World._questGiver.bCompletedCakeQuest = false;
            }

            // Adding and removing climb status
            if ((World._player.carrying() == 0) && (World._player.bTiedUp == false))
            {
                // add climb to player movements
                if (World._player.sMoveTypes.Contains("climb") == false)
                {
                    // World._player.sMoveTypes += ",climb";
                    World._player.AddMoveType("climb");
                }
                // OutMessage += "Player's move types are now " + World._player.sMoveTypes + "\n";
            }
            else
            {
                // remove climb from player movements
                if (World._player.sMoveTypes.Contains("climb"))
                {
                    // World._player.sMoveTypes = World._player.sMoveTypes.Replace(",climb", "");
                    World._player.RemoveMoveType("climb");
                }
                // OutMessage += "Player's move types are now " + World._player.sMoveTypes + "\n";
            }


            // Do any location specific post-actions.
            _player.CurrentLocation.PostAction(ref OutMessage);

            // Test
            //foreach (Direction d in AllDirections)
            //{
            //    OutMessage += d.sName + " ";
            //}
            //OutMessage += "\n";

            /*
            Pathway p = World._centralCavern.Pathways.Find(x => x.dir == World._south);

            OutMessage += "(1) central cavern south movement types: " + p.sMovementTypes + "\n";

            World._centralCavern.AddMoveType(World._south, "locked");

            OutMessage += "(2) central cavern south movement types: " + p.sMovementTypes + "\n";

            World._centralCavern.AddMoveType(World._south, "blocked");

            OutMessage += "(3) central cavern south movement types: " + p.sMovementTypes + "\n";

            World._centralCavern.RemoveMoveType(World._south, "blocked");

            OutMessage += "(4) central cavern south movement types: " + p.sMovementTypes + "\n";

            World._centralCavern.RemoveMoveType(World._south, "locked");

            OutMessage += "(5) central cavern south movement types: " + p.sMovementTypes + "\n";
         
            World._centralCavern.AddMoveType(World._south, "parkour");

            OutMessage += "(6) central cavern south movement types: " + p.sMovementTypes + "\n";

            World._centralCavern.RemoveMoveType(World._south, "fly");

            OutMessage += "(7) central cavern south movement types: " + p.sMovementTypes + "\n";
            */

            /*
            _player.AddMoveType("fly");

            OutMessage += "(1) player movement types: " + _player.sMoveTypes + "\n";

            _player.AddMoveType("parkour");

            OutMessage += "(2) player movement types: " + _player.sMoveTypes + "\n";

            _player.RemoveMoveType("teleport");

            OutMessage += "(3) player movement types: " + _player.sMoveTypes + "\n";

            _player.RemoveMoveType("parkour");

            OutMessage += "(4) player movement types: " + _player.sMoveTypes + "\n";
            */

            //string s = ",blah,blah,,blah,blah,,,";
            //s = s.Trim(',');
            //s = s.Replace(",,", ",");
            //OutMessage += s + "\n";

            // Bodgy serialisation test
            /*
            FileStream writer = new FileStream("C:\\Users\\Steven\\Documents\\Visual Studio 2013\\Projects\\Tiny Cave Adventure\\temp.xml", FileMode.Create, FileAccess.Write);
//            DataContractSerializer serializer = new DataContractSerializer(typeof(HasInventory));
            DataContractSerializer serializer = new DataContractSerializer(typeof(HasInventory));
            using (writer)
            {
                // So I had the idea to simply serialize and deserialize the world class
                // However, it's a static class, so that doesn't make sense.
                // Make it a singleton and it might yet make sense... but then there is so much
                // code that refers to World.this and World.that.
                // serializer.WriteObject(writer, World.);

                
                // serializer.WriteObject(writer, _player);
                // serializer.WriteObject(writer, _blocked);
                // serializer.WriteObject(writer, _locked);
                // serializer.WriteObject(writer, _EastOfHouse);
                // serializer.WriteObject(writer, _ImpassableMountains);
                // serializer.WriteObject(writer, _FrontDoor);
                

                // When I check the XML I can see full items being written out multiple times
                // I don't quite get what's going on yet.
                // Not even sure if DataContractSerializer is the right thing to use!  I was
                // trying to figure out how to serialize references, it was one of the first
                // things that came up in my searches, and was most understandable.

//                serializer.WriteObject(writer, _FrontDoor);

//                foreach(Location l in AllLocations.LocationList)
//                {
//                    serializer.WriteObject(writer, l);
//                }
            }
            */

            // Bodgy deserialisation test
            //            FileStream reader = new FileStream("C:\\Users\\Steven\\Documents\\Visual Studio 2013\\Projects\\Tiny Cave Adventure\\temp.xml", FileMode.Open, FileAccess.Read);
            //            DataContractSerializer serializer = new DataContractSerializer(typeof(HasInventory));
            //            using (reader)
            //            {
            //                _stalagmiteCave = (Location)serializer.ReadObject(reader);
            //            }

            // Not much luck with serialisation yet.

        }

        public static void MakeRuins()
        {
            _FrontDoor.sName = "South of ruins";
            _FrontDoor.sDescription = "You are standing in an open field, to the south of the " +
                "smoking ruins of a white house.  It is mostly charred wood and ash now, " +
                "drifting in the wind.  A lonely mailbox stands here.";
            _FrontDoor.NorthLoc = _BackDoor;

            _WestOfHouse.sName = "West of ruins";
            _WestOfHouse.sDescription = "To the east are the smoking ruins of a white house, all " +
                "charred wood and ash now.  Smoking remains of washing lines and clothes are " +
                "strewn amongst the trees.";
            _WestOfHouse.EastLoc = _EastOfHouse;

            _EastOfHouse.sName = "East of ruins";
            _EastOfHouse.sDescription = "To the west are the smoking ruins of a white house, all " +
                "charred wood and ash now.";
            _EastOfHouse.WestLoc = _WestOfHouse;

            _BackDoor.sName = "North of ruins";
            _BackDoor.sDescription = "You are standing in an open field, to the north of the " +
                "smoking ruins of a white house.  It is mostly charred wood and ash now.  " +
                "There used to be a garden here - the flowers and plants are scattered all over.";
            _BackDoor.SouthLoc = _FrontDoor;

            _questGiver.sName = "Dotty";
            _questGiver.sIndefiniteName = "Dotty";
            _questGiver.sDefiniteName = "Dotty";
            _questGiver.sDescription = "The House Owner and Quest Giver is here, though she " +
                "at this point it's obvious " +
                "there's a lot more to her than a simple Quest Giver owning a white house.  " +
                "Let's call her The Witch.  Or better yet, let's call her by her name, Dotty.\n\n" +
                "Dotty is sprawled ungainly in the middle of the ruins of her house, very " +
                "very still.  Around her neck is a gold necklace with a vivid green gemstone.";
            _questGiver.bCanTalkTo = false;
            _BackDoor.Add(_lostGemNecklace);

            _crazyGuy.sDescription = "The Crazy Old Guy is here.  The bandage over his right " +
                "arm is gone, as are most of his clothes - you can see that his hand wasn't " +
                "just swollen and misshapen - it was a lobster claw.  He is lying very very " +
                "still.";
            _crazyGuy.bCanTalkTo = false;

            _lobster.hiOwner.Add(_crazyGuy);
            _lobster.hiOwner.Remove(_lobster);
            _lobster.hiOwner = null;
        }


        public static string Hint()
        {
            string s = "";

            if (_caveEntrance.bVisited == false)
            {
                s = "Right now you're in the beginning part of the game, and you haven't even " +
                    "reached the Tiny Cave.  Don't worry, you'll get there.\n\n" +
                    "When the game started, you were in front of a house.  Explore the area, " +
                    "see what's around, " +
                    "find the owner of the house, and get her quests.  In this prologue, " +
                    "everything is pretty simple, and the most difficult thing you have to do " +
                    "is a fetch quest.";
                return s;
            }

            if ( (_highLedge.bVisited == false)
                )
            {
                s = "So now you're in the cave itself.  That was a rude introduction to the " +
                    "cave, wasn't it?\n\n" +
                    "You'll see in your available actions that you can jump on various " +
                    "things.  You'll also periodically see the Crazy Old Guy show up at the top " +
                    "of the rockface.  Try to figure out how he's moving and it should become " +
                    "clear what you need to do to get up to the next location.";
                return s;
                
            }

            if ( (_highLedge.bVisited) && (_mazeEntrance.bVisited == false) )
            {
                s = "Not much to do here!  Just talk to the guy.  He's trying to be helpful." +
                    "  He's just strange about it.";
                return s;
            }
                 

            if ( (_specialMazeLocation.bVisited == false) &&
                 (_sage.bClueGiven == false)
                )
            {
                s = "So you've gone through the maze once and you're in the central cavern.  " +
                    "Explore the area and see what you can find.  Most importantly, go visit " +
                    "maze and try going through it.  The book at the Maze Entrance tells you " +
                    "a bit about mazes in adventure games - so while in the maze, try " +
                    "dropping something, and see what happens.\n\n" +
                    "In your search through the Tiny Cave, you can find a Wise Old Sage.  " +
                    "It might seem that all she says is weird ravings that make no sense, " +
                    "but a lot of what she says is important.. including a hint as to how " +
                    "to get through the maze.";
                return s;
            }

            if ((_specialMazeLocation.bVisited == false) &&
                 (_sage.bClueGiven)
                )
            {
                s = "Hill and dale and blahblahblah... what a weird thing for the sage to " +
                    "say.  But you've seen all these things before.  Where?  Think on that, " +
                    "and the way through the maze will become clearer.";
                return s;
            }

            if ((_specialMazeLocation.bVisited) &&
                 (_sage.bClueGiven) &&
                 (_lobster.hiOwner == null)
                )
            {
                s = "You made it to the maze's wall map.  Congratulations!  That was probably " +
                    "the hardest puzzle.  Everything else is mostly straightforward now.\n\n" +
                    "The sage has one more cryptic clue to help you out of the cave, but it's " +
                    "not nearly as hard to understand as her first cryptic clue.";

                return s;
            }

            if (_lobster.hiOwner != null)
            {
                s = "The Lobster has been unleashed.\n\n" +
                    "In typical end-of-game fashion, there is an item you may have seen but " +
                    "not used yet.  Go get it.  It might seem completely useless, you may " +
                    "be wondering how it'll help against " +
                    "a giant enemy lobster, but trust me, it's the item you need.";
                return s;
            }

            s = "If you're seeing this hint, then something has gone wrong.  You have a new " +
                "quest - you must contact the programmer, at once!";
            return s;
        }
    }
    /*

    public static class ReflectionUtility
    {
        public static IEnumerable<Type> GetDerivedTypes(this Type baseType, Assembly assembly)
        {
            var types = from t in assembly.GetTypes()
                        where t.IsSubclassOf(baseType)
                        select t;

            return types;
        }
    }
     * */
}
