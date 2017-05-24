using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using CustomExtensions;
using System.Reflection;

// 24/5/2017 - Bug 8 - Making sure that Remove works if object is in a container in Inventory.

namespace Engine
{
    [DataContractAttribute(IsReference=true)]
    [KnownType("DerivedTypes")]
    public class HasInventory
    // Superclass used by player, locations, and objects, as all three can have an inventory.
    // Serialization note:  You don't need to serialize the Inventory attribute.  It can be
    //     reconstructed by looking at the hiOwner of all objects.
    {
        public List<Engine.Object> Inventory = new List<Engine.Object>();
        
        [DataMember()]
        public string sName { get; set; }

        public bool HasItem(Engine.Object i)
        // A HasInventory has an object if it's in the Inventory, or in a container
        // in the Inventory.
        // While it probably wouldn't be too difficult to put containers into other containers
        // via recursion, this is being disallowed to reduce complexity in the UI.
        // Note that sometimes you just want to know if an item is in the inventory
        // (and not in any containers).  In that case, do not use HasItem.  Instead,
        // you can use "if (item.hiOwner = _player)".
        {
            return ((i.hiOwner == this) ||
                     (Inventory.Exists(x => x == i.hiOwner))
                   );

        }

        public void Add(Engine.Object i)
        // Bodyparts and NPCs go to the top of the list.  Everything else gets added to the end.
        // This'll keep things like "Your head" at the top of the player's inventory, and NPCs
        // at the top of the current location's inventory.
        {
            if (i.bBodypart)
            {
                Inventory.Insert(0, i);
            }
            else if (i.bNPC)
            {
                Inventory.Insert(0, i);
            }
            else
            {
                Inventory.Add(i);
            }
            i.hiOwner = this;
        }

        public void Remove(Engine.Object i)
        // Remove doesn't know where the object is going so can't assign its new hiOwner.
        // Make sure to assign it afterwards, either manually or by adding the object to
        // something else with HasInventory.Add().
        // 24/5/2017 - Bug 8 - Making sure that this works if object is in a container in Inventory.
        {
            if (i.hiOwner == this)
            {
                Inventory.Remove(i);
            }
            if (Inventory.Exists(x => x == i.hiOwner))
            {
                i.hiOwner.Remove(i);
            }
        }


        public void DetermineDisplayList(ref List<Engine.Object> FullList)
        // Come up with a list of objects that is everything in an inventory, including inside
        // containers.  Used to determine everything to display in the UI.
        // This is something that straddles the line a bit between UI function and game engine
        // function, so not sure of the best place for it.
        {
            FullList.Clear();
            foreach (var item in this.Inventory)
            {
                FullList.Add(item);
                if ((item.bContainer) && (item.Inventory.Count > 0) && (item.bDiscoveredContents) && (item.bLocked == false))
                {
                    foreach (var item2 in item.Inventory)
                    {
                        FullList.Add(item2);
                    }
                }
            }
        }


        private static Type[] DerivedTypes()
        {
            return typeof(HasInventory).GetDerivedTypes(Assembly.GetExecutingAssembly()).ToArray();
        }
    }
}
