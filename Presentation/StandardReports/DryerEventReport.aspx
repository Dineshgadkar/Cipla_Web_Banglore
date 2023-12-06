<%@ Page Title="" Language="C#" MasterPageFile="~/Presentation/MasterPages/MasterPage.master" AutoEventWireup="true" CodeFile="DryerEventReport.aspx.cs" Inherits="Presentation_StandardReports_BCTReports" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=15.0.0.0, Culture=neutral, PublicKeyToken=89845DCD8080CC91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

     <div class="content-wrapper" id="bodyDiv"  style="height: 1170px;">
         <section class="content-header">
            <h1>Event Report
    
            </h1>

            <ol class="breadcrumb">

            </ol>

        </section>
     
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">

            <ContentTemplate>


                <div class="content-header box-header with-border">

                     <div class="col-md-3">
                        <b id="B2" runat="server">From Date : </b>
                        <asp:TextBox ID="txtDate" runat="server" CssClass="form-control"></asp:TextBox>
                        <asp:CalendarExtender ID="Cal1_Extender" TargetControlID="txtDate" Format="yyyy-MM-dd" runat="server"></asp:CalendarExtender>
                        <span>
                            <asp:RequiredFieldValidator Id="RequiredFromDate" ErrorMessage="Please Select From Date" ForeColor="Red" ControlToValidate="txtDate" runat="server"></asp:RequiredFieldValidator>
                        </span>
                    </div>
                    <div class="col-md-3">
                        <b id="B1" runat="server">To Date : </b>
                        <asp:TextBox ID="txtEDate" runat="server" CssClass="form-control"></asp:TextBox>
                        <asp:CalendarExtender ID="CalendarExtender1" TargetControlID="txtEDate" Format="yyyy-MM-dd" runat="server"></asp:CalendarExtender>
                        <span>
                            <asp:RequiredFieldValidator Id="RequiredFieldValidator1" ErrorMessage="Please Select To Date" ForeColor="Red" ControlToValidate="txtEDate" runat="server"></asp:RequiredFieldValidator>
                        </span>
                    </div>
                    <div class="col-md-2">
                        <b id="B3" runat="server">Equipment Name : </b>
                        <asp:DropDownList ID="ddl_Equipment" AutoPostBack="true" runat="server"  CssClass="form-control"  OnSelectedIndexChanged="ddl_EquipmentName_SelectedIndexChanged">
                               <asp:ListItem>Select Reactor</asp:ListItem>
                                <asp:ListItem>FP-FBD-94</asp:ListItem>
                                 <asp:ListItem>FP-FBD-120</asp:ListItem>
                                 <asp:ListItem>FP-VD-249</asp:ListItem>
                                 <asp:ListItem>FP-VD-299</asp:ListItem>
                                 <asp:ListItem>FP-RCVD-300</asp:ListItem>
                                 <asp:ListItem>FP-RCVD-301</asp:ListItem>
                                 <asp:ListItem>FP-RCVD-302</asp:ListItem>
                                 <asp:ListItem>FP-RCVD-95</asp:ListItem>
                                 <asp:ListItem>FP_HUMIDIFIER</asp:ListItem>
                        </asp:DropDownList>
                    </div>

                    

                     <div class="col-md-2">
                        <b id="B4" runat="server">Batch Number : </b>
                        <asp:DropDownList ID="ddl_BatchNo" AutoPostBack="true" runat="server"  CssClass="form-control" OnSelectedIndexChanged="ddl_BatchNo_SelectedIndexChanged" >
                        </asp:DropDownList>
                    </div>

                    <div class="col-md-2">
                     
                        <asp:Button ID="btn_Submit" runat="server" Text="Submit"  CssClass="btn btn-primary" style="margin-top:15%;" OnClick="btn_Submit_Click" />
                         
                    </div>
                    

                </div>   
                

                <div class="content-header box-header with-border">

                     <div class="col-md-3">
                        <b id="B5" runat="server">Batch Min DateAndTime : </b>
                        <asp:TextBox ID="BatchMinTime" runat="server" CssClass="form-control"></asp:TextBox>
                        <asp:CalendarExtender ID="CalendarExtender2" TargetControlID="txtDate" Format="yyyy-MM-dd" runat="server"></asp:CalendarExtender>
                        <span>
                            <asp:RequiredFieldValidator Id="RequiredFieldValidator2" ErrorMessage="Please Select From Date" ForeColor="Red" ControlToValidate="txtDate" runat="server"></asp:RequiredFieldValidator>
                        </span>
                    </div>
                    <div class="col-md-3">
                        <b id="B6" runat="server">Batch Max DateAndTime : </b>
                        <asp:TextBox ID="BatchMaxTime" runat="server" CssClass="form-control"></asp:TextBox>
                        <asp:CalendarExtender ID="CalendarExtender3" TargetControlID="txtEDate" Format="yyyy-MM-dd" runat="server"></asp:CalendarExtender>
                        <span>
                            <asp:RequiredFieldValidator Id="RequiredFieldValidator3" ErrorMessage="Please Select To Date" ForeColor="Red" ControlToValidate="txtEDate" runat="server"></asp:RequiredFieldValidator>
                        </span>
                    </div>
                    

                </div>

                
         </ContentTemplate>                

        </asp:UpdatePanel>
         
                <!-- Main content -->
                <section class="content" style="width: 100%;">

                <!-- col -->
                <section class="col-md-12">

                    <div class="box">
                        <!-- Custom tabs (Charts with tabs)-->

                        <!-- Tabs within a box -->


                        <!-- /.row -->
                       <div id="d1" style="height: auto;">
                                <rsweb:ReportViewer ID="ReportViewer1" Style="width: auto; height: auto;" runat="server" Font-Names="Verdana" Font-Size="8pt" WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt" ShowPrintButton="true" ShowToolBar="true" Height="900px">

                                </rsweb:ReportViewer>

                            </div>


                    </div>

                    <div>
                        <div class="col-sm-5">
                    </div>
                    <div class="col-sm-6">
                        <asp:LinkButton ID="btnPrint" runat="server" CssClass="btn btn-primary form-control" Height="60px" Width="70px" OnClick="btnPrint_Click"><i class="glyphicon glyphicon-print fa-3x"></i></asp:LinkButton>
                    </div>
                    </div>
                </section>


        </section>
                <!-- /.content -->
    </div>

</asp:Content>


