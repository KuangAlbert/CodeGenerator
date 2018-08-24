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
using irq_set;
using IO;
//******************************************************************************************************************************************************
//                                            文件功能：主界面，包含CG所有功能配置
//------------------------------------------------------------------------------------------------------------------------------------------------------
//                             
//******************************************************************************************************************************************************
namespace CodeGenerator
{
    public partial class Form3 : Form
    {

        Io gpio = new Io();
        IRQ irq = new IRQ();
        public string pin_path;
        public string irq_path;
        public Form3()
        {
            InitializeComponent();
            panel1.Enabled = false;
            foreach (Control c in groupBox13.Controls)
            {
                if (c is ComboBox)
                {
                    ComboBox cc = (ComboBox)c;
                    cc.DropDownStyle = ComboBoxStyle.DropDownList;
                }

            }
            foreach (Control c in panel1.Controls)
            {
                if (c is ComboBox)
                {
                    ComboBox cc = (ComboBox)c;
                    cc.DropDownStyle = ComboBoxStyle.DropDownList;
                }
                if (c is TextBox)
                {
                    TextBox cc = (TextBox)c;
                    cc.ReadOnly = true;
                }

            }
        }

        public void Current_Path(ref string value)
        {
            textBox11.Text = value;
            pin_path = textBox11.Text + "\\pin100.xml";
            irq_path = textBox11.Text + "\\IRQ100.xml";
        }

        //******************************************************************************************************************************************************
        //                                                    渐变出现效果
        //******************************************************************************************************************************************************
        public const Int32 AW_HOR_POSITIVE = 0X00000001;
        public const Int32 AW_HOR_NEGATIVE = 0X00000002;
        public const Int32 AW_VER_POSITIVE = 0X00000004;
        public const Int32 AW_VER_NEGATIVE = 0X00000008;
        public const Int32 AW_CENTER = 0X00000010;
        public const Int32 AW_HIDE = 0X00010000;
        public const Int32 AW_ACTIVATE = 0X00020000;
        public const Int32 AW_SLIDE = 0X00040000;
        public const Int32 AW_BLEND = 0X00080000;
        [System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
        private static extern bool AnimateWindow(IntPtr hwnd, int dwTime, int dwFlags);
        private void Form3_Load_1(object sender, EventArgs e)
        {
            AnimateWindow(this.Handle, 1500, AW_BLEND);
        }

        //******************************************************************************************************************************************************
        //                                                                  Alternative模式选择
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        //                                                           
        //******************************************************************************************************************************************************


        //******************************************************************************************************************************************************
        //                                             标志位
        //******************************************************************************************************************************************************
        public int Node_Change_Flag = 0, Node_Start_Flag = 0;//Node_Change_Flag:判断在非第一次配置时选择pin有没有配置过（1：配置过，直接修改值 2：没配置，先加入node再修改值）
        //Node_Start_Flag:判断是否第一次配置（1：非第一次 0：第一次）
        public int group_flag = 100;
        public int[] item_flag = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };//中断配置行标志位
        public int flag_count = 0;
        //******************************************************************************************************************************************************
        //                                             用于赋值的模式、组、端口
        //******************************************************************************************************************************************************
        public string Alt_mode = "";//A\R\S mode
        public string alt_mode = "";//用于赋值
        public string alt_group = "";//用于赋值
        public int alt_number = 0;//选择的模式，组，端口（移位个数）
        public string[,] irq_list_value = new string[300, 300];




        //******************************************************************************************************************************************************
        //                                             模式、功能值
        //******************************************************************************************************************************************************
        public int Fun_Num = 0;//选择了那种功能，In 5种，Out 5种
        public int ARS = 0;//选择了那种模式，active、reset、standby





        //******************************************************************************************************************************************************
        //                                             寄存器值    数组:P0  P8  P9  P10  JP0  AP0
        //******************************************************************************************************************************************************
        public int Alt_PM_value = 0, Alt_PFC_value = 0, Alt_PFCE_value = 0, Alt_PFCAE_value = 0, Alt_PMC_value = 0, Alt_PIBC_value = 0, Alt_PU_value = 0, Alt_PD_value = 0, Alt_PBDC_value = 0, Alt_PDSC_value = 0, Alt_PODC_value = 0, Alt_PIPC_value = 0, Alt_Port_value = 0;//中介初值
        public int[] Alt_PM_value_Act = { 65535, 65535, 65535, 65535, 65535, 65535, 65535 }, Alt_PFC_value_Act = { 0, 0, 0, 0, 0, 0, 0 }, Alt_PFCE_value_Act = { 0, 0, 0, 0, 0, 0, 0 }, Alt_PFCAE_value_Act = { 0, 0, 0, 0, 0, 0, 0 }, Alt_PMC_value_Act = { 0, 0, 0, 0, 0, 0, 0 }, Alt_PIBC_value_Act = { 0, 0, 0, 0, 0, 0, 0 }, Alt_PU_value_Act = { 0, 0, 0, 0, 0, 0, 0 }, Alt_PD_value_Act = { 0, 0, 0, 0, 0, 0, 0 }, Alt_PBDC_value_Act = { 0, 0, 0, 0, 0, 0, 0 }, Alt_PDSC_value_Act = { 0, 0, 0, 0, 0, 0, 0 }, Alt_PODC_value_Act = { 0, 0, 0, 0, 0, 0, 0 }, Alt_PIPC_value_Act = { 0, 0, 0, 0, 0, 0, 0 }, Alt_Port_value_Act = { 0, 0, 0, 0, 0, 0, 0 };
        public int[] Alt_PM_value_Res = { 65535, 65535, 65535, 65535, 65535, 65535, 65535 }, Alt_PFC_value_Res = { 0, 0, 0, 0, 0, 0, 0 }, Alt_PFCE_value_Res = { 0, 0, 0, 0, 0, 0, 0 }, Alt_PFCAE_value_Res = { 0, 0, 0, 0, 0, 0, 0 }, Alt_PMC_value_Res = { 0, 0, 0, 0, 0, 0, 0 }, Alt_PIBC_value_Res = { 0, 0, 0, 0, 0, 0, 0 }, Alt_PU_value_Res = { 0, 0, 0, 0, 0, 0, 0 }, Alt_PD_value_Res = { 0, 0, 0, 0, 0, 0, 0 }, Alt_PBDC_value_Res = { 0, 0, 0, 0, 0, 0, 0 }, Alt_PDSC_value_Res = { 0, 0, 0, 0, 0, 0, 0 }, Alt_PODC_value_Res = { 0, 0, 0, 0, 0, 0, 0 }, Alt_PIPC_value_Res = { 0, 0, 0, 0, 0, 0, 0 }, Alt_Port_value_Res = { 0, 0, 0, 0, 0, 0, 0 };
        public int[] Alt_PM_value_Sta = { 65535, 65535, 65535, 65535, 65535, 65535, 65535 }, Alt_PFC_value_Sta = { 0, 0, 0, 0, 0, 0, 0 }, Alt_PFCE_value_Sta = { 0, 0, 0, 0, 0, 0, 0 }, Alt_PFCAE_value_Sta = { 0, 0, 0, 0, 0, 0, 0 }, Alt_PMC_value_Sta = { 0, 0, 0, 0, 0, 0, 0 }, Alt_PIBC_value_Sta = { 0, 0, 0, 0, 0, 0, 0 }, Alt_PU_value_Sta = { 0, 0, 0, 0, 0, 0, 0 }, Alt_PD_value_Sta = { 0, 0, 0, 0, 0, 0, 0 }, Alt_PBDC_value_Sta = { 0, 0, 0, 0, 0, 0, 0 }, Alt_PDSC_value_Sta = { 0, 0, 0, 0, 0, 0, 0 }, Alt_PODC_value_Sta = { 0, 0, 0, 0, 0, 0, 0 }, Alt_PIPC_value_Sta = { 0, 0, 0, 0, 0, 0, 0 }, Alt_Port_value_Sta = { 0, 0, 0, 0, 0, 0, 0 };//三种模式下各寄存器初值
        public string pm_flag = "", pmc_flag = "", pibc_flag = "", pu_flag = "", pd_flag = "", pbdc_flag = "", pdsc_flag = "", podc_flag = "", pipc_flag = "", pv_flag = "";
        public Boolean interrupt_flag;//添加中断配置标志位，= true：直接自动加入中断配置；= false ：手动在 Peripheral界面增添中断
        //******************************************************************************************************************************************************
        //                                             界面刷新函数组
        //******************************************************************************************************************************************************
        public void alt_fun_disp(string alt, string group, int number)//父界面进行选择后弹窗，并刷新界面值
        {
            panel1.Enabled = true;
            panel2.Enabled = false;

            groupBox9.Enabled = false;
            Alt_mode = alt_mode;//用于赋值
            alt_group = group;//组
            alt_number = number;//位数，代表位移
            switch (alt_group)
            {
                case "0": group_flag = 0; break;//P0
                case "8": group_flag = 1; break;//P8
                case "9": group_flag = 2; break;//P9
                case "10": group_flag = 3; break;//P10
                case "20": group_flag = 4; break;//JP0
                case "30": group_flag = 5; break;//AP0
                case "11": group_flag = 6; break;//P11
            }
            textBox23.Text = alt;
            resetAltRadioButton();//重置Alt选择按钮
            if (comboBox1.Text == "GPIO")
                comboBox1.Text = "Alternative";
            comboBox1.Text = "GPIO";
            comboBox1.Text = "";
            this.Show();
        }

        public void refesh_node(string Group_Number, string location)//刷新界面十个功能提示框的值，Group_Number选择的组，location xml文件需要保存的地址
        {
            XmlDocument doc = new XmlDocument();
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.IgnoreComments = true;
            XmlReader reader = XmlReader.Create(System.Environment.CurrentDirectory + location, settings);
            doc.Load(reader);
            XmlNodeList xn = doc.SelectSingleNode("AltSQL").ChildNodes;
            foreach (XmlNode node in xn)
            {
                XmlElement xe = (XmlElement)node;
                if (xe.GetAttribute("Group") == Group_Number)
                {
                    XmlNodeList sxe = xe.ChildNodes;
                    foreach (XmlNode node1 in sxe)//遍历AltSQL,提取功能表并显示
                    {
                        if (node1.Name == "In1")
                        {
                            textBox1.Text = node1.InnerText;
                            if (node1.InnerText.Equals(""))
                                radioButton1.Enabled = false;
                        }
                        if (node1.Name == "In2")
                        {
                            textBox2.Text = node1.InnerText;
                            if (node1.InnerText.Equals(""))
                                radioButton9.Enabled = false;
                        }
                        if (node1.Name == "In3")
                        {
                            textBox3.Text = node1.InnerText;
                            if (node1.InnerText.Equals(""))
                                radioButton8.Enabled = false;
                        }
                        if (node1.Name == "In4")
                        {
                            textBox4.Text = node1.InnerText;
                            if (node1.InnerText.Equals(""))
                                radioButton7.Enabled = false;
                        }
                        if (node1.Name == "In5")
                        {
                            textBox5.Text = node1.InnerText;
                            if (node1.InnerText.Equals(""))
                                radioButton6.Enabled = false;
                        }
                        if (node1.Name == "Out1")
                        {
                            textBox10.Text = node1.InnerText;
                            if (node1.InnerText.Equals(""))
                                radioButton5.Enabled = false;
                        }
                        if (node1.Name == "Out2")
                        {
                            textBox9.Text = node1.InnerText;
                            if (node1.InnerText.Equals(""))
                                radioButton4.Enabled = false;
                        }
                        if (node1.Name == "Out3")
                        {
                            textBox8.Text = node1.InnerText;
                            if (node1.InnerText.Equals(""))
                                radioButton3.Enabled = false;
                        }
                        if (node1.Name == "Out4")
                        {
                            textBox7.Text = node1.InnerText;
                            if (node1.InnerText.Equals(""))
                                radioButton2.Enabled = false;
                        }
                        if (node1.Name == "Out5")
                        {
                            textBox6.Text = node1.InnerText;
                            if (node1.InnerText.Equals(""))
                                radioButton10.Enabled = false;
                        }
                    }
                }
            }
            reader.Close();
            doc.Save(System.Environment.CurrentDirectory + location);

        }



        //******************************************************************************************************************************************************
        //                                             切换GPIO/ALT模式组
        //******************************************************************************************************************************************************
        private void Reset_select()
        {

            foreach (Control ctrl in groupBox10.Controls)
            {
                if (ctrl is RadioButton)
                {
                    RadioButton rck = ctrl as RadioButton;
                    if (rck.Checked == true)
                    {
                        rck.Checked = false;
                    }
                }
            }

            foreach (Control ctrl1 in groupBox11.Controls)
            {
                if (ctrl1 is RadioButton)
                {
                    RadioButton rck1 = ctrl1 as RadioButton;
                    if (rck1.Checked == true)
                    {
                        rck1.Checked = false;
                    }
                }
            }

            foreach (Control ctrl2 in groupBox3.Controls)
            {
                if (ctrl2 is RadioButton)
                {
                    RadioButton rck2 = ctrl2 as RadioButton;
                    if (rck2.Checked == true)
                    {
                        rck2.Checked = false;
                    }
                }
            }

            foreach (Control ctrl3 in groupBox5.Controls)
            {
                if (ctrl3 is RadioButton)
                {
                    RadioButton rck3 = ctrl3 as RadioButton;
                    if (rck3.Checked == true)
                    {
                        rck3.Checked = false;
                    }
                }
            }

            checkBox1.Checked = false;
            checkBox2.Checked = false;
            checkBox3.Checked = false;
            checkBox4.Checked = false;
            checkBox5.Checked = false;
            checkBox6.Checked = false;
            checkBox7.Checked = false;
            checkBox8.Checked = false;
            checkBox9.Checked = false;
            checkBox10.Checked = false;
            checkBox25.Checked = false;
            checkBox26.Checked = false;
            checkBox27.Checked = false;
            checkBox28.Checked = false;
            checkBox31.Checked = false;
            checkBox32.Checked = false;

            comboBox2.Text = "No";
            comboBox3.Text = "No";

        }

        private void resetAltRadioButton()
        {
            foreach (Control ctrl in groupBox10.Controls)
            {
                if (ctrl is RadioButton)
                {
                    RadioButton rck = ctrl as RadioButton;
                    rck.Enabled = true;
                }
            }

            foreach (Control ctrl1 in groupBox11.Controls)
            {
                if (ctrl1 is RadioButton)
                {
                    RadioButton rck1 = ctrl1 as RadioButton;
                    rck1.Enabled = true;
                }
            }
        }

