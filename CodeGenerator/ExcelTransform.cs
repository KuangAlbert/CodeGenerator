using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Microsoft.Office.Interop.Excel;
using IO;
using irq_set;
using System.Xml.Xsl;
using System.Xml;
using System.IO;
using System.Collections;

namespace CodeGenerator
{
    class ExcelTransform
    {
        //******************************************************************************************************************************************************
        //                                                   初始化
        //******************************************************************************************************************************************************
        Io gpio = new Io();
        IRQ irq = new IRQ();
        public ArrayList listIO = new ArrayList();
        public ArrayList listIRQ = new ArrayList();
        public string pinType, path_generator,pin_path, irq_path;//pinType判断配置是Pin100还是Pin64,只能取"Pin100"或者"Pin64"
        //pinPath:生成XML所在文件夹位置 

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

        //******************************************************************************************************************************************************
        //                                             Reset
        //******************************************************************************************************************************************************
        public void resetVariable()
        {
            Fun_Num = 0;
            ARS = 0;
            Alt_mode = ""; alt_mode = ""; alt_group = ""; alt_number = 0;
            Alt_PM_value = 0; Alt_PFC_value = 0; Alt_PFCE_value = 0; Alt_PFCAE_value = 0; Alt_PMC_value = 0; Alt_PIBC_value = 0; Alt_PU_value = 0; Alt_PD_value = 0; Alt_PBDC_value = 0; Alt_PDSC_value = 0; Alt_PODC_value = 0; Alt_PIPC_value = 0; Alt_Port_value = 0;//中介初值
            pm_flag = ""; pmc_flag = ""; pibc_flag = ""; pu_flag = ""; pd_flag = ""; pbdc_flag = ""; pdsc_flag = ""; podc_flag = ""; pipc_flag = ""; pv_flag = "";
        }
        //******************************************************************************************************************************************************
        //                                             读Excel表格中配置的引脚
        //******************************************************************************************************************************************************
        public void OpenExcel(string strFileName, string path_generator)//读取EXCEL的方法(用范围区域读取数据)
        {
            this.path_generator = path_generator;
            string rangeRow = null, rangeLine = null;
            int line = 0;
            if (strFileName.IndexOf("100") >= 0)
            {
                pinType = "Pin100";
                rangeRow = "A" + 2;
                rangeLine = "P" + 301;
                line = 300;
                this.pin_path = path_generator + "\\Pin100.xml";
                
            }
            if (strFileName.IndexOf("64") >= 0)
            {
                pinType = "Pin64";
                rangeRow = "A" + 2;
                rangeLine = "P" + 193;
                line = 192;
                this.pin_path = path_generator + "\\Pin64.xml";
            }
            if (strFileName.IndexOf("Interrupt") >= 0)
            {
                pinType = "Interrupt";
                rangeRow = "A" + 2;
                rangeLine = "C" + 120;
                line = 119;
                this.irq_path = path_generator + "\\IRQ.xml";
            }

            object missing = System.Reflection.Missing.Value;
            Microsoft.Office.Interop.Excel.Application excel = new Microsoft.Office.Interop.Excel.Application();//lauch excel application
            excel.Visible = false;
            excel.UserControl = true;
            // 以只读的形式打开EXCEL文件
            Workbook wb = excel.Application.Workbooks.Open(strFileName, missing, true, missing, missing, missing,
                missing, missing, missing, true, missing, missing, missing, missing, missing);
            //取得第一个工作薄
            Worksheet ws = (Worksheet)wb.Worksheets.get_Item(1);


            //取得总记录行数   (包括标题列)
            int rowsint = ws.UsedRange.Cells.Rows.Count; //得到行数
            int columnsint = ws.UsedRange.Cells.Columns.Count;//得到列数


            //取得数据范围区域
            Range rng = ws.Cells.get_Range(rangeRow, rangeLine);   //item


            object[,] arrayItem = (object[,])rng.Value2;   //get range's value

            //将要导入的引脚放入新数组
            if (pinType.Equals("Interrupt"))
            {
                for (int i = 1; i <= line; i++)
                {
                    if (((String)arrayItem[i, 1]).Equals("Yes"))
                    {
                        String[] arr = new String[2];
                        for (int j = 0; j < 2; j++)
                            arr[j] = arrayItem[i, j + 2].ToString();
                        listIRQ.Add(arr);
                    }

                }
                excel.Quit(); excel = null;
                Process[] procs = Process.GetProcessesByName("excel");


                foreach (Process pro in procs)
                {
                    pro.Kill();//没有更好的方法,只有杀掉进程
                }
                GC.Collect();
                generateIRQ(listIRQ);//生成Pin64/Pin100.xml
            }
            else
            {
                for (int i = 1; i <= line; i++)
                {
                    if (((String)arrayItem[i, 1]).Equals("Yes"))
                    {
                        String[] arr = new String[14];
                        for (int j = 0; j < 14; j++)
                            arr[j] = arrayItem[i, j + 3].ToString();
                        listIO.Add(arr);
                    }
                }
                excel.Quit(); excel = null;
                Process[] procs = Process.GetProcessesByName("excel");


                foreach (Process pro in procs)
                {
                    pro.Kill();//没有更好的方法,只有杀掉进程
                }
                GC.Collect();
                generatePin(listIO, pinType);//生成Pin64/Pin100.xml
            }
        }

