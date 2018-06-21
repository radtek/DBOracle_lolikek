using System;
using System.Windows.Forms;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System.Data;

namespace SBD
{
    public partial class MainForm : Form
    {
        String warehouses = "SELECT name, GOOD_COUNT from (SELECT GOOD_ID, sum(GOOD_COUNT) as GOOD_COUNT FROM(SELECT * FROM WAREHOUSE1 UNION SELECT * FROM WAREHOUSE2) t group by GOOD_ID), GOODS where GOODS.ID = GOOD_ID";
        String top5 = "SELECT NAME, GOOD_COUNT FROM (SELECT GOOD_ID,sum(GOOD_COUNT) as GOOD_COUNT FROM SALES group by GOOD_ID order by GOOD_COUNT DESC), GOODS where rownum < 6 AND GOODS.ID = GOOD_ID";
        String sales = "SELECT NAME,GOOD_COUNT,CREATE_DATE FROM SALES,GOODS where GOOD_ID = GOODS.ID order by CREATE_DATE";
        String goods = "SELECT NAME, PRIORITY FROM GOODS";


        OracleConnection cn;
        public MainForm()
        {
            InitializeComponent();
            tableToolStripMenuItem.Enabled = false;
            
        }
        private void gun_TextChanged(object sender, EventArgs e)
        {
            Table persons = new Table(cn, "select priority from goods where name = '" + gun.Text+ "'", "Persons", false);
            gup.Text = persons.getCell(0);
        }

        private void tabControl1_Click(object sender, EventArgs e)
        {
        }

        private void sdel_id_TextChanged(object sender, EventArgs e)
        {
            Table persons = new Table(cn, "select GOOD_COUNT from sales where id = " + sci.Text, "Persons", false);
            scc.Text = persons.getCell(0);
            persons = new Table(cn, "select NAME from sales,goods where goods.id = GOOD_ID and sales.id = " + sci.Text, "Persons", false);
            sn.Text = persons.getCell(0);
        }

        private void showMessage(string s)
        {
            MessageBox.Show(s, "System", MessageBoxButtons.OK);
        }

        private void studentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Table persons = new Table(cn, top5, "Students" , true);
            persons.MdiParent = this;
            persons.Show();
        }

