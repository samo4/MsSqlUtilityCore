using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Threading.Tasks;

// http://www.c-sharpcorner.com/article/net-core-1-0-connecting-sql-server-database/
// http://msdn.microsoft.com/library/en-us/dnbda/html/daab-rm.asp

namespace MsSqlUtilityCore
{
    public abstract class SqlHelperCore
    {
        //[System.Diagnostics.DebuggerHidden]
        [DebuggerStepThrough]
        private static void PrepareCommand(SqlCommand cmd, SqlConnection conn, CommandType cmdType, string cmdText, SqlParameter[] cmdParms)
        {
            if (conn.State != ConnectionState.Open)
            {
                conn.Open();
            }

            cmd.Connection = conn;
            cmd.CommandText = cmdText;
            cmd.CommandType = cmdType;

            if (cmdParms != null)
            {
                cmd.Parameters.Clear();
                foreach (SqlParameter parm in cmdParms)
                    cmd.Parameters.Add(parm);
            }
        }

        [DebuggerStepThrough]
        public static int ExecuteNonQuery(string connString, CommandType cmdType, string cmdText, params SqlParameter[] cmdParms)
        {
            using (var conn = new SqlConnection(connString))
            {
                using (var cmd = new SqlCommand())
                {
                    PrepareCommand(cmd, conn, cmdType, cmdText, cmdParms);
                    int val = cmd.ExecuteNonQuery();
                    cmd.Parameters.Clear();
                    return val;
                }
            }
        }

        [DebuggerStepThrough]
        public static object ExecuteScalar(string connString, CommandType cmdType, string cmdText, params SqlParameter[] cmdParms)
        {
            using (var conn = new SqlConnection(connString))
            {
                using (var cmd = new SqlCommand())
                {
                    PrepareCommand(cmd, conn, cmdType, cmdText, cmdParms);
                    object val = cmd.ExecuteScalar();
                    cmd.Parameters.Clear();
                    return val;
                }
            }
        }

        [DebuggerStepThrough]
        public static IDataReader ExecuteReader(string connString, CommandType cmdType, string cmdText, params SqlParameter[] cmdParms)
        {
            SqlConnection conn = new SqlConnection(connString);
            SqlCommand cmd = new SqlCommand();

            // we use a try/catch here because if the method throws an exception we want to
            // close the connection throw code, because no datareader will exist, hence the
            // commandBehaviour.CloseConnection will not work
            try
            {
                PrepareCommand(cmd, conn, cmdType, cmdText, cmdParms);
                SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                cmd.Parameters.Clear();
                return rdr;
            }
            catch
            {
                conn.Close();
                throw;
            }
        }

        [DebuggerStepThrough]
        public static async Task<IDataReader> ExecuteReaderAsync(string connString, CommandType cmdType, string cmdText, params SqlParameter[] cmdParms)
        {
            SqlConnection conn = new SqlConnection(connString);
            SqlCommand cmd = new SqlCommand();

            // we use a try/catch here because if the method throws an exception we want to
            // close the connection throw code, because no datareader will exist, hence the
            // commandBehaviour.CloseConnection will not work
            try
            {
                PrepareCommand(cmd, conn, cmdType, cmdText, cmdParms);
                SqlDataReader rdr = await cmd.ExecuteReaderAsync(CommandBehavior.CloseConnection);
                cmd.Parameters.Clear();
                return rdr;
            }
            catch
            {
                conn.Close();
                throw;
            }
        }

        public static DataSet ExecuteDataset(string connString, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {
            using (var conn = new SqlConnection(connString))
            {
                using (var cmd = new SqlCommand())
                {
                    PrepareCommand(cmd, conn, commandType, commandText, commandParameters);

                    using (var da = new SqlDataAdapter(cmd))
                    {
                        var ds = new DataSet();
                        da.Fill(ds);
                        cmd.Parameters.Clear();
                        return ds;
                    }
                }
            }
        }
    }
}
