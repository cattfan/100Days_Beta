using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
namespace Assets.Model
{
    [System.Serializable]
    public class GameData
    {
        public Player player;
        public List<Player> npcTraders = new List<Player>();

        public GameData(string playerName)
        {
            player = new Player(playerName);
        }
    }
}
