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

namespace IO
{
    public class Io
    {
        public void Creat_Gpio_Xml(ref string location)//创建XML文件
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
            //**************************************************************************************//
            XmlElement xel1 = xmldoc.CreateElement("LIST");
            xel1.SetAttribute("Item", "LPORT");
            //**************************************************************************************//
            XmlElement xel2 = xmldoc.CreateElement("LIST");
            xel2.SetAttribute("Item", "LMODE");
            //**************************************************************************************//
            XmlElement xel3 = xmldoc.CreateElement("LIST");
            xel3.SetAttribute("Item", "LAG");
            //**************************************************************************************//
            XmlElement xel4 = xmldoc.CreateElement("LIST");
            xel4.SetAttribute("Item", "LFUN");
            //**************************************************************************************//
            XmlElement xel5 = xmldoc.CreateElement("LIST");
            xel5.SetAttribute("Item", "LINOUT");
            //**************************************************************************************//
            XmlElement xel6 = xmldoc.CreateElement("LIST");
            xel6.SetAttribute("Item", "LPIBC");
            //**************************************************************************************//
            XmlElement xel7 = xmldoc.CreateElement("LIST");
            xel7.SetAttribute("Item", "LPU");
            //**************************************************************************************//
            XmlElement xel8 = xmldoc.CreateElement("LIST");
            xel8.SetAttribute("Item", "LPD");
            //**************************************************************************************//
            XmlElement xel9 = xmldoc.CreateElement("LIST");
            xel9.SetAttribute("Item", "LPBDC");
            //**************************************************************************************//
            XmlElement xel10 = xmldoc.CreateElement("LIST");
            xel10.SetAttribute("Item", "LPDSC");
            //**************************************************************************************//
            XmlElement xel11 = xmldoc.CreateElement("LIST");
            xel11.SetAttribute("Item", "LPODC");
            //**************************************************************************************//
            XmlElement xel12 = xmldoc.CreateElement("LIST");
            xel12.SetAttribute("Item", "LPV");

            XmlElement xel13 = xmldoc.CreateElement("INF");

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

            root.AppendChild(xel1);
            root.AppendChild(xel2);
            root.AppendChild(xel3);
            root.AppendChild(xel4);
            root.AppendChild(xel5);
            root.AppendChild(xel6);
            root.AppendChild(xel7);
            root.AppendChild(xel8);
            root.AppendChild(xel9);
            root.AppendChild(xel10);
            root.AppendChild(xel11);
            root.AppendChild(xel12);
            root.AppendChild(xel13);
            xmldoc.Save(location);

        }

        public void Add_Node(string mode, string Group_Number, ref string location)//xml文件增加节点
        {

            XmlDocument xmlDoc = new XmlDocument();
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.IgnoreComments = true;
            XmlReader reader = XmlReader.Create(location, settings);
            xmlDoc.Load(reader);
            XmlNode root = xmlDoc.SelectSingleNode("GPIO_Set");
            XmlNodeList xn = xmlDoc.SelectSingleNode("GPIO_Set").ChildNodes;
            foreach (XmlNode node in xn)
            {
                XmlElement xe = (XmlElement)node;//NODE FUN
                //&& xe.GetAttribute("Register") != "PFCAE"&&xe.GetAttribute("Register") !="PFCE"&&xe.GetAttribute("Register") !="PFC"
                if (node.Name == "FUN")
                {
                    XmlElement xe1 = xmlDoc.CreateElement("Group");
                    xe1.SetAttribute("Mode", mode);
                    xe1.SetAttribute("Group_Number", Group_Number);
                    XmlElement xe2 = xmlDoc.CreateElement("value");
                    xe2.InnerText = string.Format("0x{0:X4}", 0); 
                    xe1.AppendChild(xe2);
                    xe.AppendChild(xe1);

                }
            }

            reader.Close();

            xmlDoc.Save(location);

        }

        public void change_node(string name, string Group_Number, int change, string mode, ref string location)//修改XML文件
        {
            XmlDocument doc = new XmlDocument();
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.IgnoreComments = true;
            XmlReader reader = XmlReader.Create(location, settings);
            doc.Load(reader);
            XmlNodeList xn = doc.SelectSingleNode("GPIO_Set").ChildNodes;
            foreach (XmlNode node in xn)
            {
                XmlElement xe = (XmlElement)node;
                if (xe.GetAttribute("Register") == name)
                {
                    XmlNodeList sxe = xe.ChildNodes;
                    foreach (XmlNode node1 in sxe)//foreach group in FUN
                    {
                        XmlElement xe1 = (XmlElement)node1;
                        if ((xe1.GetAttribute("Group_Number") == Group_Number) && (xe1.GetAttribute("Mode") == mode))
                        {
                            XmlNodeList xe2 = xe1.ChildNodes;
                            foreach (XmlNode node2 in xe2)//foreach 
                            {
                                XmlElement xe3 = (XmlElement)node2;
                                if (node2.Name == "value")
                                {
                                    node2.InnerText = string.Format("0x{0:X4}", change);   //node2.InnerText = Convert.ToString(change, 16);
                                }
                            }
                        }
                    }
                }
            }
            reader.Close();
            doc.Save(location);

        }

