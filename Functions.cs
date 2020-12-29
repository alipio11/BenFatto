//using Devart.Data.PostgreSql;
using BenFatto.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;

namespace BenFatto {

    public class DataLayer {

        //private readonly string ConnectionString = "User Id=postgres;Host=localhost;Database=BancoDeDados;Password=a;Initial Schema=public";
        private readonly string ConnectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\--User\Desktop\BenFatto\Database1\BancoDeDados.mdf;Integrated Security=True;Connect Timeout=30";

        public List<Table> SelectLista(string FilterIp = "", string FilterUser = "", string FilterHora = "") {
            List<Table> Lista = new List<Table>();

            string Filter = "";
            if (FilterIp != "")
                Filter += "AND[Ip]LIKE'%" + FilterIp + "%'";
            if (FilterUser != "")
                Filter += "AND[User]LIKE'%" + FilterUser + "%'";
            if (FilterHora != "")
                Filter += "AND cast (Datetime as time) >= '" + FilterHora + ":00:00' AND cast (Datetime as time) < '" + FilterHora + ":59:59'";
            if (Filter.StartsWith("AND"))
                Filter = "WHERE " + Filter.Substring(3);

            string Select = "SELECT Id, [Ip], [User], [Datetime], [Detail] FROM DATA " + Filter;
            SelectSQL(Select, out DataTable dt);
            foreach (DataRow R in dt.Rows) {
                Lista.Add(new Table { ID = (int)R[0], Ip = R[1].ToString(), User = R[2].ToString(), Datetime = R[3].ToString(), Detail = R[4].ToString() });
            }

            return Lista;
        }

        public string InsertLista(string[] FileData) {
            string Insert = "";
            foreach (string L in FileData) {
                try {
                    string Ip = L.Split(" ")[0].Replace("'", "\"");
                    string User = L.Split(" [")[0].Split(" ")[1].Replace("'", "\"");
                    string Datetime = L.Split(" [")[1].Split("] ")[0];
                    string Detail = L.Split("] ")[1].Replace("'", "\"");

                    DateTime D = DateTime.ParseExact(Datetime.Split(" ")[0], "dd/MMM/yyyy:HH:mm:ss", CultureInfo.InvariantCulture);

                    string TimeZone = Datetime.Split(" ")[1];
                    TimeZone = TimeZone.Remove(TimeZone.Length - 2, 2) + ":00";
                    Datetime = D.ToString("yyyy-MM-dd HH:mm:ss") + " " + TimeZone;

                    Insert += "INSERT INTO DATA ([Ip],[User],[Datetime],[Detail]) VALUES ('" + Ip + "','" + User + "','" + Datetime + "','" + Detail + "')";
                } catch {
                }
            }
            string msg = InsertSQL(Insert);
            if (msg.Contains("Error"))
                return "false";

            return "true";
        }

        public List<Table> SelectValue(string FilterId) {
            List<Table> Lista = new List<Table>();

            if (FilterId == "")
                FilterId = "-1";

            string Select = "SELECT Id, [Ip], [User], [Datetime], [Detail] FROM DATA WHERE [Id]='" + FilterId + "'";
            SelectSQL(Select, out DataTable dt);
            try {
                DataRow R = dt.Rows[0];
                Lista.Add(new Table { ID = (int)R[0], Ip = R[1].ToString(), User = R[2].ToString(), Datetime = R[3].ToString(), Detail = R[4].ToString() });
            } catch {
                Lista.Add(new Table { ID = -1, Ip = "", User = "", Datetime = "", Detail = "" });
            }
            return Lista;
        }

        public bool Salvar(Table d) {
            string Insert;

            if (d.ID == -1)
                Insert = "INSERT INTO DATA ([Ip],[User],[Datetime],[Detail]) VALUES ('" + d.Ip.Replace("'", "\"") + "','" + d.User.Replace("'", "\"") + "','" + d.Datetime + "','" + d.Detail.Replace("'", "\"") + "')";
            else
                Insert = "UPDATE DATA SET [Ip]='" + d.Ip.Replace("'", "\"") + "',[User]='" + d.User.Replace("'", "\"") + "',[Datetime]='" + d.Datetime + "',[Detail]='" + d.Detail.Replace("'", "\"") + "'WHERE[Id]='" + d.ID + "'";

            string msg = InsertSQL(Insert);
            if (msg.Contains("Error"))
                return false;

            return true;
        }

        public bool Delete(Table d) {
            string Insert = "DELETE FROM DATA WHERE[Id]='" + d.ID + "'";

            string msg = InsertSQL(Insert);
            if (msg.Contains("Error"))
                return false;

            return true;
        }

        private string InsertSQL(string Input) {
            try {
                string Result = "";

                using (SqlConnection mConnection = new SqlConnection(ConnectionString)) {
                    mConnection.Open();
                    try {
                        mConnection.InfoMessage += (s, e) => {
                            Result = e.Message;
                        };
                        using (SqlCommand myCmd = new SqlCommand(Input, mConnection))
                            myCmd.ExecuteNonQuery();
                    } catch (Exception ex) {
                        Result = "Error Not Correct Format\r\n" + ex.Message.ToString();
                    }
                }
                return Result;
            } catch {
                return "Error Connecting to Database";
            }
        }

        private string SelectSQL(string sCommand, out DataTable dt) {
            dt = new DataTable();

            string Result = "Successful";
            try {
                using (SqlConnection mConnection = new SqlConnection(ConnectionString)) {
                    mConnection.Open();
                    try {
                        mConnection.InfoMessage += (s, e) => {
                            Result = e.Message;
                        };
                        using (SqlCommand myCmd = new SqlCommand(sCommand, mConnection))
                        using (SqlDataAdapter rdr = new SqlDataAdapter(myCmd))
                            rdr.Fill(dt);
                    } catch (Exception ex) {
                        Result = "Error Not Correct Format\r\n" + ex.ToString();
                    }
                }
                return Result;
            } catch {
                return "Error Connecting to Database";
            }
        }

        //private string InsertSQL(string Input) {
        //    try {
        //        string Result = "";

        //        using (PgSqlConnection mConnection = new PgSqlConnection(ConnectionString)) {
        //            mConnection.Open();
        //            try {
        //                mConnection.InfoMessage += (s, e) => {
        //                    Result = e.Message;
        //                };
        //                using (PgSqlCommand myCmd = new PgSqlCommand(Input, mConnection))
        //                    myCmd.ExecuteNonQuery();
        //            } catch (Exception ex) {
        //                Result = "Error Not Correct Format\r\n" + ex.Message.ToString();
        //            }
        //        }
        //        return Result;
        //    } catch {
        //        return "Error Connecting to Database";
        //    }
        //}

        //private string SelectSQL(string sCommand, out DataTable dt) {
        //    dt = new DataTable();

        //    string Result = "Successful";
        //    try {
        //        using (PgSqlConnection mConnection = new PgSqlConnection(ConnectionString)) {
        //            mConnection.Open();
        //            try {
        //                mConnection.InfoMessage += (s, e) => {
        //                    Result = e.Message;
        //                };
        //                using (PgSqlCommand myCmd = new PgSqlCommand(sCommand, mConnection))
        //                using (PgSqlDataAdapter rdr = new PgSqlDataAdapter(myCmd))
        //                    rdr.Fill(dt);
        //            } catch (Exception ex) {
        //                Result = "Error Not Correct Format\r\n" + ex.ToString();
        //            }
        //        }
        //        return Result;
        //    } catch {
        //        return "Error Connecting to Database";
        //    }
        //}
    }
}