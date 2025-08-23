using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Model
{
    [System.Serializable]
    public class Player
    {
        public string playerName;
        public int level;
        public int gold;

        public Inventory inventory;

        public Player(string name)
        {
            playerName = name;
            level = 1;
            gold = 100;
            inventory = new Inventory();
        }

        public void AddGold(int amount)
        {
            gold += amount;
        }

        public bool SpendGold(int amount)
        {
            if (gold >= amount)
            {
                gold -= amount;
                return true;
            }
            return false;
        }
    }
}
