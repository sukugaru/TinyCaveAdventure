using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{

    public class TextSequence
    {
        protected List<string> SceneList;
        protected int pos;

        public TextSequence()
        {
            pos = 0;
        }

        public TextSequence(List<string> inList)
        {
            pos = 0;
            SceneList = inList;
        }

        public void Set(List<string> inList)
        {
            SceneList = inList;
        }

        public virtual string Current()
        {
            string returnValue = "";

            if (pos < SceneList.Count())
            {
                returnValue +=
                    // pos.ToString() + " of " + SceneList.Count.ToString() + ") " + 
                    SceneList[pos];
            }

            pos++;

            return returnValue;

        }

        public bool AtEnd()
        {
            return (pos == SceneList.Count());
        }

        public bool AtBeginning()
        {
            return ((pos == 0) || (pos == 1));

        }


        public string Skip()
        {
            int i;
            string s = "";

            for (i = pos; i <= SceneList.Count(); i++)
            {
                if (i > pos)
                {
                    s += "\n";
                }
                s += SceneList[i] + "\n";
            }

            return s;
        }

        public virtual void EndSequence()
        {
            pos = 0;
        }
    }

    public class TribeVistSequence : TextSequence
    {
        public TribeVistSequence(List<string> inList) : base (inList)
        {
        }

        public override void EndSequence()
        {
            pos = 0;
            World._tribalCavern.bPartying = true;
        }

    }

    public class EndingSequence : TextSequence
    {
        public EndingSequence(List<string> inList)
            : base(inList)
        {
        }

        
        public override string Current()
        // So this is a bit different to other TextSequences, in that it modifies your location
        // and what you have, based on where you are in the sequence.
        {

            string returnValue = "";

            if (pos < SceneList.Count())
            {
                returnValue +=
                    // pos.ToString() + " of " + SceneList.Count.ToString() + ") " + 
                    SceneList[pos];
            }

            switch (pos)
            {
                case 0:
                    World._player.Remove(World._lostGemNecklace);
                    World._player.CurrentLocation = World._sageGrotto;
                    break;

                case 1:
                    World._player.CurrentLocation = World._tribalCavern;
                    if (World._player.HasItem(World._tribalHeadgear) == false)
                    {
                        World._player.Add(World._tribalHeadgear);
                    }
                    if (World._player.HasItem(World._tribalCostume) == false)
                    {
                        World._player.Add(World._tribalCostume);
                    }
                    World._tribalCostume.bWorn = true;
                    World._tribalHeadgear.bWorn = true;
                    break;

                case 3:
                    World._player.CurrentLocation = World._BackDoor;
                    World._player.Remove(World._tribalHeadgear);
                    World._player.Remove(World._tribalCostume);

                    World._tribalCostume.bWorn = false;
                    World._tribalHeadgear.bWorn = false;

                    if (World._player.HasItem(World._recipe) == false)
                    {
                        World._player.Add(World._recipe);
                    }
                    break;

                case 4:
                    World._player.CurrentLocation = (Location)World._crazyGuy.hiOwner;
                    World._player.Remove(World._recipe);
                    break;

                default:
                    break;
            }

            pos++;

            return returnValue;

        }
        

        public override void EndSequence()
        {
            pos = 0;
            World._player.sState = "Finished";
        }

    }

}
