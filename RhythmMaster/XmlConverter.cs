using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Diagnostics;
using System.Text;
using Microsoft.Xna.Framework;


    public class XmlConverter
    {
        public XmlConverter()
        {
        }

        public void saveBeatmap(List<BeatTimerData> beatTimerList, String filename)
        {
            XDocument xDoc = new XDocument(
                new XDeclaration("1.0", "UTF-8", null), 
                    new XElement("Root")
            );
            foreach (BeatTimerData btd in beatTimerList)
            {
                XElement child = new XElement("beat");

                child.Add(new XElement("timestamp", btd.Timestamp));
                child.Add(new XElement("xstart", btd.StartPosition.X));
                child.Add(new XElement("ystart", btd.StartPosition.Y));
                child.Add(new XElement("xend", btd.EndPosition.X));
                child.Add(new XElement("yend", btd.EndPosition.Y));
                child.Add(new XElement("isslider", btd.IsSlider));
                child.Add(new XElement("isspinner", btd.IsSpinner));

                xDoc.Root.Add(child);
            }
            using (var storage = IsolatedStorageFile.GetUserStoreForApplication())
            {
                using (Stream stream = storage.CreateFile(filename + ".xml"))
                {
                    xDoc.Save(stream);
                }
            }
        }

        public List<BeatTimerData> loadBeatmapXML(String filename)
        {
            List<BeatTimerData> tempBeatTimerDataList = new List<BeatTimerData>();
            using (var storage = IsolatedStorageFile.GetUserStoreForApplication())
            {
                using (Stream stream = storage.OpenFile(filename + ".xml", FileMode.Open))
                {
                    XDocument yDoc = XDocument.Load(stream);
                    IEnumerable<XElement> beats = yDoc.Descendants("beat");
                    

                    foreach (var beat in beats)
                    {
                       tempBeatTimerDataList.Add(
                           new BeatTimerData(
                               int.Parse(beat.Element("timestamp").Value), 
                               new Microsoft.Xna.Framework.Vector2(float.Parse(beat.Element("xstart").Value), float.Parse(beat.Element("ystart").Value)), 
                               new Microsoft.Xna.Framework.Vector2(float.Parse(beat.Element("xend").Value), float.Parse(beat.Element("yend").Value)), 
                               Boolean.Parse(beat.Element("isslider").Value),
                               Boolean.Parse(beat.Element("isslider").Value)
                           )
                       );
                    }

                }
            }
            return tempBeatTimerDataList;
        }

        
    }

