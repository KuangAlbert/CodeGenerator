<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
<xsl:output method="text" indent="yes" encoding="ISO-8859-1"/>
<xsl:template match="/">
/***********************************************************************
*   Copyright (C) Huizhou Desay SV Automotive Co., Ltd.                *				  
*   All Rights Reserved.                                               *                                    					   
*   Department : RN RD SW1                                             *      									  
*   AUTHOR     : Shao Guangxian                                        *
************************************************************************
* Object        :  Mcu_pin_cc.h
* Module        :
* Instance      :
* Description   :
*-----------------------------------------------------------------------
* Version:
* Date:
* Author:
***********************************************************************/
/*-History--------------------------------------------------------------
* Version       Date           Name         Changes and comment
 <xsl:for-each select="GPIO_Set/INF">
 <xsl:for-each select="info_Group">
 ------------------------------------------------------------------------
  *<xsl:value-of select="@Version"/>         .<xsl:value-of select="@Date"/>    .<xsl:value-of select="@Name"/>     .<xsl:value-of select="@Comment"/>
 </xsl:for-each>
</xsl:for-each>
*=====================================================================*/
#ifndef _PIN_MAP_CC_H
#define _PIN_MAP_CC_H
#include "CONFIG.H"
#include "SystemMode_Cfg.h"
/*--------------------configration method-------------------
* 1. independent IO
    PMC:bit=0 -> PM
                         1:input  -> PIBC,PU,PD
                 
                         0:output->PBDC,PDSC,PODC
*2. alternative mode
   PMC:bit=1 ->PIPC
                         0   -> PFC,PFCE,PFCAE
                              ->PM
                                  1: input   -> same as 1.
                                  0:output-> same as 1.
                                   
                         1  ->PFC,PFCE,PFCAE
                       (directIO chapter2.11)
*---------------------------------------------------------*/
/*-- MCU_RH850_F1L:  PORT 0,8,9,10,11,JP0,AP0 --*/
#define MCU_PORT_NUM      5u
#define MCU_APORT_NUM     1u
#define MCU_JPORT_NUM     1u

/* PMC: 0=port mode 1=alternative */
 <xsl:for-each select="GPIO_Set/FUN">
 <xsl:if test="@Register='PMC'">    
 <xsl:for-each select="Group">
  <xsl:if test="@Group_Number='20'" >  
	<xsl:if test="@Mode='ACTIVE'" >  
#define GPIO_JPMC0_<xsl:value-of select="@Mode"/>_VAL         (<xsl:value-of select="value"/>u)</xsl:if>
 </xsl:if>
 <xsl:if test="@Group_Number &lt;'20'">  
	<xsl:if test="@Mode='ACTIVE'" >
#define GPIO_PMC<xsl:value-of select="@Group_Number"/>_<xsl:value-of select="@Mode"/>_VAL         (<xsl:value-of select="value"/>u)</xsl:if>
 </xsl:if>
 </xsl:for-each>
 </xsl:if> 
</xsl:for-each> 
//no APMC0

//no Anolog
#define GPIO_REG_NONE_VAL      0x0000  //none register
 
 /* Port IP Control: IN alternative mode, mask PM.  1=unuse PM */
  <xsl:for-each select="GPIO_Set/FUN">
 <xsl:if test="@Register='PIPC'"> 
 <xsl:for-each select="Group">
  <xsl:if test="@Group_Number='20'" >  
	<xsl:if test="@Mode='ACTIVE'" >
#define GPIO_JPIPC0_<xsl:value-of select="@Mode"/>_VAL         (<xsl:value-of select="value"/>u)</xsl:if>
 </xsl:if>
  <xsl:if test="@Group_Number='30'">  
	<xsl:if test="@Mode='ACTIVE'" >
#define GPIO_APIPC0_<xsl:value-of select="@Mode"/>_VAL         (<xsl:value-of select="value"/>u)</xsl:if>
 </xsl:if>
 <xsl:if test="@Group_Number &lt;'20'">  
	<xsl:if test="@Mode='ACTIVE'" >
#define GPIO_PIPC<xsl:value-of select="@Group_Number"/>_<xsl:value-of select="@Mode"/>_VAL         (<xsl:value-of select="value"/>u)</xsl:if>
 </xsl:if>
 </xsl:for-each>
 </xsl:if>
 </xsl:for-each>
//no Jtag
//no Anolog

/*PM: 0=output 1=input */
 <xsl:for-each select="GPIO_Set/FUN">
 <xsl:if test="@Register='PM'"> 
 <xsl:for-each select="Group">
  <xsl:if test="@Group_Number='20'" > 
	<xsl:if test="@Mode='ACTIVE'" >
#define GPIO_JPM0_<xsl:value-of select="@Mode"/>_VAL         (<xsl:value-of select="value"/>u)</xsl:if>
  </xsl:if>	
  <xsl:if test="@Group_Number='30'">  
	<xsl:if test="@Mode='ACTIVE'" >
#define GPIO_APM0_<xsl:value-of select="@Mode"/>_VAL         (<xsl:value-of select="value"/>u)</xsl:if>
 </xsl:if>
 <xsl:if test="@Group_Number &lt;'20'">  
	<xsl:if test="@Mode='ACTIVE'" >
#define GPIO_PM<xsl:value-of select="@Group_Number"/>_<xsl:value-of select="@Mode"/>_VAL         (<xsl:value-of select="value"/>u)</xsl:if>
 </xsl:if>
 </xsl:for-each>
 </xsl:if>
 </xsl:for-each>
 
/*Port InputBuffer Control: all 0,unused here */
 <xsl:for-each select="GPIO_Set/FUN">
 <xsl:if test="@Register='PIBC'"> 
 <xsl:for-each select="Group">
  <xsl:if test="@Group_Number='20'" >  
	<xsl:if test="@Mode='ACTIVE'" >
#define GPIO_JPIBC0_<xsl:value-of select="@Mode"/>_VAL         (<xsl:value-of select="value"/>u)</xsl:if>
 </xsl:if>
  <xsl:if test="@Group_Number='30'">  
	<xsl:if test="@Mode='ACTIVE'" >
#define GPIO_APIBC0_<xsl:value-of select="@Mode"/>_VAL         (<xsl:value-of select="value"/>u)</xsl:if>
 </xsl:if>
 <xsl:if test="@Group_Number &lt;'20'">  
	<xsl:if test="@Mode='ACTIVE'" >
#define GPIO_PIBC<xsl:value-of select="@Group_Number"/>_<xsl:value-of select="@Mode"/>_VAL         (<xsl:value-of select="value"/>u)</xsl:if>
 </xsl:if>
 </xsl:for-each>
 </xsl:if>
 </xsl:for-each>
 
 /* PFCAE,PFCE,PFC:IN alternative mode, select a alternative function in 0~4.
*  binary value [PFCAEn_m,PFCEn_m,PFCn_m]b  from 0~4  is a alternative function.
*/
 <xsl:for-each select="GPIO_Set/FUN">
 <xsl:if test="@Register='PFC'"> 
 <xsl:for-each select="Group">
  <xsl:if test="@Group_Number='20'" > 
	<xsl:if test="@Mode='ACTIVE'" >  
#define GPIO_JPFC0_<xsl:value-of select="@Mode"/>_VAL         (<xsl:value-of select="value"/>u)</xsl:if>
 </xsl:if>
 </xsl:for-each>
 </xsl:if>
 <xsl:if test="@Register='PFCE'"> 
 <xsl:for-each select="Group">
  <xsl:if test="@Group_Number='20'" >  
	<xsl:if test="@Mode='ACTIVE'" >
#define GPIO_JPFCE0_<xsl:value-of select="@Mode"/>_VAL         (<xsl:value-of select="value"/>u)</xsl:if>
 </xsl:if>
 </xsl:for-each>
 </xsl:if> 
 <xsl:if test="@Register='PFCAE'"> 
 <xsl:for-each select="Group">
  <xsl:if test="@Group_Number='20'" >  
	<xsl:if test="@Mode='ACTIVE'" >
#define GPIO_JPFCAE0_<xsl:value-of select="@Mode"/>_VAL         (<xsl:value-of select="value"/>u)</xsl:if>
 </xsl:if> 
 </xsl:for-each>
 </xsl:if> 
 </xsl:for-each>
  
 <xsl:for-each select="GPIO_Set/FUN">
 <xsl:if test="@Register='PFC'"> 
 <xsl:for-each select="Group"> 
  <xsl:if test="@Group_Number='30'">  
	<xsl:if test="@Mode='ACTIVE'" >
