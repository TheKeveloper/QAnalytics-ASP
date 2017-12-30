<%@ Page Language="C#" Inherits="QAnalytics.Default" %>
<!DOCTYPE html>
<html>
<head runat="server">
    <title>Default</title>
    <script>
        function listPages_SelectionChanged(){
            var listPages = document.getElementById("listPages");
            //alert("Default.aspx?page=" + listPages.selectedIndex);
            window.location = "Default.aspx?page=" + listPages.selectedIndex;
        }
    </script>
</head>
<body>
    <h1 align="center">Q-Analytics</h1>
	<form id="form1" runat="server">
            <asp:DropDownList id = "listPages" onchange = "listPages_SelectionChanged();" runat="server"></asp:DropDownList>

            <asp:Table id = "tblCourses" runat="server"></asp:Table>

    </form>
</body>
</html>
