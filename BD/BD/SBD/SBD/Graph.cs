using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
namespace SBD
{
    public partial class Graph : Form
    {
        String name;
        public Graph(String n, OracleConnection cn)
        {
            InitializeComponent();
            this.name = n;
            gr(cn);
        }

        private void gr(OracleConnection cn)
        {
            
            Table persons = new Table(cn, "select count(*) from (SELECT sum(GOOD_COUNT) as COUNT , cast(CREATE_DATE as date) as DAY FROM SALES , GOODS WHERE SALES.GOOD_ID = GOODS.ID and NAME ='"+name+ "'group by cast(CREATE_DATE as date) order by cast(CREATE_DATE as date))", "Persons", false);
            int n = Int32.Parse(persons.getCell(0));
            for (int i = 1; i < n+1; i++)
            {
                Table t = new Table(cn, "select day from(SELECT rownum as r, COUNT, DAY from (SELECT sum(GOOD_COUNT) as COUNT , cast(CREATE_DATE as date) as DAY FROM SALES , GOODS WHERE SALES.GOOD_ID = GOODS.ID and NAME ='" + name + "'group by cast(CREATE_DATE as date) order by cast(CREATE_DATE as date))) where r = "+i, "Persons", false);
                String d = t.getCell(0);
                t = new Table(cn, "select COUNT from (SELECT rownum as r, COUNT, DAY from (SELECT sum(GOOD_COUNT) as COUNT , cast(CREATE_DATE as date) as DAY FROM SALES , GOODS WHERE SALES.GOOD_ID = GOODS.ID and NAME ='" + name + "'group by cast(CREATE_DATE as date) order by cast(CREATE_DATE as date))) where r = " + i, "Persons", false);
                int num = Int32.Parse(t.getCell(0));
                this.chart1.Series["COUNT"].Points.AddXY(d, num);
            }
            
        }

       
    }
}