        public void delete_list_node(string value0, string value1, string value2, string value3, string value4, string value5, string value6, string value7, string value8, string value9, string value10, string value11, ref string location, string gro, int pro)//添加时，对表格内容进行写入。修改时，对表格内容进行替换
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.IgnoreComments = true;
            XmlReader reader = XmlReader.Create(location, settings);
            xmlDoc.Load(reader);
            XmlNode root = xmlDoc.SelectSingleNode("GPIO_Set");
            XmlNodeList xn = xmlDoc.SelectSingleNode("GPIO_Set").ChildNodes;
            int enable_port = 0, enable_mode = 0, new_count = 0, statusCount = 0, index = 0 ;
            string[] number = new string[3];
            foreach (XmlNode node in xn)//foreach fun and list
            {
                XmlElement xe = (XmlElement)node;

                if (xe.GetAttribute("Item") == "LPORT")
                {


                    if (xe.ChildNodes.Count != 0)
                    {
                        XmlNodeList sxe = xe.ChildNodes;
                        foreach (XmlNode node1 in sxe)
                        {

                            XmlElement sxe1 = (XmlElement)node1;


                            if (sxe1.InnerText == value0)
                            {
                                number[statusCount] = sxe1.GetAttribute("cnt");

                                enable_port = 1;
                                statusCount++;
                                if (statusCount == 3)
                                    statusCount = 0;
                            }
                        }

                    }


                }
                if (xe.GetAttribute("Item") == "LMODE")
                {
                    if (xe.ChildNodes.Count != 0)
                    {
                        XmlNodeList sxe = xe.ChildNodes;
                        foreach (XmlNode node1 in sxe)
                        {
                            XmlElement sxe1 = (XmlElement)node1;
                            if (sxe1.InnerText == value1 )
                            {
                                for (int i = 0; i < number.Length; i++)
                                {
                                    if (sxe1.GetAttribute("cnt") == number[i])
                                    {
                                        enable_mode = 1;
                                        index = i;
                                    }
                                }
                                    
                            }
                        }
                    }
                }
            }
            if ((enable_port == 1) && (enable_mode == 1))
            {
                foreach (XmlNode node in xn)
                {
                    XmlElement xe = (XmlElement)node;
                    XmlNodeList sxe = xe.ChildNodes;
                    if (xe.GetAttribute("Item") == "LAG")
                    {
                        foreach (XmlNode node1 in sxe)
                        {
                            XmlElement sxe1 = (XmlElement)node1;
                            if (sxe1.GetAttribute("cnt") == number[index])
                            {
                                sxe1.InnerText = value2;
                            }
                        }
                    }
                    if (xe.GetAttribute("Item") == "LFUN")
                    {
                        foreach (XmlNode node1 in sxe)
                        {
                            XmlElement sxe1 = (XmlElement)node1;
                            if (sxe1.GetAttribute("cnt") == number[index])
                            {
                                sxe1.InnerText = value3;
                            }
                        }
                    }
                    if (xe.GetAttribute("Item") == "LINOUT")
                    {
                        foreach (XmlNode node1 in sxe)
                        {
                            XmlElement sxe1 = (XmlElement)node1;
                            if (sxe1.GetAttribute("cnt") == number[index])
                            {
                                sxe1.InnerText = value4;
                            }
                        }
                    }
                    if (xe.GetAttribute("Item") == "LPIBC")
                    {
                        foreach (XmlNode node1 in sxe)
                        {
                            XmlElement sxe1 = (XmlElement)node1;
                            if (sxe1.GetAttribute("cnt") == number[index])
                            {
                                sxe1.InnerText = value5;
                            }
                        }
                    }
                    if (xe.GetAttribute("Item") == "LPU")
                    {
                        foreach (XmlNode node1 in sxe)
                        {
                            XmlElement sxe1 = (XmlElement)node1;
                            if (sxe1.GetAttribute("cnt") == number[index])
                            {
                                sxe1.InnerText = value6;
                            }
                        }
                    }
                    if (xe.GetAttribute("Item") == "LPD")
                    {
                        foreach (XmlNode node1 in sxe)
                        {
                            XmlElement sxe1 = (XmlElement)node1;
                            if (sxe1.GetAttribute("cnt") == number[index])
                            {

                                sxe1.InnerText = value7;
                            }
                        }
                    }
                    if (xe.GetAttribute("Item") == "LPBDC")
                    {
                        foreach (XmlNode node1 in sxe)
                        {
                            XmlElement sxe1 = (XmlElement)node1;
                            if (sxe1.GetAttribute("cnt") == number[index])
                            {
                                sxe1.InnerText = value8;
                            }
                        }
                    }
                    if (xe.GetAttribute("Item") == "LPDSC")
                    {
                        foreach (XmlNode node1 in sxe)
                        {
                            XmlElement sxe1 = (XmlElement)node1;
                            if (sxe1.GetAttribute("cnt") == number[index])
                            {
                                sxe1.InnerText = value9;
                            }
                        }
                    }
                    if (xe.GetAttribute("Item") == "LPODC")
                    {
                        foreach (XmlNode node1 in sxe)
                        {
                            XmlElement sxe1 = (XmlElement)node1;
                            if (sxe1.GetAttribute("cnt") == number[index])
                            {
                                sxe1.InnerText = value10;
                            }
                        }
                    }
                    if (xe.GetAttribute("Item") == "LPV")
                    {
                        foreach (XmlNode node1 in sxe)
                        {
                            XmlElement sxe1 = (XmlElement)node1;
                            if (sxe1.GetAttribute("cnt") == number[index])
                            {
                                sxe1.InnerText = value11;
                            }
                        }
                    }
                }
            }
            else
            {
                foreach (XmlNode node in xn)
                {
                    XmlElement xe = (XmlElement)node;//NODE FUN

                    if (xe.GetAttribute("Item") == "LPORT")
                    {

                        XmlNodeList sxe = xe.ChildNodes;
                        foreach (XmlNode node1 in sxe)
                        {
                            XmlElement sxe1 = (XmlElement)node1;
                            new_count = Convert.ToInt32(sxe1.GetAttribute("cnt"));
                        }
                        new_count++;
                        XmlElement xe1 = xmlDoc.CreateElement("v");
                        xe1.SetAttribute("cnt", new_count.ToString());
                        xe1.SetAttribute("gro", gro);
                        xe1.SetAttribute("por", pro.ToString());
                        xe1.InnerText = value0;
                        xe.AppendChild(xe1);
                    }
                    if (xe.GetAttribute("Item") == "LMODE")
                    {
                        XmlNodeList sxe = xe.ChildNodes;
                        XmlElement xe2 = xmlDoc.CreateElement("v");
                        xe2.SetAttribute("cnt", new_count.ToString());
                        xe2.InnerText = value1;
                        xe.AppendChild(xe2);
                    }
                    if (xe.GetAttribute("Item") == "LAG")
                    {
                        XmlNodeList sxe = xe.ChildNodes;
                        XmlElement xe3 = xmlDoc.CreateElement("v");
                        xe3.SetAttribute("cnt", new_count.ToString());
                        xe3.InnerText = value2;
                        xe.AppendChild(xe3);
                    }
                    if (xe.GetAttribute("Item") == "LFUN")
                    {
                        XmlNodeList sxe = xe.ChildNodes;
                        XmlElement xe4 = xmlDoc.CreateElement("v");
                        xe4.SetAttribute("cnt", new_count.ToString());
                        xe4.InnerText = value3;
                        xe.AppendChild(xe4);
                    }
                    if (xe.GetAttribute("Item") == "LINOUT")
                    {
                        XmlNodeList sxe = xe.ChildNodes;
                        XmlElement xe5 = xmlDoc.CreateElement("v");
                        xe5.SetAttribute("cnt", new_count.ToString());
                        xe5.InnerText = value4;
                        xe.AppendChild(xe5);
                    }
                    if (xe.GetAttribute("Item") == "LPIBC")
                    {
                        XmlNodeList sxe = xe.ChildNodes;
                        XmlElement xe6 = xmlDoc.CreateElement("v");
                        xe6.SetAttribute("cnt", new_count.ToString());
                        xe6.InnerText = value5;
                        xe.AppendChild(xe6);
                    }
                    if (xe.GetAttribute("Item") == "LPU")
                    {
                        XmlNodeList sxe = xe.ChildNodes;
                        XmlElement xe7 = xmlDoc.CreateElement("v");
                        xe7.SetAttribute("cnt", new_count.ToString());
                        xe7.InnerText = value6;
                        xe.AppendChild(xe7);
                    }
                    if (xe.GetAttribute("Item") == "LPD")
                    {
                        XmlNodeList sxe = xe.ChildNodes;
                        XmlElement xe8 = xmlDoc.CreateElement("v");
                        xe8.SetAttribute("cnt", new_count.ToString());
                        xe8.InnerText = value7;
                        xe.AppendChild(xe8);
                    }
                    if (xe.GetAttribute("Item") == "LPBDC")
                    {
                        XmlNodeList sxe = xe.ChildNodes;
                        XmlElement xe9 = xmlDoc.CreateElement("v");
                        xe9.SetAttribute("cnt", new_count.ToString());
                        xe9.InnerText = value8;
                        xe.AppendChild(xe9);
                    }
                    if (xe.GetAttribute("Item") == "LPDSC")
                    {
                        XmlNodeList sxe = xe.ChildNodes;
                        XmlElement xe10 = xmlDoc.CreateElement("v");
                        xe10.SetAttribute("cnt", new_count.ToString());
                        xe10.InnerText = value9;
                        xe.AppendChild(xe10);
                    }
                    if (xe.GetAttribute("Item") == "LPODC")
                    {
                        XmlNodeList sxe = xe.ChildNodes;
                        XmlElement xe11 = xmlDoc.CreateElement("v");
                        xe11.SetAttribute("cnt", new_count.ToString());
                        xe11.InnerText = value10;
                        xe.AppendChild(xe11);
                    }
                    if (xe.GetAttribute("Item") == "LPV")
                    {
                        XmlNodeList sxe = xe.ChildNodes;
                        XmlElement xe12 = xmlDoc.CreateElement("v");
                        xe12.SetAttribute("cnt", new_count.ToString());
                        xe12.InnerText = value11;
                        xe.AppendChild(xe12);
                    }
                }
            }
            reader.Close();

