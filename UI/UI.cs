using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Engine;

// 28/6/2017 - General fixing - Development on this may not continue anymore, as all the engine
// specific stuff is now done.  I've updated "About" to have a last updated date, and "How does
// this work" to show that save and load may not be coming after all.
// (Done while working on Enhancement 9.)
//
// 7/6/2017 - Bug 6 - Renaming the Object class to Item.
// 
// 20/5/2017 - SS - Bug 4 - In button1_click, during the conversation handling code.  After
// HandleResponse(), make sure to clear OutMessage before calling GetDialogue, to avoid the
// result of HandleResponse being output twice.

namespace WindowsFormsApplication2
{
    public partial class UI : Form
    {
        // Player
        private Player _player;

        // Actions
        // (UI only needs to know about report hiOwner; this is a debugging Action)
        private Engine.Action _Report_hiOwner;

        // UI objects
        public Item i;         // Object to act on 
        public Item i2;        // Second object to act on, if required by selected action
        private string ProtoCmdLine;    // Used in calculating final command line
        private int NumOfObjs;          // Number of objects required by selected action
        List<Engine.Action> ActionList = new List<Engine.Action>();     // These are the lists 
        List<Direction> DirectionList = new List<Direction>();          // used in all the 
        List<string> ResponseList = new List<string>();                 // various listbox
        List<Item> FinalPlayerInv = new List<Item>(); // controls.
        List<Item> FinalLocInv = new List<Item>();    


        public UI()
        {
            InitializeComponent();
            InitialiseValues();            
        }

        private void InitialiseValues()
        {
            // A lot of things are initialised by the World static class (see the Engine namespace)

            // The UI needs to know what an action is if it's a nonstandard action.
            // report hiOwner is a reporting action used in debugging

            // Create player
            _player = World._player;
            
            // Define actions
            _Report_hiOwner = World._Report_hiOwner;

            // UI - put the beginning text in, and get the UI ready for use

            if ( (World._BeginningText != "") && (World._BeginningText != null) )
            {
                richTextBox1.AppendText(World._BeginningText + "\n");
            }
            _player.DetermineActions(ref ActionList);
            UpdateUIAfterTurn();
        }


