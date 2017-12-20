using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intellidoorn_software
{
    class Stand
    {
        public string tagId1 { get; private set; }
        public string tagId2 { get; private set; }
        public string tagId3 { get; private set; }
        public string tagId4 { get; private set; }
        public List<String> tags = new List<string>();
        public double height;
        public double baseHeight;
        public int row;

        public string locationDescription { get; private set; }

        public Stand(string tagId1, string tagId2, string tagId3, string tagId4, string locationDescription, double height, double baseHeight, int row)
        {
            this.tagId1 = tagId1;
            this.tagId2 = tagId2;
            this.tagId3 = tagId3;
            this.tagId4 = tagId4;
            this.locationDescription = locationDescription;
            this.height = height;
            this.baseHeight = baseHeight;
            this.row = row;
            tags.Add(tagId1);
            tags.Add(tagId2);
            tags.Add(tagId3);
            tags.Add(tagId4);
        }

        public override string ToString()
        {
            return $"Location description:  tagID 1: { tagId1} tagID 2: { tagId2} tagID 3: { tagId3} tagID 4: { tagId4}"; 
        }
    }
}
