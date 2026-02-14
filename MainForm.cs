using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;


namespace RepoHariTool
{
    public partial class Main : Form
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool AllocConsole();
        private CancellationTokenSource cts; //処理をキャンセルするやつ？
        private List<int> Report;
        private PIDPrm Param;
        public Main()
        {
            InitializeComponent();
            //AllocConsole();

            Report = new List<int>();
            cts?.Cancel();
            cts = new CancellationTokenSource();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Report.Add(0);
            UpdateReportBox();
            if (!GetPrm()) return;
            if (checkBox4.Checked)
            {
                SeedSearcher SeedSearch = new SeedSearcher(Param, Report);
                SeedSearch.GenerateSeeds(cts.Token, dataGridView1);
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            Report.Add(1);
            UpdateReportBox();
            if (!GetPrm()) return;
            if (checkBox4.Checked)
            {
                SeedSearcher SeedSearch = new SeedSearcher(Param, Report);
                SeedSearch.GenerateSeeds(cts.Token, dataGridView1);
            }
        }
        private void button3_Click(object sender, EventArgs e)
        {
            Report.Add(2);
            UpdateReportBox();
            if (!GetPrm()) return;
            if (checkBox4.Checked)
            {
                SeedSearcher SeedSearch = new SeedSearcher(Param, Report);
                SeedSearch.GenerateSeeds(cts.Token, dataGridView1);
            }
        }
        private void button4_Click(object sender, EventArgs e)
        {
            Report.Add(3);
            UpdateReportBox();
            if (!GetPrm()) return;
            if (checkBox4.Checked)
            {
                SeedSearcher SeedSearch = new SeedSearcher(Param, Report);
                SeedSearch.GenerateSeeds(cts.Token, dataGridView1);
            }
        }
        private void button5_Click(object sender, EventArgs e)
        {
            Report.Add(4);
            UpdateReportBox();
            if (!GetPrm()) return;
            if (checkBox4.Checked)
            {
                SeedSearcher SeedSearch = new SeedSearcher(Param, Report);
                SeedSearch.GenerateSeeds(cts.Token, dataGridView1);
            }
        }
        private void button6_Click(object sender, EventArgs e)
        {
            Report.Add(5);
            UpdateReportBox();
            if (!GetPrm()) return;
            if (checkBox4.Checked)
            {
                SeedSearcher SeedSearch = new SeedSearcher(Param, Report);
                SeedSearch.GenerateSeeds(cts.Token, dataGridView1);
            }
        }
        private void button7_Click(object sender, EventArgs e)
        {
            Report.Add(6);
            UpdateReportBox();
            if (!GetPrm()) return;
            if (checkBox4.Checked)
            {
                SeedSearcher SeedSearch = new SeedSearcher(Param, Report);
                SeedSearch.GenerateSeeds(cts.Token, dataGridView1);
            }
        }
        private void button8_Click(object sender, EventArgs e)
        {
            Report.Add(7);
            UpdateReportBox();
            if (!GetPrm()) return;
            if (checkBox4.Checked)
            {
                SeedSearcher SeedSearch = new SeedSearcher(Param, Report);
                SeedSearch.GenerateSeeds(cts.Token, dataGridView1);
            }
        }
        private void button12_Click(object sender, EventArgs e)
        {
            Report.Add(8);
            UpdateReportBox();
            if (!GetPrm()) return;
        }

        private void button9_Click(object sender, EventArgs e)//一つ戻る
        {
            if (Report.Count > 0)
            {
                Report.RemoveAt(Report.Count - 1);
                UpdateReportBox();
            }
        }

        private void button10_Click(object sender, EventArgs e)//クリア

        {
            if (Report.Count > 0)
            {
                Report.Clear();
                UpdateReportBox();
            }
        }
        private void button11_Click(object sender, EventArgs e)
        {
            if (Report.Count > 0)
            {
                if (!checkBox4.Checked)
                {
                    if (!GetPrm()) return;
                    SeedSearcher SeedSearch = new SeedSearcher(Param, Report);
                    SeedSearch.GenerateSeeds(cts.Token, dataGridView1);
                }
            }
        }
        private void UpdateReportBox()
        {
            textBox7.Clear();

            for (int i = 0; i < Report.Count; i++)
            {
                int Val = Report[i];

                if (Report[i] == 0) textBox7.AppendText("↑");
                else if (Report[i] == 1) textBox7.AppendText("↗");
                else if (Report[i] == 2) textBox7.AppendText("→");
                else if (Report[i] == 3) textBox7.AppendText("↘");
                else if (Report[i] == 4) textBox7.AppendText("↓");
                else if (Report[i] == 5) textBox7.AppendText("↙");
                else if (Report[i] == 6) textBox7.AppendText("←");
                else if (Report[i] == 7) textBox7.AppendText("↖");
                else if (Report[i] == 8) textBox7.AppendText("?");
                textBox7.AppendText(", ");
            }
        }

        private bool GetPrm()
        {
            if (!GetDateTime(
                numericUpDown1, numericUpDown2, numericUpDown3,
                numericUpDown4, numericUpDown5, numericUpDown6,
                out DateTime DateTime))
            {
                MessageBox.Show("検索範囲が不正です！");
                return false;
            }

            //-------------------------
            //Values[0] = Nazo1
            //Values[1] = Nazo2
            //Values[2] = Nazo3
            //Values[3] = Vount
            //Values[4] = InTimer0
            //Values[5] = EnTimer0 
            //Values[6] = Mac 上位
            //Values[7] = Mac 下位
            //Values[8] = GxFrame
            //Values[9] = Frame
            //Values[10] = Key 入力なし
            //Values[11] = Prm1
            //Values[12] = Prm2
            //Values[13] = Prm3
            //Values[14] = Version // 01:JPBW 23:JPBW2
            //Values[15] = 消費数
            //Values[16] = おもいでリンク //0:なし 1:あり
            //-------------------------

            var param = new PIDPrm();

            //nazo値
            if (comboBox1.SelectedIndex == 0) //JPB
            {
                param.Values[0] = 0x02215F10;
                param.Values[1] = 0x02215F10 + 0xFC;
                param.Values[2] = 0x02215F10 + 0xFC;
                param.Values[3] = 0x60;
                param.Values[4] = 0xC79;
                param.Values[5] = 0xC7A;
                param.Values[14] = 0;
            }
            else if (comboBox1.SelectedIndex == 1) //JPW
            {
                param.Values[0] = 0x02215F30;
                param.Values[1] = 0x02215F30 + 0xFC;
                param.Values[2] = 0x02215F30 + 0xFC;
                param.Values[3] = 0x5F;
                param.Values[4] = 0xC67;
                param.Values[5] = 0xC69;
                param.Values[14] = 1;
            }
            else if (comboBox1.SelectedIndex == 2) //JPB2
            {
                param.Values[0] = 0x0209A8DC;
                param.Values[1] = 0x02039AC9;
                param.Values[2] = 0x021FF9B0;
                param.Values[3] = 0x82;
                param.Values[4] = 0x1102;
                param.Values[5] = 0x1108;
                param.Values[14] = 2;
            }
            else if (comboBox1.SelectedIndex == 3) //JPW2
            {
                param.Values[0] = 0x0209A8FC;
                param.Values[1] = 0x02039AF5;
                param.Values[2] = 0x021FF9D0;
                param.Values[3] = 0x82;
                param.Values[4] = 0x10F5;
                param.Values[5] = 0x10FB;
                param.Values[14] = 3;
            }

            byte M1 = Convert.ToByte(textBox5.Text, 16);
            byte M2 = Convert.ToByte(textBox6.Text, 16);

            param.Values[6] = (uint)((M1 << 8) + M2);

            byte M3 = Convert.ToByte(textBox1.Text, 16);
            byte M4 = Convert.ToByte(textBox2.Text, 16);
            byte M5 = Convert.ToByte(textBox3.Text, 16);
            byte M6 = Convert.ToByte(textBox4.Text, 16);

            param.Values[7] = (uint)((M3 << 24) | (M4 << 16) | (M5 << 8) | M6);

            param.Values[8] = 0x06000000;
            param.Values[9] = 0x8;
            if (comboBox2.SelectedIndex == 2)
            {
                param.Values[9] = 0x6;
            }

            param.Values[10] = 0x00002FFF;
            param.Values[11] = 0x80000000;
            param.Values[12] = 0x00000000;
            param.Values[13] = 0x000001A0;

            param.Increment = TimeSpan.FromSeconds(1);

            int DateTimeRange = 0;

            if (checkBox1.Checked) DateTimeRange = 5;
            else if (checkBox2.Checked) DateTimeRange = 10;
            else if (checkBox3.Checked) DateTimeRange = 15;

            DateTime min = new DateTime(2000, 1, 1, 0, 0, 0);
            DateTime max = new DateTime(2099, 12, 31, 23, 59, 59);

            param.InDt = DateTime.AddSeconds(-DateTimeRange);
            param.EnDt = DateTime.AddSeconds(+DateTimeRange);

            if (param.InDt < min) param.InDt = min;
            if (param.EnDt > max) param.EnDt = max;

            param.Values[15] = (uint)numericUpDown8.Value;

            param.Values[16] = 0;
            if (checkBox5.Checked) param.Values[16] = 1;

            Param = param;

            return true;
        }

        private bool GetDateTime(
            NumericUpDown year, NumericUpDown month, NumericUpDown day,
            NumericUpDown hour, NumericUpDown minute, NumericUpDown second,
            out DateTime result)
        {
            result = default;

            try
            {
                int y = (int)year.Value;
                int m = (int)month.Value;
                int d = (int)day.Value;
                int h = (int)hour.Value;
                int min = (int)minute.Value;
                int s = (int)second.Value;

                result = new DateTime(y, m, d, h, min, s);
                return true;
            }
            catch
            {
                // 不正
                return false;
            }
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == 0)
            {
                textBox8.Text = "C79";
                textBox9.Text = "C7A";
            }
            else if (comboBox1.SelectedIndex == 1)
            {
                textBox8.Text = "C67";
                textBox9.Text = "C69";
            }
            else if (comboBox1.SelectedIndex == 2)
            {
                textBox8.Text = "1102";
                textBox9.Text = "1108";
            }
            else if (comboBox1.SelectedIndex == 3)
            {
                textBox8.Text = "10F5";
                textBox9.Text = "10FB";
            }
        }
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                checkBox2.Checked = false;
                checkBox3.Checked = false;
            }
        }
        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked)
            {
                checkBox1.Checked = false;
                checkBox3.Checked = false;
            }
        }
        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox3.Checked)
            {
                checkBox1.Checked = false;
                checkBox2.Checked = false;
            }
        }

        public class PIDPrm
        {
            public List<uint> Values { get; set; } = Enumerable.Repeat(0u, 17).ToList();
            public DateTime InDt { get; set; }
            public DateTime EnDt { get; set; }
            public TimeSpan Increment { get; set; }
        }

    }
}
