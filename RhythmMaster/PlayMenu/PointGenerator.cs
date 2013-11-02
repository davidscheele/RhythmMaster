using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using RhythmMaster.Functions;

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

        static int totalPoints = 0;
        public static int TotalPoints
        {
            get { return totalPoints; }
        }

        static int multiplicator = 1;
        public static String Multiplicator
        {
            get { return "x" + multiplicator; }
        }

        public static void Load(ContentManager _contentManager)
        {
            fullpointsSoundeffect = _contentManager.Load<SoundEffect>("PointEffects/tambourine");
            halfpointsSoundeffect = _contentManager.Load<SoundEffect>("PointEffects/hi-hat");
            nopointsSoundeffect = _contentManager.Load<SoundEffect>("PointEffects/drum");

            fullpointsTexture = _contentManager.Load<Texture2D>("PointEffects/300points");
            halfpointsTexture = _contentManager.Load<Texture2D>("PointEffects/100points");
            nopointsTexture = _contentManager.Load<Texture2D>("PointEffects/nopoints");
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

        public static void ResetPoints()
        {
            multiplicator = 1;
            totalPoints = 0;
        }

        public static void generatePointEffect(Vector2 _center, float _scale, int _currentPlayTime)
        {
            if (_scale >= 0.7f)
            {
                pointEffectsDictionary.Add(_currentPlayTime, new PointEffect(nopointsTexture, nopointsSoundeffect, _center, _currentPlayTime));
                multiplicator = 1;
            }
            else if ((_scale < 0.7 && _scale >= 0.6f) || _scale <= 0.4)
            {
                pointEffectsDictionary.Add(_currentPlayTime, new PointEffect(halfpointsTexture, halfpointsSoundeffect, _center, _currentPlayTime));
                totalPoints += 100*multiplicator;
                multiplicator++;
            }
            else
            {
                pointEffectsDictionary.Add(_currentPlayTime, new PointEffect(fullpointsTexture, fullpointsSoundeffect, _center, _currentPlayTime));
                totalPoints += 300*multiplicator;
                multiplicator++;
            }


        }
        public static void generatePointEffect(Vector2 _center, PointEffectState _state, int _currentPlayTime)
        {
            switch (_state)
            {
                case PointEffectState.FullPoints:
                    pointEffectsDictionary.Add(_currentPlayTime, new PointEffect(fullpointsTexture, fullpointsSoundeffect, _center, _currentPlayTime));
                totalPoints += 300*multiplicator;
                multiplicator++;
                    break;
                case PointEffectState.ReducedPoints:
                    pointEffectsDictionary.Add(_currentPlayTime, new PointEffect(halfpointsTexture, halfpointsSoundeffect, _center, _currentPlayTime));
                totalPoints += 100*multiplicator;
                multiplicator++;
                    break;
                case PointEffectState.NoPoints:
                    pointEffectsDictionary.Add(_currentPlayTime, new PointEffect(nopointsTexture, nopointsSoundeffect, _center, _currentPlayTime));
                multiplicator = 1;
                    break;
            }
        }

    }
