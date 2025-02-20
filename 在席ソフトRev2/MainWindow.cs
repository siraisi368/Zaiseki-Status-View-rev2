using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Text;
using System.Windows.Forms;
using System.Collections.Generic;

namespace 在席ソフトRev2
{
    public partial class MainWindow : Form
    {

        private SubWindow subWindow;

        public MainWindow()
        {
            InitializeComponent();
        }

        public List<PresetData> presets = new List<PresetData>();
        private preset pre = new preset();
        private string iconpath;
        private bool isShowSubWindow = false;

        /// <summary>
        /// 画面を描画します。
        /// </summary>
        /// <param name="data">PresetData.ViewDatas形式</param>
        /// <returns></returns>
        public Image WriteInformationToDisp(ViewDatas data)
        {
            Font StatusFont = new Font("M PLUS 1 SemiBold", 45); //状態文字
            Font TitleFont = new Font("M PLUS 1 SemiBold", 16); //配信者・地
            Font ChFont = new Font("M PLUS 1 SemiBold", 15); //配信者・地
            Font DataFont = new Font("M PLUS 1 SemiBold", 33); //配信者名・地名

            SolidBrush StatusFontColor = new SolidBrush(data.stateColors.stateForeColor);
            SolidBrush TitleFontColor = new SolidBrush(Color.FromArgb(51, 51, 51));
            SolidBrush ChFontColor = new SolidBrush(Color.FromArgb(254,254,254));

            int memoStartPixels = 65;
            int nameStartPixels = 65;
            
            if (data.iconDatas.prefectureIcon > 0)
            {
                memoStartPixels += 50;
            }

            if (data.iconDatas.isViewAuthorIcon)
            {
                nameStartPixels += 50;
            }
            
            Bitmap canvas = new Bitmap(540, 172);
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
                using (SolidBrush b = new SolidBrush(data.stateColors.stateBackColor)) //状態背景
                {
                    g.FillRectangle(b, 0, 20, 65, 150);
                }

                //文字
                using (StringFormat sf = new StringFormat())
                {
                    sf.FormatFlags = StringFormatFlags.DirectionVertical;
                    g.DrawString(data.authorState, StatusFont, StatusFontColor,-20, 18, sf); //状態文字
                }
                using (StringFormat sf = new StringFormat())
                {
                    sf.FormatFlags = StringFormatFlags.FitBlackBox;
                    sf.Alignment = StringAlignment.Center;
                    g.DrawString(data.castStaName, ChFont, ChFontColor,(RectangleF)rectangle,sf);
                }

                //アイコン
                if(data.iconDatas.isViewAuthorIcon && data.iconDatas.authorIconPath != null)
                {
                    try
                    {
                        Image icon = Image.FromFile(data.iconDatas.authorIconPath);
                        g.DrawImage(icon, 73, 48, 40, 40);
                    }
                    catch {
                        MessageBox.Show("アイコンの画像が見つかりませんでした。\r\n(画像を移動した？)", "エラー",MessageBoxButtons.OK,MessageBoxIcon.Information);
                    }
                }

                if (data.iconDatas.prefectureIcon > 0)
                {
                    Image icon = MakePrefImage.MakePrefImageFromGeoJson("./lib/prefectures.geojson", data.iconDatas.prefectureIcon);
                    g.DrawImage(icon, 73, 120, 45,40);
                }

                g.DrawString(data.isRecorder ? "記録者" : "配信者", TitleFont, TitleFontColor, 65, 17);
                g.DrawString(data.isRecorder ? "記録地" : "配信地", TitleFont, TitleFontColor, 65, 89);
                g.DrawString(data.authorName, DataFont, TitleFontColor, nameStartPixels, 32);
                g.DrawString(data.authorMemo, DataFont, TitleFontColor, memoStartPixels, 104);
            }
            return canvas;
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            this.Location = Properties.Settings.Default.mainwindow;

            this.MinimumSize = this.Size;
            this.MaximumSize = this.Size;
            comboBox1.SelectedIndex = 0;
            preset preset = new preset();
            lastdata lastdata = new lastdata();
            presets = preset.load();
            pictureBox1.Image = WriteInformationToDisp(lastdata.load());
            ReDrawList();
            dataToTextbox(new PresetData() { viewDatas = lastdata.load() });
            
