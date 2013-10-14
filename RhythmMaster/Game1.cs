using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Media;


using ShakeGestures;

using RhythmMaster.Functions;
using RhythmMaster.PlayMenu;


//Testlibs
using System.IO;
using System.IO.IsolatedStorage;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Diagnostics;
using System.Text;
//Testlibs

namespace RhythmMaster
{

    public class Game1 : Microsoft.Xna.Framework.Game
    {
        NavigationButton playBeatmapsButton = new PlayBeatmapsButton(new Vector2(100, 100));
        NavigationButton createBeatmapsButton = new CreateBeatmapsButton(new Vector2(400, 300));
        NavigationButton returnToMainMenuButton = new ReturnToMainMenuButton(new Vector2(675, 420));
        GameState CurrentGameState = GameState.MainMenu;
        XmlConverter xmlConverter = new XmlConverter();
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteFont debugFont;
        LoadMenu loadMenu;

        Shaker testShaker = new Shaker(1200);

        int debugint = 0;
        String debugstring1 = "nothin";
        String debugstring2 = "nothin";
        String debugstring3 = "nothin";

        Song munchymonk;

        Boolean isHeld = false;

        int gameTimeSinceStart = 0;
        int gameTimeSincePlaying = 0;
        int gameTimeSinceCreating = 0;
        int gameTimeSinceHolding = 0;

        Dictionary<int, ClickablePlayObject> BeatDictionary = new Dictionary<int,ClickablePlayObject>();

       List<BeatTimerData> BeatTimerList = new List<BeatTimerData>();
        

        public Game1()
        {   
            ShakeGesturesHelper.Instance.ShakeGesture += new EventHandler<ShakeGestureEventArgs>(Instance_ShakeGesture);
            ShakeGesturesHelper.Instance.MinimumRequiredMovesForShake = 1;
            ShakeGesturesHelper.Instance.Active = true;

            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            TouchPanel.EnabledGestures = GestureType.Tap | GestureType.Hold;
            BeatTimerList.Add(new BeatTimerData(1000, new Vector2(200, 100), new Vector2(0, 0), 0, false, false));
            BeatTimerList.Add(new BeatTimerData(3000, new Vector2(200, 200), new Vector2(0,0), 0, false, false));
            BeatTimerList.Add(new BeatTimerData(3500, new Vector2(300, 200), new Vector2(0,0), 0, false, false));
            BeatTimerList.Add(new BeatTimerData(5000, new Vector2(400, 200), new Vector2(0,0), 0, false, false));

            debugMessageXMLFiles();
            
            // Frame rate is 30 fps by default for Windows Phone.
            TargetElapsedTime = TimeSpan.FromTicks(333333);

            // Extend battery life under lock.
            InactiveSleepTime = TimeSpan.FromSeconds(1);
        }

        private void Instance_ShakeGesture(object sender, ShakeGestureEventArgs e)
        {
            debugstring1 = "shook it";
            if (CurrentGameState == GameState.TestMenu)
            {                
                testShaker.completeSomeMore();
            }
        }


        public void Save(string filename)
        {

        }

