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
using CodeGenerator;
namespace irq_set
{
    class IRQ
    {
     
        public int[] confirm_flag = new int[11] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        public Boolean firstAdd = true;//默认group_number第一次加入
       
        public void Creat_irq_Xml(ref string location)//XML mode for pin64 mcu's GPIO configuration.
        {
            XmlDocument xmldoc = new XmlDocument();

            XmlDeclaration xmldecl = xmldoc.CreateXmlDeclaration("1.0", "ISO-8859-1", null);
            xmldoc.AppendChild(xmldecl);

            XmlElement xmlelem = xmldoc.CreateElement("IRQ_Set");
            xmldoc.AppendChild(xmlelem);

            XmlNode root = xmldoc.SelectSingleNode("IRQ_Set");


            //**************************************************************************************//
            XmlElement nxel = xmldoc.CreateElement("FUN");
            nxel.SetAttribute("Register", "OS");


            //**************************************************************************************//
            XmlElement axel = xmldoc.CreateElement("FUN");
            axel.SetAttribute("Register", "EX");




            ////**************************************************************************************//
            XmlElement bxel = xmldoc.CreateElement("FUN");
            bxel.SetAttribute("Register", "TAU");



            ////**************************************************************************************//
            XmlElement cxel = xmldoc.CreateElement("FUN");
            cxel.SetAttribute("Register", "ADC");




            ////**************************************************************************************//
            XmlElement dxel = xmldoc.CreateElement("FUN");
            dxel.SetAttribute("Register", "CAN");




            ////**************************************************************************************//
            XmlElement exel = xmldoc.CreateElement("FUN");
            exel.SetAttribute("Register", "UART");



            ////**************************************************************************************//
            XmlElement fxel = xmldoc.CreateElement("FUN");
            fxel.SetAttribute("Register", "IIC");


            ////**************************************************************************************//
            XmlElement gxel = xmldoc.CreateElement("FUN");
            gxel.SetAttribute("Register", "SPI");

            //**************************************************************************************//


            ////**************************************************************************************//
            XmlElement hxel = xmldoc.CreateElement("FUN");
            hxel.SetAttribute("Register", "PWM");

            //**************************************************************************************//


            ////**************************************************************************************//
            XmlElement ixel = xmldoc.CreateElement("FUN");
            ixel.SetAttribute("Register", "WD");

            ////**************************************************************************************//
            XmlElement jxel = xmldoc.CreateElement("FUN");
            jxel.SetAttribute("Register", "DMA");


            //**************************************************************************************//
            XmlElement kxel = xmldoc.CreateElement("FUN");
            kxel.SetAttribute("Register", "Var");
            XmlElement kxel1 = xmldoc.CreateElement("Var_value");
            kxel1.InnerText = "0";
            kxel.AppendChild(kxel1);
            //**************************************************************************************//
            XmlElement lxel = xmldoc.CreateElement("FUN");
            lxel.SetAttribute("Register", "Count");
            //**************************************************************************************//

            XmlElement mxel= xmldoc.CreateElement("INF");


            root.AppendChild(nxel);
            root.AppendChild(axel);
            root.AppendChild(bxel);
            root.AppendChild(cxel);
            root.AppendChild(dxel);
            root.AppendChild(exel);
            root.AppendChild(fxel);
            root.AppendChild(gxel);
            root.AppendChild(hxel);
            root.AppendChild(ixel);
            root.AppendChild(jxel);
            root.AppendChild(kxel);
            root.AppendChild(lxel);
            root.AppendChild(mxel);
            xmldoc.Save(location);
        }
        public void read_xml_info(ref string location, ref string version, ref string date, ref string name, ref string comment)//name 寄存器)
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.IgnoreComments = true;
            XmlReader reader = XmlReader.Create(location, settings);
            xmlDoc.Load(reader);
            XmlNode root = xmlDoc.SelectSingleNode("IRQ_Set");
            XmlNodeList xn = xmlDoc.SelectSingleNode("IRQ_Set").ChildNodes;
            foreach (XmlNode node in xn)
            {
                XmlElement xe = (XmlElement)node;//NODE FUN
                if (node.Name == "INF")
                {
                    foreach (XmlNode node1 in xe)
                    {
                        XmlElement xe1 = (XmlElement)node1;//NODE FUN
                        if (node1.Name == "info_Group")
                        {
                            version = xe1.GetAttribute("Version");
                            date = xe1.GetAttribute("Date");
                            name = xe1.GetAttribute("Name");
                            comment = xe1.GetAttribute("Comment");
                        }
                    }
                }
            }
            reader.Close();
            xmlDoc.Save(location);
        }
        public void change_xml_info(ref string location, ref string version, ref string date, ref string name, ref string comment)//name 寄存器)
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.IgnoreComments = true;
            XmlReader reader = XmlReader.Create(location, settings);
            xmlDoc.Load(reader);
            XmlNode root = xmlDoc.SelectSingleNode("IRQ_Set");
            XmlNodeList xn = xmlDoc.SelectSingleNode("IRQ_Set").ChildNodes;
            foreach (XmlNode node in xn)
            {
                XmlElement xe = (XmlElement)node;//NODE FUN
                if (node.Name == "INF")
                {
                    XmlElement xe1 = xmlDoc.CreateElement("info_Group");
                    xe1.SetAttribute("Version", version);
                    xe1.SetAttribute("Date", date);
                    xe1.SetAttribute("Name", name);
                    xe1.SetAttribute("Comment", comment);
                    xe.AppendChild(xe1);
                }
            }
            reader.Close();
            xmlDoc.Save(location);
        }
        public void IRQ_change_node(int var, int count, ref string location)//name 寄存器
        {
            XmlDocument doc = new XmlDocument();
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.IgnoreComments = true;
            XmlReader reader = XmlReader.Create(location, settings);
            doc.Load(reader);
            XmlNodeList xn = doc.SelectSingleNode("IRQ_Set").ChildNodes;
            foreach (XmlNode node in xn)
            {

                XmlElement xe = (XmlElement)node;
                if (xe.GetAttribute("Register") == "Var")
                {
                    XmlNodeList sxe = xe.ChildNodes;
                    foreach (XmlNode node1 in sxe)//foreach group in FUN
                    {
                        XmlElement xe1 = (XmlElement)node1;
                        node1.InnerText = Convert.ToString(var);

                    }
                }
                if ((xe.GetAttribute("Register") == "Count"))
                {
                    for (int i = 0; i < count; i++)
                    {
                        XmlElement xe1 = doc.CreateElement("value_count");
                        xe1.InnerText = "0";
                        xe.AppendChild(xe1);
                    }
                }

            }
            reader.Close();
            doc.Save(location);

        }

        public Boolean IRQ_Add_Node(string mode, string Group_Number, ref string location)
        {

            XmlDocument xmlDoc = new XmlDocument();
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.IgnoreComments = true;
            XmlReader reader = XmlReader.Create(location, settings);
            xmlDoc.Load(reader);
            XmlNode root = xmlDoc.SelectSingleNode("IRQ_Set");
            XmlNodeList xn = xmlDoc.SelectSingleNode("IRQ_Set").ChildNodes;
            foreach (XmlNode node in xn)
            {
                XmlElement xe = (XmlElement)node;//NODE FUN
                if (node.Name == "FUN")
                {
                    if ((xe.GetAttribute("Register") == mode))
                    {
                        XmlNodeList xn1 = xe.SelectNodes("value");
                        foreach (XmlNode node1 in xn1)
                        {
                            XmlElement xe1 = (XmlElement)node1;//node value
                            if ((xe1.InnerText == Group_Number))
                                firstAdd = false;
                        }
                        if (firstAdd)
                        {
                            XmlElement xe11 = xmlDoc.CreateElement("value");
                            xe11.InnerText = Group_Number;
                            xe.AppendChild(xe11);
                        }
                    }
                }
            }
            reader.Close();

            xmlDoc.Save(location);
            if (firstAdd)
                return true;
            else
            {
                firstAdd = true;//恢复默认值
                return false;
            }

        }
        public void read_irq_xml_node(ref int value, ref string location)//name 寄存器)
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.IgnoreComments = true;
            XmlReader reader = XmlReader.Create(location, settings);
            xmlDoc.Load(reader);
            XmlNode root = xmlDoc.SelectSingleNode("IRQ_Set");
            XmlNodeList xn = xmlDoc.SelectSingleNode("IRQ_Set").ChildNodes;
            foreach (XmlNode node in xn)
            {
                XmlElement xe = (XmlElement)node;//NODE FUN

                if (xe.GetAttribute("Register") == "Var")
                {
                    XmlNodeList sxe = xe.ChildNodes;
                    foreach (XmlNode node1 in sxe)
                    {
                        XmlElement xe1 = (XmlElement)node1;
                        if (node1.Name == "Var_value")
                        {
                            value = Convert.ToInt32(xe1.InnerText, 16);
                        }
                      
                    }
                }


            }

            reader.Close();

            xmlDoc.Save(location);
        }
    }
}
