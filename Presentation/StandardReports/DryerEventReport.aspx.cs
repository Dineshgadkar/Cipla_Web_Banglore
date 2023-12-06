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
            FDate = FDate + " " + "00:00:00";
            string EDate = txtEDate.Text;
            EDate = EDate + " " + "23:59:59";
            Batch_Number = ddl_BatchNo.SelectedValue;

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
        
        string FDate = BatchMinTime.Text;
        string EDate = BatchMaxTime.Text;
        Batch_Number = ddl_BatchNo.SelectedValue;
        string rdlcReportname = "";
        try
        {

            rdlcReportname = BAL.getRDLCname(Equipment_Name);
            ReportViewer1.ProcessingMode = ProcessingMode.Local;
            //ReportViewer1.LocalReport.ReportPath = "Presentation/Reports/batchreport.rdlc";
            if (Equipment_Name == "FP-FBD-94" || Equipment_Name == "FP-FBD-120")
            {
                ReportViewer1.LocalReport.ReportPath = "Presentation/Reports/DryerReport/Event-FBD.rdlc";
            }
            else
            {
                ReportViewer1.LocalReport.ReportPath = "Presentation/Reports/DryerReport/EventReport.rdlc";
            }   

            dataset = new DataTable();

            DataTable dt = new DataTable();
            string ViewName = BAL.GetViewName(Equipment_Name);
            dataset = BAL.getRepogetEventReportDatartData(Equipment_Name, Batch_Number, FDate, EDate, ViewName);

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


    #region Print Button Code

    protected void btnPrint_Click(object sender, EventArgs e)
    {
        try
        {
            LocalReport report = new LocalReport();
            report.ReportPath = "Presentation/Reports/BatchReports/BatchReport.rdlc";
            // report.ReportPath = "Presentation/Reports/BatchReports/123.rdlc";
            btn_Submit_Click(new object(), new EventArgs());
            report.DataSources.Add(new ReportDataSource("V101", dataset));
            PrintReport.Export(report);
        }
        catch (Exception ex)
        {
            this.LogError(ex);
        }
    }

    public static class PrintReport
    {

        private static int m_currentPageIndex;
        private static IList<Stream> m_streams;

        public static Stream CreateStream(string name, string fileNameExtension, Encoding encoding, string mimeType, bool willSeek)
        {
            Stream stream = new MemoryStream();
            m_streams.Add(stream);
            return stream;
        }

        public static void Export(LocalReport report, bool print = true)
        {
            try
            {
                string deviceInfo =
                  @"<DeviceInfo>
                <OutputFormat>EMF</OutputFormat>
                <PageWidth>20in</PageWidth>
                <PageHeight>16in</PageHeight>
                <MarginTop>0.25in</MarginTop>
                <MarginLeft>0.5in</MarginLeft>
                <MarginRight>0.5in</MarginRight>
                <MarginBottom>0.25in</MarginBottom>
                </DeviceInfo>";
                Warning[] warnings;
                m_streams = new List<Stream>();
                report.Render("Image", deviceInfo, CreateStream,
                   out warnings);
                foreach (Stream stream in m_streams)
                    stream.Position = 0;

                if (print)
                {
                    Print();
                }
            }
            catch (Exception ex)
            {

            }
        }

        // Handler for PrintPageEvents
        public static void PrintPage(object sender, PrintPageEventArgs ev)
        {
            try
            {
                Metafile pageImage = new
                   Metafile(m_streams[m_currentPageIndex]);

                // Adjust rectangular area with printer margins.
                Rectangle adjustedRect = new Rectangle(
                    ev.PageBounds.Left - (int)ev.PageSettings.HardMarginX,
                    ev.PageBounds.Top - (int)ev.PageSettings.HardMarginY,
                    ev.PageBounds.Width,
                    ev.PageBounds.Height);

                // Draw a white background for the report
                ev.Graphics.FillRectangle(Brushes.White, adjustedRect);

                // Draw the report content
                ev.Graphics.DrawImage(pageImage, adjustedRect);

                // Prepare for the next page. Make sure we haven't hit the end.
                m_currentPageIndex++;
                ev.HasMorePages = (m_currentPageIndex < m_streams.Count);

            }
            catch (Exception ex)
            {

            }
        }

        public static void Print()
        {
            if (m_streams == null || m_streams.Count == 0)
                throw new Exception("Error: no stream to print.");
            PrintDocument printDoc = new PrintDocument();

            //string PrinterName = System.Configuration.ConfigurationManager.AppSettings["PrinterName"];
            //printDoc.PrinterSettings.PrinterName = PrinterName;
            //printDoc.PrinterSettings.PrinterName = "HP LaserJet M203-M206 PCL 6"; //change it after printer name change

            if (!printDoc.PrinterSettings.IsValid)
            {
                throw new Exception("Error: cannot find the default printer.");
            }
            else
            {
                printDoc.PrintPage += new PrintPageEventHandler(PrintPage);
                m_currentPageIndex = 0;
                printDoc.DefaultPageSettings.Landscape = true;
                printDoc.Print();
            }
        }

        //public static void PrintToPrinter(this LocalReport report)
        //{
        //    Export(report);
        //}

        public static void DisposePrint()
        {
            if (m_streams != null)
            {
                foreach (Stream stream in m_streams)
                    stream.Close();
                m_streams = null;
            }
        }
    }

    #endregion      
}