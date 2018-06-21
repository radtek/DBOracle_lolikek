using System;
using System.Windows.Forms;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
namespace SBD
{
    public partial class Table : Form
    {
        OracleDataAdapter da;
        public Table(OracleConnection cn, String cmd , String name, bool v)
        {
            InitializeComponent();
            da = new OracleDataAdapter(cmd, cn);
            da.Fill(ds, name);
            dgvTable.DataSource = ds;
            dgvTable.DataMember = name;
            dgvTable.Visible = v;

        }

        public string getCell(int rowNum)
        {
            DataGridViewCell cell = dgvTable.Rows[rowNum].Cells[0];
            dgvTable.CurrentCell = cell;
            dgvTable.CurrentCell.Selected = true;
            return (string)this.dgvTable.CurrentRow.Cells[0].Value.ToString();
        }

        private void Table_Load(object sender, EventArgs e)
        {
            

        }
    }
}
