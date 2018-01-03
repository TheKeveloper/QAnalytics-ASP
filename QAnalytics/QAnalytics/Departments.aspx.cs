using System;
using System.Web;
using System.Web.UI;
using System.Collections.Generic;
using QAnalytics.Models;

namespace QAnalytics
{

    public partial class Departments : System.Web.UI.Page
    {
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            DBManager manager = new DBManager();
            manager.Open();
            var depts = Department.GetDepartments(manager);
            manager.Close();
            foreach(var dept in depts){
                listDepts.Items.Add(dept);
            }
        }
    }
}
