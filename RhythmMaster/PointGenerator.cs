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
        static Dictionary<int, PointEffect> pointEffectsDictionary = new Dictionary<int, PointEffect>();

        public static void Load(ContentManager _contentManager)
        {
            fullpointsSoundeffect = _contentManager.Load<SoundEffect>("tambourine");
            halfpointsSoundeffect = _contentManager.Load<SoundEffect>("hi-hat");
            nopointsSoundeffect = _contentManager.Load<SoundEffect>("drum");

            fullpointsTexture = _contentManager.Load<Texture2D>("300points");
            halfpointsTexture = _contentManager.Load<Texture2D>("100points");
            nopointsTexture = _contentManager.Load<Texture2D>("nopoints");
        }

        public static void Draw(SpriteBatch _spriteBatch, int _timeSinceStart)
        {
            PointEffect tempPointEffect;
            Dictionary<int, PointEffect> tempPointEffectsDictionary = pointEffectsDictionary;

            foreach (int key in pointEffectsDictionary.Keys)
            {

                if (key <= _timeSinceStart)
                {
                    pointEffectsDictionary.TryGetValue(key, out tempPointEffect);

                    if (tempPointEffect.Draw(_spriteBatch, _timeSinceStart))
                    {
                        tempPointEffectsDictionary.Remove(key);
                        break;
                    }

                }
                else
                {
                    break;
                }
            }
            pointEffectsDictionary = tempPointEffectsDictionary;

        }

        public static void generatePointEffect(Vector2 _center, float _scale, int _gameTimeSinceStart)
        {
            if (_scale >= 0.7f)
            {
                pointEffectsDictionary.Add(_gameTimeSinceStart, new PointEffect(nopointsTexture, nopointsSoundeffect, _center, _gameTimeSinceStart));
            }
            else if ((_scale < 0.7 && _scale >= 0.6f) || _scale <= 0.4)
            {
                pointEffectsDictionary.Add(_gameTimeSinceStart, new PointEffect(halfpointsTexture, halfpointsSoundeffect, _center, _gameTimeSinceStart));
            }
            else
            {
                pointEffectsDictionary.Add(_gameTimeSinceStart, new PointEffect(fullpointsTexture, fullpointsSoundeffect, _center, _gameTimeSinceStart));
            }


        }

    }