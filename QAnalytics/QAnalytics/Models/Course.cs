using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace QAnalytics.Models
{
    public enum Semester{
        Fall = 0, Spring = 1
    }
    public class Course
    {
        public struct Info{
            public Semester Semester;
            public int Year;
            public int Enrollment;
            public float Recommend;
            public float Workload;
            public Info(Semester semester, int year, int enrollment, float recommend, float workload){
                this.Semester = semester;
                this.Year = year;
                this.Enrollment = enrollment;
                this.Recommend = recommend;
                this.Workload = workload;
            }

            public Info(MySqlDataReader reader)
            {
                this.Semester = (Semester)reader.GetInt32("semester");
                this.Year = reader.GetInt32("year");
                this.Enrollment = reader.GetInt32("enrollment");
                this.Recommend = reader.GetFloat("recommend");
                this.Workload = reader.GetFloat("workload");
            }
        }

        public string Code;
        public string Name;
        public List<Info> Infos;

        public Course(string code, string name, List<Info> infos){
            this.Code = code;
            this.Name = name;
            this.Infos = infos;
        }
        public Course(string code, string name) : this(code, name, new List<Info>()){}
        public Course() : this(string.Empty, string.Empty){}

        public void Load(DBManager manager, string code){
            MySqlCommand cmd = manager.CreateCommand();

            this.Code = code;
            cmd.CommandText = "SELECT * FROM courses WHERE code = @code;";
            cmd.Parameters.AddWithValue("@code", code);

            MySqlDataReader reader = cmd.ExecuteReader();
            int n = 0; 
            while(reader.Read()){
                if(n == 0){
                    this.Name = reader.GetString("name");
                    n++;
                }
                this.Infos.Add(AdjustWorkload(new Info(reader)));
            }

            Infos.Sort((Info a, Info b) => {
                if(a.Year != b.Year){
                    return a.Year - b.Year;
                }
                return b.Semester - a.Semester;
            });
        }

        public static Info AdjustWorkload(Info info){ 
            if (info.Year > 2014 || (info.Year == 2014 && info.Semester == Semester.Fall))
            {
                info.Workload = info.Workload * 3.0f / 10.0f;
                if (info.Workload < 1) info.Workload = 1;
                else if (info.Workload > 5) info.Workload = 5;
                info.Workload = (float)Math.Round((double)info.Workload, 2);
            }
            return info;
        }

        public static List<Course> GetCoursesSimple(DBManager manager, int minSems = 2){
            List<Course> courses = new List<Course>();
            MySqlCommand cmd = manager.CreateCommand();

            cmd.CommandText = "SELECT code, name, count(code) as c FROM courses GROUP BY code, name HAVING c >= @minsems ORDER BY code ASC;";
            cmd.Parameters.AddWithValue("@minsems", minSems);


            var reader = cmd.ExecuteReader();

            while(reader.Read()){
                courses.Add(new Course(reader.GetString("code"), reader.GetString("name")));
            }
            return courses;
        }

        public static List<Course> GetCoursesFull(DBManager manager, int minSems = 2){
            List<Course> courses = new List<Course>();
            MySqlCommand cmd = manager.CreateCommand();

            cmd.CommandText = "SELECT * FROM courses ORDER BY code ASC;";
            var reader = cmd.ExecuteReader();

            while(reader.Read()){
                string code = reader.GetString("code");
                if (courses.Count == 0 || !courses[courses.Count - 1].Code.Equals(code))
                {
                    courses.Add(new Course(code, reader.GetString("name"))); 
                }
                courses[courses.Count - 1].Infos.Add(AdjustWorkload(new Info(reader)));
            }

            if(minSems > 1){
                //Double check that this is how predicate works
                courses.RemoveAll((c) => {
                    return c.Infos.Count < minSems;
                });
            }
            return courses;  
        }

        public static List<Course> SearchCourses(DBManager manager, string search, int minSems = 2)
        {
            List<Course> courses = new List<Course>();

            var cmd = manager.CreateCommand();

            search = "%" + search.Replace("_", "%").ToUpper() + "%";
            search = parseCommon(search);
            cmd.CommandText = "SELECT code, name, count(code) as c FROM courses WHERE code LIKE @code OR name LIKE @name GROUP BY code, name HAVING c >= @minsems";
            cmd.Parameters.AddWithValue("@code", search);
            cmd.Parameters.AddWithValue("@name", search);
            cmd.Parameters.AddWithValue("@minsems", minSems);

            var reader = cmd.ExecuteReader();

            while(reader.Read()){
                courses.Add(new Course(reader.GetString("code"), reader.GetString("name")));
            }

            return courses;
        }

        private static string parseCommon(string search){
            return search.Replace("%CS%", "%COMPSCI%").Replace("%EC%", "%ECON%");
        }
    }
}