        private void UpdateUIAfterTurn ()
        {
            string s = "";
            
            // If in normal mode, display the usual controls, while hiding Conversation control.
            // If in conversation / branching actions mode, hide most of them and display 
            // the Conversation control.
            // Used to manually resize Action listbox.  Interesting to note that the widths 
            // in the form designer were 282 and 862, but here I needed to use 2/3 those values,
            // so 189 and 576.

            if (_player.CurrentConversation != null)
            {
                /*
                lbAction.Visible = false;
                lbInventory.Visible = false;
                lblInventory.Visible = false;
                lbLocInv.Visible = false;
                lblObjects.Visible = false;
                lbDirections.Visible = false;
                lblDirections.Visible = false;
                lbConversation.Visible = true;
                */
                pnlConversation.Visible = true;
                pnlNormalMode.Visible = false;
                // Conversation Dialogue responses are done here.  If the turn just gone
                // initiated a conversation, we want the first line of Dialogue to be
                // output straight away, with no further clicks needed.

                if (_player.CurrentConversation.AtBeginning())
                {
                    _player.CurrentConversation.GetDialogue(ref s);
                    if (s != "")
                    {
                        richTextBox1.AppendText(s);
                    }

                    richTextBox1.AppendText("\n> ");
                }

                _player.CurrentConversation.DetermineResponses(ref ResponseList);
                lbConversation.DataSource = null;
                lbConversation.DataSource = ResponseList;

            }
            else
            {
                /*
                lbAction.Visible = true;
                lbAction.Enabled = true;
                lbInventory.Visible = true;
                lbInventory.Enabled = true;
                lblInventory.Visible = true;
                lbLocInv.Visible = true;
                lbLocInv.Enabled = true;
                lblObjects.Visible = true;
                lbDirections.Visible = true;
                lbDirections.Enabled = true;
                lblDirections.Visible = true;
                lbConversation.Visible = false;
                 * */
                pnlConversation.Visible = false;
                pnlNormalMode.Visible = true;
                lbAction.Enabled = true;
                lbInventory.Enabled = true;
                lbLocInv.Enabled = true;
                lbDirections.Enabled = true;

                button1.Text = "Do Action";

                _player.DetermineActions(ref ActionList);
                lbAction.DataSource = null;
                lbAction.DataSource = ActionList;

            }

            if (_player.CurrentTextSequence != null)
            {
                
                pnlConversation.Visible = false;
                pnlNormalMode.Visible = true;
                lbAction.Enabled = false;
                lbInventory.Enabled = false;
                lbLocInv.Enabled = false;
                lbDirections.Enabled = false;
                /*
                lbAction.Enabled = false;
                lbInventory.Enabled = false;
                lbLocInv.Enabled = false;
                lbDirections.Enabled = false; */
                button1.Text = "Next";

                _player.DetermineActions(ref ActionList);
                lbAction.DataSource = null;
                lbAction.DataSource = ActionList;

                // The very beginning of a text sequence gets kicked off here.
                // This is to make sure that if the turn just gone
                // Text Sequences are otherwise handled in button1_click.
                if (_player.CurrentTextSequence.AtBeginning())
                {
                    s = _player.CurrentTextSequence.Current() + "\n---\n\n";
                    richTextBox1.AppendText(s);

                }
            }

            // Clear Selection in Conversations and Actions lists
            lbAction.ClearSelected();
            lbConversation.ClearSelected();

            // Determine and display player Inventory
            _player.DetermineDisplayList(ref FinalPlayerInv);
            lbInventory.DataSource = null;
            lbInventory.DataSource = FinalPlayerInv;
            lbInventory.ClearSelected();

            // Determine and display Location inventory
            _player.CurrentLocation.DetermineDisplayList(ref FinalLocInv);
            lbLocInv.DataSource = null;
            lbLocInv.DataSource = FinalLocInv;
            lbLocInv.ClearSelected();
           
            // Determine and display directions
            DetermineDirections();

            // If finished, disable all listboxes and the
            // action button
            if (_player.sState == "Finished")
            {
                lbAction.Enabled = false;
                lbInventory.Enabled = false;
                lbLocInv.Enabled = false;
                lbDirections.Enabled = false;
                lbConversation.Enabled = false;
            }

            // Diagnostics
            /*
            richTextBox1.AppendText("In public: ");
            foreach(Location l in World.InPublic.LocationList)
            {
                richTextBox1.AppendText(l.sName + " ");
            } 
            richTextBox1.AppendText("\n");
            richTextBox1.AppendText("In apartment: ");
            foreach (Location l in World.InApartment.LocationList)
            {
                richTextBox1.AppendText(l.sName + " ");
            }
            
            richTextBox1.AppendText("\n");
            richTextBox1.AppendText("In all apartment: ");
            foreach (Location l in World.InAllApartment.LocationList)
            {
                richTextBox1.AppendText(l.sName + " ");
            }
            richTextBox1.AppendText("\n");
            */

            // Various other bits           
            rtbCommandLine.Text = "";
            i = null;
            i2 = null;
            NumOfObjs = 0;
            ProtoCmdLine = "";

            //Location box
            rtbLocation.Text = _player.CurrentLocation.sName + "\n";
            rtbLocation.AppendText(_player.CurrentLocation.sDescription + "\n");

            s = _player.CurrentLocation.BlockedExits();
            if (s != "") { s += "\n"; }
            rtbLocation.AppendText(s);

            s = _player.CurrentLocation.LockedExits();
            rtbLocation.AppendText(s);

            // Diagnosis box
            rtbDiagnosis.Text = _player.getDiagnosis();

        }

        private void DetermineDirections()
        // Determine possible directions, and display them.
        // Is a separate procedure because once upon a time this was much larger
        // and more complicated.
        {
            DirectionList.Clear();
            _player.CurrentLocation.DetermineAvailableDirections(ref DirectionList);

            lbDirections.DataSource = null;
            lbDirections.DataSource = DirectionList;
            lbDirections.ClearSelected();
        }

