using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace RhythmMaster.PlayMenu
{
    interface PlayObject
    {
        void LoadContent(ContentManager _contentManager);
        void Draw(SpriteBatch _spriteBatch);
    }
}
