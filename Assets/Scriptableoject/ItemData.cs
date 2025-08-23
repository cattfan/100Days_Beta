using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scriptableoject
{
    [CreateAssetMenu(fileName = "NewItem", menuName = "Game/Item data")]
    public class ItemData : ScriptableObject
    {
        public int itemID;          // ID dùng trong DB
        public string itemName;     // Tên hiển thị
        public Sprite icon;         // Ảnh icon
        public int value;           // Giá trị cơ bản
        public int sellPrice;       // Giá bán tại shop
    }
}
