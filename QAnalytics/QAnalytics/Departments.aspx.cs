using System;
using System.Web;
using System.Web.UI;
using System.Collections.Generic;
using QAnalytics.Models;
using Newtonsoft.Json;

namespace QAnalytics
{

    public partial class Departments : System.Web.UI.Page
    {
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            DBManager manager = new DBManager();
            manager.Open();
            var deptNames = Department.GetDepartments(manager);

            foreach(var name in deptNames){
                listDepts.Items.Add(name);
            }
            int dept = -1;
            if (int.TryParse(Request.Params["dept"], out dept))
            {
                if(!IsPostBack) listDepts.SelectedIndex = dept;
            }
            else
            {
                dept = 0;
            }

            Department department = new Department(deptNames[dept]);

            department.Load(manager);
            manager.Close();

            valDept.Value = JsonConvert.SerializeObject(department);

            listDepts.SelectedIndexChanged += (sender, args) => {
                Response.Redirect("/Departments.aspx?dept=" + listDepts.SelectedIndex);
            };
        }
    }
}
