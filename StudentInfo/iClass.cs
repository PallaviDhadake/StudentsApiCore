using Microsoft.Data.SqlClient;
using System.Data;

namespace StudentInfo
{
    public class iClass
    {
        private readonly IConfiguration _configuration;

        public iClass(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GetConnectionString()
        {
            return ConfigurationExtensions.GetConnectionString(_configuration, "CRUDDbContext");
        }
        public DataTable GetDataTable(string strQuery)
        {
            SqlConnection con = new SqlConnection(GetConnectionString());

            SqlDataAdapter da = new SqlDataAdapter(strQuery, con);
            DataTable dt = new DataTable();

            da.Fill(dt);

            con.Close();
            con = null;

            return dt;
        }



        public DataTable GetDataTableWithParameter(string strQuery, params SqlParameter[] parameters)
        {
            DataTable dt = new DataTable();

            using (SqlConnection con = new SqlConnection(GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand(strQuery, con))
                {
                    // Add parameters to the command
                    if (parameters != null)
                    {
                        cmd.Parameters.AddRange(parameters);
                    }

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dt);
                    }
                }
            }

            return dt;
        }


        public void ExecuteQuery(string strQuery)
        {
            try
            {
                SqlConnection con = new SqlConnection(GetConnectionString());
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = strQuery;
                cmd.CommandTimeout = 3000000; // seconds
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                con.Close();
                con = null;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

    }
}
