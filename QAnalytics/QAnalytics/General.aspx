<%@ Page Language="C#" Inherits="QAnalytics.General" %>
<!DOCTYPE html>
<html>
<head runat="server">
    <title>General</title>
    <link rel = "stylesheet" type = "text/css" href = "/styles/all.css"/>
</head>
<body>
    <a href = "Default.aspx" id = "pageTitle">Q-Analytics</a>
    <div id = "header" align = "center">
            <a href = "Default.aspx">Courses</a>
            <a href = "General.aspx">General</a>
            <a href = "Departments.aspx">Department</a>
    </div>
	<form id="mainForm" runat="server" align = "center">
           <asp:DropDownList id = "listSemesters" align = "center" runat="server" AutoPostBack = "true" /> 
	</form>
</body>
</html>
