namespace MolyCoreWeb.Models.Services
{
    public class DataTableModel
    {
        public class Column
        {
            public string field { get; set; }
            public string title { get; set; }
        }

        public class RowData
        {
            public object Data { get; set; }
            public Dictionary<string, string> CssList { get; set; }
        }

        public class DataTable
        {
            public List<Column> columns { get; set; } = new List<Column>();
            public List<RowData> data { get; set; } = new List<RowData>();
        }
    }
}
