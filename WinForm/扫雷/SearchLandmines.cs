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
    public class SearchLandmines
    {
        #region 变量定义 DataTypes
        private int oldLandmineNum; 
        private int nlandmines;  //雷数
        private int markCount;   //标志的数量
        private int Width;       //雷区的宽
        private int Height;      //雷区的高
        private int nAreaCount;  //界面中的 panel 个数
        private Panel vp;        //装 panel 的框架
        private int[,] mark;     //标记
        private bool[,] landmines;//雷的位置
        private Panel[,] panel;   //地雷
        private Label[,] label;   //位置四周的地雷数
        ImageList IL;             //图片集合
        Form1 FatherForm;

        public int Nlandmines//雷数
        {
            get { return nlandmines; }
            set
            {
                if (value>0 && markCount>0)
                {
                    nlandmines = value;
                    markCount = value;
                }
            }
        }

        public int MarkCount//标记数量
        {
            get { return markCount; }
        }

        public int NAreaCount
        {
            get { return nAreaCount; }
        }
        #endregion

        #region 构造函数
        public SearchLandmines(int w, int h,int ld, 
                        System.Windows.Forms.Panel p1,ImageList il,Form1 ftherform)
        {
            setWH(w, h, ld);//设置长、宽与雷数
            vp = p1;//框架
            IL = il;
            FatherForm = ftherform;

            Create();//创建游戏
        }
        #endregion

        #region 创建游戏 Create
        //
        //创建游戏
        //
        public void Create()
        {
            FormFace();
        }
        #endregion

        #region 重新开始游戏 ReStart
        //重新开始游戏
        public void ReStart(int a)
        {
            //a：1 重新开始本局 0 再来一局
            #region 复原每个 panel
            for (int i=0;i<Width;i++)
            {
                for (int j=0;j<Height;j++)
                {
                    panel[i, j].Size = new Size(19, 19);
                    panel[i, j].BorderStyle = BorderStyle.FixedSingle;
                    label[i, j].Visible = false;
                    panel[i, j].BackgroundImage = null;
                    panel[i, j].BackColor = Color.Navy;
                    mark[i, j] = 0;
                }
            }
            #endregion
            markCount = oldLandmineNum;
            nlandmines = oldLandmineNum;
            nAreaCount = Width * Height;
            FatherForm.textBox2.Text = markCount.ToString();
            FatherForm.times = 0;
            FatherForm.textBox1.Text = "0";
            if (a == 0)//再来一局
            {
                #region 清楚雷区
                for (int i = 0; i < Width; i++)
                {
                    for (int j = 0; j < Height; j++)
                    {
                        landmines[i, j] = false;//区中都没有雷
                        label[i, j].Text = "0";
                    }
                }
                #endregion
                ProductLandmine();//重新生成雷
            }
        }
        #endregion

        #region 设置框架大小 SetWH
        //
        //设置框架大小
        //
        public void setWH(int w,int h,int landmineNum)
        {
            if (w<0 || h<0 || landmineNum<0)
            {
                MessageBox.Show("数据出错，长宽或雷数不能小于 0 ");
            }
            Width = w;
            Height = h;
            nlandmines = landmineNum;
            panel = new Panel[Width, Height];
            label = new Label[Width, Height];
            mark = new int[Width, Height];
            landmines = new bool[Width, Height];
            markCount = landmineNum;
            oldLandmineNum = nlandmines;
        }
        #endregion

        #region 生成界面及变量的初始化 FormFace
        //
        //生成界面及变量的初始化
        //
        public void FormFace()
        {
            #region 重绘 窗体 与 panel2 控件
            FatherForm.Size = new Size(FatherForm.str.Width * 18 + 78, FatherForm.str.Height * 18 + 121);
            FatherForm.panel2.Size = new Size(FatherForm.str.Width * 18, FatherForm.str.Height * 18);
            FatherForm.pictureBox1.Location = new Point(33, FatherForm.panel2.Size.Height + 50);
            FatherForm.pictureBox2.Location = new Point(FatherForm.panel2.Size.Width + 1, FatherForm.pictureBox1.Location.Y);
            FatherForm.textBox1.Location = new Point(71, FatherForm.pictureBox2.Location.Y + 4);
            FatherForm.textBox2.Location = new Point(FatherForm.pictureBox2.Location.X - 40, FatherForm.pictureBox2.Location.Y + 4);
            FatherForm.times = 0;//游戏时间设 0
            FatherForm.textBox2.Text = markCount.ToString();
            nAreaCount = Width * Height;
            #endregion
            for (int i = 0; i < Width; i++)
            {
                for (int j = 0; j < Height; j++)
                {
                    mark[i, j] = 0;//区中没有标记

                    #region 初始化label
                    label[i, j] = new Label();
                    label[i, j].Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
                    label[i, j].ForeColor = System.Drawing.Color.Navy;
                    label[i, j].Location = new System.Drawing.Point(0, 0);
                    label[i, j].Name = ((i * 1000) + j).ToString();
                    label[i, j].Size = new System.Drawing.Size(20, 20);
                    label[i,j].MouseDown +=new MouseEventHandler(label_mouseDown);
                    label[i,j].MouseUp += new MouseEventHandler(label_mouseUp);
                    label[i, j].MouseDoubleClick += new MouseEventHandler(label_mouseClick);
                    #endregion

                    #region 初始化panel
                    panel[i, j] = new Panel();
                    panel[i, j].Location = new System.Drawing.Point(i*18, j*18);
                    panel[i, j].Name = ((i * 1000) + j).ToString();
                    panel[i, j].MouseEnter += new EventHandler(mouseEnter);
                    panel[i, j].MouseLeave += new EventHandler(mouseLeave);
                    panel[i,j].MouseDown +=new MouseEventHandler(mouseDown);
                    panel[i,j].MouseUp +=new MouseEventHandler(mouseUp);
                    panel[i,j].MouseDoubleClick +=new MouseEventHandler(mouseClick);
                    #endregion
                    panel[i, j].Controls.Add(label[i, j]);    //将 label 包含进 panel 里面
                    vp.Controls.Add(panel[i, j]);            //将 panel 包含进框架里
                }
            }
            ReStart(0);
        }
        #endregion

        #region 生成 nlnlandmines 个雷 ProdProductLandmine
        //生成 nLandmines 个雷
        public void ProductLandmine()
        {
            Random ro = new Random(10);
            long tick = DateTime.Now.Ticks;
            Random rd = new Random ((int)(tick & 0xfffffffL) | (int)(tick>>32));

            int [,] at = {{-1,-1},{0,-1},{+1,-1},{-1,0},{+1,0},{-1,+1},{0,+1},{+1,+1}};

            for(int i=0;i<nlandmines;i++)
            {
                int lx = rd.Next(0, Width);
                int ly = rd.Next(0, Height);
                while (landmines[lx,ly])
                {
                    lx = rd.Next(0, Width);
                    ly = rd.Next(0, Height);
                }
                landmines[lx,ly] = true;
                #region 将该雷附近的 label 的 text +1
                //将该雷附近的 label 的 text +1
                for (int j=0;j<8;j++)
                {
                    if (lx + at[j, 0] < 0 || ly + at[j, 1] < 0 || lx + at[j, 0] >= Width || ly + at[j, 1] >= Height)
                        continue;
                    int a = Convert.ToInt32(label[(lx+at[j,0]),(ly+at[j,1])].Text);
                    label[lx+at[j,0],ly+at[j,1]].Text = (a+1).ToString();
                }
                #endregion
            }
        }
        #endregion

        public void End()
        {
            DownTime = 0;
            #region 点到雷，游戏结束
            FatherForm.timer1.Enabled = false;
            #region 显示所有雷
            //游戏结束,显示所有雷
            for (int i = 0; i < Width; i++)
            {
                for (int j = 0; j < Height; j++)
                {
                    if (landmines[i, j])
                    {
                        panel[i, j].BackgroundImage = IL.Images[0];
                    }
                }
            }
            #endregion
            游戏失败 fail = new 游戏失败(FatherForm);
            fail.ShowDialog();
            if (fail.DialogResult == DialogResult.Yes)
            {
                ReStart(1);
            }
            else
            {
                ReStart(0);
            }
            FatherForm.timer1.Enabled = true;
            return;
            #endregion
        }

        #region label_mouseDown
        public void label_mouseDown(object sender, MouseEventArgs e)
        {
            Label lb = (Label)sender;
            #region 获取按键位置 (i,j)
            //获取按键位置 (i,j)
            int a = Convert.ToInt32(lb.Name);
            int i = a / 1000;
            int j = a % 1000;
            #endregion
            mouseDown(panel[i,j], e);
        }
        #endregion
        #region label_mouseUp
        public void label_mouseUp(object sender, MouseEventArgs e)
        {
            DownTime = 0;
        }
        #endregion

        #region Label_mouseCick
        public void label_mouseClick(object sender, MouseEventArgs e)
        {
            Label lb = (Label)sender;
            #region 获取按键位置 (i,j)
            //获取按键位置 (i,j)
            int a = Convert.ToInt32(lb.Name);
            int i = a / 1000;
            int j = a % 1000;
            #endregion
            DownTime = 1;
            mouseDown(panel[i, j], e);
            DownTime = 0;
        }
        #endregion
        #region mouseClick
        public void mouseClick(object sender, MouseEventArgs e)
        {
            DownTime = 1;
            mouseDown(sender, e);
            DownTime = 0;
        }
        #endregion
        #region 鼠标按下、松开
        int DownTime = 0;
        Panel now, old;
        private void mouseDown(object sender, MouseEventArgs e)
        {
            DownTime++;
            old = now;
            now = (Panel)sender;
            #region 获取按键位置 (i,j)
            //获取按键位置 (i,j)
            int a = Convert.ToInt32(now.Name);
            int i = a / 1000;
            int j = a % 1000;
            #endregion
            #region 左键按下且没按过
            if (e.Button == MouseButtons.Left && mark[i,j]!=1 && panel[i,j].BorderStyle!= BorderStyle.Fixed3D)
            {
                if (landmines[i, j])//是雷
                {
                    End();
                }
                else if (Convert.ToInt32(label[i, j].Text) != 0)//label不为0
                {
                    #region label的数字变色
                    switch (Convert.ToInt32(label[i, j].Text))//label的数字
                    {
                        //1 RoyalBlue, 2 Green, 3 (192,0,0), 4 DarkBlue 5 Maroon
                        case 1:
                            label[i, j].ForeColor = Color.RoyalBlue;
                            break;
                        case 2:
                            label[i, j].ForeColor = Color.Green;
                            break;
                        case 3:
                            label[i, j].ForeColor = Color.Red;
                            break;
                        case 4:
                            label[i, j].ForeColor = Color.DarkBlue;
                            break;
                        case 5:
                            label[i, j].ForeColor = Color.Maroon;
                            break;
                    }
                    #endregion
                    // 	                显示 label（i,j)
                    // 	                panel外观改为 3D,背景色改为 controlsLight
                    // 	                nAreaCount--;
                    label[i, j].Visible = true;
                    panel[i, j].BorderStyle = BorderStyle.Fixed3D;
                    panel[i, j].Size = new Size(20, 20);
                    panel[i, j].BackColor = SystemColors.ControlLight;
                    nAreaCount--;
                }
                else
                {
                    panel[i, j].BorderStyle = BorderStyle.FixedSingle;
                    Expand(i, j);
                }

                if (nAreaCount == 10)
                {
                    Congratulation();
                }
                return;
            }
            #endregion
            #region 直接按鼠标右键
            else if (e.Button == MouseButtons.Right && DownTime == 1 && panel[i,j].BorderStyle!= BorderStyle.Fixed3D)
            {
                switch (mark[i, j])//按右键时该位置的标记
                {
                    case 0:// 从无标记转为封印
                        if (landmines[i, j])//如果封印的那个刚好是雷
                        {
                            nlandmines--;//灭掉 1 个雷
                            nAreaCount--;//panel-1
                        }
                        markCount--;//标记数 --
                        break;
                    case 1:// 从封印转为问号
                        if (landmines[i, j])//之前封印的那个刚好是雷
                        {
                            nlandmines++;//恢复 1 个雷
                            nAreaCount++;//panel+1
                        }
                        markCount++;
                        break;
                }
                FatherForm.textBox2.Text = markCount.ToString();
                mark[i, j] = (mark[i, j] + 1) % 3;
                if (mark[i, j] != 0)
                    panel[i,j].BackgroundImage = IL.Images[mark[i, j]];
                else
                    panel[i,j].BackgroundImage = null;
            }
            #endregion
            #region 按下鼠标左右键
            
            else if (DownTime == 2 && Convert.ToInt32(label[i, j].Text) != 0 && panel[i, j].BorderStyle == BorderStyle.Fixed3D && label[i, j].Visible)
            {
                int nNum = 0;
                int nMark = 0;
                int[,] at = { { -1, -1 }, { 0, -1 }, { +1, -1 }, { -1, 0 }, { +1, 0 }, { -1, +1 }, { 0, +1 }, { +1, +1 } };
                for (int m = 0; m < 8; m++)
                {
                    int line = i + at[m, 0];
                    int low = j + at[m, 1];
                    if (line < 0 || low < 0 || line >= Width || low >= Height)
                        continue;
                    if (landmines[line, low] && mark[line, low] != 1)
                    {
                        nNum++;
                    }
                    if (mark[line, low] == 1)
                    {
                        nMark++;
                    }
                }
                if (nMark == Convert.ToInt32(label[i, j].Text) && nNum != 0)
                {
                    //标记做错位置
                    End();
                }
                else if (nMark != Convert.ToInt32(label[i, j].Text))
                {
                    return;
                }
                else if (nNum == 0)
                {
                    Expand(i - 1, j - 1);
                    Expand(i - 1, j + 1);
                    Expand(i + 1, j - 1);
                    Expand(i + 1, j + 1);
                    Expand(i + 1, j);
                    Expand(i - 1, j);
                    Expand(i, j - 1);
                    Expand(i, j + 1);
                    /*
                    
                                        for (int m = 0; m < 8; m++)
                                        {
                                            int line = i + at[m, 0];
                                            int low = j + at[m, 1];
                                            if (line < 0 || low < 0 || line >= Width || low >= Height)
                                                continue;
                                            if (panel[line, low].BorderStyle!= BorderStyle.Fixed3D && !landmines[line, low] && mark[line, low] != 1)
                                            {
                                                panel[line, low].BorderStyle = BorderStyle.Fixed3D;
                                                panel[line, low].BackColor = SystemColors.ControlLight;
                                                panel[line, low].BackgroundImage = null;
                                                panel[line, low].Size = new Size(20, 20);
                                                if (Convert.ToInt32(label[line, low].Text) != 0)
                                                {
                                                    #region label的数字变色
                                                    switch (Convert.ToInt32(label[line, low].Text))//label的数字
                                                    {
                                                        //1 RoyalBlue, 2 Green, 3 (192,0,0), 4 DarkBlue 5 Maroon
                                                        case 1:
                                                            label[line, low].ForeColor = Color.RoyalBlue;
                                                            break;
                                                        case 2:
                                                            label[line, low].ForeColor = Color.Green;
                                                            break;
                                                        case 3:
                                                            label[line, low].ForeColor = Color.Red;
                                                            break;
                                                        case 4:
                                                            label[line, low].ForeColor = Color.DarkBlue;
                                                            break;
                                                        case 5:
                                                            label[line, low].ForeColor = Color.Maroon;
                                                            break;
                                                    }
                                                    #endregion
                                                    label[line, low].Visible = true;
                                                    nAreaCount--;
                                                }
                                            }
                                        }*/
                    
                }
            }
            
            #endregion

            #region 胜利的条件
            if (nlandmines == nAreaCount)
            {
                Congratulation();
            }
            #endregion
        }
        private void Congratulation()
        {
            FatherForm.timer1.Enabled = false;
            游戏胜利 cg = new 游戏胜利(FatherForm);
            cg.ShowDialog();
            if (cg.DialogResult == DialogResult.OK)
                ReStart(0);
            FatherForm.timer1.Enabled = true;
        }
        private void mouseUp(object sender, MouseEventArgs e)
        {
            DownTime = 0;
        }
        #endregion

        #region 鼠标进入、离开控件区域
        private bool bmark = false;
        #region 鼠标进入控件区域
        private void mouseEnter(object sender, EventArgs e)
        {
            Panel p1 = (Panel)sender;
            if (p1.BackColor == Color.Aqua)
            {
                return;
            }
            int a = Convert.ToInt32(p1.Name);
            int i = a / 1000;
            int j = a % 1000;
            if (p1.BorderStyle == BorderStyle.Fixed3D || mark[i, j]!=0)
            {
                bmark = true;
            }
            else if (p1.BorderStyle != BorderStyle.Fixed3D && mark[i, j] == 0)
            {
                p1.BackColor = Color.Aqua;
            }
        }
        #endregion

        #region 鼠标离开控件区域
        private void mouseLeave(object sender, EventArgs e)
        {
            Panel p1 = (Panel)sender;
            int a = Convert.ToInt32(p1.Name);
            int i = a / 1000;
            int j = a % 1000;
            if (!bmark && p1.BorderStyle!= BorderStyle.Fixed3D && mark[i,j]==0)
            {
                p1.BackColor = Color.Navy;
            }
            bmark = false;
        }
        #endregion

        #endregion

        #region 扩张空白区域 Expand
        //
        //扩张空白区域
        //
        public void Expand(int i, int j)
        {
            #region 越界
            if (i < 0 || j < 0 || i >= Width || j >= Height)
            {
                return ;
            }
            #endregion

            if (panel[i, j].BorderStyle == BorderStyle.Fixed3D || landmines[i, j]/*是雷*/ || mark[i,j]!=0)
            {
                return;
            }
            //   改变 panel 的形状为 3D && 颜色改为 controlsLight
            panel[i, j].BorderStyle = BorderStyle.Fixed3D;
            panel[i, j].Size = new Size(20, 20);
            panel[i, j].BackColor = SystemColors.ControlLight;
            nAreaCount--;
            if (Convert.ToInt32(label[i, j].Text) != 0) //该位置的label不为0
            {
                label[i, j].Visible = true;//显示label
                switch (Convert.ToInt32(label[i, j].Text))//label的数字
                {
                    //1 RoyalBlue, 2 Green, 3 (192,0,0), 4 DarkBlue 5 Maroon
                    case 1:
                        label[i, j].ForeColor = Color.RoyalBlue;
                        break;
                    case 2:
                        label[i, j].ForeColor = Color.Green;
                        break;
                    case 3:
                        label[i, j].ForeColor = Color.Red;
                        break;
                    case 4:
                        label[i, j].ForeColor = Color.DarkBlue;
                        break;
                    case 5:
                        label[i, j].ForeColor = Color.Maroon;
                        break;
                }
            }
            else
            {
                //
                //向附近八个方向扩充
                //
                Expand(i - 1, j - 1);
                Expand(i - 1, j + 1);
                Expand(i + 1, j - 1);
                Expand(i + 1, j + 1);
                Expand(i + 1, j);
                Expand(i - 1, j);
                Expand(i, j - 1);
                Expand(i, j + 1);
            }
        }
        #endregion

        #region 清除控件
        //清除 panel 与 label 控件
        public void Dispose()
        {
            for (int i=0;i<Width;i++)
            {
                for (int j=0;j<Height;j++)
                {
                    FatherForm.panel2.Dispose();
                    FatherForm.panel2 = new Panel();
                    FatherForm.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
                    FatherForm.panel2.BackColor = SystemColors.ControlDark;
                    FatherForm.panel2.Location = new System.Drawing.Point(33, 46);
                    FatherForm.panel2.Name = "panel2";
                    FatherForm.panel2.Size = new System.Drawing.Size(162, 162);
                    FatherForm.panel2.TabIndex = 0;
                    FatherForm.Controls.Add(FatherForm.panel2);
                }
            }
        }
        #endregion
    }
}
