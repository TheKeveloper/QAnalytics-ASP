using System;
using System.Web;
using System.Web.UI;
using QAnalytics.Models;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace QAnalytics
{

    public partial class Default : System.Web.UI.Page
    {
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            string strPage = Request.Params["page"];
            int page = strPage == null ? 0 : int.Parse(strPage);
            DBManager manager = new DBManager();
            manager.Open();
            List<Course> courses = Course.GetCoursesSimple(manager);

            int startIndex = page * 25;
            int endIndex = startIndex + 25;
            if (endIndex > courses.Count) endIndex = courses.Count;

            for (int i = startIndex; i < endIndex; i++){
                var row = new TableRow();
                var codeCell = new TableCell();
                var codeLink = new HyperLink();
                codeCell.Width = new Unit(150);
                codeLink.Text = courses[i].Code;
                codeLink.NavigateUrl = "Courses.aspx?code=" + courses[i].Code.Replace(' ', '_');
                codeCell.Controls.Add(codeLink);

                var nameCell = new TableCell();
                var nameLink = new HyperLink();
                nameLink.Text = courses[i].Name;
                nameLink.NavigateUrl = codeLink.NavigateUrl;
                nameCell.Controls.Add(nameLink);

                row.Cells.Add(codeCell);
                row.Cells.Add(nameCell);

                tblCourses.Rows.Add(row);
            }

            for (int i = 0; i < Math.Ceiling((double) courses.Count / 25); i++){
                listPages.Items.Add(i.ToString());
            }
            listPages.SelectedIndex = page;
        }

    }
}