            if(Properties.Settings.Default.is_subwindow)
            {
                checkBox2.Checked = true;
                subWindow = new SubWindow();
                subWindow.FormClosed += SubWindow_FormClosed;
                subWindow.Load += SubWindow_FormLoad;
                subWindow.Show();
                isShowSubWindow = true;
            }
        }

        private void ReDrawList()
        {
            listView1.Items.Clear();
            foreach(PresetData pr in presets)
            {
                string[] values = { pr.presetName };
                ListViewItem item = new ListViewItem(values);
                item.ToolTipText = pr.presetDescription;
                listView1.Items.Add(item);
            }
        }

        private Color? moziColor;
        private Color? haikeiColor;
        private void button1_Click(object sender, EventArgs e)
        {
            ViewDatas viewDatas = new ViewDatas()
            {
                authorName = textBox1.Text,
                authorMemo = textBox2.Text,
                castStaName = textBox3.Text,
                authorState = textBox4.Text,
                isRecorder = radioButton2.Checked,
                stateColors = new StateColors()
                {
                    stateForeColor = moziColor.Value,
                    stateBackColor = haikeiColor.Value,
                },
                iconDatas = new IconDatas()
                {
                    prefectureIcon = comboBox1.SelectedIndex,
                    isViewAuthorIcon = checkBox1.Checked,
                    authorIconPath = iconpath
                }
            };

            if (isShowSubWindow)
            {
                subWindow.SetImage(WriteInformationToDisp(viewDatas));
            }
            else
            {
                pictureBox1.Image = WriteInformationToDisp(viewDatas);
            }
            lastdata lastdata = new lastdata();
            lastdata.save(viewDatas);
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

        private void dataToTextbox(PresetData preset)
        {
            if (preset.viewDatas != null)
            {
                textBox1.Text = preset.viewDatas.authorName;
                textBox2.Text = preset.viewDatas.authorMemo;
                textBox3.Text = preset.viewDatas.castStaName;
                textBox4.Text = preset.viewDatas.authorState;
                radioButton2.Checked = preset.viewDatas.isRecorder;

                if (preset.viewDatas.stateColors != null)
                {
                    moziColor = preset.viewDatas.stateColors.stateForeColor;
                    haikeiColor = preset.viewDatas.stateColors.stateBackColor;
                }

                if(preset.viewDatas.iconDatas != null)
                {
                    comboBox1.SelectedIndex = preset.viewDatas.iconDatas.prefectureIcon;
                    checkBox1.Checked = preset.viewDatas.iconDatas.isViewAuthorIcon;
                    iconpath = preset.viewDatas.iconDatas.authorIconPath;
                }
            }
        }

        private void この表示を画像として保存ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Filter = "GIF|*.gif|JPEG|*.jpeg;*.jpg|PNG|*.png";
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

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            button4.Enabled = checkBox1.Checked;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "PNG|*.png|JPEG|*.jpeg;*.jpg|GIF|*.gif";
            openFileDialog1.ShowDialog();
            iconpath = openFileDialog1.FileName;
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Program.editWindowState = editWindowState.NewData;
            PresetSettings form2 = new PresetSettings();
            if (form2.ShowDialog() == DialogResult.OK)
            {
                presets = pre.load();
                ReDrawList();
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if(listView1.SelectedItems.Count > 0)
            {
                int selindex = 0;
                selindex = listView1.SelectedItems[0].Index;

                if (isShowSubWindow)
                {
                    subWindow.SetImage(WriteInformationToDisp(presets[selindex].viewDatas));
                }
                else
                {
                    pictureBox1.Image = WriteInformationToDisp(presets[selindex].viewDatas);
                }

                lastdata lastdata = new lastdata();
                lastdata.save(presets[selindex].viewDatas);
            }
        }

        private void プリセットの新規作成ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Program.editWindowState = editWindowState.NewData;
            PresetSettings form2 = new PresetSettings();
            if (form2.ShowDialog() == DialogResult.OK)
            {
                presets = pre.load();
                ReDrawList();
            }
        }

