using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scriptableoject
{
    [CreateAssetMenu(fileName = "NewTicket", menuName = "Game/Ticket Data")]
    public class TicketData : ScriptableObject
    {
        [Header("Thông tin vé số")]
        public string ticketID;
        public string ticketName;     // Ví dụ: "Vé số miền Nam"
        public float price;           // Giá vé
        public int numberLength = 6;  // Số chữ số (VD: 6 số)

        [Header("Thông tin giải thưởng")]
        public float jackpotPrize;    // Giải đặc biệt
        public float secondPrize;     // Giải nhì
        public float thirdPrize;      // Giải ba

        [TextArea]
        public string rules;          // Mô tả quy tắc thắng (VD: "Khớp đủ 6 số = Jackpot")
    }
}