#define GPIO_APFC0_<xsl:value-of select="@Mode"/>_VAL         (<xsl:value-of select="value"/>u)</xsl:if>
 </xsl:if>
 </xsl:for-each>
 </xsl:if>
 <xsl:if test="@Register='PFCE'"> 
 <xsl:for-each select="Group">
  <xsl:if test="@Group_Number='30'" >  
	<xsl:if test="@Mode='ACTIVE'" >
#define GPIO_APFCE0_<xsl:value-of select="@Mode"/>_VAL         (<xsl:value-of select="value"/>u)</xsl:if>
 </xsl:if>
 </xsl:for-each>
 </xsl:if> 
 <xsl:if test="@Register='PFCAE'"> 
 <xsl:for-each select="Group">
  <xsl:if test="@Group_Number='30'" >  
	<xsl:if test="@Mode='ACTIVE'" >
#define GPIO_APFCAE0_<xsl:value-of select="@Mode"/>_VAL         (<xsl:value-of select="value"/>u)</xsl:if>
 </xsl:if> 
 </xsl:for-each>
 </xsl:if>
  </xsl:for-each>
  
 <xsl:for-each select="GPIO_Set/FUN">
 <xsl:if test="@Register='PFC'"> 
 <xsl:for-each select="Group"> 
  <xsl:if test="@Group_Number='0'">  
	<xsl:if test="@Mode='ACTIVE'" >
#define GPIO_PFC0_<xsl:value-of select="@Mode"/>_VAL         (<xsl:value-of select="value"/>u)</xsl:if>
 </xsl:if>
 </xsl:for-each>
 </xsl:if>
 <xsl:if test="@Register='PFCE'"> 
 <xsl:for-each select="Group">
  <xsl:if test="@Group_Number='0'">  
	<xsl:if test="@Mode='ACTIVE'" >
#define GPIO_PFCE0_<xsl:value-of select="@Mode"/>_VAL         (<xsl:value-of select="value"/>u)</xsl:if>
 </xsl:if>
 </xsl:for-each>
 </xsl:if> 
 <xsl:if test="@Register='PFCAE'"> 
 <xsl:for-each select="Group">
  <xsl:if test="@Group_Number='0'">  
	<xsl:if test="@Mode='ACTIVE'" >
#define GPIO_PFCAE0_<xsl:value-of select="@Mode"/>_VAL        (<xsl:value-of select="value"/>u)</xsl:if>
 </xsl:if> 
 </xsl:for-each>
 </xsl:if>
 </xsl:for-each>
  
  <xsl:for-each select="GPIO_Set/FUN">
 <xsl:if test="@Register='PFC'"> 
 <xsl:for-each select="Group"> 
  <xsl:if test="@Group_Number='8'">  
	<xsl:if test="@Mode='ACTIVE'" >
#define GPIO_PFC8_<xsl:value-of select="@Mode"/>_VAL         (<xsl:value-of select="value"/>u)</xsl:if>
 </xsl:if>
 </xsl:for-each>
 </xsl:if>
 <xsl:if test="@Register='PFCE'"> 
 <xsl:for-each select="Group">
  <xsl:if test="@Group_Number='8'">  
	<xsl:if test="@Mode='ACTIVE'" >
#define GPIO_PFCE8_<xsl:value-of select="@Mode"/>_VAL         (<xsl:value-of select="value"/>u)</xsl:if>
 </xsl:if>
 </xsl:for-each>
 </xsl:if> 
 <xsl:if test="@Register='PFCAE'"> 
 <xsl:for-each select="Group">
  <xsl:if test="@Group_Number='8'">  
	<xsl:if test="@Mode='ACTIVE'" >
#define GPIO_PFCAE8_<xsl:value-of select="@Mode"/>_VAL        (<xsl:value-of select="value"/>u)</xsl:if>
 </xsl:if> 
 </xsl:for-each>
 </xsl:if>
 </xsl:for-each>
  
  <xsl:for-each select="GPIO_Set/FUN">
 <xsl:if test="@Register='PFC'"> 
 <xsl:for-each select="Group"> 
  <xsl:if test="@Group_Number='9'">  
	<xsl:if test="@Mode='ACTIVE'" >
#define GPIO_PFC9_<xsl:value-of select="@Mode"/>_VAL         (<xsl:value-of select="value"/>u)</xsl:if>
 </xsl:if>
 </xsl:for-each>
 </xsl:if>
 <xsl:if test="@Register='PFCE'"> 
 <xsl:for-each select="Group">
  <xsl:if test="@Group_Number='9'">  
	<xsl:if test="@Mode='ACTIVE'" >
#define GPIO_PFCE9_<xsl:value-of select="@Mode"/>_VAL         (<xsl:value-of select="value"/>u)</xsl:if>
 </xsl:if>
 </xsl:for-each>
 </xsl:if> 
 <xsl:if test="@Register='PFCAE'"> 
 <xsl:for-each select="Group">
  <xsl:if test="@Group_Number='9'">  
	<xsl:if test="@Mode='ACTIVE'" >
#define GPIO_PFCAE9_<xsl:value-of select="@Mode"/>_VAL        (<xsl:value-of select="value"/>u)</xsl:if>
 </xsl:if> 
 </xsl:for-each>
 </xsl:if>
 </xsl:for-each>
  
  <xsl:for-each select="GPIO_Set/FUN">
 <xsl:if test="@Register='PFC'"> 
 <xsl:for-each select="Group"> 
  <xsl:if test="@Group_Number='10'">  
	<xsl:if test="@Mode='ACTIVE'" >
#define GPIO_PFC10_<xsl:value-of select="@Mode"/>_VAL         (<xsl:value-of select="value"/>u)</xsl:if>
 </xsl:if>
 </xsl:for-each>
 </xsl:if>
 <xsl:if test="@Register='PFCE'"> 
 <xsl:for-each select="Group">
  <xsl:if test="@Group_Number='10'">  
	<xsl:if test="@Mode='ACTIVE'" >
#define GPIO_PFCE10_<xsl:value-of select="@Mode"/>_VAL         (<xsl:value-of select="value"/>u)</xsl:if>
 </xsl:if>
 </xsl:for-each>
 </xsl:if> 
 <xsl:if test="@Register='PFCAE'"> 
 <xsl:for-each select="Group">
  <xsl:if test="@Group_Number='10'">  
	<xsl:if test="@Mode='ACTIVE'" >
#define GPIO_PFCAE10_<xsl:value-of select="@Mode"/>_VAL        (<xsl:value-of select="value"/>u)</xsl:if>
 </xsl:if> 
 </xsl:for-each>
 </xsl:if>
 </xsl:for-each>
  
  <xsl:for-each select="GPIO_Set/FUN">
 <xsl:if test="@Register='PFC'"> 
 <xsl:for-each select="Group"> 
  <xsl:if test="@Group_Number='11'">  
	<xsl:if test="@Mode='ACTIVE'" >
#define GPIO_PFC11_<xsl:value-of select="@Mode"/>_VAL         (<xsl:value-of select="value"/>u)</xsl:if>
 </xsl:if>
 </xsl:for-each>
 </xsl:if>
 <xsl:if test="@Register='PFCE'"> 
 <xsl:for-each select="Group">
  <xsl:if test="@Group_Number='11'">  
	<xsl:if test="@Mode='ACTIVE'" >
#define GPIO_PFCE11_<xsl:value-of select="@Mode"/>_VAL         (<xsl:value-of select="value"/>u)</xsl:if>
 </xsl:if>
 </xsl:for-each>
 </xsl:if> 
 <xsl:if test="@Register='PFCAE'"> 
 <xsl:for-each select="Group">
  <xsl:if test="@Group_Number='11'">  
	<xsl:if test="@Mode='ACTIVE'" >
#define GPIO_PFCAE11_<xsl:value-of select="@Mode"/>_VAL        (<xsl:value-of select="value"/>u)</xsl:if>
 </xsl:if> 
 </xsl:for-each>
 </xsl:if>
 </xsl:for-each>

/*PullUp Control: input mode.  1= internal pullup./* Care that PUn.PUn_m=1 and PDn.PDn_m=1, then the real Pin is pulldown*/
 <xsl:for-each select="GPIO_Set/FUN">
 <xsl:if test="@Register='PU'"> 
 <xsl:for-each select="Group">
   <xsl:if test="@Group_Number='20'" >  
	<xsl:if test="@Mode='ACTIVE'" >
