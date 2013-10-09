using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using RhythmMaster.Functions;

namespace RhythmMaster.PlayMenu
{
    public class Shaker
    {

        int completionInt = 0;

        Texture2D texture;

        ShakerColorList colorList = new ShakerColorList();

        public Shaker()
        {
        }

        public void LoadContent(ContentManager _contentManager)
        {
            texture = _contentManager.Load<Texture2D>("shakeindicator");
        }

        public Boolean completeSomeMore()
        {
            if (completionInt <= 10)
            {
                completionInt++;
                return false;
            }
            else
            {
                return true;
            }
        }

        public void Draw(SpriteBatch _spriteBatch)
        {

            for (int i = 1; i <= completionInt; i++)
            {
                Color tempColor = Color.Wheat;
                colorList.TryGetValue(i, out tempColor);
                _spriteBatch.Draw(texture, new Vector2(0, 480f - i*48f), tempColor);

            }

        }

    }
}
