<%@ Page Language="C#" Inherits="QAnalytics.Departments" %>
<!DOCTYPE html>
<html>
<head runat="server">
	<title>Departments</title>
	<link rel = "stylesheet" type = "text/css" href = "/styles/general.css"/>
</head>
<body>
	<a href = "Default.aspx" id = "pageTitle">Q-Analytics</a>
	<div id = "header" align = "center">
			<a href = "Default.aspx">Courses</a>
			<a href = "General.aspx">General</a>
			<a href = "Departments.aspx">Department</a>
	</div>
	<form id="mainForm" runat="server">
            <div id = "nav">
                <asp:DropDownList id = "listDepts" runat="server"/>
            </div>
	</form>
</body>
</html>
