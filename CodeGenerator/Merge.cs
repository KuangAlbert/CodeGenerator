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

namespace MERGE
{
    public class Merge
    {
        string[] port = { "0", "8", "9", "10", "20", "30" ,"11"};//引脚端口号
        string[] status = { "ACTIVE", "RESET", "STANDBY" };
        int pin_port_number;//端口号个数
        public void Creat_Gpio_Xml(string location)//创建XML文件
        {
            XmlDocument xmldoc = new XmlDocument();
            XmlDeclaration xmldecl = xmldoc.CreateXmlDeclaration("1.0", "ISO-8859-1", null);
            xmldoc.AppendChild(xmldecl);
            XmlElement xmlelem = xmldoc.CreateElement("GPIO_Set");
            xmldoc.AppendChild(xmlelem);
            XmlNode root = xmldoc.SelectSingleNode("GPIO_Set");
            //**************************************************************************************//
            XmlElement nxel = xmldoc.CreateElement("FUN");
            nxel.SetAttribute("Register", "PMC");
            //**************************************************************************************//
            XmlElement axel = xmldoc.CreateElement("FUN");
            axel.SetAttribute("Register", "PM");
            ////**************************************************************************************//
            XmlElement bxel = xmldoc.CreateElement("FUN");
            bxel.SetAttribute("Register", "PIBC");
            ////**************************************************************************************//
            XmlElement cxel = xmldoc.CreateElement("FUN");
            cxel.SetAttribute("Register", "PU");
            ////**************************************************************************************//
            XmlElement dxel = xmldoc.CreateElement("FUN");
            dxel.SetAttribute("Register", "PD");
            ////**************************************************************************************//
            XmlElement exel = xmldoc.CreateElement("FUN");
            exel.SetAttribute("Register", "PBDC");
            ////**************************************************************************************//
            XmlElement fxel = xmldoc.CreateElement("FUN");
            fxel.SetAttribute("Register", "PDSC");
            ////**************************************************************************************//
            XmlElement gxel = xmldoc.CreateElement("FUN");
            gxel.SetAttribute("Register", "PODC");
            //**************************************************************************************//
            XmlElement hxel = xmldoc.CreateElement("FUN");
            hxel.SetAttribute("Register", "PIPC");
            //**************************************************************************************//
            XmlElement ixel = xmldoc.CreateElement("FUN");
            ixel.SetAttribute("Register", "PFCAE");
            //**************************************************************************************//
            XmlElement jxel = xmldoc.CreateElement("FUN");
            jxel.SetAttribute("Register", "PFCE");
            //**************************************************************************************//
            XmlElement kxel = xmldoc.CreateElement("FUN");
            kxel.SetAttribute("Register", "PFC");
            //**************************************************************************************//
            XmlElement lxel = xmldoc.CreateElement("FUN");//set Port Value Register
            lxel.SetAttribute("Register", "PV");

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

            xmldoc.Save(location);

        }
        public void Create_Default_Gpio_Xml( string location,int pin_number )//创建Default XML文件
        {
            XmlDocument xmldoc = new XmlDocument();
            XmlDeclaration xmldecl = xmldoc.CreateXmlDeclaration("1.0", "ISO-8859-1", null);
            xmldoc.AppendChild(xmldecl);
            XmlElement xmlelem = xmldoc.CreateElement("GPIO_Set");
            xmldoc.AppendChild(xmlelem);
            XmlNode root = xmldoc.SelectSingleNode("GPIO_Set");
            //**************************************************************************************//
            XmlElement nxel = xmldoc.CreateElement("FUN");
            nxel.SetAttribute("Register", "PMC");
            //**************************************************************************************//
            XmlElement axel = xmldoc.CreateElement("FUN");
            axel.SetAttribute("Register", "PM");
            ////**************************************************************************************//
            XmlElement bxel = xmldoc.CreateElement("FUN");
            bxel.SetAttribute("Register", "PIBC");
            ////**************************************************************************************//
            XmlElement cxel = xmldoc.CreateElement("FUN");
            cxel.SetAttribute("Register", "PU");
            ////**************************************************************************************//
            XmlElement dxel = xmldoc.CreateElement("FUN");
            dxel.SetAttribute("Register", "PD");
            ////**************************************************************************************//
            XmlElement exel = xmldoc.CreateElement("FUN");
            exel.SetAttribute("Register", "PBDC");
            ////**************************************************************************************//
            XmlElement fxel = xmldoc.CreateElement("FUN");
            fxel.SetAttribute("Register", "PDSC");
            ////**************************************************************************************//
            XmlElement gxel = xmldoc.CreateElement("FUN");
            gxel.SetAttribute("Register", "PODC");
            //**************************************************************************************//
            XmlElement hxel = xmldoc.CreateElement("FUN");
            hxel.SetAttribute("Register", "PIPC");
            //**************************************************************************************//
            XmlElement ixel = xmldoc.CreateElement("FUN");
            ixel.SetAttribute("Register", "PFCAE");
            //**************************************************************************************//
            XmlElement jxel = xmldoc.CreateElement("FUN");
            jxel.SetAttribute("Register", "PFCE");
            //**************************************************************************************//
            XmlElement kxel = xmldoc.CreateElement("FUN");
            kxel.SetAttribute("Register", "PFC");
            //**************************************************************************************//
            XmlElement lxel = xmldoc.CreateElement("FUN");
            lxel.SetAttribute("Register", "PV");


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
            xmldoc.Save(location);

            Add_Default_Node(location, pin_number);

        }
        public void Add_Default_Node(string location,int pin_number)//xml文件增加节点
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.IgnoreComments = true;
            XmlReader reader = XmlReader.Create(location, settings);
            xmlDoc.Load(reader);
            XmlNode root = xmlDoc.SelectSingleNode("GPIO_Set");
            XmlNodeList xn = xmlDoc.SelectSingleNode("GPIO_Set").ChildNodes;
            if (pin_number == 64)
            {
                pin_port_number = 6;
            }
            else if(pin_number == 100)
            { 
                pin_port_number = 7;
            }
                
            
            foreach (XmlNode node in xn)
            {
                XmlElement xe = (XmlElement)node;//NODE FUN
                //&& xe.GetAttribute("Register") != "PFCAE"&&xe.GetAttribute("Register") !="PFCE"&&xe.GetAttribute("Register") !="PFC"
                if (node.Name == "FUN" && xe.GetAttribute("Register").Equals("PM"))
                {
                    for (int j = 0; j < 3; j++)
                        for (int i = 0; i < pin_port_number; i++)
                        {
                            XmlNodeList sxe = xe.ChildNodes;
                            XmlElement xe1 = xmlDoc.CreateElement("Group");
                            xe1.SetAttribute("Mode", status[j]);
                            xe1.SetAttribute("Group_Number", port[i]);
                            XmlElement xe2 = xmlDoc.CreateElement("value");
                            xe2.InnerText = "FFFF";
                            xe1.AppendChild(xe2);
                            xe.AppendChild(xe1);
                        }
                }
                else
                {
                    for (int j = 0; j < 3; j++)
                        for (int i = 0; i < pin_port_number; i++)
                        {
                            XmlNodeList sxe = xe.ChildNodes;
                            XmlElement xe1 = xmlDoc.CreateElement("Group");
                            xe1.SetAttribute("Mode", status[j]);
                            xe1.SetAttribute("Group_Number", port[i]);
                            XmlElement xe2 = xmlDoc.CreateElement("value");
                            xe2.InnerText = "0000";
                            xe1.AppendChild(xe2);
                            xe.AppendChild(xe1);
                        }
                }
            }

