using System;
using System.Web;
using System.Web.UI;
using System.Collections.Generic;


using QAnalytics.Models;


namespace QAnalytics
{

    public partial class General : System.Web.UI.Page
    {
        protected override void OnLoad(EventArgs e){
            base.OnLoad(e);

            int sem;

            if(int.TryParse(Request.Params["sem"], out sem)){
                if (!IsPostBack) listSemesters.SelectedIndex = sem;
            }
            else{
                sem = 0; 
            }

            DBManager manager = new DBManager();
            manager.Open();
            List<Semester> semesters = Course.GetSemesters(manager);
            semesters.Reverse();
            foreach(Semester s in semesters){
                listSemesters.Items.Add(s.Year.ToString() + " " + s.Season.ToString());
            }
            manager.Close();
        }
    }
}
