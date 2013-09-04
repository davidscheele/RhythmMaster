using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

public class Beat : Clickable
{
 

	public Beat(Vector2 _position)
	{
        this.TopLeft = _position;
        this.AssetName = "beat_s";
        this.BeatRing = new BeatRing(new Vector2(_position.X - 50, _position.Y - 50));

	}

  



}
