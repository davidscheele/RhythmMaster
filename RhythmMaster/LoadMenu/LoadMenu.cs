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
    public class LoadMenu
    {
        

        enum LoadState
        {
            LoadBeatmap,
            LoadSong,
        }

        String[] xmlNames;
        String[] pageContents;
        SpriteFont font;
        LoadState LoadMenuState;
        NavigationButton mainMenuButton = new MainMenuButton(new Vector2(30, 400));
        NavigationButton nextButton = new NextButton(new Vector2(160, 400));
        NavigationButton backButton = new BackButton(new Vector2(290, 400));
        NavigationButton listForwardButton = new ListForwardButton(new Vector2(420, 400));
        NavigationButton listBackwardButton = new ListBackwardButton(new Vector2(550, 400));
        Texture2D loadSelectionButtonTexture;
        Dictionary<String, NavigationButton> loadSelectionButtonList;
        int Page = 0;

        public LoadMenu(ContentManager contentManager)
        {
            font = contentManager.Load<SpriteFont>("LoadMenu/LoadMenuFont");
            loadSelectionButtonTexture = contentManager.Load<Texture2D>("LoadMenu/loadselectionbutton");
            mainMenuButton.Load(contentManager);
            nextButton.Load(contentManager);
            backButton.Load(contentManager);
            listForwardButton.Load(contentManager);
            listBackwardButton.Load(contentManager);
            LoadMenuState = LoadState.LoadBeatmap;

            using (var storage = IsolatedStorageFile.GetUserStoreForApplication())
            {
                xmlNames = storage.GetFileNames("*.xml");
            }
            loadSelectionButtonList = new Dictionary<string,NavigationButton>();
            foreach (String name in xmlNames)
            {
                loadSelectionButtonList.Add(name, new LoadSelectionButton(loadSelectionButtonTexture));
            }
            
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(font, "Loadmenu", new Vector2(30, 10), Color.Black);
            mainMenuButton.Draw(spriteBatch);
            nextButton.Draw(spriteBatch);
            if (LoadMenuState == LoadState.LoadSong)    backButton.Draw(spriteBatch);
            if (xmlNames.Length > (Page+1)*10)                   listForwardButton.Draw(spriteBatch);
            if (Page > 0)                               listBackwardButton.Draw(spriteBatch);
            if (xmlNames.Length == 0)
            {
                spriteBatch.DrawString(font, "No Beatmaps to Load.", new Vector2(100, 200), Color.Black);
                spriteBatch.DrawString(font, "Create some with the Beatmap Creator!", new Vector2(100, 230), Color.Black);
            }
            else
            {
                int yOffset = 65;
                int xOffset = 50;
                
                foreach (String file in xmlNames)
                {
                    String[] splittedName = Regex.Split(file, ".xml");
                    
                    loadSelectionButtonList[file].Draw(spriteBatch, new Vector2(xOffset-10, yOffset-10));
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
            if (tap.Intersects(mainMenuButton.Bounds))
            {
                return GameState.MainMenu;
            }
            else
            {
                if (tap.Intersects(backButton.Bounds))
                {
                    
                }
                else
                {
                    if (tap.Intersects(nextButton.Bounds))
                    {

                    }
                    else
                    {
                        if (tap.Intersects(listBackwardButton.Bounds))
                        {

                        }
                        else
                        {
                            if (tap.Intersects(listForwardButton.Bounds))
                            {

                            }
                        }
                    }
                }
            }    
        }
        private void turnPage(int modifier)
        {
            Page += modifier;
            int loadtemp = 10;
            if (xmlNames.Length < (Page + 1) * 10) loadtemp = (Page + 1) * 10 - xmlNames.Length;
            pageContents = new String[loadtemp];
            for (int i = 0; i < loadtemp; i++)
            {
               pageContents[i] = xmlNames[i+Page*10];
            }
        }
    }
}