        private void BuildCommandLine()
        //Build and display the command line in the rtbCommandLine textbox
        {
            StringBuilder FinalCommandLine = new StringBuilder(ProtoCmdLine);
            int j;

            if (NumOfObjs > 0)
            {
                if (i == null)
                {
                    FinalCommandLine.Replace("[item1]", "");
                }
                else
                {
                    FinalCommandLine.Replace("[item1]", i.sName);
                }
            }

            if (NumOfObjs == 2)
            {
                if (i2 == null)
                {
                    FinalCommandLine.Replace("[item2]", "");
                }
                else
                {
                    FinalCommandLine.Replace("[item2]", i2.sName);
                }
            }
            rtbCommandLine.Text = FinalCommandLine.ToString();
        }


        private void button1_Click(object sender, EventArgs e)
        // Perform currently selected action, whatever it is.
        // After doing the action, it will have come back with some sort of OutMessage.
        // OutMessage ends with \n.
        // This procedure outputs OutMessage, and the "> " prompt and newlines etc. after it.
        {
            Direction selectedDir;
            Location TargetLocation;
            string OutMessage = "";
            bool bSuccess = false;

            // If there's no command then exit.
            // Note that in Text Sequence mode there's no command.
            if ( (rtbCommandLine.Text == "") &&
                 (_player.CurrentTextSequence == null) )
            {
                return;
            }

            // ========================================================================
            // Put command line into main window
            // This is for regular actions and conversations.
            // When going through a Text Sequence there's no command line.
            if (_player.CurrentTextSequence == null)
            {
                richTextBox1.AppendText(rtbCommandLine.Text + "\n\n");
            }

            // Any game-engine logic that should happen before the turn.
            // Game engine logic is done in the engine, with World.PreAction().  Put
            // new checks there.  Not needed when in Text Sequence mode.
            if (_player.CurrentTextSequence == null)
            {
                OutMessage = "";
                World.PreAction(ref OutMessage);
                if (OutMessage != "")
                {
                    if (OutMessage == "\n")
                        richTextBox1.AppendText(OutMessage);
                    else
                        richTextBox1.AppendText("\n" + OutMessage);
                } 
            }

            // ========================================================================
            // Actually do the player's turn
            // Can either be moving in a direction, an Action, or a conversation response.
            OutMessage = "";

            // Direction
            if (lbDirections.SelectedItem != null)
            {
                selectedDir = (Direction)lbDirections.SelectedItem;
                TargetLocation = selectedDir.TargetLocation(_player.CurrentLocation);

                World.MoveTo(TargetLocation, selectedDir, false, ref OutMessage, ref bSuccess);
                richTextBox1.AppendText(OutMessage);
            } else 

            // Action
            if ((lbAction.SelectedItem != null) && (rtbCommandLine.Text != ""))
            {
                DoAction();
            }
            else

            // Conversation response            
            if (lbConversation.SelectedItem != null)
            {
                // Handle the response
                _player.CurrentConversation.HandleResponse(lbConversation.SelectedItem.ToString(),
                                                           ref OutMessage);
                richTextBox1.AppendText(OutMessage);

                // Get the next line of dialogue
                // Note that the response might have quit the conversation!
                // Note that the very first use of GetDialogue is in UpdateUIAfterTurn, to
                // kick a conversation off immediately.
                if (_player.CurrentConversation != null)
                {
                    // 20/5/2017 - SS - Bug 4 - Make sure to clear OutMessage before calling
                    // GetDialogue, to avoid the result of HandleResponse being output twice.
                    OutMessage = "";

                    _player.CurrentConversation.GetDialogue(ref OutMessage);
                    if (OutMessage != "")
                    {
                        richTextBox1.AppendText(OutMessage);
                    }

//                    richTextBox1.AppendText("\n> ");
                }

            } else

            // Text Sequence
            if (_player.CurrentTextSequence != null)
            {
                OutMessage = _player.CurrentTextSequence.Current();

                if (OutMessage != "")
                {
                    richTextBox1.AppendText(OutMessage);
                }

                if (_player.CurrentTextSequence.AtEnd())
                {
                    _player.CurrentTextSequence.EndSequence();
                    _player.CurrentTextSequence = null;
                }
            }

            // ========================================================================
            // Anything that should happen after the turn

            // First up, call World.PostAction().  If there is any game logic that should be
            // performed after every action, put it into PostAction().
            // If in a text sequence then clicking through it doesn't count as performing 
            // different actions, so PostAction isn't called.

            /* Some diagnostic stuff, from trying to debug things.
            if (_player.CurrentConversation == null)
                richTextBox1.AppendText("Current Conv: null\n");
            else
            {
                richTextBox1.AppendText("Current Conv: " + _player.CurrentConversation.ToString());
                // richTextBox1.AppendText(" (At beginning: " + _player.CurrentConversation.AtBeginning() + ")");
                richTextBox1.AppendText("\n");
            }

            if (_player.CurrentTextSequence == null)
                richTextBox1.AppendText("Current TS: null\n");
            else
                richTextBox1.AppendText("Current TS: " + _player.CurrentTextSequence.ToString() + "\n");
            */
            
            if (_player.CurrentTextSequence == null)
            {
                OutMessage = "";
                World.PostAction(ref OutMessage);
                if (OutMessage != "")
                {
                    if (OutMessage == "\n")
                        richTextBox1.AppendText(OutMessage);
                    else
                        richTextBox1.AppendText("\n" + OutMessage);
                }
            }
            
            // Next, the "> " prompt (Normal Mode) or "---" scene break (Text Sequence)

            // This was all very complicated to make work!  The prompts and breaks from one
            // mode (normal, conversation, text sequence) kept interfering with each other
            // and throwing newlines and extra ">"s out in odd places.
            // As a result of getting it all to work, Conversations handle their own ">" prompt,
            // but the rest are handled here.
            // ...though I think I might have fixed that.
            if ( (_player.CurrentTextSequence == null)
                  && (_player.CurrentConversation == null)
                )
            {
                richTextBox1.AppendText("\n> ");
            }
            else if (_player.CurrentConversation != null)
            {
                if (_player.CurrentConversation.AtBeginning() == false)
                {
                    richTextBox1.AppendText("\n> ");
                }

            }
            else if (_player.CurrentTextSequence != null)
            {
                if (_player.CurrentTextSequence.AtBeginning())
                    richTextBox1.AppendText("\n");
                else
                    richTextBox1.AppendText("\n---\n\n");
            }
            

            // If anything else needs to be done in the UI after a turn, put it here.

            // Keep UpdateUIAfterTurn() at end of button1_click.  It's the very last thing
            // that should be done.
            // Additionally, if a conversation or text sequence has been started, 
            // UpdateUIAfterTurn will start it.

            UpdateUIAfterTurn();

        }

