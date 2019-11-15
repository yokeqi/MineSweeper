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
    public partial class 游戏胜利 : Form
    {
        public 游戏胜利(Form1 FatherForm)
        {
            InitializeComponent();
            label3.Text += FatherForm.textBox1.Text + " 秒";
            label6.Text += DateTime.Now.ToString();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.zsc.edu.cn");
        }

        private void 游戏胜利_Load(object sender, EventArgs e)
        {

        }
    }
}
