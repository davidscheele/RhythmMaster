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
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteFont debugFont;
        String debug01String = "Startup!";
        int timeSinceStart = 0;
        int startTime = 1;

        Boolean testBoolForRing = true;
        //String debug02String = "";
        //String debug03String = "";
        String debug04String = "";
        //String debug05String = "";

        Dictionary<long, Clickable> BeatDictionary = new Dictionary<long,Clickable>();

       List<BeatTimerData> BeatTimerList = new List<BeatTimerData>();

       //List<Clickable> BeatList = new List<Clickable>();

        Clickable testbeat;
        //BeatRing testring;
        

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            TouchPanel.EnabledGestures = GestureType.Tap;
            BeatTimerList.Add(new BeatTimerData(1, new Vector2(200, 100), new Vector2(0, 0), false, false));
            BeatTimerList.Add(new BeatTimerData(50, new Vector2(200, 200), new Vector2(0,0), false, false));
            BeatTimerList.Add(new BeatTimerData(70, new Vector2(300, 200), new Vector2(0,0), false, false));
            BeatTimerList.Add(new BeatTimerData(100, new Vector2(400, 200), new Vector2(0,0), false, false));



            // Frame rate is 30 fps by default for Windows Phone.
            TargetElapsedTime = TimeSpan.FromTicks(333333);

            // Extend battery life under lock.
            InactiveSleepTime = TimeSpan.FromSeconds(1);
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            testbeat = new Beat(new Vector2(200, 100));
            //testring = new BeatRing(new Vector2(100, 100));
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            debugFont = Content.Load<SpriteFont>("Debugfont");

            foreach (BeatTimerData btd in BeatTimerList)
            {
                BeatDictionary.Add(btd.Timestamp, new Beat(btd.StartPosition));

            }

            foreach (KeyValuePair<long, Clickable> kvp in BeatDictionary)
            {
                kvp.Value.LoadContent(this.Content);
            }

            //testbeat.LoadContent(this.Content);
            //testring.LoadContent(this.Content);
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            //while (TouchPanel.IsGestureAvailable)
            //{
            //    GestureSample gesture = TouchPanel.ReadGesture();

            //    switch (gesture.GestureType)
            //    {
            //        case (GestureType.Tap):
            //            if (checkIntersect(gesture.Position))
            //            {
            //                if (testbeat != null)
            //                {
            //                    if (testbeat.BeatRing.Scale > 0.6f || testbeat.BeatRing.Scale < 0.4f)
            //                    {
            //                        debug04String = "FAIL!!!!";
            //                        testbeat = null;
            //                    }
            //                    else
            //                    {
            //                        debug04String = "SPOT ON!!!";
            //                        testbeat = null;
            //                    }
            //                }
            //            }
            //            break;

            //    }

            //}

            // TODO: Add your update logic here

            //foreach (BeatTimerData btd in BeatTimerList)
            //{
            //    debug04String = btd.Timestamp + ";" + btd.StartPosition + ";" + btd.EndPosition + ";" + btd.IsSlider + ";" + btd.IsSpinner;
            //}

            base.Update(gameTime);
        }

        //private Boolean checkIntersect(Vector2 tapPosition)
        //{
        //    if (testbeat != null)
        //    {
        //        if (testbeat.Bounds.Intersects(new Rectangle((int)tapPosition.X, (int)tapPosition.Y, 1, 1)))
        //        {
        //            return true;
        //        }
        //        else
        //        {
        //            return false;
        //        }
        //    }
        //    return false;
        //}

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            int gameTimeSinceTheStart = (int) gameTime.TotalGameTime.TotalMilliseconds;
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();
            //spriteBatch.DrawString(debugFont, debug01String, new Vector2(10, 10), Color.Black);

            spriteBatch.DrawString(debugFont, gameTimeSinceTheStart.ToString(), new Vector2(10, 20), Color.Black);
            //spriteBatch.DrawString(debugFont, debug04String, new Vector2(10, 30), Color.Black);
            spriteBatch.DrawString(debugFont, debug04String, new Vector2(10, 40), Color.Black);



            Clickable testBeat;
            if (BeatDictionary.TryGetValue(gameTimeSinceTheStart, out testBeat))
            {
                testBeat.Draw(spriteBatch);
            }
            //spriteBatch.DrawString(debugFont, debug05String, new Vector2(10, 50), Color.Black);
            //if (testbeat != null)
            //{
            //testbeat.Draw(this.spriteBatch);
            //}
            //if (testBoolForRing)
            //{
            //    testBoolForRing = testring.Draw(this.spriteBatch);
            //}
            // TODO: Add your drawing code here
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
