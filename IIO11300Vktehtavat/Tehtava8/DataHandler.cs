using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tehtava5
{
    class DataHandler
    {
        DataReader dataReader;

        public DataHandler()
        {
            dataReader = new DataReader();
        }

        public DataTable getDataTable()
        {
            return getDataTable("");
        }

        public DataTable getDataTable(string search)
        {
            DataReader attendanceReader = new DataReader();
            ArrayList attendances = attendanceReader.ReadToArrayList(search);

            if (attendances == null) return null;

            DataTable attendanceTable = new DataTable();
            attendanceTable.Columns.Add("asioid", typeof(string));
            attendanceTable.Columns.Add("lastname", typeof(string));
            attendanceTable.Columns.Add("firstname", typeof(string));
            attendanceTable.Columns.Add("date", typeof(string));

            foreach (Attendance attendee in attendances)
            {
                attendanceTable.Rows.Add(attendee.ToStringArray());
            }

            return attendanceTable;
        }

        public DataView getDataView()
        {
            return getDataView("");
        }

        public DataView getDataView(string search)
        {
            DataTable attendanceTable = getDataTable(search);
            if (attendanceTable == null) return null;
            DataView attendanceView = new DataView(attendanceTable);
            return attendanceView;
        }
    }
}
