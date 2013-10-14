using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace RhythmMaster.PlayMenu
{
    public class Beat : ClickablePlayObject
    {

        public Beat(Vector2 _position)
        {
            this.TopLeft = _position;
            this.AssetName = "beat_s_w";
            this.BeatRing = new BeatRing(new Vector2(_position.X - 50, _position.Y - 50));

        }

    }
}