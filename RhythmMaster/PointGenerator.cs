using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

    public static class PointGenerator
    {
        static SoundEffect fullpointsSoundeffect;
        static SoundEffect halfpointsSoundeffect;
        static SoundEffect nopointsSoundeffect;

        static Texture2D fullpointsTexture;
        static Texture2D halfpointsTexture;
        static Texture2D nopointsTexture;

        static List<PointEffect> pointEffectList = new List<PointEffect>();

        public static void Load(ContentManager _contentManager)
        {
            fullpointsSoundeffect = _contentManager.Load<SoundEffect>("tambourine");
            halfpointsSoundeffect = _contentManager.Load<SoundEffect>("hi-hat");
            nopointsSoundeffect = _contentManager.Load<SoundEffect>("drum");

            fullpointsTexture = _contentManager.Load<Texture2D>("300points");
            halfpointsTexture = _contentManager.Load<Texture2D>("100points");
            nopointsTexture = _contentManager.Load<Texture2D>("nopoints");
        }

        public static void Draw(SpriteBatch _spriteBatch)
        {
            foreach (PointEffect _pointEffect in pointEffectList)
            {
                _pointEffect.Draw(_spriteBatch);
            }

        }

        public static void generatePointEffect(Vector2 _center, float _scale)
        {
            if (_scale >= 0.7f)
            {
                pointEffectList.Add(new PointEffect(nopointsTexture, nopointsSoundeffect, _center));
            }
            else if ((_scale < 0.7 && _scale >= 0.6f) || _scale <= 0.4)
            {
                pointEffectList.Add(new PointEffect(halfpointsTexture, halfpointsSoundeffect, _center));
            }
            else
            {
                pointEffectList.Add(new PointEffect(fullpointsTexture, fullpointsSoundeffect, _center));
            }


        }

    }
