using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Model
{
    [System.Serializable]
    public class Inventory
    {
        public List<ItemInstance> items = new List<ItemInstance>();

        public void AddItem(ItemInstance item)
        {
            items.Add(item);
        }

        public void RemoveItem(ItemInstance item)
        {
            items.Remove(item);
        }

        public ItemInstance FindItem(int itemId)    
        {
            return items.Find(i => i.itemData.itemID == itemId);
        }
    }
}
