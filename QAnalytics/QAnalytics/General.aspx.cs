using System;
using System.Web;
using System.Web.UI;
using System.Collections.Generic;


using QAnalytics.Models;
using Newtonsoft.Json;

namespace QAnalytics
{

    public partial class General : System.Web.UI.Page
    {
        protected override void OnLoad(EventArgs e){
            base.OnLoad(e);

            DBManager manager = new DBManager();
            manager.Open();
            List<Semester> semesters = Course.GetSemesters(manager);
            semesters.Reverse();
            foreach(Semester s in semesters){
                listSemesters.Items.Add(s.Year.ToString() + " " + s.Season.ToString());
            }

            int sem = -1;
            if (int.TryParse(Request.Params["sems"], out sem))
            {
                if (!IsPostBack)
                {
                    listSemesters.SelectedIndex = sem;
                }
            }
            else
            {
                sem = 0;
            }

            List<Course> courses = Course.LoadSemester(manager, semesters[sem]);
            manager.Close();

            List<double> enrollments = new List<double>();
            List<double> recommends = new List<double>();
            List<double> workloads = new List<double>();

            foreach(var c in courses){
                enrollments.Add(c.Infos[0].Enrollment);
                recommends.Add(c.Infos[0].Recommend);
                workloads.Add(c.Infos[0].Workload);
            }

            LinReg enrollRec = new LinReg(enrollments.ToArray(), recommends.ToArray()).Calc();
            LinReg workRec = new LinReg(workloads.ToArray(), recommends.ToArray()).Calc();

            enrollRec.R = Math.Round(enrollRec.R, 3);
            enrollRec.M = Math.Round(enrollRec.M, 4);
            enrollRec.B = Math.Round(enrollRec.B, 4);

            workRec.R = Math.Round(workRec.R, 3);
            workRec.M = Math.Round(workRec.M, 4);
            workRec.B = Math.Round(workRec.B, 4);

            valEnrollments.Value = JsonConvert.SerializeObject(enrollments);
            valRecommends.Value = JsonConvert.SerializeObject(recommends);
            valWorkloads.Value = JsonConvert.SerializeObject(workloads);
            valEnrollRec.Value = JsonConvert.SerializeObject(enrollRec);
            valWorkRec.Value = JsonConvert.SerializeObject(workRec);

            listSemesters.SelectedIndexChanged += (sender, args) => {
                Response.Redirect("/General.aspx?sems=" + listSemesters.SelectedIndex);
            };
        }
    }
}
