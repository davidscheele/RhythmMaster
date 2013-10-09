using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace RhythmMaster.Functions
{
    public class ShakerColorList : Dictionary<int, Color>
    {
        public ShakerColorList()
        {
            this.Add(10, new Color(246, 20, 0));
            this.Add(9, new Color(246, 45, 0));
            this.Add(8, new Color(246, 70, 0));
            this.Add(7, new Color(246, 95, 0));
            this.Add(6, new Color(246, 120, 0));
            this.Add(5, new Color(246, 145, 0));
            this.Add(4, new Color(246, 170, 0));
            this.Add(3, new Color(246, 195, 0));
            this.Add(2, new Color(246, 220, 0));
            this.Add(1, new Color(246, 255, 0));
        }

    }
}
