using System.Collections.Generic;
using System.Drawing;

namespace BaseSpace
{
    public class Demand
    {
        public string code;

        public int num;
        public int quantity;
        public Color color;
        public string name;

        public Dictionary<int, double> occupiedMaterials;

        public Demand(List<int> mats, List<double> nums, string code, int num, int quantity, Color color, string name) 
        {
            for (int m = 0; m < mats.Count; m++) 
            {
                occupiedMaterials.Add(mats[m], nums[m]);
            }
            this.code = code;
            this.num = num;
            this.quantity = quantity;
            this.color = color;
            this.name = name;
        }

        public Demand(Dictionary<int, double> mats, string code, int num, int quantity, Color color, string name)
        {
            this.occupiedMaterials = mats;
            this.code = code;
            this.num = num;
            this.quantity = quantity;
            this.color = color;
            this.name = name;
        }
    }
}