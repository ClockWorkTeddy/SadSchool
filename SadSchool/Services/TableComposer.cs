namespace SadSchool.Services
{
    public class TableComposer<T>
    {
        private List<T> _tableData = new List<T>();
        private List<T> _xAxisData = new List<T>();
        private List<T> _yAxisData = new List<T>();

        public TableComposer(List<T> tableData)
        {
            _tableData = tableData;
        }

        public void GetXAxis()
        {
            
        }
    }
}
