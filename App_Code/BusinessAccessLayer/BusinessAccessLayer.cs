using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Security.Cryptography;
using System.Text;
using System.Net.Mail;
using System.Configuration;
using System.IO;
using System.Web.Script.Serialization;
using System.Collections;
using System.Globalization;
//using Excel = Microsoft.Office.Interop.Excel;



/// <summary>
/// Summary description for BusinessAccessLayer
/// </summary>
public class BusinessAccessLayer
{

    DataAccessLayer DAL = new DataAccessLayer();


    public BusinessAccessLayer()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    //static Excel._Application WA_Csv = new Excel.Application();
    ////static string workbookPath_Csv = @"C:\CASTROL\PRODUCTS\F-0\FORMULA\FORMULA1\1_F1.CSV";
    //static string workbookPath_Csv = "";





    #region BatchReport

    public DataTable getEquipmentData()
    {
        DataTable dt = new DataTable();
        try
        {
            dt = DAL.getEquipmentData();
        }
        catch (Exception ex)
        {
            this.LogError(ex);
        }
        return dt;
    }

    public DataTable getProductList(string blender_Name)
    {
        DataTable dt = new DataTable();
        try
        {
            dt = DAL.getProductList(blender_Name);
        }
        catch (Exception ex)
        {
            this.LogError(ex);
        }
        return dt;
    }

    public DataTable getMinMax(string Equipment_Name, string Batch_Number,string FDate, string EDate, string ViewName)
    {
        DataTable dt = new DataTable();
        try
        {
            dt = DAL.getMinMax(Equipment_Name, Batch_Number, FDate, EDate, ViewName);
        }
        catch (Exception ex)
        {
            this.LogError(ex);
        }
        return dt;
    }

    public DataTable getBatchNo(string Equipment_Name, string FDate, string EDate,string ViewName)
    {
        DataTable dt = new DataTable();
        try
        {
            dt = DAL.getBatchNo(Equipment_Name, FDate, EDate, ViewName);
        }
        catch (Exception ex)
        {
            this.LogError(ex);
        }
        return dt;
    }

    public DataTable getReportData(string Equipment_Name,string Batch_Number, string FDate, string EDate,string ViewName, string TimeInterval)
    {
        DataTable dt = new DataTable();
        DataTable dtFinalReport = new DataTable();
        try
        {

            dt = DAL.getReportData(Equipment_Name, Batch_Number, FDate, EDate, ViewName, TimeInterval);
           

        }
        catch (Exception ex)
        {

        }
        return dt;
    }

    #endregion


    #region Database
    public string  GetViewName(string Equipment_Name)
    {
        string ViewName = "";
        try
        {
              ViewName = DAL.GetViewName(Equipment_Name);
        }
        catch (Exception ex)
        {
            this.LogError(ex);
        }
        return ViewName;
    }
    public string  GetAuditTableName(string Equipment_Name)
    {
        string ViewName = "";
        DataTable dt = new DataTable();
        DataTable dt1 = new DataTable();
        try
        {
            if (Equipment_Name == "ROS1_Audit(R-110 and R-128)")
            {
                ViewName = "ROS1_Audit_table";
            }
            if (Equipment_Name == "ROS2_Audit(R-107 and R-131 and R-133)")
            {
                ViewName = "ROS2_TABLE";
            }
            if (Equipment_Name == "ROS3_Audit(R-112&R-116&R-125)")
            {
                ViewName = "ROS3_TABLE";
            }
            if (Equipment_Name == "ROS4_Audit(R-115&R-120&R-135)")
            {
                ViewName = "ROS4_TABLE";
            }
            if (Equipment_Name == "ROS5_Audit(R-127&R-139&R-138&R-113)")
            {
                ViewName = "ROS5_TABLE";
            }
            if (Equipment_Name == "ROS6_Audit(R-101&R-132&R-108)")
            {
                ViewName = "ROS6_TABLE";
            }
            if (Equipment_Name == "ROS7_Audit(R-104&R-105&R-137&R-142)")
            {
                ViewName = "ROS7_TABLE";
            }
            if (Equipment_Name == "ROS8_Audit(CF-105&CF-109&CF-112&RCVD-105)")
            {
                ViewName = "ROS8_TABLE";
            }
            if (Equipment_Name == "ROS9_Audit(FBD-102)")
            {
                ViewName = "ROS9_TABLE";
            }
            if (Equipment_Name == "ROS10_Audit(FBD-103)")
            {
                ViewName = "ROS10_TABLE";
            }
            if (Equipment_Name == "ROS11_Audit(FBD-104)")
            {
                ViewName = "ROS11_TABLE";
            }
            if (Equipment_Name == "ROS12_Audit(VTD-101&RCVD-102)")
            {
                ViewName = "ROS12_TABLE";
            }
            if (Equipment_Name == "OS13_Audit(Jetmill)")
            {
                ViewName = "ROS13_TABLE";
            }
            if (Equipment_Name == "OWS_H_table(HydrogenationR-134)")
            {
                ViewName = "OWS_H_table";
            }
            if (Equipment_Name == "OWS_table(Reactor&Solvent)")
            {
                ViewName = "OWS_table";
            }
            if (Equipment_Name == "EWS_table")
            {
                ViewName = "Audit";
            }


        }
        catch (Exception ex)
        {
            this.LogError(ex);
        }
        finally
        {

        }

        return ViewName;
    }

