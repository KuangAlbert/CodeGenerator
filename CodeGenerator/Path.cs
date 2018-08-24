using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.IO;
using System.Xml.Xsl;

namespace CodeGenerator
{
    class Path
    {
        public string path_generator1 = "";
        public string path_generator_pin641 ="";
        public string path_generator_pin1001 ="";
        public string path_generator_interrupt641 ="";
        public string path_generator_interrupt1001 = "";

        public string path_generator_excel1 = "";
        public string path_generator_excel641 = "";
        public string path_generator_excel1001 = "";
        public string path_generator_interrupt1 = "";

        public void getPathGenertor(string location)
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.IgnoreComments = true;
            XmlReader reader = XmlReader.Create(location, settings);
            xmlDoc.Load(reader);
            XmlNode root = xmlDoc.SelectSingleNode("path");
            XmlNodeList xn = xmlDoc.SelectSingleNode("path").ChildNodes;
            foreach (XmlNode node in xn)
            {
                XmlElement xe = (XmlElement)node;
                if (node.Name.Equals("path_generator"))
                    path_generator1 = xe.GetAttribute("value");
                if (node.Name.Equals("path_generator_excel"))
                    path_generator_excel1 = xe.GetAttribute("value");
                if (node.Name.Equals("path_generator_excel64"))
                    path_generator_excel641 = xe.GetAttribute("value");
                if (node.Name.Equals("path_generator_excel100"))
                    path_generator_excel1001 = xe.GetAttribute("value");
                if (node.Name.Equals("path_generator_pin64"))
                    path_generator_pin641 = xe.GetAttribute("value");
                if (node.Name.Equals("path_generator_pin100"))
                    path_generator_pin1001 = xe.GetAttribute("value");
                if (node.Name.Equals("path_generator_interrupt64"))
                    path_generator_interrupt641 = xe.GetAttribute("value");
                if (node.Name.Equals("path_generator_interrupt1001"))
                    path_generator_interrupt1001 = xe.GetAttribute("value");
                if (node.Name.Equals("path_generator_interrupt1"))
                    path_generator_interrupt1 = xe.GetAttribute("value");
            }
            reader.Close();
            xmlDoc.Save(location);
        }
        public void change_node(string name, string value, string location)//修改XML文件
        {
            XmlDocument doc = new XmlDocument();
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.IgnoreComments = true;
            XmlReader reader = XmlReader.Create(location, settings);
            doc.Load(reader);
            XmlNodeList xn = doc.SelectSingleNode("path").ChildNodes;
            foreach (XmlNode node in xn)
            {
                XmlElement xe = (XmlElement)node;
                if (node.Name.Equals(name))
                    xe.SetAttribute("value", value);
            }
            reader.Close();
            doc.Save(location);
        }
    }
}