#define GPIO_JPU0_<xsl:value-of select="@Mode"/>_VAL         (<xsl:value-of select="value"/>u)</xsl:if>
 </xsl:if>
  <xsl:if test="@Group_Number='30'">  
	<xsl:if test="@Mode='ACTIVE'" >
#define GPIO_APU0_<xsl:value-of select="@Mode"/>_VAL         (<xsl:value-of select="value"/>u)</xsl:if>
 </xsl:if>
 <xsl:if test="@Group_Number &lt;'20'">  
	<xsl:if test="@Mode='ACTIVE'" >
#define GPIO_PU<xsl:value-of select="@Group_Number"/>_<xsl:value-of select="@Mode"/>_VAL         (<xsl:value-of select="value"/>u)</xsl:if>
 </xsl:if>
 </xsl:for-each>
 </xsl:if>
</xsl:for-each>

/*PullDown Control: input mode.  1= internal pulldown */
<xsl:for-each select="GPIO_Set/FUN">
 <xsl:if test="@Register='PD'"> 
 <xsl:for-each select="Group">
   <xsl:if test="@Group_Number='20'" >  
	<xsl:if test="@Mode='ACTIVE'" >
#define GPIO_JPD0_<xsl:value-of select="@Mode"/>_VAL         (<xsl:value-of select="value"/>u)</xsl:if>
 </xsl:if>
  <xsl:if test="@Group_Number='30'">  
	<xsl:if test="@Mode='ACTIVE'" >
#define GPIO_APD0_<xsl:value-of select="@Mode"/>_VAL         (<xsl:value-of select="value"/>u)</xsl:if>
 </xsl:if>
 <xsl:if test="@Group_Number &lt;'20'"> 
	<xsl:if test="@Mode='ACTIVE'" > 
#define GPIO_PD<xsl:value-of select="@Group_Number"/>_<xsl:value-of select="@Mode"/>_VAL         (<xsl:value-of select="value"/>u)</xsl:if>
 </xsl:if>
 </xsl:for-each>
 </xsl:if>
</xsl:for-each>

/*Port Bidirection Control: 1= Bidirection */
<xsl:for-each select="GPIO_Set/FUN">
 <xsl:if test="@Register='PBDC'"> 
 <xsl:for-each select="Group">
   <xsl:if test="@Group_Number='20'" >  
	<xsl:if test="@Mode='ACTIVE'" >
#define GPIO_JPBDC0_<xsl:value-of select="@Mode"/>_VAL         (<xsl:value-of select="value"/>u)</xsl:if>
 </xsl:if>
  <xsl:if test="@Group_Number='30'">  
	<xsl:if test="@Mode='ACTIVE'" >
#define GPIO_APBDC0_<xsl:value-of select="@Mode"/>_VAL         (<xsl:value-of select="value"/>u)</xsl:if>
 </xsl:if>
 <xsl:if test="@Group_Number &lt;'20'">  
	<xsl:if test="@Mode='ACTIVE'" >
#define GPIO_PBDC<xsl:value-of select="@Group_Number"/>_<xsl:value-of select="@Mode"/>_VAL         (<xsl:value-of select="value"/>u)</xsl:if>
 </xsl:if>
 </xsl:for-each>
 </xsl:if>
</xsl:for-each>

/*Port Driver Strength: output mode.*/
<xsl:for-each select="GPIO_Set/FUN">
 <xsl:if test="@Register='PDSC'"> 
 <xsl:for-each select="Group">
    <xsl:if test="@Group_Number='20'" > 
		<xsl:if test="@Mode='ACTIVE'" >	
#define GPIO_JPDSC0_<xsl:value-of select="@Mode"/>_VAL         (<xsl:value-of select="value"/>u)</xsl:if>
	</xsl:if>
  <xsl:if test="@Group_Number='30'">  
	<xsl:if test="@Mode='ACTIVE'" >
#define GPIO_APDSC0_<xsl:value-of select="@Mode"/>_VAL         (<xsl:value-of select="value"/>u)</xsl:if>
 </xsl:if>
 <xsl:if test="@Group_Number &lt;'20'">  
	<xsl:if test="@Mode='ACTIVE'" >
#define GPIO_PDSC<xsl:value-of select="@Group_Number"/>_<xsl:value-of select="@Mode"/>_VAL         (<xsl:value-of select="value"/>u)</xsl:if>
 </xsl:if>
 </xsl:for-each>
 </xsl:if>
</xsl:for-each>

/*Port open drain control: output mode. 0:push-pull, 1:open-drain*/
<xsl:for-each select="GPIO_Set/FUN">
 <xsl:if test="@Register='PODC'"> 
 <xsl:for-each select="Group">
    <xsl:if test="@Group_Number='20'" >  
		<xsl:if test="@Mode='ACTIVE'" >
#define GPIO_JPODC0_<xsl:value-of select="@Mode"/>_VAL         (<xsl:value-of select="value"/>u)</xsl:if>
	</xsl:if>
  <xsl:if test="@Group_Number='30'">  
	<xsl:if test="@Mode='ACTIVE'" >
#define GPIO_APODC0_<xsl:value-of select="@Mode"/>_VAL         (<xsl:value-of select="value"/>u)</xsl:if>
 </xsl:if>
 <xsl:if test="@Group_Number &lt;'20'">  
	<xsl:if test="@Mode='ACTIVE'" >
#define GPIO_PODC<xsl:value-of select="@Group_Number"/>_<xsl:value-of select="@Mode"/>_VAL         (<xsl:value-of select="value"/>u)</xsl:if>
 </xsl:if>
 </xsl:for-each>
 </xsl:if>
</xsl:for-each>

/* Set default for output pin */
<xsl:for-each select="GPIO_Set/FUN">
 <xsl:if test="@Register='PV'"> 
 <xsl:for-each select="Group">
    <xsl:if test="@Group_Number='20'" >  
		<xsl:if test="@Mode='ACTIVE'" >
#define GPIO_JP0_<xsl:value-of select="@Mode"/>_VAL         (<xsl:value-of select="value"/>u)</xsl:if>
	</xsl:if>
  <xsl:if test="@Group_Number='30'">  
	<xsl:if test="@Mode='ACTIVE'" >
#define GPIO_AP0_<xsl:value-of select="@Mode"/>_VAL         (<xsl:value-of select="value"/>u)</xsl:if>
 </xsl:if>
 <xsl:if test="@Group_Number &lt;'20'">  
	<xsl:if test="@Mode='ACTIVE'" >
#define GPIO_P<xsl:value-of select="@Group_Number"/>_<xsl:value-of select="@Mode"/>_VAL         (<xsl:value-of select="value"/>u)</xsl:if>
 </xsl:if>
 </xsl:for-each>
 </xsl:if>
</xsl:for-each>


/*----------------------- GPIO RESET ---------------------------- */
/* PMC: 0=port mode 1=alternative */
 <xsl:for-each select="GPIO_Set/FUN">
 <xsl:if test="@Register='PMC'">    
 <xsl:for-each select="Group">
  <xsl:if test="@Group_Number='20'" >  
	<xsl:if test="@Mode='RESET'" >  
#define GPIO_JPMC0_<xsl:value-of select="@Mode"/>_VAL         (<xsl:value-of select="value"/>u)</xsl:if>
 </xsl:if>
  <xsl:if test="@Group_Number='30'">  
    <xsl:if test="@Mode='RESET'" > 
#define GPIO_APMC0_<xsl:value-of select="@Mode"/>_VAL         (<xsl:value-of select="value"/>u)</xsl:if>
 </xsl:if>
 <xsl:if test="@Group_Number &lt;'20'">  
	<xsl:if test="@Mode='RESET'" >
#define GPIO_PMC<xsl:value-of select="@Group_Number"/>_<xsl:value-of select="@Mode"/>_VAL         (<xsl:value-of select="value"/>u)</xsl:if>
 </xsl:if>
 </xsl:for-each>
 </xsl:if> 
</xsl:for-each> 

//no Anolog
#define GPIO_REG_NONE_VAL      0x0000  //none register
 
 /* Port IP Control: IN alternative mode, mask PM.  1=unuse PM */
  <xsl:for-each select="GPIO_Set/FUN">
 <xsl:if test="@Register='PIPC'"> 
 <xsl:for-each select="Group">
  <xsl:if test="@Group_Number='20'" >  
	<xsl:if test="@Mode='RESET'" >