            xmlDoc.Save(location);
        }

        public void delete_list_node_con(string value0, string value1, string value2, string value3, string value4, string value5, string value6, string value7, string value8, string value9, string value10, string value11, ref string location)//remove时对表格内容进行清空
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.IgnoreComments = true;
            XmlReader reader = XmlReader.Create(location, settings);
            xmlDoc.Load(reader);
            XmlNode root = xmlDoc.SelectSingleNode("GPIO_Set");
            XmlNodeList xn = xmlDoc.SelectSingleNode("GPIO_Set").ChildNodes;
            int enable_port = 0, enable_mode = 0, trans_value = 0;
            string delete_group = "", delete_port = "";
            string number = "";
            foreach (XmlNode node in xn)//foreach fun and list
            {

                XmlElement xe = (XmlElement)node;

                if (xe.GetAttribute("Item") == "LPORT")
                {
                    if (xe.ChildNodes.Count != 0)
                    {
                        XmlNodeList sxe = xe.ChildNodes;

                        foreach (XmlNode node1 in sxe)
                        {
                            XmlElement sxe1 = (XmlElement)node1;
                            if (sxe1.InnerText == value0)
                            {
                                delete_group = sxe1.GetAttribute("gro");
                                delete_port = sxe1.GetAttribute("por");
                                number = sxe1.GetAttribute("cnt");
                                enable_port = 1;
                                node.RemoveChild(sxe1);
                            }
                        }
                    }

                }

            }
            foreach (XmlNode node in xn)//foreach fun and list
            {

                XmlElement xe = (XmlElement)node;
                if ((xe.GetAttribute("Item") == "LMODE"))
                {
                    if (xe.ChildNodes.Count != 0)
                    {
                        XmlNodeList sxe = xe.ChildNodes;
                        foreach (XmlNode node1 in sxe)
                        {
                            XmlElement sxe1 = (XmlElement)node1;
                            if (sxe1.InnerText == value1 && sxe1.GetAttribute("cnt") == number)
                            {
                                enable_mode = 1;
                                node.RemoveChild(sxe1);
                            }
                        }
                    }
                }
            }
            if ((enable_port == 1) && (enable_mode == 1))
            {
                foreach (XmlNode node in xn)
                {
                    XmlElement xe = (XmlElement)node;
                    XmlNodeList sxe = xe.ChildNodes;
                    if (xe.GetAttribute("Item") == "LAG")
                    {
                        foreach (XmlNode node1 in sxe)
                        {
                            XmlElement sxe1 = (XmlElement)node1;
                            if (sxe1.GetAttribute("cnt") == number)
                            {
                                node.RemoveChild(sxe1);
                            }
                        }
                    }

                    if (xe.GetAttribute("Item") == "LFUN")
                    {
                        foreach (XmlNode node1 in sxe)
                        {
                            XmlElement sxe1 = (XmlElement)node1;
                            if (sxe1.GetAttribute("cnt") == number)
                            {
                                node.RemoveChild(sxe1);
                            }
                        }
                    }

                    if (xe.GetAttribute("Item") == "LINOUT")
                    {
                        foreach (XmlNode node1 in sxe)
                        {
                            XmlElement sxe1 = (XmlElement)node1;
                            if (sxe1.GetAttribute("cnt") == number)
                            {
                                node.RemoveChild(sxe1);
                            }
                        }
                    }

                    if (xe.GetAttribute("Item") == "LPIBC")
                    {
                        foreach (XmlNode node1 in sxe)
                        {
                            XmlElement sxe1 = (XmlElement)node1;
                            if (sxe1.GetAttribute("cnt") == number)
                            {
                                node.RemoveChild(sxe1);
                            }
                        }
                    }

                    if (xe.GetAttribute("Item") == "LPU")
                    {
                        foreach (XmlNode node1 in sxe)
                        {
                            XmlElement sxe1 = (XmlElement)node1;
                            if (sxe1.GetAttribute("cnt") == number)
                            {
                                node.RemoveChild(sxe1);
                            }
                        }
                    }

                    if (xe.GetAttribute("Item") == "LPD")
                    {
                        foreach (XmlNode node1 in sxe)
                        {
                            XmlElement sxe1 = (XmlElement)node1;
                            if (sxe1.GetAttribute("cnt") == number)
                            {
                                node.RemoveChild(sxe1);
                            }
                        }
                    }

                    if (xe.GetAttribute("Item") == "LPBDC")
                    {
                        foreach (XmlNode node1 in sxe)
                        {
                            XmlElement sxe1 = (XmlElement)node1;
                            if (sxe1.GetAttribute("cnt") == number)
                            {
                                node.RemoveChild(sxe1);
                            }
                        }
                    }

                    if (xe.GetAttribute("Item") == "LPDSC")
                    {
                        foreach (XmlNode node1 in sxe)
                        {
                            XmlElement sxe1 = (XmlElement)node1;
                            if (sxe1.GetAttribute("cnt") == number)
                            {
                                node.RemoveChild(sxe1);
                            }
                        }
                    }

                    if (xe.GetAttribute("Item") == "LPODC")
                    {
                        foreach (XmlNode node1 in sxe)
                        {
                            XmlElement sxe1 = (XmlElement)node1;
                            if (sxe1.GetAttribute("cnt") == number)
                            {
                                node.RemoveChild(sxe1);
                            }
                        }
                    }
                    if (xe.GetAttribute("Item") == "LPV")
                    {
                        foreach (XmlNode node1 in sxe)
                        {
                            XmlElement sxe1 = (XmlElement)node1;
                            if (sxe1.GetAttribute("cnt") == number)
                            {
                                node.RemoveChild(sxe1);
                            }
                        }
                    }
                }
            }

            foreach (XmlNode node in xn)//foreach fun and list
            {
                XmlElement xe = (XmlElement)node;
                if (xe.GetAttribute("Register") == "PMC")
                {
                    if (xe.ChildNodes.Count != 0)
                    {
                        XmlNodeList sxe = xe.ChildNodes;

                        foreach (XmlNode node1 in sxe)
                        {
                            XmlElement sxe1 = (XmlElement)node1;
                            if ((sxe1.GetAttribute("Group_Number") == delete_group) && (sxe1.GetAttribute("Mode") == value1))
                            {
                                XmlNodeList xe2 = sxe1.ChildNodes;
                                foreach (XmlNode node2 in xe2)//foreach 
                                {
                                    XmlElement xe3 = (XmlElement)node2;
                                    if (node2.Name == "value")
                                    {

                                        trans_value = Convert.ToInt32(node2.InnerText, 16);
                                        trans_value = trans_value & ((1 << Convert.ToInt32(delete_port, 16)) ^ 65535);
                                        node2.InnerText = string.Format("0x{0:X4}", trans_value);    //node2.InnerText = Convert.ToString(trans_value, 16);

                                    }
                                }
                            }
                        }
                    }


                }
                if (xe.GetAttribute("Register") == "PM")
                {
                    if (xe.ChildNodes.Count != 0)
                    {
                        XmlNodeList sxe = xe.ChildNodes;

                        foreach (XmlNode node1 in sxe)
                        {
                            XmlElement sxe1 = (XmlElement)node1;
                            if ((sxe1.GetAttribute("Group_Number") == delete_group) && (sxe1.GetAttribute("Mode") == value1))
                            {
                                XmlNodeList xe2 = sxe1.ChildNodes;
                                foreach (XmlNode node2 in xe2)//foreach 
                                {
                                    XmlElement xe3 = (XmlElement)node2;
                                    if (node2.Name == "value")
                                    {
                                        trans_value = Convert.ToInt32(node2.InnerText, 16);
                                        trans_value = trans_value | (1 << Convert.ToInt32(delete_port, 16));
                                        node2.InnerText = string.Format("0x{0:X4}", trans_value); //node2.InnerText = Convert.ToString(trans_value, 16);

                                    }
                                }
                            }
                        }
                    }


                }
                if (xe.GetAttribute("Register") == "PIBC")
                {
                    if (xe.ChildNodes.Count != 0)
                    {
                        XmlNodeList sxe = xe.ChildNodes;

                        foreach (XmlNode node1 in sxe)
                        {
                            XmlElement sxe1 = (XmlElement)node1;
                            if ((sxe1.GetAttribute("Group_Number") == delete_group) && (sxe1.GetAttribute("Mode") == value1))
                            {
                                XmlNodeList xe2 = sxe1.ChildNodes;
                                foreach (XmlNode node2 in xe2)//foreach 
                                {
                                    XmlElement xe3 = (XmlElement)node2;
                                    if (node2.Name == "value")
                                    {
                                        trans_value = Convert.ToInt32(node2.InnerText, 16);
                                        trans_value = trans_value & ((1 << Convert.ToInt32(delete_port, 16)) ^ 65535);
                                        node2.InnerText = string.Format("0x{0:X4}", trans_value); //node2.InnerText = Convert.ToString(trans_value, 16);

                                    }
                                }
                            }
                        }
                    }


                }
                if (xe.GetAttribute("Register") == "PU")
                {
                    if (xe.ChildNodes.Count != 0)
                    {
                        XmlNodeList sxe = xe.ChildNodes;

                        foreach (XmlNode node1 in sxe)
                        {
                            XmlElement sxe1 = (XmlElement)node1;
                            if ((sxe1.GetAttribute("Group_Number") == delete_group) && (sxe1.GetAttribute("Mode") == value1))
                            {
                                XmlNodeList xe2 = sxe1.ChildNodes;
                                foreach (XmlNode node2 in xe2)//foreach 
                                {
                                    XmlElement xe3 = (XmlElement)node2;
                                    if (node2.Name == "value")
                                    {
                                        trans_value = Convert.ToInt32(node2.InnerText, 16);
                                        trans_value = trans_value & ((1 << Convert.ToInt32(delete_port, 16)) ^ 65535);
                                        node2.InnerText = string.Format("0x{0:X4}", trans_value); //node2.InnerText = Convert.ToString(trans_value, 16);

                                    }
                                }
                            }
                        }
                    }


                }
                if (xe.GetAttribute("Register") == "PD")
                {
                    if (xe.ChildNodes.Count != 0)
                    {
                        XmlNodeList sxe = xe.ChildNodes;

                        foreach (XmlNode node1 in sxe)
                        {
                            XmlElement sxe1 = (XmlElement)node1;
                            if ((sxe1.GetAttribute("Group_Number") == delete_group) && (sxe1.GetAttribute("Mode") == value1))
                            {
                                XmlNodeList xe2 = sxe1.ChildNodes;
                                foreach (XmlNode node2 in xe2)//foreach 
                                {
                                    XmlElement xe3 = (XmlElement)node2;
                                    if (node2.Name == "value")
                                    {
                                        trans_value = Convert.ToInt32(node2.InnerText, 16);
                                        trans_value = trans_value & ((1 << Convert.ToInt32(delete_port, 16)) ^ 65535);
                                        node2.InnerText = string.Format("0x{0:X4}", trans_value); //node2.InnerText = Convert.ToString(trans_value, 16);

                                    }
                                }
                            }
                        }
                    }


                }
                if (xe.GetAttribute("Register") == "PBDC")
                {
                    if (xe.ChildNodes.Count != 0)
                    {
                        XmlNodeList sxe = xe.ChildNodes;

                        foreach (XmlNode node1 in sxe)
                        {
                            XmlElement sxe1 = (XmlElement)node1;
                            if ((sxe1.GetAttribute("Group_Number") == delete_group) && (sxe1.GetAttribute("Mode") == value1))
                            {
                                XmlNodeList xe2 = sxe1.ChildNodes;
                                foreach (XmlNode node2 in xe2)//foreach 
                                {
                                    XmlElement xe3 = (XmlElement)node2;
                                    if (node2.Name == "value")
                                    {

                                        trans_value = Convert.ToInt32(node2.InnerText, 16);
                                        trans_value = trans_value & ((1 << Convert.ToInt32(delete_port, 16)) ^ 65535);
                                        node2.InnerText = string.Format("0x{0:X4}", trans_value); //node2.InnerText = Convert.ToString(trans_value, 16);

                                    }
                                }
                            }
                        }
                    }


                }
                if (xe.GetAttribute("Register") == "PDSC")
                {
                    if (xe.ChildNodes.Count != 0)
                    {
                        XmlNodeList sxe = xe.ChildNodes;

                        foreach (XmlNode node1 in sxe)
                        {
                            XmlElement sxe1 = (XmlElement)node1;
                            if ((sxe1.GetAttribute("Group_Number") == delete_group) && (sxe1.GetAttribute("Mode") == value1))
                            {
                                XmlNodeList xe2 = sxe1.ChildNodes;
                                foreach (XmlNode node2 in xe2)//foreach 
                                {
                                    XmlElement xe3 = (XmlElement)node2;
                                    if (node2.Name == "value")
                                    {
                                        trans_value = Convert.ToInt32(node2.InnerText, 16);
                                        trans_value = trans_value & ((1 << Convert.ToInt32(delete_port, 16)) ^ 65535);
                                        node2.InnerText = string.Format("0x{0:X4}", trans_value); //node2.InnerText = Convert.ToString(trans_value, 16);

                                    }
                                }
                            }
                        }
                    }


                }
                if (xe.GetAttribute("Register") == "PODC")
                {
                    if (xe.ChildNodes.Count != 0)
                    {
                        XmlNodeList sxe = xe.ChildNodes;

                        foreach (XmlNode node1 in sxe)
                        {
                            XmlElement sxe1 = (XmlElement)node1;
                            if ((sxe1.GetAttribute("Group_Number") == delete_group) && (sxe1.GetAttribute("Mode") == value1))
                            {
                                XmlNodeList xe2 = sxe1.ChildNodes;
                                foreach (XmlNode node2 in xe2)//foreach 
                                {
                                    XmlElement xe3 = (XmlElement)node2;
                                    if (node2.Name == "value")
                                    {

                                        trans_value = Convert.ToInt32(node2.InnerText, 16);
                                        trans_value = trans_value & ((1 << Convert.ToInt32(delete_port, 16)) ^ 65535);
                                        node2.InnerText = string.Format("0x{0:X4}", trans_value); //node2.InnerText = Convert.ToString(trans_value, 16);

                                    }
                                }
                            }
                        }
                    }


                }
                if (xe.GetAttribute("Register") == "PIPC")
                {
                    if (xe.ChildNodes.Count != 0)
                    {
                        XmlNodeList sxe = xe.ChildNodes;

                        foreach (XmlNode node1 in sxe)
                        {
                            XmlElement sxe1 = (XmlElement)node1;
                            if ((sxe1.GetAttribute("Group_Number") == delete_group) && (sxe1.GetAttribute("Mode") == value1))
                            {
                                XmlNodeList xe2 = sxe1.ChildNodes;
                                foreach (XmlNode node2 in xe2)//foreach 
                                {
                                    XmlElement xe3 = (XmlElement)node2;
                                    if (node2.Name == "value")
                                    {

                                        trans_value = Convert.ToInt32(node2.InnerText, 16);
                                        trans_value = trans_value & ((1 << Convert.ToInt32(delete_port, 16)) ^ 65535);
                                        node2.InnerText = string.Format("0x{0:X4}", trans_value); //node2.InnerText = Convert.ToString(trans_value, 16);

                                    }
                                }
                            }
                        }
                    }


                }
                if (xe.GetAttribute("Register") == "PFCAE")
                {
                    if (xe.ChildNodes.Count != 0)
                    {
                        XmlNodeList sxe = xe.ChildNodes;

                        foreach (XmlNode node1 in sxe)
                        {
                            XmlElement sxe1 = (XmlElement)node1;
                            if ((sxe1.GetAttribute("Group_Number") == delete_group) && (sxe1.GetAttribute("Mode") == value1))
                            {
                                XmlNodeList xe2 = sxe1.ChildNodes;
                                foreach (XmlNode node2 in xe2)//foreach 
                                {
                                    XmlElement xe3 = (XmlElement)node2;
                                    if (node2.Name == "value")
                                    {

                                        trans_value = Convert.ToInt32(node2.InnerText, 16);
                                        trans_value = trans_value & ((1 << Convert.ToInt32(delete_port, 16)) ^ 65535);
                                        node2.InnerText = Convert.ToString(trans_value, 16);

                                    }
                                }
                            }
                        }
                    }


                }
                if (xe.GetAttribute("Register") == "PFCE")
                {
                    if (xe.ChildNodes.Count != 0)
                    {
                        XmlNodeList sxe = xe.ChildNodes;

                        foreach (XmlNode node1 in sxe)
                        {
                            XmlElement sxe1 = (XmlElement)node1;
                            if ((sxe1.GetAttribute("Group_Number") == delete_group) && (sxe1.GetAttribute("Mode") == value1))
                            {
                                XmlNodeList xe2 = sxe1.ChildNodes;
                                foreach (XmlNode node2 in xe2)//foreach 
                                {
                                    XmlElement xe3 = (XmlElement)node2;
                                    if (node2.Name == "value")
                                    {

                                        trans_value = Convert.ToInt32(node2.InnerText, 16);
                                        trans_value = trans_value & ((1 << Convert.ToInt32(delete_port, 16)) ^ 65535);
                                        node2.InnerText = string.Format("0x{0:X4}", trans_value); //node2.InnerText = Convert.ToString(trans_value, 16);

                                    }
                                }
                            }
                        }
                    }


                }
                if (xe.GetAttribute("Register") == "PFC")
                {
                    if (xe.ChildNodes.Count != 0)
                    {
                        XmlNodeList sxe = xe.ChildNodes;

                        foreach (XmlNode node1 in sxe)
                        {
                            XmlElement sxe1 = (XmlElement)node1;
                            if ((sxe1.GetAttribute("Group_Number") == delete_group) && (sxe1.GetAttribute("Mode") == value1))
                            {
                                XmlNodeList xe2 = sxe1.ChildNodes;
                                foreach (XmlNode node2 in xe2)//foreach 
                                {
                                    XmlElement xe3 = (XmlElement)node2;
                                    if (node2.Name == "value")
                                    {

                                        trans_value = Convert.ToInt32(node2.InnerText, 16);
                                        trans_value = trans_value & ((1 << Convert.ToInt32(delete_port, 16)) ^ 65535);
                                        node2.InnerText = string.Format("0x{0:X4}", trans_value); //node2.InnerText = Convert.ToString(trans_value, 16);

                                    }
                                }
                            }
                        }
                    }


                }
                if (xe.GetAttribute("Register") == "PV")
                {
                    if (xe.ChildNodes.Count != 0)
                    {
                        XmlNodeList sxe = xe.ChildNodes;

                        foreach (XmlNode node1 in sxe)
                        {
                            XmlElement sxe1 = (XmlElement)node1;
                            if ((sxe1.GetAttribute("Group_Number") == delete_group) && (sxe1.GetAttribute("Mode") == value1))
                            {
                                XmlNodeList xe2 = sxe1.ChildNodes;
                                foreach (XmlNode node2 in xe2)//foreach 
                                {
                                    XmlElement xe3 = (XmlElement)node2;
                                    if (node2.Name == "value")
                                    {

                                        trans_value = Convert.ToInt32(node2.InnerText, 16);
                                        trans_value = trans_value & ((1 << Convert.ToInt32(delete_port, 16)) ^ 65535);
                                        node2.InnerText = string.Format("0x{0:X4}", trans_value); //node2.InnerText = Convert.ToString(trans_value, 16);

                                    }
                                }
                            }
                        }
                    }


                }
            }

            reader.Close();

            xmlDoc.Save(location);
        }

        public void read_xml_node(string Register, string mode, ref  int[] value, ref string location)//open时读取xml里的备份数据
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.IgnoreComments = true;
            XmlReader reader = XmlReader.Create(location, settings);
            xmlDoc.Load(reader);
            XmlNode root = xmlDoc.SelectSingleNode("GPIO_Set");
            XmlNodeList xn = xmlDoc.SelectSingleNode("GPIO_Set").ChildNodes;
            foreach (XmlNode node in xn)
            {
                XmlElement xe = (XmlElement)node;//NODE FUN

                if (xe.GetAttribute("Register") == Register)
                {
                    XmlNodeList sxe = xe.ChildNodes;
                    foreach (XmlNode node1 in sxe)
                    {
                        XmlElement xe1 = (XmlElement)node1;//NODE FUN
                        XmlNodeList sxe1 = xe1.ChildNodes;
                        if (xe1.GetAttribute("Mode") == mode)
                        {
                            if (xe1.GetAttribute("Group_Number") == "0")
                            {
                                foreach (XmlNode node2 in sxe1)
                                {
                                    XmlElement xe2 = (XmlElement)node2;//NODE F
                                    value[0] = Convert.ToInt32(xe2.InnerText, 16);
                                }
                            }
                            if (xe1.GetAttribute("Group_Number") == "8")
                            {
                                foreach (XmlNode node2 in sxe1)
                                {
                                    XmlElement xe2 = (XmlElement)node2;//NODE FUN
                                    value[1] = Convert.ToInt32(xe2.InnerText, 16);
                                }
                            }

                            if (xe1.GetAttribute("Group_Number") == "9")
                            {
                                foreach (XmlNode node2 in sxe1)
                                {
                                    XmlElement xe2 = (XmlElement)node2;//NODE FUN
                                    value[2] = Convert.ToInt32(xe2.InnerText, 16);
                                }

                            }
                            if (xe1.GetAttribute("Group_Number") == "10")
                            {
                                foreach (XmlNode node2 in sxe1)
                                {
                                    XmlElement xe2 = (XmlElement)node2;//NODE FUN
                                    value[3] = Convert.ToInt32(xe2.InnerText, 16);
                                }
                            }
                            if (xe1.GetAttribute("Group_Number") == "20")
                            {
                                foreach (XmlNode node2 in sxe1)
                                {
                                    XmlElement xe2 = (XmlElement)node2;//NODE FUN
                                    value[4] = Convert.ToInt32(xe2.InnerText, 16);
                                }
                            }
                            if (xe1.GetAttribute("Group_Number") == "30")
                            {
                                foreach (XmlNode node2 in sxe1)
                                {
                                    XmlElement xe2 = (XmlElement)node2;//NODE FUN
                                    value[5] = Convert.ToInt32(xe2.InnerText, 16);
                                }
                            }
                            if (xe1.GetAttribute("Group_Number") == "11")
                            {
                                foreach (XmlNode node2 in sxe1)
                                {
                                    XmlElement xe2 = (XmlElement)node2;//NODE FUN
                                    value[6] = Convert.ToInt32(xe2.InnerText, 16);
                                }
                            }
                        }
                        else
                        {
                            ;
                        }
                    }
                }


            }

            reader.Close();

            xmlDoc.Save(location);
        }

        public void read_xml_info(ref string location, ref string version, ref string date, ref string name, ref string comment)//open时读取xml里版本号数据
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.IgnoreComments = true;
            XmlReader reader = XmlReader.Create(location, settings);
            xmlDoc.Load(reader);
            XmlNode root = xmlDoc.SelectSingleNode("GPIO_Set");
            XmlNodeList xn = xmlDoc.SelectSingleNode("GPIO_Set").ChildNodes;
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

        public void change_xml_info(ref string location, ref string version, ref string date, ref string name, ref string comment)//修改xml里的版本号数据
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.IgnoreComments = true;
            XmlReader reader = XmlReader.Create(location, settings);
            xmlDoc.Load(reader);
            XmlNode root = xmlDoc.SelectSingleNode("GPIO_Set");
            XmlNodeList xn = xmlDoc.SelectSingleNode("GPIO_Set").ChildNodes;
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
    }
}