            reader.Close();
            xmlDoc.Save(location);
        }
        public void mergeXml(string info_path, string default_path, string merge_path)
        {
            XmlDocument doc1 = new XmlDocument();
            doc1.Load(info_path);
            XmlNodeList xn1 = doc1.SelectSingleNode("GPIO_Set").ChildNodes;

            XmlDocument doc2 = new XmlDocument();
            doc2.Load(default_path);
            XmlNodeList xn2 = doc2.SelectSingleNode("GPIO_Set").ChildNodes;

            foreach (XmlNode node2 in xn2)
            {
                XmlElement xe2 = (XmlElement)node2;
                if (xe2.Name == "FUN")
                {
                    String name2 = xe2.GetAttribute("Register");
                    foreach (XmlNode node21 in node2)
                    {
                        XmlElement xe21 = (XmlElement)node21;
                        String mode2 = xe21.GetAttribute("Mode");
                        String number2 = xe21.GetAttribute("Group_Number");
                        XmlElement xs2 = (XmlElement)node21.FirstChild;
                        int value2 = Convert.ToInt32(xs2.InnerText, 16);
                        int flag_node = 0;
                        foreach (XmlNode node1 in xn1)
                        {
                            XmlElement xe1 = (XmlElement)node1;
                            if (xe1.Name == "FUN" && xe1.GetAttribute("Register").Equals(name2))
                            {
                                foreach (XmlNode node11 in node1)
                                {
                                    XmlElement xe11 = (XmlElement)node11;
                                    String mode1 = xe11.GetAttribute("Mode");
                                    String number1 = xe11.GetAttribute("Group_Number");
                                    XmlElement xs1 = (XmlElement)node11.FirstChild;
                                    int value1 = Convert.ToInt32(xs1.InnerText, 16);
                                    if (mode1.Equals(mode2) && number1.Equals(number2)) if (mode2.Equals(mode1) && number2.Equals(number1))
                                        {
                                            this.add_node(name2, number1, value1, mode1, merge_path);
                                            flag_node = 1;
                                        }
                                }
                            }
                        }
                        if (flag_node == 0)
                        {
                            this.add_node(name2, number2, value2, mode2, merge_path);
                            flag_node = 0;

                        }
                    }
                }
            }
            foreach (XmlNode node in xn1)
            {
                XmlElement xe = (XmlElement)node;
                if (xe.Name == "LIST")
                {
                    String item = xe.GetAttribute("Item");
                    foreach (XmlNode node1 in node)
                    {
                        XmlElement xe1 = (XmlElement)node1;
                        String cnt = xe1.GetAttribute("cnt");
                        String gro = xe1.GetAttribute("gro");
                        String por = xe1.GetAttribute("por");
                        String port = xe1.InnerText;
                        this.Add_List_Node(item, cnt, gro, por, port, merge_path);
                    }
                }

            }
        }

