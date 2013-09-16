using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Diagnostics;
using System.Text;


    public class XmlConverter
    {
        XmlConverter(String filename)
        {
            var xDoc = new XDocument(
        new XDeclaration("1.0", "UTF-8", null),
        new XElement("Adressen",
            new XElement("Adresse",
                new XElement("Name", "Paul Faul"),
                new XElement("Telefon", "010101")
                )
            )
            );
            var yDoc = new XDocument();
            using (var storage = IsolatedStorageFile.GetUserStoreForApplication())
            {
                using (Stream stream = storage.CreateFile("test.xml"))
                {
                    xDoc.Save(stream);
                }
            }

            using (var storage = IsolatedStorageFile.GetUserStoreForApplication())
            {
                using (Stream stream = storage.OpenFile("test.xml", FileMode.Open))
                {
                    yDoc = XDocument.Load(stream);

                    Debug.WriteLine(yDoc.FirstNode.ToString());

                }
            }
        }
    }

