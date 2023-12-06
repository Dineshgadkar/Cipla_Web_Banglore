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

  

    
       

    protected void btn_Submit_Click(object sender, EventArgs e)
    {
        //Equipment_Name = ddl_Equipment.SelectedValue;
     
        string FDate = txtDate.Text + " " + "00:00:00";
        string EDate = txtEDate.Text + " " + "23:59:59";
    
        string rdlcReportname = "";
        try
        {

            //rdlcReportname = BAL.getAuditRDLCname(Equipment_Name);
            ReportViewer1.ProcessingMode = ProcessingMode.Local;
            ReportViewer1.LocalReport.ReportPath = "Presentation/Reports/DryerReport/AuditReport.rdlc";
           // ReportViewer1.LocalReport.ReportPath = "Presentation/Reports/BatchReport/" + rdlcReportname + ".rdlc";

            dataset = new DataTable();

            DataTable dt = new DataTable();
            //string TableName = BAL.GetAuditTableName(Equipment_Name);
            dataset = BAL.getAuditReportData( FDate, EDate);


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