        public void add_node(string name, string number, int value, string mode, string merge_path)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(merge_path);
            XmlNode root = xmlDoc.SelectSingleNode("GPIO_Set");
            XmlNodeList xn = xmlDoc.SelectSingleNode("GPIO_Set").ChildNodes;
            foreach (XmlNode node in xn)
            {
                XmlElement xe = (XmlElement)node;//NODE FUN
                //&& xe.GetAttribute("Register") != "PFCAE"&&xe.GetAttribute("Register") !="PFCE"&&xe.GetAttribute("Register") !="PFC"
                if (node.Name == "FUN" && xe.GetAttribute("Register").Equals(name))
                {
                    XmlElement xe1 = xmlDoc.CreateElement("Group");
                    xe1.SetAttribute("Mode", mode);
                    xe1.SetAttribute("Group_Number", number);
                    XmlElement xe2 = xmlDoc.CreateElement("value");
                    xe2.InnerText = string.Format("0x{0:X4}", value); //xe2.InnerText = Convert.ToString(value, 16);
                    xe1.AppendChild(xe2);
                    xe.AppendChild(xe1);

                }
            }
            xmlDoc.Save(merge_path);
        }


        public void Add_List_Node(string item, string cnt, string gro, string pro, string port, string location)//xml文件增加节点
        {

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(location);
            XmlNode root = xmlDoc.SelectSingleNode("GPIO_Set");
            XmlElement xn = xmlDoc.CreateElement("LIST");
            xn.SetAttribute("Item", item);
            XmlElement xe = xmlDoc.CreateElement("v");
            xe.SetAttribute("cnt", cnt);
            xe.SetAttribute("gro", gro);
            xe.SetAttribute("pro", pro);
            xe.InnerText = port;

            root.AppendChild(xn);
            xn.AppendChild(xe);

            xmlDoc.Save(location);

        }
    }
}