using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Media;

namespace RhythmMaster.Functions
{
    public static class DataSaver
    {
        static String xmlName;
        public static void SelectedXml(String xml)
        { xmlName = xml; }
        public static String SelectedXml()
        { return xmlName; }

        static Song songName;
        public static void SelectedSong(Song song)
        { songName = song; }
        public static Song SelectedSong()
        { return songName; }

    }
}
