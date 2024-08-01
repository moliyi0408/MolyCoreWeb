
using System.ComponentModel;
using System.Reflection;
using static MolyCoreWeb.Models.Services.DataTableModel;

namespace MolyCoreWeb.Services
{
    public class DataTableService : IDataTableService
    {
        private DataTable DataTable = new DataTable();

        public void AutoSetColumns<T>()
        {
   
            PropertyInfo[] properties = typeof(T).GetProperties();
            foreach (PropertyInfo propertyInfo in properties)
            {
                var propertyName = propertyInfo.Name;
                DisplayNameAttribute customAttribute = propertyInfo.GetCustomAttribute<DisplayNameAttribute>();
                if (customAttribute != null)
                {
                    Column item = new Column
                    {
                        field = propertyInfo.Name,
                        title = customAttribute.DisplayName
                    };
                    DataTable.columns.Add(item);
                }
                else
                {
                    Column item2 = new Column
                    {
                        field = propertyInfo.Name,
                        title = propertyInfo.Name
                    };
                    DataTable.columns.Add(item2);
                }
            }
        }
     
        public void SetRowData(IEnumerable<object> dataList)
        {
            foreach (var data in dataList)
            {
                DataTable.data.Add(new RowData { Data = data });
            }
        }

        public DataTable GetDataTable()
        {
            return DataTable;
        }
    }
}
