using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GridMakerProject
{
    public partial class Form2 : Form
    {
        public GridSetting gS { get; private set; }
        Color cl2 = Color.Black;
        public Form2(bool SubGridChecked,GridSetting SubGridSetting)
        {
            InitializeComponent();
            if(SubGridChecked)
            {
                checkBox1.Checked = true;
            }
            if(SubGridSetting != null)
            {
                gS = SubGridSetting;
                numericUpDown2_3.Value = gS.rows;
                numericUpDown2_4.Value = gS.rows;
                cl2=gS.color;
                numericUpDown2_1.Value = (decimal)gS.lineWeight;
                numericUpDown2_2.Value = (decimal)gS.lineOpacity*100;
            }
        }


        private void numericUpDown2_3_ValueChanged(object sender, EventArgs e)
        {
            numericUpDown2_4.Value = numericUpDown2_3.Value;
        }

        private void numericUpDown2_4_ValueChanged(object sender, EventArgs e)
        {
            numericUpDown2_3.Value = numericUpDown2_4.Value;
        }

        private void button2_1_Click(object sender, EventArgs e)
        {

            if (int.TryParse(numericUpDown2_3.Value.ToString(), out int rows)
                && int.TryParse(numericUpDown2_4.Value.ToString(), out int columns)
                && float.TryParse(numericUpDown2_1.Value.ToString(), out float lineWeight)
                && float.TryParse(numericUpDown2_2.Value.ToString(), out float lineOpacity))
            {
                lineOpacity *= 0.01f;
                gS = new GridSetting(rows, columns, GridType.Squares, cl2, Align.None, lineWeight, lineOpacity);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show($"입력 데이터가 올바르지 않습니다.");
            }
        }

        private void button2_7_Click(object sender, EventArgs e)
        {
            
            colorDialog2_1.ShowDialog();
            cl2 = colorDialog2_1.Color;
            button2_7.BackColor = cl2;
        }

    }
    
}