        private void DoAction()
        {
            bool bSuccess = false;
            string OutMessage = "";
            Engine.Action SelectedAction = null;

            // Update FromObj and FromLoc
            // Look into getting rid of this code - I don't think FromObj and FromLoc
            // are used anymore.
/*            FromObj = false;
            if (i != null)
            {
                if (i.oContainerObject != null)
                {
                    FromLoc = false;
                    FromObj = true;
                }
            } */

            // Still a bit kludgy.
            // There are some old and nonstandard actions that need to be checked for and run in
            // their own way.
            if (lbAction.SelectedItem == _Report_hiOwner)
            {
                richTextBox1.AppendText("Owner of " + i.sName + " is " + i.hiOwner.sName + "\n");
            }

            else
            // OK so by this point we've weeded out all the nonstandard actions.
            // Time for the SelectedAction.DoAction polymorphism I've been wanting to do ever
            // since this project started!
            // (This project started 13 Aug 2016 (ish) and writing this comment 29 Aug 2016)
            {
                SelectedAction = (Engine.Action)lbAction.SelectedItem;

                OutMessage = "";
                SelectedAction.DoAction(i, i2, false, ref OutMessage, ref bSuccess);
                richTextBox1.AppendText(OutMessage);

            }

            
        }

        
        private void lbInventory_SelectedIndexChanged(object sender, EventArgs e)
        {
            int j;

            // Clear directions listbox.  Also clear object listboxes if needed.
            j = lbInventory.SelectedIndex;
            if (NumOfObjs != 2) { lbLocInv.ClearSelected(); }
            lbDirections.ClearSelected();
            lbInventory.SelectedIndex = j;

            if ((NumOfObjs == 0) && (lbAction.SelectedItem != null))
            {
                lbInventory.ClearSelected();
            }

            // Diagnostic
            /*
            richTextBox1.AppendText(NumOfObjs.ToString() + "   ");
            if (i == null)
            {
                richTextBox1.AppendText("i is null   ");
            }
            else
            {
                richTextBox1.AppendText(i.sName + "   ");
            }
            richTextBox1.AppendText("FromLoc is " + FromLoc.ToString() + "   ");

            if (i2 == null)
            {
                richTextBox1.AppendText("i2 is null   ");
            }
            else
            {
                richTextBox1.AppendText(i2.sName + "   ");
            }
            richTextBox1.AppendText("\n");
            */

            // Assign i and i2 based on the Number of Objects, and which ones are
            // currently assigned.
            // This is some code I worked out before and didn't comment and as a result
            // it looks a bit arcane and complicated.

            if ((NumOfObjs == 1) || (lbAction.SelectedItem == null))
            {
                i = (Item)lbInventory.SelectedItem;
                // FromLoc = false;
            }

            if (NumOfObjs == 2)
            {
                if ((i == null) && (i2 == null))
                {
                    i = (Item)lbInventory.SelectedItem;
                    // FromLoc = false;
                } else

                if ((i == null) && (i2 != null))
                {
                    i = (Item)lbInventory.SelectedItem;
                    i2 = null;
                    // FromLoc = false;
                } else

                if ((i != null) && (i2 == null))
                {
                    i2 = (Item)lbInventory.SelectedItem;
                } else

                if ((i != null) && (i2 != null))
                {
                    i = (Item)lbInventory.SelectedItem;
                    i2 = null;
                    // FromLoc = false;

                }

            }

            BuildCommandLine();
        }