#define GPIO_JPIPC0_<xsl:value-of select="@Mode"/>_VAL         (<xsl:value-of select="value"/>u)</xsl:if>
 </xsl:if>
  <xsl:if test="@Group_Number='30'">  
	<xsl:if test="@Mode='RESET'" >
#define GPIO_APIPC0_<xsl:value-of select="@Mode"/>_VAL         (<xsl:value-of select="value"/>u)</xsl:if>
 </xsl:if>
 <xsl:if test="@Group_Number &lt;'20'">  
	<xsl:if test="@Mode='RESET'" >
#define GPIO_PIPC<xsl:value-of select="@Group_Number"/>_<xsl:value-of select="@Mode"/>_VAL         (<xsl:value-of select="value"/>u)</xsl:if>
 </xsl:if>
 </xsl:for-each>
 </xsl:if>
 </xsl:for-each>
//no Jtag
//no Anolog

/*PM: 0=output 1=input */
 <xsl:for-each select="GPIO_Set/FUN">
 <xsl:if test="@Register='PM'"> 
 <xsl:for-each select="Group">
  <xsl:if test="@Group_Number='20'" > 
	<xsl:if test="@Mode='RESET'" >
#define GPIO_JPM0_<xsl:value-of select="@Mode"/>_VAL         (<xsl:value-of select="value"/>u)</xsl:if>
  </xsl:if>	
  <xsl:if test="@Group_Number='30'">  
	<xsl:if test="@Mode='RESET'" >
#define GPIO_APM0_<xsl:value-of select="@Mode"/>_VAL         (<xsl:value-of select="value"/>u)</xsl:if>
 </xsl:if>
 <xsl:if test="@Group_Number &lt;'20'">  
	<xsl:if test="@Mode='RESET'" >
#define GPIO_PM<xsl:value-of select="@Group_Number"/>_<xsl:value-of select="@Mode"/>_VAL         (<xsl:value-of select="value"/>u)</xsl:if>
 </xsl:if>
 </xsl:for-each>
 </xsl:if>
 </xsl:for-each>
 
/*Port InputBuffer Control: all 0,unused here */
 <xsl:for-each select="GPIO_Set/FUN">
 <xsl:if test="@Register='PIBC'"> 
 <xsl:for-each select="Group">
  <xsl:if test="@Group_Number='20'" >  
	<xsl:if test="@Mode='RESET'" >
#define GPIO_JPIBC0_<xsl:value-of select="@Mode"/>_VAL         (<xsl:value-of select="value"/>u)</xsl:if>
 </xsl:if>
  <xsl:if test="@Group_Number='30'">  
	<xsl:if test="@Mode='RESET'" >
#define GPIO_APIBC0_<xsl:value-of select="@Mode"/>_VAL         (<xsl:value-of select="value"/>u)</xsl:if>
 </xsl:if>
 <xsl:if test="@Group_Number &lt;'20'">  
	<xsl:if test="@Mode='RESET'" >
#define GPIO_PIBC<xsl:value-of select="@Group_Number"/>_<xsl:value-of select="@Mode"/>_VAL         (<xsl:value-of select="value"/>u)</xsl:if>
 </xsl:if>
 </xsl:for-each>
 </xsl:if>
 </xsl:for-each>
 
 /* PFCAE,PFCE,PFC:IN alternative mode, select a alternative function in 0~4.
*  binary value [PFCAEn_m,PFCEn_m,PFCn_m]b  from 0~4  is a alternative function.
*/
 <xsl:for-each select="GPIO_Set/FUN">
 <xsl:if test="@Register='PFC'"> 
 <xsl:for-each select="Group">
  <xsl:if test="@Group_Number='20'" > 
	<xsl:if test="@Mode='RESET'" >  
#define GPIO_JPFC0_<xsl:value-of select="@Mode"/>_VAL         (<xsl:value-of select="value"/>u)</xsl:if>
 </xsl:if>
 </xsl:for-each>
 </xsl:if>
 <xsl:if test="@Register='PFCE'"> 
 <xsl:for-each select="Group">
  <xsl:if test="@Group_Number='20'" >  
	<xsl:if test="@Mode='RESET'" >
#define GPIO_JPFCE0_<xsl:value-of select="@Mode"/>_VAL         (<xsl:value-of select="value"/>u)</xsl:if>
 </xsl:if>
 </xsl:for-each>
 </xsl:if> 
 <xsl:if test="@Register='PFCAE'"> 
 <xsl:for-each select="Group">
  <xsl:if test="@Group_Number='20'" >  
	<xsl:if test="@Mode='RESET'" >
#define GPIO_JPFCAE0_<xsl:value-of select="@Mode"/>_VAL         (<xsl:value-of select="value"/>u)</xsl:if>
 </xsl:if> 
 </xsl:for-each>
 </xsl:if> 
 </xsl:for-each>
  
 <xsl:for-each select="GPIO_Set/FUN">
 <xsl:if test="@Register='PFC'"> 
 <xsl:for-each select="Group"> 
  <xsl:if test="@Group_Number='30'">  
	<xsl:if test="@Mode='RESET'" >
#define GPIO_APFC0_<xsl:value-of select="@Mode"/>_VAL         (<xsl:value-of select="value"/>u)</xsl:if>
 </xsl:if>
 </xsl:for-each>
 </xsl:if>
 <xsl:if test="@Register='PFCE'"> 
 <xsl:for-each select="Group">
  <xsl:if test="@Group_Number='30'" >  
	<xsl:if test="@Mode='RESET'" >
#define GPIO_APFCE0_<xsl:value-of select="@Mode"/>_VAL         (<xsl:value-of select="value"/>u)</xsl:if>
 </xsl:if>
 </xsl:for-each>
 </xsl:if> 
 <xsl:if test="@Register='PFCAE'"> 
 <xsl:for-each select="Group">
  <xsl:if test="@Group_Number='30'" >  
	<xsl:if test="@Mode='RESET'" >
#define GPIO_APFCAE0_<xsl:value-of select="@Mode"/>_VAL         (<xsl:value-of select="value"/>u)</xsl:if>
 </xsl:if> 
 </xsl:for-each>
 </xsl:if>
  </xsl:for-each>
  
 <xsl:for-each select="GPIO_Set/FUN">
 <xsl:if test="@Register='PFC'"> 
 <xsl:for-each select="Group"> 
  <xsl:if test="@Group_Number='0'">  
	<xsl:if test="@Mode='RESET'" >
#define GPIO_PFC0_<xsl:value-of select="@Mode"/>_VAL         (<xsl:value-of select="value"/>u)</xsl:if>
 </xsl:if>
 </xsl:for-each>
 </xsl:if>
 <xsl:if test="@Register='PFCE'"> 
 <xsl:for-each select="Group">
  <xsl:if test="@Group_Number='0'">  
	<xsl:if test="@Mode='RESET'" >
#define GPIO_PFCE0_<xsl:value-of select="@Mode"/>_VAL         (<xsl:value-of select="value"/>u)</xsl:if>
 </xsl:if>
 </xsl:for-each>
 </xsl:if> 
 <xsl:if test="@Register='PFCAE'"> 
 <xsl:for-each select="Group">
  <xsl:if test="@Group_Number='0'">  
	<xsl:if test="@Mode='RESET'" >
#define GPIO_PFCAE0_<xsl:value-of select="@Mode"/>_VAL        (<xsl:value-of select="value"/>u)</xsl:if>
 </xsl:if> 
 </xsl:for-each>
 </xsl:if>
 </xsl:for-each>
  
  <xsl:for-each select="GPIO_Set/FUN">
 <xsl:if test="@Register='PFC'"> 
 <xsl:for-each select="Group"> 
  <xsl:if test="@Group_Number='8'">  
	<xsl:if test="@Mode='RESET'" >
#define GPIO_PFC8_<xsl:value-of select="@Mode"/>_VAL         (<xsl:value-of select="value"/>u)</xsl:if>
 </xsl:if>
 </xsl:for-each>
 </xsl:if>
 <xsl:if test="@Register='PFCE'"> 
 <xsl:for-each select="Group">
  <xsl:if test="@Group_Number='8'">  
	<xsl:if test="@Mode='RESET'" >
