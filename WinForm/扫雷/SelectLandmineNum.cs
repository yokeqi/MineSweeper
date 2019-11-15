using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace 扫雷
{
    public partial class SelectLandmineNum : Form
    {
        Form1 FatherForm;
        public SelectLandmineNum(Form1 fatherform)
        {
            InitializeComponent();
            FatherForm = fatherform;
        }

        private void SelectLandmineNum_Load(object sender, EventArgs e)
        {
            radioButton1.Text = "初级(&B)\n10个雷\n9×9平铺网络";
            radioButton2.Text = "中级(&I)\n40个雷\n16×16平铺网络";
            radioButton3.Text = "高级(&V)\n99个雷\n16*30平铺网络";
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            label1.Enabled = textBox1.Enabled = radioButton4.Checked;
            label2.Enabled = textBox2.Enabled = radioButton4.Checked;
            label3.Enabled = textBox3.Enabled = radioButton4.Checked;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                FatherForm.str.Width = FatherForm.str.Height = 9;
                FatherForm.str.Nlandmines = 10;
            }
            else if (radioButton2.Checked)
            {
                FatherForm.str.Width = FatherForm.str.Height = 16;
                FatherForm.str.Nlandmines = 40;
            }
            else if (radioButton3.Checked)
            {
                FatherForm.str.Width = 30;
                FatherForm.str.Height = 16;
                FatherForm.str.Nlandmines = 99;
            }
            else if (textBox1.Text=="" || textBox2.Text == "" || textBox3.Text == "")
            {
                string sMsg = "请先设置 ";
                if (textBox1.Text == "")
                     sMsg+="宽度 ";
                if (textBox1.Text == "")
                     sMsg+="高度 ";
                if (textBox1.Text == "")
                     sMsg+="雷数 ";
                MessageBox.Show(sMsg);
                return;
            }
            else
            {
                int a = 0, b = 0;
                a = Convert.ToInt32(textBox1.Text);
                if (a < 9 && a > 24)
                {
                    MessageBox.Show("宽度的参数设置不对");
                    return;
                }
                b = Convert.ToInt32(textBox2.Text);
                if (b <= 9 && b > 30)
                {
                    MessageBox.Show("高度的参数设置不对");
                    return;
                }
                if (Convert.ToInt32(textBox3.Text) >= 10 && Convert.ToInt32(textBox1.Text) <= a * b)
                {
                    FatherForm.str.Nlandmines = Convert.ToInt32(textBox3.Text);
                    FatherForm.str.Width = b;
                    FatherForm.str.Height = a;
                }
                else
                {
                    MessageBox.Show("雷数的参数设置范围因是 10-" + a*b);
                    return;
                }
            }
            this.DialogResult = DialogResult.OK;
        }
    }
}
