using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using APIAccessor;

namespace TVMonitorFS
{
    public class DBFileReader
    {
        private string FileName;
        public DBFileReader(string fileName)
        {
            FileName = fileName;
        }

        public void Write(List<TVMetaData> metas)
        {
            if (metas == null) { return; }

            try
            {
                XmlDocument xmlDocument = new XmlDocument();
                XmlSerializer serializer = new XmlSerializer(metas.GetType());
                using (MemoryStream stream = new MemoryStream())
                {
                    serializer.Serialize(stream, metas);
                    stream.Position = 0;
                    xmlDocument.Load(stream);
                    xmlDocument.Save(FileName);
                }
            }
            catch (Exception ex)
            {
                //Log exception here
            }
        }

        public List<TVMetaData> Read()
        {
            if (string.IsNullOrEmpty(FileName)) { return default(List<TVMetaData>); }

            List<TVMetaData> list = new List<TVMetaData>();

            try
            {
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.Load(FileName);
                string xmlString = xmlDocument.OuterXml;

                using (StringReader read = new StringReader(xmlString))
                {
                    Type outType = typeof(List<TVMetaData>);

                    XmlSerializer serializer = new XmlSerializer(outType);
                    using (XmlReader reader = new XmlTextReader(read))
                    {
                        list = (List<TVMetaData>)serializer.Deserialize(reader);
                    }
                }
            }
            catch (Exception ex)
            {
                //Log exception here
            }

            return list;
        }
    }
}
