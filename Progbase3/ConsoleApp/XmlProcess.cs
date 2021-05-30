using System.IO;
using System.IO.Compression;
using System.Xml.Serialization;

public class XmlProcess
{
    public void XmlExport(Good[] goods, string filepath)
    {
        if (filepath.EndsWith(".xml"))
        {
            XmlSerializer ser = new XmlSerializer(typeof(Good[]));
            System.IO.StreamWriter writer = new System.IO.StreamWriter(filepath);
            ser.Serialize(writer, goods);
            writer.Close();
        }
    }

    public void Zipping(string filepath, string zippath)
    {
        ZipFile.CreateFromDirectory(filepath, zippath);
    }

    public void XmlImport(string filepath, GoodRepository goodRepository)
    {
        if (File.Exists(filepath) && filepath.EndsWith(".xml"))
        {
            XmlSerializer ser = new XmlSerializer(typeof(Good[]));
            StreamReader reader = new StreamReader(filepath);
            Good[] goods = (Good[])ser.Deserialize(reader);
            reader.Close();

            for (int i = 0; i < goods.Length; i++)
            {
                if (!goodRepository.GoodExists(goods[i].name))
                {
                    goodRepository.Insert(goods[i]);
                }
            }

        }
    }
}
