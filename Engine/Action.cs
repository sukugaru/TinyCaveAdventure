using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using CustomExtensions;
using System.Reflection;


namespace Engine
{
    [DataContractAttribute(IsReference=true)]
    [KnownType("DerivedTypes")]
    public class Action
    {
        // This is basically an abstract / virtual class.
        // Create a new, specific action class that inherits from this one, populates the
        // attributes with a constructor, and does stuff in DoAction().

        // When creating a new action, you need to:
        //    create the new class,
        //    instantiate it in the World static class,
        //    populate its values in World.populateActions(),
        //    And then make sure player.DetermineActions() has the new action.
        // Whew.
        [DataMember]
        public int iNumArgs { get; set; }

        [DataMember]
        public string sProtoCmdLine { get; set; }

        [DataMember]
        public string sName { get; set; }

        public override string ToString()
        {
            return sName;
        }

        public virtual void DoAction(Engine.Object Item1, Engine.Object Item2, bool Suppress, ref string OutMessage, ref bool bSuccess)
        // Item1 - the item to act on
        // Item2 - if the action works on two items, this is the second
        // Suppress - if the action is successful, do we want to suppress the successful message?
        // OutMessage - the message that will be displayed
        // bSuccess - if the action was successful or not
        { }

        private static Type[] DerivedTypes()
        {
            return typeof(Action).GetDerivedTypes(Assembly.GetExecutingAssembly()).ToArray();
        }

    }

}
