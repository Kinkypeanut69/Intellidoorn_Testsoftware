using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intellidoorn_software
{
    class TagInfo
    {
        public string itemCode { get; private set; }
        public int signalStrength { get; set; }

        public TagInfo(string itemCode, int signalStrength)
        {
            this.itemCode = itemCode;
            this.signalStrength = signalStrength;
        }

        public override string ToString()
        {
            return $"Tag: { itemCode }\nStrength: { signalStrength }";
        }
    }

}