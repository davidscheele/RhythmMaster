using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

namespace RhythmMaster.PlayMenu
{
    public abstract class ClickablePlayObject : PlayObject
    {
        
        

        private Boolean draw = true;
        private Vector2 position = new Vector2(0, 0);
        public Boolean thisDraw
        {
            set
            {
                draw = value;
            }
        }
        public PlayObjectIdentifier Identifier()
        {
            return PlayObjectIdentifier.Beat;
        }
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
        private BeatRing beatRing;
        public BeatRing BeatRing
        {
            get
            {
                return beatRing;
            }
            set
            {
                beatRing = value;
            }
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
        private float scale = 1f;
        public float Scale
        {
            get
            {
                return scale;
            }
        }

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
        private Texture2D texture;
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

        public ClickablePlayObject()
        {


        }
        public void LoadContent(ContentManager _contentManager)
        {
            this.texture = _contentManager.Load<Texture2D>(assetName);
            this.BeatRing.LoadContent(_contentManager);
        }
        public void Draw(SpriteBatch _spriteBatch)
        {
            if (this.draw)
            {
                _spriteBatch.Draw(texture, position, null, Color.Yellow, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
                if (!this.BeatRing.Draw(_spriteBatch))
                {
                    PointGenerator.generatePointEffect(this.Center, 2f, (int)MediaPlayer.PlayPosition.TotalMilliseconds);
                    this.vanish();
                }
            }


            this.Bounds = new Rectangle((int)this.TopLeft.X, (int)this.TopLeft.Y, (int)this.Width, (int)this.Height);



        }
        public void vanish()
        {
            this.Center = new Vector2(-500, -500);
            this.draw = false;
        }





    }
}