using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
namespace Assets.Scriptableoject
{
    [CreateAssetMenu(fileName ="NewNPC", menuName = "Game/NPC Data")]
    internal class NPC
    {
        public int npcID;
        public string npcName;
        public Sprite portrait;      // ảnh NPC trong UI
        public string npcRole;       // Shop / VehicleShop / VeSo
        [TextArea]
        public string[] dialogues;   // Các câu thoại
    }
}