#define GPIO_PFCE8_<xsl:value-of select="@Mode"/>_VAL         (<xsl:value-of select="value"/>u)</xsl:if>
 </xsl:if>
 </xsl:for-each>
 </xsl:if> 
 <xsl:if test="@Register='PFCAE'"> 
 <xsl:for-each select="Group">
  <xsl:if test="@Group_Number='8'">  
	<xsl:if test="@Mode='RESET'" >
#define GPIO_PFCAE8_<xsl:value-of select="@Mode"/>_VAL        (<xsl:value-of select="value"/>u)</xsl:if>
 </xsl:if> 
 </xsl:for-each>
 </xsl:if>
 </xsl:for-each>
  
  <xsl:for-each select="GPIO_Set/FUN">
 <xsl:if test="@Register='PFC'"> 
 <xsl:for-each select="Group"> 
  <xsl:if test="@Group_Number='9'">  
	<xsl:if test="@Mode='RESET'" >
#define GPIO_PFC9_<xsl:value-of select="@Mode"/>_VAL         (<xsl:value-of select="value"/>u)</xsl:if>
 </xsl:if>
 </xsl:for-each>
 </xsl:if>
 <xsl:if test="@Register='PFCE'"> 
 <xsl:for-each select="Group">
  <xsl:if test="@Group_Number='9'">  
	<xsl:if test="@Mode='RESET'" >
#define GPIO_PFCE9_<xsl:value-of select="@Mode"/>_VAL         (<xsl:value-of select="value"/>u)</xsl:if>
 </xsl:if>
 </xsl:for-each>
 </xsl:if> 
 <xsl:if test="@Register='PFCAE'"> 
 <xsl:for-each select="Group">
  <xsl:if test="@Group_Number='9'">  
	<xsl:if test="@Mode='RESET'" >
#define GPIO_PFCAE9_<xsl:value-of select="@Mode"/>_VAL        (<xsl:value-of select="value"/>u)</xsl:if>
 </xsl:if> 
 </xsl:for-each>
 </xsl:if>
 </xsl:for-each>
  
  <xsl:for-each select="GPIO_Set/FUN">
 <xsl:if test="@Register='PFC'"> 
 <xsl:for-each select="Group"> 
  <xsl:if test="@Group_Number='10'">  
	<xsl:if test="@Mode='RESET'" >
#define GPIO_PFC10_<xsl:value-of select="@Mode"/>_VAL         (<xsl:value-of select="value"/>u)</xsl:if>
 </xsl:if>
 </xsl:for-each>
 </xsl:if>
 <xsl:if test="@Register='PFCE'"> 
 <xsl:for-each select="Group">
  <xsl:if test="@Group_Number='10'">  
	<xsl:if test="@Mode='RESET'" >
#define GPIO_PFCE10_<xsl:value-of select="@Mode"/>_VAL         (<xsl:value-of select="value"/>u)</xsl:if>
 </xsl:if>
 </xsl:for-each>
 </xsl:if> 
 <xsl:if test="@Register='PFCAE'"> 
 <xsl:for-each select="Group">
  <xsl:if test="@Group_Number='10'">  
	<xsl:if test="@Mode='RESET'" >
#define GPIO_PFCAE10_<xsl:value-of select="@Mode"/>_VAL        (<xsl:value-of select="value"/>u)</xsl:if>
 </xsl:if> 
 </xsl:for-each>
 </xsl:if>
 </xsl:for-each>
  
  <xsl:for-each select="GPIO_Set/FUN">
 <xsl:if test="@Register='PFC'"> 
 <xsl:for-each select="Group"> 
  <xsl:if test="@Group_Number='11'">  
	<xsl:if test="@Mode='RESET'" >
#define GPIO_PFC11_<xsl:value-of select="@Mode"/>_VAL         (<xsl:value-of select="value"/>u)</xsl:if>
 </xsl:if>
 </xsl:for-each>
 </xsl:if>
 <xsl:if test="@Register='PFCE'"> 
 <xsl:for-each select="Group">
  <xsl:if test="@Group_Number='11'">  
	<xsl:if test="@Mode='RESET'" >
#define GPIO_PFCE11_<xsl:value-of select="@Mode"/>_VAL         (<xsl:value-of select="value"/>u)</xsl:if>
 </xsl:if>
 </xsl:for-each>
 </xsl:if> 
 <xsl:if test="@Register='PFCAE'"> 
 <xsl:for-each select="Group">
  <xsl:if test="@Group_Number='11'">  
	<xsl:if test="@Mode='RESET'" >
#define GPIO_PFCAE11_<xsl:value-of select="@Mode"/>_VAL        (<xsl:value-of select="value"/>u)</xsl:if>
 </xsl:if> 
 </xsl:for-each>
 </xsl:if>
 </xsl:for-each>
 
/*PullUp Control: input mode.  1= internal pullup./* Care that PUn.PUn_m=1 and PDn.PDn_m=1, then the real Pin is pulldown*/
 <xsl:for-each select="GPIO_Set/FUN">
 <xsl:if test="@Register='PU'"> 
 <xsl:for-each select="Group">
   <xsl:if test="@Group_Number='20'" >  
	<xsl:if test="@Mode='RESET'" >
#define GPIO_JPU0_<xsl:value-of select="@Mode"/>_VAL         (<xsl:value-of select="value"/>u)</xsl:if>
 </xsl:if>
  <xsl:if test="@Group_Number='30'">  
	<xsl:if test="@Mode='RESET'" >
#define GPIO_APU0_<xsl:value-of select="@Mode"/>_VAL         (<xsl:value-of select="value"/>u)</xsl:if>
 </xsl:if>
 <xsl:if test="@Group_Number &lt;'20'">  
	<xsl:if test="@Mode='RESET'" >
#define GPIO_PU<xsl:value-of select="@Group_Number"/>_<xsl:value-of select="@Mode"/>_VAL         (<xsl:value-of select="value"/>u)</xsl:if>
 </xsl:if>
 </xsl:for-each>
 </xsl:if>
</xsl:for-each>

/*PullDown Control: input mode.  1= internal pulldown */
<xsl:for-each select="GPIO_Set/FUN">
 <xsl:if test="@Register='PD'"> 
 <xsl:for-each select="Group">
   <xsl:if test="@Group_Number='20'" >  
	<xsl:if test="@Mode='RESET'" >
#define GPIO_JPD0_<xsl:value-of select="@Mode"/>_VAL         (<xsl:value-of select="value"/>u)</xsl:if>
 </xsl:if>
  <xsl:if test="@Group_Number='30'">  
	<xsl:if test="@Mode='RESET'" >
#define GPIO_APD0_<xsl:value-of select="@Mode"/>_VAL         (<xsl:value-of select="value"/>u)</xsl:if>
 </xsl:if>
 <xsl:if test="@Group_Number &lt;'20'"> 
	<xsl:if test="@Mode='RESET'" > 
#define GPIO_PD<xsl:value-of select="@Group_Number"/>_<xsl:value-of select="@Mode"/>_VAL         (<xsl:value-of select="value"/>u)</xsl:if>
 </xsl:if>
 </xsl:for-each>
 </xsl:if>
</xsl:for-each>

/*Port Bidirection Control: 1= Bidirection */
<xsl:for-each select="GPIO_Set/FUN">
 <xsl:if test="@Register='PBDC'"> 
 <xsl:for-each select="Group">
   <xsl:if test="@Group_Number='20'" >  
	<xsl:if test="@Mode='RESET'" >
#define GPIO_JPBDC0_<xsl:value-of select="@Mode"/>_VAL         (<xsl:value-of select="value"/>u)</xsl:if>
 </xsl:if>
  <xsl:if test="@Group_Number='30'">  
	<xsl:if test="@Mode='RESET'" >
#define GPIO_APBDC0_<xsl:value-of select="@Mode"/>_VAL         (<xsl:value-of select="value"/>u)</xsl:if>
 </xsl:if>
 <xsl:if test="@Group_Number &lt;'20'">  
	<xsl:if test="@Mode='RESET'" >
#define GPIO_PBDC<xsl:value-of select="@Group_Number"/>_<xsl:value-of select="@Mode"/>_VAL         (<xsl:value-of select="value"/>u)</xsl:if>
 </xsl:if>
 </xsl:for-each>
 </xsl:if>
</xsl:for-each>

/*Port Driver Strength: output mode.*/
<xsl:for-each select="GPIO_Set/FUN">
 <xsl:if test="@Register='PDSC'"> 
 <xsl:for-each select="Group">
    <xsl:if test="@Group_Number='20'" > 
		<xsl:if test="@Mode='RESET'" >	
