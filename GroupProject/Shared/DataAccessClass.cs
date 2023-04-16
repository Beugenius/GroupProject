using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GroupProject.Shared
{
    /// <summary>
    /// Class used for accessing the data in the database 
    /// </summary>
    public class DataAccessClass
    {
        /// <summary>
        /// The connection string for connecting to the database 
        /// </summary>
        private string ConnectionString;
        public DataAccessClass()
        {
            try
            {
                ConnectionString = @"Provider=Microsoft.Jet.OLEDB.4.0;Data source= " + Directory.GetCurrentDirectory() + "\\Invoice.mdb";
            }
            catch (Exception ex)
            {
                // Throw the exception with a message 
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }
        /// <summary>
        /// Method that executes a SQL statement in the data base
        /// </summary>
        /// <param name="SQLString">SQL statement to be executed</param>
        /// <param name="RetValInt">Number of rows affected</param>
        /// <returns>Dataset of tables from SQL statement</returns>
        /// <exception cref="Exception"></exception>
        public DataSet ExecuteSQLStatement(string SQLString, ref int RetValInt)
        {
            try
            {
                DataSet ds = new DataSet();
                using (OleDbConnection connection = new OleDbConnection(ConnectionString))
                {
                    using (OleDbDataAdapter adapter = new OleDbDataAdapter())
                    {
                        // Open the connection 
                        connection.Open();

                        // add the information for the selectcomand using the sql statement and the connection object 
                        adapter.SelectCommand = new OleDbCommand(SQLString, connection);
                        adapter.SelectCommand.CommandTimeout = 0;

                        // fill up the dataset with data
                        adapter.Fill(ds);
                    }
                }

                // number of values returned
                RetValInt = ds.Tables[0].Rows.Count;
                // return the data set 
                return ds;
            }
            catch (Exception ex)
            {
                // Throw the exception with a message 
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }
        /// <summary>
        /// Method that executes a SQL statement meant for grabbing one object from the database. 
        /// </summary>
        /// <param name="SQLString">SQL statement to be executed</param>
        /// <returns>string representation of object from database</returns>
        /// <exception cref="Exception"></exception>
        public string ExecuteScalarSQL(string SQLString)
        {
            try
            {
                object obj;
                using (OleDbConnection connection = new OleDbConnection(ConnectionString))
                {
                    using (OleDbDataAdapter adapter = new OleDbDataAdapter())
                    {
                        // Open the connection 
                        connection.Open();

                        // add the information for the selectcomand using the sql statement and the connection object 
                        adapter.SelectCommand = new OleDbCommand(SQLString, connection);
                        adapter.SelectCommand.CommandTimeout = 0;

                        // execute the scalar SQL statement
                        obj = adapter.SelectCommand.ExecuteScalar();
                    }
                }

                // see if the object is null
                return obj?.ToString() ?? string.Empty;
            }
            catch (Exception ex)
            {
                // Throw the exception with a message 
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }
        /// <summary>
        /// This method takes an SQL statement that is a non query and executes it.
        /// </summary>
        /// <param name="sSQL">The SQL statement to be executed.</param>
        /// <returns>Returns the number of rows affected by the SQL statement.</returns>
        public int ExecuteNonQuery(string sSQL)
        {
            try
            {
                //Number of rows affected
                int iNumRows;

                using (OleDbConnection conn = new OleDbConnection(ConnectionString))
                {
                    //Open the connection to the database
                    conn.Open();

                    //Add the information for the SelectCommand using the SQL statement and the connection object
                    OleDbCommand cmd = new OleDbCommand(sSQL, conn);
                    cmd.CommandTimeout = 0;

                    //Execute the non query SQL statement
                    iNumRows = cmd.ExecuteNonQuery();
                }

                //return the number of rows affected
                return iNumRows;
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }
    }
}
