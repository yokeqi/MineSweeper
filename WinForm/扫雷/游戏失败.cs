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
    public partial class 游戏失败 : Form
    {
        public 游戏失败(Form1 FatherForm)
        {
            InitializeComponent();
            label2.Text += FatherForm.textBox1.Text + " 秒";
            label3.Text += DateTime.Now.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.zsc.edu.cn");
        }
    }
}
