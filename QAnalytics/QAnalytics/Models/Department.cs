using System;
using System.Collections.Generic;
using System.Linq;

namespace QAnalytics.Models
{
    public class Department
    {
        public class Info{
            public Semester Semester;
            public int AggregateEnrollment;
            public double AggregateRecommend;
            public double AggregateWorkload;

            public float AvgRecommend{ 
                get{
                    return (float)Math.Round(AggregateRecommend / (double) AggregateEnrollment, 2);
                }
            }

            public float AvgWorkload{
                get{
                    return (float) Math.Round(AggregateWorkload / (double) AggregateEnrollment, 2);
                }
            }

            public Info(Semester semester, int enroll, double recommend, double workload)
            {
                this.Semester = semester;
                this.AggregateEnrollment = enroll;
                this.AggregateRecommend = recommend;
                this.AggregateWorkload = workload;
            }

            public Info(Semester semester) : this(semester, 0, 0, 0) {}
        }
        public string Code;
        public List<Info> Infos;

        public Department(string code, List<Info> infos){
            this.Code = code;
            this.Infos = infos;
        }

        public Department(string code) : this(code, new List<Info>()){}

        public void Load(DBManager manager){
            var cmd = manager.CreateCommand();
            cmd.CommandText = "SELECT * FROM courses WHERE code LIKE @code ORDER BY year ASC, semester DESC";
            cmd.Parameters.AddWithValue("@code", this.Code + " %");

            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                Semester sem = new Semester((Season)reader.GetInt32("semester"), reader.GetInt32("year"));
                if (Infos.Count == 0 || !Infos[Infos.Count - 1].Semester.Equals(sem))
                {
                    Infos.Add(new Info(sem));
                }

                Infos[Infos.Count - 1].AggregateEnrollment += reader.GetInt32("enrollment");
                Infos[Infos.Count - 1].AggregateRecommend += reader.GetFloat("recommend");
                Infos[Infos.Count - 1].AggregateWorkload += reader.GetFloat("workload");   
            }
        }

        public static List<string> GetDepartments(DBManager manager){
            List<string> depts = new List<string>();
            List<string> codes = new List<string>();
            var cmd = manager.CreateCommand();
            cmd.CommandText = "SELECT code FROM courses GROUP BY code";
            var reader = cmd.ExecuteReader();
            while (reader.Read()) {
                codes.Add(reader.GetString("code"));
            }

            var queryCodes = from code in codes 
                                  group code by code.Split(' ')[0] into department 
                                  orderby department.Key 
                                  select department;
            foreach (var department in queryCodes){
                depts.Add(department.Key);
            }
            return depts;
        }
    }
}
