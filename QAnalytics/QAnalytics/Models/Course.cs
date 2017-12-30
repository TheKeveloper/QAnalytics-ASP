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
                float workload = reader.GetFloat("workload");
                Semester semester = (Semester)reader.GetInt32("semester");
                int year = reader.GetInt32("year");

                //Adjust the workload 
                if(year > 2014 || (year == 2014 && semester == Semester.Fall)){
                    workload = workload * 3.0f / 14.0f;
                    if (workload < 1) workload = 1;
                    else if (workload > 5) workload = 5;
                    workload = (float)Math.Round((double)workload, 2);
                }
                this.Infos.Add(new Info(semester,
                                        year,
                                        reader.GetInt32("enrollment"),
                                        reader.GetFloat("recommend"),
                                        workload));
            }

            Infos.Sort((Info a, Info b) => {
                if(a.Year != b.Year){
                    return a.Year - b.Year;
                }
                return b.Semester - a.Semester;
            });
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
    }
}
