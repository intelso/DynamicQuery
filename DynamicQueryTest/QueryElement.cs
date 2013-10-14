using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DynamicQueryTest
{
    class QueryElement: IDisposable
    {
        #region Private
        Control header;
        Control editor;
        #endregion

        #region Properties
        public Control Editor
        {
            get
            {
                return editor;
            }
        }
        public Control Header
        {
            get
            {
                return header;
            }
        }
        public bool IsRequired { get; set; }
        
        public object DefaultValue { get; set; }
        public PropertyInfo Property { get; set; }
        public Func<DataTable> GetData { get; set; }
        public Func<string, string> Localize { get; set; }
        public Control Parent { get; set; }        
        public QueryTypes ElementType { get; set; }
        public string ValueMemebr { get; set; }
        public string DisplayMember { get; set; }

        internal bool IsValid
        {
            get
            {
                if (!editor.Enabled) return true;
                if (Property.PropertyType == typeof(string) && String.IsNullOrEmpty(editor.Text)) return false;
                object value = GetCurrentValue();
                if (value == null) return false;
                return true;
            }
        }
        internal bool HasValue
        {
            get
            {
                if (!editor.Enabled) return false;
                return GetCurrentValue() != null;
            }
        }
        #endregion

        #region IDisposable
        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                Localize = null;
                GetData = null;
                Property = null;
                if (Parent != null)
                {
                    Parent.Controls.Remove(header);
                    Parent.Controls.Remove(editor);
                }
                Parent = null;
                if (header!=null && header is CheckBox)
                {
                    ((CheckBox)header).CheckedChanged -= temp_CheckedChanged;
                }
                if (header!=null)
                {
                    header.Dispose();
                }
                if (editor != null)
                {
                    editor.Dispose();
                }
                header = null;
                editor = null;
            }
        }
        #endregion

        #region Private methods
        internal void Initialize()
        {
            CreateEditor();
            CreateHeader();
            AddControls();
            SetDefaultValue();
        }

        private void SetDefaultValue()
        {
            object value = GetDefaultValue();
            if (value != null)
            {
                if (editor is TextBox)
                {
                    ((TextBox)editor).Text = value.ToString();
                }
                if (editor is DateTimePicker)
                {
                    ((DateTimePicker)editor).Value = (DateTime)value;
                }
                if (editor is ComboBox)
                {
                    ComboBox edit = editor as ComboBox;
                    if (GetData != null)
                    {
                        edit.DisplayMember = DisplayMember;
                        edit.ValueMember = ValueMemebr;
                        edit.DataSource = GetData();
                    }
                    edit.SelectedValue = value; 
                }
            }
        }

        private void AddControls()
        {
            if (Parent != null)
            {
                Parent.Controls.Add(header);
                Parent.Controls.Add(editor);                
            }
        }

        private void CreateEditor()
        {
            if (ElementType == QueryTypes.SingleValue)
            {
                if (Property.PropertyType == typeof(DateTime) || Property.PropertyType == typeof(DateTime?))
                {
                    DateTimePicker temp = new DateTimePicker();
                    editor = temp;
                }
                else
                {
                    TextBox temp = new TextBox();
                    editor = temp;
                }
            }
            else
            {
                ComboBox temp = new ComboBox();
                if (ElementType== QueryTypes.FixedList || IsRequired)
                     temp.DropDownStyle = ComboBoxStyle.DropDownList;
                else temp.DropDownStyle = ComboBoxStyle.DropDown;
                
                editor = temp;
            }
            editor.Enabled = IsRequired;
        }

        private object GetDefaultValue()
        {
            if (DefaultValue == null) return null;
            object result = null;
            try
            {
                result = Convert.ChangeType(DefaultValue, Property.PropertyType);
            }
            catch { result = null; }
            return result;
        }

        private void CreateHeader()
        {
            if (IsRequired)
            {
                Label temp = new Label();
                temp.AutoSize = true;
                header = temp;
            }
            else
            {
                CheckBox temp = new CheckBox();
                temp.AutoSize = true;
                temp.CheckedChanged += temp_CheckedChanged;
                header = temp;
            }
            header.Name = "h" + Property.Name;
            header.Text = Localize(Property.Name);
            header.Margin = new Padding(3, 8, 0, 1);
        }

        void temp_CheckedChanged(object sender, EventArgs e)
        {
            if (editor!=null)
            {
                editor.Enabled = ((CheckBox)sender).Checked;
                editor.Select();
            }
        }

        #endregion

        #region Public methods

        public object GetCurrentValue()
        {
            object result = null;
            if (editor is TextBox)
            {
                try
                {
                    result = Convert.ChangeType(((TextBox)editor).Text,Property.PropertyType);
                }
                catch
                { result = null; }
            }
            if (editor is DateTimePicker)
            {
                try
                {
                    result = Convert.ChangeType(((DateTimePicker)editor).Value, Property.PropertyType);
                }
                catch
                { result = null; }
            }
            if (editor is ComboBox)
            {
                if (ElementType== QueryTypes.FixedList)
                {
                    try
                    {
                        result = Convert.ChangeType(((ComboBox)editor).SelectedValue, Property.PropertyType);
                    }
                    catch
                    { result = null; }
                }
                if (ElementType == QueryTypes.EditableList)
                {
                    try
                    {
                        result = Convert.ChangeType(((ComboBox)editor).Text, Property.PropertyType);
                    }
                    catch
                    { result = null; }
                }                
            }
            return result;
        }
        #endregion

    }
}
