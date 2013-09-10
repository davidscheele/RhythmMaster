using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

public class PointEffect
{
    private Vector2 topLeftPosition;
    private Texture2D texture;
    private int gameTimeOnInit;

    public Vector2 Center
    {
        get
        {
            Vector2 _center = new Vector2(0,0);
            _center.X = topLeftPosition.X + 60;
            _center.Y = topLeftPosition.Y + 60;
            return _center;
        }
        set
        {
            topLeftPosition.X = value.X - 60;
            topLeftPosition.Y = value.Y - 60;
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


    public PointEffect(Texture2D _texture, SoundEffect _soundEffect, Vector2 _centerPoint, int _gameTimeOnInit)
    {
        gameTimeOnInit = _gameTimeOnInit;
        this.Texture = _texture;
        this.Center = _centerPoint;
        _soundEffect.Play();
    }

    public Boolean Draw(SpriteBatch _spriteBatch, int _actualGameTime)
    {

        if (gameTimeOnInit + 1000 > _actualGameTime)
        {
            _spriteBatch.Draw(texture, topLeftPosition, Color.White);
            return false;
        }
        else
        {
            return true;
        }

    }



}