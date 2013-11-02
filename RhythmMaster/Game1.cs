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
        SpriteFont scoreFont;
        LoadMenu loadMenu;
        GameState menuIdentifier;

        Shaker testShaker = new Shaker(1200);

        int debugint = 0;
        int debugshakercount = 0;
        int debugbeatcount = 0;
        String debugstring1 = "nothin";
        String debugstring2 = "nothin";
        String debugstring3 = "nothin";

        Song munchymonk;

        Boolean isHeld = false;

        int gameTimeSinceStart = 0;
        int gameTimeSincePlaying = 0;
        int gameTimeSinceCreating = 0;
        int gameTimeSinceHolding = 0;
        int mediaplayerTime = 0;
        long endTime = 999999999999;

        Dictionary<int, PlayObject> BeatDictionary = new Dictionary<int,PlayObject>();

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
            if (CurrentGameState == GameState.Playing)
            {
                foreach (var kv in BeatDictionary.ToDictionary(kv => kv.Key, kv => kv.Value))
                {
                    if (kv.Key <= MediaPlayer.PlayPosition.TotalMilliseconds)
                    {
                        if (kv.Value.Identifier() == PlayObjectIdentifier.Shaker)
                        {
                            Shaker tempShaker = kv.Value as Shaker;
                            tempShaker.completeSomeMore();
                        }
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }


        public void Save(string filename)
        {

        }

        private void checkIntersect(Vector2 tapPosition)
        {

            foreach (var kv in BeatDictionary.ToDictionary(kv => kv.Key, kv => kv.Value))
            {
                if (kv.Key <= MediaPlayer.PlayPosition.TotalMilliseconds)
                {
                    ClickablePlayObject tempObject = kv.Value as ClickablePlayObject;

                    if (tempObject != null)
                    {
                        if (tempObject.Bounds.Intersects(new Rectangle((int)tapPosition.X, (int)tapPosition.Y, 1, 1)))
                        {
                            PointGenerator.generatePointEffect(tempObject.Center, tempObject.BeatRing.Scale, (int) MediaPlayer.PlayPosition.TotalMilliseconds);
                            BeatDictionary.Remove(kv.Key);
                            break;
                        }
                    }
                }
                else
                {
                    break;
                }
            }
            checkForEndOfSong();
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        private void LoadLateContent()
        {
            debugshakercount = 0;
            debugbeatcount = 0;
            BeatDictionary = new Dictionary<int, PlayObject>();
            foreach (BeatTimerData btd in BeatTimerList)
            {
                if (btd.IsShaker)
                {
                    BeatDictionary.Add(btd.Timestamp, new Shaker(btd.ShakerLength));
                    debugshakercount++;
                    debugstring3 = "shaker from " + btd.Timestamp + " for " + btd.ShakerLength;
                }
                else
                {
                    if (btd.IsSlider)
                    {
                    }
                    else
                    {
                        BeatDictionary.Add(btd.Timestamp, new Beat(btd.StartPosition));
                        debugbeatcount++;
                    }
                }
                

            }

            debugstring1 = "Shaker Created: " + debugshakercount;
            debugstring2 = "Beats Created: " + debugbeatcount;

            foreach (KeyValuePair<int, PlayObject> kvp in BeatDictionary)
            {
                kvp.Value.LoadContent(this.Content);
            }
        }
        private void countShakersAndBeats()
        {
            int shakerCount = 0;
            int beatCount = 0;
            int unknownCount = 0;
            foreach (KeyValuePair<int, PlayObject> kvp in BeatDictionary)
            {
                if (kvp.Value.Identifier() == PlayObjectIdentifier.Shaker)
                {
                    shakerCount++;
                }
                else
                {
                    if (kvp.Value.Identifier() == PlayObjectIdentifier.Beat)
                    {
                        beatCount++;
                    }
                    else
                    {
                        unknownCount++;
                    }
                }
            }
            debugstring1 = "Beats: " + beatCount;
            debugstring2 = "Shaker: " + shakerCount;
            debugstring3 = "Unknown: " + unknownCount;
        }

        private void checkForEndOfSong()
        {
            if (BeatDictionary.Count == 0)
            {
                endTime = (long)MediaPlayer.PlayPosition.TotalMilliseconds + 5000;
            }
        }

        protected override void LoadContent()
        {
            
            spriteBatch = new SpriteBatch(GraphicsDevice);
            debugFont = Content.Load<SpriteFont>("Debugfont");
            scoreFont = Content.Load<SpriteFont>("ScoreFont");
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

            foreach (KeyValuePair<int, PlayObject> kvp in BeatDictionary)
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
            switch (CurrentGameState){
                case GameState.MainMenu:                                            //Check for Clicks in Main Menu
                    if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                        this.Exit();
                    while (TouchPanel.IsGestureAvailable)
                    {
                        GestureSample gesture = TouchPanel.ReadGesture();

                        switch (gesture.GestureType)
                        {
                            case (GestureType.Tap):
                                if(createBeatmapsButton.checkClick(gesture))
                                {
                                    this.CurrentGameState = GameState.SongLoadMenu; // actual
                                    this.menuIdentifier = GameState.BeatmapCreator;
                                    //this.CurrentGameState = GameState.TestMenu; //testing purposes
                                    loadMenu = new SongLoadMenu(this.Content);
                                    BeatTimerList = new List<BeatTimerData>(); //Actual
                                    //MediaPlayer.Play(munchymonk); //debug
                                };
                                if(playBeatmapsButton.checkClick(gesture))
                                {
                                    this.CurrentGameState = GameState.SongLoadMenu;
                                    this.menuIdentifier = GameState.Playing;
                                    loadMenu = new SongLoadMenu(this.Content);
                                };
                                break;
                        }
                    }
                    break;

                case GameState.SongLoadMenu:
                    if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                    {
                        CurrentGameState = GameState.XMLLoadMenu;
                        loadMenu = new XMLLoadMenu(this.Content);
                        break;
                    }
                    while (TouchPanel.IsGestureAvailable)
                    {
                        GestureSample gesture = TouchPanel.ReadGesture();

                        switch (gesture.GestureType)
                        {
                            case (GestureType.Tap):
                                switch (loadMenu.checkClick(gesture.Position))
                                {

                                    //case GameState.Playing:
                                    //    this.BeatTimerList = xmlConverter.loadBeatmapXML(DataSaver.SelectedXml());
                                    //    this.LoadLateContent();
                                    //    //MediaPlayer.Play(munchymonk);

                                    //    CurrentGameState = GameState.Playing;
                                    //    break;

                                    case GameState.XMLLoadMenu:
                                        if (menuIdentifier == GameState.Playing)
                                        {
                                            this.CurrentGameState = GameState.XMLLoadMenu;
                                            loadMenu = new XMLLoadMenu(this.Content);
                                        }
                                        else
                                        {

                                            this.CurrentGameState = GameState.BeatmapCreator;
                                            MediaPlayer.Play(DataSaver.SelectedSong());
                                        }
                                        break;

                                    case GameState.MainMenu:
                                        this.CurrentGameState = GameState.MainMenu;
                                        break;
                                }

                                break;
                        }
                    }
                    break;

                case GameState.XMLLoadMenu:
                    if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                    { 
                        CurrentGameState = GameState.MainMenu;
                        break;
                    }
                    while (TouchPanel.IsGestureAvailable)
                    {
                        GestureSample gesture = TouchPanel.ReadGesture();

                        switch (gesture.GestureType)
                        {
                            case (GestureType.Tap):
                                switch (loadMenu.checkClick(gesture.Position))
                                {
                                    case GameState.Playing: //debug

                                        this.BeatTimerList = xmlConverter.loadBeatmapXML(DataSaver.SelectedXml());
                                        this.LoadLateContent();
                                        MediaPlayer.Play(DataSaver.SelectedSong());
                                        PointGenerator.ResetPoints();
                                        CurrentGameState = GameState.Playing;
                                        break;

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

                
                case GameState.Playing:                                             //Check for Beat klicks
                    if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                    {
                        CurrentGameState = GameState.MainMenu;
                        MediaPlayer.Stop();
                        break;
                    }
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
                    if ((long)MediaPlayer.PlayPosition.TotalMilliseconds >= endTime)
                    {
                        MediaPlayer.Stop();
                        CurrentGameState = GameState.ScoreMenu;
                    }
                    
                    break;

                case GameState.BeatmapCreator:                                      //Check for Taps to create Beatmap
                    if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                    {
                        CurrentGameState = GameState.MainMenu;
                        MediaPlayer.Stop();
                        break;
                    }
                    TouchCollection touchcol = TouchPanel.GetState();
                    foreach (TouchLocation touchlocation in touchcol)
                    {
                        if (touchlocation.State == TouchLocationState.Released && isHeld)
                        {
                            int heldTime = (int)MediaPlayer.PlayPosition.TotalMilliseconds - gameTimeSinceHolding;

                            debugstring1 = "Held for: " + heldTime;
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
                            
                                    BeatTimerList.Add(new BeatTimerData((int) MediaPlayer.PlayPosition.TotalMilliseconds - 1100, gesture.Position, new Vector2(0, 0), 0, false, false));
                                };
                                break;
                            case (GestureType.Hold):
                                
                                gameTimeSinceHolding = (int) MediaPlayer.PlayPosition.TotalMilliseconds - 1000;
                                debugstring2 = "held since: " + gameTimeSinceHolding + " since holding";
                                isHeld = true;
                                break;
                        }
                    }
                    break;

                case GameState.ScoreMenu:
                    while (TouchPanel.IsGestureAvailable)
                    {
                        GestureSample gesture = TouchPanel.ReadGesture();

                        switch (gesture.GestureType)
                        {

                            case (GestureType.Tap):
                                if (returnToMainMenuButton.checkClick(gesture))
                                {
                                    this.CurrentGameState = GameState.MainMenu;
                                }
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
                    //gameTimeSincePlaying = (int)gameTime.TotalGameTime.TotalMilliseconds - gameTimeSinceStart;

                    spriteBatch.DrawString(debugFont, PointGenerator.TotalPoints.ToString(), new Vector2(10, 10), Color.Black);
                    spriteBatch.DrawString(debugFont, PointGenerator.Multiplicator, new Vector2(10, 30), Color.Black);
                    //this.countShakersAndBeats();
                    //spriteBatch.DrawString(debugFont, debugstring1, new Vector2(10, 50), Color.Black);
                    //spriteBatch.DrawString(debugFont, debugstring2, new Vector2(10, 70), Color.Black);
                    //spriteBatch.DrawString(debugFont, endTime.ToString(), new Vector2(10, 90), Color.Black);
                    

                    PointGenerator.Draw(spriteBatch, (int) MediaPlayer.PlayPosition.TotalMilliseconds);

                    foreach (var kv in BeatDictionary.ToDictionary(kv => kv.Key, kv => kv.Value))
                    {
                        if (kv.Key <= MediaPlayer.PlayPosition.TotalMilliseconds)
                        {
                            if (kv.Value.Identifier() == PlayObjectIdentifier.Shaker)
                            {
                                Shaker tempShaker = kv.Value as Shaker;
                                if ((tempShaker.Length + kv.Key) <= (int)MediaPlayer.PlayPosition.TotalMilliseconds)
                                {
                                    BeatDictionary.Remove(kv.Key);
                                    if (tempShaker.isComplete())
                                    {
                                        PointGenerator.generatePointEffect(new Vector2(400f, 240f), PointEffectState.FullPoints, (int)MediaPlayer.PlayPosition.TotalMilliseconds);
                                    }
                                    else
                                    {
                                        PointGenerator.generatePointEffect(new Vector2(400f, 240f), PointEffectState.ReducedPoints, (int)MediaPlayer.PlayPosition.TotalMilliseconds);
                                    }
                                    checkForEndOfSong();
                                }

                            }
                            else
                            {
                                ClickablePlayObject tempBeat = kv.Value as ClickablePlayObject;
                                if (!tempBeat.thisDraw)
                                {
                                    BeatDictionary.Remove(kv.Key);
                                    PointGenerator.generatePointEffect(tempBeat.Center, 2f, (int)MediaPlayer.PlayPosition.TotalMilliseconds);
                                }
                            }
                            kv.Value.Draw(spriteBatch);
                            
                        }
                        else
                        {
                            break;
                        }

                    }
                    spriteBatch.DrawString(debugFont, MediaPlayer.PlayPosition.TotalMilliseconds.ToString(), new Vector2(10, 430), Color.Yellow);
                   
                    break;

                case GameState.BeatmapCreator:
                    gameTimeSinceCreating = (int)gameTime.TotalGameTime.TotalMilliseconds - gameTimeSinceStart;
                    returnToMainMenuButton.Draw(spriteBatch);
                    //spriteBatch.DrawString(debugFont, MediaPlayer.PlayPosition.TotalMilliseconds.ToString(), new Vector2(10, 10), Color.Black);
                    //spriteBatch.DrawString(debugFont, debugstring1, new Vector2(10, 30), Color.Black);
                    //spriteBatch.DrawString(debugFont, debugstring2, new Vector2(10, 50), Color.Black);
                    //spriteBatch.DrawString(debugFont, debugstring3, new Vector2(10, 70), Color.Black);
                    break;
                    

                case GameState.SaveMenu:
                    
                    break;

                case GameState.XMLLoadMenu:
                    loadMenu.Draw(spriteBatch);
                    break;

                case GameState.SongLoadMenu:
                    loadMenu.Draw(spriteBatch);
                    break;
                case GameState.ScoreMenu:
                    returnToMainMenuButton.Draw(spriteBatch);
                    spriteBatch.DrawString(scoreFont, "Final Score:", new Vector2(150, 200), Color.Red);
                    spriteBatch.DrawString(scoreFont, PointGenerator.TotalPoints.ToString(), new Vector2(150, 300), Color.Red);
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
