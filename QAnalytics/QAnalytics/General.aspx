<%@ Page Language="C#" Inherits="QAnalytics.General" %>
<!DOCTYPE html>
<html>
<head runat="server">
    <title>General</title>
    <link rel = "stylesheet" type = "text/css" href = "/styles/all.css"/>
    <script type = "text/javascript" src = "/scripts/libraries/Chart.js"></script>
    <script type = "text/javascript" src = "/scripts/general.js"></script>
</head>
<body onload = "createChart();">
    <a href = "Default.aspx" id = "pageTitle">Q-Analytics</a>
    <div id = "header" align = "center">
            <a href = "Default.aspx">Courses</a>
            <a href = "General.aspx">General</a>
            <a href = "Departments.aspx">Department</a>
    </div>
	<form id="mainForm" runat="server" align = "center">
           <asp:DropDownList id = "listSemesters" align = "center" runat="server" AutoPostBack = "true" /> 

            <asp:HiddenField id = "valEnrollments" runat="server"/>
            <asp:HiddenField id = "valRecommends" runat="server"/>
            <asp:HiddenField id = "valWorkloads" runat="server"/>
            <asp:HiddenField id = "valEnrollRec" runat="server"/>
            <asp:HiddenField id = "valWorkRec" runat="server"/>
    </form>
    
    <div id = "charts" align = "center">
        <canvas id = "chartEnroll" width = "800" height = "400"></canvas> <br/>
        <h6 id = "lblEnrollReg"></h6> <br/>
        <canvas id = "chartWork" width = "800" height = "400"></canvas> <br/>
        <h6 id = "lblWorkReg"></h6>
    </div>
</body>
</html>