#define GPIO_JPDSC0_<xsl:value-of select="@Mode"/>_VAL         (<xsl:value-of select="value"/>u)</xsl:if>
	</xsl:if>
  <xsl:if test="@Group_Number='30'">  
	<xsl:if test="@Mode='RESET'" >
#define GPIO_APDSC0_<xsl:value-of select="@Mode"/>_VAL         (<xsl:value-of select="value"/>u)</xsl:if>
 </xsl:if>
 <xsl:if test="@Group_Number &lt;'20'">  
	<xsl:if test="@Mode='RESET'" >
#define GPIO_PDSC<xsl:value-of select="@Group_Number"/>_<xsl:value-of select="@Mode"/>_VAL         (<xsl:value-of select="value"/>u)</xsl:if>
 </xsl:if>
 </xsl:for-each>
 </xsl:if>
</xsl:for-each>

/*Port open drain control: output mode. 0:push-pull, 1:open-drain*/
<xsl:for-each select="GPIO_Set/FUN">
 <xsl:if test="@Register='PODC'"> 
 <xsl:for-each select="Group">
    <xsl:if test="@Group_Number='20'" >  
		<xsl:if test="@Mode='RESET'" >
#define GPIO_JPODC0_<xsl:value-of select="@Mode"/>_VAL         (<xsl:value-of select="value"/>u)</xsl:if>
	</xsl:if>
  <xsl:if test="@Group_Number='30'">  
	<xsl:if test="@Mode='RESET'" >
#define GPIO_APODC0_<xsl:value-of select="@Mode"/>_VAL         (<xsl:value-of select="value"/>u)</xsl:if>
 </xsl:if>
 <xsl:if test="@Group_Number &lt;'20'">  
	<xsl:if test="@Mode='RESET'" >
#define GPIO_PODC<xsl:value-of select="@Group_Number"/>_<xsl:value-of select="@Mode"/>_VAL         (<xsl:value-of select="value"/>u)</xsl:if>
 </xsl:if>
 </xsl:for-each>
 </xsl:if>
</xsl:for-each>

/* Set default for output pin */
<xsl:for-each select="GPIO_Set/FUN">
 <xsl:if test="@Register='PV'"> 
 <xsl:for-each select="Group">
    <xsl:if test="@Group_Number='20'" >  
		<xsl:if test="@Mode='RESET'" >
#define GPIO_JP0_<xsl:value-of select="@Mode"/>_VAL         (<xsl:value-of select="value"/>u)</xsl:if>
	</xsl:if>
  <xsl:if test="@Group_Number='30'">  
	<xsl:if test="@Mode='RESET'" >
#define GPIO_AP0_<xsl:value-of select="@Mode"/>_VAL         (<xsl:value-of select="value"/>u)</xsl:if>
 </xsl:if>
 <xsl:if test="@Group_Number &lt;'20'">  
	<xsl:if test="@Mode='RESET'" >
#define GPIO_P<xsl:value-of select="@Group_Number"/>_<xsl:value-of select="@Mode"/>_VAL         (<xsl:value-of select="value"/>u)</xsl:if>
 </xsl:if>
 </xsl:for-each>
 </xsl:if>
</xsl:for-each>

/*----------------------- GPIO STANDBY ---------------------------- */
/* PMC: 0=port mode 1=alternative */
 <xsl:for-each select="GPIO_Set/FUN">
 <xsl:if test="@Register='PMC'">    
 <xsl:for-each select="Group">
  <xsl:if test="@Group_Number='20'" >  
	<xsl:if test="@Mode='STANDBY'" >  
#define GPIO_JPMC0_<xsl:value-of select="@Mode"/>_VAL         (<xsl:value-of select="value"/>u)</xsl:if>
 </xsl:if>
  <xsl:if test="@Group_Number='30'">  
    <xsl:if test="@Mode='STANDBY'" > 
#define GPIO_APMC0_<xsl:value-of select="@Mode"/>_VAL         (<xsl:value-of select="value"/>u)</xsl:if>
 </xsl:if>
 <xsl:if test="@Group_Number &lt;'20'">  
	<xsl:if test="@Mode='STANDBY'" >
#define GPIO_PMC<xsl:value-of select="@Group_Number"/>_<xsl:value-of select="@Mode"/>_VAL         (<xsl:value-of select="value"/>u)</xsl:if>
 </xsl:if>
 </xsl:for-each>
 </xsl:if> 
</xsl:for-each> 

//no Anolog
#define GPIO_REG_NONE_VAL      0x0000  //none register
 
 /* Port IP Control: IN alternative mode, mask PM.  1=unuse PM */
  <xsl:for-each select="GPIO_Set/FUN">
 <xsl:if test="@Register='PIPC'"> 
 <xsl:for-each select="Group">
  <xsl:if test="@Group_Number='20'" >  
	<xsl:if test="@Mode='STANDBY'" >
#define GPIO_JPIPC0_<xsl:value-of select="@Mode"/>_VAL         (<xsl:value-of select="value"/>u)</xsl:if>
 </xsl:if>
  <xsl:if test="@Group_Number='30'">  
	<xsl:if test="@Mode='STANDBY'" >
#define GPIO_APIPC0_<xsl:value-of select="@Mode"/>_VAL         (<xsl:value-of select="value"/>u)</xsl:if>
 </xsl:if>
 <xsl:if test="@Group_Number &lt;'20'">  
	<xsl:if test="@Mode='STANDBY'" >
#define GPIO_PIPC<xsl:value-of select="@Group_Number"/>_<xsl:value-of select="@Mode"/>_VAL         (<xsl:value-of select="value"/>u)</xsl:if>
 </xsl:if>
 </xsl:for-each>
 </xsl:if>
 </xsl:for-each>
//no Jtag
//no Anolog

/*PM: 0=output 1=input */
 <xsl:for-each select="GPIO_Set/FUN">
 <xsl:if test="@Register='PM'"> 
 <xsl:for-each select="Group">
  <xsl:if test="@Group_Number='20'" > 
	<xsl:if test="@Mode='STANDBY'" >
#define GPIO_JPM0_<xsl:value-of select="@Mode"/>_VAL         (<xsl:value-of select="value"/>u)</xsl:if>
  </xsl:if>	
  <xsl:if test="@Group_Number='30'">  
	<xsl:if test="@Mode='STANDBY'" >
#define GPIO_APM0_<xsl:value-of select="@Mode"/>_VAL         (<xsl:value-of select="value"/>u)</xsl:if>
 </xsl:if>
 <xsl:if test="@Group_Number &lt;'20'">  
	<xsl:if test="@Mode='STANDBY'" >
#define GPIO_PM<xsl:value-of select="@Group_Number"/>_<xsl:value-of select="@Mode"/>_VAL         (<xsl:value-of select="value"/>u)</xsl:if>
 </xsl:if>
 </xsl:for-each>
 </xsl:if>
 </xsl:for-each>
 
/*Port InputBuffer Control: all 0,unused here */
 <xsl:for-each select="GPIO_Set/FUN">
 <xsl:if test="@Register='PIBC'"> 
 <xsl:for-each select="Group">
  <xsl:if test="@Group_Number='20'" >  
	<xsl:if test="@Mode='STANDBY'" >
#define GPIO_JPIBC0_<xsl:value-of select="@Mode"/>_VAL         (<xsl:value-of select="value"/>u)</xsl:if>
 </xsl:if>
  <xsl:if test="@Group_Number='30'">  
	<xsl:if test="@Mode='STANDBY'" >
#define GPIO_APIBC0_<xsl:value-of select="@Mode"/>_VAL         (<xsl:value-of select="value"/>u)</xsl:if>
 </xsl:if>
 <xsl:if test="@Group_Number &lt;'20'">  
	<xsl:if test="@Mode='STANDBY'" >
#define GPIO_PIBC<xsl:value-of select="@Group_Number"/>_<xsl:value-of select="@Mode"/>_VAL         (<xsl:value-of select="value"/>u)</xsl:if>
 </xsl:if>
 </xsl:for-each>
 </xsl:if>
 </xsl:for-each>
 
 /* PFCAE,PFCE,PFC:IN alternative mode, select a alternative function in 0~4.
*  binary value [PFCAEn_m,PFCEn_m,PFCn_m]b  from 0~4  is a alternative function.
*/
 <xsl:for-each select="GPIO_Set/FUN">
 <xsl:if test="@Register='PFC'"> 
 <xsl:for-each select="Group">
  <xsl:if test="@Group_Number='20'" > 
	<xsl:if test="@Mode='STANDBY'" >  
