using System;
using System.Data;
using System.Data.SqlClient;
using System.ComponentModel;
using System.Windows;
using MySql;
using MySql.Data.MySqlClient;
using MyIM;

namespace MyIM
{
    public class ClassOptionData:Component
    {
        private BaseDB DB = new BaseDB();
        public ClassOptionData()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
        }

        public int ExSQL(string SQLStr)
        {
            try
            {
                DataTable dt = new DataTable();
                dt = DB.DB.GetData(SQLStr);
                return dt.Rows.Count;
            }
            catch
            { return 0; }
        }

        public object ExSQLReField(string field, string SQLStr)//执行任何SQL查询语句，返回一个字段值
        {
            try
            {
                DataTable dt = new DataTable();
                dt = DB.DB.GetData(SQLStr);
                object fieldValue = null;
                fieldValue = dt.Rows[0][field];
                return fieldValue;
                //SqlConnection cnn = new SqlConnection(ConStr);
                //SqlCommand cmd = new SqlCommand(SQLStr, cnn);
                //cnn.Open();
                //SqlDataReader dr;
                //object fieldValue = null;
                //dr = cmd.ExecuteReader();
                //if (dr.Read())
                //{ fieldValue = dr[field]; }
                //cmd.Dispose();
                //cnn.Close();
                //cnn.Dispose();
                //return fieldValue;
            }
            catch { return null; }

        }

        public DataTable ExSQLReDr(string SQLStr)//执行任何SQL查询语句，返回一个SqlDataReader
        {
            try
            {
                DataTable dt = new DataTable();
                dt=DB.DB.GetData(SQLStr);
                return dt;                
            }
            catch { return null; }
        }
    }
}
