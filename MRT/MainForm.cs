using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MRTLib;

namespace MRTRoutesApp
{
    public partial class MainForm : Form
    {
        private MRTRoute[] routes = null;

        public MainForm()
        {
            InitializeComponent();
            MRTMap map = MRTMap.SharedMap;
            Dictionary<string, MRTExit> exits = map.exits;
            string[] keys = exits.Keys.ToArray();
            this.fromComboBox.Items.AddRange(keys);
            this.toComboBox.Items.AddRange(keys);
        }

        private void UpdateRoutes()
        {
            this.routesDataGridView.Rows.Clear();
            object[] buffer = new object[3];
            List<DataGridViewRow> rows = new List<DataGridViewRow>();
            foreach (MRTRoute route in this.routes)
            {
                buffer[0] = route.StationCount;
                buffer[1] = route.TransionCount;
                buffer[2] = route.Description;
                rows.Add(new DataGridViewRow());
                rows[rows.Count - 1].CreateCells(this.routesDataGridView, buffer);
            }
            this.routesDataGridView.Rows.AddRange(rows.ToArray());
        }

        private void ShowRoute(MRTRoute route)
        {
            this.dataGridView.Rows.Clear();
            MRTExit lastExit= route.from;
            object[] buffer = new object[3];
            List<DataGridViewRow> rows = new List<DataGridViewRow>();
            foreach (MRTLink link in route.links)
            {
                buffer[0] = link.routeID;
                buffer[1] = lastExit.name;
                buffer[2] = link.to.name;

                rows.Add(new DataGridViewRow());
                rows[rows.Count - 1].CreateCells(this.dataGridView, buffer);
                lastExit = link.to;
            }
            this.dataGridView.Rows.AddRange(rows.ToArray());
        }

        private void button_Click(object sender, EventArgs e)
        {
            try
            {
                MRTRoute[] routes = MRTMap.SharedMap.FindRoutes(this.fromComboBox.Text, this.toComboBox.Text);
                this.routes = routes;
                this.UpdateRoutes();
                this.routesDataGridView.CurrentCell = this.routesDataGridView.Rows[0].Cells[0];
                this.ShowRoute(this.routes[0]);
            }
            catch (Exception exception) {
                MessageBox.Show(exception.Message);
            }
        }

        private void routesDataGridView_SelectionChanged(object sender, EventArgs e)
        {
            int selectedRow = this.routesDataGridView.CurrentRow.Index;
            this.ShowRoute(this.routes[selectedRow]);
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("MRT Route Finder\nby Weizhong Yang. A.k.a zonble\nzonble@gmail.com", "About");
        }
    }
}
