using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Configuration;
using System.IO;
using System.Globalization;

/// <summary>
/// Summary description for DataAccessLayer
/// </summary>
/// 

public class DataAccessLayer
{

    static string strcon = ConfigurationManager.ConnectionStrings["FP-RCVD-300"].ConnectionString;
    SqlConnection con;


    #region BatchReport

    public DataTable getEquipmentData()
    {
        string strcon = ConfigurationManager.ConnectionStrings["db"].ConnectionString;
        SqlConnection con;
        DataTable dt = new DataTable();
        using (con = new SqlConnection(strcon))
        {
            try
            {
                con.Open();

                SqlCommand cmd = new SqlCommand("select * from MINT_BlenderName", con);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);

                sda.Fill(dt);
            }
            catch (Exception ex)
            {
                this.LogError(ex);
            }
            finally
            {
                con.Close();
                SqlConnection.ClearPool(con);
            }
        }
        return dt;
    }

    public DataTable getProductList(string Equipment_Name)
    {
        string selectedValue = Equipment_Name;
        string strcon = System.Configuration.ConfigurationManager.ConnectionStrings[selectedValue].ConnectionString;
        SqlConnection con;
        DataTable dt = new DataTable();
        DataTable dt1 = new DataTable();
        using (con = new SqlConnection(strcon))
        {
            try
            {
                con.Open();
                SqlCommand cmd1 = new SqlCommand("select ViewName from MINT_BlenderName where BlenderName=@BlenderName", con);
                cmd1.ExecuteNonQuery();
                SqlDataAdapter sda1 = new SqlDataAdapter(cmd1);
                sda1.Fill(dt1);

                SqlCommand cmd = new SqlCommand("Select distinct PRODUCT_NAME from " + dt1.Rows[0]["ViewName"] + "", con);
                //cmd.Parameters.AddWithValue("@ViewName", dt1.Rows[0]["ViewName"]);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);

                sda.Fill(dt);
            }
            catch (Exception ex)
            {
                this.LogError(ex);
            }
            finally
            {
                con.Close();
                SqlConnection.ClearPool(con);
            }
        }
        return dt;

    }

    public DataTable getMinMax(string Equipment_Name, string Batch_Number, string FDate, string EDate,string ViewName)
    {
        string selectedValue = Equipment_Name;
        string strcon = System.Configuration.ConfigurationManager.ConnectionStrings[selectedValue].ConnectionString;

        DataTable dt = new DataTable();
        DataTable dt1 = new DataTable();
        using (con = new SqlConnection(strcon))
        {
            try
            {
     
                con.Open();
                //SqlCommand cmd1 = new SqlCommand("select min(DateAndTime) as min_time ,max(DateAndTime) as max_time from " + dt1.Rows[0]["ViewName"] + " where DateAndTime between '" + FDate + "' and '" + EDate + "' and BatchNo ='" + Batch_Number + "'  ", con);
                SqlCommand cmd1 = new SqlCommand("select min(DateAndTime) as min_time ,max(DateAndTime) as max_time from " + ViewName + " where DateAndTime between '" + FDate + "' and '" + EDate + "' and BatchNo ='" + Batch_Number + "'  ", con);

                cmd1.Parameters.AddWithValue("@BlenderName", Equipment_Name);
                cmd1.ExecuteNonQuery();
                SqlDataAdapter sda1 = new SqlDataAdapter(cmd1);
                sda1.Fill(dt);

               
            }
            catch (Exception ex)
            {
                this.LogError(ex);
            }
            finally
            {
                con.Close();
                SqlConnection.ClearPool(con);
            }
        }
        return dt;
    }

    public DataTable getBatchNo(string Equipment_Name, string FDate, string EDate,string ViewName)
    {
        string selectedValue = Equipment_Name;
        string strcon = System.Configuration.ConfigurationManager.ConnectionStrings[selectedValue].ConnectionString;

        DataTable dt = new DataTable();
        DataTable dt1 = new DataTable();
        using (con = new SqlConnection(strcon))
        {
            try
            {
                //con.Open();
                //SqlCommand cmd = new SqlCommand("select ViewName from MINT_BlenderName where EquipmentName='" + Equipment_Name + "'", con);
                //cmd.Parameters.AddWithValue("@EquipmentName", Equipment_Name);
                //cmd.ExecuteNonQuery();
                //SqlDataAdapter sda = new SqlDataAdapter(cmd);
                //sda.Fill(dt1);

                con.Open();
                //  SqlCommand cmd1 = new SqlCommand("Select Distinct(BatchNo) as BatchNo from " + dt1.Rows[0]["ViewName"] + " where DateAndTime between '" + FDate + "' and '" + EDate + "' and BatchNo !='' ", con);
                SqlCommand cmd1 = new SqlCommand("Select Distinct(BatchNo) as BatchNo from " + ViewName + " where DateAndTime between '" + FDate + "' and '" + EDate + "' and BatchNo !='' ", con);

                // cmd1.Parameters.AddWithValue("@BlenderName", Equipment_Name);
                cmd1.ExecuteNonQuery();
                SqlDataAdapter sda1 = new SqlDataAdapter(cmd1);
                sda1.Fill(dt);


            }
            catch (Exception ex)
            {
                this.LogError(ex);
            }
            finally
            {
                con.Close();
                SqlConnection.ClearPool(con);
            }
        }
        return dt;
    }

    public DataTable getReportData(string Equipment_Name, string Batch_Number, string FDate, string EDate, string ViewName , string TimeInterval)
    {
        string selectedValue = Equipment_Name;
        string strcon = System.Configuration.ConfigurationManager.ConnectionStrings[selectedValue].ConnectionString;

        DataTable dt = new DataTable();
        DataTable dtDttm = new DataTable();
        DataTable dt1 = new DataTable();
        double BatchTime = 0.0;
        string PrintBy = "";
        string query;
        using (con = new SqlConnection(strcon))
        {
            try
            {
                
                con.Open();
                if(TimeInterval=="1sec")
                {
                     query = @"Select * from " + ViewName + " WHERE(DateAndTime >= '" + FDate + "' and DateAndTime <= '" + EDate + "')and BatchNo = '" + Batch_Number + "'order by DateAndTime asc";
                 
                }
                else
                {
                     query = @"WITH added_row_number AS (SELECT*,ROW_NUMBER() OVER(PARTITION BY CONVERT(Char(16), [DateAndTime] , 20) ORDER BY[DateAndTime] ASC) AS row_number FROM   " + ViewName + "   WHERE(DateAndTime >= '" + FDate + "' and DateAndTime <= '" + EDate + "')and BatchNo = '" + Batch_Number + "')SELECT* FROM added_row_number WHERE row_number = 1 and DATEPART(MINUTE, DATEADD(MINUTE, " + TimeInterval + ", [DateAndTime])) % " + TimeInterval + " = 00  order by DateAndTime asc ";
                }
                //SqlCommand cmd1 = new SqlCommand("Select Distinct(BatchNo) as BatchNo from " + dt1.Rows[0]["ViewName"] + " where DateAndTime between '" + FDate + "' and '" + EDate + "' and BatchNo !='' ", con);
                //SqlCommand cmd1 = new SqlCommand("Select * from "+ ViewName + " where DateAndTime between '" + FDate + "' and '" + EDate + "' and BatchNo !='' ", con);
                //SqlCommand cmd1 = new SqlCommand("WITH added_row_number AS (SELECT*,ROW_NUMBER() OVER(PARTITION BY CONVERT(Char(16), [DateAndTime] , 20) ORDER BY[DateAndTime] ASC) AS row_number FROM   "+ ViewName + "   WHERE(DateAndTime >= '" + FDate + "' and DateAndTime <= '" + EDate + "')and BatchNo = '" + Batch_Number + "')SELECT* FROM added_row_number WHERE row_number = 1 and DATEPART(MINUTE, DATEADD(MINUTE, " + TimeInterval + ", [DateAndTime])) % " + TimeInterval + " = 00  order by DateAndTime asc ", con);
                SqlCommand cmd1 = new SqlCommand(query, con);
                cmd1.CommandTimeout = 120000;
                cmd1.ExecuteNonQuery();
                SqlDataAdapter sda1 = new SqlDataAdapter(cmd1);
                sda1.Fill(dt);
                string printby = getprintdata();

                {   if (dt.Rows.Count > 0)
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        
                        DataColumn dcTimeInterval = new DataColumn("TimeInterval", typeof(System.String));
                        DataColumn dcToDate = new DataColumn("ToDate", typeof(System.String));
                        DataColumn dcPrintBy = new DataColumn("PrintBy", typeof(System.String));
                        dt.Columns.Add(dcTimeInterval);
                        dt.Columns.Add(dcToDate);
                        dt.Columns.Add(dcPrintBy);
                        dt.Rows[i]["TimeInterval"] = TimeInterval;
                        dt.Rows[i]["ToDate"] = EDate;  
                       dt.Rows[i]["PrintBy"] = printby;
                    }
                }
               

            }
            catch (Exception ex)
            {
                this.LogError(ex);
            }
            finally
            {
                con.Close();
                SqlConnection.ClearPool(con);
            }
        }
        return dt;
    }

    public string getprintdata()
    {
        string selectedValue = "AUDIT";
        string strcon = System.Configuration.ConfigurationManager.ConnectionStrings[selectedValue].ConnectionString;

        DataTable dt = new DataTable();
        DataTable dtDttm = new DataTable();
        DataTable dt1 = new DataTable();
        double BatchTime = 0.0;
        string PrintBy = "";

        using (con = new SqlConnection(strcon))
        {
            try
            {
               
                con.Open();

                SqlCommand cmd1 = new SqlCommand("Select top 1 DATEADD(MINUTE, 30, DATEADD(HOUR, 5, TimeStmp)) AS TimeStmp ,UserID,Severity,Location,[MessageText] from Audit order by DATEADD(MINUTE, 30, DATEADD(HOUR, 5, TimeStmp)) desc ", con);
              
                cmd1.CommandTimeout = 120000;
                cmd1.ExecuteNonQuery();
                SqlDataAdapter sda1 = new SqlDataAdapter(cmd1);
                sda1.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    string userName = Convert.ToString(dt.Rows[0]["UserID"]);
                    string datetime = DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss");
                    PrintBy = userName + " " + datetime;
                }
                else
                { }



            }
            catch (Exception ex)
            {
                this.LogError(ex);
            }
            finally
            {
                con.Close();
                SqlConnection.ClearPool(con);
            }
        }
        return PrintBy;
    }


    public DataTable getAuditReportData( string FDate, string EDate)
    {
        string selectedValue = "AUDIT";
        string strcon = System.Configuration.ConfigurationManager.ConnectionStrings[selectedValue].ConnectionString;

        DataTable dt = new DataTable();
        DataTable dtDttm = new DataTable();
        DataTable dt1 = new DataTable();
        double BatchTime = 0.0;
        string PrintBy = "";

        using (con = new SqlConnection(strcon))
        {
            try
            {

                con.Open();

                //SqlCommand cmd1 = new SqlCommand("Select Distinct(BatchNo) as BatchNo from " + dt1.Rows[0]["ViewName"] + " where DateAndTime between '" + FDate + "' and '" + EDate + "' and BatchNo !='' ", con);
                SqlCommand cmd1 = new SqlCommand("select DATEADD(MINUTE, 30, DATEADD(HOUR, 5, TimeStmp)) AS TimeStmp,UserID,Severity,Location,[MessageText] from Audit " +
                                                  "where DATEADD(MINUTE, 30, DATEADD(HOUR, 5, TimeStmp)) >= '" + FDate + "' " +
                                                  " and DATEADD(MINUTE, 30, DATEADD(HOUR, 5, TimeStmp)) <= '" + EDate + "' "
                                             + "  and UserID not Like '%NT AUTHORITY\\SYSTEM%' and UserID not Like '%NT AUTHORITY\\LOCAL SERVICE%' "
                                             + " and UserID not Like '%CIPLA\\INPTG01A3W-006$%' and UserID not Like '%CIPLA\\INPTGSRSDS-01$%' "
                                             + " and UserID not Like '%NT AUTHORITY\\LOCAL SERVICE%' and UserID not Like '%LOCAL SERVICE%' and Severity!=1 and Severity!=2 "
                                             + " and UserID not Like '%CIPLA%' and MessageText not like '%Executed macro:%'  "
                                             + " and UserID not Like '%WORKGROUP%' and UserID not Like '%FactoryTalk Service%'", con);
                //SqlCommand cmd1 = new SqlCommand("WITH added_row_number AS (SELECT*,ROW_NUMBER() OVER(PARTITION BY CONVERT(Char(16), [DateAndTime] , 20) ORDER BY[DateAndTime] ASC) AS row_number FROM   " + ViewName + "   WHERE(DateAndTime >= '" + FDate + "' and DateAndTime <= '" + EDate + "')and BatchNo = '" + Batch_Number + "')SELECT* FROM added_row_number WHERE row_number = 1 and DATEPART(MINUTE, DATEADD(MINUTE, " + TimeInterval + ", [DateAndTime])) % " + TimeInterval + " = 00  order by DateAndTime asc ", con);

                cmd1.CommandTimeout = 120000;
                cmd1.ExecuteNonQuery();
                SqlDataAdapter sda1 = new SqlDataAdapter(cmd1);
                sda1.Fill(dt);
                string printby = getprintdata();
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataColumn dcFromDate = new DataColumn("FromDate", typeof(System.String));
                        DataColumn dcToDate = new DataColumn("ToDate", typeof(System.String));
                        DataColumn dcPrintBy = new DataColumn("PrintBy", typeof(System.String));
                        dt.Columns.Add(dcFromDate);
                        dt.Columns.Add(dcToDate);
                        dt.Columns.Add(dcPrintBy);
                        dt.Rows[i]["FromDate"] = FDate;
                        dt.Rows[i]["ToDate"] = EDate;
                        dt.Rows[i]["PrintBy"] = printby;
                    }
                }



            }
            catch (Exception ex)
            {
                this.LogError(ex);
            }
            finally
            {
                con.Close();
                SqlConnection.ClearPool(con);
            }
        }
        return dt;
    }

    #endregion



    #region database

    public string GetViewName(string Equipment_Name)
    {
        string ViewName = "";
        DataTable dt = new DataTable();
        DataTable dt1 = new DataTable();
        try
            {
                if(Equipment_Name== "FP-FBD-94")
                {
                    ViewName = "v_FPFBD94";
                }
                else if (Equipment_Name == "FP-FBD-120")
                {
                    ViewName = "v_FPFBD120";
                }
            else if(Equipment_Name == "FP-VD-249")
                {
                    ViewName = "v_FPVD249";
                }
                else if (Equipment_Name == "FP-VD-299")
                {
                    ViewName = "v_FPRCVD302";
                }
            else if(Equipment_Name == "FP-RCVD-300")
                {
                    ViewName = "v_FPRCVD300";
                }
                else if (Equipment_Name == "FP-RCVD-301")
                {
                    ViewName = "v_FPRCVD301";
                }
                else if (Equipment_Name == "FP-RCVD-302")
                {
                    ViewName = "v_FPRCVD302";
                }
                else if (Equipment_Name == "FP-RCVD-95")
                {
                    ViewName = "vFPRCVD95";

                }
                else if (Equipment_Name == "FP_HUMIDIFIER")
                {
                    ViewName = "v_FPRCVD302";
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


    #region EventReport
    public DataTable getRepogetEventReportDatartData_FDB(string Equipment_Name, string Batch_Number, string FDate, string EDate, string ViewName)
    {
        string selectedValue = "FTAE";
        string strcon = System.Configuration.ConfigurationManager.ConnectionStrings[selectedValue].ConnectionString;

        DataTable dt = new DataTable();
        DataTable dtDttm = new DataTable();
        DataTable dt1 = new DataTable();
        double BatchTime = 0.0;
        using (con = new SqlConnection(strcon))
        {
            try
            {

                con.Open();
                SqlCommand cmd = new SqlCommand("SELECT Min(EventTimeStamp) as EventTimeStamp,Message FROM UDV_Events WHERE BatchNo = '" + Batch_Number + "' and EventTimeStamp >= DATEADD(SECOND,-20,'" + FDate + "') and EventTimeStamp <= '" + EDate + "'  AND Message not like '%Alarm fault%' and GroupPath='" + Equipment_Name + "_E' Group by Message order by Min(EventTimeStamp) asc ", con);
                cmd.ExecuteNonQuery();
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                sda.Fill(dt);
                string printby = getprintdata();

                 selectedValue = Equipment_Name;
                 strcon = System.Configuration.ConfigurationManager.ConnectionStrings[selectedValue].ConnectionString;
                using (con = new SqlConnection(strcon))
                {
                    if (dt.Rows.Count > 0)
                    {
                        SqlCommand cmd1 = new SqlCommand("SELECT * FROM " + ViewName + " WHERE DateAndTime BETWEEN '" + FDate + "' AND '" + EDate + "' AND EquipmentNo='" + Equipment_Name + "'AND BatchNo='" + Batch_Number + "' ORDER BY DateAndTime ASC", con);
                        cmd1.ExecuteNonQuery();
                        SqlDataAdapter sda1 = new SqlDataAdapter(cmd1);
                        sda1.Fill(dt1);

                        DataColumn dcFromDate = new DataColumn("BatchStartTime", typeof(System.String));
                        DataColumn dcToDate = new DataColumn("BatchEndTime", typeof(System.String));
                        DataColumn dcreactorname = new DataColumn("reactorname", typeof(System.String));
                        DataColumn dcSection_StepNo = new DataColumn("Section_StepNo", typeof(System.String));
                        DataColumn dcBatchNo = new DataColumn("BatchNo", typeof(System.String));
                        DataColumn dcProductName = new DataColumn("ProductName", typeof(System.String));
                        DataColumn dcPDS_pageNo = new DataColumn("PDS_pageNo", typeof(System.String));
                        DataColumn dcPDS_SectionNo = new DataColumn("PDS_SectionNo", typeof(System.String));
                        DataColumn dcPrintBy = new DataColumn("PrintBy", typeof(System.String));
                        dt.Columns.Add(dcFromDate);
                        dt.Columns.Add(dcToDate);
                        dt.Columns.Add(dcreactorname);
                        dt.Columns.Add(dcSection_StepNo);
                        dt.Columns.Add(dcBatchNo);
                        dt.Columns.Add(dcProductName);
                        dt.Columns.Add(dcPDS_pageNo);
                        dt.Columns.Add(dcPDS_SectionNo);
                        dt.Columns.Add(dcPrintBy);
                        dt.Rows[0]["BatchStartTime"] = FDate;
                        dt.Rows[0]["BatchEndTime"] = EDate;
                        dt.Rows[0]["reactorname"] = dt1.Rows[0]["EquipmentNo"].ToString();
                        dt.Rows[0]["Section_StepNo"] = dt1.Rows[0]["StepNumber"].ToString();
                        dt.Rows[0]["BatchNo"] = dt1.Rows[0]["EquipmentNo"].ToString();
                        dt.Rows[0]["PDS_pageNo"] = dt1.Rows[0]["PDS_pageNo"].ToString();
                        dt.Rows[0]["PDS_SectionNo"] = dt1.Rows[0]["PDS_SectionNo"].ToString();
                        dt.Rows[0]["PrintBy"] = printby;
                        dt.Rows[0]["ProductName"] = dt1.Rows[0]["ProductName"].ToString();


                    }
                }
                 

            }
            catch (Exception ex)
            {
                this.LogError(ex);
            }
            finally
            {
                con.Close();
                SqlConnection.ClearPool(con);
            }
        }
        return dt;
    }

    public DataTable getRepogetEventReportDatartData(string Equipment_Name, string Batch_Number, string FDate, string EDate, string ViewName)
    {
        string selectedValue = Equipment_Name;
        string strcon = System.Configuration.ConfigurationManager.ConnectionStrings[selectedValue].ConnectionString;

        DataTable dt = new DataTable();
        DataTable dtDttm = new DataTable();
        DataTable dt1 = new DataTable();
        double BatchTime = 0.0;
        using (con = new SqlConnection(strcon))
        {
            try
            {

                con.Open();

                SqlCommand cmd = new SqlCommand("SELECT Min(EventTimeStamp) as EventTimeStamp,Message FROM UDV_Events WHERE BatchNo = '" + Batch_Number + "' and EventTimeStamp >= DATEADD(SECOND,-20,'" + FDate + "') and EventTimeStamp <= '" + EDate + "'  AND Message not like '%Alarm fault%' and GroupPath='" + Equipment_Name + "_E' Group by Message order by Min(EventTimeStamp) asc ", con);
                cmd.ExecuteNonQuery();
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                sda.Fill(dt);
                string printby = getprintdata();

                if (dt.Rows.Count > 0)
                {
                    SqlCommand cmd1 = new SqlCommand("SELECT * FROM " + ViewName + " WHERE DateAndTime BETWEEN '" + FDate + "' AND '" + EDate + "' AND EquipmentNo='" + Equipment_Name + "'AND BatchNo='" + Batch_Number + "' ORDER BY DateAndTime ASC", con);
                    cmd1.ExecuteNonQuery();
                    SqlDataAdapter sda1 = new SqlDataAdapter(cmd1);
                    sda1.Fill(dt1);

                    DataColumn dcFromDate = new DataColumn("BatchStartTime", typeof(System.String));
                    DataColumn dcToDate = new DataColumn("BatchEndTime", typeof(System.String));
                    DataColumn dcreactorname = new DataColumn("reactorname", typeof(System.String));
                    DataColumn dcSection_StepNo = new DataColumn("Section_StepNo", typeof(System.String));
                    DataColumn dcBatchNo = new DataColumn("BatchNo", typeof(System.String));
                    DataColumn dcProductName = new DataColumn("ProductName", typeof(System.String));
                    DataColumn dcPDS_pageNo = new DataColumn("PDS_pageNo", typeof(System.String));
                    DataColumn dcPDS_SectionNo = new DataColumn("PDS_SectionNo", typeof(System.String));
                    DataColumn dcInitial_Drying_temp_set = new DataColumn("Initial_Drying_temp_set", typeof(System.String));
                    DataColumn dcInitial_Drying_vacuum_set = new DataColumn("Initial_Drying_vacuum_set", typeof(System.String));
                    DataColumn dcDrying_Temp_set = new DataColumn("Drying_Temp_set", typeof(System.String));
                    DataColumn dcDrying_Vacuum_set = new DataColumn("Drying_Vacuum_set", typeof(System.String));
                    DataColumn dcINITIAL_DRYING_TEMP_SET_1 = new DataColumn("INITIAL_DRYING_TEMP_SET_1", typeof(System.String));
                    DataColumn dcDRYING_TEMP_SET_1 = new DataColumn("DRYING_TEMP_SET_1", typeof(System.String));
                    DataColumn dcDRYING_VACUUM_SET_1 = new DataColumn("DRYING_VACUUM_SET_1", typeof(System.String));
                    DataColumn dcINITIAL_DRYING_VACUUM_SET_1 = new DataColumn("INITIAL_DRYING_VACUUM_SET_1", typeof(System.String));
                    DataColumn dcPrintBy = new DataColumn("PrintBy", typeof(System.String));

                    dt.Columns.Add(dcFromDate);
                    dt.Columns.Add(dcToDate);
                    dt.Columns.Add(dcreactorname);
                    dt.Columns.Add(dcSection_StepNo);
                    dt.Columns.Add(dcBatchNo);
                    dt.Columns.Add(dcProductName);
                    dt.Columns.Add(dcPDS_pageNo);
                    dt.Columns.Add(dcPDS_SectionNo);
                    dt.Columns.Add(dcInitial_Drying_temp_set);
                    dt.Columns.Add(dcInitial_Drying_vacuum_set);
                    dt.Columns.Add(dcDrying_Temp_set);
                    dt.Columns.Add(dcDrying_Vacuum_set);
                    dt.Columns.Add(dcINITIAL_DRYING_TEMP_SET_1);
                    dt.Columns.Add(dcDRYING_TEMP_SET_1);
                    dt.Columns.Add(dcDRYING_VACUUM_SET_1);
                    dt.Columns.Add(dcINITIAL_DRYING_VACUUM_SET_1);
                    dt.Columns.Add(dcPrintBy);

                    dt.Rows[0]["BatchStartTime"] = FDate;
                    dt.Rows[0]["BatchEndTime"] = EDate;
                    dt.Rows[0]["reactorname"] = dt1.Rows[0]["EquipmentNo"].ToString();
                    dt.Rows[0]["Section_StepNo"] = dt1.Rows[0]["StepNumber"].ToString();
                    dt.Rows[0]["BatchNo"] = dt1.Rows[0]["EquipmentNo"].ToString();
                    dt.Rows[0]["PDS_pageNo"] = dt1.Rows[0]["PDS_pageNo"].ToString();
                    dt.Rows[0]["PDS_SectionNo"] = dt1.Rows[0]["PDS_SectionNo"].ToString();
                    dt.Rows[0]["ProductName"] = dt1.Rows[0]["ProductName"].ToString();
                    dt.Rows[0]["Initial_Drying_temp_set"] = dt1.Rows[0]["Initial_Drying_temp_set"].ToString();
                    dt.Rows[0]["Initial_Drying_vacuum_set"] = dt1.Rows[0]["Initial_Drying_vacuum_set"].ToString();
                    dt.Rows[0]["Drying_Temp_set"] = dt1.Rows[0]["Drying_Temp_set"].ToString();
                    dt.Rows[0]["Drying_Vacuum_set"] = dt1.Rows[0]["Drying_Vacuum_set"].ToString();
                    dt.Rows[0]["INITIAL_DRYING_TEMP_SET_1"] = dt1.Rows[0]["INITIAL_DRYING_TEMP_SET_1"].ToString();
                    dt.Rows[0]["DRYING_TEMP_SET_1"] = dt1.Rows[0]["DRYING_TEMP_SET_1"].ToString();
                    dt.Rows[0]["DRYING_VACUUM_SET_1"] = dt1.Rows[0]["DRYING_VACUUM_SET_1"].ToString();
                    dt.Rows[0]["INITIAL_DRYING_VACUUM_SET_1"] = dt1.Rows[0]["INITIAL_DRYING_VACUUM_SET_1"].ToString();
                    dt.Rows[0]["PrintBy"] = printby;


                }

            }
            catch (Exception ex)
            {
                this.LogError(ex);
            }
            finally
            {
                con.Close();
                SqlConnection.ClearPool(con);
            }
        }
        return dt;
    }

    #endregion

    #region solventreport
    public DataTable getSolventBatchReportData(string Equipment_Name, string Batch_Number, string FDate, string EDate, string ViewName)
    {
        string selectedValue = Equipment_Name;
        string strcon = System.Configuration.ConfigurationManager.ConnectionStrings[selectedValue].ConnectionString;

        DataTable dt = new DataTable();
        DataTable dtDttm = new DataTable();
        DataTable dt1 = new DataTable();
        double BatchTime = 0.0;
        string PrintBy = "";

        using (con = new SqlConnection(strcon))
        {
            try
            {

                con.Open();

                //SqlCommand cmd1 = new SqlCommand("Select Distinct(BatchNo) as BatchNo from " + dt1.Rows[0]["ViewName"] + " where DateAndTime between '" + FDate + "' and '" + EDate + "' and BatchNo !='' ", con);
                SqlCommand cmd1 = new SqlCommand("Select * from "+ ViewName + " where DateAndTime between '" + FDate + "' and '" + EDate + "' and BatchNo !='' ", con);
                //SqlCommand cmd1 = new SqlCommand("WITH added_row_number AS (SELECT*,ROW_NUMBER() OVER(PARTITION BY CONVERT(Char(16), [DateAndTime] , 20) ORDER BY[DateAndTime] ASC) AS row_number FROM   " + ViewName + "   WHERE(DateAndTime >= '" + FDate + "' and DateAndTime <= '" + EDate + "')and BatchNo = '" + Batch_Number + "')SELECT* FROM added_row_number WHERE row_number = 1 and DATEPART(MINUTE, DATEADD(MINUTE, " + TimeInterval + ", [DateAndTime])) % " + TimeInterval + " = 00  order by DateAndTime asc ", con);

                cmd1.CommandTimeout = 120000;
                cmd1.ExecuteNonQuery();
                SqlDataAdapter sda1 = new SqlDataAdapter(cmd1);
                sda1.Fill(dt);
                string printby = getprintdata();

                {
                    if (dt.Rows.Count > 0)
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            DataColumn dcFromDate = new DataColumn("FromDate", typeof(System.String));
                            DataColumn dcToDate = new DataColumn("ToDate", typeof(System.String));
                            DataColumn dcPrintBy = new DataColumn("PrintBy", typeof(System.String));
                            dt.Columns.Add(dcFromDate);
                            dt.Columns.Add(dcToDate);
                            dt.Columns.Add(dcPrintBy);
                            dt.Rows[i]["FromDate"] = FDate;
                            dt.Rows[i]["ToDate"] = EDate;
                            dt.Rows[i]["PrintBy"] = printby;
                        }
                }


            }
            catch (Exception ex)
            {
                this.LogError(ex);
            }
            finally
            {
                con.Close();
                SqlConnection.ClearPool(con);
            }
        }
        return dt;
    }

    #endregion
}