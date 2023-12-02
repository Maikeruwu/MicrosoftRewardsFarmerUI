using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicrosoftRewardsFarmerUI.model {
    internal class Discord {
        public string webhook { get; set; }

        public Discord(string webhook) {
            this.webhook = webhook;
        }
    }
}