    #endregion

    #region Audit

    public DataTable getAuditReportData( string FDate, string EDate)
    {
        DataTable dt = new DataTable();
        DataTable dtFinalReport = new DataTable();
        try
        {

            dt = DAL.getAuditReportData(FDate, EDate);


        }
        catch (Exception ex)
        {

        }
        return dt;
    }

    #endregion
    #region ErrorLogs

    private void LogError(Exception ex)
    {
        string message = string.Format("Time: {0}", DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt"));
        message += Environment.NewLine;
        message += "-----------------------------------------------------------";
        message += Environment.NewLine;
        message += string.Format("Message: {0}", ex.Message);
        message += Environment.NewLine;
        message += string.Format("StackTrace: {0}", ex.StackTrace);
        message += Environment.NewLine;
        message += string.Format("Source: {0}", ex.Source);
        message += Environment.NewLine;
        message += string.Format("TargetSite: {0}", ex.TargetSite.ToString());
        message += Environment.NewLine;
        message += "-----------------------------------------------------------";
        message += Environment.NewLine;
        string path = System.Web.HttpContext.Current.Server.MapPath("~/Logs/BusinessLayerErrorLog.txt");
        using (StreamWriter writer = new StreamWriter(path, true))
        {
            writer.WriteLine(message);
            writer.Close();
        }
    }

    #endregion


    #region trendReport

    #region GetTrend RDLC name
    public string getRDLCname(string reactor)
    {
        string rdlcReportname = "";
        try
        {
            if (reactor == "FP-FBD-94" || reactor == "FP-FBD-120")
            {
                rdlcReportname = "Trend-FP-FBD-Report";
            }
            else if (reactor == "FP-VD-249" || reactor == "FP-VD-299")
            {
                rdlcReportname = "Trend-VD-Report";
            }
            else if (reactor == "FP-RCVD-300" || reactor == "FP-RCVD-301" || reactor == "FP-RCVD-302" || reactor == "FP-RCVD-95")
            {
                rdlcReportname = "Trend-RCVD-Report";
            }
            else if (reactor == "FP_HUMIDIFIER")
            {
                rdlcReportname = "FP_HUMIDIFIER";
            }


        }
        catch (Exception ex)
        {
            this.LogError(ex);
        }
        return rdlcReportname;
    }
    public string getBatchRDLCname(string reactor)
    {
        string rdlcReportname = "";
        try
        {
            if (reactor == "FP-FBD-94" || reactor == "FP-FBD-120")
            {
                rdlcReportname = "FP-FBD-Report";
            }
            else if (reactor == "FP-VD-249" || reactor == "FP-VD-299")
            {
                rdlcReportname = "FP-VD-Report";
            }
            else if (reactor == "FP-RCVD-300" || reactor == "FP-RCVD-301" || reactor == "FP-RCVD-302" || reactor == "FP-RCVD-95")
            {
                rdlcReportname = "FP-RCVD-Report";
            }
            else if (reactor == "FP_HUMIDIFIER")
            {
                rdlcReportname = "FP_HUMIDIFIER";
            }


        }
        catch (Exception ex)
        {
            this.LogError(ex);
        }
        return rdlcReportname;
    }
    public string getAuditRDLCname(string Equipment_Name)
    {
        string rdlcReportname = "";
        try
        {
         
         rdlcReportname = "AuditReport";
          

        }
        catch (Exception ex)
        {
            this.LogError(ex);
        }
        return rdlcReportname;
    }
    #endregion

    #endregion


    #region EventReport

    public DataTable getRepogetEventReportDatartData(string Equipment_Name, string Batch_Number, string FDate, string EDate, string ViewName)
    {
        DataTable dt = new DataTable();
        DataTable dtFinalReport = new DataTable();
        try
        {
            if (Equipment_Name == "FP-FBD-94" || Equipment_Name == "FP-FBD-120")
            {
                dt = DAL.getRepogetEventReportDatartData_FDB(Equipment_Name, Batch_Number, FDate, EDate, ViewName);
            }
            else
            {
                dt = DAL.getRepogetEventReportDatartData(Equipment_Name, Batch_Number, FDate, EDate, ViewName);

            }
        }
        catch (Exception ex)
        {

        }
        return dt;
    }
    #endregion


    #region solvent Report

    public DataTable getSolventBatchReportData(string Equipment_Name, string Batch_Number, string FDate, string EDate, string ViewName)
    {
        DataTable dt = new DataTable();
        DataTable dtFinalReport = new DataTable();
        try
        {

            dt = DAL.getSolventBatchReportData(Equipment_Name, Batch_Number, FDate, EDate, ViewName);


        }
        catch (Exception ex)
        {

        }
        return dt;
    }

    #endregion



}