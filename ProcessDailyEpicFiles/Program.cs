using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Net.Mail;

namespace ProcessDailyEpicFiles
{
    class Program
    {
        static string connectionString = "";
        static SqlConnection conn;
        static void Main(string[] args)
        {
            ProcessFiles();
        }

        private static void ProcessFiles()
        {
            string procName = "";
            string currentFile = "";
            try
            {
                connectionString = ConfigurationManager.ConnectionStrings["TransafeRx"].ConnectionString;
                string[] files = Directory.GetFiles(filePath);
                

                conn = new SqlConnection(connectionString);
                conn.Open();

                foreach (string file in files)
                {
                    currentFile = file;
                    if (file.ToLower().Contains("tacrolimus"))
                    {
                        procName = "BulkInsertTacrolimus";
                        string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Path.GetFileName(file));
                        File.Copy(file, path, true);
                        ExecuteProc(procName, path);
                    }
                    else if (file.ToLower().Contains("medsdispensed"))
                    {
                        procName = "BulkInsertMedsDispensed";
                        string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Path.GetFileName(file));
                        File.Copy(file, path, true);
                        ExecuteProc(procName, path);
                    }
                    else if (file.ToLower().Contains("currentmeds"))
                    {
                        procName = "BulkInsertCurrentMeds";
                        string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Path.GetFileName(file));
                        File.Copy(file, path, true);
                        ExecuteProc(procName, path);

                        var command = new SqlCommand("AddUpdateAllUserMedicationFromEPIC", conn);
                        command.CommandType = CommandType.StoredProcedure;
                        command.ExecuteNonQuery();
                    }
                    else if (file.ToLower().Contains("appointments"))
                    {
                        procName = "BulkInsertAppointments";
                        string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Path.GetFileName(file));
                        File.Copy(file, path, true);
                        ExecuteProc(procName, path);
                    }
                    else if (file.ToLower().Contains("rosterclarity"))
                    {
                        procName = "BulkInsertRosterClarity";
                        string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Path.GetFileName(file));
                        File.Copy(file, path, true);
                        ExecuteProc(procName, path);
                    }
                    File.Move(file, Path.Combine(archiveFilePath, Path.GetFileNameWithoutExtension(file) + "_" + DateTime.Now.ToString("MMddyyyyHHmmss") + Path.GetExtension(file)));
                }
            }
            catch(Exception ex)
            {
                MailMessage message = new MailMessage(from, to);
                message.IsBodyHtml = false;
                message.Body = "ProcName = " + procName + Environment.NewLine + " File = " + currentFile + Environment.NewLine + " Error = " + ex.Message;
                message.Subject = "TransafeRx Import Error";
                client.Send(message);
                message.Dispose();
            }

        }

        private static void ExecuteProc(string procName, object file)
        {
            var command = new SqlCommand(procName, conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add(new SqlParameter("@fileName", file));
            command.ExecuteNonQuery();
        }
    }
}
