﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS_TAsks
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("HAndling Mutiple Tasks");
            // The StartNew() will start the TAsk so no seperate Start() call is needed
            Task task = Task.Factory.StartNew(() => { RunTasks(); });
            Console.WriteLine("Back to Main");
            Console.ReadLine(); 
        }

        static void RunTasks()
        {
            var timeSpan = Stopwatch.StartNew();
            Task t1 = new Task(() => { Console.WriteLine("Task 1"); });
            //t1.Start();
            
            Task t2 = new Task(() => { Console.WriteLine("Task 2"); });
           // t2.Start();
            Task t3 = new Task(() => { Console.WriteLine("Task 3"); });
            // t3.Start();

            t3.Start();
            t1.Start();
            t2.Start();


            // Wait for All Tasks to Complete
            //Task.WaitAll(t1, t2, t3);
            Task t4 = new Task(() => {
                SqlConnection Conn = new SqlConnection("Data Source=.; Initial Catalog=Enterprise;Integrated Security=SSPI");
                Conn.Open();
                SqlCommand Cmd = new SqlCommand();
                Cmd.Connection = Conn;
                Cmd.CommandText = "Select * from Department";
                SqlDataReader Reader = Cmd.ExecuteReader();
                while (Reader.Read())
                {
                    Console.WriteLine($"DeptNo {Reader["DeptNo"]}, DeptName: {Reader["DeptName"]}");
                }
                Reader.Close();
                Conn.Close();
                Conn.Dispose();
            });
            t4.Start();
            Console.WriteLine($"Total Time for all TAsks {timeSpan.Elapsed.TotalMilliseconds}");
        }
    }


}
