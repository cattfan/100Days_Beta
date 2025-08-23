using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Model
{
    [System.Serializable]
    public class GiaoDich
    {
        public Player buyer;
        public Player seller;
        public ItemInstance item;
        public int price;

        public GiaoDich(Player buyer, Player seller, ItemInstance item, int price)
        {
            this.buyer = buyer;
            this.seller = seller;
            this.item = item;
            this.price = price;
        }

        public bool ThucHien()
        {
            if (buyer.SpendGold(price))
            {
                seller.AddGold(price);
                seller.inventory.RemoveItem(item);
                buyer.inventory.AddItem(item);
                return true;
            }
            return false;
        }
    }
}
