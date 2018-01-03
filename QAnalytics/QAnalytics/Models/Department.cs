using System;
using System.Collections.Generic;

namespace QAnalytics.Models
{
    public class Department
    {
        public string Code;
        public Semester Semester;
        public int TotalEnrollment;
        public double AggregateRecommend;
        public double AggregateWorkload;
        public List<Course> Courses;

        public float AverageRecommend {
            get{
                return (float)Math.Round(AggregateRecommend / (double)TotalEnrollment, 2);
            }
        }

        public float AverageWorkload { 
            get{
                return (float)Math.Round(AggregateWorkload / (double)TotalEnrollment, 2);
            }
        }

        public Department(string code, Season season, int year)
        {
            this.Code = code;
            this.Semester = new Semester(season, year);
            TotalEnrollment = 0;
            AggregateRecommend = 0;
            AggregateWorkload = 0;
            Courses = new List<Course>();
        }

        public Department(string code) : this(code, Season.None, 0){}
        public Department() : this(null) { }

        public void LoadCourses(DBManager manager){
            var cmd = manager.CreateCommand();

            cmd.CommandText = "SELECT * FROM courses WHERE code LIKE @code AND semester = @sem AND year = @year;";
            cmd.Parameters.AddWithValue("@code", this.Code + "%");
            cmd.Parameters.AddWithValue("@sem", (int) this.Semester.Season);
            cmd.Parameters.AddWithValue("@year", this.Semester.Year);

            var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                Course c = new Course(reader.GetString("code"), reader.GetString("name"));
                c.Infos.Add(new Course.Info(reader));
                Courses.Add(c);

                TotalEnrollment += c.Infos[0].Enrollment;
                if (c.Infos[0].Recommend > 0) AggregateRecommend += c.Infos[0].Recommend;
                if (c.Infos[0].Workload > 0) AggregateWorkload += c.Infos[0].Workload;
            }
        }

        public List<Course> SortEnrollment(){
            Courses.Sort((x, y) =>
            {
                return x.Infos[0].Enrollment - y.Infos[0].Enrollment;
            });

            return Courses;
        }
    }
}
