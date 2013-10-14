using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DynamicQueryTest
{
    public partial class Form1 : Form
    {
        DynamicQueryTestClass query;

        public Form1()
        {
            InitializeComponent();
            InitializeQuery();
        }

        private void InitializeQuery()
        {
            dynamicQueryControl1.LoadDataAction = LoadData;
            query = new DynamicQueryTestClass();
            query.GetCountries = GetCountries;
            query.GetYears = GetYears;
            dynamicQueryControl1.Query = query;
        }

        private DataTable GetYears()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("ID", typeof(int));
            dt.Columns.Add("Description", typeof(string));
            dt.Rows.Add(2011, "Y2011");
            dt.Rows.Add(2012, "Y2012");
            dt.Rows.Add(2013, "Y2013");
            return dt;
        }

        private DataTable GetCountries()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Description", typeof(string));
            dt.Rows.Add("Russia");
            dt.Rows.Add("Hungary");
            dt.Rows.Add("EU");
            return dt;
        }

        private void LoadData()
        {
            
            //string results = PrepareString();
            //MessageBox.Show(results);
            DataTable data = new DataTable();
            data.Columns.Add("Name",typeof(string));
            data.Columns.Add("HasValue", typeof(string));
            data.Columns.Add("Value", typeof(string));
            foreach (var item in query.AssignedValues)
            {
                string name = item.Key.Name;
                string hasvalue = item.Value.ToString();
                string val = String.Empty;
                if (item.Value)
                {
                    val = item.Key.GetValue(query).ToString();
                }
                data.Rows.Add(name,hasvalue,val);
            }
            dataGridView1.DataSource = data;
        }

        //private string PrepareString()
        //{
        //    if (query.AssignedValues == null) return "no assigned results";
        //    StringBuilder build = new StringBuilder();
        //    foreach (var item in query.AssignedValues)
        //    {
        //        build.Append(item.Key.Name + ": has value = ");
        //        if (item.Value)
        //        {
        //            build.Append(item.Value + "; value = ");
        //            build.AppendLine(item.Key.GetValue(query).ToString() + ".");
        //        }
        //        else
        //        {
        //            build.AppendLine(item.Value + ".");
        //        }
        //    }
        //    return build.ToString();
        //}
    }
}
