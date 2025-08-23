using Assets.Scriptableoject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Model
{
    [System.Serializable]
    public class ItemInstance
    {
        public ItemData itemData;  // Tham chiếu ScriptableObject chứa thông tin gốc
        public int quantity;

        public ItemInstance(ItemData data, int qty = 1)
        {
            itemData = data;

            quantity = qty;
        }
    }
    }
