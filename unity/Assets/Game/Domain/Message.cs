using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Game.Domain
{
    public class Message
    {
        public string type { get; set; }
        public object payload { get; set; }
    }
}
