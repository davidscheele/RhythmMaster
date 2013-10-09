using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.IO.IsolatedStorage;
using System.Text.RegularExpressions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using RhythmMaster.Functions;

namespace RhythmMaster
{
     interface LoadMenu
    {

        void Draw(SpriteBatch spriteBatch);
        GameState checkClick(Vector2 tapLocation);

    }
}
