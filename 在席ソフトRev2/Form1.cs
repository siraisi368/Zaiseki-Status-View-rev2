using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 在席ソフトRev2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        //引数:色, 状態, 名前, メモ, 放送局名
        public void WriteInformationToDisp((Color?,Color?) stateColor,string status,string name,string memo,string channelName, string haishin, string basyo)
        {
            Font StatusFont = new Font("M PLUS 1 SemiBold", 45); //状態文字
            Font TitleFont = new Font("M PLUS 1 SemiBold", 16); //配信者・地
            Font ChFont = new Font("M PLUS 1 SemiBold", 15); //配信者・地
            Font DataFont = new Font("M PLUS 1 SemiBold", 33); //配信者名・地名

            SolidBrush StatusFontColor = new SolidBrush(stateColor.Item1.Value);
            SolidBrush TitleFontColor = new SolidBrush(Color.FromArgb(51, 51, 51));
            SolidBrush ChFontColor = new SolidBrush(Color.FromArgb(254,254,254));

            Bitmap canvas = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            using (Graphics g = Graphics.FromImage(canvas))
            {
                g.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
                Rectangle rectangle = new Rectangle(new Point(0, -5), new Size(540, 25));

                using (SolidBrush b = new SolidBrush(Color.FromArgb(254, 254, 254))) //背景
                {
                    g.FillRectangle(b, 0, 0, 540, 170);
                }
                using (SolidBrush b = new SolidBrush(Color.FromArgb(51,51,51))) //上棒,配信者・地
                {
                    g.FillRectangle(b, 0, 0, 540, 20);
                    g.FillRectangle(b, 70, 45, 464, 1);
                    g.FillRectangle(b, 70, 117, 464, 1);
                }
                using (SolidBrush b = new SolidBrush(stateColor.Item2.Value)) //状態背景
                {
                    g.FillRectangle(b, 0, 20, 65, 150);
                }

                //文字
                using (StringFormat sf = new StringFormat())
                {
                    sf.FormatFlags = StringFormatFlags.DirectionVertical;
                    g.DrawString(status.ToString(), StatusFont, StatusFontColor,-20, 18, sf); //状態文字
                }
                using (StringFormat sf = new StringFormat())
                {
                    sf.FormatFlags = StringFormatFlags.FitBlackBox;
                    sf.Alignment = StringAlignment.Center;
                    g.DrawString(channelName, ChFont, ChFontColor,(RectangleF)rectangle,sf);
                }
                g.DrawString(haishin, TitleFont, TitleFontColor, 65, 17);
                g.DrawString(basyo, TitleFont, TitleFontColor, 65, 89);
                g.DrawString(name, DataFont, TitleFontColor, 65, 32);
                g.DrawString(memo, DataFont, TitleFontColor, 65, 104);
            }
            pictureBox1.Image = canvas;
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            (moziColor,haikeiColor) = (Properties.Settings.Default.moziColor, Properties.Settings.Default.haikeiColor);
            string status = Properties.Settings.Default.status;
            string name = Properties.Settings.Default.name;
            string memo = Properties.Settings.Default.memo;
            string channelName = Properties.Settings.Default.channel;
            string haishin = Properties.Settings.Default.haishin;
            string basyo = Properties.Settings.Default.basyo;

            textBox1.Text = name;
            textBox2.Text = memo;
            textBox3.Text = channelName;
            textBox4.Text = status;

            radioButton1.Checked = Properties.Settings.Default.chbx1;
            radioButton2.Checked = Properties.Settings.Default.chbx2;

            WriteInformationToDisp((moziColor, haikeiColor), status, name, memo,channelName,haishin,basyo);
        }
        private Color? moziColor;
        private Color? haikeiColor;
        private void button1_Click(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                WriteInformationToDisp((moziColor, haikeiColor), textBox4.Text, textBox1.Text, textBox2.Text, textBox3.Text,"配信者", "配信地");
                save_Config((moziColor, haikeiColor), textBox4.Text, textBox1.Text, textBox2.Text, textBox3.Text, "配信者", "配信地");
            }
            else if (radioButton2.Checked)
            {
                WriteInformationToDisp((moziColor, haikeiColor), textBox4.Text, textBox1.Text, textBox2.Text, textBox3.Text, "記録者", "記録地");
                save_Config((moziColor, haikeiColor), textBox4.Text, textBox1.Text, textBox2.Text, textBox3.Text, "記録者", "記録地");
            }
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ColorDialog cd = new ColorDialog();
            if(cd.ShowDialog() == DialogResult.OK)
            {
                moziColor = cd.Color;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ColorDialog cd = new ColorDialog();
            if (cd.ShowDialog() == DialogResult.OK)
            {
                haikeiColor = cd.Color;
            }
        }

        private void save_Config ((Color?, Color?) stateColor, string status, string name, string memo, string channelName, string haishin, string basyo)
        {
            (Properties.Settings.Default.moziColor, Properties.Settings.Default.haikeiColor) = (stateColor.Item1.Value, stateColor.Item2.Value);
            Properties.Settings.Default.status = status;
            Properties.Settings.Default.name = name;
            Properties.Settings.Default.memo = memo;
            Properties.Settings.Default.channel = channelName;
            Properties.Settings.Default.haishin = haishin;
            Properties.Settings.Default.basyo = basyo;
            Properties.Settings.Default.chbx1 = radioButton1.Checked;
            Properties.Settings.Default.chbx2 = radioButton2.Checked;

            Properties.Settings.Default.Save();
        }

        private void この表示を画像として保存ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Filter = "GIF|*.gif|JPEG|*.jpeg|PNG|*.png";
            saveFileDialog1.ShowDialog();
        }

        private void saveFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            string extension = System.IO.Path.GetExtension(saveFileDialog1.FileName);
            switch (extension.ToUpper())
            {
                case ".GIF":
                    pictureBox1.Image.Save(saveFileDialog1.FileName, System.Drawing.Imaging.ImageFormat.Gif);
                    break;
                case ".JPEG":
                    pictureBox1.Image.Save(saveFileDialog1.FileName, System.Drawing.Imaging.ImageFormat.Jpeg);
                    break;
                case ".PNG":
                    pictureBox1.Image.Save(saveFileDialog1.FileName, System.Drawing.Imaging.ImageFormat.Png);
                    break;
            }
        }
    }
}
