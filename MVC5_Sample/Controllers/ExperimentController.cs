using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC5_Sample.Controllers
{
    public class ExperimentController : Controller
    {
        // GET: Experiment
        public ActionResult Index()
        {
            this.AuditTrailLogsData("2014-10-15", "2015-10-16", 2/*20*/);
            return View();
        }

        // Helper Method -> Fetch Audit Trail Logs from DB using Stored Procedure | Return Type - DataTable
        private DataTable AuditTrailLogsData(string startDate, string endDate, int auditTrailTypeId, bool isExport = false)
        {
            DateTime parsedStartDate, parsedEndDate;

            if (!(DateTime.TryParse(startDate, CultureInfo.InvariantCulture, DateTimeStyles.None, out parsedStartDate) && DateTime.TryParse(endDate, CultureInfo.InvariantCulture, DateTimeStyles.None, out parsedEndDate) && (auditTrailTypeId > 0)))
            //if (!(DateTime.TryParse(startDate, out parsedStartDate) && DateTime.TryParse(endDate, out parsedEndDate) && (auditTrailTypeId > 0)))
            {
                return null;
            }

            DataTable tableAuditLogs = new DataTable("AuditTable");
            using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["EY-PF-MasterDB"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("[usp_GetAuditTrailLog]", sqlConnection))
                {
                    cmd.Parameters.Add("@StartDate", SqlDbType.DateTime, 50).Value = parsedStartDate.ToString("yyyy-MM-dd HH:mm:ss");
                    cmd.Parameters.Add("@EndDate", SqlDbType.DateTime, 50).Value = parsedEndDate.ToString("yyyy-MM-dd HH:mm:ss");
                    cmd.Parameters.Add("@AuditTrailTypeId", SqlDbType.Int, 50).Value = auditTrailTypeId;

                    try
                    {
                        if (cmd.Connection.State == ConnectionState.Closed) { cmd.Connection.Open(); }
                        using (var da = new SqlDataAdapter(cmd))
                        {
                            cmd.CommandTimeout = 200;
                            cmd.CommandType = CommandType.StoredProcedure;
                            //da.Fill(0, 10000, tableAuditLogs);
                            da.Fill(0, 1, tableAuditLogs);

                            
                            //tableAuditLogs.Load(cmd.ExecuteReader(CommandBehavior.CloseConnection));
                            //tableAuditLogs.Load(cmd.ExecuteReader(CommandBehavior.SingleRow));

                            SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                            List<Object> listErrors = new List<Object>();
                            while (dr.Read())
                            {
                                object[] dRow = new object[8];
                                dr.GetValues(dRow);

                                DataRow dtRow = null;
                                dtRow.ItemArray = dRow;
                                //dtRow[0] = dr.GetValue(0);

                                listErrors.Add(new { DateTime = dr.GetValue(2) } );
                                //OutputFile of = new OutputFile();
                                //of.FileId = (Guid)dr[0];
                                //of.ProjectID = Convert.ToInt32(dr[1].ToString());
                                //of.JobId = Convert.ToInt32(dr[2].ToString());
                                //of.FileName = dr[3].ToString();
                                //listOutFileDetails.Add(of);
                            }

                            if (!isExport)
                            {
                                for (int i = 0; i < tableAuditLogs.Columns.Count; i++)
                                {
                                    tableAuditLogs.Columns[i].ColumnName = tableAuditLogs.Columns[i].ColumnName.Replace(" ", "").Replace("/", "");
                                }

                                DataTable dtCloned = tableAuditLogs.Clone();
                                dtCloned.Columns["DateTime"].DataType = typeof(string);
                                foreach (DataRow row in tableAuditLogs.Rows)
                                {
                                    dtCloned.ImportRow(row);
                                }

                                for (int j = 0; j < tableAuditLogs.Rows.Count; j++)
                                {
                                    //dtCloned.Rows[j]["DateTime"] = Convert.ToDateTime(tableAuditLogs.Rows[j]["DateTime"]).ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture);
                                    dtCloned.Rows[j]["DateTime"] = Convert.ToDateTime(tableAuditLogs.Rows[j]["DateTime"]).ToString("s", CultureInfo.InvariantCulture);
                                }
                                tableAuditLogs = dtCloned.Copy();
                            }
                            else
                            {
                                DataTable dtCloned = tableAuditLogs.Clone();
                                dtCloned.Columns["Date/Time"].DataType = typeof(string);
                                foreach (DataRow row in tableAuditLogs.Rows)
                                {
                                    dtCloned.ImportRow(row);
                                }

                                for (int j = 0; j < tableAuditLogs.Rows.Count; j++)
                                {
                                    dtCloned.Rows[j]["Date/Time"] = Convert.ToDateTime(tableAuditLogs.Rows[j]["Date/Time"]).ToString("dd-MM-yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                                }
                                tableAuditLogs = dtCloned.Copy();
                            }
                        }
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                    finally
                    {
                        if (sqlConnection.State == ConnectionState.Open) { cmd.Connection.Close(); }
                    }
                }
            }
            return (tableAuditLogs.Rows.Count > 0) ? tableAuditLogs : null;
        }
    }
}