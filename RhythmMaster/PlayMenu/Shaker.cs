using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

using RhythmMaster.Functions;

namespace RhythmMaster.PlayMenu
{
    public class Shaker : PlayObject
    {
        int shakesToComplete;
        int shakesDone;
        float percentageFloat;
        int colorChooseInt;
        int length;
        Texture2D texture;
        SoundEffect progressBleep;
        SpriteFont shakerFont;

        public int Length
        {
            get { return length; }
        }

        ShakerColorList colorList = new ShakerColorList();

        public Shaker(int _length)
        {
            length = _length;
            shakesToComplete = (int)(_length / 100) / 4;        //2.5 Shakes per second
            shakesDone = 0;
        }

        public void LoadContent(ContentManager _contentManager)
        {
            shakerFont = _contentManager.Load<SpriteFont>("ShakerFont");
            texture = _contentManager.Load<Texture2D>("shakeindicator2");
            progressBleep = _contentManager.Load<SoundEffect>("bleep");
        }

        public void completeSomeMore()
        {
            if (shakesDone < shakesToComplete)
            {
                shakesDone++;
                float pitch = -1f + (((float)shakesDone / (float)shakesToComplete) * 2f);
                progressBleep.Play(1f, pitch, 0f);
            }

        }

        public PlayObjectIdentifier Identifier()
        {
            return PlayObjectIdentifier.Shaker;
        }

        public Boolean isComplete()
        {
            if (shakesDone >= shakesToComplete)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        public void Draw(SpriteBatch _spriteBatch)
        {

            percentageFloat =  (((float)shakesDone/(float)shakesToComplete) *  48f);
            colorChooseInt = (int) (((float)shakesDone / (float)shakesToComplete) * (float) 10) + 1;

            Color tempColor = Color.Yellow;
            colorList.TryGetValue(colorChooseInt, out tempColor);

            _spriteBatch.Draw(texture, new Vector2(0, 0), null, Color.Black, 0f, Vector2.Zero, new Vector2(1f, 480f), SpriteEffects.None, 0f);

            _spriteBatch.Draw(texture, new Vector2(0, 0), null, tempColor, 0f, Vector2.Zero, new Vector2(1f, percentageFloat), SpriteEffects.None, 0f);

            _spriteBatch.DrawString(shakerFont, "SHAKE!!", new Vector2(150, 100), Color.Red);

        }

    }
}
