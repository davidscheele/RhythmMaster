using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

public class BeatRing
{
    private Clickable hostBeat;
    public Clickable Beat
    {
        get
        {
            return hostBeat;
        }
        set
        {
            hostBeat = value;
        }
    }
    private Vector2 position = new Vector2(0, 0);
    public Vector2 Center
    {
        get
        {
            Vector2 _position = originalPosition;
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
    private Vector2 originalPosition = new Vector2(0,0);
    private Vector2 OriginalTopLeft
    {
        get
        {
            return originalPosition;
        }
        set
        {
            originalPosition = value;
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
    public String Scale
    {
        get
        {
            return scale.ToString();
        }
    }
    private Boolean scaleswitcher = true;
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
    private String assetName = "beatring_s";
    public String AssetName
    {
        get { return this.assetName; }
        set { this.assetName = value; }
    }

	public BeatRing(Vector2 posi)
	{
        this.TopLeft = new Vector2(posi.X - 100,posi.Y - 100);
        this.OriginalTopLeft = posi;

	}

    public void LoadContent(ContentManager _contentManager)
    {
        this.texture = _contentManager.Load<Texture2D>(assetName);

    }
    public Boolean Draw(SpriteBatch _spriteBatch)
    {
        scaleDown();
        giveNewTopLeft();
            _spriteBatch.Draw(texture, position, null, Color.White, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);

            //this.scaleUpAndDown();
            return checkScale();
    }

    private Boolean checkPosition()
    {
        if (this.Beat.TopLeft.X <= this.TopLeft.X)
        {
            return true;
        }
        else
        {
            return false;
        }

    }
    private void giveNewTopLeft()
    {
        Vector2 _topLeft = new Vector2(0,0);
        _topLeft.X = (100 - (100 * scale)) + this.OriginalTopLeft.X;
        _topLeft.Y = (100 - (100 * scale)) + this.OriginalTopLeft.Y;
        this.TopLeft = _topLeft;
    }
    public Boolean checkScale() //Bigger than 0.5f scale
    {
        if (scale <= 0.5f)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
    private void scaleDown()
    {
        scale = scale - 0.02f;
    }
    private void scaleUpAndDown()
    {
        if (scaleswitcher)
        {
            scale = scale + 0.1f;
        }
        else
        {
            scale = scale - 0.1f;
        }

        if (scale >= 2f)
        {
            scaleswitcher = false;
        }
        else
        {
            if (scale <= 0.1f)
            {
                scaleswitcher = true;
            }
        }
    }





}