        private void comboBox1_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem.ToString() == "GPIO")
            {
                Reset_select();
                panel2.Enabled = true;
                groupBox9.Enabled = false;
                checkBox31.Enabled = true;
                checkBox31.Checked = true;//默认选择IN功能
                groupBox3.Enabled = true;
                checkBox32.Enabled = true;
                checkBox32.Checked = false;
                groupBox5.Enabled = false;
                Fun_Num = 0;
            }
            if (comboBox1.SelectedItem.ToString() == "Alternative")
            {
                Reset_select();
                panel2.Enabled = true;
                groupBox9.Enabled = true;
                checkBox31.Checked = false;
                checkBox32.Checked = false;
                checkBox31.Enabled = false;
                checkBox32.Enabled = false;
                groupBox5.Enabled = false;
                groupBox3.Enabled = false;
                refesh_node(textBox23.Text, "\\xsl_src\\AltSQL100.xml");
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox2.SelectedItem.ToString() == "Yes")
            {
                pipc_flag = "s";
                groupBox3.Enabled = true;
                groupBox5.Enabled = true;
            }
            if (comboBox2.SelectedItem.ToString() == "No")
            {
                pipc_flag = "r";
                if (checkBox31.Checked == true)
                {
                    groupBox3.Enabled = true;
                    groupBox5.Enabled = false;
                }
                if (checkBox32.Checked == true)
                {
                    groupBox5.Enabled = true;
                    groupBox3.Enabled = false;
                }
            }
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox3.SelectedItem.ToString() == "Yes")
            {
                interrupt_flag = true;
            }
            if (comboBox3.SelectedItem.ToString() == "No")
            {
                interrupt_flag = false;
            }
        }

        //******************************************************************************************************************************************************
        //                                             判断Alt功能选择
        //******************************************************************************************************************************************************
        public Boolean judgeRationButton()
        {
            foreach (Control ctrl in groupBox10.Controls)
            {
                if (ctrl is RadioButton)
                {
                    RadioButton rck = ctrl as RadioButton;
                    if (rck.Checked == true) return true;
                }
            }
            foreach (Control ctrl1 in groupBox11.Controls)
            {
                if (ctrl1 is RadioButton)
                {
                    RadioButton rck1 = ctrl1 as RadioButton;
                    if (rck1.Checked == true) return true;
                }
            }
            return false;
        }

        //******************************************************************************************************************************************************
        //                                             选择赋值函数组
        //******************************************************************************************************************************************************
        public void ForeachPanelControls(string ctrlName)//保证按钮互斥
        {
            foreach (Control ctrl in groupBox10.Controls)
            {
                if (ctrl is RadioButton)
                {
                    RadioButton rck = ctrl as RadioButton;
                    if (!(rck.Name.Equals(ctrlName)))
                    {
                        rck.Checked = false;
                    }
                }
            }

            foreach (Control ctrl1 in groupBox11.Controls)
            {
                if (ctrl1 is RadioButton)
                {
                    RadioButton rck1 = ctrl1 as RadioButton;
                    if (!(rck1.Name.Equals(ctrlName)))
                    {
                        rck1.Checked = false;
                    }
                }
            }
        }
        public void ForeachPanelSelects(ref int num)//通过用户选择的端口，确定PM,PFC,PFCE,PFCAE,PMC的值（调用了alt_switch(int a, int b, int c, int d, int e)）
        {
            foreach (Control ctrl in groupBox10.Controls)
            {

                if (ctrl is RadioButton)
                {
                    RadioButton rck = ctrl as RadioButton;
                    if (rck.Checked == true)
                    {
                        switch (num)
                        {

                            case 1: alt_switch("s", "r", "r", "r", "s", pibc_flag, pu_flag, pd_flag, pbdc_flag, pdsc_flag, podc_flag, pipc_flag, pv_flag, group_flag);//PM  PFC  PFCE  PFCAE  PMC  PIBC  PU  PD  PBDC  PDSC PODC PIPC
                                break;
                            case 2: alt_switch("s", "s", "r", "r", "s", pibc_flag, pu_flag, pd_flag, pbdc_flag, pdsc_flag, podc_flag, pipc_flag, pv_flag, group_flag);
                                break;
                            case 3: alt_switch("s", "r", "s", "r", "s", pibc_flag, pu_flag, pd_flag, pbdc_flag, pdsc_flag, podc_flag, pipc_flag, pv_flag, group_flag);
                                break;
                            case 4: alt_switch("s", "s", "s", "r", "s", pibc_flag, pu_flag, pd_flag, pbdc_flag, pdsc_flag, podc_flag, pipc_flag, pv_flag, group_flag);
                                break;
                            case 5: alt_switch("s", "r", "r", "s", "s", pibc_flag, pu_flag, pd_flag, pbdc_flag, pdsc_flag, podc_flag, pipc_flag, pv_flag, group_flag);
                                break;
                            case 6: alt_switch("r", "r", "r", "r", "s", pibc_flag, pu_flag, pd_flag, pbdc_flag, pdsc_flag, podc_flag, pipc_flag, pv_flag, group_flag);
                                break;
                            case 7: alt_switch("r", "s", "r", "r", "s", pibc_flag, pu_flag, pd_flag, pbdc_flag, pdsc_flag, podc_flag, pipc_flag, pv_flag, group_flag);
                                break;
                            case 8: alt_switch("r", "r", "s", "r", "s", pibc_flag, pu_flag, pd_flag, pbdc_flag, pdsc_flag, podc_flag, pipc_flag, pv_flag, group_flag);
                                break;
                            case 9: alt_switch("r", "s", "s", "r", "s", pibc_flag, pu_flag, pd_flag, pbdc_flag, pdsc_flag, podc_flag, pipc_flag, pv_flag, group_flag);
                                break;
                            case 10: alt_switch("r", "r", "r", "s", "s", pibc_flag, pu_flag, pd_flag, pbdc_flag, pdsc_flag, podc_flag, pipc_flag, pv_flag, group_flag);
                                break;


                        }
                    }

                }
            }
            foreach (Control ctrl1 in groupBox11.Controls)
            {

                if (ctrl1 is RadioButton)
                {
                    RadioButton rck1 = ctrl1 as RadioButton;
                    if (rck1.Checked == true)
                    {
                        switch (num)
                        {

                            case 1: alt_switch("s", "r", "r", "r", "s", pibc_flag, pu_flag, pd_flag, pbdc_flag, pdsc_flag, podc_flag, pipc_flag, pv_flag, group_flag);//PM  PFC  PFCE  PFCAE  PMC  PIBC  PU  PD  PBDC  PDSC PODC PIPC
                                break;
                            case 2: alt_switch("s", "s", "r", "r", "s", pibc_flag, pu_flag, pd_flag, pbdc_flag, pdsc_flag, podc_flag, pipc_flag, pv_flag, group_flag);
                                break;
                            case 3: alt_switch("s", "r", "s", "r", "s", pibc_flag, pu_flag, pd_flag, pbdc_flag, pdsc_flag, podc_flag, pipc_flag, pv_flag, group_flag);
                                break;
                            case 4: alt_switch("s", "s", "s", "r", "s", pibc_flag, pu_flag, pd_flag, pbdc_flag, pdsc_flag, podc_flag, pipc_flag, pv_flag, group_flag);
                                break;
                            case 5: alt_switch("s", "r", "r", "s", "s", pibc_flag, pu_flag, pd_flag, pbdc_flag, pdsc_flag, podc_flag, pipc_flag, pv_flag, group_flag);
                                break;
                            case 6: alt_switch("r", "r", "r", "r", "s", pibc_flag, pu_flag, pd_flag, pbdc_flag, pdsc_flag, podc_flag, pipc_flag, pv_flag, group_flag);
                                break;
                            case 7: alt_switch("r", "s", "r", "r", "s", pibc_flag, pu_flag, pd_flag, pbdc_flag, pdsc_flag, podc_flag, pipc_flag, pv_flag, group_flag);
                                break;
                            case 8: alt_switch("r", "r", "s", "r", "s", pibc_flag, pu_flag, pd_flag, pbdc_flag, pdsc_flag, podc_flag, pipc_flag, pv_flag, group_flag);
                                break;
                            case 9: alt_switch("r", "s", "s", "r", "s", pibc_flag, pu_flag, pd_flag, pbdc_flag, pdsc_flag, podc_flag, pipc_flag, pv_flag, group_flag);
                                break;
                            case 10: alt_switch("r", "r", "r", "s", "s", pibc_flag, pu_flag, pd_flag, pbdc_flag, pdsc_flag, podc_flag, pipc_flag, pv_flag, group_flag);
                                break;


                        }
                    }

                }
            }
        }
        public void alt_switch(string pm, string pfc, string pfce, string pfcae, string pmc, string pibc, string pu, string pd, string pbdc, string pdsc, string podc, string pipc, string pv, int i)//对PM,PFC,PFCE,PFCAE,PMC等寄存器进行位移，并通过中介赋值，保证三种模式互不干扰
        {
            switch (ARS)
            {

                case 1: Alt_PM_value = Alt_PM_value_Act[i];
                    Alt_PFC_value = Alt_PFC_value_Act[i];
                    Alt_PFCE_value = Alt_PFCE_value_Act[i];
                    Alt_PFCAE_value = Alt_PFCAE_value_Act[i];
                    Alt_PMC_value = Alt_PMC_value_Act[i];


                    Alt_PIBC_value = Alt_PIBC_value_Act[i];
                    Alt_PU_value = Alt_PU_value_Act[i];
                    Alt_PD_value = Alt_PD_value_Act[i];
                    Alt_PBDC_value = Alt_PBDC_value_Act[i];
                    Alt_PDSC_value = Alt_PDSC_value_Act[i];
                    Alt_PODC_value = Alt_PODC_value_Act[i];
                    Alt_PIPC_value = Alt_PIPC_value_Act[i];

                    Alt_Port_value = Alt_Port_value_Act[i];
                    break;
                case 2: Alt_PM_value = Alt_PM_value_Res[i];
                    Alt_PFC_value = Alt_PFC_value_Res[i];
                    Alt_PFCE_value = Alt_PFCE_value_Res[i];
                    Alt_PFCAE_value = Alt_PFCAE_value_Res[i];
                    Alt_PMC_value = Alt_PMC_value_Res[i];

                    Alt_PIBC_value = Alt_PIBC_value_Res[i];
                    Alt_PU_value = Alt_PU_value_Res[i];
                    Alt_PD_value = Alt_PD_value_Res[i];
                    Alt_PBDC_value = Alt_PBDC_value_Res[i];
                    Alt_PDSC_value = Alt_PDSC_value_Res[i];
                    Alt_PODC_value = Alt_PODC_value_Res[i];
                    Alt_PIPC_value = Alt_PIPC_value_Res[i];

                    Alt_Port_value = Alt_Port_value_Res[i];
                    break;
                case 3: Alt_PM_value = Alt_PM_value_Sta[i];
                    Alt_PFC_value = Alt_PFC_value_Sta[i];
                    Alt_PFCE_value = Alt_PFCE_value_Sta[i];
                    Alt_PFCAE_value = Alt_PFCAE_value_Sta[i];
                    Alt_PMC_value = Alt_PMC_value_Sta[i];

                    Alt_PIBC_value = Alt_PIBC_value_Sta[i];
                    Alt_PU_value = Alt_PU_value_Sta[i];
                    Alt_PD_value = Alt_PD_value_Sta[i];
                    Alt_PBDC_value = Alt_PBDC_value_Sta[i];
                    Alt_PDSC_value = Alt_PDSC_value_Sta[i];
                    Alt_PODC_value = Alt_PODC_value_Sta[i];
                    Alt_PIPC_value = Alt_PIPC_value_Sta[i];

                    Alt_Port_value = Alt_Port_value_Sta[i];
                    break;
            }
            if (pm == "s") Alt_PM_value = Alt_PM_value | (1 << alt_number);
            if (pm == "r") Alt_PM_value = Alt_PM_value & ((1 << alt_number) ^ 65535);

            if (pfc == "s") Alt_PFC_value = Alt_PFC_value | (1 << alt_number);
            if (pfc == "r") Alt_PFC_value = Alt_PFC_value & ((1 << alt_number) ^ 65535);

            if (pfce == "s") Alt_PFCE_value = Alt_PFCE_value | (1 << alt_number);
            if (pfce == "r") Alt_PFCE_value = Alt_PFCE_value & ((1 << alt_number) ^ 65535);

            if (pfcae == "s") Alt_PFCAE_value = Alt_PFCAE_value | (1 << alt_number);
            if (pfcae == "r") Alt_PFCAE_value = Alt_PFCAE_value & ((1 << alt_number) ^ 65535);

            if (pmc == "s") Alt_PMC_value = Alt_PMC_value | (1 << alt_number);
            if (pmc == "r") Alt_PMC_value = Alt_PMC_value & ((1 << alt_number) ^ 65535);

            if (pibc == "s") Alt_PIBC_value = Alt_PIBC_value | (1 << alt_number);
            if (pibc == "r") Alt_PIBC_value = Alt_PIBC_value & ((1 << alt_number) ^ 65535);//Disable

            if (pu == "s") Alt_PU_value = Alt_PU_value | (1 << alt_number);
            if (pu == "r") Alt_PU_value = Alt_PU_value & ((1 << alt_number) ^ 65535);

            if (pd == "s") Alt_PD_value = Alt_PD_value | (1 << alt_number);
            if (pd == "r") Alt_PD_value = Alt_PD_value & ((1 << alt_number) ^ 65535);

            if (pbdc == "s") Alt_PBDC_value = Alt_PBDC_value | (1 << alt_number);
            if (pbdc == "r") Alt_PBDC_value = Alt_PBDC_value & ((1 << alt_number) ^ 65535);

            if (pdsc == "s") Alt_PDSC_value = Alt_PDSC_value | (1 << alt_number);
            if (pdsc == "r") Alt_PDSC_value = Alt_PDSC_value & ((1 << alt_number) ^ 65535);

            if (podc == "s") Alt_PODC_value = Alt_PODC_value | (1 << alt_number);
            if (podc == "r") Alt_PODC_value = Alt_PODC_value & ((1 << alt_number) ^ 65535);

            if (pipc == "s") Alt_PIPC_value = Alt_PIPC_value | (1 << alt_number);
            if (pipc == "r") Alt_PIPC_value = Alt_PIPC_value & ((1 << alt_number) ^ 65535);

            if (pv == "s") Alt_Port_value = Alt_Port_value | (1 << alt_number);
            if (pv == "r") Alt_Port_value = Alt_Port_value & ((1 << alt_number) ^ 65535);

            switch (ARS)
            {

                case 1: Alt_PM_value_Act[i] = Alt_PM_value;
                    Alt_PFC_value_Act[i] = Alt_PFC_value;
                    Alt_PFCE_value_Act[i] = Alt_PFCE_value;
                    Alt_PFCAE_value_Act[i] = Alt_PFCAE_value;
                    Alt_PMC_value_Act[i] = Alt_PMC_value;

                    Alt_PIBC_value_Act[i] = Alt_PIBC_value;
                    Alt_PU_value_Act[i] = Alt_PU_value;
                    Alt_PD_value_Act[i] = Alt_PD_value;
                    Alt_PBDC_value_Act[i] = Alt_PBDC_value;
                    Alt_PDSC_value_Act[i] = Alt_PDSC_value;
                    Alt_PODC_value_Act[i] = Alt_PODC_value;
                    Alt_PIPC_value_Act[i] = Alt_PIPC_value;

                    Alt_Port_value_Act[i] = Alt_Port_value;
                    break;
                case 2: Alt_PM_value_Res[i] = Alt_PM_value;
                    Alt_PFC_value_Res[i] = Alt_PFC_value;
                    Alt_PFCE_value_Res[i] = Alt_PFCE_value;
                    Alt_PFCAE_value_Res[i] = Alt_PFCAE_value;
                    Alt_PMC_value_Res[i] = Alt_PMC_value;

                    Alt_PIBC_value_Res[i] = Alt_PIBC_value;
                    Alt_PU_value_Res[i] = Alt_PU_value;
                    Alt_PD_value_Res[i] = Alt_PD_value;
                    Alt_PBDC_value_Res[i] = Alt_PBDC_value;
                    Alt_PDSC_value_Res[i] = Alt_PDSC_value;
                    Alt_PODC_value_Res[i] = Alt_PODC_value;
                    Alt_PIPC_value_Res[i] = Alt_PIPC_value;

                    Alt_Port_value_Res[i] = Alt_Port_value;
                    break;
                case 3: Alt_PM_value_Sta[i] = Alt_PM_value;
                    Alt_PFC_value_Sta[i] = Alt_PFC_value;
                    Alt_PFCE_value_Sta[i] = Alt_PFCE_value;
                    Alt_PFCAE_value_Sta[i] = Alt_PFCAE_value;
                    Alt_PMC_value_Sta[i] = Alt_PMC_value;

                    Alt_PIBC_value_Sta[i] = Alt_PIBC_value;
                    Alt_PU_value_Sta[i] = Alt_PU_value;
                    Alt_PD_value_Sta[i] = Alt_PD_value;
                    Alt_PBDC_value_Sta[i] = Alt_PBDC_value;
                    Alt_PDSC_value_Sta[i] = Alt_PDSC_value;
                    Alt_PODC_value_Sta[i] = Alt_PODC_value;
                    Alt_PIPC_value_Sta[i] = Alt_PIPC_value;

                    Alt_Port_value_Sta[i] = Alt_Port_value;
                    break;
            }

        }

        //******************************************************************************************************************************************************
        //                                             xml读写函数组
        //******************************************************************************************************************************************************
        public void Foreach_node(string Group_Number, string mode, string location)//遍历GPIO xml文件，确定用是否有必要增加节点
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
                if (node.Name == "FUN")
                {
                    XmlNodeList sxe = xe.ChildNodes;
                    if (sxe.Count != 0)
                    {
                        Node_Start_Flag = 1;
                        foreach (XmlNode node1 in sxe)//foreach group in FUN
                        {
                            XmlElement xe1 = (XmlElement)node1;
                            if ((xe1.GetAttribute("Group_Number") == Group_Number) && (xe1.GetAttribute("Mode") == mode))
                            {
                                Node_Change_Flag = 1;

                            }

                        }
                    }
                    else
                    { Node_Start_Flag = 0; }
                }
            }
            reader.Close();
            doc.Save(location);//"\\Pin100.xml"

        }
        public void return_list(string[,] list_array, int k)
        {


            for (int i = 1; i < k; i++)
            {
                this.listView1.BeginUpdate();
                ListViewItem lvi = new ListViewItem();
                if (list_array[i, 0] != "0")
                {
                    for (int j = 0; j < 12; j++)
                    {
                        if (j == 0)
                        {
                            lvi.Text = list_array[i, j];//端口
                            // add this code for changing linkLabel's linkColor
                            foreach (Control c in this.tabControl1.TabPages[1].Controls)
                            {
                                if (c is LinkLabel)
                                {
                                    LinkLabel linkLabel = (LinkLabel)c;
                                    if (linkLabel.Text == lvi.Text || lvi.Text == "P" + linkLabel.Text)
                                    {
                                        linkLabel.LinkColor = Color.Green;
                                    }
                                }
                                if (c is Label)
                                {
                                    Label label = (Label)c;
                                    if (label.Text == lvi.Text || lvi.Text == "P" + label.Text)
                                    {
                                        label.ForeColor = Color.Green;
                                    }
                                }
                            }
                            //

                        }
                        else lvi.SubItems.Add(list_array[i, j]); //模式
                    }
                    this.listView1.Items.Add(lvi);
                    this.listView1.EndUpdate();
                }
            }//XML文件写入
        }
        public void irq_return_list(string[,] list_array, int k)
        {


            for (int i = 0; i < k; i++)
            {
                this.listView2.BeginUpdate();
                ListViewItem lvi = new ListViewItem();
                for (int j = 0; j < 12; j++)
                {
                    if (j == 0) lvi.Text = list_array[i, j];//端口
                    else lvi.SubItems.Add(list_array[i, j]); //模式
                }
                this.listView2.Items.Add(lvi);
                this.listView2.EndUpdate();

            }
        }
        private void button1_Click_1(object sender, EventArgs e)//XML文件写入
        {
            if (!(checkBox33.Checked || checkBox34.Checked || checkBox35.Checked))
            {
                MessageBox.Show("还未选择模式，请选择模式");
            }
            else if (comboBox1.Text.Equals("Alternative") && !judgeRationButton())
            {
                MessageBox.Show("还未选择可选择功能，请选择功能");
            }
            else
            {
                if (Fun_Num != 0)
                {
                    ForeachPanelSelects(ref Fun_Num);
                }
                else
                {
                    alt_switch(pm_flag, "", "", "", "r", pibc_flag, pu_flag, pd_flag, pbdc_flag, pdsc_flag, podc_flag, "r", pv_flag, group_flag);//PM  PFC  PFCE  PFCAE  PMC  PIBC  PU  PD  PBDC  PDSC PODC PIPC PV普通模式下的赋值
                }
                Foreach_node(alt_group, alt_mode, pin_path);
                this.listView1.BeginUpdate();
                ListViewItem lvi = new ListViewItem();

                if (listView1.Items.Count > 0)
                {
                    for (int i = 0; i < listView1.Items.Count; i++)
                    {
                        if ((listView1.Items[i].SubItems[0].Text == textBox23.Text) && (listView1.Items[i].SubItems[1].Text == alt_mode))
                        {
                            listView1.Items.Remove(listView1.Items[i]);
                            foreach (Control c in this.tabControl1.TabPages[1].Controls)
                            {
                                if (c is LinkLabel)
                                {
                                    LinkLabel linkLabel = (LinkLabel)c;
                                    if (textBox23.Text == "P" + linkLabel.Text || textBox23.Text == linkLabel.Text)
                                    {
                                        linkLabel.LinkColor = Color.Blue;
                                    }
                                }
                                if (c is Label)
                                {
                                    Label label = (Label)c;
                                    if (label.Text == lvi.Text || lvi.Text == "P" + label.Text)
                                    {
                                        label.ForeColor = Color.Green;
                                    }
                                }
                            }
                        }


                    }

                }

                lvi.Text = textBox23.Text;//端口
                lvi.SubItems.Add(alt_mode); //模式
                foreach (Control c in this.tabControl1.TabPages[1].Controls)
                {
                    if (c is LinkLabel)
                    {
                        LinkLabel linkLabel = (LinkLabel)c;
                        if (lvi.Text == "P" + linkLabel.Text || lvi.Text == linkLabel.Text)
                        {
                            linkLabel.LinkColor = Color.Green;
                        }
                    }
                    if (c is Label)
                    {
                        Label label = (Label)c;
                        if (label.Text == lvi.Text || lvi.Text == "P" + label.Text)
                        {
                            label.ForeColor = Color.Green;
                        }
                    }
                }

                if (Fun_Num != 0)//alt&gpio
                {
                    lvi.SubItems.Add("ALT");
                    switch (Fun_Num)
                    {

                        case 1: lvi.SubItems.Add(textBox1.Text);
                            break;
                        case 2: lvi.SubItems.Add(textBox2.Text);
                            break;
                        case 3: lvi.SubItems.Add(textBox3.Text);
                            break;
                        case 4: lvi.SubItems.Add(textBox4.Text);
                            break;
                        case 5: lvi.SubItems.Add(textBox5.Text);
                            break;
                        case 6: lvi.SubItems.Add(textBox10.Text);
                            break;
                        case 7: lvi.SubItems.Add(textBox9.Text);
                            break;
                        case 8: lvi.SubItems.Add(textBox8.Text);
                            break;
                        case 9: lvi.SubItems.Add(textBox7.Text);
                            break;
                        case 10: lvi.SubItems.Add(textBox6.Text);
                            break;
                        default: lvi.SubItems.Add("");
                            break;
                    }
                    if (checkBox31.Checked == true) //IN
                    {
                        lvi.SubItems.Add("IN");//in
                        if (checkBox7.Checked == true) { lvi.SubItems.Add("Enable"); }//pibc enable
                        if (checkBox8.Checked == true) { lvi.SubItems.Add("Disable"); }//pibc Disable
                        if (checkBox25.Checked == true) { lvi.SubItems.Add("Enable"); }//pull-up
                        if (checkBox26.Checked == true) { lvi.SubItems.Add("Disable"); }
                        if (checkBox25.Checked == false && checkBox26.Checked == false) { lvi.SubItems.Add("Disable"); }
                        if (checkBox27.Checked == true) { lvi.SubItems.Add("Enable"); }//pull-down
                        if (checkBox28.Checked == true) { lvi.SubItems.Add("Disable"); }
                        if (checkBox27.Checked == false && checkBox28.Checked == false) { lvi.SubItems.Add("Disable"); }
                        if (pbdc_flag == "r") lvi.SubItems.Add("Disable"); //pbdc
                        if (pbdc_flag == "s") lvi.SubItems.Add("Enable"); //pbdc
                        if (pdsc_flag == "r") lvi.SubItems.Add("Disable");//pdsc
                        if (pdsc_flag == "s") lvi.SubItems.Add("Enable");//pdsc
                        if (podc_flag == "r") lvi.SubItems.Add("Disable"); //podc
                        if (podc_flag == "s") lvi.SubItems.Add("Enable"); //podc
                        if (pv_flag == "r") lvi.SubItems.Add("Low-Level");// input mode default : Low-Level
                        if (pv_flag == "s") lvi.SubItems.Add("High-Level");// input mode default : Low-Level
                    }
                    if (checkBox32.Checked == true)
                    {
                        lvi.SubItems.Add("OUT");
                        if (pibc_flag == "r") lvi.SubItems.Add("Disable"); //pibc
                        if (pibc_flag == "s") lvi.SubItems.Add("Enable"); //pibc
                        if (pu_flag == "r") lvi.SubItems.Add("Disable");//pull-up
                        if (pu_flag == "s") lvi.SubItems.Add("Enable");//pull-up
                        if (pd_flag == "r") lvi.SubItems.Add("Disable"); //pull-down
                        if (pd_flag == "s") lvi.SubItems.Add("Enable"); //pull-down
                        if (checkBox1.Checked == true) { lvi.SubItems.Add("Enable"); }//pbdc
                        if (checkBox2.Checked == true) { lvi.SubItems.Add("Disable"); }
                        if (checkBox1.Checked == false && checkBox2.Checked == false) { lvi.SubItems.Add("Disable"); }
                        if (checkBox4.Checked == true) { lvi.SubItems.Add("40Mhz"); }//pdsc
                        if (checkBox3.Checked == true) { lvi.SubItems.Add("10Mhz"); }
                        if (checkBox4.Checked == false && checkBox3.Checked == false) { lvi.SubItems.Add("10Mhz"); }
                        if (checkBox6.Checked == true) { lvi.SubItems.Add("Push-Pull"); }//podc
                        if (checkBox5.Checked == true) { lvi.SubItems.Add("Open-Drain"); }
                        if (checkBox6.Checked == false && checkBox5.Checked == false) { lvi.SubItems.Add("Open-Drain"); }
                        if (checkBox10.Checked == true) { lvi.SubItems.Add("High-Level"); }//pv
                        if (checkBox9.Checked == true) { lvi.SubItems.Add("Low-Level"); }
                    }
                    if (checkBox32.Checked == false && checkBox31.Checked == false)
                    {
                        lvi.SubItems.Add("IN");//in
                        lvi.SubItems.Add("Enable");//pibc
                        lvi.SubItems.Add("Disable");//pull-up
                        lvi.SubItems.Add("Disable");//pull-down
                        lvi.SubItems.Add("Disable"); //pbdc
                        lvi.SubItems.Add("Disable");//pdsc
                        lvi.SubItems.Add("Disable"); //podc
                        lvi.SubItems.Add("Low-Level");// pv
                    }
                }
                else
                {
                    lvi.SubItems.Add("GPIO");
                    lvi.SubItems.Add("NONE");

                    if (checkBox31.Checked == true) //IN/OUT
                    {
                        lvi.SubItems.Add("IN");//in
                        if (checkBox7.Checked == true) { lvi.SubItems.Add("Enable"); }//pibc enable
                        if (checkBox8.Checked == true) { lvi.SubItems.Add("Disable"); }//pibc Disable
                        if (checkBox25.Checked == true) { lvi.SubItems.Add("Enable"); }//pull-up
                        if (checkBox26.Checked == true) { lvi.SubItems.Add("Disable"); }
                        if (checkBox25.Checked == false && checkBox26.Checked == false) { lvi.SubItems.Add("Disable"); }
                        if (checkBox27.Checked == true) { lvi.SubItems.Add("Enable"); }//pull-down
                        if (checkBox28.Checked == true) { lvi.SubItems.Add("Disable"); }
                        if (checkBox27.Checked == false && checkBox28.Checked == false) { lvi.SubItems.Add("Disable"); }
                        if (pbdc_flag == "r") lvi.SubItems.Add("Disable"); //pbdc
                        if (pbdc_flag == "s") lvi.SubItems.Add("Enable"); //pbdc
                        if (pdsc_flag == "r") lvi.SubItems.Add("Disable");//pdsc
                        if (pdsc_flag == "s") lvi.SubItems.Add("Enable");//pdsc
                        if (podc_flag == "r") lvi.SubItems.Add("Disable"); //podc
                        if (podc_flag == "s") lvi.SubItems.Add("Enable"); //podc
                        lvi.SubItems.Add("Low-Level");// input mode cannot have port value
                    }

                    if (checkBox32.Checked == true)
                    {
                        lvi.SubItems.Add("OUT");
                        if (pibc_flag == "r") lvi.SubItems.Add("Disable"); //pibc
                        if (pibc_flag == "s") lvi.SubItems.Add("Enable"); //pibc
                        if (pu_flag == "r") lvi.SubItems.Add("Disable");//pull-up
                        if (pu_flag == "s") lvi.SubItems.Add("Enable");//pull-up
                        if (pd_flag == "r") lvi.SubItems.Add("Disable"); //pull-down
                        if (pd_flag == "s") lvi.SubItems.Add("Enable"); //pull-down
                        if (checkBox1.Checked == true) { lvi.SubItems.Add("Enable"); }//pbdc
                        if (checkBox2.Checked == true) { lvi.SubItems.Add("Disable"); }
                        if (checkBox4.Checked == true) { lvi.SubItems.Add("40Mhz"); }//pdsc
                        if (checkBox3.Checked == true) { lvi.SubItems.Add("10Mhz"); }
                        if (checkBox6.Checked == true) { lvi.SubItems.Add("Push-Pull"); }//podc
                        if (checkBox5.Checked == true) { lvi.SubItems.Add("Open-Drain"); }
                        if (checkBox10.Checked == true) { lvi.SubItems.Add("High-Level"); }//pv
                        if (checkBox9.Checked == true) { lvi.SubItems.Add("Low-Level"); }
                    }
                    if (checkBox32.Checked == false && checkBox31.Checked == false)
                    {
                        lvi.SubItems.Add("IN");//in
                        lvi.SubItems.Add("Enable");//pibc
                        lvi.SubItems.Add("Disable");//pull-up
                        lvi.SubItems.Add("Disable");//pull-down
                        lvi.SubItems.Add("Disable"); //pbdc
                        lvi.SubItems.Add("Disable");//pdsc
                        lvi.SubItems.Add("Disable"); //podc
                        lvi.SubItems.Add("Low-Level");// input mode cannot have port value
                    }
                }
                this.listView1.Items.Add(lvi);
                this.listView1.EndUpdate();
                if (Node_Start_Flag == 1)
                {
                    if (Node_Change_Flag == 1)
                    {
                        switch (ARS)
                        {
                            case 1: gpio.change_node("PM", alt_group, Alt_PM_value_Act[group_flag], alt_mode, ref pin_path);
                                gpio.change_node("PFCE", alt_group, Alt_PFCE_value_Act[group_flag], alt_mode, ref pin_path);
                                gpio.change_node("PFCAE", alt_group, Alt_PFCAE_value_Act[group_flag], alt_mode, ref pin_path);
                                gpio.change_node("PFC", alt_group, Alt_PFC_value_Act[group_flag], alt_mode, ref pin_path);
                                gpio.change_node("PMC", alt_group, Alt_PMC_value_Act[group_flag], alt_mode, ref pin_path);
                                gpio.change_node("PIBC", alt_group, Alt_PIBC_value_Act[group_flag], alt_mode, ref pin_path);
                                gpio.change_node("PU", alt_group, Alt_PU_value_Act[group_flag], alt_mode, ref pin_path);
                                gpio.change_node("PD", alt_group, Alt_PD_value_Act[group_flag], alt_mode, ref pin_path);
                                gpio.change_node("PBDC", alt_group, Alt_PBDC_value_Act[group_flag], alt_mode, ref pin_path);
                                gpio.change_node("PDSC", alt_group, Alt_PDSC_value_Act[group_flag], alt_mode, ref pin_path);
                                gpio.change_node("PODC", alt_group, Alt_PODC_value_Act[group_flag], alt_mode, ref pin_path);
                                gpio.change_node("PIPC", alt_group, Alt_PIPC_value_Act[group_flag], alt_mode, ref pin_path);
                                gpio.change_node("PV", alt_group, Alt_Port_value_Act[group_flag], alt_mode, ref pin_path);
                                break;
                            case 2: gpio.change_node("PM", alt_group, Alt_PM_value_Res[group_flag], alt_mode, ref pin_path);
                                gpio.change_node("PFCE", alt_group, Alt_PFCE_value_Res[group_flag], alt_mode, ref pin_path);
                                gpio.change_node("PFCAE", alt_group, Alt_PFCAE_value_Res[group_flag], alt_mode, ref pin_path);
                                gpio.change_node("PFC", alt_group, Alt_PFC_value_Res[group_flag], alt_mode, ref pin_path);
                                gpio.change_node("PMC", alt_group, Alt_PMC_value_Res[group_flag], alt_mode, ref pin_path);
                                gpio.change_node("PIBC", alt_group, Alt_PIBC_value_Res[group_flag], alt_mode, ref pin_path);
                                gpio.change_node("PU", alt_group, Alt_PU_value_Res[group_flag], alt_mode, ref pin_path);
                                gpio.change_node("PD", alt_group, Alt_PD_value_Res[group_flag], alt_mode, ref pin_path);
                                gpio.change_node("PBDC", alt_group, Alt_PBDC_value_Res[group_flag], alt_mode, ref pin_path);
                                gpio.change_node("PDSC", alt_group, Alt_PDSC_value_Res[group_flag], alt_mode, ref pin_path);
                                gpio.change_node("PODC", alt_group, Alt_PODC_value_Res[group_flag], alt_mode, ref pin_path);
                                gpio.change_node("PIPC", alt_group, Alt_PIPC_value_Res[group_flag], alt_mode, ref pin_path);
                                gpio.change_node("PV", alt_group, Alt_Port_value_Res[group_flag], alt_mode, ref pin_path);
                                break;
                            case 3: gpio.change_node("PM", alt_group, Alt_PM_value_Sta[group_flag], alt_mode, ref pin_path);
                                gpio.change_node("PFCE", alt_group, Alt_PFCE_value_Sta[group_flag], alt_mode, ref pin_path);
                                gpio.change_node("PFCAE", alt_group, Alt_PFCAE_value_Sta[group_flag], alt_mode, ref pin_path);
                                gpio.change_node("PFC", alt_group, Alt_PFC_value_Sta[group_flag], alt_mode, ref pin_path);
                                gpio.change_node("PMC", alt_group, Alt_PMC_value_Sta[group_flag], alt_mode, ref pin_path);
                                gpio.change_node("PIBC", alt_group, Alt_PIBC_value_Sta[group_flag], alt_mode, ref pin_path);
                                gpio.change_node("PU", alt_group, Alt_PU_value_Sta[group_flag], alt_mode, ref pin_path);
                                gpio.change_node("PD", alt_group, Alt_PD_value_Sta[group_flag], alt_mode, ref pin_path);
                                gpio.change_node("PBDC", alt_group, Alt_PBDC_value_Sta[group_flag], alt_mode, ref pin_path);
                                gpio.change_node("PDSC", alt_group, Alt_PDSC_value_Sta[group_flag], alt_mode, ref pin_path);
                                gpio.change_node("PODC", alt_group, Alt_PODC_value_Sta[group_flag], alt_mode, ref pin_path);
                                gpio.change_node("PIPC", alt_group, Alt_PIPC_value_Sta[group_flag], alt_mode, ref pin_path);
                                gpio.change_node("PV", alt_group, Alt_Port_value_Sta[group_flag], alt_mode, ref pin_path);
                                break;
                        }
                    }
                    else
                    {
                        switch (ARS)
                        {
                            case 1: gpio.Add_Node(alt_mode, alt_group, ref pin_path);
                                gpio.change_node("PM", alt_group, Alt_PM_value_Act[group_flag], alt_mode, ref pin_path);
                                gpio.change_node("PFCE", alt_group, Alt_PFCE_value_Act[group_flag], alt_mode, ref pin_path);
                                gpio.change_node("PFCAE", alt_group, Alt_PFCAE_value_Act[group_flag], alt_mode, ref pin_path);
                                gpio.change_node("PFC", alt_group, Alt_PFC_value_Act[group_flag], alt_mode, ref pin_path);
                                gpio.change_node("PMC", alt_group, Alt_PMC_value_Act[group_flag], alt_mode, ref pin_path);
                                gpio.change_node("PIBC", alt_group, Alt_PIBC_value_Act[group_flag], alt_mode, ref pin_path);
                                gpio.change_node("PU", alt_group, Alt_PU_value_Act[group_flag], alt_mode, ref pin_path);
                                gpio.change_node("PD", alt_group, Alt_PD_value_Act[group_flag], alt_mode, ref pin_path);
                                gpio.change_node("PBDC", alt_group, Alt_PBDC_value_Act[group_flag], alt_mode, ref pin_path);
                                gpio.change_node("PDSC", alt_group, Alt_PDSC_value_Act[group_flag], alt_mode, ref pin_path);
                                gpio.change_node("PODC", alt_group, Alt_PODC_value_Act[group_flag], alt_mode, ref pin_path);
                                gpio.change_node("PIPC", alt_group, Alt_PIPC_value_Act[group_flag], alt_mode, ref pin_path);
                                gpio.change_node("PV", alt_group, Alt_Port_value_Act[group_flag], alt_mode, ref pin_path);
                                break;
                            case 2: gpio.Add_Node(alt_mode, alt_group, ref pin_path);
                                gpio.change_node("PM", alt_group, Alt_PM_value_Res[group_flag], alt_mode, ref pin_path);
                                gpio.change_node("PFCE", alt_group, Alt_PFCE_value_Res[group_flag], alt_mode, ref pin_path);
                                gpio.change_node("PFCAE", alt_group, Alt_PFCAE_value_Res[group_flag], alt_mode, ref pin_path);
                                gpio.change_node("PFC", alt_group, Alt_PFC_value_Res[group_flag], alt_mode, ref pin_path);
                                gpio.change_node("PMC", alt_group, Alt_PMC_value_Res[group_flag], alt_mode, ref pin_path);
                                gpio.change_node("PIBC", alt_group, Alt_PIBC_value_Res[group_flag], alt_mode, ref pin_path);
                                gpio.change_node("PU", alt_group, Alt_PU_value_Res[group_flag], alt_mode, ref pin_path);
                                gpio.change_node("PD", alt_group, Alt_PD_value_Res[group_flag], alt_mode, ref pin_path);
                                gpio.change_node("PBDC", alt_group, Alt_PBDC_value_Res[group_flag], alt_mode, ref pin_path);
                                gpio.change_node("PDSC", alt_group, Alt_PDSC_value_Res[group_flag], alt_mode, ref pin_path);
                                gpio.change_node("PODC", alt_group, Alt_PODC_value_Res[group_flag], alt_mode, ref pin_path);
                                gpio.change_node("PIPC", alt_group, Alt_PIPC_value_Res[group_flag], alt_mode, ref pin_path);
                                gpio.change_node("PV", alt_group, Alt_Port_value_Res[group_flag], alt_mode, ref pin_path);
                                break;
                            case 3: gpio.Add_Node(alt_mode, alt_group, ref pin_path);
                                gpio.change_node("PM", alt_group, Alt_PM_value_Sta[group_flag], alt_mode, ref pin_path);
                                gpio.change_node("PFCE", alt_group, Alt_PFCE_value_Sta[group_flag], alt_mode, ref pin_path);
                                gpio.change_node("PFCAE", alt_group, Alt_PFCAE_value_Sta[group_flag], alt_mode, ref pin_path);
                                gpio.change_node("PFC", alt_group, Alt_PFC_value_Sta[group_flag], alt_mode, ref pin_path);
                                gpio.change_node("PMC", alt_group, Alt_PMC_value_Sta[group_flag], alt_mode, ref pin_path);
                                gpio.change_node("PIBC", alt_group, Alt_PIBC_value_Sta[group_flag], alt_mode, ref pin_path);
                                gpio.change_node("PU", alt_group, Alt_PU_value_Sta[group_flag], alt_mode, ref pin_path);
                                gpio.change_node("PD", alt_group, Alt_PD_value_Sta[group_flag], alt_mode, ref pin_path);
                                gpio.change_node("PBDC", alt_group, Alt_PBDC_value_Sta[group_flag], alt_mode, ref pin_path);
                                gpio.change_node("PDSC", alt_group, Alt_PDSC_value_Sta[group_flag], alt_mode, ref pin_path);
                                gpio.change_node("PODC", alt_group, Alt_PODC_value_Sta[group_flag], alt_mode, ref pin_path);
                                gpio.change_node("PIPC", alt_group, Alt_PIPC_value_Sta[group_flag], alt_mode, ref pin_path);
                                gpio.change_node("PV", alt_group, Alt_Port_value_Sta[group_flag], alt_mode, ref pin_path);
                                break;
                        }
                    }
                }
                else
                {
                    switch (ARS)
                    {
                        case 1: gpio.Add_Node(alt_mode, alt_group, ref pin_path);
                            gpio.change_node("PM", alt_group, Alt_PM_value_Act[group_flag], alt_mode, ref pin_path);
                            gpio.change_node("PFCE", alt_group, Alt_PFCE_value_Act[group_flag], alt_mode, ref pin_path);
                            gpio.change_node("PFCAE", alt_group, Alt_PFCAE_value_Act[group_flag], alt_mode, ref pin_path);
                            gpio.change_node("PFC", alt_group, Alt_PFC_value_Act[group_flag], alt_mode, ref pin_path);
                            gpio.change_node("PMC", alt_group, Alt_PMC_value_Act[group_flag], alt_mode, ref pin_path);
                            gpio.change_node("PIBC", alt_group, Alt_PIBC_value_Act[group_flag], alt_mode, ref pin_path);
                            gpio.change_node("PU", alt_group, Alt_PU_value_Act[group_flag], alt_mode, ref pin_path);
                            gpio.change_node("PD", alt_group, Alt_PD_value_Act[group_flag], alt_mode, ref pin_path);
                            gpio.change_node("PBDC", alt_group, Alt_PBDC_value_Act[group_flag], alt_mode, ref pin_path);
                            gpio.change_node("PDSC", alt_group, Alt_PDSC_value_Act[group_flag], alt_mode, ref pin_path);
                            gpio.change_node("PODC", alt_group, Alt_PODC_value_Act[group_flag], alt_mode, ref pin_path);
                            gpio.change_node("PIPC", alt_group, Alt_PIPC_value_Act[group_flag], alt_mode, ref pin_path);
                            gpio.change_node("PV", alt_group, Alt_Port_value_Act[group_flag], alt_mode, ref pin_path);
                            break;
                        case 2: gpio.Add_Node(alt_mode, alt_group, ref pin_path);
                            gpio.change_node("PM", alt_group, Alt_PM_value_Res[group_flag], alt_mode, ref pin_path);
                            gpio.change_node("PFCE", alt_group, Alt_PFCE_value_Res[group_flag], alt_mode, ref pin_path);
                            gpio.change_node("PFCAE", alt_group, Alt_PFCAE_value_Res[group_flag], alt_mode, ref pin_path);
                            gpio.change_node("PFC", alt_group, Alt_PFC_value_Res[group_flag], alt_mode, ref pin_path);
                            gpio.change_node("PMC", alt_group, Alt_PMC_value_Res[group_flag], alt_mode, ref pin_path);
                            gpio.change_node("PIBC", alt_group, Alt_PIBC_value_Res[group_flag], alt_mode, ref pin_path);
                            gpio.change_node("PU", alt_group, Alt_PU_value_Res[group_flag], alt_mode, ref pin_path);
                            gpio.change_node("PD", alt_group, Alt_PD_value_Res[group_flag], alt_mode, ref pin_path);
                            gpio.change_node("PBDC", alt_group, Alt_PBDC_value_Res[group_flag], alt_mode, ref pin_path);
                            gpio.change_node("PDSC", alt_group, Alt_PDSC_value_Res[group_flag], alt_mode, ref pin_path);
                            gpio.change_node("PODC", alt_group, Alt_PODC_value_Res[group_flag], alt_mode, ref pin_path);
                            gpio.change_node("PIPC", alt_group, Alt_PIPC_value_Res[group_flag], alt_mode, ref pin_path);
                            gpio.change_node("PV", alt_group, Alt_Port_value_Res[group_flag], alt_mode, ref pin_path);
                            break;
                        case 3: gpio.Add_Node(alt_mode, alt_group, ref pin_path);
                            gpio.change_node("PM", alt_group, Alt_PM_value_Sta[group_flag], alt_mode, ref pin_path);
                            gpio.change_node("PFCE", alt_group, Alt_PFCE_value_Sta[group_flag], alt_mode, ref pin_path);
                            gpio.change_node("PFCAE", alt_group, Alt_PFCAE_value_Sta[group_flag], alt_mode, ref pin_path);
                            gpio.change_node("PFC", alt_group, Alt_PFC_value_Sta[group_flag], alt_mode, ref pin_path);
                            gpio.change_node("PMC", alt_group, Alt_PMC_value_Sta[group_flag], alt_mode, ref pin_path);
                            gpio.change_node("PIBC", alt_group, Alt_PIBC_value_Sta[group_flag], alt_mode, ref pin_path);
                            gpio.change_node("PU", alt_group, Alt_PU_value_Sta[group_flag], alt_mode, ref pin_path);
                            gpio.change_node("PD", alt_group, Alt_PD_value_Sta[group_flag], alt_mode, ref pin_path);
                            gpio.change_node("PBDC", alt_group, Alt_PBDC_value_Sta[group_flag], alt_mode, ref pin_path);
                            gpio.change_node("PDSC", alt_group, Alt_PDSC_value_Sta[group_flag], alt_mode, ref pin_path);
                            gpio.change_node("PODC", alt_group, Alt_PODC_value_Sta[group_flag], alt_mode, ref pin_path);
                            gpio.change_node("PIPC", alt_group, Alt_PIPC_value_Sta[group_flag], alt_mode, ref pin_path);
                            gpio.change_node("PV", alt_group, Alt_Port_value_Sta[group_flag], alt_mode, ref pin_path);
                            break;
                    }

                }
                gpio.delete_list_node(lvi.SubItems[0].Text, lvi.SubItems[1].Text, lvi.SubItems[2].Text, lvi.SubItems[3].Text, lvi.SubItems[4].Text, lvi.SubItems[5].Text, lvi.SubItems[6].Text, lvi.SubItems[7].Text, lvi.SubItems[8].Text, lvi.SubItems[9].Text, lvi.SubItems[10].Text, lvi.SubItems[11].Text, ref pin_path, alt_group, alt_number);//group:string   number:int
                checkBox33.Checked = false;
                checkBox34.Checked = false;
                checkBox35.Checked = false;

                addInterrupt(lvi.SubItems[3].Text, interrupt_flag);//直接在Pin界面配置中断设置

                Node_Change_Flag = 0;
            }
        }


        //******************************************************************************************************************************************************
        //                                        功能选择按键组，5 in，5 out
        //******************************************************************************************************************************************************
        private void radioButton1_CheckedChanged(object sender, EventArgs e)//IN1
        {
            if (radioButton1.Checked == true)
                ForeachPanelControls("radioButton1");
            Fun_Num = 1;
            if (pipc_flag == "r")
            {
                checkBox32.Enabled = false;
                groupBox3.Enabled = true;
                groupBox5.Enabled = false;

                checkBox31.Checked = true;
                checkBox32.Checked = false;
            }
            else
            {
                checkBox32.Enabled = false;
                checkBox31.Checked = true;
                groupBox3.Enabled = true;
                groupBox5.Enabled = true;
            }


        }

        private void radioButton9_CheckedChanged(object sender, EventArgs e)//IN2
        {
            if (radioButton9.Checked == true)
                ForeachPanelControls("radioButton9");
            Fun_Num = 2;
            if (pipc_flag == "r")
            {
                checkBox32.Enabled = false;
                groupBox3.Enabled = true;
                groupBox5.Enabled = false;
                checkBox31.Checked = true;
                checkBox32.Checked = false;
            }
            else
            {
                checkBox32.Enabled = false;
                checkBox31.Checked = true;
                groupBox3.Enabled = true;
                groupBox5.Enabled = true;
            }

        }

        private void radioButton8_CheckedChanged(object sender, EventArgs e)//IN3
        {
            if (radioButton8.Checked == true)
                ForeachPanelControls("radioButton8");
            Fun_Num = 3;
            if (pipc_flag=="r")
            {
                checkBox32.Enabled = false;
                groupBox3.Enabled = true;
                groupBox5.Enabled = false;

                checkBox31.Checked = true;
                checkBox32.Checked = false;
            }
            else
            {
                checkBox32.Enabled = false;
                checkBox31.Checked = true;
                groupBox3.Enabled = true;
                groupBox5.Enabled = true;
            }
        }

        private void radioButton7_CheckedChanged(object sender, EventArgs e)//IN4
        {
            if (radioButton7.Checked == true)
                ForeachPanelControls("radioButton7");
            Fun_Num = 4;
            if (pipc_flag == "r")
            {
                checkBox32.Enabled = false;
                groupBox3.Enabled = true;
                groupBox5.Enabled = false;

                checkBox31.Checked = true;
                checkBox32.Checked = false;
            }
            else
            {
                checkBox32.Enabled = false;
                checkBox31.Checked = true;
                groupBox3.Enabled = true;
                groupBox5.Enabled = true;
            }
        }

        private void radioButton6_CheckedChanged(object sender, EventArgs e)//IN5
        {
            if (radioButton6.Checked == true)
                ForeachPanelControls("radioButton6");
            if (pipc_flag == "r")
            {
                Fun_Num = 5;
                checkBox32.Enabled = false;
                groupBox3.Enabled = true;
                groupBox5.Enabled = false;

                checkBox31.Checked = true;
                checkBox32.Checked = false;
            }
            else
            {
                checkBox32.Enabled = false;
                checkBox31.Checked = true;
                groupBox3.Enabled = true;
                groupBox5.Enabled = true;
            }
        }

        private void radioButton5_CheckedChanged(object sender, EventArgs e)//OUT1
        {
            if (radioButton5.Checked == true)
                ForeachPanelControls("radioButton5");
            Fun_Num = 6;
            if (pipc_flag=="r")
            {
                checkBox31.Enabled = false;
                groupBox3.Enabled = false;
                groupBox5.Enabled = true;

                checkBox32.Checked = true;
                checkBox31.Checked = false;
            }
            else
            {
                checkBox31.Enabled = false;
                checkBox32.Checked = true;
                groupBox3.Enabled = true;
                groupBox5.Enabled = true;
            }
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)//OUT2
        {
            if (radioButton4.Checked == true)
                ForeachPanelControls("radioButton4");
            Fun_Num = 7;
            if (pipc_flag == "r")
            {
                checkBox31.Enabled = false;
                groupBox3.Enabled = false;
                groupBox5.Enabled = true;

                checkBox32.Checked = true;
                checkBox31.Checked = false;
            }
            else
            {
                checkBox31.Enabled = false;
                checkBox32.Checked = true;
                groupBox3.Enabled = true;
                groupBox5.Enabled = true;
            }
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)//OUT3
        {
            if (radioButton3.Checked == true)
                ForeachPanelControls("radioButton3");
            Fun_Num = 8;
            if (pipc_flag == "r")
            {
                checkBox31.Enabled = false;
                groupBox3.Enabled = false;
                groupBox5.Enabled = true;

                checkBox32.Checked = true;
                checkBox31.Checked = false;
            }
            else
            {
                checkBox31.Enabled = false;
                checkBox32.Checked = true;
                groupBox3.Enabled = true;
                groupBox5.Enabled = true;
            }
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)//OUT4
        {
            if (radioButton2.Checked == true)
                ForeachPanelControls("radioButton2");
            Fun_Num = 9;
            if (pipc_flag == "r")
            {
                checkBox31.Enabled = false;
                groupBox3.Enabled = false;
                groupBox5.Enabled = true;

                checkBox32.Checked = true;
                checkBox31.Checked = false;
            }
            else
            {
                checkBox31.Enabled = false;
                checkBox32.Checked = true;
                groupBox3.Enabled = true;
                groupBox5.Enabled = true;
            }
        }

        private void radioButton10_CheckedChanged(object sender, EventArgs e)//OUT5
        {
            if (radioButton10.Checked == true)
                ForeachPanelControls("radioButton10");
            Fun_Num = 10;
            if (pipc_flag == "r")
            {
                checkBox31.Enabled = false;
                groupBox3.Enabled = false;
                groupBox5.Enabled = true;

                checkBox32.Checked = true;
                checkBox31.Checked = false;
            }
            else
            {
                checkBox31.Enabled = false;
                checkBox32.Checked = true;
                groupBox3.Enabled = true;
                groupBox5.Enabled = true;
            }
        }

        private void checkBox35_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox35.Checked == true)
            {
                checkBox34.Checked = false;
                checkBox33.Checked = false;
                alt_mode = "ACTIVE";//A,R,S
                ARS = 1;

            }
        }

        private void checkBox34_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox34.Checked == true)
            {
                checkBox33.Checked = false;
                checkBox35.Checked = false;
                alt_mode = "RESET";//A,R,S
                ARS = 2;

            }
        }

        private void checkBox33_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox33.Checked == true)
            {
                checkBox34.Checked = false;
                checkBox35.Checked = false;
                alt_mode = "STANDBY";//A,R,S
                ARS = 3;

            }
        }





        //******************************************************************************************************************************************************
        //                                                           调用ALT设置函数
        //******************************************************************************************************************************************************

        private void linkLabel29_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)//传入端口名称、端口组号、端口号、模式（在模式已选择的前提下）
        {


            alt_fun_disp("P9_3", "9", 3);

        }

        private void linkLabel30_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

            alt_fun_disp("P9_4", "9", 4);

        }

        private void linkLabel31_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {


            alt_fun_disp("P9_5", "9", 5);

        }

        private void linkLabel32_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {


            alt_fun_disp("P9_6", "9", 6);

        }

        private void linkLabel4_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {



            alt_fun_disp("P0_0", "0", 0);


        }

        private void linkLabel5_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {



            alt_fun_disp("P0_1", "0", 1);


        }

        private void linkLabel6_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {


            alt_fun_disp("P0_2", "0", 2);

        }

        private void linkLabel7_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {


            alt_fun_disp("P0_3", "0", 3);

        }

        private void linkLabel9_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

            alt_fun_disp("P0_4", "0", 4);


        }

        private void linkLabel10_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {


            alt_fun_disp("P0_5", "0", 5);

        }

        private void linkLabel11_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {


            alt_fun_disp("P0_6", "0", 6);

        }

        private void linkLabel43_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {


            alt_fun_disp("P10_0", "10", 0);

        }

        private void linkLabel44_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

            alt_fun_disp("P10_1", "10", 1);

        }

        private void linkLabel33_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {


            alt_fun_disp("P10_2", "10", 2);

        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {


            alt_fun_disp("P10_3", "10", 3);


        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {


            alt_fun_disp("P10_4", "10", 4);

        }

        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

            alt_fun_disp("P10_5", "10", 5);

        }

        private void linkLabel47_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {


            alt_fun_disp("P10_6", "10", 6);

        }

        private void linkLabel66_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {


            alt_fun_disp("P10_7", "10", 7);

        }

        private void linkLabel34_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {


            alt_fun_disp("P10_8", "10", 8);

        }

        private void linkLabel38_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {


            alt_fun_disp("P10_9", "10", 9);

        }

        private void linkLabel35_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {


            alt_fun_disp("P10_10", "10", 10);

        }

        private void linkLabel37_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {


            alt_fun_disp("P10_11", "10", 11);

        }

        private void linkLabel36_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

            alt_fun_disp("P10_12", "10", 12);

        }

        private void linkLabel39_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {


            alt_fun_disp("P10_13", "10", 13);

        }

        private void linkLabel46_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {


            alt_fun_disp("P10_14", "10", 14);

        }

        private void linkLabel13_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {


            alt_fun_disp("P8_2", "8", 2);

        }

        private void linkLabel28_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

            alt_fun_disp("P9_1", "9", 1);

        }

        private void linkLabel17_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

            alt_fun_disp("P9_2", "9", 2);

        }

        private void linkLabel27_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {


            alt_fun_disp("P9_0", "9", 0);

        }

        private void linkLabel26_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

            alt_fun_disp("AP0_0", "30", 0);

        }

        private void linkLabel25_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {


            alt_fun_disp("AP0_1", "30", 1);

        }

        private void linkLabel24_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

            alt_fun_disp("AP0_2", "30", 2);

        }

        private void linkLabel23_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

            alt_fun_disp("AP0_3", "30", 3);

        }

        private void linkLabel22_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {


            alt_fun_disp("AP0_4", "30", 4);

        }

        private void linkLabel21_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

            alt_fun_disp("AP0_5", "30", 5);

        }

        private void linkLabel20_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {


            alt_fun_disp("AP0_6", "30", 6);

        }

        private void linkLabel16_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

            alt_fun_disp("JP0_3", "20", 3);


        }

        private void linkLabel15_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

            alt_fun_disp("JP0_4", "20", 4);

        }
        //******************************************************************************************************************************************************
        //                                        功能选择按键组，5 in，5 out
        //******************************************************************************************************************************************************
        private void linkLabel14_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {


            alt_fun_disp("JP0_5", "20", 5);

        }

        private void linkLabel61_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

            alt_fun_disp("JP0_1", "20", 1);

        }

        private void linkLabel59_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {


            alt_fun_disp("JP0_0", "20", 0);

        }

        private void linkLabel60_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {


            alt_fun_disp("JP0_2", "20", 2);

        }

        /*       private void linkLabel52_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
               {


                   alt_fun_disp("P8_0", "8", 0);

               }
         * */

        private void linkLabel51_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

            alt_fun_disp("P0_10", "0", 10);

        }

        private void linkLabel49_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {


            alt_fun_disp("P0_9", "0", 9);

        }

        private void linkLabel50_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {


            alt_fun_disp("P0_8", "0", 8);

        }

        private void linkLabel40_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {


            alt_fun_disp("P0_7", "0", 7);

        }

        /*        private void linkLabel63_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
                {


                    alt_fun_disp("P8_6", "8", 6);


                }
         * */



        private void checkBox25_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox25.Checked == true)
            {
                checkBox26.Checked = false;
                pu_flag = "s";
            }
            else
            {
                pu_flag = "r";

            }

        }

        private void checkBox26_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox26.Checked == true)
            {
                checkBox25.Checked = false;
                pu_flag = "r";
            }
            else
            {
                pu_flag = "s";

            }
        }

        private void checkBox27_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox27.Checked == true)
            {
                checkBox28.Checked = false;
                pd_flag = "s";
            }
            else
            {
                pd_flag = "r";

            }

        }

        private void checkBox28_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox28.Checked == true)
            {
                checkBox27.Checked = false;
                pd_flag = "r";
            }
            else
            {
                pd_flag = "s";

            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {
                checkBox2.Checked = false;
                pbdc_flag = "s";

                groupBox3.Enabled = true;
            }
            else
            {
                pbdc_flag = "r";
                checkBox31.Checked = false;
                checkBox8.Checked = true;
                checkBox26.Checked = true;
                checkBox28.Checked = true;
                groupBox3.Enabled = false;
                checkBox31.Enabled = false;
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked == true)
            {
                checkBox1.Checked = false;
                pbdc_flag = "r";
            }
            else
            {
                pbdc_flag = "s";
                groupBox1.Enabled = true;
                groupBox2.Enabled = true;

            }
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox4.Checked == true)
            {
                checkBox3.Checked = false;
                pdsc_flag = "s";
            }
            else
            {
                pdsc_flag = "r";

            }
        }


        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox3.Checked == true)
            {
                checkBox4.Checked = false;
                pdsc_flag = "r";
            }
            else
            {
                pdsc_flag = "s";

            }
        }
        private void checkBox6_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox6.Checked == true)
            {
                checkBox5.Checked = false;
                podc_flag = "r";
            }
            else
            {
                podc_flag = "s";

            }
        }

        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox5.Checked == true)
            {
                checkBox6.Checked = false;
                podc_flag = "s";
            }
            else
            {
                podc_flag = "r";

            }
        }

        private void checkBox7_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox7.Checked == true)
            {
                checkBox8.Checked = false;
                pibc_flag = "s";
            }
            else
            {
                pibc_flag = "r";

            }
        }

        private void checkBox8_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox8.Checked == true)
            {
                checkBox7.Checked = false;
                pibc_flag = "r";
            }
            else
            {
                pibc_flag = "s";

            }
        }

        private void checkBox10_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox10.Checked == true)
            {
                checkBox9.Checked = false;
                pv_flag = "s";
            }
            else
            {
                pv_flag = "r";

            }
        }

        private void checkBox9_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox9.Checked == true)
            {
                checkBox10.Checked = false;
                pv_flag = "r";
            }
            else
            {
                pv_flag = "s";

            }
        }

        private void checkBox31_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox31.Checked == true)
            {
                checkBox32.Checked = false;
                checkBox1.Checked = false;
                checkBox2.Checked = false;
                checkBox3.Checked = false;
                checkBox4.Checked = false;
                checkBox5.Checked = false;
                checkBox6.Checked = false;
                groupBox3.Enabled = true;
                groupBox5.Enabled = false;

                checkBox8.Checked = true;
                checkBox26.Checked = true;
                checkBox28.Checked = true;

                pm_flag = "s";
                pv_flag = "r";
                pbdc_flag = "r";
                pdsc_flag = "r";
                podc_flag = "r";
            }
        }

        private void checkBox32_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox32.Checked == true)
            {
                checkBox31.Checked = false;
                checkBox7.Checked = false;
                checkBox8.Checked = false;
                checkBox25.Checked = false;
                checkBox26.Checked = false;
                checkBox27.Checked = false;
                checkBox28.Checked = false;
                groupBox3.Enabled = false;
                groupBox5.Enabled = true;

                checkBox2.Checked = true;
                checkBox3.Checked = true;
                checkBox6.Checked = true;
                checkBox9.Checked = true;

                pm_flag = "r";
                pibc_flag = "r";
                pu_flag = "r";
                pd_flag = "r";
            }
        }


        //******************************************************************************************************************************************************
        //                                                                  中断配置
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        //                                                           
        //******************************************************************************************************************************************************

        private void comboBox13_SelectedIndexChanged_1(object sender, EventArgs e)
        {

            irq.confirm_flag[0] = 1;

        }

        private void comboBox10_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            irq.confirm_flag[1] = 1;

        }

        private void comboBox5_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            irq.confirm_flag[2] = 1;

        }

        private void comboBox7_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            irq.confirm_flag[3] = 1;
        }

        private void comboBox8_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            irq.confirm_flag[4] = 1;
        }

        private void comboBox9_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            irq.confirm_flag[5] = 1;
        }

        private void comboBox12_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            irq.confirm_flag[6] = 1;
        }

        private void comboBox16_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            irq.confirm_flag[7] = 1;

        }

        private void comboBox14_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            irq.confirm_flag[8] = 1;
        }

        private void comboBox15_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            irq.confirm_flag[9] = 1;
        }

        private void comboBox11_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            irq.confirm_flag[10] = 1;
        }
        //******************************************************************************************************************************************************
        //                                                                  中断配置
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        //                                                           
        //******************************************************************************************************************************************************

        //******************************************************************************************************************************************************
        //                                                                  Peripheral中断初始化对应值                                                      
        //******************************************************************************************************************************************************  
        public string[] TAUKey = new string[]{"B0_0","B0_1","B0_2","B0_3","B0_4","B0_5","B0_6","B0_7","B0_8","B0_9","B0_10","B0_11","B0_12","B0_13","B0_14","B0_15",
								        "D0_0","D0_1","D0_2","D0_3","D0_4","D0_5","D0_6","D0_7","D0_8","D0_9","D0_10","D0_11","D0_12","D0_13","D0_14","D0_15",
								        "J0_0","J0_1","J0_2","J0_3","J1_0","J1_1","J1_2","J1_3"};
        public string[] TAUValue = new string[]{"INTTAUB0I0","INTTAUB0I1","INTTAUB0I2","INTTAUB0I3_PWGA16","INTTAUB0I4","INTTAUB0I5_PWGA17","INTTAUB0I6","INTTAUB0I7_PWGA18","INTTAUB0I8","INTTAUB0I9_PWGA19","INTTAUB0I10","INTTAUB0I11_PWGA26","INTTAUB0I12","INTTAUB0I13_PWGA30","INTTAUB0I14","INTTAUB0I15_PWGA31",
								          "INTTAUD0I0","INTTAUD0I1","INTTAUD0I2","INTTAUD0I3","INTTAUD0I4","INTTAUD0I5","INTTAUD0I6","INTTAUD0I7","INTTAUD0I8","INTTAUD0I9","INTTAUD0I10","INTTAUD0I11","INTTAUD0I12","INTTAUD0I13","INTTAUD0I14","INTTAUD0I15",
                                          "INTTAUJ0I0","INTTAUJ0I1","INTTAUJ0I2","INTTAUJ0I3",
                                          "INTTAUJ1I0","INTTAUJ1I1","INTTAUJ1I2","INTTAUJ1I3"};

        public string[] ADCKey = new string[] { "0", "1", "2", };
        public string[] ADCValue = new string[] { "INTADCA0I0", "INTADCA0I1", "INTADCA0I2" };

        public string[] CANKey = new string[] { "0", "1", "2" };
        public string[,] CANValue = new string[,]{{"INTRCANGERR","INTRCANGRECC","INTRCAN0ERR","INTRCAN0REC","INTRCAN0TRX"},
                                           {"INTRCANGERR","INTRCANGRECC","INTRCAN1ERR","INTRCAN1REC","INTRCAN1TRX"},
                                           {"INTRCANGERR","INTRCANGRECC","INTRCAN2ERR","INTRCAN2REC","INTRCAN2TRX"} };

        public string[] SPIKey = new string[] { "0", "1", "2", "3" };
        public string[,] SPIValue = new string[,]{{"INTCSIH0IC","INTCSIH0IR","INTCSIH0IRE"},
                                           {"INTCSIH1IC","INTCSIH1IR","INTCSIH1IRE"},
                                           {"INTCSIH2IC","INTCSIH2IR","INTCSIH2IRE"},
                                           {"INTCSIH3IC","INTCSIH3IR","INTCSIH3IRE"}};

        public string[] UARTKey = new string[] { "0", "1", "2", "3" };
        public string[,] UARTValue = new string[,]{{"INTRLIN30","INTRLIN30UR0","INTRLIN30UR1","INTRLIN30UR2"},
                                           {"INTRLIN31","INTRLIN31UR0","INTRLIN31UR1","INTRLIN31UR2"},
                                           {"INTRLIN32","INTRLIN32UR0","INTRLIN32UR1","INTRLIN32UR2"},
                                           {"INTRLIN33","INTRLIN33UR0","INTRLIN33UR1","INTRLIN33UR2"} };

        public string[] EXKey = new string[] { "0", "1", "2", "3", "4", "5", "6", "7", "8", "10", "11", "12" };
        public string[] EXValue = new string[] { "INTP0", "INTP1", "INTP2", "INTP3", "INTP4", "INTP5", "INTP6", "INTP7", "INTP8", "INTP10", "INTP11", "INTP12", };

        public string[] WDKey = new string[] { "0", "1" };
        public string[] WDValue = new string[] { "INTWDTA0", "INTWDTA1" };

        public string[] DMAKey = new string[] { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15" };
        public string[] DMAValue = new string[] { "INTDMA0", "INTDMA1", "INTDMA2", "INTDMA3", "INTDMA4", "INTDMA5", "INTDMA6", "INTDMA7", "INTDMA8", "INTDMA9", "INTDMA10", "INTDMA11", "INTDMA12", "INTDMA13", "INTDMA14", "INTDMA15" };

        public string[] IICKey = new string[] { "0" };
        public string[] IICValue = new string[] { "INTRIIC0TI", "INTRIIC0TEI", "INTRIIC0RI", "INTRIIC0EE" };

        public string[] OSKey = new string[] { "0" };
        public string[] OSValue = new string[] { "INTOSTM0" };

        public string[] PWMKey = new string[]{"0","1","2","3",
                               "8","9","10","11","12","13","14","15",
                               "20","21","22","23","24","25","26","27","28","29","30","31","32","33","34","35","36","37","38","39","40","41","42","43","44","45","46","47"};
        public string[] PWMValue = new string[]{"INTPWGA0","INTPWGA1","INTPWGA2","INTPWGA3",
                                         "INTPWGA8","INTPWGA9","INTPWGA10","INTPWGA11","INTPWGA12","INTPWGA13","INTPWGA14","INTPWGA15",
                                         "INTPWGA20","INTPWGA21","INTPWGA22","INTPWGA23","INTPWGA24","INTPWGA25","INTPWGA26","INTPWGA27","INTPWGA28","INTPWGA29","INTPWGA30","INTPWGA31","INTPWGA32","INTPWGA33","INTPWGA34","INTPWGA35","INTPWGA36","INTPWGA37","INTPWGA38","INTPWGA39","INTPWGA40","INTPWGA41","INTPWGA42","INTPWGA43","INTPWGA44","INTPWGA45","INTPWGA46","INTPWGA47"};
        //******************************************************************************************************************************************************
        //                                                                  Pin中断初始化对应值                                                      
        //******************************************************************************************************************************************************  
        public string[] interruptFlag = new string[] { "OS", "INTP", "TAU", "ADC", "CAN", "RLIN", "IIC", "CSIH", "PWGA", "WD", "DMA" };

        //******************************************************************************************************************************************************
        //                                                                  中断赋值                                                      
        //******************************************************************************************************************************************************        
        public List<string> valueOS = new List<string>();
        public List<string> valueEX = new List<string>();
        public List<string> valueTAU = new List<string>();
        public List<string> valueSPI = new List<string>();
        public List<string> valueWD = new List<string>();
        public List<string> valuePWM = new List<string>();
        public List<string> valueUART = new List<string>();
        public List<string> valueDMA = new List<string>();
        public List<string> valueIIC = new List<string>();
        public List<string> valueCAN = new List<string>();
        public List<string> valueADC = new List<string>();

        public int flagIrq;
        public string valueListOS = "", valueListEX = "", valueListTAU = "", valueListSPI = "", valueListWD = "", valueListPWM = "", valueListUART = "", valueListDMA = "", valueListIIC = "", valueListCAN = "", valueListADC = "";


        public void switch_combox(int value)
        {
            switch (value)
            {
                case 0:
                    int keyOS = Array.IndexOf(OSKey.ToArray(), comboBox13.SelectedItem.ToString());
                    flagIrq = valueOS.IndexOf(OSValue[keyOS]);
                    if (flagIrq < 0)
                    {
                        valueOS.Add(OSValue[keyOS]);
                        if (valueListOS == "")
                            valueListOS = OSValue[keyOS];
                        else
                            valueListOS = valueListOS + "、" + OSValue[keyOS];
                    }
                    if (irq.IRQ_Add_Node("OS", comboBox13.SelectedItem.ToString(), ref irq_path))
                    {
                        richTextBox2.Text += "OS Timer " + comboBox13.SelectedItem.ToString() + " interrupt : " + OSValue[keyOS] + " confirm done.\n";
                        textBox12.Text = valueListOS;
                        flag_count++;
                        irq.IRQ_change_node(flag_count, 1, ref irq_path);
                    }
                    break;
                case 1:
                    int keyEX = Array.IndexOf(EXKey.ToArray(), comboBox10.SelectedItem.ToString());
                    flagIrq = valueEX.IndexOf(EXValue[keyEX]);
                    if (flagIrq < 0)
                    {
                        valueEX.Add(EXValue[keyEX]);
                        if (valueListEX == "")
                            valueListEX = EXValue[keyEX];
                        else
                            valueListEX = valueListEX + " 、 " + EXValue[keyEX];
                    }
                    if (irq.IRQ_Add_Node("EX", comboBox10.SelectedItem.ToString(), ref irq_path))
                    {
                        richTextBox2.Text += "Extern " + comboBox10.SelectedItem.ToString() + " interrupt : " + EXValue[keyEX] + " confirm done.\n";
                        textBox21.Text = valueListEX;
                        flag_count++;
                        irq.IRQ_change_node(flag_count, 1, ref irq_path);
                    }
                    break;
                case 2:
                    int keyTAU = Array.IndexOf(TAUKey.ToArray(), comboBox5.SelectedItem.ToString());
                    flagIrq = valueTAU.IndexOf(TAUValue[keyTAU]);
                    if (flagIrq < 0)
                    {
                        valueTAU.Add(TAUValue[keyTAU]);
                        if (valueListTAU == "")
                            valueListTAU = TAUValue[keyTAU];
                        else
                            valueListTAU = valueListTAU + "、" + TAUValue[keyTAU];
                    }
                    if (irq.IRQ_Add_Node("TAU", comboBox5.SelectedItem.ToString(), ref irq_path))
                    {
                        richTextBox2.Text += "Time array uint " + comboBox5.SelectedItem.ToString() + " interrupt : " + TAUValue[keyTAU] + " confirm done.\n";
                        textBox22.Text = valueListTAU;
                        flag_count++;
                        irq.IRQ_change_node(flag_count, 1, ref  irq_path);

                    } break;
                case 3:
                    int keyADC = Array.IndexOf(ADCKey.ToArray(), comboBox7.SelectedItem.ToString());
                    flagIrq = valueADC.IndexOf(ADCValue[keyADC]);
                    if (flagIrq < 0)
                    {
                        valueADC.Add(ADCValue[keyADC]);
                        if (valueListADC == "")
                            valueListADC = ADCValue[keyADC];
                        else
                            valueListADC = valueListADC + "、" + ADCValue[keyADC];
                    }
                    if (irq.IRQ_Add_Node("ADC", comboBox7.SelectedItem.ToString(), ref irq_path))
                    {
                        richTextBox2.Text += "ADC " + comboBox7.SelectedItem.ToString() + " interrupt : " + ADCValue[keyADC] + " confirm done.\n";
                        textBox13.Text = valueListADC;
                        flag_count++;
                        irq.IRQ_change_node(flag_count, 1, ref  irq_path);
                    }
                    break;
                case 4:
                    int keyCAN = Array.IndexOf(CANKey.ToArray(), comboBox8.SelectedItem.ToString());
                    flagIrq = valueCAN.IndexOf(CANValue[keyCAN, 4]);
                    if (flagIrq < 0)
                    {
                        valueCAN.Add(CANValue[keyCAN, 0]); valueCAN.Add(CANValue[keyCAN, 1]); valueCAN.Add(CANValue[keyCAN, 2]); valueCAN.Add(CANValue[keyCAN, 3]); valueCAN.Add(CANValue[keyCAN, 4]);
                        if (valueListCAN == "")
                        {
                            flag_count += 2; //flag_count += 5;
                            valueListCAN = CANValue[keyCAN, 0] + "、" + CANValue[keyCAN, 1] + "、" + CANValue[keyCAN, 2] + "、" + CANValue[keyCAN, 3] + "、" + CANValue[keyCAN, 4];
                        }
                        else
                        {
                            flag_count += 2;
                            valueListCAN = valueListCAN + "、" + CANValue[keyCAN, 3] + "、" + CANValue[keyCAN, 4];
                        }
                    }
                    if (irq.IRQ_Add_Node("CAN", comboBox8.SelectedItem.ToString(), ref irq_path))
                    {
                        richTextBox2.Text += "CAN " + comboBox8.SelectedItem.ToString() + " interrupt : " + CANValue[keyCAN, 0] + "、" + CANValue[keyCAN, 1] + "、" + CANValue[keyCAN, 2] + "、" + CANValue[keyCAN, 3] + "、" + CANValue[keyCAN, 4] + " confirm done.\n";
                        textBox15.Text = valueListCAN;
                        irq.IRQ_change_node(flag_count, 3, ref  irq_path);
                    }
                    break;
                case 5:
                    int keyUART = Array.IndexOf(UARTKey.ToArray(), comboBox9.SelectedItem.ToString());
                    flagIrq = valueUART.IndexOf(UARTValue[keyUART, 0]);
                    if (flagIrq < 0)
                    {
                        valueUART.Add(UARTValue[keyUART, 0]); valueUART.Add(UARTValue[keyUART, 1]); valueUART.Add(UARTValue[keyUART, 1]); valueUART.Add(UARTValue[keyUART, 2]); valueUART.Add(UARTValue[keyUART, 3]);
                        if (valueListUART == "")
                            valueListUART = UARTValue[keyUART, 0] + "、" + UARTValue[keyUART, 1] + "、" + UARTValue[keyUART, 2] + "、" + UARTValue[keyUART, 3];
                        else
                            valueListUART = valueListUART + "、" + UARTValue[keyUART, 0] + "、" + UARTValue[keyUART, 1] + "、" + UARTValue[keyUART, 2] + "、" + UARTValue[keyUART, 3];
                    }
                    if (irq.IRQ_Add_Node("UART", comboBox9.SelectedItem.ToString(), ref irq_path))
                    {
                        richTextBox2.Text += "UART " + comboBox9.SelectedItem.ToString() + " interrupt : " + UARTValue[keyUART, 0] + "、" + UARTValue[keyUART, 1] + "、" + UARTValue[keyUART, 2] + "、" + UARTValue[keyUART, 3] + " confirm done.\n";
                        textBox17.Text = valueListUART; flag_count += 4;
                        irq.IRQ_change_node(flag_count, 4, ref irq_path);
                    }
                    break;
                case 6:
                    int keyIIC = Array.IndexOf(IICKey.ToArray(), comboBox12.SelectedItem.ToString());
                    flagIrq = valueIIC.IndexOf(IICValue[keyIIC]);
                    if (flagIrq < 0)
                    {
                        valueIIC.Add(IICValue[keyIIC]); valueIIC.Add(IICValue[keyIIC + 1]); valueIIC.Add(IICValue[keyIIC + 1]); valueIIC.Add(IICValue[keyIIC + 2]); valueIIC.Add(IICValue[keyIIC + 3]);
                        if (valueListIIC == "")
                            valueListIIC = IICValue[keyIIC] + "、" + IICValue[keyIIC + 1] + "、" + IICValue[keyIIC + 2] + "、" + IICValue[keyIIC + 3];
                        else
                            valueListIIC = valueListIIC + " 、 " + IICValue[keyIIC] + "、" + IICValue[keyIIC + 1] + "、" + IICValue[keyIIC + 2] + "、" + IICValue[keyIIC + 3];
                    }
                    if (irq.IRQ_Add_Node("IIC", comboBox12.SelectedItem.ToString(), ref irq_path))
                    {
                        richTextBox2.Text += "IIC " + comboBox12.SelectedItem.ToString() + " interrupt : " + IICValue[keyIIC] + "、" + IICValue[keyIIC + 1] + "、" + IICValue[keyIIC + 2] + "、" + IICValue[keyIIC + 3] + " confirm done.\n";
                        textBox20.Text = valueListIIC;
                        flag_count += 4;
                        irq.IRQ_change_node(flag_count, 4, ref  irq_path);
                    }
                    break;
                case 7:
                    int keySPI = Array.IndexOf(SPIKey.ToArray(), comboBox16.SelectedItem.ToString());
                    flagIrq = valueSPI.IndexOf(SPIValue[keySPI, 0]);
                    if (flagIrq < 0)
                    {
                        valueSPI.Add(SPIValue[keySPI, 0]); valueSPI.Add(SPIValue[keySPI, 1]); valueSPI.Add(SPIValue[keySPI, 2]);
                        if (valueListSPI == "")
                            valueListSPI = SPIValue[keySPI, 0] + "、" + SPIValue[keySPI, 1] + "、" + SPIValue[keySPI, 2];
                        else
                            valueListSPI = valueListSPI + "、" + SPIValue[keySPI, 0] + "、" + SPIValue[keySPI, 1] + "、" + SPIValue[keySPI, 2];
                    }
                    if (irq.IRQ_Add_Node("SPI", comboBox16.SelectedItem.ToString(), ref irq_path))
                    {
                        richTextBox2.Text += "SPI " + comboBox16.SelectedItem.ToString() + " interrupt : " + SPIValue[keySPI, 0] + "、" + SPIValue[keySPI, 1] + "、" + SPIValue[keySPI, 2] + " confirm done.\n";
                        textBox16.Text = valueListSPI;
                        flag_count += 3;
                        irq.IRQ_change_node(flag_count, 3, ref  irq_path);
                    }
                    break;
                case 8:
                    int keyPWM = Array.IndexOf(PWMKey.ToArray(), comboBox14.SelectedItem.ToString());
                    flagIrq = valuePWM.IndexOf(PWMValue[keyPWM]);
                    if (flagIrq < 0)
                    {
                        valuePWM.Add(PWMValue[keyPWM]);
                        if (valueListPWM == "")
                            valueListPWM = PWMValue[keyPWM];
                        else
                            valueListPWM = valueListPWM + "、" + PWMValue[keyPWM];
                    }
                    if (irq.IRQ_Add_Node("PWM", comboBox14.SelectedItem.ToString(), ref irq_path))
                    {
                        richTextBox2.Text += "PWM " + comboBox14.SelectedItem.ToString() + " interrupt : " + PWMValue[keyPWM] + " confirm done.\n";
                        textBox18.Text = valueListPWM;
                        flag_count++;
                        irq.IRQ_change_node(flag_count, 1, ref  irq_path);
                    }
                    break;
                case 9:
                    int keyWD = Array.IndexOf(WDKey.ToArray(), comboBox15.SelectedItem.ToString());
                    flagIrq = valueWD.IndexOf(WDValue[keyWD]);
                    if (flagIrq < 0)
                    {
                        valueWD.Add(WDValue[keyWD]);
                        if (valueListWD == "")
                            valueListWD = WDValue[keyWD];
                        else
                            valueListWD = valueListWD + "、" + WDValue[keyWD];
                    }
                    if (irq.IRQ_Add_Node("WD", comboBox15.SelectedItem.ToString(), ref irq_path))
                    {
                        richTextBox2.Text += "WD " + comboBox15.SelectedItem.ToString() + " interrupt : " + WDValue[keyWD] + " confirm done.\n";
                        textBox14.Text = valueListWD;
                        flag_count++;
                        irq.IRQ_change_node(flag_count, 1, ref  irq_path);
                    }
                    break;
                case 10:
                    int keyDMA = Array.IndexOf(DMAKey.ToArray(), comboBox11.SelectedItem.ToString());
                    flagIrq = valueDMA.IndexOf(DMAValue[keyDMA]);
                    if (flagIrq < 0)
                    {
                        valueDMA.Add(DMAValue[keyDMA]);
                        if (valueListDMA == "")
                            valueListDMA = DMAValue[keyDMA];
                        else
                            valueListDMA = valueListDMA + "、" + DMAValue[keyDMA];
                    }
                    if (irq.IRQ_Add_Node("DMA", comboBox11.SelectedItem.ToString(), ref irq_path))
                    {
                        richTextBox2.Text += "DMA " + comboBox11.SelectedItem.ToString() + " interrupt : " + DMAValue[keyDMA] + " confirm done.\n";
                        textBox19.Text = valueListDMA;
                        flag_count += 4;
                        irq.IRQ_change_node(flag_count, 4, ref  irq_path);
                    }
                    break;
            }

        }
        private void irq_switch_combox(int value)
        {

            switch (value)
            {
                case 0: irq_list_value[item_flag[0], 0] = comboBox13.SelectedItem.ToString(); item_flag[0]++;
                    break;
                case 3: irq_list_value[item_flag[1], 1] = comboBox7.SelectedItem.ToString(); item_flag[1]++;
                    break;
                case 4: irq_list_value[item_flag[2], 2] = comboBox8.SelectedItem.ToString(); item_flag[2]++;
                    break;
                case 6: irq_list_value[item_flag[3], 3] = comboBox12.SelectedItem.ToString(); item_flag[3]++;
                    break;
                case 8: irq_list_value[item_flag[4], 4] = comboBox14.SelectedItem.ToString(); item_flag[4]++;
                    break;
                case 10: irq_list_value[item_flag[5], 5] = comboBox11.SelectedItem.ToString(); item_flag[5]++;
                    break;
                case 1: irq_list_value[item_flag[6], 6] = comboBox10.SelectedItem.ToString(); item_flag[6]++;
                    break;
                case 2: irq_list_value[item_flag[7], 7] = comboBox5.SelectedItem.ToString(); item_flag[7]++;
                    break;
                case 5: irq_list_value[item_flag[8], 8] = comboBox9.SelectedItem.ToString(); item_flag[8]++;
                    break;
                case 7: irq_list_value[item_flag[9], 9] = comboBox16.SelectedItem.ToString(); item_flag[9]++;
                    break;
                case 9: irq_list_value[item_flag[10], 10] = comboBox15.SelectedItem.ToString(); item_flag[10]++;
                    break;
            }

        }
        private void irq_switch_combox_1(int value, string channel)
        {

            switch (value)
            {
                /*
                 * case 0: irq_list_value[item_flag[0], 0] = comboBox13.SelectedItem.ToString(); item_flag[0]++;
                    break;
                 */
                case 3: irq_list_value[item_flag[1], 1] = channel; item_flag[1]++;
                    break;
                case 4: irq_list_value[item_flag[2], 2] = channel; item_flag[2]++;
                    break;
                case 6: irq_list_value[item_flag[3], 3] = channel; item_flag[3]++;
                    break;
                case 8: irq_list_value[item_flag[4], 4] = channel; item_flag[4]++;
                    break;
                /*
                case 10: irq_list_value[item_flag[5], 5] = comboBox11.SelectedItem.ToString(); item_flag[5]++;
                    break;
                 */
                case 1: irq_list_value[item_flag[6], 6] = channel; item_flag[6]++;
                    break;
                case 2: irq_list_value[item_flag[7], 7] = channel; item_flag[7]++;
                    break;
                case 5: irq_list_value[item_flag[8], 8] = channel; item_flag[8]++;
                    break;
                case 7: irq_list_value[item_flag[9], 9] = channel; item_flag[9]++;
                    break;
                /*
                case 9: irq_list_value[item_flag[10], 10] = comboBox15.SelectedItem.ToString(); item_flag[10]++;
                    break;
                */
            }

        }
        private int return_max(ref int[] value)
        {
            int mid;
            mid = value[0];
            for (int i = 1; i < 11; i++)
            {
                if (value[i] > mid)
                {
                    mid = value[i];
                }
            }
            return mid;

        }
        private void button5_Click_1(object sender, EventArgs e)
        {
            for (int i = 0; i < 11; i++)
            {
                if (irq.confirm_flag[i] == 1)
                {
                    switch_combox(i);
                    irq_switch_combox(i);
                }
                irq.confirm_flag[i] = 0;
            }
            this.listView2.Items.Clear();  //移除所有的项
            for (int a = 0; a < return_max(ref item_flag); a++)
            {
                this.listView2.BeginUpdate();
                ListViewItem lvi1 = new ListViewItem();

                lvi1.Text = irq_list_value[a, 0];//端口
                for (int j = 1; j < 11; j++)
                {
                    lvi1.SubItems.Add(irq_list_value[a, j]); //模式
                }
                this.listView2.Items.Add(lvi1);
                this.listView2.EndUpdate();
            }
        }

        private void label14_Click(object sender, EventArgs e)
        {
            alt_fun_disp("AP0_7", "30", 7);

        }

        private void label7_Click(object sender, EventArgs e)
        {

            alt_fun_disp("AP0_8", "30", 8);

        }

        private void linkLabel62_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

            alt_fun_disp("AP0_9", "30", 9);

        }

        private void linkLabel57_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

            alt_fun_disp("AP0_11", "30", 11);

        }

        private void linkLabel58_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

            alt_fun_disp("AP0_10", "30", 10);

        }

        private void linkLabel56_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

            alt_fun_disp("AP0_12", "30", 12);

        }

        private void linkLabel12_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

            alt_fun_disp("AP0_13", "30", 13);

        }

        private void linkLabel55_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

            alt_fun_disp("AP0_14", "30", 14);

        }

        private void linkLabel54_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

            alt_fun_disp("AP0_15", "30", 15);

        }

        private void linkLabel36_LinkClicked_1(object sender, LinkLabelLinkClickedEventArgs e)
        {

            alt_fun_disp("P10_6", "10", 6);

        }

        private void linkLabel38_LinkClicked_1(object sender, LinkLabelLinkClickedEventArgs e)
        {

            alt_fun_disp("P10_7", "10", 7);

        }

        private void linkLabel39_LinkClicked_1(object sender, LinkLabelLinkClickedEventArgs e)
        {

            alt_fun_disp("P10_8", "10", 8);

        }

        private void linkLabel43_LinkClicked_1(object sender, LinkLabelLinkClickedEventArgs e)
        {

            alt_fun_disp("P10_9", "10", 9);

        }

        private void linkLabel44_LinkClicked_1(object sender, LinkLabelLinkClickedEventArgs e)
        {

            alt_fun_disp("P10_10", "10", 10);

        }

        private void linkLabel46_LinkClicked_1(object sender, LinkLabelLinkClickedEventArgs e)
        {

            alt_fun_disp("P10_11", "10", 11);

        }

        private void linkLabel47_LinkClicked_1(object sender, LinkLabelLinkClickedEventArgs e)
        {

            alt_fun_disp("P10_12", "10", 12);

        }

        private void linkLabel77_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

            alt_fun_disp("P10_13", "10", 13);

        }

        private void linkLabel66_LinkClicked_1(object sender, LinkLabelLinkClickedEventArgs e)
        {

            alt_fun_disp("P10_14", "10", 14);

        }

        private void linkLabel78_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

            alt_fun_disp("P11_1", "11", 1);

        }

        private void linkLabel79_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

            alt_fun_disp("P11_2", "11", 2);

        }

        private void label9_Click(object sender, EventArgs e)
        {

            alt_fun_disp("P11_3", "11", 3);

        }

        private void label11_Click(object sender, EventArgs e)
        {

            alt_fun_disp("P11_4", "11", 4);

        }

        private void label10_Click(object sender, EventArgs e)
        {

            alt_fun_disp("P11_5", "11", 5);

        }

        private void label23_Click(object sender, EventArgs e)
        {

            alt_fun_disp("P11_6", "11", 6);

        }

        private void label25_Click(object sender, EventArgs e)
        {

            alt_fun_disp("P11_7", "11", 7);

        }

        private void linkLabel80_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

            alt_fun_disp("P10_0", "10", 0);


        }

        private void linkLabel82_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

            alt_fun_disp("P10_1", "10", 1);

        }

        private void linkLabel81_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

            alt_fun_disp("P10_2", "10", 2);

        }

        private void linkLabel67_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

            alt_fun_disp("P10_3", "10", 3);

        }

        private void linkLabel65_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

            alt_fun_disp("P10_4", "10", 4);


        }

        private void linkLabel64_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

            alt_fun_disp("P10_5", "10", 5);

        }

        private void linkLabel45_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

            alt_fun_disp("P10_15", "10", 15);

        }

        private void linkLabel10_LinkClicked_1(object sender, LinkLabelLinkClickedEventArgs e)
        {

            alt_fun_disp("P11_0", "11", 0);

        }

        private void linkLabel42_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {


            alt_fun_disp("P0_0", "0", 0);

        }

        private void linkLabel41_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

            alt_fun_disp("P0_1", "0", 1);

        }

        private void linkLabel19_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

            alt_fun_disp("P0_2", "0", 2);

        }

        private void linkLabel18_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

            alt_fun_disp("P0_3", "0", 3);

        }

        private void linkLabel15_LinkClicked_1(object sender, LinkLabelLinkClickedEventArgs e)
        {

            alt_fun_disp("P0_4", "0", 4);

        }

        private void linkLabel14_LinkClicked_1(object sender, LinkLabelLinkClickedEventArgs e)
        {

            alt_fun_disp("P0_5", "0", 5);

        }

        private void linkLabel13_LinkClicked_1(object sender, LinkLabelLinkClickedEventArgs e)
        {

            alt_fun_disp("P0_6", "0", 6);

        }

        private void linkLabel11_LinkClicked_1(object sender, LinkLabelLinkClickedEventArgs e)
        {

            alt_fun_disp("P0_11", "0", 11);

        }

        private void label1_Click(object sender, EventArgs e)
        {

            alt_fun_disp("P0_12", "0", 12);

        }

        private void label8_Click(object sender, EventArgs e)
        {

            alt_fun_disp("P0_13", "0", 13);

        }

        private void linkLabel9_LinkClicked_1(object sender, LinkLabelLinkClickedEventArgs e)
        {

            alt_fun_disp("P0_14", "0", 14);

        }

        private void linkLabel7_LinkClicked_1(object sender, LinkLabelLinkClickedEventArgs e)
        {

            alt_fun_disp("P8_2", "8", 2);

        }

        private void linkLabel6_LinkClicked_1(object sender, LinkLabelLinkClickedEventArgs e)
        {

            alt_fun_disp("P8_10", "8", 10);

        }

        private void linkLabel1_LinkClicked_1(object sender, LinkLabelLinkClickedEventArgs e)
        {

            alt_fun_disp("P8_11", "8", 11);

        }

        private void linkLabel5_LinkClicked_1(object sender, LinkLabelLinkClickedEventArgs e)
        {

            alt_fun_disp("P8_12", "8", 12);

        }

        private void linkLabel4_LinkClicked_1(object sender, LinkLabelLinkClickedEventArgs e)
        {


            alt_fun_disp("JP0_5", "20", 5);

        }

        private void linkLabel3_LinkClicked_1(object sender, LinkLabelLinkClickedEventArgs e)
        {


            alt_fun_disp("JP0_4", "20", 4);

        }


        private void linkLabel69_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {


            alt_fun_disp("P8_0", "8", 0);

        }

        private void linkLabel70_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

            alt_fun_disp("P8_1", "8", 1);

        }

        private void linkLabel71_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

            alt_fun_disp("P8_3", "8", 3);

        }

        private void linkLabel68_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

            alt_fun_disp("P8_4", "8", 4);

        }

        private void linkLabel75_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

            alt_fun_disp("P8_5", "8", 5);

        }

        private void linkLabel74_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

            alt_fun_disp("P8_7", "8", 7);

        }

        private void linkLabel72_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

            alt_fun_disp("P8_6", "8", 6);

        }

        private void linkLabel76_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

            alt_fun_disp("P8_8", "8", 8);

        }

        private void linkLabel73_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

            alt_fun_disp("P8_9", "8", 9);

        }

        private void linkLabel2_LinkClicked_1(object sender, LinkLabelLinkClickedEventArgs e)
        {

            alt_fun_disp("JP0_3", "20", 3);

        }
        private void ListView1_MouseClick(object sender, MouseEventArgs e)
        {

            if (e.Button == MouseButtons.Right)
            {
                if (this.listView1.SelectedItems.Count > 0)
                {
                    ListViewItem xy = listView1.GetItemAt(e.X, e.Y);
                    if (xy != null)
                    {
                        Point point = this.PointToClient(listView1.PointToScreen(new Point(e.X, e.Y)));
                        this.contextMenuStrip1.Show(this, point);
                    }
                }
                else
                {
                    this.contextMenuStrip1.Hide();

                }
            }

        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {

            XmlDocument xmlDoc = new XmlDocument();
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.IgnoreComments = true;
            XmlReader reader = XmlReader.Create(pin_path, settings);
            xmlDoc.Load(reader);
            XmlNode root = xmlDoc.SelectSingleNode("GPIO_Set");
            XmlNodeList xn = xmlDoc.SelectSingleNode("GPIO_Set").ChildNodes;
            string delete_group = "", delete_port = "";
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
                            if (sxe1.InnerText == listView1.FocusedItem.SubItems[0].Text)
                            {
                                delete_group = sxe1.GetAttribute("gro");
                                delete_port = sxe1.GetAttribute("por");

                            }
                        }
                    }

                }

            }
            reader.Close();

            xmlDoc.Save(pin_path);
            gpio.delete_list_node_con(listView1.FocusedItem.SubItems[0].Text, listView1.FocusedItem.SubItems[1].Text, listView1.FocusedItem.SubItems[2].Text, listView1.FocusedItem.SubItems[3].Text, listView1.FocusedItem.SubItems[4].Text, listView1.FocusedItem.SubItems[5].Text, listView1.FocusedItem.SubItems[6].Text, listView1.FocusedItem.SubItems[7].Text, listView1.FocusedItem.SubItems[8].Text, listView1.FocusedItem.SubItems[9].Text, listView1.FocusedItem.SubItems[10].Text, listView1.FocusedItem.SubItems[11].Text, ref pin_path);

            int ars = 0, por = 0, gro = 0;
            switch (delete_group)
            {
                case "0": por = 0; break;
                case "8": por = 1; break;
                case "9": por = 2; break;
                case "10": por = 3; break;
                case "20": por = 4; break;
                case "30": por = 5; break;
            }
            switch (delete_port)
            {
                case "0": gro = 0; break;
                case "1": gro = 1; break;
                case "2": gro = 2; break;
                case "3": gro = 3; break;
                case "4": gro = 4; break;
                case "5": gro = 5; break;
                case "6": gro = 6; break;
                case "7": gro = 7; break;
                case "8": gro = 8; break;
                case "9": gro = 9; break;
                case "10": gro = 10; break;
                case "11": gro = 11; break;
                case "12": gro = 12; break;
                case "13": gro = 13; break;
                case "14": gro = 14; break;
                case "15": gro = 15; break;

            }
            switch (listView1.FocusedItem.SubItems[1].Text)
            {
                case "ACTIVE": ars = 1; break;
                case "RESET": ars = 2; break;
                case "STANDBY": ars = 3; break;
            }

            switch (ars)
            {
                case 1: Alt_PM_value_Act[por] = Alt_PM_value_Act[por] | (1 << gro);
                    Alt_PFCE_value_Act[por] = Alt_PFCE_value_Act[por] & ((1 << gro) ^ 65535);
                    Alt_PFCAE_value_Act[por] = Alt_PFCAE_value_Act[por] & ((1 << gro) ^ 65535);
                    Alt_PFC_value_Act[por] = Alt_PFC_value_Act[por] & ((1 << gro) ^ 65535);
                    Alt_PMC_value_Act[por] = Alt_PMC_value_Act[por] & ((1 << gro) ^ 65535);
                    Alt_PIBC_value_Act[por] = Alt_PIBC_value_Act[por] & ((1 << gro) ^ 65535);
                    Alt_PU_value_Act[por] = Alt_PU_value_Act[por] & ((1 << gro) ^ 65535);
                    Alt_PD_value_Act[por] = Alt_PD_value_Act[por] & ((1 << gro) ^ 65535);
                    Alt_PBDC_value_Act[por] = Alt_PBDC_value_Act[por] & ((1 << gro) ^ 65535);
                    Alt_PDSC_value_Act[por] = Alt_PDSC_value_Act[por] & ((1 << gro) ^ 65535);
                    Alt_PODC_value_Act[por] = Alt_PODC_value_Act[por] & ((1 << gro) ^ 65535);
                    Alt_PIPC_value_Act[por] = Alt_PIPC_value_Act[por] & ((1 << gro) ^ 65535);

                    Alt_Port_value_Act[por] = Alt_Port_value_Act[por] & ((1 << gro) ^ 65535);
                    break;
                case 2: Alt_PM_value_Res[por] = Alt_PM_value_Res[por] | (1 << gro);
                    Alt_PFCE_value_Res[por] = Alt_PFCE_value_Res[por] & ((1 << gro) ^ 65535);
                    Alt_PFCAE_value_Res[por] = Alt_PFCAE_value_Res[por] & ((1 << gro) ^ 65535);
                    Alt_PFC_value_Res[por] = Alt_PFC_value_Res[por] & ((1 << gro) ^ 65535);
                    Alt_PMC_value_Res[por] = Alt_PMC_value_Res[por] & ((1 << gro) ^ 65535);
                    Alt_PIBC_value_Res[por] = Alt_PIBC_value_Res[por] & ((1 << gro) ^ 65535);
                    Alt_PU_value_Res[por] = Alt_PU_value_Res[por] & ((1 << gro) ^ 65535);
                    Alt_PD_value_Res[por] = Alt_PD_value_Res[por] & ((1 << gro) ^ 65535);
                    Alt_PBDC_value_Res[por] = Alt_PBDC_value_Res[por] & ((1 << gro) ^ 65535);
                    Alt_PDSC_value_Res[por] = Alt_PDSC_value_Res[por] & ((1 << gro) ^ 65535);
                    Alt_PODC_value_Res[por] = Alt_PODC_value_Res[por] & ((1 << gro) ^ 65535);
                    Alt_PIPC_value_Res[por] = Alt_PIPC_value_Res[por] & ((1 << gro) ^ 65535);

                    Alt_Port_value_Res[por] = Alt_Port_value_Res[por] & ((1 << gro) ^ 65535);
                    break;
                case 3: Alt_PM_value_Sta[por] = Alt_PM_value_Sta[por] | (1 << gro);
                    Alt_PFCE_value_Sta[por] = Alt_PFCE_value_Sta[por] & ((1 << gro) ^ 65535);
                    Alt_PFCAE_value_Sta[por] = Alt_PFCAE_value_Sta[por] & ((1 << gro) ^ 65535);
                    Alt_PFC_value_Sta[por] = Alt_PFC_value_Sta[por] & ((1 << gro) ^ 65535);
                    Alt_PMC_value_Sta[por] = Alt_PMC_value_Sta[por] & ((1 << gro) ^ 65535);
                    Alt_PIBC_value_Sta[por] = Alt_PIBC_value_Sta[por] & ((1 << gro) ^ 65535);
                    Alt_PU_value_Sta[por] = Alt_PU_value_Sta[por] & ((1 << gro) ^ 65535);
                    Alt_PD_value_Sta[por] = Alt_PD_value_Sta[por] & ((1 << gro) ^ 65535);
                    Alt_PBDC_value_Sta[por] = Alt_PBDC_value_Sta[por] & ((1 << gro) ^ 65535);
                    Alt_PDSC_value_Sta[por] = Alt_PDSC_value_Sta[por] & ((1 << gro) ^ 65535);
                    Alt_PODC_value_Sta[por] = Alt_PODC_value_Sta[por] & ((1 << gro) ^ 65535);
                    Alt_PIPC_value_Sta[por] = Alt_PIPC_value_Sta[por] & ((1 << gro) ^ 65535);

                    Alt_Port_value_Sta[por] = Alt_Port_value_Sta[por] & ((1 << gro) ^ 65535);
                    break;
            }
            switch (ars)
            {
                case 1: gpio.change_node("PM", delete_group, Alt_PM_value_Act[por], listView1.FocusedItem.SubItems[1].Text, ref pin_path);
                    gpio.change_node("PFCE", delete_group, Alt_PFCE_value_Act[por], listView1.FocusedItem.SubItems[1].Text, ref pin_path);
                    gpio.change_node("PFCAE", delete_group, Alt_PFCAE_value_Act[por], listView1.FocusedItem.SubItems[1].Text, ref pin_path);
                    gpio.change_node("PFC", delete_group, Alt_PFC_value_Act[por], listView1.FocusedItem.SubItems[1].Text, ref pin_path);
                    gpio.change_node("PMC", delete_group, Alt_PMC_value_Act[por], listView1.FocusedItem.SubItems[1].Text, ref pin_path);
                    gpio.change_node("PIBC", delete_group, Alt_PIBC_value_Act[por], listView1.FocusedItem.SubItems[1].Text, ref pin_path);
                    gpio.change_node("PU", delete_group, Alt_PU_value_Act[por], listView1.FocusedItem.SubItems[1].Text, ref pin_path);
                    gpio.change_node("PD", delete_group, Alt_PD_value_Act[por], listView1.FocusedItem.SubItems[1].Text, ref pin_path);
                    gpio.change_node("PBDC", delete_group, Alt_PBDC_value_Act[por], listView1.FocusedItem.SubItems[1].Text, ref pin_path);
                    gpio.change_node("PDSC", delete_group, Alt_PDSC_value_Act[por], listView1.FocusedItem.SubItems[1].Text, ref pin_path);
                    gpio.change_node("PODC", delete_group, Alt_PODC_value_Act[por], listView1.FocusedItem.SubItems[1].Text, ref pin_path);
                    gpio.change_node("PIPC", delete_group, Alt_PIPC_value_Act[por], listView1.FocusedItem.SubItems[1].Text, ref pin_path);
                    gpio.change_node("PV", delete_group, Alt_Port_value_Act[por], listView1.FocusedItem.SubItems[1].Text, ref pin_path);
                    break;
                case 2: gpio.change_node("PM", delete_group, Alt_PM_value_Res[por], listView1.FocusedItem.SubItems[1].Text, ref pin_path);
                    gpio.change_node("PFCE", delete_group, Alt_PFCE_value_Res[por], listView1.FocusedItem.SubItems[1].Text, ref pin_path);
                    gpio.change_node("PFCAE", delete_group, Alt_PFCAE_value_Res[por], listView1.FocusedItem.SubItems[1].Text, ref pin_path);
                    gpio.change_node("PFC", delete_group, Alt_PFC_value_Res[por], listView1.FocusedItem.SubItems[1].Text, ref pin_path);
                    gpio.change_node("PMC", delete_group, Alt_PMC_value_Res[por], listView1.FocusedItem.SubItems[1].Text, ref pin_path);
                    gpio.change_node("PIBC", delete_group, Alt_PIBC_value_Res[por], listView1.FocusedItem.SubItems[1].Text, ref pin_path);
                    gpio.change_node("PU", delete_group, Alt_PU_value_Res[por], listView1.FocusedItem.SubItems[1].Text, ref pin_path);
                    gpio.change_node("PD", delete_group, Alt_PD_value_Res[por], listView1.FocusedItem.SubItems[1].Text, ref pin_path);
                    gpio.change_node("PBDC", delete_group, Alt_PBDC_value_Res[por], listView1.FocusedItem.SubItems[1].Text, ref pin_path);
                    gpio.change_node("PDSC", delete_group, Alt_PDSC_value_Res[por], listView1.FocusedItem.SubItems[1].Text, ref pin_path);
                    gpio.change_node("PODC", delete_group, Alt_PODC_value_Res[por], listView1.FocusedItem.SubItems[1].Text, ref pin_path);
                    gpio.change_node("PIPC", delete_group, Alt_PIPC_value_Res[por], listView1.FocusedItem.SubItems[1].Text, ref pin_path);
                    gpio.change_node("PV", delete_group, Alt_Port_value_Res[por], listView1.FocusedItem.SubItems[1].Text, ref pin_path);
                    break;
                case 3: gpio.change_node("PM", delete_group, Alt_PM_value_Sta[por], listView1.FocusedItem.SubItems[1].Text, ref pin_path);
                    gpio.change_node("PFCE", delete_group, Alt_PFCE_value_Sta[por], listView1.FocusedItem.SubItems[1].Text, ref pin_path);
                    gpio.change_node("PFCAE", delete_group, Alt_PFCAE_value_Sta[por], listView1.FocusedItem.SubItems[1].Text, ref pin_path);
                    gpio.change_node("PFC", delete_group, Alt_PFC_value_Sta[por], listView1.FocusedItem.SubItems[1].Text, ref pin_path);
                    gpio.change_node("PMC", delete_group, Alt_PMC_value_Sta[por], listView1.FocusedItem.SubItems[1].Text, ref pin_path);
                    gpio.change_node("PIBC", delete_group, Alt_PIBC_value_Sta[por], listView1.FocusedItem.SubItems[1].Text, ref pin_path);
                    gpio.change_node("PU", delete_group, Alt_PU_value_Sta[por], listView1.FocusedItem.SubItems[1].Text, ref pin_path);
                    gpio.change_node("PD", delete_group, Alt_PD_value_Sta[por], listView1.FocusedItem.SubItems[1].Text, ref pin_path);
                    gpio.change_node("PBDC", delete_group, Alt_PBDC_value_Sta[por], listView1.FocusedItem.SubItems[1].Text, ref pin_path);
                    gpio.change_node("PDSC", delete_group, Alt_PDSC_value_Sta[por], listView1.FocusedItem.SubItems[1].Text, ref pin_path);
                    gpio.change_node("PODC", delete_group, Alt_PODC_value_Sta[por], listView1.FocusedItem.SubItems[1].Text, ref pin_path);
                    gpio.change_node("PIPC", delete_group, Alt_PIPC_value_Sta[por], listView1.FocusedItem.SubItems[1].Text, ref pin_path);
                    gpio.change_node("PV", delete_group, Alt_Port_value_Sta[por], listView1.FocusedItem.SubItems[1].Text, ref pin_path);
                    break;
            }
            this.listView1.Items.Remove(listView1.FocusedItem);

        }


        private void listView1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (this.listView1.SelectedItems.Count > 0)
                {
                    ListViewItem xy = listView1.GetItemAt(e.X, e.Y);
                    if (xy != null)
                    {
                        Point point = this.PointToClient(listView1.PointToScreen(new Point(e.X, e.Y)));
                        this.contextMenuStrip1.Show(this, point);
                    }
                }
                else
                {
                    this.contextMenuStrip1.Hide();

                }
            }

        }

        //******************************************************************************************************************************************************
        //                                             增加中断配置设置
        //******************************************************************************************************************************************************
        public void addInterrupt(String fun, Boolean flag) //判断是否增加中断设置
        {
            if (!fun.Equals("NONE") && flag)//选择Alt模式
            {
                int i = 0;
                string str;
                if (fun.IndexOf("/") > 0)
                {
                    string[] strArr = fun.Split(new char[1] { '/' });
                    fun = strArr[0];
                }
                if (fun.IndexOf(interruptFlag[0]) >= 0) i = 0;//OS
                if (fun.IndexOf(interruptFlag[1]) >= 0) i = 1;//INTP
                if (fun.IndexOf(interruptFlag[2]) >= 0) i = 2;//TAU
                if (fun.IndexOf(interruptFlag[3]) >= 0) i = 3;//ADC
                if (fun.IndexOf(interruptFlag[4]) >= 0) i = 4;//CAN
                if (fun.IndexOf(interruptFlag[5]) >= 0) i = 5;//RLIN
                if (fun.IndexOf(interruptFlag[6]) >= 0) i = 6;//IIC
                if (fun.IndexOf(interruptFlag[7]) >= 0) i = 7;//CSIH
                if (fun.IndexOf(interruptFlag[8]) >= 0) i = 8;//PWGA
                if (fun.IndexOf(interruptFlag[9]) >= 0) i = 9;//WD
                if (fun.IndexOf(interruptFlag[10]) >= 0) i = 10;//DMA
                switch (i)
                {
                    case 1:
                        if (fun.Length == 6) str = fun.Substring(fun.IndexOf(interruptFlag[i]) + 4, 2);
                        else str = fun.Substring(fun.IndexOf(interruptFlag[i]) + 4, 1);
                        for (int j = 0; j < EXKey.Length; j++)
                        {
                            if (EXKey[j].Equals(str))
                            {
                                int keyEX = Array.IndexOf(EXKey.ToArray(), str);
                                flagIrq = valueEX.IndexOf(EXValue[keyEX]);
                                if (flagIrq < 0)
                                {
                                    valueEX.Add(EXValue[keyEX]);
                                    if (valueListEX == "")
                                        valueListEX = EXValue[keyEX];
                                    else
                                        valueListEX = valueListEX + " 、 " + EXValue[keyEX];
                                }
                                irq.IRQ_Add_Node("EX", str, ref irq_path);
                                richTextBox2.Text += "Extern " + str + " interrupt : " + EXValue[keyEX] + " confirm done.\n"; textBox21.Text = valueListEX; flag_count++; irq.IRQ_change_node(flag_count, 1, ref irq_path);
                            }
                        }
                        irq_switch_combox_1(1, str);
                        this.listView2.Items.Clear();  //移除所有的项
                        for (int a = 0; a < return_max(ref item_flag); a++)
                        {
                            this.listView2.BeginUpdate();
                            ListViewItem lvi1 = new ListViewItem();

                            lvi1.Text = irq_list_value[a, 0];//端口
                            for (int j = 1; j < 11; j++)
                            {
                                lvi1.SubItems.Add(irq_list_value[a, j]); //模式
                            }
                            this.listView2.Items.Add(lvi1);
                            this.listView2.EndUpdate();
                        }
                        MessageBox.Show("Pin configuration and interrupt configuration have been completed.");
                        break;
                    case 2:
                        if (fun.Substring(fun.Length - 5, 1) == "B" || fun.Substring(fun.Length - 5, 1) == "D")//通道号是否为两位数
                        {
                            str = fun.Substring(fun.Length - 5, 5);
                            if (str.IndexOf("I") >= 0) str = str.Replace("I", "_");
                            if (str.IndexOf("O") >= 0) str = str.Replace("O", "_");
                        }
                        else
                        {
                            str = fun.Substring(fun.Length - 4, 4);
                            if (str.IndexOf("I") >= 0) str = str.Replace("I", "_");
                            if (str.IndexOf("O") >= 0) str = str.Replace("O", "_");
                        }
                        for (int j = 0; j < TAUKey.Length; j++)
                        {
                            if (TAUKey[j].Equals(str))
                            {
                                int keyTAU = Array.IndexOf(TAUKey.ToArray(), str);
                                flagIrq = valueTAU.IndexOf(TAUValue[keyTAU]);
                                if (flagIrq < 0)
                                {
                                    valueTAU.Add(TAUValue[keyTAU]);
                                    if (valueListTAU == "")
                                        valueListTAU = TAUValue[keyTAU];
                                    else
                                        valueListTAU = valueListTAU + "、" + TAUValue[keyTAU];
                                }
                                irq.IRQ_Add_Node("TAU", str, ref irq_path);
                                richTextBox2.Text += "Time array uint " + str + " interrupt : " + TAUValue[keyTAU] + " confirm done.\n"; textBox22.Text = valueListTAU; flag_count++; irq.IRQ_change_node(flag_count, 1, ref  irq_path);
                            }
                        }
                        irq_switch_combox_1(2, str);
                        this.listView2.Items.Clear();  //移除所有的项
                        for (int a = 0; a < return_max(ref item_flag); a++)
                        {
                            this.listView2.BeginUpdate();
                            ListViewItem lvi1 = new ListViewItem();

                            lvi1.Text = irq_list_value[a, 0];//端口
                            for (int j = 1; j < 11; j++)
                            {
                                lvi1.SubItems.Add(irq_list_value[a, j]); //模式
                            }
                            this.listView2.Items.Add(lvi1);
                            this.listView2.EndUpdate();
                        }
                        MessageBox.Show("Pin configuration and interrupt configuration have been completed.");
                        break;
                    case 3:
                        str = fun.Substring(fun.Length - 1, 1);
                        for (int j = 0; j < ADCKey.Length; j++)
                        {
                            if (ADCKey[j].Equals(str))
                            {
                                int keyADC = Array.IndexOf(ADCKey.ToArray(), str);
                                flagIrq = valueADC.IndexOf(ADCValue[keyADC]);
                                if (flagIrq < 0)
                                {
                                    valueADC.Add(ADCValue[keyADC]);
                                    if (valueListADC == "")
                                        valueListADC = ADCValue[keyADC];
                                    else
                                        valueListADC = valueListADC + "、" + ADCValue[keyADC];
                                }
                                irq.IRQ_Add_Node("ADC", str, ref irq_path);
                                richTextBox2.Text += "ADC " + str + " interrupt : " + ADCValue[keyADC] + " confirm done.\n"; textBox13.Text = valueListADC; flag_count++; irq.IRQ_change_node(flag_count, 1, ref  irq_path);
                            }
                        }
                        irq_switch_combox_1(3, str);
                        this.listView2.Items.Clear();  //移除所有的项
                        for (int a = 0; a < return_max(ref item_flag); a++)
                        {
                            this.listView2.BeginUpdate();
                            ListViewItem lvi1 = new ListViewItem();

                            lvi1.Text = irq_list_value[a, 0];//端口
                            for (int j = 1; j < 11; j++)
                            {
                                lvi1.SubItems.Add(irq_list_value[a, j]); //模式
                            }
                            this.listView2.Items.Add(lvi1);
                            this.listView2.EndUpdate();
                        }
                        MessageBox.Show("Pin configuration and interrupt configuration have been completed.");
                        break;
                    case 4:
                        str = fun.Substring(fun.IndexOf(interruptFlag[i]) + 3, 1);
                        for (int j = 0; j < CANKey.Length; j++)
                        {
                            if (CANKey[j].Equals(str))
                            {
                                int keyCAN = Array.IndexOf(CANKey.ToArray(), str);
                                flagIrq = valueCAN.IndexOf(CANValue[keyCAN, 4]);
                                if (flagIrq < 0)
                                {
                                    valueCAN.Add(CANValue[keyCAN, 0]); valueCAN.Add(CANValue[keyCAN, 1]); valueCAN.Add(CANValue[keyCAN, 2]); valueCAN.Add(CANValue[keyCAN, 3]); valueCAN.Add(CANValue[keyCAN, 4]);
                                    if (valueListCAN == "")
                                    {
                                        flag_count += 2; //flag_count += 5;
                                        valueListCAN = CANValue[keyCAN, 0] + "、" + CANValue[keyCAN, 1] + "、" + CANValue[keyCAN, 2] + "、" + CANValue[keyCAN, 3] + "、" + CANValue[keyCAN, 4];
                                    }
                                    else
                                    {
                                        flag_count += 2;
                                        valueListCAN = valueListCAN + "、" + CANValue[keyCAN, 3] + "、" + CANValue[keyCAN, 4];
                                    }
                                }
                                irq.IRQ_Add_Node("CAN", str, ref irq_path);
                                richTextBox2.Text += "CAN " + str + " interrupt : " + CANValue[keyCAN, 0] + "、" + CANValue[keyCAN, 1] + "、" + CANValue[keyCAN, 2] + "、" + CANValue[keyCAN, 3] + "、" + CANValue[keyCAN, 4] + " confirm done.\n"; textBox15.Text = valueListCAN; irq.IRQ_change_node(flag_count, 3, ref  irq_path);
                            }
                        }
                        irq_switch_combox_1(4, str);
                        this.listView2.Items.Clear();  //移除所有的项
                        for (int a = 0; a < return_max(ref item_flag); a++)
                        {
                            this.listView2.BeginUpdate();
                            ListViewItem lvi1 = new ListViewItem();

                            lvi1.Text = irq_list_value[a, 0];//端口
                            for (int j = 1; j < 11; j++)
                            {
                                lvi1.SubItems.Add(irq_list_value[a, j]); //模式
                            }
                            this.listView2.Items.Add(lvi1);
                            this.listView2.EndUpdate();
                        }
                        MessageBox.Show("Pin configuration and interrupt configuration have been completed.");
                        break;
                    case 5:
                        if (fun.Substring(fun.IndexOf(interruptFlag[i]) + 4, 1).Equals("3"))//RLIN30/RLIN31/RLIN32
                        {
                            str = fun.Substring(fun.IndexOf(interruptFlag[i]) + 5, 1);
                            for (int j = 0; j < UARTKey.Length; j++)
                            {
                                if (UARTKey[j].Equals(str))
                                {
                                    int keyUART = Array.IndexOf(UARTKey.ToArray(), str);
                                    flagIrq = valueUART.IndexOf(UARTValue[keyUART, 0]);
                                    if (flagIrq < 0)
                                    {
                                        valueUART.Add(UARTValue[keyUART, 0]); valueUART.Add(UARTValue[keyUART, 1]); valueUART.Add(UARTValue[keyUART, 1]); valueUART.Add(UARTValue[keyUART, 2]); valueUART.Add(UARTValue[keyUART, 3]);
                                        if (valueListUART == "")
                                            valueListUART = UARTValue[keyUART, 0] + "、" + UARTValue[keyUART, 1] + "、" + UARTValue[keyUART, 2] + "、" + UARTValue[keyUART, 3];
                                        else
                                            valueListUART = valueListUART + "、" + UARTValue[keyUART, 0] + "、" + UARTValue[keyUART, 1] + "、" + UARTValue[keyUART, 2] + "、" + UARTValue[keyUART, 3];
                                    }
                                    irq.IRQ_Add_Node("UART", str, ref irq_path);
                                    richTextBox2.Text += "UART " + str + " interrupt : " + UARTValue[keyUART, 0] + "、" + UARTValue[keyUART, 1] + "、" + UARTValue[keyUART, 2] + "、" + UARTValue[keyUART, 3] + " confirm done.\n"; textBox17.Text = valueListUART; flag_count += 4; irq.IRQ_change_node(flag_count, 4, ref irq_path);
                                }
                            }
                            irq_switch_combox_1(5, str);
                            this.listView2.Items.Clear();  //移除所有的项
                            for (int a = 0; a < return_max(ref item_flag); a++)
                            {
                                this.listView2.BeginUpdate();
                                ListViewItem lvi1 = new ListViewItem();

                                lvi1.Text = irq_list_value[a, 0];//端口
                                for (int j = 1; j < 11; j++)
                                {
                                    lvi1.SubItems.Add(irq_list_value[a, j]); //模式
                                }
                                this.listView2.Items.Add(lvi1);
                                this.listView2.EndUpdate();
                            }
                            MessageBox.Show("Pin configuration and interrupt configuration have been completed.");
                        }
                        else MessageBox.Show("Pin configuration has been completed.");
                        break;
                    case 6:
                        str = fun.Substring(fun.IndexOf(interruptFlag[i]) + 3, 1);
                        for (int j = 0; j < IICKey.Length; j++)
                        {
                            if (IICKey[j].Equals(str))
                            {
                                int keyIIC = Array.IndexOf(IICKey.ToArray(), str);
                                flagIrq = valueIIC.IndexOf(IICValue[keyIIC]);
                                if (flagIrq < 0)
                                {
                                    valueIIC.Add(IICValue[keyIIC]); valueIIC.Add(IICValue[keyIIC + 1]); valueIIC.Add(IICValue[keyIIC + 1]); valueIIC.Add(IICValue[keyIIC + 2]); valueIIC.Add(IICValue[keyIIC + 3]);
                                    if (valueListIIC == "")
                                        valueListIIC = IICValue[keyIIC] + "、" + IICValue[keyIIC + 1] + "、" + IICValue[keyIIC + 2] + "、" + IICValue[keyIIC + 3];
                                    else
                                        valueListIIC = valueListIIC + " 、 " + IICValue[keyIIC] + "、" + IICValue[keyIIC + 1] + "、" + IICValue[keyIIC + 2] + "、" + IICValue[keyIIC + 3];
                                }
                                irq.IRQ_Add_Node("IIC", str, ref irq_path);
                                richTextBox2.Text += "IIC " + str + " interrupt : " + IICValue[keyIIC] + "、" + IICValue[keyIIC + 1] + "、" + IICValue[keyIIC + 2] + "、" + IICValue[keyIIC + 3] + " confirm done.\n"; textBox20.Text = valueListIIC; flag_count += 4; irq.IRQ_change_node(flag_count, 4, ref  irq_path);
                            }
                        }
                        irq_switch_combox_1(6, str);
                        this.listView2.Items.Clear();  //移除所有的项
                        for (int a = 0; a < return_max(ref item_flag); a++)
                        {
                            this.listView2.BeginUpdate();
                            ListViewItem lvi1 = new ListViewItem();

                            lvi1.Text = irq_list_value[a, 0];//端口
                            for (int j = 1; j < 11; j++)
                            {
                                lvi1.SubItems.Add(irq_list_value[a, j]); //模式
                            }
                            this.listView2.Items.Add(lvi1);
                            this.listView2.EndUpdate();
                        }
                        MessageBox.Show("Pin configuration and interrupt configuration have been completed.");
                        break;
                    case 7:
                        str = fun.Substring(fun.IndexOf(interruptFlag[i]) + 4, 1);
                        for (int j = 0; j < SPIKey.Length; j++)
                        {
                            if (SPIKey[j].Equals(str))
                            {
                                int keySPI = Array.IndexOf(SPIKey.ToArray(), str);
                                flagIrq = valueSPI.IndexOf(SPIValue[keySPI, 0]);
                                if (flagIrq < 0)
                                {
                                    valueSPI.Add(SPIValue[keySPI, 0]); valueSPI.Add(SPIValue[keySPI, 1]); valueSPI.Add(SPIValue[keySPI, 2]);
                                    if (valueListSPI == "")
                                        valueListSPI = SPIValue[keySPI, 0] + "、" + SPIValue[keySPI, 1] + "、" + SPIValue[keySPI, 2];
                                    else
                                        valueListSPI = valueListSPI + "、" + SPIValue[keySPI, 0] + "、" + SPIValue[keySPI, 1] + "、" + SPIValue[keySPI, 2];
                                }
                                irq.IRQ_Add_Node("SPI", str, ref irq_path);
                                richTextBox2.Text += "SPI " + str + " interrupt : " + SPIValue[keySPI, 0] + "、" + SPIValue[keySPI, 1] + "、" + SPIValue[keySPI, 2] + " confirm done.\n"; textBox16.Text = valueListSPI; flag_count += 3; irq.IRQ_change_node(flag_count, 3, ref  irq_path);
                            }
                        }
                        irq_switch_combox_1(7, str);
                        this.listView2.Items.Clear();  //移除所有的项
                        for (int a = 0; a < return_max(ref item_flag); a++)
                        {
                            this.listView2.BeginUpdate();
                            ListViewItem lvi1 = new ListViewItem();

                            lvi1.Text = irq_list_value[a, 0];//端口
                            for (int j = 1; j < 11; j++)
                            {
                                lvi1.SubItems.Add(irq_list_value[a, j]); //模式
                            }
                            this.listView2.Items.Add(lvi1);
                            this.listView2.EndUpdate();
                        }
                        MessageBox.Show("Pin configuration and interrupt configuration have been completed.");
                        break;
                    case 8:
                        byte[] b = System.Text.Encoding.Unicode.GetBytes(fun.Substring(fun.IndexOf(interruptFlag[i]) + 5, 1));
                        if (b[0] >= 48 && b[0] <= 57)//通道号是否为两位数
                        {
                            str = fun.Substring(fun.IndexOf(interruptFlag[i]) + 4, 2);
                        }
                        else
                            str = fun.Substring(fun.IndexOf(interruptFlag[i]) + 4, 1);
                        for (int j = 0; j < PWMKey.Length; j++)
                        {
                            if (PWMKey[j].Equals(str))
                            {
                                int keyPWM = Array.IndexOf(PWMKey.ToArray(), str);
                                flagIrq = valuePWM.IndexOf(PWMValue[keyPWM]);
                                if (flagIrq < 0)
                                {
                                    valuePWM.Add(PWMValue[keyPWM]);
                                    if (valueListPWM == "")
                                        valueListPWM = PWMValue[keyPWM];
                                    else
                                        valueListPWM = valueListPWM + "、" + PWMValue[keyPWM];
                                }
                                irq.IRQ_Add_Node("PWM", str, ref irq_path);
                                richTextBox2.Text += "PWM " + str + " interrupt : " + PWMValue[keyPWM] + " confirm done.\n"; textBox18.Text = valueListPWM; flag_count++; irq.IRQ_change_node(flag_count, 1, ref  irq_path);
                            }
                        }
                        irq_switch_combox_1(8, str);
                        this.listView2.Items.Clear();  //移除所有的项
                        for (int a = 0; a < return_max(ref item_flag); a++)
                        {
                            this.listView2.BeginUpdate();
                            ListViewItem lvi1 = new ListViewItem();

                            lvi1.Text = irq_list_value[a, 0];//端口
                            for (int j = 1; j < 11; j++)
                            {
                                lvi1.SubItems.Add(irq_list_value[a, j]); //模式
                            }
                            this.listView2.Items.Add(lvi1);
                            this.listView2.EndUpdate();
                        }
                        MessageBox.Show("Pin configuration and interrupt configuration have been completed.");
                        break;
                }
            }
            else
                MessageBox.Show("Pin configuration has been completed.");
        }

        private void linkLabel84_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Form4 f4 = new Form4();
            f4.Show();
        }

        private void linkLabel83_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Form5 f5 = new Form5();
            f5.Show();
        }

        private void comboBox1_TextChanged(object sender, EventArgs e)
        {
            if (comboBox1.Text == "GPIO")
            {
                Reset_select();
                panel2.Enabled = true;
                groupBox9.Enabled = false;
                checkBox31.Enabled = true;
                checkBox31.Checked = true;//默认选择IN功能
                groupBox3.Enabled = true;
                checkBox32.Enabled = true;
                checkBox32.Checked = false;
                groupBox5.Enabled = false;
                Fun_Num = 0;
            }
            if (comboBox1.Text == "Alternative")
            {
                Reset_select();
                panel2.Enabled = true;
                groupBox9.Enabled = true;
                checkBox31.Checked = false;
                checkBox32.Checked = false;
                checkBox31.Enabled = false;
                checkBox32.Enabled = false;
                groupBox5.Enabled = false;
                groupBox3.Enabled = false;
                refesh_node(textBox23.Text, "\\xsl_src\\AltSQL100.xml");
            }
        }
    }
}