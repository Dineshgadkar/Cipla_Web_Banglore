using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Configuration;
using System.IO;
using System.Diagnostics;

/// <summary>
/// Summary description for Global
/// </summary>
/// 


public class Global:System.Web.HttpApplication
{
	public Global()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    void Application_Error(object sender, EventArgs e)
    {

        Exception objError = Server.GetLastError().GetBaseException();
        string strError = "Error Has Been Caught in Application_Error event\n" +
        "Error in: " + Request.Url.ToString() + "\nError Message:" +
        objError.Message.ToString() + "\nStack Trace:" +
        objError.StackTrace.ToString();
        EventLog.WriteEntry("CustomError", strError, EventLogEntryType.Error);


        Server.Transfer("~/Close.aspx");

    }



}