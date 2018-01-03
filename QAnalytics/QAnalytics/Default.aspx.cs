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
            string search = Request.Params["search"];
            if (search == null) search = String.Empty;
            valSearch.Value = search;
            if(!IsPostBack){
                txtSearch.Text = search.Replace("_", " ");
            }

            int page = strPage == null ? 0 : int.Parse(strPage);
            DBManager manager = new DBManager();
            manager.Open();
            List<Course> courses = search.Equals(String.Empty) ? Course.GetCoursesSimple(manager) :
                                         Course.SearchCourses(manager, search);
            manager.Close();

            loadCourses(courses, page);

            int pageMax = (int) Math.Ceiling((double)courses.Count / 25) - 1;

            for (int i = 0; i <= pageMax; i++){
                listPages.Items.Add(i.ToString());
            }
            listPages.SelectedIndex = page;

            int prevPage = page - 1 < 0 ? 0 : page - 1;
            int nextPage = page + 1 > pageMax ? pageMax : page + 1;

            linkPrev.NavigateUrl = "/Default.aspx?page=" + (prevPage) + "&search=" + search;
            linkNext.NavigateUrl = "/Default.aspx?page=" + (nextPage) + "&search=" + search;

            btnSearch.Click += (sender, args) =>
            {
                string s = txtSearch.Text.Replace(" ", "_");

                Response.Redirect("/Default.aspx?page=0&search=" + s);
            };
        }

        private void loadCourses(List<Course> courses, int page){
            int startIndex = page * 25;
            int endIndex = startIndex + 25;
            if (endIndex > courses.Count) endIndex = courses.Count;

            for (int i = startIndex; i < endIndex; i++)
            {
                var row = new TableRow();
                var codeCell = new TableCell();
                var codeLink = new HyperLink();

                codeCell.Width = new Unit(250);
                codeLink.Text = courses[i].Code;
                codeLink.NavigateUrl = "Courses.aspx?code=" + courses[i].Code.Replace(' ', '_');
                codeLink.CssClass = "courseLink";

                codeCell.Controls.Add(codeLink);
                codeCell.CssClass = "cellCode";

                var nameCell = new TableCell();
                var nameLink = new HyperLink();

                nameLink.Text = courses[i].Name;
                nameLink.NavigateUrl = codeLink.NavigateUrl;
                nameLink.CssClass = "courseLink";

                nameCell.Controls.Add(nameLink);
                nameCell.CssClass = "cellName";

                row.Cells.Add(codeCell);
                row.Cells.Add(nameCell);

                tblCourses.Rows.Add(row);
            }
        }

    }
}
