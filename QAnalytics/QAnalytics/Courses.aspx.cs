using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.DataVisualization.Charting;
using QAnalytics.Models;
using Newtonsoft.Json;

namespace QAnalytics
{

    public partial class Courses : System.Web.UI.Page
    {
        protected override void OnLoad(EventArgs e){
            base.OnLoad(e);
            string code = Request.Params["code"].Replace("_", " ");
            Page.Title = code;
            DBManager manager = new DBManager();
            Course c = new Course();
            manager.Open();
            c.Load(manager, code);
            manager.Close();

            lblTitle.Text = c.Name + " (" + c.Code + ")" + "</br>";
            valCourse.Value = JsonConvert.SerializeObject(c);
            //Chart chart = new Chart();

            /*string[] semesters = new string[c.Infos.Count];
            int[] enrollments = new int[c.Infos.Count];
            float[] recommends = new float[c.Infos.Count];
            float[] workloads = new float[c.Infos.Count];

            for (int i = 0; i < c.Infos.Count; i++)
            {
                semesters[i] = c.Infos[i].Semester.ToString() + " " + c.Infos[i].Year.ToString();
                enrollments[i] = c.Infos[i].Enrollment;
                recommends[i] = c.Infos[i].Recommend;
                workloads[i] = c.Infos[i].Workload;
            }*/


        }
    }
}
