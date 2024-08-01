using static MolyCoreWeb.Models.Services.DataTableModel;

namespace MolyCoreWeb.Services
{
    public interface IDataTableService
    {
        void AutoSetColumns<T>();
        void SetRowData(IEnumerable<object> dataList);
        DataTable GetDataTable();
    }
}
