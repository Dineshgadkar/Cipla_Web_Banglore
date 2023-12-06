using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Reporting.WebForms;
using System.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Reporting.WebForms;
using System.Data;
using System.IO;
using System.Text;
using System.Drawing.Imaging;
using System.Reflection;
using System.Drawing.Printing;
using System.Drawing;

public partial class Presentation_StandardReports_BCTReports : System.Web.UI.Page
{
    public string Equipment_Name = "";
    public string Batch_Number = "";
    BusinessAccessLayer BAL = new BusinessAccessLayer();

    DataTable dataset;
    private int m_currentPageIndex;
    private IList<Stream> m_streams;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            // GetData();
        }
    }

    public void GetData()
    {
        try
        {
            DataTable dt = new DataTable();
            //dt = BAL.getBlenderData();
            dt = BAL.getEquipmentData();

            ddl_Equipment.DataSource = dt;

            ddl_Equipment.DataTextField = "BlenderName";
            ddl_Equipment.DataValueField = "BlenderName";

            ddl_Equipment.DataBind();

            ddl_Equipment.Items.Insert(0, "Select Reactor");
        }
        catch (Exception ex)
        {
            this.LogError(ex);
        }
    }

    protected void ddl_EquipmentName_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            Equipment_Name = ddl_Equipment.SelectedValue;
            string FDate = txtDate.Text;
            string EDate = txtEDate.Text;

            DataTable dt = new DataTable();
                string ViewName = BAL.GetViewName(Equipment_Name);
                dt = BAL.getBatchNo(Equipment_Name, FDate, EDate, ViewName);  //Get Batch No as per the BatchNo and Product Name selection

                ddl_BatchNo.DataSource = dt;
           
            ddl_BatchNo.DataTextField = "BatchNo";
            ddl_BatchNo.DataValueField = "BatchNo";
            ddl_BatchNo.DataBind();

            ddl_BatchNo.Items.Insert(0, "Select Batch");

            string selectedValue = ddl_Equipment.SelectedValue;
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings[selectedValue].ConnectionString;


        }
        catch (Exception ex)
        {
            this.LogError(ex);
        }
    }

    protected void ddl_BatchNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        Equipment_Name = ddl_Equipment.SelectedValue;
        string FDate = txtDate.Text;
        FDate = FDate + " " + "00:00:00";
        string EDate = txtEDate.Text;
        EDate = EDate + " " + "23:59:59";
        Batch_Number = ddl_BatchNo.SelectedValue;
        try
        {

            dataset = new DataTable();

            DataTable dt = new DataTable();
            string ViewName = BAL.GetViewName(Equipment_Name);
            dt = BAL.getMinMax(Equipment_Name, Batch_Number, FDate, EDate, ViewName);  //Get MIN and MAX of batch no
            if (dt.Rows.Count > 0)
            {
                FDate = dt.Rows[0]["min_time"].ToString();
                EDate = dt.Rows[0]["max_time"].ToString();
            }

            BatchMinTime.Text = dt.Rows[0]["min_time"].ToString();
            BatchMaxTime.Text = dt.Rows[0]["max_time"].ToString();



        }
        catch (Exception ex)
        {
            this.LogError(ex);
        }
    }

    protected void btn_Submit_Click(object sender, EventArgs e)
    {
        Equipment_Name = ddl_Equipment.SelectedValue;
        string TimeInterval = ddl_TimeInterval.SelectedValue;
        string FDate = BatchMinTime.Text;
        string EDate = BatchMaxTime.Text;
        Batch_Number = ddl_BatchNo.SelectedValue;
        string rdlcReportname = "";
        try
        {

            rdlcReportname = BAL.getRDLCname(Equipment_Name);
            ReportViewer1.ProcessingMode = ProcessingMode.Local;
            //ReportViewer1.LocalReport.ReportPath = "Presentation/Reports/batchreport.rdlc";
            ReportViewer1.LocalReport.ReportPath = "Presentation/Reports/DryerReport/" + rdlcReportname + ".rdlc";

            dataset = new DataTable();

            DataTable dt = new DataTable();
            string ViewName = BAL.GetViewName(Equipment_Name);
            dataset = BAL.getReportData(Equipment_Name, Batch_Number, FDate, EDate, ViewName, TimeInterval);

            ReportDataSource datasource = new ReportDataSource("DataSet1", dataset);
            ReportViewer1.LocalReport.EnableHyperlinks = true;
            ReportViewer1.LocalReport.DataSources.Clear();
            ReportViewer1.LocalReport.DataSources.Add(datasource);
            ReportViewer1.LocalReport.Refresh();


        }
        catch (Exception ex)
        {
            this.LogError(ex);
        }
    }

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
}