using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Media;

namespace RhythmMaster
{

    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteFont debugFont;
        String debug01String = "Startup!";
        int timeSinceStart = 0;
        int startTime = 1;
        int gameTimeSinceStart = 0;

        Boolean testBoolForRing = true;
        String debug04String = "";

        Dictionary<int, Clickable> BeatDictionary = new Dictionary<int,Clickable>();

       List<BeatTimerData> BeatTimerList = new List<BeatTimerData>();
        

        public Game1()
        {

            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            TouchPanel.EnabledGestures = GestureType.Tap;
            BeatTimerList.Add(new BeatTimerData(1000, new Vector2(200, 100), new Vector2(0, 0), false, false));
            BeatTimerList.Add(new BeatTimerData(3000, new Vector2(200, 200), new Vector2(0,0), false, false));
            BeatTimerList.Add(new BeatTimerData(3500, new Vector2(300, 200), new Vector2(0,0), false, false));
            BeatTimerList.Add(new BeatTimerData(5000, new Vector2(400, 200), new Vector2(0,0), false, false));



            // Frame rate is 30 fps by default for Windows Phone.
            TargetElapsedTime = TimeSpan.FromTicks(333333);

            // Extend battery life under lock.
            InactiveSleepTime = TimeSpan.FromSeconds(1);
        }

        protected override void Initialize()
        {
            base.Initialize();
        }


        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            debugFont = Content.Load<SpriteFont>("Debugfont");
            PointGenerator.Load(this.Content);

            foreach (BeatTimerData btd in BeatTimerList)
            {
                BeatDictionary.Add(btd.Timestamp, new Beat(btd.StartPosition));

            }

            foreach (KeyValuePair<int, Clickable> kvp in BeatDictionary)
            {
                kvp.Value.LoadContent(this.Content);
            }

        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            while (TouchPanel.IsGestureAvailable)
            {
                GestureSample gesture = TouchPanel.ReadGesture();

                switch (gesture.GestureType)
                {
                    case (GestureType.Tap):
                        checkIntersect(gesture.Position);
                        break;
                }
            }
            base.Update(gameTime);
        }

        private void checkIntersect(Vector2 tapPosition)
        {
            Clickable testBeat;
            foreach (int key in BeatDictionary.Keys)
            {
                if (key <= gameTimeSinceStart)
                {
                    BeatDictionary.TryGetValue(key, out testBeat);

                        if (testBeat.Bounds.Intersects(new Rectangle((int)tapPosition.X, (int)tapPosition.Y, 1, 1)))
                        {
                            PointGenerator.generatePointEffect(testBeat.Center, testBeat.BeatRing.Scale, gameTimeSinceStart);
                            testBeat.thisDraw = false;
                        }

                    
                }
                else
                {
                    break;
                }
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            gameTimeSinceStart = (int) gameTime.TotalGameTime.TotalMilliseconds;
            
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();

            spriteBatch.DrawString(debugFont, gameTimeSinceStart.ToString(), new Vector2(10, 20), Color.Black);
            spriteBatch.DrawString(debugFont, startTime.ToString(), new Vector2(10, 30), Color.Black);
            spriteBatch.DrawString(debugFont, debug04String, new Vector2(10, 40), Color.Black);

            PointGenerator.Draw(spriteBatch, gameTimeSinceStart);

            Clickable tempClickable;

            foreach (int key in BeatDictionary.Keys)
            {
                if (key <= gameTimeSinceStart)
                {
                    BeatDictionary.TryGetValue(key, out tempClickable);
                    tempClickable.Draw(spriteBatch);
                }
                else
                {
                    break;
                }
            }

            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
