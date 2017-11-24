using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intellidoorn_software
{
    class Stand
    {
        public int locationId { get; private set; }
        public string tagId { get; private set; }
        public string locationDescription { get; private set; }
        public bool isDock { get; private set; }

        public Stand(int locationId, string tagId, string description, bool isDock)
        {
            this.locationId = locationId;
            this.tagId = tagId;
            locationDescription = description;
            this.isDock = isDock;
        }
    }
}