        private void プリセットの編集ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                int selindex = 0;
                selindex = listView1.SelectedItems[0].Index;
                Program.editWindowState = editWindowState.Edit;
                Program.editPresetNum = selindex;
                PresetSettings form2 = new PresetSettings();
                if (form2.ShowDialog() == DialogResult.OK)
                {
                    presets = pre.load();
                    ReDrawList();
                }
            }
        }

        private void 複製して編集ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                int selindex = 0;
                selindex = listView1.SelectedItems[0].Index;
                dataToTextbox(presets[selindex]);
                Program.editWindowState = editWindowState.CloneEdit;
                Program.editPresetNum = selindex;
                PresetSettings form2 = new PresetSettings();
                if (form2.ShowDialog() == DialogResult.OK)
                {
                    presets = pre.load();
                    ReDrawList();
                }
            }
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                int selindex = 0;
                selindex = listView1.SelectedItems[0].Index;
                dataToTextbox(presets[selindex]);
            }
        }

        private void プリセットの削除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                int selindex = 0;
                selindex = listView1.SelectedItems[0].Index;
                dataToTextbox(presets[selindex]);
                if(MessageBox.Show("プリセットを削除しますか？\r\nこの作業は元に戻せません！", "プリセットの削除", MessageBoxButtons.YesNo,MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    presets.RemoveAt(selindex);
                    ReDrawList();
                    pre.save(presets);
                }
            }
        }

        private void 複製のみToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                int selindex = 0;
                selindex = listView1.SelectedItems[0].Index;
                dataToTextbox(presets[selindex]);
                presets.Add(presets[selindex]);
                ReDrawList();
                pre.save(presets);
            }
        }

        private void MainWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            Properties.Settings.Default.mainwindow = this.Location;

            if (isShowSubWindow)
            {
                subWindow.Close();
            }

            ViewDatas viewDatas = new ViewDatas()
            {
                authorName = textBox1.Text,
                authorMemo = textBox2.Text,
                castStaName = textBox3.Text,
                authorState = textBox4.Text,
                isRecorder = radioButton2.Checked,
                stateColors = new StateColors()
                {
                    stateForeColor = moziColor.Value,
                    stateBackColor = haikeiColor.Value,
                },
                iconDatas = new IconDatas()
                {
                    prefectureIcon = comboBox1.SelectedIndex,
                    isViewAuthorIcon = checkBox1.Checked,
                    authorIconPath = iconpath
                }
            };
            lastdata lastdata = new lastdata();
            lastdata.save(viewDatas);
            pre.save(presets);
            Properties.Settings.Default.Save();
        }

        private void SubWindow_FormClosed(object sender, FormClosedEventArgs e)
        {
            isShowSubWindow = false;

            ViewDatas viewDatas = new ViewDatas()
            {
                authorName = textBox1.Text,
                authorMemo = textBox2.Text,
                castStaName = textBox3.Text,
                authorState = textBox4.Text,
                isRecorder = radioButton2.Checked,
                stateColors = new StateColors()
                {
                    stateForeColor = moziColor.Value,
                    stateBackColor = haikeiColor.Value,
                },
                iconDatas = new IconDatas()
                {
                    prefectureIcon = comboBox1.SelectedIndex,
                    isViewAuthorIcon = checkBox1.Checked,
                    authorIconPath = iconpath
                }
            };

            pictureBox1.Image = WriteInformationToDisp(viewDatas);
            lastdata lastdata = new lastdata();
            lastdata.save(viewDatas);
        }

        private void SubWindow_FormLoad(object sender, EventArgs e)
        {
            isShowSubWindow = true;
            pictureBox1.Image = null;

            ViewDatas viewDatas = new ViewDatas()
            {
                authorName = textBox1.Text,
                authorMemo = textBox2.Text,
                castStaName = textBox3.Text,
                authorState = textBox4.Text,
                isRecorder = radioButton2.Checked,
                stateColors = new StateColors()
                {
                    stateForeColor = moziColor.Value,
                    stateBackColor = haikeiColor.Value,
                },
                iconDatas = new IconDatas()
                {
                    prefectureIcon = comboBox1.SelectedIndex,
                    isViewAuthorIcon = checkBox1.Checked,
                    authorIconPath = iconpath
                }
            };

            subWindow.SetImage(WriteInformationToDisp(viewDatas));
            lastdata lastdata = new lastdata();
            lastdata.save(viewDatas);
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if(!isShowSubWindow)
            {
                subWindow = new SubWindow();
                subWindow.FormClosed += SubWindow_FormClosed;
                subWindow.Load += SubWindow_FormLoad;
                subWindow.Show();
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.is_subwindow = checkBox2.Checked;
        }
    }
}
