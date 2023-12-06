<%@ Application Inherits="Global" Language="C#" %>

<script runat="server">

    void Application_Start(object sender, EventArgs e) 
    {
        // Code that runs on application startup

    }
    
    void Application_End(object sender, EventArgs e) 
    {
        //  Code that runs on application shutdown

    }
        
    void Application_Error(object sender, EventArgs e) 
    { 
        // Code that runs when an unhandled error occurs

        //Exception ex = Server.GetLastError();
        //var serverError = Server.GetLastError() as HttpException;
        //if (ex is HttpUnhandledException)
        //{
        //    if (ex.InnerException != null)
        //    {
        //        ex = new Exception(ex.InnerException.Message);
        //        Server.Transfer("~/Presentation/ServerError.aspx?handler=Application_Error%20-%20Global.asax", true);
        //    }

        //}
        //if (null != serverError)
        //{
        //    Server.ClearError();
        //    Server.Transfer("~/Presentation/Error404.aspx?handler=Application_Error%20-%20Global.asax", true);
        //}
     
 
    }

    void Session_Start(object sender, EventArgs e) 
    {
        // Code that runs when a new session is started

    }

    void Session_End(object sender, EventArgs e) 
    {
        // Code that runs when a session ends. 
        // Note: The Session_End event is raised only when the sessionstate mode
        // is set to InProc in the Web.config file. If session mode is set to StateServer 
        // or SQLServer, the event is not raised.

    }
       
</script>
