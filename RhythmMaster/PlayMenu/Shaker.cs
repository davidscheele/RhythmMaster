using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

using RhythmMaster.Functions;

namespace RhythmMaster.PlayMenu
{
    public class Shaker : PlayObject
    {
        int shakesToComplete;
        int shakesDone;
        float percentageFloat;
        int colorChooseInt;
        Texture2D texture;
        SoundEffect progressBleep;

        ShakerColorList colorList = new ShakerColorList();

        public Shaker(int _length)
        {
            shakesToComplete = (int)(_length / 100) / 2;        //5 Shakes per second
            shakesDone = 0;
        }

        public void LoadContent(ContentManager _contentManager)
        {
            texture = _contentManager.Load<Texture2D>("shakeindicator2");
            progressBleep = _contentManager.Load<SoundEffect>("bleep");
        }

        public Boolean completeSomeMore()
        {
            if (shakesDone < shakesToComplete)
            {
                shakesDone++;
                float pitch =-1f + (((float)shakesDone / (float)shakesToComplete) * 2f) ;
                progressBleep.Play(1f, pitch, 0f);
                return false;
            }
            else
            {
                shakesDone = 0; //debug resetter
                return true;
            }

        }

        public void Draw(SpriteBatch _spriteBatch)
        {
            percentageFloat =  (((float)shakesDone/(float)shakesToComplete) *  48f);
            colorChooseInt = (int) (((float)shakesDone / (float)shakesToComplete) * (float) 10) + 1;

            Color tempColor = Color.Wheat;
            colorList.TryGetValue(colorChooseInt, out tempColor);

            _spriteBatch.Draw(texture, new Vector2(0, 0), null, tempColor, 0f, Vector2.Zero, new Vector2(1f, percentageFloat), SpriteEffects.None, 0f);

        }

    }
}