        public void Foreach_node(string Group_Number, string mode, string location)
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

        public void alt_switch(string pm, string pfc, string pfce, string pfcae, string pmc, string pibc, string pu, string pd, string pbdc, string pdsc, string podc, string pipc, string pv, int i)
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

        public void ForeachPanelSelects(ref int num, String function)
        {
            if (function.IndexOf("In") >= 0)
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
            if (function.IndexOf("Out") >= 0)
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

        //******************************************************************************************************************************************************
        //                                             配置寄存器值，设置Pin XML
        //******************************************************************************************************************************************************
        public void generatePin(ArrayList list, String pinType)
        {
            gpio.Creat_Gpio_Xml(ref pin_path);
            for (int i = 0; i < list.Count; i++)
            {
                String[] register = (String[])list[i];
                resetVariable();//置位所有中间变量和标志位
                for (int j = 0; j < register.Length; j++)
                {
                    alt_group = register[1];
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

                    alt_number = int.Parse(register[2]);

                    alt_mode = register[3];
                    if (alt_mode.Equals("ACTIVE")) ARS = 1;
                    if (alt_mode.Equals("RESET")) ARS = 2;
                    if (alt_mode.Equals("STANDBY")) ARS = 3;

                    string gpioAlt = register[4];
                    if (gpioAlt.Equals("GPIO")) pmc_flag = "r";
                    if (gpioAlt.Equals("ALT")) pmc_flag = "s";

                    string function = register[5];
                    if (function.Equals("NONE")) Fun_Num = 0;
                    if (function.IndexOf("1") >= 0) Fun_Num = 1;
                    if (function.IndexOf("2") >= 0) Fun_Num = 2;
                    if (function.IndexOf("3") >= 0) Fun_Num = 3;
                    if (function.IndexOf("4") >= 0) Fun_Num = 4;
                    if (function.IndexOf("5") >= 0) Fun_Num = 5;

                    string inOut = register[6];
                    if (inOut.Equals("IN")) pm_flag = "r";
                    if (inOut.Equals("OUT")) pm_flag = "s";

                    string inputBuffer = register[7];
                    if (inputBuffer.Equals("Disable"))
                        pibc_flag = "r";
                    if (inputBuffer.Equals("Enable"))
                        pibc_flag = "s";

                    string pullUp = register[8];
                    if (pullUp.Equals("Disable"))
                        pu_flag = "r";
                    if (pullUp.Equals("Enable"))
                        pu_flag = "s";

                    string pullDown = register[9];
                    if (pullDown.Equals("Disable"))
                        pd_flag = "r";
                    if (pullDown.Equals("Enable"))
                        pd_flag = "s";

                    string bidMode = register[10];
                    if (bidMode.Equals("Disable"))
                        pbdc_flag = "r";
                    if (bidMode.Equals("Enable"))
                        pbdc_flag = "s";

                    string driverStrength = register[11];
                    if (driverStrength.Equals("Disable"))
                        pdsc_flag = "r";
                    if (driverStrength.Equals("Enable"))
                        pdsc_flag = "s";

                    string outputMode = register[12];
                    if (driverStrength.Equals("Disable"))
                        podc_flag = "r";
                    if (driverStrength.Equals("Enable"))
                        podc_flag = "s";

                    string portValue = register[13];
                    if (driverStrength.Equals("Low-Level"))
                        pv_flag = "r";
                    if (driverStrength.Equals("High-Level"))
                        pv_flag = "s";

                    if (Fun_Num != 0)
                    {
                        ForeachPanelSelects(ref Fun_Num, function);
                    }
                    else
                    {
                        alt_switch(pm_flag, "", "", "", "r", pibc_flag, pu_flag, pd_flag, pbdc_flag, pdsc_flag, podc_flag, "r", pv_flag, group_flag);//PM  PFC  PFCE  PFCAE  PMC  PIBC  PU  PD  PBDC  PDSC PODC PIPC PV普通模式下的赋值
                    }
                    Foreach_node(alt_group, alt_mode, pin_path);

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
                    gpio.delete_list_node(register[0], alt_mode, gpioAlt, function, inOut, inputBuffer, pullUp, pullDown, bidMode, driverStrength, outputMode, portValue, ref pin_path, alt_group, alt_number);//group:string   number:int

                    Node_Change_Flag = 0;
                }
            }
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

        public void switch_combox(int value, String channel)
        {
            switch (value)
            {
                case 0:
                    int keyOS = Array.IndexOf(OSKey.ToArray(), channel);
                    flagIrq = valueOS.IndexOf(OSValue[keyOS]);
                    if (flagIrq < 0)
                    {
                        valueOS.Add(OSValue[keyOS]);
                        if (valueListOS == "")
                            valueListOS = OSValue[keyOS];
                        else
                            valueListOS = valueListOS + "、" + OSValue[keyOS];
                    }
                    if (irq.IRQ_Add_Node("OS", channel, ref irq_path))
                    {
                        flag_count++;
                        irq.IRQ_change_node(flag_count, 1, ref irq_path);
                    }
                    break;
                case 1:
                    int keyEX = Array.IndexOf(EXKey.ToArray(), channel);
                    flagIrq = valueEX.IndexOf(EXValue[keyEX]);
                    if (flagIrq < 0)
                    {
                        valueEX.Add(EXValue[keyEX]);
                        if (valueListEX == "")
                            valueListEX = EXValue[keyEX];
                        else
                            valueListEX = valueListEX + " 、 " + EXValue[keyEX];
                    }
                    if (irq.IRQ_Add_Node("EX", channel, ref irq_path))
                    {
                        flag_count++;
                        irq.IRQ_change_node(flag_count, 1, ref irq_path);
                    }
                    break;
                case 2:
                    int keyTAU = Array.IndexOf(TAUKey.ToArray(), channel);
                    flagIrq = valueTAU.IndexOf(TAUValue[keyTAU]);
                    if (flagIrq < 0)
                    {
                        valueTAU.Add(TAUValue[keyTAU]);
                        if (valueListTAU == "")
                            valueListTAU = TAUValue[keyTAU];
                        else
                            valueListTAU = valueListTAU + "、" + TAUValue[keyTAU];
                    }
                    if (irq.IRQ_Add_Node("TAU", channel, ref irq_path))
                    {
                        flag_count++;
                        irq.IRQ_change_node(flag_count, 1, ref  irq_path);

                    } break;
                case 3:
                    int keyADC = Array.IndexOf(ADCKey.ToArray(), channel);
                    flagIrq = valueADC.IndexOf(ADCValue[keyADC]);
                    if (flagIrq < 0)
                    {
                        valueADC.Add(ADCValue[keyADC]);
                        if (valueListADC == "")
                            valueListADC = ADCValue[keyADC];
                        else
                            valueListADC = valueListADC + "、" + ADCValue[keyADC];
                    }
                    if (irq.IRQ_Add_Node("ADC", channel, ref irq_path))
                    {
                        flag_count++;
                        irq.IRQ_change_node(flag_count, 1, ref  irq_path);
                    }
                    break;
                case 4:
                    int keyCAN = Array.IndexOf(CANKey.ToArray(), channel);
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
                    if (irq.IRQ_Add_Node("CAN", channel, ref irq_path))
                    {
                        irq.IRQ_change_node(flag_count, 3, ref  irq_path);
                    }
                    break;
                case 5:
                    int keyUART = Array.IndexOf(UARTKey.ToArray(), channel);
                    flagIrq = valueUART.IndexOf(UARTValue[keyUART, 0]);
                    if (flagIrq < 0)
                    {
                        valueUART.Add(UARTValue[keyUART, 0]); valueUART.Add(UARTValue[keyUART, 1]); valueUART.Add(UARTValue[keyUART, 1]); valueUART.Add(UARTValue[keyUART, 2]); valueUART.Add(UARTValue[keyUART, 3]);
                        if (valueListUART == "")
                            valueListUART = UARTValue[keyUART, 0] + "、" + UARTValue[keyUART, 1] + "、" + UARTValue[keyUART, 2] + "、" + UARTValue[keyUART, 3];
                        else
                            valueListUART = valueListUART + "、" + UARTValue[keyUART, 0] + "、" + UARTValue[keyUART, 1] + "、" + UARTValue[keyUART, 2] + "、" + UARTValue[keyUART, 3];
                    }
                    if (irq.IRQ_Add_Node("UART", channel, ref irq_path))
                    {
                        flag_count += 4;
                        irq.IRQ_change_node(flag_count, 4, ref irq_path);
                    }
                    break;
                case 6:
                    int keyIIC = Array.IndexOf(IICKey.ToArray(), channel);
                    flagIrq = valueIIC.IndexOf(IICValue[keyIIC]);
                    if (flagIrq < 0)
                    {
                        valueIIC.Add(IICValue[keyIIC]); valueIIC.Add(IICValue[keyIIC + 1]); valueIIC.Add(IICValue[keyIIC + 1]); valueIIC.Add(IICValue[keyIIC + 2]); valueIIC.Add(IICValue[keyIIC + 3]);
                        if (valueListIIC == "")
                            valueListIIC = IICValue[keyIIC] + "、" + IICValue[keyIIC + 1] + "、" + IICValue[keyIIC + 2] + "、" + IICValue[keyIIC + 3];
                        else
                            valueListIIC = valueListIIC + " 、 " + IICValue[keyIIC] + "、" + IICValue[keyIIC + 1] + "、" + IICValue[keyIIC + 2] + "、" + IICValue[keyIIC + 3];
                    }
                    if (irq.IRQ_Add_Node("IIC", channel, ref irq_path))
                    {
                        flag_count += 4;
                        irq.IRQ_change_node(flag_count, 4, ref  irq_path);
                    }
                    break;
                case 7:
                    int keySPI = Array.IndexOf(SPIKey.ToArray(), channel);
                    flagIrq = valueSPI.IndexOf(SPIValue[keySPI, 0]);
                    if (flagIrq < 0)
                    {
                        valueSPI.Add(SPIValue[keySPI, 0]); valueSPI.Add(SPIValue[keySPI, 1]); valueSPI.Add(SPIValue[keySPI, 2]);
                        if (valueListSPI == "")
                            valueListSPI = SPIValue[keySPI, 0] + "、" + SPIValue[keySPI, 1] + "、" + SPIValue[keySPI, 2];
                        else
                            valueListSPI = valueListSPI + "、" + SPIValue[keySPI, 0] + "、" + SPIValue[keySPI, 1] + "、" + SPIValue[keySPI, 2];
                    }
                    if (irq.IRQ_Add_Node("SPI", channel, ref irq_path))
                    {
                        flag_count += 3;
                        irq.IRQ_change_node(flag_count, 3, ref  irq_path);
                    }
                    break;
                case 8:
                    int keyPWM = Array.IndexOf(PWMKey.ToArray(), channel);
                    flagIrq = valuePWM.IndexOf(PWMValue[keyPWM]);
                    if (flagIrq < 0)
                    {
                        valuePWM.Add(PWMValue[keyPWM]);
                        if (valueListPWM == "")
                            valueListPWM = PWMValue[keyPWM];
                        else
                            valueListPWM = valueListPWM + "、" + PWMValue[keyPWM];
                    }
                    if (irq.IRQ_Add_Node("PWM", channel, ref irq_path))
                    {
                        flag_count++;
                        irq.IRQ_change_node(flag_count, 1, ref  irq_path);
                    }
                    break;
                case 9:
                    int keyWD = Array.IndexOf(WDKey.ToArray(), channel);
                    flagIrq = valueWD.IndexOf(WDValue[keyWD]);
                    if (flagIrq < 0)
                    {
                        valueWD.Add(WDValue[keyWD]);
                        if (valueListWD == "")
                            valueListWD = WDValue[keyWD];
                        else
                            valueListWD = valueListWD + "、" + WDValue[keyWD];
                    }
                    if (irq.IRQ_Add_Node("WD", channel, ref irq_path))
                    {
                        flag_count++;
                        irq.IRQ_change_node(flag_count, 1, ref  irq_path);
                    }
                    break;
                case 10:
                    int keyDMA = Array.IndexOf(DMAKey.ToArray(), channel);
                    flagIrq = valueDMA.IndexOf(DMAValue[keyDMA]);
                    if (flagIrq < 0)
                    {
                        valueDMA.Add(DMAValue[keyDMA]);
                        if (valueListDMA == "")
                            valueListDMA = DMAValue[keyDMA];
                        else
                            valueListDMA = valueListDMA + "、" + DMAValue[keyDMA];
                    }
                    if (irq.IRQ_Add_Node("DMA", channel, ref irq_path))
                    {
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
                case 0: item_flag[0]++;
                    break;
                case 3: item_flag[1]++;
                    break;
                case 4: item_flag[2]++;
                    break;
                case 6: item_flag[3]++;
                    break;
                case 8: item_flag[4]++;
                    break;
                case 10: item_flag[5]++;
                    break;
                case 1: item_flag[6]++;
                    break;
                case 2: item_flag[7]++;
                    break;
                case 5: item_flag[8]++;
                    break;
                case 7: item_flag[9]++;
                    break;
                case 9: item_flag[10]++;
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
                case 3: item_flag[1]++;
                    break;
                case 4: item_flag[2]++;
                    break;
                case 6: item_flag[3]++;
                    break;
                case 8: item_flag[4]++;
                    break;
                /*
                case 10: irq_list_value[item_flag[5], 5] = comboBox11.SelectedItem.ToString(); item_flag[5]++;
                    break;
                 */
                case 1: item_flag[6]++;
                    break;
                case 2: item_flag[7]++;
                    break;
                case 5: item_flag[8]++;
                    break;
                case 7: item_flag[9]++;
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


        public void generateIRQ(ArrayList list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                String[] register = (String[])list[i];
                if (register[0].Equals("OSTimer"))
                {
                    irq.confirm_flag[0] = 1;
                    switch_combox(0, register[1]);
                    irq_switch_combox(0);
                    irq.confirm_flag[0] = 0;
                }
                if (register[0].Equals("External interrupt"))
                {
                    irq.confirm_flag[1] = 1;
                    switch_combox(1, register[1]);
                    irq_switch_combox(1);
                    irq.confirm_flag[1] = 0;

                }
                if (register[0].Equals("Timer Array Unit"))
                {
                    irq.confirm_flag[2] = 1;
                    switch_combox(2, register[1]);
                    irq_switch_combox(2);
                    irq.confirm_flag[2] = 0;
                }
                if (register[0].Equals("ADCA0"))
                {
                    irq.confirm_flag[3] = 1;
                    switch_combox(3, register[1]);
                    irq_switch_combox(3);
                    irq.confirm_flag[3] = 0;
                }
                if (register[0].Equals("CAN"))
                {
                    irq.confirm_flag[4] = 1;
                    switch_combox(4, register[1]);
                    irq_switch_combox(4);
                    irq.confirm_flag[4] = 0;
                }
                if (register[0].Equals("RLIN/UART"))
                {
                    irq.confirm_flag[5] = 1;
                    switch_combox(5, register[1]);
                    irq_switch_combox(5);
                    irq.confirm_flag[5] = 0;
                }
                if (register[0].Equals("IIC"))
                {
                    irq.confirm_flag[6] = 1;
                    switch_combox(6, register[1]);
                    irq_switch_combox(6);
                    irq.confirm_flag[6] = 0;
                }
                if (register[0].Equals("SPIH"))
                {
                    irq.confirm_flag[7] = 1;
                    switch_combox(7, register[1]);
                    irq_switch_combox(7);
                    irq.confirm_flag[7] = 0;
                }
                if (register[0].Equals("PWM"))
                {
                    irq.confirm_flag[8] = 1;
                    switch_combox(8, register[1]);
                    irq_switch_combox(8);
                    irq.confirm_flag[8] = 0;
                }
                if (register[0].Equals("WatchDog"))
                {
                    irq.confirm_flag[9] = 1;
                    switch_combox(9, register[1]);
                    irq_switch_combox(9);
                    irq.confirm_flag[9] = 0;
                }
                if (register[0].Equals("DMA"))
                {
                    irq.confirm_flag[10] = 1;
                    switch_combox(10, register[1]);
                    irq_switch_combox(10);
                    irq.confirm_flag[10] = 0;
                }
            }
        }
    }
}