        private void personsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Table persons = new Table(cn, warehouses, "Warehouse", true);
            persons.MdiParent = this;
            persons.Show();
        }

        private void salesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Table persons = new Table(cn, sales, "Warehouse", true);
            persons.MdiParent = this;
            persons.Show();
        }

        private void goodsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Table persons = new Table(cn, goods, "Warehouse", true);
            persons.MdiParent = this;
            persons.Show();
        }

        private void connectToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            
            try
            {
               
                string connectionString = "DATA SOURCE=192.168.56.101:1521/xe;PERSIST SECURITY INFO=True;USER ID=SYSTEM;PASSWORD=ouser";
                cn = new OracleConnection(connectionString);
                cn.Open();
                tableToolStripMenuItem.Enabled = true;
                update();
                MessageBox.Show("YRA", "Connection", MessageBoxButtons.OK);
            }
            catch
            {
                MessageBox.Show("No", "Connection", MessageBoxButtons.OK);
            }
        }

        private void up_goods()
        {
            OracleCommand IDS = new OracleCommand("select name from goods", cn);
            OracleDataReader rdr = IDS.ExecuteReader();

            gdn.Items.Clear();
            while (rdr.Read())
                gdn.Items.Add(rdr["name"]);
            gdn.SelectedIndex = 0;

            IDS = new OracleCommand("select name from goods", cn);
            rdr = IDS.ExecuteReader();
            gun.Items.Clear();
            while (rdr.Read())
                gun.Items.Add(rdr["name"]);
            gun.SelectedIndex = 0;
        }

        private void up_sales()
        {
            OracleCommand IDS = new OracleCommand("select name from goods", cn);
            OracleDataReader rdr = IDS.ExecuteReader();

            zcn.Items.Clear();
            while (rdr.Read())
                zcn.Items.Add(rdr["name"]);
            zcn.SelectedIndex = 0;

            IDS = new OracleCommand("select ID from sales", cn);
            rdr = IDS.ExecuteReader();
            sdn.Items.Clear();
            while (rdr.Read())
                sdn.Items.Add(rdr["ID"]);
            sdn.SelectedIndex = 0;

            IDS = new OracleCommand("select ID from sales", cn);
            rdr = IDS.ExecuteReader();
            sci.Items.Clear();
            while (rdr.Read())
                sci.Items.Add(rdr["ID"]);
            sci.SelectedIndex = 0;

        }

        private void up_dop()
        {
            OracleCommand IDS = new OracleCommand("select name from goods", cn);
            OracleDataReader rdr = IDS.ExecuteReader();

            goods_1.Items.Clear();
            while (rdr.Read())
                goods_1.Items.Add(rdr["name"]);
            goods_1.SelectedIndex = 0;

            IDS = new OracleCommand("select distinct name from goods,sales where goods.id = GOOD_ID", cn);
            rdr = IDS.ExecuteReader();

            final.Items.Clear();
            while (rdr.Read())
                final.Items.Add(rdr["name"]);
            final.SelectedIndex = 0;




        }

        private void update()
        {
            up_goods();
            up_sales();
            up_dop();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure?", " Внимание!", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                if (gname.Text == "" || gprior.Text =="")
                {
                    showMessage("Заполните поля для дальнейшей работы");
                    return;
                }

                int s = 0;
                if (!int.TryParse(gprior.Text, out s))
                {
                    showMessage("Приоритет должен быть типа int");
                    return;
                }

                Table gt = new Table(cn, "SELECT count(*) FROM goods where name = '" + gname.Text +"'", "Tempo", false);
                if (gt.getCell(0) != "0")
                {
                    showMessage("Товар уже существует");
                    return;
                }

                try
                {
                    OracleCommand cmd = new OracleCommand();
                    cmd.Connection = cn;
                    cmd.CommandText = "addgood";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("n", OracleDbType.Varchar2).Value = gname.Text;
                    cmd.Parameters.Add("pr", OracleDbType.Int32).Value = gprior.Text;
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Sucsess!");
                    update();
                }
                catch (OracleException exc)
                {
                    MessageBox.Show(exc.ToString());
                }              
             
            }
        }

        private void cd_button_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure?", " Внимание!", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                if (gup.Text == "")
                {
                    showMessage("Заполните поля для дальнейшей работы");
                    return;
                }

                int s = 0;
                if (!int.TryParse(gup.Text, out s))
                {
                    showMessage("Приоритет должен быть типа int");
                    return;
                }

                try
                {
                    OracleCommand cmd = new OracleCommand();
                    cmd.Connection = cn;
                    cmd.CommandText = "upgood";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("n", OracleDbType.Varchar2).Value = gun.Text;
                    cmd.Parameters.Add("pr", OracleDbType.Int32).Value = gup.Text;
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Sucsess!");
                    update();
                }
                catch (OracleException exc)
                {
                    MessageBox.Show(exc.ToString());
                }

            }
        }

        private void delete_button_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure?", " Внимание!", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {

                try
                {
                    OracleCommand cmd = new OracleCommand();
                    cmd.Connection = cn;
                    cmd.CommandText = "delgood";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("n", OracleDbType.Varchar2).Value = gdn.Text;
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Sucsess!");
                    update();
                }
                catch (OracleException exc)
                {
                    MessageBox.Show(exc.ToString());
                }

            }
        }

        private void cp_button_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure?", " Внимание!", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                if (zcc.Text == "")
                {
                    showMessage("Заполните поля для дальнейшей работы");
                    return;
                }

                int s = 0;
                if (!int.TryParse(zcc.Text, out s))
                {
                    showMessage("Приоритет должен быть типа int");
                    return;
                }

                try
                {
                    OracleCommand cmd = new OracleCommand();
                    cmd.Connection = cn;
                    cmd.CommandText = "addsale";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("na", OracleDbType.Varchar2).Value = zcn.Text;
                    cmd.Parameters.Add("co", OracleDbType.Int32).Value = zcc.Text;
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Sucsess!");
                    update();
                }
                catch (OracleException exc)
                {
                    MessageBox.Show(exc.ToString());
                }

            }
        }

        private void delsd_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure?", " Внимание!", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                
                try
                {
                    OracleCommand cmd = new OracleCommand();
                    cmd.Connection = cn;
                    cmd.CommandText = "addsale";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("n", OracleDbType.Varchar2).Value = sdn.Text;

                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Sucsess!");
                    update();
                }
                catch (OracleException exc)
                {
                    MessageBox.Show(exc.ToString());
                }

            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            String cmd = "SELECT sum(GOOD_COUNT) as COUNT , cast(CREATE_DATE as date) as DAY FROM SALES , GOODS WHERE SALES.GOOD_ID = GOODS.ID and name = '" + goods_1.Text + "' and cast(CREATE_DATE as date) BETWEEN '" + std.Text + "' AND '" + ed.Text + "' group by cast(CREATE_DATE as date) order by cast(CREATE_DATE as date)";
            Table persons = new Table(cn, cmd, "UP", true);
            persons.MdiParent = this;
            persons.Show();
        }


        private void know_Click(object sender, EventArgs e)
        {
            Graph g = new Graph(final.Text,cn);
            g.MdiParent = this;
            g.Show();
        }








        private void tabPage3_Click(object sender, EventArgs e)
        {

        }
        private void fileToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
        private void lastName_TextChanged(object sender, EventArgs e)
        {

        }
        private void tabPage1_Click(object sender, EventArgs e)
        {

        }
        private void MainForm_Load(object sender, EventArgs e)
        {

        }
        private void cd_fname_Click(object sender, EventArgs e)
        {

        }
        private void delete_id_TextChanged(object sender, EventArgs e)
        {
            // Table persons = new Table(cn, "SELECT FirstName FROM Person where IdTeacher ="+delete_id.Text, "Persons");
            //  delete_name.Text = persons.getCell(0);
            // Table personss = new Table(cn, "SELECT LastName FROM Person where IdTeacher =" + delete_id.Text, "Persons");
            //delete_lastname.Text = personss.getCell(0);
        }
        private void sdel_name_Click(object sender, EventArgs e)
        {

        }
        private void sdel_button_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure?", " Внимание!", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                string cmd = "Exec deleteStudent " + sci.Text + ";";
                //exec(cmd);
                MessageBox.Show("Sucsess!");

            }
        }
        private void cd_id_TextChanged(object sender, EventArgs e)
        {
            //Table persons = new Table(cn, "SELECT FirstName FROM Person where IdTeacher =" + cd_id.Text, "Persons");
            //cd_fname.Text = persons.getCell(0);
            // Table personss = new Table(cn, "SELECT LastName FROM Person where IdTeacher =" + cd_id.Text, "Persons");
            //cd_lname.Text = personss.getCell(0);
            //Table personsss = new Table(cn, "exec get_room_byPerson " + cd_id.Text , "Persons");
            //cd_roomn.Text = personsss.getCell(0);
        }
        private void cp_id_TextChanged(object sender, EventArgs e)
        {

            //Table persons = new Table(cn, "select Name from Student where IdStudent = " + cp_id.Text, "Persons");
            //cp_name.Text = persons.getCell(0);
            //Table personss = new Table(cn, "exec get_class_byStudent " + cp_id.Text, "Persons");
            //cp_class.Text = personss.getCell(0);

        }
        private void cadd_button_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure?", " Внимание!", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {


                //string cmd = "Exec AddStudent" + "'" + cadd_name.Text + "', '" + cadd_class.Text + "'";
                //exec(cmd);
                MessageBox.Show("Sucsess!");
                //cadd_name.Text = "";
            }
        }
        private void select_Click(object sender, EventArgs e)
        {
            // Table persons = new Table(cn, "exec showPassStudents '" + coursers.Text +"'", "Students");
            //persons.MdiParent = this;
            //persons.Show();
        }
        
        private void no_three_Click(object sender, EventArgs e)
        {
            //Table persons = new Table(cn, "exec showPersonsNoThree " , "Students");
            //persons.MdiParent = this;
            //persons.Show();
        }
        private void button_show_Click(object sender, EventArgs e)
        {
            //Table persons = new Table(cn, "exec showYchPlan '" + Classs.Text + "'", "Students");
            //persons.MdiParent = this;
            //persons.Show();
        }

        
    }
    
    
 }

