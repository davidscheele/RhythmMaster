using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using RhythmMaster.Functions;

public class BeatRing
{
    private float downSizeSpeed = 0.02f; //Speed at which the Ring shrinks down
    private float upperNoPointsLimit = 1f; //Scale of the Ring at the start of appearing
    private float lowerNoPointsLimit = 0.8f; //Maximum Scale can get lowered to before Player is awarded reduced Points
    private float upperFullPointsLimit = 0.55f; //Minimum Scale must get lowered to so Player is awarded full Points 
    private float lowerFullPointsLimit = 0.45f; //Maximum Scale can get lowered to before Player is awarded reduced Points again 
    private float failLimit = 0.35f; //Maximum Scale can get lowered to before respective Beat is ended with Fail
    private ClickablePlayObject hostObject; //Object associated with this BeatRing
    public ClickablePlayObject HostObject
    {
        get { return hostObject; }
        set { hostObject = value; }
    }
    private Vector2 position = new Vector2(0, 0); //Top Left Position of the Ring
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
        get { return position; }
        set { position = value; }
        
    }
    private Vector2 originalPosition = new Vector2(0,0); //Original Position of the BeatRing needed for reference when scaling down.
    private Vector2 OriginalTopLeft
    {
        get { return originalPosition; }
        set { originalPosition = value; }
    }
    private float scale = 1f; //The Scale of the BeatRing Object
    public float Scale
    {
        get { return scale; }
    }
    public Texture2D Texture
    {
        get { return texture; }
        set { texture = value; }
    }
    private Texture2D texture;
    public float Height
    {
        get { return this.texture.Height; }
    }
    public float Width
    {
        get { return this.texture.Width; }
    }
    private String assetName = "beatring_s";
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
        if (checkScale())
        {
            _spriteBatch.Draw(texture, position, null, Color.White, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
        }
            //this.scaleUpAndDown();
            return checkScale();
    }

    private Boolean checkPosition()
    {
        if (this.HostObject.TopLeft.X <= this.TopLeft.X)
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
        if (scale <= 0.35f)
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





}