        private void lbLocInv_SelectedIndexChanged(object sender, EventArgs e)
        {
            int j;

            // Clear directions listbox.  Also clear object listboxes if needed.
            j = lbLocInv.SelectedIndex;
            if (NumOfObjs != 2) { lbInventory.ClearSelected(); }
            lbDirections.ClearSelected();
            lbLocInv.SelectedIndex = j;

            if ((NumOfObjs == 0) && (lbAction.SelectedItem != null))
            {
                lbLocInv.ClearSelected();
            }

            // Diagnostic
            /*
            richTextBox1.AppendText(NumOfObjs.ToString() + "   ");
            if (i == null)
            {
                richTextBox1.AppendText("i is null   ");
            }
            else
            {
                richTextBox1.AppendText(i.sName + "   ");
            }
            richTextBox1.AppendText("FromLoc is " + FromLoc.ToString() + "   ");

            if (i2 == null)
            {
                richTextBox1.AppendText("i2 is null   ");
            }
            else
            {
                richTextBox1.AppendText(i2.sName + "   ");
            }
            richTextBox1.AppendText("\n");
            */

            // Assign i and i2 based on the Number of Objects, and which ones are
            // currently assigned.
            // This is some code I worked out before and didn't comment and as a result
            // it looks a bit arcane and complicated.

            if ((NumOfObjs == 1) || (lbAction.SelectedItem == null))
            {
                i = (Item)lbLocInv.SelectedItem;
                // FromLoc = true;
            }

            if (NumOfObjs == 2)
            {
                if ((i == null) && (i2 == null))
                {
                    i = (Item)lbLocInv.SelectedItem;
                    // FromLoc = true;
                } else                 if ((i == null) && (i2 != null))
                {
                    i = (Item)lbLocInv.SelectedItem;
                    i2 = null;
                    // FromLoc = true;
                } else                 if ((i != null) && (i2 == null))
                {
                    i2 = (Item)lbLocInv.SelectedItem;
                } else                 if ((i != null) && (i2 != null))
                {
                    i = (Item)lbLocInv.SelectedItem;
                    i2 = null;
                    // FromLoc = true;

                }

            }
            BuildCommandLine();

        }

