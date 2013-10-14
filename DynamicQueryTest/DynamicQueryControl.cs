using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;
using System.Linq.Expressions;

namespace DynamicQueryTest
{
    public partial class DynamicQueryControl : UserControl
    {
        #region Private
        DynamicQueryBase query;
        Action loadDataAction;
        Func<string, string> localizationMethod;
        List<QueryElement> elements;
        ErrorProvider error;
        #endregion

        #region ctor
        public DynamicQueryControl()
        {
            error = new ErrorProvider();
            elements = new List<QueryElement>();
            InitializeComponent();
        }

        #endregion

        #region Public Properties
        /// <summary>
        /// the Query that will be used to build the visual part
        /// </summary>
        public DynamicQueryBase Query
        {
            get
            {
                return query;
            }
            set
            {
                if (query!=value)
                {
                    query = value;
                    PrepareView();
                }                
            }
        }

        /// <summary>
        /// the delegate for action that will be called after query validation by the calling class
        /// </summary>
        public Action LoadDataAction
        {
            get
            {
                return loadDataAction;
            }
            set
            {
                if (loadDataAction!=value)
                {
                    loadDataAction = value;
                }
            }
        }

        #endregion

        #region Private methods
        private void PrepareView()
        {
            ClearView();
            if (query == null) return;
            localizationMethod = query.GetLocalizedString;
            Type type = query.GetType();
            PropertyInfo[] props = type.GetProperties();
            foreach (PropertyInfo p in props)
            {
                QueryFieldAttribute at = p.GetCustomAttribute(typeof(QueryFieldAttribute)) as QueryFieldAttribute;
                if (at!=null)
                {
                    QueryElement element = new QueryElement();
                    element.Property = p; //Remember PropertyInfo for setting values
                    element.IsRequired = at.IsRequired;
                    element.DefaultValue = at.DefaultValue;
                    element.ElementType = QueryTypes.SingleValue;
                    if (!String.IsNullOrEmpty(at.DataMethodName))
                    {
                        PropertyInfo method = type.GetProperty(at.DataMethodName);
                        if (method!=null)
                        {
                            QueryMethodAttribute mat = method.GetCustomAttribute(typeof(QueryMethodAttribute)) as QueryMethodAttribute;
                            if (mat != null)
                            {
                                object res = method.GetValue(query);
                                Func<DataTable> getDataMethod = method.GetValue(query) as Func<DataTable>; 
                                if (getDataMethod != null)
                                {
                                    element.ElementType = mat.IsFixedQuery ? QueryTypes.FixedList : QueryTypes.EditableList;
                                    element.DisplayMember = mat.DisplayMember;
                                    element.ValueMemebr = mat.ValueMember;
                                    element.GetData = getDataMethod;
                                }
                            }
                        }
                        //MethodInfo method = type.GetMethod(at.DataMethodName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                        //if (method != null && method.ReturnType.Equals(typeof(DataTable)))
                        //{
                        //    QueryMethodAttribute mat = method.GetCustomAttribute(typeof(QueryMethodAttribute)) as QueryMethodAttribute;
                        //    if (mat!=null)
                        //    {
                        //        Func<DataTable> getDataMethod = Expression.Lambda<Func<DataTable>>(Expression.Call(Expression.Constant(query), method)).Compile();
                        //        if (getDataMethod!=null)
                        //        {
                        //            element.ElementType = mat.IsFixedQuery ? QueryTypes.FixedList : QueryTypes.EditableList;
                        //            element.DisplayMember = mat.DisplayMember;
                        //            element.ValueMemebr = mat.ValueMember;
                        //            element.GetData = getDataMethod;
                        //        }
                        //    }
                        //}
                    }
                    element.Localize = localizationMethod;
                    element.Parent = flowLayoutPanel1;
                    element.Initialize();
                    elements.Add(element);
                    
                }
            }
        }

        private void ClearView()
        {
            localizationMethod = null;
            foreach (QueryElement element in elements)
            {
                element.Dispose();
            }
            elements.Clear();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (IsValid())
            {
                PrepareReturnResults();
                if (loadDataAction != null)
                {
                    loadDataAction();
                }
            }            
        }

        private void PrepareReturnResults()
        {
            Dictionary<PropertyInfo, bool> assignedValues = new Dictionary<PropertyInfo, bool>();
            foreach (QueryElement item in elements)
            {
                bool hasvalue = true;
                if (item.HasValue)
                {
                    item.Property.SetValue(query,item.GetCurrentValue());
                }
                else
                {
                    hasvalue = false;
                }
                assignedValues.Add(item.Property, hasvalue);
            }
            query.AssignedValues = assignedValues;
        }

        private bool IsValid()
        {
            error.Clear();            
            bool res = true;
            foreach (QueryElement item in elements)
            {
                if (!item.IsValid)
                {
                    if (item.Property.PropertyType==typeof(string))
                    {
                        error.SetError(item.Header, "String values cannot be empty!");
                    }
                    else
                    {
                        error.SetError(item.Header, "Check spelling!");
                    }
                    res = false;
                }
            }
            if (!res)
            {
                ShowNotification();
            }
            else
            {
                HideNotification();
            }
            return res;
        }

        private void ShowNotification()
        {
            label1.Visible = true;
        }

        private void HideNotification()
        {
            label1.Visible = false;
        }
        #endregion

    }
}
