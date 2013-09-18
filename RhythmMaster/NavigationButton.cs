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
    public abstract class NavigationButton
    {
        Color color;
        public Color Color
        {
            get
            {
                return color;
            }
            set
            {
                color = value;
            }
        }

        private Vector2 position;
        public Vector2 Center
        {
            get
            {
                Vector2 _position = position;
                _position.X = _position.X + (this.Width / 2);
                _position.Y = _position.Y + (this.Height / 2);
                return _position;
            }
            set
            {
                position.X = value.X - (this.Width / 2);
                position.Y = value.Y - (this.Height / 2);
            }
        }
        public Vector2 TopLeft
        {
            get
            {
                return position;
            }
            set
            {
                position = value;
            }
        }
        private Texture2D texture;
        public Texture2D Texture
        {
            get
            {
                return texture;
            }
            set
            {
                texture = value;
            }
        }
        public float Height
        {
            get
            {
                return this.texture.Height;
            }
        }
        public float Width
        {
            get
            {
                return this.texture.Width;
            }
        }
        private String assetName;
        public String AssetName
        {
            get { return this.assetName; }
            set { this.assetName = value; }
        }
        private Rectangle bounds;
        public Rectangle Bounds
        {
            get
            {
                return bounds;
            }
            set
            {
                bounds = value;
            }
        }
        public NavigationButton()
	{

	}

    public void Load(ContentManager _contentManager)
    {
        this.texture = _contentManager.Load<Texture2D>(assetName);
    }
    public void Draw(SpriteBatch _spriteBatch)
    {
            _spriteBatch.Draw(texture, position, null, color);
            this.Bounds = new Rectangle((int)this.TopLeft.X, (int)this.TopLeft.Y, (int)this.Width, (int)this.Height);
    }
    public void Draw(SpriteBatch _spriteBatch, Vector2 _position)
    {
        this.TopLeft = _position;
        _spriteBatch.Draw(texture, _position, null, color);
        this.Bounds = new Rectangle((int)this.TopLeft.X, (int)this.TopLeft.Y, (int)this.Width, (int)this.Height);
    }
    public Boolean checkClick(GestureSample gesture)
    {
        if (this.Bounds.Intersects(new Rectangle((int)gesture.Position.X, (int)gesture.Position.Y, 1, 1)))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    }
}
