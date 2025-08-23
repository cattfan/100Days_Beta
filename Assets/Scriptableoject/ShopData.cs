using NUnit.Framework.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scriptableoject
{
    [CreateAssetMenu(fileName = "NewShop", menuName = "Game/Shop Data")]
    public class ShopData : ScriptableObject
    {
        [Header("Thông tin cửa hàng")]
        public string shopID;
        public string shopName;
        public string shopType; // Ví dụ: "Weapon", "Food", "General"

        [Header("Danh sách hàng hóa")]
        public List<ShopItem> itemsForSale;
    }
    [System.Serializable]
    public class ShopItem
    {
        public ItemData item;    // Tham chiếu đến ItemData
        public int stock;        // Số lượng hàng tồn kho (-1 = vô hạn)
        public float price;      // Giá bán (nếu muốn khác giá gốc trong ItemData)
    }
}
