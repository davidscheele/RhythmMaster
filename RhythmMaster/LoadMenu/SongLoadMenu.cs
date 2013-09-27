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

namespace RhythmMaster
{
    public class SongLoadMenu : LoadMenu
    {
       

        String[] songNames;
        String[] pageContents;
        SpriteFont font;
        NavigationButton mainMenuButton = new MainMenuButton(new Vector2(30, 400));
        NavigationButton backButton = new BackButton(new Vector2(160, 400));
        NavigationButton nextButton = new NextButton(new Vector2(290, 400));
        NavigationButton listBackwardButton = new ListBackwardButton(new Vector2(420, 400));
        NavigationButton listForwardButton = new ListForwardButton(new Vector2(550, 400));
        Texture2D loadSelectionButtonTexture;
        Dictionary<String, NavigationButton> loadSelectionButtonList;
        String selectedSong = "000000";
        NavigationButton selectedNavButton;
        int Page = 0;
        Boolean test = true;

        public SongLoadMenu(ContentManager contentManager)
        {
            font = contentManager.Load<SpriteFont>("LoadMenu/LoadMenuFont");
            loadSelectionButtonTexture = contentManager.Load<Texture2D>("LoadMenu/loadselectionbutton");
            mainMenuButton.Load(contentManager);
            nextButton.Load(contentManager);
            backButton.Load(contentManager);
            listForwardButton.Load(contentManager);
            listBackwardButton.Load(contentManager);
            
            using (var storage = IsolatedStorageFile.GetUserStoreForApplication())
            {
                songNames = storage.GetFileNames("*.mp3");
            }
            loadSelectionButtonList = new Dictionary<string,NavigationButton>();
            foreach (String name in songNames)
            {
                loadSelectionButtonList.Add(name, new LoadSelectionButton(loadSelectionButtonTexture));
            }
            turnPage(0);
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(font, "Loadmenu", new Vector2(30, 10), Color.Black);
         
            mainMenuButton.Draw(spriteBatch);
            if (selectedNavButton != null) nextButton.Draw(spriteBatch);
            backButton.Draw(spriteBatch);
            if (songNames.Length > (Page+1)*10)                   listForwardButton.Draw(spriteBatch);
            if (Page > 0)                               listBackwardButton.Draw(spriteBatch);

                    if (songNames.Length == 0)
                    {
                        spriteBatch.DrawString(font, "No Songs to Load.", new Vector2(100, 200), Color.Black);
                    }
                    else
                    {
                        int yOffset = 65;
                        int xOffset = 50;

                        foreach (String file in pageContents)
                        {
                            String[] splittedName = Regex.Split(file, ".mp3");

                            loadSelectionButtonList[file].Draw(spriteBatch, new Vector2(xOffset - 10, yOffset - 10));
                            spriteBatch.DrawString(font, splittedName[0], new Vector2(xOffset, yOffset), Color.Black);


                            yOffset += 60;
                            if (yOffset > 340)
                            {
                                yOffset = 65;
                                xOffset = 400;
                            }
                        }
                    
            }

            
        }
        public GameState checkClick(Vector2 tapLocation)
        {
            Rectangle tap = new Rectangle((int)tapLocation.X, (int)tapLocation.Y, 1, 1);
            if (tap.Intersects(mainMenuButton.Bounds)) return GameState.MainMenu;
            
            if (tap.Intersects(backButton.Bounds))
            {
                return GameState.XMLLoadMenu;
            }
                
            if (tap.Intersects(nextButton.Bounds))
            {
                PointGenerator.settestsong(selectedSong);
                return GameState.Playing;
            }
                    
            if (tap.Intersects(listBackwardButton.Bounds))
            {
                turnPage(-1);
            }
                        
            if (tap.Intersects(listForwardButton.Bounds))
            {
                turnPage(1);
            }
            foreach (KeyValuePair<String, NavigationButton> kvp in loadSelectionButtonList)
            {
                if (tap.Intersects(kvp.Value.Bounds))
                {
                    if (selectedNavButton != null) selectedNavButton.Color = Color.Aqua;
                    kvp.Value.Color = Color.DarkBlue;
                    selectedNavButton = kvp.Value;
                    selectedSong = kvp.Key;
                    break;
                }
            }
            return GameState.XMLLoadMenu;
        }
        private void turnPage(int modifier)
        {
            Page += modifier;
            int loadtemp = 10;
            if (songNames.Length < (Page + 1) * 10) loadtemp = 10 - ((Page + 1) * 10 - songNames.Length);
            pageContents = new String[loadtemp];
            for (int i = 0; i < loadtemp; i++)
            {
               pageContents[i] = songNames[i+Page*10];
            }
        }
    }
}