#define GPIO_JPFC0_<xsl:value-of select="@Mode"/>_VAL         (<xsl:value-of select="value"/>u)</xsl:if>
 </xsl:if>
 </xsl:for-each>
 </xsl:if>
 <xsl:if test="@Register='PFCE'"> 
 <xsl:for-each select="Group">
  <xsl:if test="@Group_Number='20'" >  
	<xsl:if test="@Mode='STANDBY'" >
#define GPIO_JPFCE0_<xsl:value-of select="@Mode"/>_VAL         (<xsl:value-of select="value"/>u)</xsl:if>
 </xsl:if>
 </xsl:for-each>
 </xsl:if> 
 <xsl:if test="@Register='PFCAE'"> 
 <xsl:for-each select="Group">
  <xsl:if test="@Group_Number='20'" >  
	<xsl:if test="@Mode='STANDBY'" >
#define GPIO_JPFCAE0_<xsl:value-of select="@Mode"/>_VAL         (<xsl:value-of select="value"/>u)</xsl:if>
 </xsl:if> 
 </xsl:for-each>
 </xsl:if> 
 </xsl:for-each>
  
 <xsl:for-each select="GPIO_Set/FUN">
 <xsl:if test="@Register='PFC'"> 
 <xsl:for-each select="Group"> 
  <xsl:if test="@Group_Number='30'">  
	<xsl:if test="@Mode='STANDBY'" >
#define GPIO_APFC0_<xsl:value-of select="@Mode"/>_VAL         (<xsl:value-of select="value"/>u)</xsl:if>
 </xsl:if>
 </xsl:for-each>
 </xsl:if>
 <xsl:if test="@Register='PFCE'"> 
 <xsl:for-each select="Group">
  <xsl:if test="@Group_Number='30'" >  
	<xsl:if test="@Mode='STANDBY'" >
#define GPIO_APFCE0_<xsl:value-of select="@Mode"/>_VAL         (<xsl:value-of select="value"/>u)</xsl:if>
 </xsl:if>
 </xsl:for-each>
 </xsl:if> 
 <xsl:if test="@Register='PFCAE'"> 
 <xsl:for-each select="Group">
  <xsl:if test="@Group_Number='30'" >  
	<xsl:if test="@Mode='STANDBY'" >
#define GPIO_APFCAE0_<xsl:value-of select="@Mode"/>_VAL         (<xsl:value-of select="value"/>u)</xsl:if>
 </xsl:if> 
 </xsl:for-each>
 </xsl:if>
  </xsl:for-each>
  
 <xsl:for-each select="GPIO_Set/FUN">
 <xsl:if test="@Register='PFC'"> 
 <xsl:for-each select="Group"> 
  <xsl:if test="@Group_Number='0'">  
	<xsl:if test="@Mode='STANDBY'" >
#define GPIO_PFC0_<xsl:value-of select="@Mode"/>_VAL         (<xsl:value-of select="value"/>u)</xsl:if>
 </xsl:if>
 </xsl:for-each>
 </xsl:if>
 <xsl:if test="@Register='PFCE'"> 
 <xsl:for-each select="Group">
  <xsl:if test="@Group_Number='0'">  
	<xsl:if test="@Mode='STANDBY'" >
#define GPIO_PFCE0_<xsl:value-of select="@Mode"/>_VAL         (<xsl:value-of select="value"/>u)</xsl:if>
 </xsl:if>
 </xsl:for-each>
 </xsl:if> 
 <xsl:if test="@Register='PFCAE'"> 
 <xsl:for-each select="Group">
  <xsl:if test="@Group_Number='0'">  
	<xsl:if test="@Mode='STANDBY'" >
#define GPIO_PFCAE0_<xsl:value-of select="@Mode"/>_VAL        (<xsl:value-of select="value"/>u)</xsl:if>
 </xsl:if> 
 </xsl:for-each>
 </xsl:if>
 </xsl:for-each>
  
  <xsl:for-each select="GPIO_Set/FUN">
 <xsl:if test="@Register='PFC'"> 
 <xsl:for-each select="Group"> 
  <xsl:if test="@Group_Number='8'">  
	<xsl:if test="@Mode='STANDBY'" >
#define GPIO_PFC8_<xsl:value-of select="@Mode"/>_VAL         (<xsl:value-of select="value"/>u)</xsl:if>
 </xsl:if>
 </xsl:for-each>
 </xsl:if>
 <xsl:if test="@Register='PFCE'"> 
 <xsl:for-each select="Group">
  <xsl:if test="@Group_Number='8'">  
	<xsl:if test="@Mode='STANDBY'" >
#define GPIO_PFCE8_<xsl:value-of select="@Mode"/>_VAL         (<xsl:value-of select="value"/>u)</xsl:if>
 </xsl:if>
 </xsl:for-each>
 </xsl:if> 
 <xsl:if test="@Register='PFCAE'"> 
 <xsl:for-each select="Group">
  <xsl:if test="@Group_Number='8'">  
	<xsl:if test="@Mode='STANDBY'" >
#define GPIO_PFCAE8_<xsl:value-of select="@Mode"/>_VAL        (<xsl:value-of select="value"/>u)</xsl:if>
 </xsl:if> 
 </xsl:for-each>
 </xsl:if>
 </xsl:for-each>
  
  <xsl:for-each select="GPIO_Set/FUN">
 <xsl:if test="@Register='PFC'"> 
 <xsl:for-each select="Group"> 
  <xsl:if test="@Group_Number='9'">  
	<xsl:if test="@Mode='STANDBY'" >
#define GPIO_PFC9_<xsl:value-of select="@Mode"/>_VAL         (<xsl:value-of select="value"/>u)</xsl:if>
 </xsl:if>
 </xsl:for-each>
 </xsl:if>
 <xsl:if test="@Register='PFCE'"> 
 <xsl:for-each select="Group">
  <xsl:if test="@Group_Number='9'">  
	<xsl:if test="@Mode='STANDBY'" >
#define GPIO_PFCE9_<xsl:value-of select="@Mode"/>_VAL         (<xsl:value-of select="value"/>u)</xsl:if>
 </xsl:if>
 </xsl:for-each>
 </xsl:if> 
 <xsl:if test="@Register='PFCAE'"> 
 <xsl:for-each select="Group">
  <xsl:if test="@Group_Number='9'">  
	<xsl:if test="@Mode='STANDBY'" >
#define GPIO_PFCAE9_<xsl:value-of select="@Mode"/>_VAL        (<xsl:value-of select="value"/>u)</xsl:if>
 </xsl:if> 
 </xsl:for-each>
 </xsl:if>
 </xsl:for-each>
  
  <xsl:for-each select="GPIO_Set/FUN">
 <xsl:if test="@Register='PFC'"> 
 <xsl:for-each select="Group"> 
  <xsl:if test="@Group_Number='10'">  
	<xsl:if test="@Mode='STANDBY'" >
#define GPIO_PFC10_<xsl:value-of select="@Mode"/>_VAL         (<xsl:value-of select="value"/>u)</xsl:if>
 </xsl:if>
 </xsl:for-each>
 </xsl:if>
 <xsl:if test="@Register='PFCE'"> 
 <xsl:for-each select="Group">
  <xsl:if test="@Group_Number='10'">  
	<xsl:if test="@Mode='STANDBY'" >
#define GPIO_PFCE10_<xsl:value-of select="@Mode"/>_VAL         (<xsl:value-of select="value"/>u)</xsl:if>
 </xsl:if>
 </xsl:for-each>
 </xsl:if> 
 <xsl:if test="@Register='PFCAE'"> 
 <xsl:for-each select="Group">
  <xsl:if test="@Group_Number='10'">  
	<xsl:if test="@Mode='STANDBY'" >
#define GPIO_PFCAE10_<xsl:value-of select="@Mode"/>_VAL        (<xsl:value-of select="value"/>u)</xsl:if>
 </xsl:if> 
 </xsl:for-each>
 </xsl:if>
 </xsl:for-each>
  
  <xsl:for-each select="GPIO_Set/FUN">
 <xsl:if test="@Register='PFC'"> 
 <xsl:for-each select="Group"> 
  <xsl:if test="@Group_Number='11'">  
	<xsl:if test="@Mode='STANDBY'" >
