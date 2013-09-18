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
    class LoadSelectionButton : NavigationButton
    {
        public LoadSelectionButton(Texture2D _texture)
        {
            this.Texture = _texture;
            this.Color = Color.Aqua;
        }
    }
}
