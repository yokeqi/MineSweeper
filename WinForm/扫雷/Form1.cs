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
    #region struct cstr
    public struct cstr
    {
        #region 属性字段 Width、Height、nlandmines
        private int width;

        public int Width
        {
            get { return width; }
            set { width = value; }
        }
        private int height;

        public int Height
        {
            get { return height; }
            set { height = value; }
        }
        private int nlandmines;

        public int Nlandmines
        {
            get { return nlandmines; }
            set { nlandmines = value; }
        }
        #endregion
    }
    #endregion
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        SearchLandmines Sl;
        public cstr str;
        public int times = 0;


        private void Form1_Load(object sender, EventArgs e)
        {
            str.Width = 9;
            str.Height = 9;
            str.Nlandmines = 10;
            Sl = new SearchLandmines(str.Width, str.Height, str.Nlandmines, panel2,imageList1,this);
            timer1.Enabled = true;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            textBox1.Text = (times++).ToString();
            if (times > 999)
            {
                timer1.Enabled = false;
            }
        }

        #region 帮助、联机获取更多信息
        private void 查看帮助ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://gitee.com/yokeqi/game_saolei");
        }

        private void 关于扫雷ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
        #endregion
        
        private void 选项OToolStripMenuItem_Click(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            SelectLandmineNum sln = new SelectLandmineNum(this);
            sln.ShowDialog();
            if (sln.DialogResult == DialogResult.OK)
            {
                Sl.Dispose();
                Sl = new SearchLandmines(str.Width, str.Height, str.Nlandmines, panel2, imageList1, this);
            }
            timer1.Enabled = true;
        }

        private void 更改外观ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Sl.Dispose();
        }

        private void 新游戏ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form1_Load(null, null);
        }

        private void 退出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