#define GPIO_PFC11_<xsl:value-of select="@Mode"/>_VAL         (<xsl:value-of select="value"/>u)</xsl:if>
 </xsl:if>
 </xsl:for-each>
 </xsl:if>
 <xsl:if test="@Register='PFCE'"> 
 <xsl:for-each select="Group">
  <xsl:if test="@Group_Number='11'">  
	<xsl:if test="@Mode='STANDBY'" >
#define GPIO_PFCE11_<xsl:value-of select="@Mode"/>_VAL         (<xsl:value-of select="value"/>u)</xsl:if>
 </xsl:if>
 </xsl:for-each>
 </xsl:if> 
 <xsl:if test="@Register='PFCAE'"> 
 <xsl:for-each select="Group">
  <xsl:if test="@Group_Number='11'">  
	<xsl:if test="@Mode='STANDBY'" >
#define GPIO_PFCAE11_<xsl:value-of select="@Mode"/>_VAL        (<xsl:value-of select="value"/>u)</xsl:if>
 </xsl:if> 
 </xsl:for-each>
 </xsl:if>
 </xsl:for-each>

/*PullUp Control: input mode.  1= internal pullup./* Care that PUn.PUn_m=1 and PDn.PDn_m=1, then the real Pin is pulldown*/
 <xsl:for-each select="GPIO_Set/FUN">
 <xsl:if test="@Register='PU'"> 
 <xsl:for-each select="Group">
   <xsl:if test="@Group_Number='20'" >  
	<xsl:if test="@Mode='STANDBY'" >
#define GPIO_JPU0_<xsl:value-of select="@Mode"/>_VAL         (<xsl:value-of select="value"/>u)</xsl:if>
 </xsl:if>
  <xsl:if test="@Group_Number='30'">  
	<xsl:if test="@Mode='STANDBY'" >
#define GPIO_APU0_<xsl:value-of select="@Mode"/>_VAL         (<xsl:value-of select="value"/>u)</xsl:if>
 </xsl:if>
 <xsl:if test="@Group_Number &lt;'20'">  
	<xsl:if test="@Mode='STANDBY'" >
#define GPIO_PU<xsl:value-of select="@Group_Number"/>_<xsl:value-of select="@Mode"/>_VAL         (<xsl:value-of select="value"/>u)</xsl:if>
 </xsl:if>
 </xsl:for-each>
 </xsl:if>
</xsl:for-each>

/*PullDown Control: input mode.  1= internal pulldown */
<xsl:for-each select="GPIO_Set/FUN">
 <xsl:if test="@Register='PD'"> 
 <xsl:for-each select="Group">
   <xsl:if test="@Group_Number='20'" >  
	<xsl:if test="@Mode='STANDBY'" >
#define GPIO_JPD0_<xsl:value-of select="@Mode"/>_VAL         (<xsl:value-of select="value"/>u)</xsl:if>
 </xsl:if>
  <xsl:if test="@Group_Number='30'">  
	<xsl:if test="@Mode='STANDBY'" >
#define GPIO_APD0_<xsl:value-of select="@Mode"/>_VAL         (<xsl:value-of select="value"/>u)</xsl:if>
 </xsl:if>
 <xsl:if test="@Group_Number &lt;'20'"> 
	<xsl:if test="@Mode='STANDBY'" > 
#define GPIO_PD<xsl:value-of select="@Group_Number"/>_<xsl:value-of select="@Mode"/>_VAL         (<xsl:value-of select="value"/>u)</xsl:if>
 </xsl:if>
 </xsl:for-each>
 </xsl:if>
</xsl:for-each>

/*Port Bidirection Control: 1= Bidirection */
<xsl:for-each select="GPIO_Set/FUN">
 <xsl:if test="@Register='PBDC'"> 
 <xsl:for-each select="Group">
   <xsl:if test="@Group_Number='20'" >  
	<xsl:if test="@Mode='STANDBY'" >
#define GPIO_JPBDC0_<xsl:value-of select="@Mode"/>_VAL         (<xsl:value-of select="value"/>u)</xsl:if>
 </xsl:if>
  <xsl:if test="@Group_Number='30'">  
	<xsl:if test="@Mode='STANDBY'" >
#define GPIO_APBDC0_<xsl:value-of select="@Mode"/>_VAL         (<xsl:value-of select="value"/>u)</xsl:if>
 </xsl:if>
 <xsl:if test="@Group_Number &lt;'20'">  
	<xsl:if test="@Mode='STANDBY'" >
#define GPIO_PBDC<xsl:value-of select="@Group_Number"/>_<xsl:value-of select="@Mode"/>_VAL         (<xsl:value-of select="value"/>u)</xsl:if>
 </xsl:if>
 </xsl:for-each>
 </xsl:if>
</xsl:for-each>

/*Port Driver Strength: output mode.*/
<xsl:for-each select="GPIO_Set/FUN">
 <xsl:if test="@Register='PDSC'"> 
 <xsl:for-each select="Group">
    <xsl:if test="@Group_Number='20'" > 
		<xsl:if test="@Mode='STANDBY'" >	
#define GPIO_JPDSC0_<xsl:value-of select="@Mode"/>_VAL         (<xsl:value-of select="value"/>u)</xsl:if>
	</xsl:if>
  <xsl:if test="@Group_Number='30'">  
	<xsl:if test="@Mode='STANDBY'" >
#define GPIO_APDSC0_<xsl:value-of select="@Mode"/>_VAL         (<xsl:value-of select="value"/>u)</xsl:if>
 </xsl:if>
 <xsl:if test="@Group_Number &lt;'20'">  
	<xsl:if test="@Mode='STANDBY'" >
#define GPIO_PDSC<xsl:value-of select="@Group_Number"/>_<xsl:value-of select="@Mode"/>_VAL         (<xsl:value-of select="value"/>u)</xsl:if>
 </xsl:if>
 </xsl:for-each>
 </xsl:if>
</xsl:for-each>

/*Port open drain control: output mode. 0:push-pull, 1:open-drain*/
<xsl:for-each select="GPIO_Set/FUN">
 <xsl:if test="@Register='PODC'"> 
 <xsl:for-each select="Group">
    <xsl:if test="@Group_Number='20'" >  
		<xsl:if test="@Mode='STANDBY'" >
#define GPIO_JPODC0_<xsl:value-of select="@Mode"/>_VAL         (<xsl:value-of select="value"/>u)</xsl:if>
	</xsl:if>
  <xsl:if test="@Group_Number='30'">  
	<xsl:if test="@Mode='STANDBY'" >
#define GPIO_APODC0_<xsl:value-of select="@Mode"/>_VAL         (<xsl:value-of select="value"/>u)</xsl:if>
 </xsl:if>
 <xsl:if test="@Group_Number &lt;'20'">  
	<xsl:if test="@Mode='STANDBY'" >
#define GPIO_PODC<xsl:value-of select="@Group_Number"/>_<xsl:value-of select="@Mode"/>_VAL         (<xsl:value-of select="value"/>u)</xsl:if>
 </xsl:if>
 </xsl:for-each>
 </xsl:if>
</xsl:for-each>

/* Set default for output pin */
<xsl:for-each select="GPIO_Set/FUN">
 <xsl:if test="@Register='PV'"> 
 <xsl:for-each select="Group">
    <xsl:if test="@Group_Number='20'" >  
		<xsl:if test="@Mode='STANDBY'" >
#define GPIO_JP0_<xsl:value-of select="@Mode"/>_VAL         (<xsl:value-of select="value"/>u)</xsl:if>
	</xsl:if>
  <xsl:if test="@Group_Number='30'">  
	<xsl:if test="@Mode='STANDBY'" >
#define GPIO_AP0_<xsl:value-of select="@Mode"/>_VAL         (<xsl:value-of select="value"/>u)</xsl:if>
 </xsl:if>
 <xsl:if test="@Group_Number &lt;'20'">  
	<xsl:if test="@Mode='STANDBY'" >
#define GPIO_P<xsl:value-of select="@Group_Number"/>_<xsl:value-of select="@Mode"/>_VAL         (<xsl:value-of select="value"/>u)</xsl:if>
 </xsl:if>
 </xsl:for-each>
 </xsl:if>
</xsl:for-each>

 #endif //_PIN_MAP_CC_H
/**************** END OF FILE *****************************************/
</xsl:template>
</xsl:stylesheet>