        private void checkIntersect(Vector2 tapPosition)
        {
            ClickablePlayObject testBeat;
            foreach (int key in BeatDictionary.Keys)
            {
                if (key <= gameTimeSincePlaying)
                {
                    BeatDictionary.TryGetValue(key, out testBeat);

                    if (testBeat.Bounds.Intersects(new Rectangle((int)tapPosition.X, (int)tapPosition.Y, 1, 1)))
                    {
                        PointGenerator.generatePointEffect(testBeat.Center, testBeat.BeatRing.Scale, gameTimeSincePlaying);
                        testBeat.thisDraw = false;
                    }


                }
                else
                {
                    break;
                }
            }
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        private void LoadLateContent()
        {
            BeatDictionary = new Dictionary<int, ClickablePlayObject>();
            foreach (BeatTimerData btd in BeatTimerList)
            {
                BeatDictionary.Add(btd.Timestamp, new Beat(btd.StartPosition));

            }

            foreach (KeyValuePair<int, ClickablePlayObject> kvp in BeatDictionary)
            {
                kvp.Value.LoadContent(this.Content);
            }
        }

        protected override void LoadContent()
        {
            
            spriteBatch = new SpriteBatch(GraphicsDevice);
            debugFont = Content.Load<SpriteFont>("Debugfont");
            PointGenerator.Load(this.Content);
            playBeatmapsButton.Load(this.Content);
            createBeatmapsButton.Load(this.Content);
            returnToMainMenuButton.Load(this.Content);
            testShaker.LoadContent(this.Content);

            munchymonk = Content.Load<Song>("munchymonk");

            foreach (BeatTimerData btd in BeatTimerList)
            {
                BeatDictionary.Add(btd.Timestamp, new Beat(btd.StartPosition));

            }

            foreach (KeyValuePair<int, ClickablePlayObject> kvp in BeatDictionary)
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


            switch (CurrentGameState){
                case GameState.MainMenu:                                            //Check for Clicks in Main Menu
                    while (TouchPanel.IsGestureAvailable)
                    {
                        GestureSample gesture = TouchPanel.ReadGesture();

                        switch (gesture.GestureType)
                        {
                            case (GestureType.Tap):
                                if(createBeatmapsButton.checkClick(gesture))
                                {
                                    this.CurrentGameState = GameState.BeatmapCreator; // actual
                                    //this.CurrentGameState = GameState.TestMenu; //testing purposes
                                    BeatTimerList = new List<BeatTimerData>(); //Actual
                                    //MediaPlayer.Play(munchymonk); //actual
                                };
                                if(playBeatmapsButton.checkClick(gesture))
                                {
                                    this.CurrentGameState = GameState.XMLLoadMenu;
                                    loadMenu = new XMLLoadMenu(this.Content);
                                };
                                break;
                        }
                    }
                    break;
                case GameState.XMLLoadMenu:
                    while (TouchPanel.IsGestureAvailable)
                    {
                        GestureSample gesture = TouchPanel.ReadGesture();

                        switch (gesture.GestureType)
                        {
                            case (GestureType.Tap):
                                switch (loadMenu.checkClick(gesture.Position))
                                {
                                    case GameState.SongLoadMenu:
                                        this.CurrentGameState = GameState.SongLoadMenu;
                                        loadMenu = new SongLoadMenu(this.Content);
                                        break;

                                    case GameState.MainMenu:
                                        this.CurrentGameState = GameState.MainMenu;
                                        break;
                                }
                                
                                break;
                        }
                    }
                    break;

                case GameState.SongLoadMenu:
                    while (TouchPanel.IsGestureAvailable)
                    {
                        GestureSample gesture = TouchPanel.ReadGesture();

                        switch (gesture.GestureType)
                        {
                            case (GestureType.Tap):
                                switch (loadMenu.checkClick(gesture.Position))
                                {

                                    case GameState.Playing:
                                        this.BeatTimerList = xmlConverter.loadBeatmapXML(PointGenerator.gettestxml()); //getXML auslagern!!
                                        this.LoadLateContent();
                                        MediaPlayer.Play(munchymonk);
                                        CurrentGameState = GameState.Playing;
                                        break;

                                    case GameState.XMLLoadMenu:
                                        this.CurrentGameState = GameState.XMLLoadMenu;
                                        loadMenu = new XMLLoadMenu(this.Content);
                                        break;

                                    case GameState.MainMenu:
                                        this.CurrentGameState = GameState.MainMenu;
                                        break;
                                }

                                break;
                        }
                    }
                    break;
                case GameState.Playing:                                             //Check for Beat klicks
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
                    break;

                case GameState.BeatmapCreator:                                      //Check for Taps to create Beatmap

                    TouchCollection touchcol = TouchPanel.GetState();
                    foreach (TouchLocation touchlocation in touchcol)
                    {
                        if (touchlocation.State == TouchLocationState.Released && isHeld)
                        {
                            int heldTime = MediaPlayer.PlayPosition.Milliseconds - gameTimeSinceHolding;
                            //debugstring1 = "Held for: " + heldTime;
                            BeatTimerList.Add(new BeatTimerData(gameTimeSinceHolding, new Vector2(0, 0), new Vector2(0, 0), heldTime, false, true)); //Adds a Shaker reference with the start time as the screen was touched and the total shaker time as the touch was held.
                            isHeld = false;
                            }
                    }   

                    while (TouchPanel.IsGestureAvailable)
                    {
                        GestureSample gesture = TouchPanel.ReadGesture();
                        
                        switch (gesture.GestureType)
                        {
                                
                            case (GestureType.Tap):
                                if (returnToMainMenuButton.checkClick(gesture))
                                {
                                    MediaPlayer.Stop();
                                    this.CurrentGameState = GameState.SaveMenu;
                                    Guide.BeginShowKeyboardInput(
                                        PlayerIndex.One,
                                        "Saving Beatmap",
                                        "Title of Beatmap",
                                        "",
                                        saveBeatmapAs, null);
                                    //this.LoadLateContent();
                                }
                                else
                                {
                                    //debugstring2 = "Created Beat at: " + (MediaPlayer.PlayPosition.Milliseconds - 1500);
                                    //debugstring2 = "Created Beat at: " + (gameTimeSinceCreating - 1500);
                                    //BeatTimerList.Add(new BeatTimerData(gameTimeSinceCreating - 1500, gesture.Position, new Vector2(0, 0), false, false));
                                    BeatTimerList.Add(new BeatTimerData(MediaPlayer.PlayPosition.Milliseconds - 1500, gesture.Position, new Vector2(0, 0), 0, false, false));
                                };
                                break;
                            case (GestureType.Hold):
                                gameTimeSinceHolding = MediaPlayer.PlayPosition.Milliseconds - 1000;
                                isHeld = true;
                                break;
                        }
                    }
                    break;
                case GameState.TestMenu:

                    //TouchCollection touchcol = TouchPanel.GetState();
                    //foreach (TouchLocation touchlocation in touchcol)
                    //{
                    //    if (touchlocation.State == TouchLocationState.Released)
                    //    {
                    //        int time = gameTimeSincePlaying - gameTimeSinceHolding;
                    //        isReleased = true;
                    //    }
                    //    else
                    //    {
                    //        if (isReleased)
                    //        {
                    //            isReleased = false;
                    //            gameTimeSinceHolding = gameTimeSincePlaying;
                    //        }
                    //    }

                    //}                   
                    break;
            }
            
            base.Update(gameTime);
        }

        

        protected override void Draw(GameTime gameTime)
        {
            
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();
            //spriteBatch.DrawString(debugFont, debugstring1, new Vector2(10, 10), Color.Black);
            //spriteBatch.DrawString(debugFont, debugstring2, new Vector2(10, 40), Color.Black);
            //spriteBatch.DrawString(debugFont, debugstring3, new Vector2(10, 70), Color.Black);
            switch (CurrentGameState)
            {
                case GameState.MainMenu:
                    //spriteBatch.DrawString(debugFont, gameTimeSinceStart.ToString(), new Vector2(10, 10), Color.Black);
                    //spriteBatch.DrawString(debugFont, gameTimeSincePlaying.ToString(), new Vector2(10, 40), Color.Black);
                    playBeatmapsButton.Draw(spriteBatch);
                    createBeatmapsButton.Draw(spriteBatch);
                    gameTimeSinceStart = (int) gameTime.TotalGameTime.TotalMilliseconds;
                    break;

                case GameState.Playing:
                    gameTimeSincePlaying = (int)gameTime.TotalGameTime.TotalMilliseconds - gameTimeSinceStart;

                    PointGenerator.Draw(spriteBatch, gameTimeSincePlaying);

                    ClickablePlayObject tempClickable;

                    foreach (int key in BeatDictionary.Keys)
                    {
                        if (key <= gameTimeSincePlaying)
                        {
                            BeatDictionary.TryGetValue(key, out tempClickable);
                            tempClickable.Draw(spriteBatch);
                        }
                        else
                        {
                            break;
                        }
                    }

                    break;

                case GameState.BeatmapCreator:
                    gameTimeSinceCreating = (int)gameTime.TotalGameTime.TotalMilliseconds - gameTimeSinceStart;
                    returnToMainMenuButton.Draw(spriteBatch);
                    spriteBatch.DrawString(debugFont, debugstring1, new Vector2(10, 10), Color.Black);
                    spriteBatch.DrawString(debugFont, debugstring2, new Vector2(10, 30), Color.Black);
                    spriteBatch.DrawString(debugFont, gameTimeSinceCreating.ToString(), new Vector2(10, 50), Color.Black);
                    break;

                case GameState.SaveMenu:
                    
                    break;

                case GameState.XMLLoadMenu:
                    loadMenu.Draw(spriteBatch);
                    break;

                case GameState.SongLoadMenu:
                    loadMenu.Draw(spriteBatch);
                    break;
                case GameState.TestMenu:
                    gameTimeSincePlaying = (int)gameTime.TotalGameTime.TotalMilliseconds - gameTimeSinceStart;
                    spriteBatch.DrawString(debugFont, debugstring1, new Vector2(10, 10), Color.Black);
                    spriteBatch.DrawString(debugFont, debugstring2, new Vector2(10, 40), Color.Black);
                    spriteBatch.DrawString(debugFont, debugstring3, new Vector2(10, 70), Color.Black);
                    spriteBatch.DrawString(debugFont, gameTimeSincePlaying.ToString(), new Vector2(10, 100), Color.Black);
                    testShaker.Draw(spriteBatch);
                    break;
            }


            spriteBatch.End();
            base.Draw(gameTime);
        }

        private void saveBeatmapAs(IAsyncResult r)
        {
            xmlConverter.saveBeatmap(BeatTimerList, Guide.EndShowKeyboardInput(r));
            CurrentGameState = GameState.MainMenu;
        }

        private void debugMessageXMLFiles()
        {
            using (var storage = IsolatedStorageFile.GetUserStoreForApplication())
            {
                String[] filenames = storage.GetFileNames("*.xml");
                foreach (String filename in filenames)
                {
                    Debug.WriteLine(filename);
                }
            }
        }

    }
}
