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
                child.Add(new XElement("shakerlength", btd.ShakerLength));
                child.Add(new XElement("isslider", btd.IsSlider));
                child.Add(new XElement("isshaker", btd.IsShaker));

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
                    IEnumerable<XElement> beatdata = yDoc.Descendants("beat");
                    

                    foreach (var beatpart in beatdata)
                    {
                       tempBeatTimerDataList.Add(
                           new BeatTimerData(
                               int.Parse(beatpart.Element("timestamp").Value), 
                               new Microsoft.Xna.Framework.Vector2(float.Parse(beatpart.Element("xstart").Value), float.Parse(beatpart.Element("ystart").Value)), 
                               new Microsoft.Xna.Framework.Vector2(float.Parse(beatpart.Element("xend").Value), float.Parse(beatpart.Element("yend").Value)), 
                               int.Parse(beatpart.Element("shakerlength").Value),
                               Boolean.Parse(beatpart.Element("isslider").Value),
                               Boolean.Parse(beatpart.Element("isslider").Value)
                           )
                       );
                    }

                }
            }
            return tempBeatTimerDataList;
        }

        
    }

