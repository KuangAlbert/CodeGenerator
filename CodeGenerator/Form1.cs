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
using IO;
using irq_set;
using MERGE;
using Microsoft.Office.Interop.Excel;
namespace CodeGenerator
{

    public partial class Form1 : Form
    {

        Io gpio = new Io();
        IRQ irq = new IRQ();
        Path p = new Path();
        public string path_generator = "";//xml文件夹位置
        public string path_generator_excel = "";//excel表格生成xml所在文件夹
        public string path_generator_excel64 = "";//excel表格生成MCU_PIN64.CC所在文件夹
        public string path_generator_excel100 = "";//excel表格生成MCU_PIN.CC所在文件夹
        public string path_generator_pin64 = "";//界面配置.c文件所在文件夹
        public string path_generator_pin100 = "";//界面配置.c文件所在文件夹
        public string path_generator_interrupt64 = "";//界面配置中断文件所在文件夹
        public string path_generator_interrupt100 = "";//界面配置中断文件所在文件夹
        public string path_generator_interrupt = "";//excel表格生成中断配置文件所在文件夹
        public string merge_path = "";

        public string path_generator1;
        public string path_generator_pin641;
        public string path_generator_pin1001;
        public string path_generator_interrupt641;
        public string path_generator_interrupt1001;
        public string path_generator_excel1;
        public string path_generator_interrupt1;
        public string path_generator_excel641;
        public string path_generator_excel1001;

        public string location1;
        public Form1()
        {

            InitializeComponent();
            location1 = System.Environment.CurrentDirectory + "\\xsl_src\\path.xml";
            p.getPathGenertor(location1);
            path_generator1 = p.path_generator1;
            path_generator_excel1 = p.path_generator_excel1;
            path_generator_pin641 = p.path_generator_interrupt641;
            path_generator_pin1001 = p.path_generator_pin1001;
            path_generator_interrupt641 = p.path_generator_interrupt641;
            path_generator_interrupt1001 = p.path_generator_interrupt1001;

        }

        private void 脚ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fd = new FolderBrowserDialog();
            fd.Description = "整个工程将会被建立在以下路径";
            if (!(path_generator1 == "" || path_generator1 == null))
                fd.SelectedPath = path_generator1;
            if (fd.ShowDialog() == DialogResult.OK)
            {
                if (!Directory.Exists(fd.SelectedPath))
                {

                    Directory.CreateDirectory(fd.SelectedPath);
                }
                if (Directory.Exists(fd.SelectedPath))
                {

                    Directory.CreateDirectory(fd.SelectedPath);
                    string path_value = fd.SelectedPath;
                    string pin_path = fd.SelectedPath + "\\Pin64.xml";
                    string irq_path = fd.SelectedPath + "\\IRQ64.xml";
                    gpio.Creat_Gpio_Xml(ref pin_path);
                    irq.Creat_irq_Xml(ref irq_path);
                    Form2 f2 = new Form2();
                    f2.Show();
                    f2.Current_Path(ref path_value);
                    path_generator = fd.SelectedPath;
                    if (!(path_generator == path_generator1))
                    {
                        p.change_node("path_generator", path_generator, location1);
                        p.getPathGenertor(location1);
                        path_generator1 = p.path_generator1;
                    }

                }
            }

        }

