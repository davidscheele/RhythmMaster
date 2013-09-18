using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input.Touch;

namespace RhythmMaster
{
    class ListForwardButton : NavigationButton
    {
        public ListForwardButton(Vector2 _position)
        {
            this.TopLeft = _position;
            this.AssetName = "LoadMenu/listforwardbutton";
            this.Color = Color.Aqua;
        }
    }
}
