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
namespace gpio_set
{
    public partial class GPIO : Form
    {
        public void Creat_Gpio_Xml(string location)//XML mode for pin64 mcu's GPIO configuration.
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

            root.AppendChild(nxel);
            root.AppendChild(axel);
            root.AppendChild(bxel);
            root.AppendChild(cxel);
            root.AppendChild(dxel);
            root.AppendChild(exel);
            root.AppendChild(fxel);
            root.AppendChild(gxel);


            xmldoc.Save(location);

        }
    }
}