        private void 脚ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fd = new FolderBrowserDialog();
            if (!(path_generator1 == ""||path_generator1==null))
                fd.SelectedPath = path_generator1;
            if (fd.ShowDialog() == DialogResult.OK)
            {
                if (!Directory.Exists(fd.SelectedPath))
                {
                    Directory.CreateDirectory(fd.SelectedPath);
                }
                if (Directory.Exists(fd.SelectedPath))
                {
                    Directory.CreateDirectory(fd.SelectedPath);
                    string path_value = fd.SelectedPath;
                    string pin_path = fd.SelectedPath + "\\Pin100.xml";
                    string irq_path = fd.SelectedPath + "\\IRQ100.xml";
                    gpio.Creat_Gpio_Xml(ref pin_path);
                    irq.Creat_irq_Xml(ref irq_path);
                    Form3 f3 = new Form3();
                    f3.Show();
                    f3.Current_Path(ref path_value);
                    path_generator = fd.SelectedPath;
                    if (!(path_generator == path_generator1))
                    {
                        p.change_node("path_generator", path_generator, location1);
                        p.getPathGenertor(location1);
                        path_generator1 = p.path_generator1;
                    }
                }
            }

        }
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fd = new FolderBrowserDialog();
            if (!(path_generator1 == "" || path_generator1 == null))
                fd.SelectedPath = path_generator1;
            fd.Description = "请选择您保存在以下路径的工程";
            string[,] list_value = new string[300, 300];
            for (int i = 0; i < 300; i++)
            {
                for (int j = 0; j < 300; j++)
                {
                    list_value[i, j] = "0";
                }


            }
            string[,] f1irq_list_value = new string[300, 300];

            int[] irqi64 = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            int[] irqi100 = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            if (fd.ShowDialog() == DialogResult.OK)
            {
                if (File.Exists(fd.SelectedPath + "\\Pin64.xml"))
                {
                    Io gpio = new Io();
                    IRQ irq = new IRQ();
                    Form2 f2 = new Form2();
                    string pathopen = "";
                    pathopen = fd.SelectedPath;
                    f2.Show();
                    f2.Current_Path(ref pathopen);
                    string version_value = "", data_value = "", name_value = "", comment_value = "";
                    gpio.read_xml_info(ref f2.pin_path, ref version_value, ref data_value, ref name_value, ref comment_value);
                    irq.read_xml_info(ref f2.irq_path, ref version_value, ref data_value, ref name_value, ref comment_value);
                    textBox1.Text = version_value;
                    textBox4.Text = data_value;
                    textBox2.Text = name_value;
                    textBox3.Text = comment_value;
                    path_generator = fd.SelectedPath;
                    int node_num = 0, k = 0, mid = 0;

                    gpio.read_xml_node("PMC", "ACTIVE", ref f2.Alt_PMC_value_Act, ref f2.pin_path);
                    gpio.read_xml_node("PMC", "RESET", ref f2.Alt_PMC_value_Res, ref f2.pin_path);
                    gpio.read_xml_node("PMC", "STANDBY", ref f2.Alt_PMC_value_Sta, ref f2.pin_path);

                    gpio.read_xml_node("PM", "ACTIVE", ref f2.Alt_PM_value_Act, ref f2.pin_path);
                    gpio.read_xml_node("PM", "RESET", ref f2.Alt_PM_value_Res, ref f2.pin_path);
                    gpio.read_xml_node("PM", "STANDBY", ref f2.Alt_PM_value_Sta, ref f2.pin_path);

                    gpio.read_xml_node("PIBC", "ACTIVE", ref f2.Alt_PIBC_value_Act, ref f2.pin_path);
                    gpio.read_xml_node("PIBC", "RESET", ref f2.Alt_PIBC_value_Res, ref f2.pin_path);
                    gpio.read_xml_node("PIBC", "STANDBY", ref f2.Alt_PIBC_value_Sta, ref f2.pin_path);

                    gpio.read_xml_node("PU", "ACTIVE", ref f2.Alt_PU_value_Act, ref f2.pin_path);
                    gpio.read_xml_node("PU", "RESET", ref f2.Alt_PU_value_Res, ref f2.pin_path);
                    gpio.read_xml_node("PU", "STANDBY", ref f2.Alt_PU_value_Sta, ref f2.pin_path);

                    gpio.read_xml_node("PD", "ACTIVE", ref f2.Alt_PD_value_Act, ref f2.pin_path);
                    gpio.read_xml_node("PD", "RESET", ref f2.Alt_PD_value_Res, ref f2.pin_path);
                    gpio.read_xml_node("PD", "STANDBY", ref f2.Alt_PD_value_Sta, ref f2.pin_path);

                    gpio.read_xml_node("PBDC", "ACTIVE", ref f2.Alt_PBDC_value_Act, ref f2.pin_path);
                    gpio.read_xml_node("PBDC", "RESET", ref f2.Alt_PBDC_value_Res, ref f2.pin_path);
                    gpio.read_xml_node("PBDC", "STANDBY", ref f2.Alt_PBDC_value_Sta, ref f2.pin_path);

                    gpio.read_xml_node("PDSC", "ACTIVE", ref f2.Alt_PDSC_value_Act, ref f2.pin_path);
                    gpio.read_xml_node("PDSC", "RESET", ref f2.Alt_PDSC_value_Res, ref f2.pin_path);
                    gpio.read_xml_node("PDSC", "STANDBY", ref f2.Alt_PDSC_value_Sta, ref f2.pin_path);

                    gpio.read_xml_node("PODC", "ACTIVE", ref f2.Alt_PODC_value_Act, ref f2.pin_path);
                    gpio.read_xml_node("PODC", "RESET", ref f2.Alt_PODC_value_Res, ref f2.pin_path);
                    gpio.read_xml_node("PODC", "STANDBY", ref f2.Alt_PODC_value_Sta, ref f2.pin_path);

                    gpio.read_xml_node("PIPC", "ACTIVE", ref f2.Alt_PIPC_value_Act, ref f2.pin_path);
                    gpio.read_xml_node("PIPC", "RESET", ref f2.Alt_PIPC_value_Res, ref f2.pin_path);
                    gpio.read_xml_node("PIPC", "STANDBY", ref f2.Alt_PIPC_value_Sta, ref f2.pin_path);

                    gpio.read_xml_node("PFCAE", "ACTIVE", ref f2.Alt_PFCAE_value_Act, ref f2.pin_path);
                    gpio.read_xml_node("PFCAE", "RESET", ref f2.Alt_PFCAE_value_Res, ref f2.pin_path);
                    gpio.read_xml_node("PFCAE", "STANDBY", ref f2.Alt_PFCAE_value_Sta, ref f2.pin_path);

                    gpio.read_xml_node("PFCE", "ACTIVE", ref f2.Alt_PFCE_value_Act, ref f2.pin_path);
                    gpio.read_xml_node("PFCE", "RESET", ref f2.Alt_PFCE_value_Res, ref f2.pin_path);
                    gpio.read_xml_node("PFCE", "STANDBY", ref f2.Alt_PFCE_value_Sta, ref f2.pin_path);

                    gpio.read_xml_node("PFC", "ACTIVE", ref f2.Alt_PFC_value_Act, ref f2.pin_path);
                    gpio.read_xml_node("PFC", "RESET", ref f2.Alt_PFC_value_Res, ref f2.pin_path);
                    gpio.read_xml_node("PFC", "STANDBY", ref f2.Alt_PFC_value_Sta, ref f2.pin_path);

                    gpio.read_xml_node("PV", "ACTIVE", ref f2.Alt_Port_value_Act, ref f2.pin_path);
                    gpio.read_xml_node("PV", "RESET", ref f2.Alt_Port_value_Res, ref f2.pin_path);
                    gpio.read_xml_node("PV", "STANDBY", ref f2.Alt_Port_value_Sta, ref f2.pin_path);

                    irq.read_irq_xml_node(ref f2.flag_count, ref f2.irq_path);//name 寄存器



                    XmlDocument ixmlDoc = new XmlDocument();
                    XmlReaderSettings isettings = new XmlReaderSettings();
                    isettings.IgnoreComments = true;
                    XmlReader ireader = XmlReader.Create(f2.irq_path, isettings);
                    ixmlDoc.Load(ireader);
                    XmlNode iroot = ixmlDoc.SelectSingleNode("IRQ_Set");
                    XmlNodeList ixn = ixmlDoc.SelectSingleNode("IRQ_Set").ChildNodes;

                    foreach (XmlNode node in ixn)
                    {
                        XmlElement xe = (XmlElement)node;//NODE FUN

                        if (xe.GetAttribute("Register") == "OS")
                        {
                            XmlNodeList sxe = xe.ChildNodes;

                            foreach (XmlNode node1 in sxe)
                            {
                                XmlElement xe1 = (XmlElement)node1;//NODE FUN

                                if (irqi64[0] < sxe.Count)
                                {
                                    f1irq_list_value[irqi64[0], 0] = xe1.InnerText;
                                    f2.irq_list_value[irqi64[0], 0] = xe1.InnerText;

                                    int keyOS = Array.IndexOf(f2.OSKey.ToArray(), (object)xe1.InnerText);
                                    f2.flagIrq = f2.valueOS.IndexOf(f2.OSValue[keyOS]);
                                    if (f2.flagIrq < 0)
                                    {
                                        f2.valueOS.Add(f2.OSValue[keyOS]);
                                        if (f2.valueListOS == "")
                                            f2.valueListOS = f2.OSValue[keyOS];
                                        else
                                            f2.valueListOS = f2.valueListOS + "、" + f2.OSValue[keyOS];
                                    }
                                    //f2.richTextBox2.Text += "OS Timer " + xe1.InnerText + " interrupt : " + f2.OSValue[keyOS] + " confirm done.\n"; 
                                    f2.textBox12.Text = f2.valueListOS;

                                }
                                f2.item_flag[0] = sxe.Count;

                                irqi64[0]++;
                            }
                        }
                        if (xe.GetAttribute("Register") == "ADC")
                        {
                            XmlNodeList sxe = xe.ChildNodes;

                            foreach (XmlNode node1 in sxe)
                            {
                                XmlElement xe1 = (XmlElement)node1;//NODE FUN

                                if (irqi64[1] < sxe.Count)
                                {
                                    f1irq_list_value[irqi64[1], 1] = xe1.InnerText;
                                    f2.irq_list_value[irqi64[1], 1] = xe1.InnerText;

                                    int keyADC = Array.IndexOf(f2.ADCKey.ToArray(), (object)xe1.InnerText);
                                    f2.flagIrq = f2.valueADC.IndexOf(f2.ADCValue[keyADC]);
                                    if (f2.flagIrq < 0)
                                    {
                                        f2.valueADC.Add(f2.ADCValue[keyADC]);
                                        if (f2.valueListADC == "")
                                            f2.valueListADC = f2.ADCValue[keyADC];
                                        else
                                            f2.valueListADC = f2.valueListADC + "、" + f2.ADCValue[keyADC];
                                    }
                                    //f2.richTextBox2.Text += "ADC " + xe1.InnerText + " interrupt : " + f2.ADCValue[keyADC] + " confirm done.\n"; 
                                    f2.textBox13.Text = f2.valueListADC;

                                }
                                f2.item_flag[1] = sxe.Count;

                                irqi64[1]++;
                            }
                        }
                        if (xe.GetAttribute("Register") == "CAN")
                        {
                            XmlNodeList sxe = xe.ChildNodes;

                            foreach (XmlNode node1 in sxe)
                            {
                                XmlElement xe1 = (XmlElement)node1;//NODE FUN

                                if (irqi64[2] < sxe.Count)
                                {
                                    f1irq_list_value[irqi64[2], 2] = xe1.InnerText;
                                    f2.irq_list_value[irqi64[2], 2] = xe1.InnerText;

                                    int keyCAN = Array.IndexOf(f2.CANKey.ToArray(), (object)xe1.InnerText);
                                    f2.flagIrq = f2.valueCAN.IndexOf(f2.CANValue[keyCAN, 4]);
                                    if (f2.flagIrq < 0)
                                    {
                                        f2.valueCAN.Add(f2.CANValue[keyCAN, 0]); f2.valueCAN.Add(f2.CANValue[keyCAN, 1]); f2.valueCAN.Add(f2.CANValue[keyCAN, 2]); f2.valueCAN.Add(f2.CANValue[keyCAN, 3]); f2.valueCAN.Add(f2.CANValue[keyCAN, 4]);
                                        if (f2.valueListCAN == "")
                                            f2.valueListCAN = f2.CANValue[keyCAN, 0] + "、" + f2.CANValue[keyCAN, 1] + "、" + f2.CANValue[keyCAN, 2] + "、" + f2.CANValue[keyCAN, 3] + "、" + f2.CANValue[keyCAN, 4];
                                        else
                                            f2.valueListCAN = f2.valueListCAN + "、" + f2.CANValue[keyCAN, 3] + "、" + f2.CANValue[keyCAN, 4];
                                    }
                                    //f2.richTextBox2.Text += "CAN " + xe1.InnerText + " interrupt : " + f2.CANValue[keyCAN, 0] + "、" + f2.CANValue[keyCAN, 1] + "、" + f2.CANValue[keyCAN, 2] + "、" + f2.CANValue[keyCAN, 3] + "、" + f2.CANValue[keyCAN, 4] + " confirm done.\n"; 
                                    f2.textBox15.Text = f2.valueListCAN;

                                }
                                f2.item_flag[2] = sxe.Count;

                                irqi64[2]++;
                            }
                        }
                        if (xe.GetAttribute("Register") == "IIC")
                        {
                            XmlNodeList sxe = xe.ChildNodes;

                            foreach (XmlNode node1 in sxe)
                            {
                                XmlElement xe1 = (XmlElement)node1;//NODE FUN

                                if (irqi64[3] < sxe.Count)
                                {
                                    f1irq_list_value[irqi64[3], 3] = xe1.InnerText;
                                    f2.irq_list_value[irqi64[3], 3] = xe1.InnerText;

                                    int keyIIC = Array.IndexOf(f2.IICKey.ToArray(), (object)xe1.InnerText);
                                    f2.flagIrq = f2.valueIIC.IndexOf(f2.IICValue[keyIIC]);
                                    if (f2.flagIrq < 0)
                                    {
                                        f2.valueIIC.Add(f2.IICValue[keyIIC]); f2.valueIIC.Add(f2.IICValue[keyIIC + 1]); f2.valueIIC.Add(f2.IICValue[keyIIC + 1]); f2.valueIIC.Add(f2.IICValue[keyIIC + 2]); f2.valueIIC.Add(f2.IICValue[keyIIC + 3]);
                                        if (f2.valueListIIC == "")
                                            f2.valueListIIC = f2.IICValue[keyIIC] + "、" + f2.IICValue[keyIIC + 1] + "、" + f2.IICValue[keyIIC + 2] + "、" + f2.IICValue[keyIIC + 3];
                                        else
                                            f2.valueListIIC = f2.valueListIIC + " 、 " + f2.IICValue[keyIIC] + "、" + f2.IICValue[keyIIC + 1] + "、" + f2.IICValue[keyIIC + 2] + "、" + f2.IICValue[keyIIC + 3];
                                    }
                                    //f2.richTextBox2.Text += "IIC " + xe1.InnerText + " interrupt : " + f2.IICValue[keyIIC] + "、" + f2.IICValue[keyIIC + 1] + "、" + f2.IICValue[keyIIC + 2] + "、" + f2.IICValue[keyIIC + 3] + " confirm done.\n"; 
                                    f2.textBox20.Text = f2.valueListIIC;

                                }
                                f2.item_flag[3] = sxe.Count;

                                irqi64[3]++;
                            }
                        }
                        if (xe.GetAttribute("Register") == "PWM")
                        {
                            XmlNodeList sxe = xe.ChildNodes;

                            foreach (XmlNode node1 in sxe)
                            {
                                XmlElement xe1 = (XmlElement)node1;//NODE FUN

                                if (irqi64[4] < sxe.Count)
                                {
                                    f1irq_list_value[irqi64[4], 4] = xe1.InnerText;
                                    f2.irq_list_value[irqi64[4], 4] = xe1.InnerText;

                                    int keyPWM = Array.IndexOf(f2.PWMKey.ToArray(), (object)xe1.InnerText);
                                    f2.flagIrq = f2.valuePWM.IndexOf(f2.PWMValue[keyPWM]);
                                    if (f2.flagIrq < 0)
                                    {
                                        f2.valuePWM.Add(f2.PWMValue[keyPWM]);
                                        if (f2.valueListPWM == "")
                                            f2.valueListPWM = f2.PWMValue[keyPWM];
                                        else
                                            f2.valueListPWM = f2.valueListPWM + "、" + f2.PWMValue[keyPWM];
                                    }
                                    //f2.richTextBox2.Text += "PWM " + xe1.InnerText + " interrupt : " + f2.PWMValue[keyPWM] + " confirm done.\n"; 
                                    f2.textBox18.Text = f2.valueListPWM;

                                }
                                f2.item_flag[4] = sxe.Count;

                                irqi64[4]++;
                            }
                        }
                        if (xe.GetAttribute("Register") == "DMA")
                        {
                            XmlNodeList sxe = xe.ChildNodes;

                            foreach (XmlNode node1 in sxe)
                            {
                                XmlElement xe1 = (XmlElement)node1;//NODE FUN

                                if (irqi64[5] < sxe.Count)
                                {
                                    f1irq_list_value[irqi64[5], 5] = xe1.InnerText;
                                    f2.irq_list_value[irqi64[5], 5] = xe1.InnerText;

                                    int keyDMA = Array.IndexOf(f2.DMAKey.ToArray(), (object)xe1.InnerText);
                                    f2.flagIrq = f2.valueDMA.IndexOf(f2.DMAValue[keyDMA]);
                                    if (f2.flagIrq < 0)
                                    {
                                        f2.valueDMA.Add(f2.DMAValue[keyDMA]);
                                        if (f2.valueListDMA == "")
                                            f2.valueListDMA = f2.DMAValue[keyDMA];
                                        else
                                            f2.valueListDMA = f2.valueListDMA + "、" + f2.DMAValue[keyDMA];
                                    }
                                    //f2.richTextBox2.Text += "DMA " + xe1.InnerText + " interrupt : " + f2.DMAValue[keyDMA] + " confirm done.\n"; 
                                    f2.textBox19.Text = f2.valueListDMA;

                                }
                                f2.item_flag[5] = sxe.Count;

                                irqi64[5]++;
                            }
                        }
                        if (xe.GetAttribute("Register") == "EX")
                        {
                            XmlNodeList sxe = xe.ChildNodes;

                            foreach (XmlNode node1 in sxe)
                            {
                                XmlElement xe1 = (XmlElement)node1;//NODE FUN

                                if (irqi64[6] < sxe.Count)
                                {
                                    f1irq_list_value[irqi64[6], 6] = xe1.InnerText;
                                    f2.irq_list_value[irqi64[6], 6] = xe1.InnerText;

                                    int keyEX = Array.IndexOf(f2.EXKey.ToArray(), (object)xe1.InnerText);
                                    f2.flagIrq = f2.valueEX.IndexOf(f2.EXValue[keyEX]);
                                    if (f2.flagIrq < 0)
                                    {
                                        f2.valueEX.Add(f2.EXValue[keyEX]);
                                        if (f2.valueListEX == "")
                                            f2.valueListEX = f2.EXValue[keyEX];
                                        else
                                            f2.valueListEX = f2.valueListEX + " 、 " + f2.EXValue[keyEX];
                                    }
                                    //f2.richTextBox2.Text += "Extern " + xe1.InnerText + " interrupt : " + f2.EXValue[keyEX] + " confirm done.\n"; 
                                    f2.textBox21.Text = f2.valueListEX;

                                }
                                f2.item_flag[6] = sxe.Count;

                                irqi64[6]++;
                            }
                        }
                        if (xe.GetAttribute("Register") == "TAU")
                        {
                            XmlNodeList sxe = xe.ChildNodes;

                            foreach (XmlNode node1 in sxe)
                            {
                                XmlElement xe1 = (XmlElement)node1;//NODE FUN

                                if (irqi64[10] < sxe.Count)
                                {
                                    f1irq_list_value[irqi64[7], 7] = xe1.InnerText;
                                    f2.irq_list_value[irqi64[7], 7] = xe1.InnerText;

                                    int keyTAU = Array.IndexOf(f2.TAUKey.ToArray(), (object)xe1.InnerText);
                                    f2.flagIrq = f2.valueTAU.IndexOf(f2.TAUValue[keyTAU]);
                                    if (f2.flagIrq < 0)
                                    {
                                        f2.valueTAU.Add(f2.TAUValue[keyTAU]);
                                        if (f2.valueListTAU == "")
                                            f2.valueListTAU = f2.TAUValue[keyTAU];
                                        else
                                            f2.valueListTAU = f2.valueListTAU + "、" + f2.TAUValue[keyTAU];
                                    }
                                    //f2.richTextBox2.Text += "Time array uint " + xe1.InnerText + " interrupt : " + f2.TAUValue[keyTAU] + " confirm done.\n"; 
                                    f2.textBox22.Text = f2.valueListTAU;
                                }
                                f2.item_flag[7] = sxe.Count;

                                irqi64[7]++;
                            }
                        }
                        if (xe.GetAttribute("Register") == "UART")
                        {
                            XmlNodeList sxe = xe.ChildNodes;

                            foreach (XmlNode node1 in sxe)
                            {
                                XmlElement xe1 = (XmlElement)node1;//NODE FUN

                                if (irqi64[8] < sxe.Count)
                                {
                                    f1irq_list_value[irqi64[8], 8] = xe1.InnerText;
                                    f2.irq_list_value[irqi64[8], 8] = xe1.InnerText;

                                    int keyUART = Array.IndexOf(f2.UARTKey.ToArray(), (object)xe1.InnerText);
                                    f2.flagIrq = f2.valueUART.IndexOf(f2.UARTValue[keyUART, 0]);
                                    if (f2.flagIrq < 0)
                                    {
                                        f2.valueUART.Add(f2.UARTValue[keyUART, 0]); f2.valueUART.Add(f2.UARTValue[keyUART, 1]); f2.valueUART.Add(f2.UARTValue[keyUART, 1]); f2.valueUART.Add(f2.UARTValue[keyUART, 2]); f2.valueUART.Add(f2.UARTValue[keyUART, 3]);
                                        if (f2.valueListUART == "")
                                            f2.valueListUART = f2.UARTValue[keyUART, 0] + "、" + f2.UARTValue[keyUART, 1] + "、" + f2.UARTValue[keyUART, 2] + "、" + f2.UARTValue[keyUART, 3];
                                        else
                                            f2.valueListUART = f2.valueListUART + "、" + f2.UARTValue[keyUART, 0] + "、" + f2.UARTValue[keyUART, 1] + "、" + f2.UARTValue[keyUART, 2] + "、" + f2.UARTValue[keyUART, 3];
                                    }
                                    //f2.richTextBox2.Text += "UART " + xe1.InnerText + " interrupt : " + f2.UARTValue[keyUART, 0] + "、" + f2.UARTValue[keyUART, 1] + "、" + f2.UARTValue[keyUART, 2] + "、" + f2.UARTValue[keyUART, 3] + " confirm done.\n"; 
                                    f2.textBox17.Text = f2.valueListUART;

                                }
                                f2.item_flag[8] = sxe.Count;

                                irqi64[8]++;
                            }
                        }
                        if (xe.GetAttribute("Register") == "SPI")
                        {
                            XmlNodeList sxe = xe.ChildNodes;

                            foreach (XmlNode node1 in sxe)
                            {
                                XmlElement xe1 = (XmlElement)node1;//NODE FUN

                                if (irqi64[10] < sxe.Count)
                                {
                                    f1irq_list_value[irqi64[9], 9] = xe1.InnerText;
                                    f2.irq_list_value[irqi64[9], 9] = xe1.InnerText;

                                    int keySPI = Array.IndexOf(f2.SPIKey.ToArray(), (object)xe1.InnerText);
                                    f2.flagIrq = f2.valueSPI.IndexOf(f2.SPIValue[keySPI, 0]);
                                    if (f2.flagIrq < 0)
                                    {
                                        f2.valueSPI.Add(f2.SPIValue[keySPI, 0]); f2.valueSPI.Add(f2.SPIValue[keySPI, 1]); f2.valueSPI.Add(f2.SPIValue[keySPI, 2]);
                                        if (f2.valueListSPI == "")
                                            f2.valueListSPI = f2.SPIValue[keySPI, 0] + "、" + f2.SPIValue[keySPI, 1] + "、" + f2.SPIValue[keySPI, 2];
                                        else
                                            f2.valueListSPI = f2.valueListSPI + "、" + f2.SPIValue[keySPI, 0] + "、" + f2.SPIValue[keySPI, 1] + "、" + f2.SPIValue[keySPI, 2];
                                    }
                                    //f2.richTextBox2.Text += "SPI " + xe1.InnerText + " interrupt : " + f2.SPIValue[keySPI, 0] + "、" + f2.SPIValue[keySPI, 1] + "、" + f2.SPIValue[keySPI, 2] + " confirm done.\n"; 
                                    f2.textBox16.Text = f2.valueListSPI;

                                }
                                f2.item_flag[9] = sxe.Count;

                                irqi64[9]++;
                            }
                        }
                        if (xe.GetAttribute("Register") == "WD")
                        {
                            XmlNodeList sxe = xe.ChildNodes;
                            foreach (XmlNode node1 in sxe)
                            {
                                XmlElement xe1 = (XmlElement)node1;//NODE FUN

                                if (irqi64[10] < sxe.Count)
                                {
                                    f1irq_list_value[irqi64[10], 10] = xe1.InnerText;
                                    f2.irq_list_value[irqi64[10], 10] = xe1.InnerText;

                                    int keyWD = Array.IndexOf(f2.WDKey.ToArray(), (object)xe1.InnerText);
                                    f2.flagIrq = f2.valueWD.IndexOf(f2.WDValue[keyWD]);
                                    if (f2.flagIrq < 0)
                                    {
                                        f2.valueWD.Add(f2.WDValue[keyWD]);
                                        if (f2.valueListWD == "")
                                            f2.valueListWD = f2.WDValue[keyWD];
                                        else
                                            f2.valueListWD = f2.valueListWD + "、" + f2.WDValue[keyWD];
                                    }
                                    //f2.richTextBox2.Text += "WD " + xe1.InnerText + " interrupt : " + f2.WDValue[keyWD] + " confirm done.\n"; 
                                    f2.textBox14.Text = f2.valueListWD;

                                }
                                f2.item_flag[10] = sxe.Count;

                                irqi64[10]++;
                            }
                        }
                    }
                    mid = irqi64[0];
                    for (int midi = 1; midi < 11; midi++)
                    {
                        if (irqi64[midi] > mid)
                        {
                            mid = irqi64[midi];
                        }
                    }
                    ireader.Close();
                    ixmlDoc.Save(f2.irq_path);
                    f2.irq_return_list(f1irq_list_value, mid);




                    XmlDocument xmlDoc = new XmlDocument();
                    XmlReaderSettings settings = new XmlReaderSettings();
                    settings.IgnoreComments = true;
                    XmlReader reader = XmlReader.Create(f2.pin_path, settings);
                    xmlDoc.Load(reader);
                    XmlNode root = xmlDoc.SelectSingleNode("GPIO_Set");
                    XmlNodeList xn = xmlDoc.SelectSingleNode("GPIO_Set").ChildNodes;


                    foreach (XmlNode node in xn)
                    {
                        XmlElement xe = (XmlElement)node;//NODE FUN

                        if (xe.GetAttribute("Item") == "LPORT")
                        {
                            XmlNodeList sxe = xe.ChildNodes;
                            foreach (XmlNode node1 in sxe)
                            {
                                XmlElement xe1 = (XmlElement)node1;
                                node_num = Convert.ToInt32(xe1.GetAttribute("cnt"));

                            }
                        }
                    }
                    for (int j = 1; j < node_num + 1; j++)
                    {

                        foreach (XmlNode node in xn)
                        {
                            XmlElement xe = (XmlElement)node;//NODE FUN

                            if (node.Name == "LIST")
                            {
                                foreach (XmlNode node1 in xe)
                                {
                                    XmlElement xe1 = (XmlElement)node1;//NODE FUN
                                    if ((xe1.GetAttribute("cnt") == j.ToString()) && k < 12)
                                    {

                                        list_value[j, k] = xe1.InnerText;
                                        k++;
                                    }
                                }

                            }

                        }
                        k = 0;


                    }
                    reader.Close();
                    xmlDoc.Save(f2.pin_path);
                    f2.return_list(list_value, node_num + 1);
                }


                if (File.Exists(fd.SelectedPath + "\\Pin100.xml"))
                {
                    Io gpio = new Io();
                    IRQ irq = new IRQ();
                    Form3 f3 = new Form3();
                    string pathopen = "";
                    pathopen = fd.SelectedPath;
                    f3.Show();
                    f3.Current_Path(ref pathopen);
                    string version_value = "", data_value = "", name_value = "", comment_value = "";
                    gpio.read_xml_info(ref f3.pin_path, ref version_value, ref data_value, ref name_value, ref comment_value);
                    irq.read_xml_info(ref f3.irq_path, ref version_value, ref data_value, ref name_value, ref comment_value);
                    textBox1.Text = version_value;
                    textBox2.Text = data_value;
                    textBox3.Text = name_value;
                    textBox4.Text = comment_value;
                    path_generator = fd.SelectedPath;
                    int node_num = 0, k = 0, mid = 0;
                    gpio.read_xml_node("PMC", "ACTIVE", ref f3.Alt_PMC_value_Act, ref f3.pin_path);
                    gpio.read_xml_node("PMC", "RESET", ref f3.Alt_PMC_value_Res, ref f3.pin_path);
                    gpio.read_xml_node("PMC", "STANDBY", ref f3.Alt_PMC_value_Sta, ref f3.pin_path);

                    gpio.read_xml_node("PM", "ACTIVE", ref f3.Alt_PM_value_Act, ref f3.pin_path);
                    gpio.read_xml_node("PM", "RESET", ref f3.Alt_PM_value_Res, ref f3.pin_path);
                    gpio.read_xml_node("PM", "STANDBY", ref f3.Alt_PM_value_Sta, ref f3.pin_path);

                    gpio.read_xml_node("PIBC", "ACTIVE", ref f3.Alt_PIBC_value_Act, ref f3.pin_path);
                    gpio.read_xml_node("PIBC", "RESET", ref f3.Alt_PIBC_value_Res, ref f3.pin_path);
                    gpio.read_xml_node("PIBC", "STANDBY", ref f3.Alt_PIBC_value_Sta, ref f3.pin_path);

                    gpio.read_xml_node("PU", "ACTIVE", ref f3.Alt_PU_value_Act, ref f3.pin_path);
                    gpio.read_xml_node("PU", "RESET", ref f3.Alt_PU_value_Res, ref f3.pin_path);
                    gpio.read_xml_node("PU", "STANDBY", ref f3.Alt_PU_value_Sta, ref f3.pin_path);

                    gpio.read_xml_node("PD", "ACTIVE", ref f3.Alt_PD_value_Act, ref f3.pin_path);
                    gpio.read_xml_node("PD", "RESET", ref f3.Alt_PD_value_Res, ref f3.pin_path);
                    gpio.read_xml_node("PD", "STANDBY", ref f3.Alt_PD_value_Sta, ref f3.pin_path);

                    gpio.read_xml_node("PBDC", "ACTIVE", ref f3.Alt_PBDC_value_Act, ref f3.pin_path);
                    gpio.read_xml_node("PBDC", "RESET", ref f3.Alt_PBDC_value_Res, ref f3.pin_path);
                    gpio.read_xml_node("PBDC", "STANDBY", ref f3.Alt_PBDC_value_Sta, ref f3.pin_path);

                    gpio.read_xml_node("PDSC", "ACTIVE", ref f3.Alt_PDSC_value_Act, ref f3.pin_path);
                    gpio.read_xml_node("PDSC", "RESET", ref f3.Alt_PDSC_value_Res, ref f3.pin_path);
                    gpio.read_xml_node("PDSC", "STANDBY", ref f3.Alt_PDSC_value_Sta, ref f3.pin_path);

                    gpio.read_xml_node("PODC", "ACTIVE", ref f3.Alt_PODC_value_Act, ref f3.pin_path);
                    gpio.read_xml_node("PODC", "RESET", ref f3.Alt_PODC_value_Res, ref f3.pin_path);
                    gpio.read_xml_node("PODC", "STANDBY", ref f3.Alt_PODC_value_Sta, ref f3.pin_path);

                    gpio.read_xml_node("PIPC", "ACTIVE", ref f3.Alt_PIPC_value_Act, ref f3.pin_path);
                    gpio.read_xml_node("PIPC", "RESET", ref f3.Alt_PIPC_value_Res, ref f3.pin_path);
                    gpio.read_xml_node("PIPC", "STANDBY", ref f3.Alt_PIPC_value_Sta, ref f3.pin_path);

                    gpio.read_xml_node("PFCAE", "ACTIVE", ref f3.Alt_PFCAE_value_Act, ref f3.pin_path);
                    gpio.read_xml_node("PFCAE", "RESET", ref f3.Alt_PFCAE_value_Res, ref f3.pin_path);
                    gpio.read_xml_node("PFCAE", "STANDBY", ref f3.Alt_PFCAE_value_Sta, ref f3.pin_path);

                    gpio.read_xml_node("PFCE", "ACTIVE", ref f3.Alt_PFCE_value_Act, ref f3.pin_path);
                    gpio.read_xml_node("PFCE", "RESET", ref f3.Alt_PFCE_value_Res, ref f3.pin_path);
                    gpio.read_xml_node("PFCE", "STANDBY", ref f3.Alt_PFCE_value_Sta, ref f3.pin_path);

                    gpio.read_xml_node("PFC", "ACTIVE", ref f3.Alt_PFC_value_Act, ref f3.pin_path);
                    gpio.read_xml_node("PFC", "RESET", ref f3.Alt_PFC_value_Res, ref f3.pin_path);
                    gpio.read_xml_node("PFC", "STANDBY", ref f3.Alt_PFC_value_Sta, ref f3.pin_path);
                    irq.read_irq_xml_node(ref f3.flag_count, ref f3.irq_path);//name 寄存器



                    XmlDocument ixmlDoc = new XmlDocument();
                    XmlReaderSettings isettings = new XmlReaderSettings();
                    isettings.IgnoreComments = true;
                    XmlReader ireader = XmlReader.Create(f3.irq_path, isettings);
                    ixmlDoc.Load(ireader);
                    XmlNode iroot = ixmlDoc.SelectSingleNode("IRQ_Set");
                    XmlNodeList ixn = ixmlDoc.SelectSingleNode("IRQ_Set").ChildNodes;

                    foreach (XmlNode node in ixn)
                    {
                        XmlElement xe = (XmlElement)node;//NODE FUN

                        if (xe.GetAttribute("Register") == "OS")
                        {
                            XmlNodeList sxe = xe.ChildNodes;

                            foreach (XmlNode node1 in sxe)
                            {
                                XmlElement xe1 = (XmlElement)node1;//NODE FUN

                                if (irqi100[0] < sxe.Count)
                                {
                                    f1irq_list_value[irqi100[0], 0] = xe1.InnerText;
                                    f3.irq_list_value[irqi100[0], 0] = xe1.InnerText;

                                    int keyOS = Array.IndexOf(f3.OSKey.ToArray(), (object)xe1.InnerText);
                                    f3.flagIrq = f3.valueOS.IndexOf(f3.OSValue[keyOS]);
                                    if (f3.flagIrq < 0)
                                    {
                                        f3.valueOS.Add(f3.OSValue[keyOS]);
                                        if (f3.valueListOS == "")
                                            f3.valueListOS = f3.OSValue[keyOS];
                                        else
                                            f3.valueListOS = f3.valueListOS + "、" + f3.OSValue[keyOS];
                                    }
                                    // f3.richTextBox2.Text += "OS Timer " + xe1.InnerText + " interrupt : " + f3.OSValue[keyOS] + " confirm done.\n"; 
                                    f3.textBox12.Text = f3.valueListOS;

                                }
                                f3.item_flag[0] = sxe.Count;

                                irqi100[0]++;
                            }
                        }
                        if (xe.GetAttribute("Register") == "ADC")
                        {
                            XmlNodeList sxe = xe.ChildNodes;

                            foreach (XmlNode node1 in sxe)
                            {
                                XmlElement xe1 = (XmlElement)node1;//NODE FUN

                                if (irqi100[1] < sxe.Count)
                                {
                                    f1irq_list_value[irqi100[1], 1] = xe1.InnerText;
                                    f3.irq_list_value[irqi100[1], 1] = xe1.InnerText;

                                    int keyADC = Array.IndexOf(f3.ADCKey.ToArray(), (object)xe1.InnerText);
                                    f3.flagIrq = f3.valueADC.IndexOf(f3.ADCValue[keyADC]);
                                    if (f3.flagIrq < 0)
                                    {
                                        f3.valueADC.Add(f3.ADCValue[keyADC]);
                                        if (f3.valueListADC == "")
                                            f3.valueListADC = f3.ADCValue[keyADC];
                                        else
                                            f3.valueListADC = f3.valueListADC + "、" + f3.ADCValue[keyADC];
                                    }
                                    // f3.richTextBox2.Text += "ADC " + xe1.InnerText + " interrupt : " + f3.ADCValue[keyADC] + " confirm done.\n"; 
                                    f3.textBox13.Text = f3.valueListADC;

                                }
                                f3.item_flag[1] = sxe.Count;

                                irqi100[1]++;
                            }
                        }
                        if (xe.GetAttribute("Register") == "CAN")
                        {
                            XmlNodeList sxe = xe.ChildNodes;

                            foreach (XmlNode node1 in sxe)
                            {
                                XmlElement xe1 = (XmlElement)node1;//NODE FUN

                                if (irqi100[2] < sxe.Count)
                                {
                                    f1irq_list_value[irqi100[2], 2] = xe1.InnerText;
                                    f3.irq_list_value[irqi100[2], 2] = xe1.InnerText;

                                    int keyCAN = Array.IndexOf(f3.CANKey.ToArray(), (object)xe1.InnerText);
                                    f3.flagIrq = f3.valueCAN.IndexOf(f3.CANValue[keyCAN, 4]);
                                    if (f3.flagIrq < 0)
                                    {
                                        f3.valueCAN.Add(f3.CANValue[keyCAN, 0]); f3.valueCAN.Add(f3.CANValue[keyCAN, 1]); f3.valueCAN.Add(f3.CANValue[keyCAN, 2]); f3.valueCAN.Add(f3.CANValue[keyCAN, 3]); f3.valueCAN.Add(f3.CANValue[keyCAN, 4]);
                                        if (f3.valueListCAN == "")
                                            f3.valueListCAN = f3.CANValue[keyCAN, 0] + "、" + f3.CANValue[keyCAN, 1] + "、" + f3.CANValue[keyCAN, 2] + "、" + f3.CANValue[keyCAN, 3] + "、" + f3.CANValue[keyCAN, 4];
                                        else
                                            f3.valueListCAN = f3.valueListCAN + "、" + f3.CANValue[keyCAN, 3] + "、" + f3.CANValue[keyCAN, 4];
                                    }
                                    // f3.richTextBox2.Text += "CAN " + xe1.InnerText + " interrupt : " + f3.CANValue[keyCAN, 0] + "、" + f3.CANValue[keyCAN, 1] + "、" + f3.CANValue[keyCAN, 2] + "、" + f3.CANValue[keyCAN, 3] + "、" + f3.CANValue[keyCAN, 4] + " confirm done.\n"; 
                                    f3.textBox15.Text = f3.valueListCAN;

                                }
                                f3.item_flag[2] = sxe.Count;

                                irqi100[2]++;
                            }
                        }
                        if (xe.GetAttribute("Register") == "IIC")
                        {
                            XmlNodeList sxe = xe.ChildNodes;

                            foreach (XmlNode node1 in sxe)
                            {
                                XmlElement xe1 = (XmlElement)node1;//NODE FUN

                                if (irqi100[3] < sxe.Count)
                                {
                                    f1irq_list_value[irqi100[3], 3] = xe1.InnerText;
                                    f3.irq_list_value[irqi100[3], 3] = xe1.InnerText;

                                    int keyIIC = Array.IndexOf(f3.IICKey.ToArray(), (object)xe1.InnerText);
                                    f3.flagIrq = f3.valueIIC.IndexOf(f3.IICValue[keyIIC]);
                                    if (f3.flagIrq < 0)
                                    {
                                        f3.valueIIC.Add(f3.IICValue[keyIIC]); f3.valueIIC.Add(f3.IICValue[keyIIC + 1]); f3.valueIIC.Add(f3.IICValue[keyIIC + 1]); f3.valueIIC.Add(f3.IICValue[keyIIC + 2]); f3.valueIIC.Add(f3.IICValue[keyIIC + 3]);
                                        if (f3.valueListIIC == "")
                                            f3.valueListIIC = f3.IICValue[keyIIC] + "、" + f3.IICValue[keyIIC + 1] + "、" + f3.IICValue[keyIIC + 2] + "、" + f3.IICValue[keyIIC + 3];
                                        else
                                            f3.valueListIIC = f3.valueListIIC + " 、 " + f3.IICValue[keyIIC] + "、" + f3.IICValue[keyIIC + 1] + "、" + f3.IICValue[keyIIC + 2] + "、" + f3.IICValue[keyIIC + 3];
                                    }
                                    //  f3.richTextBox2.Text += "IIC " + xe1.InnerText + " interrupt : " + f3.IICValue[keyIIC] + "、" + f3.IICValue[keyIIC + 1] + "、" + f3.IICValue[keyIIC + 2] + "、" + f3.IICValue[keyIIC + 3] + " confirm done.\n"; 
                                    f3.textBox20.Text = f3.valueListIIC;

                                }
                                f3.item_flag[3] = sxe.Count;

                                irqi100[3]++;
                            }
                        }
                        if (xe.GetAttribute("Register") == "PWM")
                        {
                            XmlNodeList sxe = xe.ChildNodes;

                            foreach (XmlNode node1 in sxe)
                            {
                                XmlElement xe1 = (XmlElement)node1;//NODE FUN

                                if (irqi100[4] < sxe.Count)
                                {
                                    f1irq_list_value[irqi100[4], 4] = xe1.InnerText;
                                    f3.irq_list_value[irqi100[4], 4] = xe1.InnerText;

                                    int keyPWM = Array.IndexOf(f3.PWMKey.ToArray(), (object)xe1.InnerText);
                                    f3.flagIrq = f3.valuePWM.IndexOf(f3.PWMValue[keyPWM]);
                                    if (f3.flagIrq < 0)
                                    {
                                        f3.valuePWM.Add(f3.PWMValue[keyPWM]);
                                        if (f3.valueListPWM == "")
                                            f3.valueListPWM = f3.PWMValue[keyPWM];
                                        else
                                            f3.valueListPWM = f3.valueListPWM + "、" + f3.PWMValue[keyPWM];
                                    }
                                    // f3.richTextBox2.Text += "PWM " + xe1.InnerText + " interrupt : " + f3.PWMValue[keyPWM] + " confirm done.\n"; 
                                    f3.textBox18.Text = f3.valueListPWM;

                                }
                                f3.item_flag[4] = sxe.Count;

                                irqi100[4]++;
                            }
                        }
                        if (xe.GetAttribute("Register") == "DMA")
                        {
                            XmlNodeList sxe = xe.ChildNodes;

                            foreach (XmlNode node1 in sxe)
                            {
                                XmlElement xe1 = (XmlElement)node1;//NODE FUN

                                if (irqi100[5] < sxe.Count)
                                {
                                    f1irq_list_value[irqi100[5], 5] = xe1.InnerText;
                                    f3.irq_list_value[irqi100[5], 5] = xe1.InnerText;

                                    int keyDMA = Array.IndexOf(f3.DMAKey.ToArray(), (object)xe1.InnerText);
                                    f3.flagIrq = f3.valueDMA.IndexOf(f3.DMAValue[keyDMA]);
                                    if (f3.flagIrq < 0)
                                    {
                                        f3.valueDMA.Add(f3.DMAValue[keyDMA]);
                                        if (f3.valueListDMA == "")
                                            f3.valueListDMA = f3.DMAValue[keyDMA];
                                        else
                                            f3.valueListDMA = f3.valueListDMA + "、" + f3.DMAValue[keyDMA];
                                    }
                                    // f3.richTextBox2.Text += "DMA " + xe1.InnerText + " interrupt : " + f3.DMAValue[keyDMA] + " confirm done.\n"; 
                                    f3.textBox19.Text = f3.valueListDMA;

                                }
                                f3.item_flag[5] = sxe.Count;

                                irqi100[5]++;
                            }
                        }
                        if (xe.GetAttribute("Register") == "EX")
                        {
                            XmlNodeList sxe = xe.ChildNodes;

                            foreach (XmlNode node1 in sxe)
                            {
                                XmlElement xe1 = (XmlElement)node1;//NODE FUN

                                if (irqi100[6] < sxe.Count)
                                {
                                    f1irq_list_value[irqi100[6], 6] = xe1.InnerText;
                                    f3.irq_list_value[irqi100[6], 6] = xe1.InnerText;

                                    int keyEX = Array.IndexOf(f3.EXKey.ToArray(), (object)xe1.InnerText);
                                    f3.flagIrq = f3.valueEX.IndexOf(f3.EXValue[keyEX]);
                                    if (f3.flagIrq < 0)
                                    {
                                        f3.valueEX.Add(f3.EXValue[keyEX]);
                                        if (f3.valueListEX == "")
                                            f3.valueListEX = f3.EXValue[keyEX];
                                        else
                                            f3.valueListEX = f3.valueListEX + " 、 " + f3.EXValue[keyEX];
                                    }
                                    //f3.richTextBox2.Text += "Extern " + xe1.InnerText + " interrupt : " + f3.EXValue[keyEX] + " confirm done.\n"; 
                                    f3.textBox21.Text = f3.valueListEX;

                                }
                                f3.item_flag[6] = sxe.Count;

                                irqi100[6]++;
                            }
                        }
                        if (xe.GetAttribute("Register") == "TAU")
                        {
                            XmlNodeList sxe = xe.ChildNodes;

                            foreach (XmlNode node1 in sxe)
                            {
                                XmlElement xe1 = (XmlElement)node1;//NODE FUN

                                if (irqi100[10] < sxe.Count)
                                {
                                    f1irq_list_value[irqi100[7], 7] = xe1.InnerText;
                                    f3.irq_list_value[irqi100[7], 7] = xe1.InnerText;

                                    int keyTAU = Array.IndexOf(f3.TAUKey.ToArray(), (object)xe1.InnerText);
                                    f3.flagIrq = f3.valueTAU.IndexOf(f3.TAUValue[keyTAU]);
                                    if (f3.flagIrq < 0)
                                    {
                                        f3.valueTAU.Add(f3.TAUValue[keyTAU]);
                                        if (f3.valueListTAU == "")
                                            f3.valueListTAU = f3.TAUValue[keyTAU];
                                        else
                                            f3.valueListTAU = f3.valueListTAU + "、" + f3.TAUValue[keyTAU];
                                    }
                                    // f3.richTextBox2.Text += "Time array uint " + xe1.InnerText + " interrupt : " + f3.TAUValue[keyTAU] + " confirm done.\n"; 
                                    f3.textBox22.Text = f3.valueListTAU;

                                }
                                f3.item_flag[7] = sxe.Count;

                                irqi100[7]++;
                            }
                        }
                        if (xe.GetAttribute("Register") == "UART")
                        {
                            XmlNodeList sxe = xe.ChildNodes;

                            foreach (XmlNode node1 in sxe)
                            {
                                XmlElement xe1 = (XmlElement)node1;//NODE FUN

                                if (irqi100[8] < sxe.Count)
                                {
                                    f1irq_list_value[irqi100[8], 8] = xe1.InnerText;
                                    f3.irq_list_value[irqi100[8], 8] = xe1.InnerText;

                                    int keyUART = Array.IndexOf(f3.UARTKey.ToArray(), (object)xe1.InnerText);
                                    f3.flagIrq = f3.valueUART.IndexOf(f3.UARTValue[keyUART, 0]);
                                    if (f3.flagIrq < 0)
                                    {
                                        f3.valueUART.Add(f3.UARTValue[keyUART, 0]); f3.valueUART.Add(f3.UARTValue[keyUART, 1]); f3.valueUART.Add(f3.UARTValue[keyUART, 1]); f3.valueUART.Add(f3.UARTValue[keyUART, 2]); f3.valueUART.Add(f3.UARTValue[keyUART, 3]);
                                        if (f3.valueListUART == "")
                                            f3.valueListUART = f3.UARTValue[keyUART, 0] + "、" + f3.UARTValue[keyUART, 1] + "、" + f3.UARTValue[keyUART, 2] + "、" + f3.UARTValue[keyUART, 3];
                                        else
                                            f3.valueListUART = f3.valueListUART + "、" + f3.UARTValue[keyUART, 0] + "、" + f3.UARTValue[keyUART, 1] + "、" + f3.UARTValue[keyUART, 2] + "、" + f3.UARTValue[keyUART, 3];
                                    }
                                    // f3.richTextBox2.Text += "UART " + xe1.InnerText + " interrupt : " + f3.UARTValue[keyUART, 0] + "、" + f3.UARTValue[keyUART, 1] + "、" + f3.UARTValue[keyUART, 2] + "、" + f3.UARTValue[keyUART, 3] + " confirm done.\n"; 
                                    f3.textBox17.Text = f3.valueListUART;

                                }
                                f3.item_flag[8] = sxe.Count;

                                irqi100[8]++;
                            }
                        }
                        if (xe.GetAttribute("Register") == "SPI")
                        {
                            XmlNodeList sxe = xe.ChildNodes;

                            foreach (XmlNode node1 in sxe)
                            {
                                XmlElement xe1 = (XmlElement)node1;//NODE FUN

                                if (irqi100[10] < sxe.Count)
                                {
                                    f1irq_list_value[irqi100[9], 9] = xe1.InnerText;
                                    f3.irq_list_value[irqi100[9], 9] = xe1.InnerText;

                                    int keySPI = Array.IndexOf(f3.SPIKey.ToArray(), (object)xe1.InnerText);
                                    f3.flagIrq = f3.valueSPI.IndexOf(f3.SPIValue[keySPI, 0]);
                                    if (f3.flagIrq < 0)
                                    {
                                        f3.valueSPI.Add(f3.SPIValue[keySPI, 0]); f3.valueSPI.Add(f3.SPIValue[keySPI, 1]); f3.valueSPI.Add(f3.SPIValue[keySPI, 2]);
                                        if (f3.valueListSPI == "")
                                            f3.valueListSPI = f3.SPIValue[keySPI, 0] + "、" + f3.SPIValue[keySPI, 1] + "、" + f3.SPIValue[keySPI, 2];
                                        else
                                            f3.valueListSPI = f3.valueListSPI + "、" + f3.SPIValue[keySPI, 0] + "、" + f3.SPIValue[keySPI, 1] + "、" + f3.SPIValue[keySPI, 2];
                                    }
                                    // f3.richTextBox2.Text += "SPI " + xe1.InnerText + " interrupt : " + f3.SPIValue[keySPI, 0] + "、" + f3.SPIValue[keySPI, 1] + "、" + f3.SPIValue[keySPI, 2] + " confirm done.\n"; 
                                    f3.textBox16.Text = f3.valueListSPI;


                                }
                                f3.item_flag[9] = sxe.Count;

                                irqi100[9]++;
                            }
                        }
                        if (xe.GetAttribute("Register") == "WD")
                        {
                            XmlNodeList sxe = xe.ChildNodes;
                            foreach (XmlNode node1 in sxe)
                            {
                                XmlElement xe1 = (XmlElement)node1;//NODE FUN

                                if (irqi100[10] < sxe.Count)
                                {
                                    f1irq_list_value[irqi100[10], 10] = xe1.InnerText;
                                    f3.irq_list_value[irqi100[10], 10] = xe1.InnerText;

                                    int keyWD = Array.IndexOf(f3.WDKey.ToArray(), (object)xe1.InnerText);
                                    f3.flagIrq = f3.valueWD.IndexOf(f3.WDValue[keyWD]);
                                    if (f3.flagIrq < 0)
                                    {
                                        f3.valueWD.Add(f3.WDValue[keyWD]);
                                        if (f3.valueListWD == "")
                                            f3.valueListWD = f3.WDValue[keyWD];
                                        else
                                            f3.valueListWD = f3.valueListWD + "、" + f3.WDValue[keyWD];
                                    }
                                    // f3.richTextBox2.Text += "WD " + xe1.InnerText + " interrupt : " + f3.WDValue[keyWD] + " confirm done.\n"; 
                                    f3.textBox14.Text = f3.valueListWD;

                                }
                                f3.item_flag[10] = sxe.Count;

                                irqi100[10]++;
                            }
                        }
                    }
                    mid = irqi100[0];
                    for (int midi = 1; midi < 11; midi++)
                    {
                        if (irqi100[midi] > mid)
                        {
                            mid = irqi100[midi];
                        }
                    }
                    ireader.Close();
                    ixmlDoc.Save(f3.irq_path);
                    f3.irq_return_list(f1irq_list_value, mid);
                    XmlDocument xmlDoc = new XmlDocument();
                    XmlReaderSettings settings = new XmlReaderSettings();
                    settings.IgnoreComments = true;
                    XmlReader reader = XmlReader.Create(f3.pin_path, settings);
                    xmlDoc.Load(reader);
                    XmlNode root = xmlDoc.SelectSingleNode("GPIO_Set");
                    XmlNodeList xn = xmlDoc.SelectSingleNode("GPIO_Set").ChildNodes;
                    foreach (XmlNode node in xn)
                    {
                        XmlElement xe = (XmlElement)node;//NODE FUN

                        if (xe.GetAttribute("Item") == "LPORT")
                        {
                            XmlNodeList sxe = xe.ChildNodes;
                            foreach (XmlNode node1 in sxe)
                            {
                                XmlElement xe1 = (XmlElement)node1;
                                node_num = Convert.ToInt32(xe1.GetAttribute("cnt"));

                            }
                        }
                    }
                    for (int j = 1; j < node_num + 1; j++)
                    {
                        foreach (XmlNode node in xn)
                        {
                            XmlElement xe = (XmlElement)node;//NODE FUN

                            if (node.Name == "LIST")
                            {
                                foreach (XmlNode node1 in xe)
                                {
                                    XmlElement xe1 = (XmlElement)node1;//NODE FUN
                                    if ((xe1.GetAttribute("cnt") == j.ToString()) && k < 12)
                                    {

                                        list_value[j, k] = xe1.InnerText;
                                        k++;
                                    }
                                }

                            }

                        }
                        k = 0;
                    }
                    reader.Close();
                    xmlDoc.Save(f3.pin_path);

                    f3.return_list(list_value, node_num + 1);
                }
                MessageBox.Show("原有数据导入完毕，请继续配置");
            }
        }

        private void 查看帮助ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("WINWORD.EXE", System.Environment.CurrentDirectory + "\\xsl_src\\README.doc");
        }

        private void loadPinxlxsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fd = new FolderBrowserDialog();
            if (!(path_generator_excel1 == "" || path_generator_excel1 == null))
                fd.SelectedPath = path_generator_excel1;
            fd.Description = "Pin64工程将会被建立在以下文件夹";
            
            if (fd.ShowDialog() == DialogResult.OK)
            {
                if (!Directory.Exists(fd.SelectedPath))
                {

                    Directory.CreateDirectory(fd.SelectedPath);
                }
                if (Directory.Exists(fd.SelectedPath))
                {
                    Directory.CreateDirectory(fd.SelectedPath);
                    string path_value = fd.SelectedPath;
                    string pin_path = fd.SelectedPath + "\\Pin64.xml";
                    gpio.Creat_Gpio_Xml(ref pin_path);
                    path_generator_excel = fd.SelectedPath;
                    if (!(path_generator_excel == path_generator_excel1))
                    {
                        p.change_node("path_generator_excel", path_generator_excel, location1);
                        p.getPathGenertor(location1);
                        path_generator_excel1 = p.path_generator_excel1;
                    }
                }
            }

            FolderBrowserDialog fd1 = new FolderBrowserDialog();
            if (!(path_generator_excel641 == "" || path_generator_excel641 == null))
                fd1.SelectedPath = path_generator_excel64;
            fd1.Description = "MCU_PIN64.CC.h将会被建立在以下文件夹";

            if (fd1.ShowDialog() == DialogResult.OK)
            {
                if (!Directory.Exists(fd1.SelectedPath))
                {

                    Directory.CreateDirectory(fd1.SelectedPath);
                }
                if (Directory.Exists(fd1.SelectedPath))
                {
                    Directory.CreateDirectory(fd1.SelectedPath);
                    path_generator_excel64 = fd1.SelectedPath;
                    if (!(path_generator_excel64 == path_generator_excel641))
                    {
                        p.change_node("path_generator_excel64", path_generator_excel64, location1);
                        p.getPathGenertor(location1);
                        path_generator_excel641 = p.path_generator_excel641;
                    }
                }
            }

            if (File.Exists(path_generator_excel64 + "\\MCU_64PIN_CC.h"))
            {
                MessageBox.Show("此文件夹下已存在MCU_64PIN_CC.h，现将原来文件后缀名改为.bat");
                if (File.Exists(path_generator_excel64 + "\\MCU_64PIN_CC.h.bat"))
                {
                    System.IO.File.Delete(path_generator_excel64 + "\\MCU_64PIN_CC.h.bat");;
                }
                System.IO.File.Move(path_generator_excel64 + "\\MCU_64PIN_CC.h", path_generator_excel64 + "\\MCU_64PIN_CC.h.bat");
            }
            ExcelTransform excelTransform = new ExcelTransform();
            excelTransform.OpenExcel(System.Environment.CurrentDirectory + "\\xsl_src\\Pin64.xlsx", path_generator_excel);
            string version_value = "", data_value = "", name_value = "", comment_value = "", info_path = "";
            version_value = textBox1.Text;
            data_value = textBox4.Text;
            name_value = textBox2.Text;
            comment_value = textBox3.Text;
            XslCompiledTransform trans = new XslCompiledTransform();
            info_path = path_generator_excel + "\\Pin64.xml";
            string default_path = path_generator_excel + "\\DefaultPin64.xml";
            string merge_path = path_generator_excel + "\\MergePin64.xml";
            Merge m = new Merge();
            m.Create_Default_Gpio_Xml(default_path, 64);//生成Reset默认值xml文档  
            m.Creat_Gpio_Xml(merge_path);//生成merge FUN xml 文档 
            //将DefaultPin64.xml和Pin64.xml合并
            m.mergeXml(info_path, default_path, merge_path);
            gpio.change_xml_info(ref info_path, ref version_value, ref data_value, ref name_value, ref comment_value);
            trans.Load(System.Environment.CurrentDirectory + "\\xsl_src\\Pin64.xsl");
            trans.Transform(path_generator_excel + "\\MergePin64.xml", path_generator_excel64 + "\\MCU_64PIN_CC.h");
            MessageBox.Show("MCU文件生成已成功");
        }

        private void loadPin100ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fd = new FolderBrowserDialog();
            if (!(path_generator_excel1 == ""||path_generator_excel1==null))
                fd.SelectedPath = path_generator_excel;
            fd.Description = "Pin100工程将会被建立在以下文件夹";
            if (fd.ShowDialog() == DialogResult.OK)
            {
                if (!Directory.Exists(fd.SelectedPath))
                {

                    Directory.CreateDirectory(fd.SelectedPath);
                }
                if (Directory.Exists(fd.SelectedPath))
                {

                    Directory.CreateDirectory(fd.SelectedPath);
                    string path_value = fd.SelectedPath;
                    string pin_path = fd.SelectedPath + "\\Pin100.xml";
                    gpio.Creat_Gpio_Xml(ref pin_path);
                    path_generator_excel = fd.SelectedPath;
                    if (!(path_generator_excel == path_generator_excel1))
                    {
                        p.change_node("path_generator_excel", path_generator_excel, location1);
                        p.getPathGenertor(location1);
                        path_generator_excel1 = p.path_generator_excel1;
                    }
                }
            }

            FolderBrowserDialog fd1 = new FolderBrowserDialog();
            if (!(path_generator_excel1001 == "" || path_generator_excel1001 == null))
                fd1.SelectedPath = path_generator_excel1001;
            fd1.Description = "MCU_PIN.CC.h将会被建立在以下文件夹";

            if (fd1.ShowDialog() == DialogResult.OK)
            {
                if (!Directory.Exists(fd1.SelectedPath))
                {

                    Directory.CreateDirectory(fd1.SelectedPath);
                }
                if (Directory.Exists(fd1.SelectedPath))
                {
                    Directory.CreateDirectory(fd1.SelectedPath);
                    path_generator_excel100 = fd1.SelectedPath;
                    if (!(path_generator_excel100 == path_generator_excel1001))
                    {
                        p.change_node("path_generator_excel100", path_generator_excel100, location1);
                        p.getPathGenertor(location1);
                        path_generator_excel1001 = p.path_generator_excel1001;
                    }
                }
            }

            if (File.Exists(path_generator_excel100 + "\\MCU_PIN_CC.h"))
            {
                MessageBox.Show("此文件夹下已存在MCU_PIN_CC.h，现将原来文件后缀名改为.bat");
                if (File.Exists(path_generator_excel100 + "\\MCU_PIN_CC.h.bat"))
                {
                    System.IO.File.Delete(path_generator_excel100 + "\\MCU_PIN_CC.h.bat");
                }
                System.IO.File.Move(path_generator_excel100 + "\\MCU_PIN_CC.h", path_generator_excel100 + "\\MCU_PIN_CC.h.bat");
            }
            ExcelTransform excelTransform = new ExcelTransform();
            excelTransform.OpenExcel(System.Environment.CurrentDirectory + "\\xsl_src\\Pin100.xlsx", path_generator_excel);
            string version_value = "", data_value = "", name_value = "", comment_value = "", info_path = "";
            version_value = textBox1.Text;
            data_value = textBox4.Text;
            name_value = textBox2.Text;
            comment_value = textBox3.Text;
            XslCompiledTransform trans = new XslCompiledTransform();
            info_path = path_generator_excel + "\\Pin100.xml";
            string default_path = path_generator_excel + "\\DefaultPin100.xml";
            string merge_path = path_generator_excel + "\\MergePin100.xml";
            Merge m = new Merge();
            m.Create_Default_Gpio_Xml(default_path, 64);//生成Reset默认值xml文档  
            m.Creat_Gpio_Xml(merge_path);//生成merge FUN xml 文档 
            //将DefaultPin64.xml和Pin64.xml合并
            m.mergeXml(info_path, default_path, merge_path);
            gpio.change_xml_info(ref info_path, ref version_value, ref data_value, ref name_value, ref comment_value);
            trans.Load(System.Environment.CurrentDirectory + "\\xsl_src\\Pin100.xsl");
            trans.Transform(path_generator_excel + "\\MergePin100.xml", path_generator_excel100 + "\\MCU_PIN_CC.h");
            MessageBox.Show("MCU文件生成已成功");
        }

        private void loadInterruptToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fd = new FolderBrowserDialog();
            if (!(path_generator_excel1 == "" || path_generator_excel1 == null))
                fd.SelectedPath = path_generator_excel1;
            fd.Description = "中断配置工程将会被建立在以下文件夹";
            if (fd.ShowDialog() == DialogResult.OK)
            {
                if (!Directory.Exists(fd.SelectedPath))
                {

                    Directory.CreateDirectory(fd.SelectedPath);
                }
                if (Directory.Exists(fd.SelectedPath))
                {

                    Directory.CreateDirectory(fd.SelectedPath);
                    string path_value = fd.SelectedPath;
                    string irq_path = fd.SelectedPath + "\\IRQ.xml";
                    irq.Creat_irq_Xml(ref irq_path);
                    path_generator_excel = fd.SelectedPath;
                    if (!(path_generator_excel == path_generator_excel1))
                    {
                        p.change_node("path_generator_excel", path_generator_excel, location1);
                        p.getPathGenertor(location1);
                        path_generator_excel1 = p.path_generator_excel1;
                    }
                }
            }

            FolderBrowserDialog fd1 = new FolderBrowserDialog();
            if (!(path_generator_interrupt1 == "" || path_generator_interrupt1 == null))
                fd1.SelectedPath = path_generator_interrupt1;
            fd1.Description = "中断配置文件将会被建立在以下文件夹";

            if (fd1.ShowDialog() == DialogResult.OK)
            {
                if (!Directory.Exists(fd1.SelectedPath))
                {

                    Directory.CreateDirectory(fd1.SelectedPath);
                }
                if (Directory.Exists(fd1.SelectedPath))
                {
                    Directory.CreateDirectory(fd1.SelectedPath);
                    path_generator_interrupt = fd1.SelectedPath;
                    if (!(path_generator_interrupt1 == path_generator_interrupt))
                    {
                        p.change_node("path_generator_interrupt", path_generator_interrupt, location1);
                        p.getPathGenertor(location1);
                        path_generator_interrupt1 = p.path_generator_interrupt1;
                    }
                }
            }

            if (File.Exists(path_generator_interrupt + "\\intvect_c0.c") && File.Exists(path_generator_interrupt + "\\Tcb.h") && File.Exists(path_generator_interrupt + "\\Tcbpost.h") && File.Exists(path_generator_interrupt + "\\osConfigBlock.c"))
            {
                MessageBox.Show("此文件夹下已存在中断配置文件，现将原来文件后缀名改为.bat");
                if (File.Exists(path_generator_interrupt + "\\intvect_c0.c.bat"))
                {
                    System.IO.File.Delete(path_generator_interrupt + "\\intvect_c0.c.bat");
                    System.IO.File.Delete(path_generator_interrupt + "\\Tcb.h.bat");
                    System.IO.File.Delete(path_generator_interrupt + "\\Tcbpost.h.bat");
                    System.IO.File.Delete(path_generator_interrupt + "\\osConfigBlock.c.bat");
                }
                System.IO.File.Move(path_generator_interrupt + "\\intvect_c0.c", path_generator_interrupt + "\\intvect_c0.c.bat");
                System.IO.File.Move(path_generator_interrupt + "\\Tcb.h", path_generator_interrupt + "\\Tcb.h.bat");
                System.IO.File.Move(path_generator_interrupt + "\\Tcbpost.h", path_generator_interrupt + "\\Tcbpost.h.bat");
                System.IO.File.Move(path_generator_interrupt + "\\osConfigBlock.c", path_generator_interrupt + "\\osConfigBlock.c.bat");
            }
            ExcelTransform excelTransform = new ExcelTransform();
            excelTransform.OpenExcel(System.Environment.CurrentDirectory + "\\xsl_src\\Interrupt.xlsx", path_generator_excel);
            string version_value = "", data_value = "", name_value = "", comment_value = "", info_path_i = "";
            version_value = textBox1.Text;
            data_value = textBox4.Text;
            name_value = textBox2.Text;
            comment_value = textBox3.Text;
            XslCompiledTransform trans = new XslCompiledTransform();
            info_path_i = path_generator_excel + "\\IRQ.xml";
            irq.change_xml_info(ref info_path_i, ref version_value, ref data_value, ref name_value, ref comment_value);
            trans.Load(System.Environment.CurrentDirectory + "\\xsl_src\\intvect_c0_c.xsl");
            trans.Transform(path_generator_excel + "\\IRQ.xml", path_generator_interrupt + "\\intvect_c0.c");
            trans.Load(System.Environment.CurrentDirectory + "\\xsl_src\\tcb_h.xsl");
            trans.Transform(path_generator_excel + "\\IRQ.xml", path_generator_interrupt + "\\Tcb.h");
            trans.Load(System.Environment.CurrentDirectory + "\\xsl_src\\Tcbpost_h.xsl");
            trans.Transform(path_generator_excel + "\\IRQ.xml", path_generator_interrupt + "\\Tcbpost.h");
            trans.Load(System.Environment.CurrentDirectory + "\\xsl_src\\osConfigBlock_c.xsl");
            trans.Transform(path_generator_excel + "\\IRQ.xml", path_generator_interrupt + "\\osConfigBlock.c");
            MessageBox.Show("Interrupt文件生成已成功");
        }

        private void 生成ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string version_value = "", data_value = "", name_value = "", comment_value = "", info_path = "", info_path_i = "";
            version_value = textBox1.Text;
            data_value = textBox4.Text;
            name_value = textBox2.Text;
            comment_value = textBox3.Text;
            XslCompiledTransform trans = new XslCompiledTransform();
            if (File.Exists(path_generator + "\\Pin64.xml"))
            {
                FolderBrowserDialog fd = new FolderBrowserDialog();
                if (!(path_generator_pin641 == "" || path_generator_pin641 == null))
                {
                    fd.SelectedPath = path_generator_pin641;
                }
                fd.Description = "MCU_64PIN_CC.h将会被建立在以下文件夹";
                if (fd.ShowDialog() == DialogResult.OK)
                {
                    if (!Directory.Exists(fd.SelectedPath))
                    {

                        Directory.CreateDirectory(fd.SelectedPath);
                    }
                    if (Directory.Exists(fd.SelectedPath))
                    {
                        Directory.CreateDirectory(fd.SelectedPath);
                        string path_value = fd.SelectedPath;
                        string pin_path = path_generator + "\\Pin64.xml";
                        path_generator_pin64 = fd.SelectedPath;
                        if (!(path_generator_pin64 == path_generator_pin641))
                        {
                            p.change_node("path_generator_pin64", path_generator_pin64, location1);
                            p.getPathGenertor(location1);
                            path_generator_pin641 = p.path_generator_pin641;
                        }
                    }
                    if (File.Exists(path_generator_pin64 + "\\MCU_64PIN_CC.h"))
                    {
                        MessageBox.Show("此文件夹下已存在MCU_64PIN_CC.h，现将原来文件后缀名改为.bat");
                        if (File.Exists(path_generator_pin64 + "\\MCU_64PIN_CC.h.bat"))
                            System.IO.File.Delete(path_generator_pin64 + "\\MCU_64PIN_CC.h.bat");
                        System.IO.File.Move(path_generator_pin64 + "\\MCU_64PIN_CC.h", path_generator_pin64 + "\\MCU_64PIN_CC.h.bat");
                    }
                    info_path = path_generator + "\\Pin64.xml";
                    string default_path = path_generator + "\\DefaultPin64.xml";
                    string merge_path = path_generator + "\\MergePin64.xml";
                    Merge m = new Merge();
                    m.Create_Default_Gpio_Xml(default_path, 64);//生成Reset默认值xml文档  
                    m.Creat_Gpio_Xml(merge_path);//生成merge FUN xml 文档 
                    //将DefaultPin64.xml和Pin64.xml合并
                    m.mergeXml(info_path, default_path, merge_path);
                    gpio.change_xml_info(ref info_path, ref version_value, ref data_value, ref name_value, ref comment_value);
                    trans.Load(System.Environment.CurrentDirectory + "\\xsl_src\\Pin64.xsl");
                    trans.Transform(path_generator + "\\MergePin64.xml", path_generator_pin64 + "\\MCU_64PIN_CC.h");
                    trans.Load(System.Environment.CurrentDirectory + "\\xsl_src\\intvect_c0_c.xsl");
                }
                MessageBox.Show("MCU_64PIN_CC.c生成已成功");
            }
            if (File.Exists(path_generator + "\\IRQ64.xml"))
            {
                FolderBrowserDialog fd = new FolderBrowserDialog();
                if (!(path_generator_interrupt641 == "" || path_generator_interrupt641 == null))
                {
                    fd.SelectedPath = path_generator_interrupt641;
                }
                fd.Description = "PIN64的中断配置文件将会被建立在以下文件夹";
                if (fd.ShowDialog() == DialogResult.OK)
                {
                    if (!Directory.Exists(fd.SelectedPath))
                    {
                        Directory.CreateDirectory(fd.SelectedPath);
                    }
                    if (Directory.Exists(fd.SelectedPath))
                    {
                        Directory.CreateDirectory(fd.SelectedPath);
                        string path_value = fd.SelectedPath;
                        string pin_path = path_generator + "\\IRQ64.xml";
                        path_generator_interrupt64 = fd.SelectedPath;
                        if (!(path_generator_interrupt64 == path_generator_interrupt641))
                        {
                            p.change_node("path_generator_interrupt64", path_generator_interrupt64, location1);
                            p.getPathGenertor(location1);
                            path_generator_interrupt641 = p.path_generator_interrupt641;
                        }
                    }
                    if (File.Exists(path_generator_interrupt64 + "\\intvect_c0.c") && File.Exists(path_generator_interrupt64 + "\\Tcb.h") && File.Exists(path_generator_interrupt64 + "\\Tcbpost.h") && File.Exists(path_generator_interrupt64 + "\\osConfigBlock.c"))
                    {
                        MessageBox.Show("此文件夹下已存在中断配置文件，现将原来文件后缀名改为.bat");
                        if (File.Exists(path_generator_interrupt64 + "\\intvect_c0.c.bat"))
                        {
                            System.IO.File.Delete(path_generator_interrupt64 + "\\intvect_c0.c.bat");
                            System.IO.File.Delete(path_generator_interrupt64 + "\\Tcb.h.bat");
                            System.IO.File.Delete(path_generator_interrupt64 + "\\Tcbpost.h.bat");
                            System.IO.File.Delete(path_generator_interrupt64 + "\\osConfigBlock.c.bat");
                        }
                        System.IO.File.Move(path_generator_interrupt64 + "\\intvect_c0.c", path_generator_interrupt64 + "\\intvect_c0.c.bat");
                        System.IO.File.Move(path_generator_interrupt64 + "\\Tcb.h", path_generator_interrupt64 + "\\Tcb.h.bat");
                        System.IO.File.Move(path_generator_interrupt64 + "\\Tcbpost.h", path_generator_interrupt64 + "\\Tcbpost.h.bat");
                        System.IO.File.Move(path_generator_interrupt64 + "\\osConfigBlock.c", path_generator_interrupt64 + "\\osConfigBlock.c.bat");
                    }
                    info_path_i = path_generator + "\\IRQ64.xml";
                    irq.change_xml_info(ref info_path_i, ref version_value, ref data_value, ref name_value, ref comment_value);
                    trans.Load(System.Environment.CurrentDirectory + "\\xsl_src\\intvect_c0_c.xsl");
                    trans.Transform(path_generator + "\\IRQ64.xml", path_generator_interrupt64 + "\\intvect_c0.c");
                    trans.Load(System.Environment.CurrentDirectory + "\\xsl_src\\tcb_h.xsl");
                    trans.Transform(path_generator + "\\IRQ64.xml", path_generator_interrupt64 + "\\Tcb.h");
                    trans.Load(System.Environment.CurrentDirectory + "\\xsl_src\\Tcbpost_h.xsl");
                    trans.Transform(path_generator + "\\IRQ64.xml", path_generator_interrupt64 + "\\Tcbpost.h");
                    trans.Load(System.Environment.CurrentDirectory + "\\xsl_src\\osConfigBlock_c.xsl");
                    trans.Transform(path_generator + "\\IRQ64.xml", path_generator_interrupt64 + "\\osConfigBlock.c");
                }
                MessageBox.Show("PIN64的中断配置文件生成成功");
            }
            if (File.Exists(path_generator + "\\Pin100.xml"))
            {
                FolderBrowserDialog fd = new FolderBrowserDialog();
                if (!(path_generator_pin1001 == "" || path_generator_pin1001 == null))
                {
                    fd.SelectedPath = path_generator_pin1001;
                }
                fd.Description = "MCU_PIN_CC.h将会被建立在以下文件夹";
                if (fd.ShowDialog() == DialogResult.OK)
                {
                    if (!Directory.Exists(fd.SelectedPath))
                    {

                        Directory.CreateDirectory(fd.SelectedPath);
                    }
                    if (Directory.Exists(fd.SelectedPath))
                    {
                        Directory.CreateDirectory(fd.SelectedPath);
                        string path_value = fd.SelectedPath;
                        string pin_path = path_generator + "\\Pin100.xml";
                        path_generator_pin100 = fd.SelectedPath;
                        if (!(path_generator_pin100 == path_generator_pin1001))
                        {
                            p.change_node("path_generator_pin100", path_generator_pin100, location1);
                            p.getPathGenertor(location1);
                            path_generator_pin1001 = p.path_generator_pin1001;
                        }
                    }
                    if (File.Exists(path_generator_pin100 + "\\MCU_PIN_CC.h"))
                    {
                        MessageBox.Show("此文件夹下已存在MCU_PIN_CC.h，现将原来文件后缀名改为.bat");
                        if (File.Exists(path_generator_pin100 + "\\MCU_PIN_CC.h.bat"))
                            System.IO.File.Delete(path_generator_pin100 + "\\MCU_PIN_CC.h.bat");
                        System.IO.File.Move(path_generator_pin100 + "\\MCU_PIN_CC.h", path_generator_pin100 + "\\MCU_PIN_CC.h.bat");

                    }
                    info_path = path_generator + "\\Pin100.xml";
                    string default_path = path_generator + "\\DefaultPin100.xml";
                    string merge_path = path_generator + "\\MergePin100.xml";
                    Merge m = new Merge();
                    m.Create_Default_Gpio_Xml(default_path, 100);//生成Reset默认值xml文档  
                    m.Creat_Gpio_Xml(merge_path);//生成merge FUN xml 文档 
                    //将DefaultPin100.xml和Pin100.xml合并
                    m.mergeXml(info_path, default_path, merge_path);
                    gpio.change_xml_info(ref info_path, ref version_value, ref data_value, ref name_value, ref comment_value);
                    trans.Load(System.Environment.CurrentDirectory + "\\xsl_src\\Pin100.xsl");
                    trans.Transform(path_generator + "\\MergePin100.xml", path_generator_pin100 + "\\MCU_PIN_CC.h");
                }
                MessageBox.Show("MCU_PIN_CC.c生成已成功");
            }
            if (File.Exists(path_generator + "\\IRQ100.xml"))
            {
                FolderBrowserDialog fd = new FolderBrowserDialog();
                if (!(path_generator_interrupt1001 == "" || path_generator_interrupt1 == null))
                {
                    fd.SelectedPath = path_generator_interrupt1001;
                }
                fd.Description = "PIN100的中断配置文件将会被建立在以下文件夹";
                if (fd.ShowDialog() == DialogResult.OK)
                {
                    if (!Directory.Exists(fd.SelectedPath))
                    {
                        Directory.CreateDirectory(fd.SelectedPath);
                    }
                    if (Directory.Exists(fd.SelectedPath))
                    {
                        Directory.CreateDirectory(fd.SelectedPath);
                        string path_value = fd.SelectedPath;
                        string pin_path = path_generator + "\\IRQ100.xml";
                        path_generator_interrupt100 = fd.SelectedPath;
                        if (!(path_generator_interrupt100 == path_generator_interrupt1001))
                        {
                            p.change_node("path_generator_interrupt100", path_generator_interrupt100, location1);
                            p.getPathGenertor(location1);
                            path_generator_interrupt1001 = p.path_generator_interrupt1001;
                        }
                        
                    }
                    if (File.Exists(path_generator_interrupt100 + "\\intvect_c0.c") && File.Exists(path_generator_interrupt100 + "\\Tcb.h") && File.Exists(path_generator_interrupt100 + "\\Tcbpost.h") && File.Exists(path_generator_interrupt100 + "\\osConfigBlock.c"))
                    {
                        MessageBox.Show("此文件夹下已存在中断配置文件，现将原来文件后缀名改为.bat");
                        if (File.Exists(path_generator_interrupt100 + "\\intvect_c0.c.bat"))
                        {
                            System.IO.File.Delete(path_generator_interrupt100 + "\\intvect_c0.c.bat");
                            System.IO.File.Delete(path_generator_interrupt100 + "\\Tcb.h.bat");
                            System.IO.File.Delete(path_generator_interrupt100 + "\\Tcbpost.h.bat");
                            System.IO.File.Delete(path_generator_interrupt100 + "\\osConfigBlock.c.bat");
                        }
                        System.IO.File.Move(path_generator_interrupt100 + "\\intvect_c0.c", path_generator_interrupt100 + "\\intvect_c0.c.bat");
                        System.IO.File.Move(path_generator_interrupt100 + "\\Tcb.h", path_generator_interrupt100 + "\\Tcb.h.bat");
                        System.IO.File.Move(path_generator_interrupt100 + "\\Tcbpost.h", path_generator_interrupt100 + "\\Tcbpost.h.bat");
                        System.IO.File.Move(path_generator_interrupt100 + "\\osConfigBlock.c", path_generator_interrupt100 + "\\osConfigBlock.c.bat");
                    }
                    info_path_i = path_generator + "\\IRQ100.xml";
                    irq.change_xml_info(ref info_path_i, ref version_value, ref data_value, ref name_value, ref comment_value);
                    trans.Load(System.Environment.CurrentDirectory + "\\xsl_src\\intvect_c0_c.xsl");
                    trans.Transform(path_generator + "\\IRQ100.xml", path_generator_interrupt100 + "\\intvect_c0.c");
                    trans.Load(System.Environment.CurrentDirectory + "\\xsl_src\\tcb_h.xsl");
                    trans.Transform(path_generator + "\\IRQ100.xml", path_generator_interrupt100 + "\\Tcb.h");
                    trans.Load(System.Environment.CurrentDirectory + "\\xsl_src\\Tcbpost_h.xsl");
                    trans.Transform(path_generator + "\\IRQ100.xml", path_generator_interrupt100 + "\\Tcbpost.h");
                    trans.Load(System.Environment.CurrentDirectory + "\\xsl_src\\osConfigBlock_c.xsl");
                    trans.Transform(path_generator + "\\IRQ100.xml", path_generator_interrupt100 + "\\osConfigBlock.c");
                }
                MessageBox.Show("PIN100的中断配置文件生成成功");
            }
        }
    }
}