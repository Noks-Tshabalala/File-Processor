using System;
using System.Web;
using System.IO;
using FileProcessor.Properties;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Configuration;

namespace FileProcessor.Models.Results
{
    public class ResultsProcessor
    {
        string cs = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        /// <summary>
        ///Reads data from flat file and stores it in the database.
        /// </summary>
        public string GetFiles(List<string> fileList, string uId, string uName)
        {
            int a = 0;
            int b = 0;
            int c = 0;
            int formula = 0;

            string feedback = "";

            string path1 = HttpContext.Current.Server.MapPath(Settings.Default.UnprocessedFiles);
            string path2 = HttpContext.Current.Server.MapPath(Settings.Default.ProcessedFiles);

            if (uId.Length > 0 && fileList != null)
            {
                int counter = 0;
                string line;
                try
                {
                    foreach (var item in fileList)
                    {
                        string filePath = path1 + uName + "_" + item;
                        string destPath = path2 + uName + "_" + item;
                        StreamReader file = new StreamReader(filePath);

                        while ((line = file.ReadLine()) != null)
                        {

                            string[] arr = line.Split(";".ToCharArray());
                            //Skip first record if it's a header row.
                            if (arr[0].ToLower() != "Formula".ToLower())
                            {
                                if (arr.Length == 4)
                                {
                                    //Check if all variables are integers then call the stored procedure to insert the record into the database
                                    if (Regex.IsMatch(arr[0], @"\d") && Regex.IsMatch(arr[1], @"\d") && Regex.IsMatch(arr[2], @"\d") && Regex.IsMatch(arr[3], @"\d"))
                                    {
                                        formula = int.Parse(arr[0]);
                                        a = int.Parse(arr[1]);
                                        b = int.Parse(arr[2]);
                                        c = int.Parse(arr[3]);

                                        using (SqlConnection con = new SqlConnection(cs))
                                        {
                                            SqlCommand cmd = con.CreateCommand();
                                            cmd.CommandText = "dbo.CalculateResult";
                                            cmd.CommandType = System.Data.CommandType.StoredProcedure;

                                            cmd.Parameters.AddWithValue("@UserId", uId);
                                            cmd.Parameters.AddWithValue("@FileName", item);
                                            cmd.Parameters.AddWithValue("@Formula", formula);
                                            cmd.Parameters.AddWithValue("@A", a);
                                            cmd.Parameters.AddWithValue("@B", b);
                                            cmd.Parameters.AddWithValue("@C", c);

                                            try
                                            {
                                                con.Open();
                                                cmd.ExecuteNonQuery();
                                                con.Close();
                                            }
                                            catch 
                                            {
                                                feedback = "Database Error";
                                            }
                                        }
                                    }
                                    else
                                    {
                                        feedback = "Data Type Error.";
                                    }
                                }
                                else
                                {
                                    feedback = "Array Length Error";
                                }
                            }

                            counter++;
                        }
                        file.Close();
                        File.Move(filePath, destPath);
                    }

                }
                catch
                {
                    feedback = "Data Processing Error.";
                }
            }

            if (feedback != "")
            {
                return feedback;
            }
            else
            {
                feedback = "Success";
                return feedback;
            }
        }
    }
}