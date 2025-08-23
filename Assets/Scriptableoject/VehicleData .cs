using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
namespace Assets.Scriptableoject
{
    [CreateAssetMenu(fileName = "NewVehicle", menuName = "Game/Vehicle Data")]
    internal class VehicleData : ScriptableObject
    {
        public int vehicleID;
        public string vehicleName;
        public Sprite icon;
        public float speed;
        public int price;
    }
}
