using FileProcessor.ViewModels;
using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.Script.Serialization;
using System.Web.Services;

namespace FileProcessor.WebServices
{
    /// <summary>
    /// Summary description for StatsService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class StatsService : System.Web.Services.WebService
    {

        string cs = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        [WebMethod]
        public dynamic GetTotals(string uId)
        {
            string result = "";

            VMSummary s = new VMSummary();
            try
            {
                using (SqlConnection conn = new SqlConnection(cs))
                {
                    SqlCommand com = conn.CreateCommand();
                    com.CommandText = "Select Count(Distinct F.Id) As TotalFiles, Count(C.Id) As TotalCalculations, Count(D.Id) As TotalDuplicates, Count(E.Id) As TotalErred "
                                      + "From FileDetails F Join CalculationDetails C ON C.FileId=F.Id "
                                      + "Left Outer Join (Select * From CalculationDetails Where ErrorMessage='Duplicate Record') D ON D.Id=C.Id "
                                      + "Left Outer Join(Select* From CalculationDetails Where ErrorMessage= 'Calculation Error') E ON E.Id = C.Id "
                                     + " Where F.UserId='" + uId + "'";
                    conn.Open();
                    using (SqlDataReader rd = com.ExecuteReader())
                    {

                        while (rd.Read())
                        {

                            s.TotalCalculations = int.Parse(rd["TotalCalculations"].ToString());
                            s.TotalDuplicates = int.Parse(rd["TotalDuplicates"].ToString());
                            s.TotalFiles = int.Parse(rd["TotalFiles"].ToString());
                            s.TotalErred = int.Parse(rd["TotalErred"].ToString());
                        }

                    }
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }

            JavaScriptSerializer js = new JavaScriptSerializer();
            dynamic json = js.Serialize(s);
            return json;
        }
    }
}
