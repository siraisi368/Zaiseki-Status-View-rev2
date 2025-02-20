using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace 在席ソフトRev2
{
    public partial class PresetSettings : Form
    {
        public PresetSettings()
        {
            InitializeComponent();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private List<PresetData> presetDatas = new List<PresetData>();
        private preset pre = new preset();
        private Color? moziColor = Color.Black;
        private Color? haikeiColor = Color.White;
        private string iconpath;
        
        
        private void button6_Click(object sender, EventArgs e)
        {
            ViewDatas viewDatas = new ViewDatas()
            {
                authorName = textBox6.Text,
                authorMemo = textBox5.Text,
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

            MainWindow mainWindow = new MainWindow();
            pictureBox1.Image = mainWindow.WriteInformationToDisp(viewDatas);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ColorDialog cd = new ColorDialog();
            if(moziColor != null)
            {
                cd.Color = moziColor.Value;
            }
            if (cd.ShowDialog() == DialogResult.OK)
            {
                moziColor = cd.Color;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ColorDialog cd = new ColorDialog();
            if (haikeiColor != null)
            {
                cd.Color = haikeiColor.Value;
            }
            if (cd.ShowDialog() == DialogResult.OK)
            {
                haikeiColor = cd.Color;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                PresetData preData = new PresetData()
                {
                    presetName = textBox1.Text,
                    presetDescription = textBox2.Text,
                    viewDatas = new ViewDatas()
                    {
                        authorName = textBox6.Text,
                        authorMemo = textBox5.Text,
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
                    }
                };

                switch (Program.editWindowState)
                {
                    case editWindowState.Edit:
                        presetDatas[Program.editPresetNum] = preData;
                        MessageBox.Show("プリセットを保存しました。", "プリセットの保存", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        break;
                    case editWindowState.NewData:
                        MessageBox.Show("プリセットを登録しました。", "プリセットの登録", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        presetDatas.Add(preData);
                        break;
                    case editWindowState.CloneEdit:
                        MessageBox.Show("プリセットを保存しました。", "プリセットの保存", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        presetDatas.Add(preData);
                        break;
                }
                pre.save(presetDatas);
                this.Close();
            }
            catch (Exception ex) {
                MessageBox.Show("プリセット登録に失敗しました。\r\nボックスに空欄がないか確認してください。", "プリセットの登録", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dataToTextbox(PresetData preset)
        {
            textBox1.Text = preset.presetName;
            textBox2.Text = preset.presetDescription;

            if (preset.viewDatas != null)
            {
                textBox6.Text = preset.viewDatas.authorName;
                textBox5.Text = preset.viewDatas.authorMemo;
                textBox3.Text = preset.viewDatas.castStaName;
                textBox4.Text = preset.viewDatas.authorState;
                radioButton2.Checked = preset.viewDatas.isRecorder;

                if (preset.viewDatas.stateColors != null)
                {
                    moziColor = preset.viewDatas.stateColors.stateForeColor;
                    haikeiColor = preset.viewDatas.stateColors.stateBackColor;
                }

                if (preset.viewDatas.iconDatas != null)
                {
                    comboBox1.SelectedIndex = preset.viewDatas.iconDatas.prefectureIcon;
                    checkBox1.Checked = preset.viewDatas.iconDatas.isViewAuthorIcon;
                    iconpath = preset.viewDatas.iconDatas.authorIconPath;
                }
            }
        }

        private void PresetSettings_Load(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = 0;
            presetDatas = pre.load();

            switch (Program.editWindowState)
            {
                case editWindowState.Edit:
                    this.Text = "プリセットの編集";
                    dataToTextbox(presetDatas[Program.editPresetNum]);
                    button1.Text = "保存";
                    comboBox2.Enabled = false;
                    break;
                case editWindowState.NewData:
                    this.Text = "プリセットの登録";
                    button1.Text = "登録";
                    comboBox2.Enabled = true;
                    break;
                case editWindowState.CloneEdit:
                    this.Text = "プリセットの複製";
                    dataToTextbox(presetDatas[Program.editPresetNum]);
                    comboBox2.Enabled = false;
                    break;
            }

            foreach(PresetData value in presetDatas)
            {
                comboBox2.Items.Add(value.presetName);
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            dataToTextbox(presetDatas[comboBox2.SelectedIndex]);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "PNG|*.png|JPEG|*.jpeg;*.jpg|GIF|*.gif";
            openFileDialog1.ShowDialog();
            iconpath = openFileDialog1.FileName;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            button4.Enabled = checkBox1.Checked;
        }
    }
}
