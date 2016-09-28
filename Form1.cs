﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using System.Threading;
// *   This program is free software: you can redistribute it and/or modify
// *   it under the terms of the GNU General Public License as published by
// *   the Free Software Foundation, either version 3 of the License, or
// *   (at your option) any later version.
// *
// *   This program is distributed in the hope that it will be useful,
// *   but WITHOUT ANY WARRANTY; without even the implied warranty of
// *   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// *   GNU General Public License for more details.
// *
// *   You should have received a copy of the GNU General Public License
// *   along with this program.  If not, see <http://www.gnu.org/licenses/>.
// *
// * @author     Xianglong He
// * @copyright  Copyright (c) 2015 Xianglong He. (http://tec.hxlxz.com)
// * @license    http://www.gnu.org/licenses/     GPL v3
// * @version    1.0
// * @discribe   RuiRuiComment弹幕显示程序
// * 本软件作者是何相龙，使用GPL v3许可证进行授权。
namespace RuiRuiComment
{
    public partial class RuiRuiComment : Form
    {
        Dictionary<string, LabelClass> labels = new Dictionary<string, LabelClass>();
        string qunnum = "";
        string qname = "", qowner = "";
        string qno = "";

        Random randr = new Random((int)(DateTime.Now.Ticks + 246656752));
        Random randg = new Random((int)(DateTime.Now.Ticks + 735237307));
        Random randb = new Random((int)(DateTime.Now.Ticks + 300259300));
        Random randx = new Random((int)(DateTime.Now.Ticks + 864248988));
        public RuiRuiComment()
        {
            InitializeComponent();
        }

        private void BarrageComment_Load(object sender, EventArgs e)
        {
            this.Width = Screen.PrimaryScreen.Bounds.Width;
            this.Height = Screen.PrimaryScreen.Bounds.Height;
            while (qname.Equals(""))
                qname = Interaction.InputBox("请准确地输入群名", "请准确地输入群名", "", 100, 100);
            while (qowner.Equals(""))
                qowner = Interaction.InputBox("请准确地输入群主QQ号", "请准确地输入群主QQ号", "", 100, 100);
            while (qno.Equals(""))
                qno = Interaction.InputBox("请准确地输入群号", "请准确地输入群号", "", 100, 100);
            qunnum = qowner + qname;

            Label l = new Label();
            l.AutoSize = true;
            l.Text = "在群 " + qno + " 内发送“弹幕＆你想说的话”即可参与互动！";
            l.Location = new Point(Screen.PrimaryScreen.Bounds.Width / 2 - 470, Screen.PrimaryScreen.Bounds.Height - 100);
            Controls.Add(l);
            Label m = new Label();
            m.AutoSize = true;
            m.Text = "By何相龙 基于RuiRuiRobot 群137777833 https://github.com/hxl9654/RuiRuiComment";
            m.Font = new Font("SimSun", 15);
            m.Location = new Point(Screen.PrimaryScreen.Bounds.Width / 2 - 400, Screen.PrimaryScreen.Bounds.Height - 50);
            Controls.Add(m);
            timer1.Enabled = true;
            timer1.Start();
            timer2.Enabled = true;
            timer2.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            foreach (var labelClass in labels)
            {
                if (labelClass.Value == null || labelClass.Value.times == 0)
                    continue;
                if (labelClass.Value.label.Text == "")
                {
                    labelClass.Value.label.Location = new Point(Screen.PrimaryScreen.Bounds.Width, labelClass.Value.label.Location.Y);
                    continue;
                }
                labelClass.Value.label.Location = new Point(labelClass.Value.label.Location.X - (labelClass.Value.label.Width + Screen.PrimaryScreen.Bounds.Width) / 500, labelClass.Value.label.Location.Y);
                if (labelClass.Value.label.Location.X < -labelClass.Value.label.Width)
                {
                    //labelClass.Value.label.Location = new Point(Screen.PrimaryScreen.Bounds.Width, labelClass.Value.label.Location.Y);
                    labelClass.Value.label.Location = new Point(Screen.PrimaryScreen.Bounds.Width, randx.Next(1, (Screen.PrimaryScreen.Bounds.Height / 50) - 2) * 50);
                    labelClass.Value.times--;
                }
            }
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            string url = "https://ruirui.hxlxz.com/getcomment.php?qunnum=" + qunnum;
            string temp = SmartQQ.HTTP.HttpGet(url);
            string[] tmp = temp.Split('★');
            for (int i = 0; i < 10 && i < tmp.Length; i++)
            {
                if (tmp[i] == null)
                    break;
                if (!labels.ContainsKey(tmp[i]))
                {

                    Label label = new Label();
                    label.ForeColor = Color.FromArgb(randr.Next(255), randg.Next(255), randb.Next(255));
                    label.Location = new Point(Screen.PrimaryScreen.Bounds.Width, randx.Next(1, (Screen.PrimaryScreen.Bounds.Height / 50) - 2) * 50);
                    label.AutoSize = true;
                    label.Text = tmp[i];
                    LabelClass labelClass = new LabelClass();
                    labelClass.label = label;
                    labelClass.times = 5;
                    labels.Add(tmp[i], labelClass);
                    Controls.Add(labels[tmp[i]].label);
                    Thread.Sleep(100);
                }
            }
        }
        internal class LabelClass
        {
            public Label label;
            public int times;
        }
    }
}
