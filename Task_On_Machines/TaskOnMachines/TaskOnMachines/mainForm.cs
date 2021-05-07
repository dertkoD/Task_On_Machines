using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TaskOnMachines
{
    public partial class mainForm : Form
    {
        public mainForm()
        {
            InitializeComponent();
        }

        double[,] density = { { 0, 0, 0.2, 0.4 }, { 0.15, 0.35, 0.45, 0.65 }, { 0.4, 0.65, 1, 1 } };

        double min_density = 0; double max_density = 1;

        double MFTrap(double a, double b, double c, double d, double xmin, double xmax, double x)
        {
            if ((x >= xmin) && (x <= xmax))
            {
                if ((x >= a) && (x <= b)) return (1 - ((b - x) / (b - a)));
                if ((x >= b) && (x <= c)) return 1;
                if ((x >= c) && (x <= d)) return (1 - ((x - c) / (d - c)));
                else return 0;
            }
            else return 0;
        }

        private void mainForm_Load(object sender, EventArgs e)
        {
            dataGridView1.ColumnCount = 6;

            dataGridView1.Columns[0].HeaderText = "ID";
            dataGridView1.Columns[1].HeaderText = "Плотность";
            dataGridView1.Columns[2].HeaderText = "Терм 'Малая'";
            dataGridView1.Columns[3].HeaderText = "Терм 'Средняя'";
            dataGridView1.Columns[4].HeaderText = "Терм 'Большая'";
            dataGridView1.Columns[5].HeaderText = "Длительность зеленого сигнала, с";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Stream path = null;
            OpenFileDialog OpenTags = new OpenFileDialog();
            OpenTags.Filter = "All file (*.*) | *.*| Text file |*.txt";
            OpenTags.FilterIndex = 2;

            if (OpenTags.ShowDialog() == DialogResult.OK)
            {
                if ((path = OpenTags.OpenFile()) != null)
                {
                    StreamReader read = new StreamReader(path);
                    while (!read.EndOfStream)
                    {
                        var line = read.ReadLine();
                        var values = line.Split('\n');
                        var rowIndex = dataGridView1.Rows.Add();

                        for (int i = 0; i < values.Length; i++)
                        {
                            dataGridView1.Rows[rowIndex].Cells[i + 1].Value = values[i];
                        }
                    }
                }
            }

            for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
            {
                dataGridView1.Rows[i].Cells[0].Value = i + 1;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            double[] duration = { 4, 8, 12 };
            double duration_gs = 0; double s = 0;

            for (int j = 0; j < dataGridView1.Rows.Count - 1; j++)
            {
                dataGridView1[2, j].Value = Math.Round(MFTrap(density[0, 0], density[0, 1], density[0, 2], density[0, 3], min_density, max_density, Convert.ToDouble(dataGridView1[1, j].Value)), 2);
                dataGridView1[3, j].Value = Math.Round(MFTrap(density[1, 0], density[1, 1], density[1, 2], density[1, 3], min_density, max_density, Convert.ToDouble(dataGridView1[1, j].Value)), 2);
                dataGridView1[4, j].Value = Math.Round(MFTrap(density[2, 0], density[2, 1], density[2, 2], density[2, 3], min_density, max_density, Convert.ToDouble(dataGridView1[1, j].Value)), 2);

                duration_gs = Convert.ToDouble(dataGridView1[2, j].Value) * duration[0] + Convert.ToDouble(dataGridView1[3, j].Value) * duration[1] + Convert.ToDouble(dataGridView1[4, j].Value) * duration[2];
                s = Convert.ToDouble(dataGridView1[2, j].Value) + Convert.ToDouble(dataGridView1[3, j].Value) + Convert.ToDouble(dataGridView1[4, j].Value);
                dataGridView1[5, j].Value = Math.Round(duration_gs / s, 2);
            }
        }
    }
}