        private void lbDirections_SelectedIndexChanged(object sender, EventArgs e)
        {
            int i;
            Location targetLocation = null;
            Direction selectedDir = null;

            // First of all, clear the selections in the other listboxes.
            i = lbDirections.SelectedIndex;

            lbLocInv.ClearSelected();
            lbInventory.ClearSelected();
            lbAction.ClearSelected();

            lbDirections.SelectedIndex = i;


            // Then create the command line.  e.g. "Go north (to hallway)"
            if (lbDirections.SelectedItem != null)
            {
                selectedDir = (Direction)lbDirections.SelectedItem;
                targetLocation = selectedDir.TargetLocation(_player.CurrentLocation);

                ProtoCmdLine = "Go " + lbDirections.SelectedItem;
                if ( (targetLocation.bDiscovered != false) &&
                     (targetLocation != World._locked) &&
                     (targetLocation != World._blocked)
                   )
                {
                    ProtoCmdLine += " (to " + targetLocation.sName + ")";
                }
            }
            else
                ProtoCmdLine = "";

            BuildCommandLine();
        }

        private void lbAction_SelectedIndexChanged(object sender, EventArgs e)
        {
            int j;
            Engine.Action SelectedAction;

            // Clear the Directions listbox
            j = lbAction.SelectedIndex;
            lbDirections.ClearSelected();
            lbAction.SelectedIndex = j;

            // Set NumOfObjs and ProtoCmdLine based on selected action
            NumOfObjs = 0;
            ProtoCmdLine = "";

            SelectedAction = (Engine.Action)lbAction.SelectedItem;
            if (SelectedAction != null)
            {
                NumOfObjs = SelectedAction.iNumArgs;
                ProtoCmdLine = SelectedAction.sProtoCmdLine;
            }

            // If Action takes no objects as arguments, then clear the
            // object listboxes.
            if (NumOfObjs == 0)
            {
                lbInventory.ClearSelected();
                lbLocInv.ClearSelected();
                i = null;
                i2 = null;
            }


            BuildCommandLine();
        }

        private void lbConversation_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Quite simple!  Just updating the command line.
            NumOfObjs = 0;
            ProtoCmdLine = (string)lbConversation.SelectedItem;
            BuildCommandLine();
        }


        private void richTextBox1_TextChanged(object sender, EventArgs e)
        // Scroll to bottom when text changes.
        {
            richTextBox1.SelectionStart = richTextBox1.Text.Length;
            richTextBox1.ScrollToCaret();
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        // Created this by accident.  Keeping it around if I need it.
        { }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        // 28/6/2017 - General fixing - Getting the last update date from World.
        {
            MessageBox.Show("Tiny Cave Adventure v0.9.Something\n" +
                "Scougall Basic Adventure Engine Thingy version 0.9S.omething\n\n" +
                "This was created basically as a way to teach myself C#.  Given that " +
                "my graphical and sounds skills are mediocre and long out of practice, I " +
                "figured an adventure game would be the best way to do it!\n\n" +
                "Most recently updated: " + World._lastUpdateDate
                ,"About"
                );
                // Note to self
                // Opening a new form would be done like this:
                // Have another form, eg Form2, or AddItemForm, or ShowAllItemsForm
                // this.Hide();  // in this context, this would be Form1)
                // Form2 frm2 = new Form2();
                // frm2.Show();
                // or frm2.ShowDialog(); // modal vs nonmodal
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void howDoesThisWorkToolStripMenuItem_Click(object sender, EventArgs e)
        // 28/6/2017 - general fixing - Updated to indicate save and load may not
        // be implemented for this game.
        {
            MessageBox.Show("When you very first start, you're in a text version of a cutscene," +
                "and most of the options are grayed out.  Click on the \"Next\" button to " +
                "continue to the next part.  Eventually all the options will become selectable " +
                "and you'll have control.\n\n" +
                "You have four windows at the bottom:  Actions you can do, your inventory, " +
                "things you can see in your current location, and directions you can go.  " +
                "Select an action and the item(s) to do it on, or a direction to go, and you'll " +
                "see the command build up.  When the command is what you want to do, click on " +
                "\"Do Action\" and see what happens!\n\n" +
                "At the moment you cannot save or load the game.  This may not be coming in this " +
                "adventure, as development has concentrated on other aspects of the game " +
                "engine.\n\n" +
                "And if something really bizarre happens or things break, you have a new quest " +
                " - you must inform the programmer, at once!"

                , "How this works");
        }

        private void hintToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string s;
            s = World.Hint();
            MessageBox.Show(s, "Hints and tips");
        }
    }
}


