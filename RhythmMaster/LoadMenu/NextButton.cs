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
    class NextButton : NavigationButton
    {
        public NextButton(Vector2 _position)
        {
            this.TopLeft = _position;
            this.AssetName = "LoadMenu/nextbutton";
            this.Color = Color.Aqua;
        }
    }
}
