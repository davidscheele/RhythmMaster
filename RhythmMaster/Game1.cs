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

        Song munchymonk;

        Boolean beatAppeared = false;
        Boolean notYetTouched = true;

        int startTime = 1;
        int gameTimeSinceStart = 0;
        int gameTimeSincePlaying = 0;
        int gameTimeSinceCreating = 0;

        int timeBeatAppeared = 0;

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

            debugMessageXMLFiles();
            
            // Frame rate is 30 fps by default for Windows Phone.
            TargetElapsedTime = TimeSpan.FromTicks(333333);

            // Extend battery life under lock.
            InactiveSleepTime = TimeSpan.FromSeconds(1);
        }

        public void Save(string filename)
        {

        }  

        protected override void Initialize()
        {
            base.Initialize();
        }

        private void LoadLateContent()
        {
            BeatDictionary = new Dictionary<int, Clickable>();
            foreach (BeatTimerData btd in BeatTimerList)
            {
                BeatDictionary.Add(btd.Timestamp, new Beat(btd.StartPosition));

            }

            foreach (KeyValuePair<int, Clickable> kvp in BeatDictionary)
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

            munchymonk = Content.Load<Song>("munchymonk");

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
                                    this.CurrentGameState = GameState.BeatmapCreator;
                                    BeatTimerList = new List<BeatTimerData>();
                                    //MediaPlayer.Play(munchymonk);
                                };
                                if(playBeatmapsButton.checkClick(gesture))
                                {
                                    this.CurrentGameState = GameState.LoadMenu;
                                    loadMenu = new LoadMenu(this.Content);
                                    //MediaPlayer.Play(munchymonk);
                                };
                                break;
                        }
                    }
                    break;
                case GameState.LoadMenu:
                    while (TouchPanel.IsGestureAvailable)
                    {
                        GestureSample gesture = TouchPanel.ReadGesture();

                        switch (gesture.GestureType)
                        {
                            case (GestureType.Tap):
                                CurrentGameState = loadMenu.checkClick(gesture.Position);
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
                    while (TouchPanel.IsGestureAvailable)
                    {
                        GestureSample gesture = TouchPanel.ReadGesture();

                        switch (gesture.GestureType)
                        {
                            case (GestureType.Tap):
                                if (returnToMainMenuButton.checkClick(gesture))
                                {
                                    //MediaPlayer.Stop();
                                    //this.BeatTimerList = xmlConverter.loadBeatmapXML("test2");
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
                                    BeatTimerList.Add(new BeatTimerData(gameTimeSinceCreating - 1500, gesture.Position, new Vector2(0, 0), false, false));
                                };
                             
                                break;
                        }
                    }
                    break;
            }
            
            base.Update(gameTime);
        }

        private void checkIntersect(Vector2 tapPosition)
        {
            Clickable testBeat;
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

        protected override void Draw(GameTime gameTime)
        {
            
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();
            switch (CurrentGameState)
            {
                case GameState.MainMenu:
                    spriteBatch.DrawString(debugFont, gameTimeSinceStart.ToString(), new Vector2(10, 10), Color.Black);
                    spriteBatch.DrawString(debugFont, gameTimeSincePlaying.ToString(), new Vector2(10, 40), Color.Black);
                    playBeatmapsButton.Draw(spriteBatch);
                    createBeatmapsButton.Draw(spriteBatch);
                    gameTimeSinceStart = (int) gameTime.TotalGameTime.TotalMilliseconds;
                    break;

                case GameState.Playing:
                    //if (beatAppeared && notYetTouched) { timeSinceBeatAppeared = (int)gameTime.TotalGameTime.TotalMilliseconds - timeBeatAppeared; }
                    gameTimeSincePlaying = (int)gameTime.TotalGameTime.TotalMilliseconds - gameTimeSinceStart;
                    //spriteBatch.DrawString(debugFont, gameTimeSinceStart.ToString(), new Vector2(10, 10), Color.Black);
                    //spriteBatch.DrawString(debugFont, gameTimeSincePlaying.ToString(), new Vector2(10, 20), Color.Black);
                    //spriteBatch.DrawString(debugFont, timeSinceBeatAppeared.ToString(), new Vector2(10, 40), Color.Black);
                    //spriteBatch.DrawString(debugFont, debug04String, new Vector2(10, 50), Color.Black);

                    PointGenerator.Draw(spriteBatch, gameTimeSincePlaying);

                    Clickable tempClickable;

                    foreach (int key in BeatDictionary.Keys)
                    {
                        if (key <= gameTimeSincePlaying)
                        {
                            BeatDictionary.TryGetValue(key, out tempClickable);
                            tempClickable.Draw(spriteBatch);
                            if (!beatAppeared) { beatAppeared = true; timeBeatAppeared = gameTimeSincePlaying + gameTimeSinceStart;}
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
                    break;

                case GameState.SaveMenu:
                    
                    break;

                case GameState.LoadMenu:
                    loadMenu.Draw(spriteBatch);
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
