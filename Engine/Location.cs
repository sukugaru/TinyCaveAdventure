using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Engine
{
    [DataContractAttribute(IsReference=true)]
    public class Location : HasInventory
    // Most of the time you can just use the standard Location class, but if you want to
    // override the locked and blocked exit descriptions, create a subclass and inherit from
    // Location.  TestLocation_Location is a class, immediately below, that demonstrates this.
    {
        [DataMember()]
        public string sDescription { get; set; }

        [DataMember()]
        public bool bDiscovered { get; set; }       // If not discovered, the name will not show up in the command line

        [DataMember()]
        public bool bVisited { get; set; }          // Use to trigger 'first visit' descriptions.

        [DataMember()]
        public string sFirstVisitText { get; set; } // Optional.  Will display the first time
                                                    // you visit a location.

        [DataMember()]
        public bool bSuppressGoMsg { get; set; }    // Defaults to false.  If true, then the
                                                    // standard "you go to X" message will not appear.


        [DataMember()]
        public Location NorthLoc { get; set; }

        [DataMember()]
        public Location NortheastLoc { get; set; }

        [DataMember()]
        public Location EastLoc { get; set; }

        [DataMember()]
        public Location SoutheastLoc { get; set; }

        [DataMember()]
        public Location SouthLoc { get; set; }

        [DataMember()]
        public Location SouthwestLoc { get; set; }

        [DataMember()]
        public Location WestLoc { get; set; }

        [DataMember()]
        public Location NorthwestLoc { get; set; }

        [DataMember()]
        public Location UpLoc { get; set; }

        [DataMember()]
        public Location DownLoc { get; set; }

        // Replacement of Northloc Westloc etc. etc.
        // If a Location has no Pathways then the old Northloc Westloc etc. code will be used
        // If there are Pathways declared then those will be used to determine available directions
        // etc.
        [DataMember()]
        public List<Pathway> Pathways;

        [DataMember()]
        public List<Action> LocationActions;


        public Location()
        // Objects are instantiated and constructed first, then locations
        // A location, if it has a constructor, would be expected to add the required objects
        {
            bDiscovered = false;
            bVisited = false;
            sFirstVisitText = "";
            bSuppressGoMsg = false;

            NorthLoc = null;
            NortheastLoc = null;
            EastLoc = null;
            SoutheastLoc = null;
            SouthLoc = null;
            SouthwestLoc = null;
            WestLoc = null;
            NorthwestLoc = null;
            UpLoc = null;
            DownLoc = null;

            Pathways = new List<Pathway>();

            LocationActions = new List<Action>();
            LocationActions.Clear();

            World.AllLocations.LocationList.Add(this);
        }

        public virtual void LinkLocation()
        // After all the Location constructors have been called, their LinkLocation
        // methods are called
        {

        }

        public string Exits()
        // Returns a generic list of ways you can go.  Not used by current graphical UI as
        // exit locations are all already shown.
        // Would be used in an old-style parser + single screen UI.
        {
            string s = "";
            // string s2 = "";
            int count = 0;
            int count2 = 0;

            count = 0;
            foreach (Direction d in World.AllDirections)
            {
                if ((d.TargetLocation(this) != World._blocked) &&
                     (d.TargetLocation(this) != World._locked) &&
                     (d.TargetLocation(this) != null)
                    )

                { count++; }
            }

            // s = "Exits.  Count is " + count.ToString() + ".  ";
            if (count > 0)
            {
                count2 = 0;
                foreach (Direction d in World.AllDirections)
                {
                    if ((d.TargetLocation(this) != World._blocked) &&
                         (d.TargetLocation(this) != World._locked) &&
                         (d.TargetLocation(this) != null)
                       )
                    {
                        count2++;
                        // s += "(Count2 is " + count2.ToString() + ") ";
                        if (s == "")
                        {
                            s = "You can go " + d.sName;
                        }
                        else
                        {
                            if (count2 < count)
                            {
                                s += ", " + d.sName;
                            }
                            else
                            {
                                s += ", and " + d.sName + ".";
                            }
                        }
                    }
                }
            }
            else
            {
                s = "You cannot go in any direction.";
            }

            return s;
        }

        public virtual string BlockedExits()
        // Returns a generic list of blocked directions.  Graphical UI puts it in the Location
        // window but it can get cramped.
        // For greater fun, create a subclass and override BlockedExits to give a more
        // specific description about blocked exits.
        {
            string s = "";
            int count = 0;
            int count2 = 0;

            count = 0;
            foreach (Direction d in World.AllDirections)
            {
                if (d.TargetLocation(this) == World._blocked) { count++; }
            }

            if (count > 0)
            {
                count2 = 0;
                foreach (Direction d in World.AllDirections)
                {
                    if (d.TargetLocation(this) == World._blocked)
                    {
                        count2++;
                        if (s == "")
                        {
                            s = "To the " + d.sName;
                        }
                        else
                        {
                            // s += "(count2 is " + count2.ToString() + " and count is " + count.ToString() + ") ";
                            if ((count == 2))
                            {
                                s += " and " + d.sName;
                            }
                            if ((count2 < count) && (count > 2))
                            {
                                s += ", " + d.sName;
                            }
                            if ((count2 == count) && (count > 2))
                            {
                                s += ", and " + d.sName + ",";
                            }

                        }
                    }
                }
                s += " the way is blocked.";
            }


            return s;
        }

        public virtual string LockedExits()
        // Returns a generic list of locked directions.  Graphical UI puts it in the Location
        // window but it can get cramped.
        // For greater fun, create a subclass and override LockedExits to give a more
        // specific description about locked exits.
        {
            string s = "";
            int count = 0;
            int count2 = 0;

            count = 0;
            foreach (Direction d in World.AllDirections)
            {
                if (d.TargetLocation(this) == World._locked) { count++; }
            }

            if (count > 0)
            {
                count2 = 0;
                foreach (Direction d in World.AllDirections)
                {
                    if (d.TargetLocation(this) == World._locked)
                    {
                        count2++;
                        if (s == "")
                        {
                            s = "To the " + d.sName;
                        }
                        else
                        {
                            // s += "(count2 is " + count2.ToString() + " and count is " + count.ToString() + ") ";
                            if ((count == 2))
                            {
                                s += " and " + d.sName;
                            }
                            if ((count2 < count) && (count > 2))
                            {
                                s += ", " + d.sName;
                            }
                            if ((count2 == count) && (count > 2))
                            {
                                s += ", and " + d.sName + ",";
                            }
                        }
                    }
                }
                s += " the way is locked.";

            }

            return s;
        }

        // Either create the specified pathway, or update the existing pathway for the given
        // pDir.
        public void SetPathway(string psMoveTypes, Direction pDir, Location pTarget)
        {
            Pathway _pathToUpdate = null;

            foreach (Pathway p in Pathways)
            {
                if (p.dir == pDir)
                {
                    _pathToUpdate = p;
                }
            }

            // Add
            if (_pathToUpdate == null)
            {
                _pathToUpdate = new Pathway();
                _pathToUpdate.sMovementTypes = psMoveTypes;
                _pathToUpdate.dir = pDir;
                _pathToUpdate.TargetLocation = pTarget;
                Pathways.Add(_pathToUpdate);

            }

            // And now update
            _pathToUpdate.sMovementTypes = psMoveTypes;
            _pathToUpdate.dir = pDir;
            _pathToUpdate.TargetLocation = pTarget;

            
        }

        // For the given pathway, clear it.  Can be done just by setting the target to null.
        public void ClearPathway (Direction pDir)
        {
            // Can't use LinQ as I want to modify the Pathway.  I've found before that
            // if you use LinQ to get an item from a List, then you can't modify that item.
            // Pathway p = Pathways.Find(x => (x.dir == pDir));

            foreach (Pathway p in Pathways)
            {
                if (p.dir == pDir)
                {
                    p.TargetLocation = null;
                }
            }
        }

        public void DetermineAvailableDirections(ref List<Direction> pDirectionList)
        {
            pDirectionList.Clear();

            // Determine and display directions

            if (Pathways.Count == 0)
//            if (Pathways == null)
            {
                // Old system
                foreach (Direction d in World.AllDirections)
                {
                    if (d.TargetLocation(World._player.CurrentLocation) != null)
                    {
                        pDirectionList.Add(d);
                    }
                }
            }
            else
            {
                // New Pathways system
                // All pathways get added, regardless of the movement types for that pathway
/*                foreach (Pathway p in Pathways)
                {
                    if (p.TargetLocation != null)
                    {
                        pDirectionList.Add(p.dir);
                    }
                } */
                // Instead of a foreach (Pathway p in Pathways), doing the following way to 
                // keep locations in the right order.
                foreach (Direction d in World.AllDirections)
                {
                    if (Pathways.Find(x => (x.dir == d)) != null)
                    {
                        pDirectionList.Add(d);

                    }
                }

            }
        }

        public virtual void PreMove(Location ToLocation, ref string OutMessage, ref bool bSuccess)
        // Called by World._MoveTo.
        // Use if you want a location-specific check to run before going to ToLocation
        // e.g. if you're about to leave a house you could put a check to make sure everything's
        // locked up, and if not, set bSuccess to false.
        // e.g. if you want some sort of custom "go to location" message you can put it here too.
        // VERY VERY COMMON PROBLEM (I keep on making this mistake):  Putting in a PreMove to
        // handle one particular thing, and forgetting to set bSuccess to true at the end.
        {
            bSuccess = true;
        }

        public virtual void PostMove(Location FromLocation, Direction direction, ref string OutMessage)
        // Called by World._MoveTo.
        // Use if you want a location-specific check to run after moving to it.
        // Differs from PostAction is that at this point we know FromLocation and direction,
        // and bVisited is still false.  It runs only once per location.
        { }

        public virtual void PostAction(ref string OutMessage)
        // Called by World._PostAction.
        // Use if you want some sort of location-specific code to run after the player's
        // action has completed.  Will run for every action that ends with the player in 
        // location.  e.g. if a player goes to the chieftan's hut, then removes
        // the Tribal Mask, you could have some code here that kills him for his impunity.
        { }



    }


    // Location specific classes
    // Are in the same .cs file because these are pretty small, only making one or two overrides
    // each.
    // Well, they were.  Some of them, in particular maze_Location, are getting quite big.

    [DataContractAttribute(IsReference = true)]
    public class baseMaze_Location : Location
    {
        public baseMaze_Location() : base()
        {

        }

        public void MoveToReturnsBox()
        // Move any dropped items back to the Entrance
        {
            List<Engine.Object> tempList = new List<Engine.Object>();
            int count;

            tempList.Clear();
            if (Inventory.Count(x => (x.bStaysInMaze == false)) > 0)
            {
                count = Inventory.Count();
                World._maze.iNumOfDroppedItems += count;

                // Put Note into Returns Box now, so it's at the top of the list
                if (World._ReturnsBox.HasItem(World._Note) == false)
                {
                    World._ReturnsBox.Add(World._Note);
                }

                // Actually move items
                // Need to build up tempList first, because you can't change the contents of
                // Inventory while iterating through it.
                foreach (var item in Inventory)
                {
                    tempList.Add(item);
                }
                foreach (var item in tempList)
                {
                    if (item.bStaysInMaze == false)
                    {
                        World._ReturnsBox.Add(item);
                        Inventory.Remove(item);
                    }
                }

                // Make adjustments to wording on note and name of returns box.
                World._Note.sDescription = "The note says:\n\"RETURNS BOX\nWe noticed you left ";
                if (World._maze.iNumOfDroppedItems > 1)
                {
                    World._Note.sDescription += "some things ";
                }
                else
                {
                    World._Note.sDescription += "something ";
                }
                World._Note.sDescription += "in the maze, so we are returning your things to you.\n - The Managament\"";

                World._ReturnsBox.sName = "Returns Box";
                World._ReturnsBox.sDefiniteName = "the Returns Box";
                World._ReturnsBox.sIndefiniteName = "a Returns Box";
                World._ReturnsBox.sDescription = "There is a large returns box here, half as large as you.  A note is taped to it.";

            }
        }
    }

    [DataContractAttribute(IsReference = true)]
    public class maze_Location : baseMaze_Location
    {
        [DataMember()]
        public int moves;

        [DataMember()]
        public int iNumOfDroppedItems;

        [DataMember()]
        public int pathProgress;

        [DataMember()]
        public List<Direction> path = new List<Direction>();

        public maze_Location() : base()
        {
            sName = "Maze";
            sDescription = "This is a maze of twisty little tunnels and passages, all alike.";
            bDiscovered = false;

            moves = 0;
            pathProgress = 0;
            iNumOfDroppedItems = 0;
            bSuppressGoMsg = true;

            path.Add(World._south);
            path.Add(World._west);
            path.Add(World._east);
            path.Add(World._north);
            path.Add(World._up);

        }

        public override void LinkLocation()
        {
            NorthLoc = this;
            NortheastLoc = this;
            EastLoc = this;
            SoutheastLoc = this;
            SouthLoc = this;
            SouthwestLoc = this;
            WestLoc = this;
            NorthwestLoc = this;
            UpLoc = this;
            DownLoc = this;
        }

        public override void PreMove(Location ToLocation, ref string OutMessage, ref bool bSuccess)
        {
            if (ToLocation == this)
            {
                OutMessage += "You walk through the twisty little tunnels and passages and come to another junction in this very confusing maze.\n";
            }
            if (ToLocation == World._mazeEntrance)
            {
                OutMessage += "You walk through the twisty little tunnels and passages and end up back at the maze's entrance.\n";
            }
            if (ToLocation == World._specialMazeLocation)
            {
                OutMessage += "You walk through the twisty little tunnels and passages and end up at a Special Maze Location.\n";
            }

            bSuccess = true;
        }

        public override void PostMove(Location FromLocation, Direction dir, ref string OutMessage)
        {
            Engine.Object i;
            List<Engine.Object> tempList = new List<Engine.Object>();
            int count;

            if (FromLocation == World._mazeEntrance)
            {
                OutMessage += "You enter the maze.\n";
                World._Note.sDescription = "Thank you for not cluttering up our maze!\n - The Management.";

            }

            moves++;

            // Move any dropped items back to the Entrance
            MoveToReturnsBox();

            // If you know the way and you go in the right directions, eventually you come
            // to... something
            if ( (World._player.bReceivedSagesClue) &&
                 (pathProgress < path.Count()) &&
                 (dir == path[pathProgress])
                )
            {
                pathProgress++;

                // OK, this next bit is SUPER KLUDGY and very hardcoded.  I'll think of something
                // better eventually.  I hope.
                
                if (pathProgress == 4)
                {
                    World._specialMazeLocation.bDiscovered = false;
                    UpLoc = World._specialMazeLocation;
                }
            }
            else
            {
                pathProgress = 0;
            }

            // After 5 moves in the maze, if you're not on the right path, you automatically
            // end up back at the Entrance
            if ((moves >= 5) && (pathProgress == 0))
            {
                World._mazeEntrance.bDiscovered = false;

                NorthLoc = World._mazeEntrance;
                SouthLoc = World._mazeEntrance;
                WestLoc = World._mazeEntrance;
                EastLoc = World._mazeEntrance;
                NortheastLoc = World._mazeEntrance;
                SoutheastLoc = World._mazeEntrance;
                NorthwestLoc = World._mazeEntrance;
                SouthwestLoc = World._mazeEntrance;
                UpLoc = World._mazeEntrance;
                DownLoc = World._mazeEntrance;
            }
        }

        public override void PostAction(ref string OutMessage)
        {
            // Suppress the "(to maze)" bit of the command line
            bDiscovered = false;

            // Suppress the normal "You go to X" message for the maze entrance and
            // the special maze location(s).
            World._mazeEntrance.bSuppressGoMsg = true;
            World._specialMazeLocation.bSuppressGoMsg = true;
        }

        
    }

    [DataContractAttribute(IsReference = true)]
    public class mazeEntrance_Location : baseMaze_Location
    {
        public mazeEntrance_Location() : base()
        {
            sName = "Maze Entrance";
            sDescription = "To the west is a maze, one of the most dreaded puzzles in all Adventuredom!";
            sFirstVisitText = "In a corner you can see an old discarded book.";
            bDiscovered = false;
            bSuppressGoMsg = true;                  // After you get led through the maze the
                                                    // first time, will be set to false.
            Add(World._ReturnsBox);
            Add(World._MazeBook);
        }

        public override void LinkLocation()
        {
            WestLoc = World._maze;
            EastLoc = World._centralCavern;

        }

        public override void PostMove(Location FromLocation, Direction dir, ref string OutMessage)
        {
            // After going through the maze and coming back to the Maze Entrance, reset the maze
            World._maze.pathProgress = 0;
            World._maze.moves = 0;
            World._maze.iNumOfDroppedItems = 0;
            bSuppressGoMsg = false;

            World._maze.NorthLoc = World._maze;
            World._maze.SouthLoc = World._maze;
            World._maze.EastLoc = World._maze;
            World._maze.WestLoc = World._maze;
            World._maze.NortheastLoc = World._maze;
            World._maze.SoutheastLoc = World._maze;
            World._maze.SouthwestLoc = World._maze;
            World._maze.NorthwestLoc = World._maze;
            World._maze.UpLoc = World._maze;
            World._maze.DownLoc = World._maze;


        }
    }

    [DataContractAttribute(IsReference=true)]
    public class specialMaze_Location : baseMaze_Location
    {
        public specialMaze_Location()
            : base()
        {
            sName = "Map Room";
            sDescription = "This is less of a room and more an extra wide part of the " + 
                "endless, confusing passages.  Someone has scribbled all over the walls here.";
            bDiscovered = false;
            Add(World._WallMap);
        }

        public override void LinkLocation()
        {
            NorthLoc = World._maze;
            SouthLoc = World._maze;
            EastLoc = World._maze;
            WestLoc = World._maze;
            NortheastLoc = World._maze;
            SoutheastLoc = World._maze;
            SouthwestLoc = World._maze;
            NorthwestLoc = World._maze;
            UpLoc = World._maze;
            DownLoc = World._maze;
        }

        public override void PreMove(Location ToLocation, ref string OutMessage, ref bool bSuccess)
        {
            if (ToLocation == World._maze)
            {
                OutMessage += "You walk through the twisty little tunnels and passages and come to another junction in this very confusing maze.\n";
            }
            if (ToLocation == World._mazeEntrance)
            {
                OutMessage += "You walk through the twisty little tunnels and passages and end up back at the maze's entrance.\n";
            }

            // If you dropped anything, make sure it gets back to the Returns Box
            MoveToReturnsBox();

            bSuccess = true;
        }


        public override void PostMove(Location FromLocation, Direction direction, ref string OutMessage)
        {
            // Reset the path
            World._maze.pathProgress = 0;
            World._maze.moves = 0;
            this.bSuppressGoMsg = true;

            // SpecialMazeLocation might get used by a number of locations, so make sure
            // all directions are set back to _maze.
            World._maze.NorthLoc = World._maze;
            World._maze.SouthLoc = World._maze;
            World._maze.EastLoc = World._maze;
            World._maze.WestLoc = World._maze;
            World._maze.NortheastLoc = World._maze;
            World._maze.SoutheastLoc = World._maze;
            World._maze.SouthwestLoc = World._maze;
            World._maze.NorthwestLoc = World._maze;
            World._maze.UpLoc = World._maze;
            World._maze.DownLoc = World._maze;

            // You now recognise the 'strange abstract drawings' as maps of the maze
            World._abstractDesigns.sDescription = "You recognise the strange abstract " +
                "designs on the walls of the passage as a partial map of the Tiny " +
                "Cave's maze.";

            // You've used the sage's clue.
            // Got a couple of variables for this so will set all of them and optimise later
            World._player.bUsedSagesClue = true;
            World._sage.bClueUsed = true;
        }
    }


    [DataContractAttribute(IsReference = true)]
    public class HolyBasket_Location : baseMaze_Location
    {
        public HolyBasket_Location()
            : base()
        {
            sName = "\"Holy Basket\" site";
            sDescription = "This is less of a room and more an extra wide part of the " +
                "endless, confusing passages.  On a raised stone platform are the remains of " +
                "an old picnic basket.";
            bDiscovered = false;
            Add(World._holyBasket);
        }

        public override void LinkLocation()
        {
            NorthLoc = World._maze;
            SouthLoc = World._maze;
            EastLoc = World._maze;
            WestLoc = World._maze;
            NortheastLoc = World._maze;
            SoutheastLoc = World._maze;
            SouthwestLoc = World._maze;
            NorthwestLoc = World._maze;
            UpLoc = World._maze;
            DownLoc = World._maze;
        }

        public override void PreMove(Location ToLocation, ref string OutMessage, ref bool bSuccess)
        {
            if (ToLocation == World._maze)
            {
                OutMessage += "You walk through the twisty little tunnels and passages and come to another junction in this very confusing maze.\n";
            }
            if (ToLocation == World._mazeEntrance)
            {
                OutMessage += "You walk through the twisty little tunnels and passages and end up back at the maze's entrance.\n";
            }

            // If you dropped anything, make sure it gets back to the Returns Box
            MoveToReturnsBox();

            bSuccess = true;
        }


        public override void PostMove(Location FromLocation, Direction direction, ref string OutMessage)
        {
            // Reset the path
            World._maze.pathProgress = 0;
            World._maze.moves = 0;
            this.bSuppressGoMsg = true;

            // SpecialMazeLocation might get used by a number of locations, so make sure
            // all directions are set back to _maze.
            World._maze.NorthLoc = World._maze;
            World._maze.SouthLoc = World._maze;
            World._maze.EastLoc = World._maze;
            World._maze.WestLoc = World._maze;
            World._maze.NortheastLoc = World._maze;
            World._maze.SoutheastLoc = World._maze;
            World._maze.SouthwestLoc = World._maze;
            World._maze.NorthwestLoc = World._maze;
            World._maze.UpLoc = World._maze;
            World._maze.DownLoc = World._maze;

        }
    }

    [DataContractAttribute(IsReference = true)]
    public class AbandonedShrine_Location : baseMaze_Location
    {
        public AbandonedShrine_Location()
            : base()
        {
            sName = "Abandoned Shrine";
            sDescription = "This is less of a room and more an extra wide part of the " +
                "endless, confusing passages, given over to some abandoned shrine you don't " +
                "quite understand.";
            bDiscovered = false;
            Add(World._abandonedShrine);
            Add(World._tribalCostume);
            Add(World._tribalHeadgear);
        }

        public override void LinkLocation()
        {
            NorthLoc = World._maze;
            SouthLoc = World._maze;
            EastLoc = World._maze;
            WestLoc = World._maze;
            NortheastLoc = World._maze;
            SoutheastLoc = World._maze;
            SouthwestLoc = World._maze;
            NorthwestLoc = World._maze;
            UpLoc = World._maze;
            DownLoc = World._maze;
        }

        public override void PreMove(Location ToLocation, ref string OutMessage, ref bool bSuccess)
        {
            if (ToLocation == World._maze)
            {
                OutMessage += "You walk through the twisty little tunnels and passages and come to another junction in this very confusing maze.\n";
            }
            if (ToLocation == World._mazeEntrance)
            {
                OutMessage += "You walk through the twisty little tunnels and passages and end up back at the maze's entrance.\n";
            }

            // If you dropped anything, make sure it gets back to the Returns Box
            MoveToReturnsBox();

            bSuccess = true;
        }


        public override void PostMove(Location FromLocation, Direction direction, ref string OutMessage)
        {
            // Reset the path
            World._maze.pathProgress = 0;
            World._maze.moves = 0;
            this.bSuppressGoMsg = true;

            // SpecialMazeLocation might get used by a number of locations, so make sure
            // all directions are set back to _maze.
            World._maze.NorthLoc = World._maze;
            World._maze.SouthLoc = World._maze;
            World._maze.EastLoc = World._maze;
            World._maze.WestLoc = World._maze;
            World._maze.NortheastLoc = World._maze;
            World._maze.SoutheastLoc = World._maze;
            World._maze.SouthwestLoc = World._maze;
            World._maze.NorthwestLoc = World._maze;
            World._maze.UpLoc = World._maze;
            World._maze.DownLoc = World._maze;

        }
    }


    [DataContractAttribute(IsReference = true)]
    public class stalagmiteCave_Location : Location
    {
        [DataMember()]
        public bool bClimbedYet { get; set; }

        public stalagmiteCave_Location()
        {
            sName = "Stalagmite cave";
            sDescription = "This is less of a cave and more of a deep hole.  " +
                "To one side is a massive wall.  At its base is a small open area, and all " +
                "around you, receding into the gloom, is a forest of stalgmites, some small, " +
                "some massive, and some broken.  High above you can see a small hole, " +
                "allowing some light into the cavern.";
            bDiscovered = true;
            bClimbedYet = false;

            Add(World._chunkOfStalagmite);
            Add(World._chunkOfStalactite);
            Add(World._stalagmiteBase);
            Add(World._boulder);
            Add(World._ledge);
            Add(World._precariousPlatform);
        }

        public override void LinkLocation()
        {
            SetPathway("climb", World._up, World._highLedge);

        }

        public override void PostMove(Location FromLocation, Direction direction, ref string OutMessage)
        {
            if ((FromLocation == World._highLedge) &&
                 (direction == World._down) &&
                 (bClimbedYet == false) 
                )
            {
                OutMessage += "With your hands free you can finally easily climb down to the Stalagmite Cave.\n";
                bSuppressGoMsg = false;
                bClimbedYet = true;
            }
            if (World._player.bBeingSacrificed)
            {
                World._player.bBeingSacrificed = false;
            }
        }

        public override void PostAction(ref string OutMessage)
        {
            // At the start of the game, before you reachthe High Ledge, the Crazy Old Guy is
            // patrolling along the High Ledge.  This bit handles that.
            if (World._highLedge.bVisited == false)
            {
                //OutMessage += "COG position: " + World._crazyGuy.iLedgePosition.ToString() + 
                //    "   and direction: " + World._crazyGuy.iDirection.ToString() +
                //    "\n"; 
                // If he's at the edge, there's lots of things he can do.
                if (World._crazyGuy.iLedgePosition == 1)
                {
                    OutMessage += "At the top of the rockface a man appears - details are " +
                        "hard to make out at this distance but it's pretty obvious he has " +
                        "a big crazy white beard and big crazy white hair.";

                    // Most annoyingly, he'll knock you back down to the ground if you're
                    // trying to jump up to the ledge.
                    if (World._player.sState.StartsWith("On "))
                    {
                        OutMessage += "  \"Hey!\" he shouts.  \"Stop jumping around like that!  " +
                            "You'll just get yourself killed!\"  He follows up by throwing a " +
                            "rock at you, which knocks you off your perch and crashing back " +
                            "down to the rocky ground below.\n\n" +
                            "Ouch.\n";
                        World._crazyGuy.bKnockedPlayerDown = true;
                        World._player.sState = "";
                    }
                    else
                    {
                        // If you're not jumping around, he'll be a bit more helpful, and
                        // throw you some food.
                        if (World._crazyGuy.bDispensedFood == false)
                        {
                            if (World._crazyGuy.bKnockedPlayerDown)
                            {
                                OutMessage += "  \"Oh good, you're staying down there,\" he " +
                                    "calls.  \"It's for your own good you know!\"  Here!";
                            }
                            else
                            {
                                OutMessage += "  \"Hey down there!\" he calls.  \"Welcome to " +
                                    "the cave!";
                            }

                            OutMessage += "  You're going to need these!\"  He follows up " +
                                "throwing some tinned fish and bottled water down to you.\n";
                            this.Add(World._tinnedFish);
                            this.Add(World._bottledWater);
                            World._crazyGuy.bDispensedFood = true;

                        }
                        else
                        {
                            OutMessage += "  \"Oh good, you're staying down there,\" he " +
                                "calls.  \"It's for your own good you know!\"\n";
                        }
                    }

                    // He's at the edge, so he's going to head towards the other end of the ledge
                    World._crazyGuy.iDirection = 1;
                }

                // move him along
                World._crazyGuy.iLedgePosition += World._crazyGuy.iDirection;

                if ( (World._crazyGuy.iLedgePosition == 2) &&
                     (World._crazyGuy.iDirection == 1)
                    )
                {
                    OutMessage += "\nHe moves away from the edge and disappears from sight.\n";
                }

                // turn him around at the other end
                if (World._crazyGuy.iLedgePosition == 6)
                {
                    World._crazyGuy.iDirection = -1;
                }
            }
        }
    }

    [DataContractAttribute(IsReference = true)]
    public class highLedge_Location : Location
    {
        [DataMember()]
        public bool bClimbedYet { get; set; }

        public highLedge_Location()
        {
            sName = "High ledge";
            sDescription = "You are at the top of a high ledge.  Above you you can " +
                "you can see a small hole, allowing some light into this massive cavern, down " +
                "below you can see stalagmites everywhere, and all around you you can see " +
                "stalactites jabbing down from the ceiling.  To the north there is a narrow " +
                "crack in the wall.";
            bDiscovered = false;
            Add(World._crazyGuy);
            bClimbedYet = false;
        }

        public override void LinkLocation()
        {
            NorthLoc = World._mazeEntrance;         // After the crazy old guy leads you
                                                    // through the maze the first time, this'll
                                                    // get set to _maze.
        }

        public override void PreMove(Location ToLocation, ref string OutMessage, ref bool bSuccess)
        {

            if (ToLocation == World._mazeEntrance)
            {
                if (World._crazyGuy.bFollowingThroughMaze == false)
                {
                    OutMessage += "You try squeezing through the crack, but it's very tight " +
                        "and it branches into a dizzying maze of very narrow cracks.  You " +
                        "beat a hasty retreat and go back to the ledge.\n";
                    bSuccess = false;
                    return;
                }
                else
                {
                    // Only want to do the following bit once.
                    if (World._crazyGuy.bFollowedThroughMaze == false)
                    {
                        OutMessage += "You squeeze through the crack, following the old man.  It " +
                            "is *very* tight and it branches into a dizzying maze of narrow " +
                            "and twisty passages and it all looks the same.  Eventually he leads you " +
                            "out of the maze and you arrive at the maze's entrance.\n\n" +
                            "\"Toodles!\" he says.  \"Maybe you won't actually kill yourself.  " +
                            "Let's find out!\"  With that he wanders eastwards.\n";

                        World._centralCavern.Add(World._crazyGuy);

                        bSuccess = true;
                        World._highLedge.NorthLoc = World._maze;
                        World._crazyGuy.bFollowedThroughMaze = true;

                        // The Maze Entrance location's PostAction will set its bsuppressgomsg
                        // to false
                    }
                    else
                    {
                        OutMessage += "You squeeze through the narrow crack and into the maze, " +
                            "then walk through the twisty little tunnels and passages back to " +
                            "the maze's entrance.\n";

                        bSuccess = true;
                    }
                }
            }
            if (ToLocation == World._maze)
            {
                OutMessage += "You squeeze through the narrow crack and into the maze.\n";
            }

            bSuccess = true;
        }


        public override void PostMove(Location FromLocation, Direction dir, ref string OutMessage)
        {
            // This is an entry and exit point for the maze, so reset it
            World._maze.pathProgress = 0;
            World._maze.moves = 0;
            World._maze.iNumOfDroppedItems = 0;
            bSuppressGoMsg = false;
        }

        public override void PostAction(ref string OutMessage)
        {
            // The first time you make it here, a bit of flavour text with the Crazy Old Guy
            // And let's stop him patrolling.  He knows he can't stop you anymore.
            if ( (World._crazyGuy.iDirection != 0) &&
                 (World._crazyGuy.hiOwner == this)
                )
            {
                OutMessage += "You see the crazy old guy hurrying towards you, carrying a " +
                    "rock.  He sees you jump up to the ledge and sighs.  \"Well, so much " +
                    "for that then,\" he says, and throws the rock over the side of the " +
                    "ledge.  \"So you're going to want to kill yourself, I see,\" he says.\n";
                World._crazyGuy.iDirection = 0;
                World._crazyGuy.iLedgePosition = 2;
            }

        }
    }

    [DataContractAttribute(IsReference = true)]
    public class frontDoor_Location : Location
    {
        public frontDoor_Location() : base()
        {
            sName = "Front of house";
            sDescription = "You are standing in an open field south of a white " +
                "house, with a boarded up front door.  There is a mailbox here.";
            bDiscovered = true;
            Add(World._mailbox);
        }

        public override void LinkLocation()
        {
             NorthwestLoc = World._WestOfHouse;
             SouthLoc = World._ForestPath;
             NortheastLoc = World._EastOfHouse;
             NorthLoc = World._blocked;

            SetPathway("", World._northwest, World._WestOfHouse);
            SetPathway("", World._south, World._ForestPath);
            SetPathway("", World._northeast, World._EastOfHouse );
            SetPathway("blocked", World._north, World._blocked);

        } 

        public override string BlockedExits()
        {
            if (NorthLoc == World._blocked)
            {
                return "To the north the front door is quite thoroughly blocked and boarded.";
            }
            else
            {
                return "";
            }
        }

        public override void PreMove(Location ToLocation, ref string OutMessage, ref bool bSuccess)
        {
            if (ToLocation == World._blocked)
            {
                bSuccess = false;
                OutMessage += "\"Go around the back!\" a voice calls.  \"The front door's " +
                    "as stuck and as blocked as a very very stuck thing!\"\n";
                return;
            }
            bSuccess = true;
        }
    }

    [DataContractAttribute(IsReference = true)]
    public class Forest1_Location : Location
    {
        public Forest1_Location() : base()
        {
            sName = "Forest";
            sDescription = "This is a forest, with trees in all directions.  To the " +
                "east, there appears to be sunlight.  To the west the forest gets much " +
                "thicker.";
            bDiscovered = false;
        }

        public override string BlockedExits()
        {
            return "To the west the trees become impassable.";
        }

        public override void LinkLocation()
        {
            EastLoc = World._WestOfHouse;
            SoutheastLoc = World._FrontDoor;
            NorthLoc = World._ForestClearing;
            WestLoc = World._blocked;
        }

    }

    [DataContractAttribute(IsReference = true)]
    public class Forest2_Location : Location
    {
        public Forest2_Location()
            : base()
        {
            sName = "Forest";
            sDescription = "This is a dimly lit forest, with large trees all around you.  To " +
                "the east is a scene of amazing devastation, with many large trees broken " +
                "and tossed around by an incredible storm.  Off to the west, there " +
                "appears to be sunlight.";
            sFirstVisitText = "Oh wow.  There are a lot of clothes scattered all over the place.";
            bDiscovered = false;
            Add(World._stormBlownClothes);
        }

        public override void PreMove(Location ToLocation, ref string OutMessage, ref bool bSuccess)
        {
            if (ToLocation == World._blocked)
            {
                bSuccess = false;
                OutMessage += "To the east your way is blocked by storm-tossed trees.\n";
                return;
            }
            bSuccess = true;
        }


        public override string BlockedExits()
        {
            return "";
        }

        public override void LinkLocation()
        {
            WestLoc = World._EastOfHouse;
            SouthLoc = World._ForestPath;
            NorthwestLoc = World._ForestClearing;
            EastLoc = World._blocked;
        }

    }

    [DataContractAttribute(IsReference = true)]
    public class ForestClearing_Location : Location
    {
        public ForestClearing_Location() : base()
        {
            sName = "Forest Clearing";
            sDescription = "You are in a small clearing at the junction of some well-marked " +
                "forest paths, leading to the east, west, and south.  There is a large tree " +
                "with low lying branches here.  To the north the forest gets much thicker.";
            bDiscovered = false;
        }

        public override string BlockedExits()
        {
            return "To the north the trees become impassable.";
        }

        public override void LinkLocation()
        {
            EastLoc = World._Forest2;
            SouthLoc = World._BackDoor;
            NorthLoc = World._blocked;
            WestLoc = World._Forest1;
            UpLoc = World._UpATree;
        }


    }

    [DataContractAttribute(IsReference = true)]
    public class ForestPath_Location : Location
    {
        public ForestPath_Location() : base()
        {
            sName = "Forest Path";
            sDescription = "This is a path winding through a dimly lit forest.  It leads " +
                "north and south, but the trees around here aren't all that thickly " +
                "spaced, and it looks like you could walk through them.";
            bDiscovered = false;

        }

        public override void LinkLocation()
        {
            EastLoc = World._Forest2;
            SouthLoc = World._ImpassableMountains;
            NorthLoc = World._FrontDoor;
            WestLoc = World._Forest1;
        }

    }

    [DataContractAttribute(IsReference = true)]
    public class WestOfHouse_Location : Location
    {
        public WestOfHouse_Location() : base()
        {
            sName = "West of House";
            sDescription = "You are facing the west side of a white house.  There are some " +
                "washing lines here, strung up between handy branches of the nearby trees.  " +
                "The west wall of the house has almost no windows, and a small door.";
            bDiscovered = false;
            Add(World._washingLine);
        }

        public override string LockedExits()
        {
            if (EastLoc == World._locked)
            {
                return "The door into the house is locked.";
            }
            else
            {
                return "";
            }
        }

        public override void LinkLocation()
        {
            EastLoc = World._locked;
            NortheastLoc = World._BackDoor;
            SoutheastLoc = World._FrontDoor;
            WestLoc = World._Forest1;
        }
    }

    [DataContractAttribute(IsReference = true)]
    public class EastOfHouse_Location : Location
    {
        public EastOfHouse_Location()
            : base()
        {
            sName = "East of House";
            sDescription = "You are facing the east side of a white house.  There's no door " +
                "here, and the windows are all firmly shut and with the curtains closed.";
            bDiscovered = false;
        }

        public override void LinkLocation()
        {
            NorthwestLoc = World._BackDoor;
            SouthwestLoc = World._FrontDoor;
            EastLoc = World._Forest2;
        }
    }

    [DataContractAttribute(IsReference = true)]
    public class BackDoor_Location : Location
    {
        public BackDoor_Location() : base()
        {
            sName = "Back door";
            sDescription = "You are at the back of the house.  There is a small garden here.  " +
                "Unlike the rest of the house, the back of the house is open and inviting, " +
                "with a welcome mat by the door, windows that open and let you see into the " +
                "house, and a door that is open.  The house owner is standing by the door.";
            bDiscovered = false;
            Add(World._questGiver);
        }

        public override string BlockedExits()
        {
            return "";
        }

        public override void PreMove(Location ToLocation, ref string OutMessage, ref bool bSuccess)
        {
            if (ToLocation == World._blocked)
            {
                bSuccess = false;
                OutMessage += "\"Nuh-uh,\" says the house owner, \"you're not coming in.  I " +
                    "know you adventurous sorts, you'd be taking all my stuff, entirely out " +
                    "of habit.\"\n";
                return;
            }
            bSuccess = true;
        }


        public override void LinkLocation()
        {
            SouthLoc = World._blocked;
            SouthwestLoc = World._WestOfHouse;
            NorthLoc = World._ForestClearing;
            SoutheastLoc = World._EastOfHouse;
        }
    }

    [DataContractAttribute(IsReference = true)]
    public class ImpassableMountains_Location : Location
    {
        public ImpassableMountains_Location() : base()
        {
            sName = "Impassable Mountains";
            sDescription = "To the south there are impassable mountains.  A pathway leads to " +
                "a forest to the north.  There are stunning valley views off to the west.";
            bDiscovered = false;
        }

        public override void LinkLocation()
        {
            NorthLoc = World._ForestPath;
        }

        
    }

    [DataContractAttribute(IsReference = true)]
    public class UpATree_Location : Location
    {
        public UpATree_Location() : base()
        {
            sName = "Up a tree";
            sDescription = "You are up in the low-lying branches of a particularly large " +
                "tree.  Beside you on one of the branches is a bird's nest.";
            bDiscovered = false;
            sFirstVisitText = "There are some pants, blown away during a storm and draped over the branches.";
            Add(World._nest);
            Add(World._stormBlownPants);
            World._nest.Add(World._ring);
        }

        public override void LinkLocation()
        {
            DownLoc = World._ForestClearing;
        }
    }

    [DataContractAttribute(IsReference = true)]
    public class CaveEntrance_Location : Location
    {
        [DataMember()]
        public bool bEnteredCave { get; set; }

        [DataMember()]
        public bool bTriedExiting { get; set; }

        public CaveEntrance_Location() : base()
        {
            sName = "Cave Entrance";
            sDescription = "This is the entrance into the infamous Tiny Cave.  Hardly anybody " +
                "who enters comes back.";
            bDiscovered = true;
            bEnteredCave = false;
            bTriedExiting = false;
        }

        

        public override void PostAction(ref string OutMessage)
        {
            bool bSuccess = false;
            
            if (bEnteredCave == false)
            {
                OutMessage += "As you enter the Tiny Cave, your eyes blink, adjusting to " +
                    "the dim light - you're probably going to need some sort of light " +
                    "source.  But before you can think much further on that, there is a " +
                    "very solid THUMP as you are hit in the back of the head, and you " +
                    "lose consciousness.\n\n" +
                    "You awake in a gloomy cavern, surrounded by stalagmites, and with your " +
                    "hands tied tightly together.  You are very feeling very sore, as if " +
                    "you were hit hard in the head, dragged for a while, and then dumped " +
                    "into this cave.\n\n";
                World._player.bTiedUp = true;
                World._player.TieUp();
                World._player.iSore = 15;
                bEnteredCave = true;

                // Move to the stalagmite cave and call postAction a second time, to kick off
                // the Crazy Old Guy's behaviour.
                World.MoveTo(World._stalagmiteCave, null, true, ref OutMessage, ref bSuccess);
                World.PostAction(ref OutMessage);
            }
          

            bSuccess = true;

        }

        public override string BlockedExits()
        {
            if (bTriedExiting)
                return "There is... something stopping you from exiting the cave.\n";
            else
                return "";
        }

        public override void PreMove(Location ToLocation, ref string OutMessage, ref bool bSuccess)
        {
            if (ToLocation == World._blocked)
            {
                OutMessage += "There is... something stopping you from exiting the cave.  " +
                    "You can see the way out just right in front of you, but no matter how " +
                    "much you try to exit, you simply don't move.\n";
                bTriedExiting = true;
                bSuccess = false;
                return;
            }
            bSuccess = true;
        }

        public override void LinkLocation()
        {
            NorthwestLoc = World._blocked;
            SouthLoc = World._centralCavern;
        }
    }

    [DataContractAttribute(IsReference = true)]
    public class CentralCavern_Location : Location
    {
        public CentralCavern_Location() : base()
        {
            sName = "Central Cavern";
            sDescription = "This is a large cavern, with tunnels and passages heading off in " +
                "many directions.  In particular, there is a long passageway to the south " +
                "and some sort of glow is coming from it, though the passage is made " +
                "impassable by a long and very dangerous-looking gap.";
            bDiscovered = false;
            Add(World._vendingMachine);

        }

        public override void LinkLocation()
        {
            SouthLoc = World._blocked;
            WestLoc = World._mazeEntrance;
            NortheastLoc = World._sageGrotto;
            SouthwestLoc = World._slopingPassage;
            NorthLoc = World._caveEntrance;
            EastLoc = World._tinyLedge;

            SetPathway("parkour", World._south, World._treasureCave);
            SetPathway("", World._west, World._mazeEntrance);
            SetPathway("", World._northeast, World._sageGrotto);
            SetPathway("", World._north, World._caveEntrance);
            SetPathway("", World._east, World._tinyLedge);
            SetPathway("", World._southwest, World._slopingPassage);

        }

        public override string BlockedExits()
        {
            return "";
        }

        public override void PreMove(Location ToLocation, ref string OutMessage, ref bool bSuccess)
        {
            bSuccess = true;

            if (World._player.sMoveTypes.IndexOf("parkour") == -1)
            {
                OutMessage += "There is absolutely no way you're jumping over that gap.  It's " +
                    "much too long.\n";
                bSuccess = false;
                return;
            }
            else
            {
                World._treasureCave.bSuppressGoMsg = true;
                OutMessage += "You Le Parkour over the gap and continue down the south " +
                    "passageway.\n";
                bSuccess = true;
                return;
            }

            if (ToLocation == World._blocked)
            {
                if (World._player.iParkourTrainingLevel != 300)
                {
                    OutMessage += "There is absolutely no way you're jumping over that gap.  It's " +
                        "much too long.\n";
                    bSuccess = false;
                    return;
                }
            }
            if (ToLocation == World._treasureCave)
            {
                World._treasureCave.bSuppressGoMsg = true;
                OutMessage += "You Le Parkour over the gap and continue down the south " +
                    "passageway.\n";
                bSuccess = true;
            }
        }

        public override void PostMove(Location FromLocation, Direction direction, ref string OutMessage)
        {
            bSuppressGoMsg = false;
        }
    }

    [DataContractAttribute(IsReference = true)]
    public class SageGrotto_Location : Location
    {
        public SageGrotto_Location() : base()
        {
            sName = "Sage's Grotto";
            sDescription = "This is a small grotto.  There are several candles, giving off " +
                "smoke and the heady, yet soothing, smell of incense.  In the middle of the " +
                "grotto is a sage, sitting calmly and serenely on the rocky ground.";
            bDiscovered = false;
            Add(World._sage);
        }

        public override void LinkLocation()
        {
            SouthwestLoc = World._centralCavern;
        }
    }

    [DataContractAttribute(IsReference = true)]
    public class SlopingPassage_Location : Location
    {
        public SlopingPassage_Location() : base()
        {
            sName = "Sloping Passage";
            sDescription = "This is a long passage, sloping upwards and to the west.  There " +
                "are strange abstract designs all over the walls.  There is also a warning " +
                "saying \"Danger - the tribe is ahead!!\"";
            bDiscovered = false;
            Add(World._abstractDesigns);
        }

        public override string BlockedExits()
        {
            return "";
        }

        public override void PreMove(Location ToLocation, ref string OutMessage, ref bool bSuccess)
        {
            bSuccess = true;
            if (ToLocation == World._blocked)
            {
                if ((World._player.HasItem(World._tribalCostume) && World._tribalCostume.bWorn) &&
                    (World._player.HasItem(World._tribalHeadgear) && World._tribalHeadgear.bWorn)
                    )
                {
                    /*
                    OutMessage += "Eh, I'm just getting bored of this now.\n\n" +
                        "Wearing the tribal costume you get past the guards and you find a " +
                        "tribal village that's grown up around the long-missing cake recipe " +
                        "and you take it and jump down through the hole in the floor and " +
                        "land in the Stalagmite Cave (yes again) and then you find your way " +
                        "to the cave entrance (and it only takes like five turns) " +
                        "and exit and take the recipe back to the Quest Giver and you win!  " +
                        "Yay!\n\n" +
                        "YOU HAVE WON\n" +
                        "(Sort of.)  Your score is 3.5 grapefruits out of a possible 4 watermelons!\n" +
                        "Next time, why don't you try...\n" +
                        "   ...parkouring up the chimney passage?\n" +
                        "   ...finding and lifting the curse on the Tiny Cave?\n" +
                        "   ...saving the Crazy Old Guy and the Sage?\n";

                    World._player.sState = "Finished";
                     * */
                }
                else
                {
                    OutMessage += "At the other end of the passage are a couple of people, " +
                        "dressed very oddly in armour made of cutlery and smashed-up pieces of " +
                        "crockery.  They look very odd, but the " +
                        "knives they threaten you with are very real and very sharp, and they " +
                        "chase you away.\n";
                    bSuccess = false;
                }
            }
            if (ToLocation == World._tribalCavern)
            {
                if ((World._player.HasItem(World._tribalCostume) && World._tribalCostume.bWorn) &&
                    (World._player.HasItem(World._tribalHeadgear) && World._tribalHeadgear.bWorn)
                    )
                {
                    if (World._tribalCavern.bVisited == false)
                    {
                        World._player.CurrentTextSequence = World._tribeFirstVisitSequence;
                    }
                    else
                    {
                        if (World._player.bSacrificed == false)
                        {
                            OutMessage += "As you pass the guards, you give them your best godly " +
                                "nod.  \"Welcome back, Wunkahpshoo-gar!\" say the guards.\n";
                        }
                        else
                        {
                            OutMessage += "As you pass the guards, you give them your best godly " +
                                "nod.  \"Welcome back to this mortal coil after your sacrifice, " +
                                "Wunkahpshoo-gar!\" say the gaurds.\n";
                        }
                        World._tribalCavern.bSuppressGoMsg = true;

                    }
                }
            }
        }

        public override void LinkLocation()
        {
            NortheastLoc = World._centralCavern;
            WestLoc = World._blocked;
        }
    }

    [DataContractAttribute(IsReference = true)]
    public class TribalCavern_Location : Location
    {
        [DataMember()]
        public bool bPartying { get; set; }

        public TribalCavern_Location() : base()
        {
            sName = "Tribal Cavern";
            sDescription = "This is a large cavern, with a hole in the floor and a tribal " +
                "village around it.";
            bDiscovered = false;
            bPartying = false;
            LocationActions.Add(World._DropThroughHole);

            Add(World._leader);
            Add(World._villagers);
            Add(World._dais);
            Add(World._beds);
            Add(World._firePits);
            Add(World._foodStores);

        }

        public override void PreMove(Location ToLocation, ref string OutMessage, ref bool bSuccess)
        {
            // Jumping down the hole is a no-no
            if (ToLocation == World._stalagmiteCave)
            {
                if (World._player.bBeingSacrificed == false)
                {
                    OutMessage += "While you could possibly survive the fall, it's a long way " +
                        "down and there are a lot of sharp stalagmites down there.  It's " +
                        "probably better that you don't jump down there, unless ABSOLUTELY " +
                        "NECESSARY.\n";
                    bSuccess = false;
                    return;
                }
            }

            // Trying to nick off without the recipe is also a no-no
            if ( (ToLocation == World._slopingPassage) && 
                 (World._player.HasItem(World._recipe)) )
                
            {
                if (World._player.bSacrificed == false)
                {
                    OutMessage += "While everyone else is partying like mad, the guards at the " +
                        "entrance to the cavern are on the ball, sober, and realise you have the " +
                        "Recipe with you.\n\n" +
                        "\"Stop!  You would take the very holy words from which we were created " +
                        "from us?  What kind of god are you?\"\n\n" +
                        "\"The Chief will decide what to do with you.\"\n\n" +
                        "The leader, once he's told of what just happened, absolutely FREAKS OUT.  " +
                        "\"AUIII!  Truly we are accursed!  No matter how much and how many we " +
                        "sacrifice to you, Wunkahpshoo-gar, you want more and more, and now you " +
                        "want the Holy Words?  Why don't we just sacrifice *you*?!?\"\n\n" +
                        "That gives him an idea.  " +
                        "\"Hey now.  Nothing else has worked.  And you're a god, so it won't be " +
                        "permanent.  Let's give it a shot!\"\n\n" +
                        "The Recipe is taken back from you and you are thrown into the hole in " +
                        "the middle of the cavern.  The stalagmites below rush towards you and " +
                        "you wonder if you are about to die, you land with a very painful " +
                        "CRUNCH, but miraculously you are alive and unbroken.\n";
                }
                else
                {
                    OutMessage += "The guards realise that once again you're trying to take the " +
                        "Recipe and leave.\n\n" +
                        "\"Wunkahpshoo-gar, we've been through this already.\"\n\n" +
                        "They take you to the leader, who wrings his hands and wails some more.  " +
                        "\"Wunkahpshoo-gar, you know how it is.  Since the curse hasn't lifted " +
                        "yet, we're going to sacrifice you again.\"\n\n" +
                        "\"Okay,\" you say.\n\n" +
                        "Once again the Recipe is taken back from you and you are thrown into " +
                        "the hole in the middle of the cavern again, and once again you land " +
                        "with a CRUNCH but no lasting injuries.\n";
                }
                World._player.bSacrificed = true;
                World._player.bBeingSacrificed = true;
                World._player.Remove(World._recipe);
                World._dais.Add(World._recipe);
                World._player.iSore = 15;
                World.MoveTo(World._stalagmiteCave, World._down, true, ref OutMessage, ref bSuccess);
                bSuccess = false;
                return;

            }

            // Trying to leave after dropping the recipe through the hole
            if ( (ToLocation == World._slopingPassage) &&
                 (World._stalagmiteCave.HasItem(World._recipe))
                )
            {
                OutMessage += "A great hue and cry goes up.  While everybody was partying like " +
                    "mad and they didn't notice you dropping the recipe through the hole in the " +
                    "floor, they have now noticed it is missing.  The leader absolutely " +
                    "FREAKS OUT.\n\n" +
                    "\"AUIII!  Truly we are accursed!  No matter how much and how many we " +
                    "sacrifice to you, Wunkahpshoo-gar, we remain stuck in this cave, " +
                    "unable to leave, and now even the " +
                    "Holy Words are gone!  ";

                if (World._player.bSacrificed == false)
                {
                    OutMessage += "Why don't we just sacrifice *you*?!?\n\n" +
                        "That gives him an idea.  " +
                        "\"Hey now.  Nothing else has worked.  And you're a god, so it won't be " +
                        "permanent.  Let's give it a shot!\"\n\n" +
                        "You are thrown into the hole in the middle of the cavern.  " +
                        "The stalagmites below rush towards you and you wonder if you are " +
                        "about to die, you land with a very painful CRUNCH, but miraculously " +
                        "you are alive and unbroken.\n";
                }
                else
                {
                    OutMessage += "Wunkahpshoo-gar, we'll just have to try sacrificing you " +
                        "again.\n\n" +
                        "Once again you are thrown into the hole in the middle of " +
                        "the cavern, and once you again with a CRUNCH but no lasting " +
                        "injuries.\n";
                }

                World._player.bSacrificed = true;
                World._player.bBeingSacrificed = true;
                World._player.iSore = 15;
                World.MoveTo(World._stalagmiteCave, World._down, true, ref OutMessage, ref bSuccess);
                bSuccess = false;
                return;
                
            }

            bSuccess = true;
        }

        public override void PostMove(Location FromLocation, Direction direction, ref string OutMessage)
        {
            // The standard go message gets suppressed a lot.  Resetting it anyway.
            bSuppressGoMsg = false;
        }

        public override void PostAction(ref string OutMessage)
        {
            if (bPartying)
            {
                sDescription = "This is a large cavern, with a hole in the floor and a tribal " +
                    "village around it.  There is a very large party going on.";

            }
        }

        public override void LinkLocation()
        {
            DownLoc = World._stalagmiteCave;
            EastLoc = World._slopingPassage;
        }

    }

    [DataContractAttribute(IsReference = true)]
    public class TreasureCave_Location : Location
    {
        public TreasureCave_Location() : base()
        {
            sName = "Treasure Cave";
            sDescription = "Ah, the fabled treasure and gold of the Tiny Cave!  It is a massive " +
                "hoard of gold, gemstones, ornate jewellery, and priceless artifacts.  The only " +
                "way in and out is the north passageway, but it has largely crumbled away, " +
                "leaving a large gap, completely impassable, well, unless you can wallrun.";
            bDiscovered = false;
            Add(World._treasure);
        }

        public override void LinkLocation()
        {
            // This gets populated later in game, once you learn how to parkour.

            SetPathway("parkour", World._north, World._centralCavern);

        }

        public override void PreMove(Location ToLocation, ref string OutMessage, ref bool bSuccess)
        {
            if (ToLocation == World._centralCavern)
            {
                World._centralCavern.bSuppressGoMsg = true;
                OutMessage += "You Le Parkour back over the gap to the main central cavern.\n";
                bSuccess = true;
            }
        }
        public override void PostMove(Location FromLocation, Direction direction, ref string OutMessage)
        {
            if (FromLocation == World._tinyLedge)
            {
                World._player.CurrentConversation = World._treasureConversation;
            }
        }
    }

    [DataContractAttribute(IsReference = true)]
    public class TinyLedge_Location : Location
    {
        [DataMember()]
        public bool bNothingnessLeap { get; set; }

        [DataMember()]
        public bool bDiscoveredTreasureCave { get; set; }

        public TinyLedge_Location() : base()
        {
            sName = "Tiny Ledge";
            sDescription = "This is a tiny ledge on the edge of a massive, empty, space.  You " +
                "can't see any walls or floor or ceiling, just a great big empty space in front " +
                "of you.  You instinctively take a step away from it, and wonder why, exactly, " +
                "this place is known as the Tiny Cave, when it has an empty space this " +
                "massive in it.";
            bDiscovered = false;
            bNothingnessLeap = false;
        }

        public override void LinkLocation()
        {
            WestLoc = World._centralCavern;
            DownLoc = World._blocked;
        }

        public override void PreMove(Location ToLocation, ref string OutMessage, ref bool bSuccess)
        {
            if (ToLocation == World._blocked)
            {
                OutMessage += "Oh, HECK NO.\n";
                bSuccess = false;
                return;
            }
            if (ToLocation == World._treasureCave)
            {
                if ( (World._sage.bGreedClueGiven) ||
                    (World._player.Inventory.Count() == 1)
                    )
                {
                    if (bNothingnessLeap == false)
                    {
                        OutMessage += "With nothing burdening you, and facing nothingness before " +
                            "you, you take a deep breath, and step forwards, arms wide, to " +
                            "embrace the nothingness all around you.\n\n" +
                            "You are falling, falling, falling, but you feel as light as a " +
                            "feather, you feel as if you are drifting, and you find yourself " +
                            "touching down in a very familiar place.\n";
                        bNothingnessLeap = true;
                        World._sage.bGreedClueUsed = true;
                    }
                    else
                    {
                        OutMessage += "You don't think you'd be able to do that again; in " +
                            "expecting the magic to work again, you wouldn't be able to embrace " +
                            "nothingness the way you did before.\n";
                        bSuccess = false;
                        return;
                    }

                }
                else
                {
                    OutMessage += "Oh, HECK NO.\n";
                    bSuccess = false;
                    return;

                }

            }

            if (ToLocation == World._centralCavern)
            {
                World._treasureCave.bDiscovered = bDiscoveredTreasureCave;
                bSuccess = true;
            }

            bSuccess = true;
        }

        public override string BlockedExits()
        {
            return "There is no way to go but back to the central cavern.";
        }

        public override void PostMove(Location FromLocation, Direction direction, ref string OutMessage)
        // While in this location, we need TreasureCave's bDiscovered to be false.
        // (Otherwise the player would see Go Down (to Treasure Cave) and thus the Nothingness
        // puzzle would be entirely trivialised.)
        // So set it to false when entering, and back to previous value when leaving.
        // (That bit's in PreMove)
        {
            bDiscoveredTreasureCave = World._treasureCave.bDiscovered;

            World._treasureCave.bDiscovered = false;
        }
    }

    public class Pathway
    {
        // Movement types allowed for this pathway
        // At the moment is just a string
        // maybe a list of strings would work better?
        public string sMovementTypes { get; set; }

        // The direction the pathway goes in
        public Direction dir { get; set; }

        // Where you go to
        public Location TargetLocation { get; set; }

        public Pathway()
        {
            sMovementTypes = "";
            dir = null;
            TargetLocation = null;
        }



    }
    

}
