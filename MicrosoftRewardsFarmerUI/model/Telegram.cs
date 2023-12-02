using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicrosoftRewardsFarmerUI.model {
    internal class Telegram {
        public string token { get; set; }
        public string chatId { get; set; }

        public Telegram(string token, string chatId) {
            this.token = token;
            this.chatId = chatId;
        }
    }
}
