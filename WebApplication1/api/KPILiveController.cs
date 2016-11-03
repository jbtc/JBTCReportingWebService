using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;

namespace WebApplication1.api
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class KPILiveController : ApiController
    {
        public int phase = 0;

        /// <summary>
        /// get data from report server based on query params
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/DBKPILiveDateBased")]
        public IEnumerable<liveLineDataWDate> DBKPILiveDateBased(string parameters = null)
        {
            List<liveLineDataWDate> set = new List<liveLineDataWDate>();

            /*
             * parameter structure
             * airport|terminal|gate|device|value
             */
            if (!string.IsNullOrEmpty(parameters))
            {
                //"DAL|Term1_Zone1|Gate12|PCA|PCAOnTime"
                string[] pars = parameters.Split('|');
                if (pars.Length == 5)
                {
                    //
                    string airport = pars[0];
                    string terminal = pars[1];
                    string gate = pars[2];
                    string device = pars[3];
                    string value = pars[4];

                    // allowed airport codes
                    //string[] permittedAirportCodesForUser = new string[] { "JFK", "CID", "DAL", "EWR", "" }; // Empty string for generic reports

                    switch (airport)
                    {
                        case "DAL":
                            runSWAQueriesWithDate(set, terminal, gate,device,value,"DAL");
                            break;
                        default:
                            break;

                    }
                }
            }return set;
        }

        [HttpGet]
        [Route("api/DBKPILive")]
        public IEnumerable<liveLineData> GetDbKPI(string parameters = null)
        {
            List<liveLineData> set = new List<liveLineData>();

            /*
             * parameter structure
             * airport|terminal|gate|device|value
             */
            if (!string.IsNullOrEmpty(parameters))
            {
                //"DAL|Term1_Zone1|Gate12|PCA|PCAOnTime"
                string[] pars = parameters.Split('|');
                if (pars.Length == 5)
                {
                    //
                    string airport = pars[0];
                    string terminal = pars[1];
                    string gate = pars[2];
                    string device = pars[3];
                    string value = pars[4];

                    // allowed airport codes
                    //string[] permittedAirportCodesForUser = new string[] { "JFK", "CID", "DAL", "EWR", "" }; // Empty string for generic reports

                    switch (airport)
                    {
                        case "DAL":
                            runSWAQueries(set, terminal, gate, device, value, "DAL");
                            break;
                        default:
                            break;

                    }
                }
            }
            else
            {
                for (int iphase = 0; iphase < (2 * 314); iphase++)
                {
                    set.Add(new liveLineData() { Index = iphase, Value = System.Math.Sin((iphase) / 100) });
                }
            }


            return set;
        }

        /// <summary>
        /// run queries for SWA
        /// </summary>
        /// <param name="set"></param>
        /// <param name="terminal"></param>
        /// <param name="gate"></param>
        /// <param name="device"></param>
        /// <param name="value"></param>
        private static void runSWAQueries(List<liveLineData> set, string terminal, string gate, string device, string value, string airport = "DAL")
        {
            string sql = @"SELECT [Airport_DAL_Term1_Zone1_Gate12_PCA_PCAOnTime_Value] FROM[SWA_IOPS_Reporting].[dbo].[Term_1_PBB_12_PCAOnTime_Data_Logging] where [DateAndTime] >= DATEADD(day,-7, GETDATE())";

            // do not change order
            string selector = airport + "_" + terminal + "_" + gate + "_" + device + "_" + value;
            switch (selector)
            {
                case "DAL_Term1_Zone1_Gate12_PCA_PCAOnTime":
                    sql = @"SELECT [Airport_DAL_Term1_Zone1_Gate12_PCA_PCAOnTime_Value] FROM[SWA_IOPS_Reporting].[dbo].[Term_1_PBB_12_PCAOnTime_Data_Logging] where [DateAndTime] >= DATEADD(day,-7, GETDATE())";
                    break;
                default:
                    break;

            }

            string connectionString = ConfigurationManager.ConnectionStrings["SWAIOPSReporting"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {                    
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        connection.Open();
                        using (IDataReader myReader = command.ExecuteReader())
                        {
                            int index = 0;
                            while (myReader.Read())
                            {
                                set.Add(new liveLineData() { Index = index, Value = myReader.GetDouble(0) });
                            }
                        }
                    }
                    connection.Close();
                }
                catch (Exception esd)
                {
                    /*Handle error*/
                    int g = 0;
                }
            }
        }

        /// <summary>
        /// queries with date based ticks
        /// </summary>
        /// <param name="set"></param>
        /// <param name="terminal"></param>
        /// <param name="gate"></param>
        /// <param name="device"></param>
        /// <param name="value"></param>
        /// <param name="airport"></param>
        private static void runSWAQueriesWithDate(List<liveLineDataWDate> set, string terminal, string gate, string device, string value, string airport = "DAL")
        {
            string sql = @"SELECT [DateAndTime],[Airport_DAL_Term1_Zone1_Gate12_PCA_PCAOnTime_Value] FROM[SWA_IOPS_Reporting].[dbo].[Term_1_PBB_12_PCAOnTime_Data_Logging] where [DateAndTime] >= DATEADD(day,-7, GETDATE())";

            // do not change order
            string selector = airport + "_" + terminal + "_" + gate + "_" + device + "_" + value;
            switch (selector)
            {
                case "DAL_Term1_Zone1_Gate12_PCA_PCAOnTime":
                    sql = @"SELECT [DateAndTime],[Airport_DAL_Term1_Zone1_Gate12_PCA_PCAOnTime_Value] FROM[SWA_IOPS_Reporting].[dbo].[Term_1_PBB_12_PCAOnTime_Data_Logging] where [DateAndTime] >= DATEADD(day,-7, GETDATE())";
                    break;
                default:
                    break;

            }

            string connectionString = ConfigurationManager.ConnectionStrings["SWAIOPSReporting"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        connection.Open();
                        using (IDataReader myReader = command.ExecuteReader())
                        {
                            int index = 0;
                            while (myReader.Read())
                            {
                                set.Add(new liveLineDataWDate() { Index = myReader.GetDateTime(0), Value = myReader.GetDouble(1) });
                            }
                        }
                    }
                    connection.Close();
                }
                catch (Exception esd)
                {
                    /*Handle error*/
                    int g = 0;
                }
            }
        }

        [HttpGet]
        [Route("api/KPILive")]
        public IEnumerable<liveLineData> GetKPI(string airport = null)
        {
            // If applicable to your application: Somehow get a list of airport codes that the user has permission 
            // for so you can do security trimming on the discovered reports
            string[] permittedAirportCodesForUser = new string[] { "JFK", "CID", "DAL", "EWR", "" }; // Empty string for generic reports
            if (airport != null)
            {
                if (!int.TryParse(airport, out phase))
                {
                    phase = 0;
                }
            }
            List<liveLineData> set = new List<liveLineData>();
            for (int iphase = 0; iphase < (2 * 314); iphase++)
            {
                set.Add(new liveLineData() { Index = iphase, Value = System.Math.Sin((iphase + phase) / 100) });
            }

            return set.ToArray();
        }
    }

    public class liveLineData
    {
        public int Index { get; set; }
        public double Value { get; set; }
    }

    public class liveLineDataWDate
    {
        public DateTime Index { get; set; }
        public double Value { get; set; }
    }
}
