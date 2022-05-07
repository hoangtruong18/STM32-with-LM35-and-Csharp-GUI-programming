using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;
using System.IO;

namespace Demo2
{
    public partial class Main : Form
    {
        #region DECLARE VARIABLES

        int sampletime, k1, k2, blink1 = 0, blink2 = 0;
        string link;
        double DataInput1 = 23, DataInput2 = 25;
        bool state = false;
        bool enable = false;
        string[] BaudRateList = { "1200", "2400", "4800", "9600", "19200", "38400" };
        int sensor_index = 0;

        #endregion DECLARE VARIABLES

        public Main()
        {
            InitializeComponent();
            string[] ComPortList = SerialPort.GetPortNames();
            Ports.Items.AddRange(ComPortList);
            BaudRate.Items.AddRange(BaudRateList);
            timer1.Enabled = true;
            timer2.Enabled = true;


        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (state == false)
            {
                serialPort1.PortName = Ports.Text;
                if (BaudRate.Text == "Baud Rate")
                    MessageBox.Show("You have not selected BaudRate", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else
                    serialPort1.BaudRate = int.Parse(BaudRate.Text);

                if (serialPort1.PortName == "Ports")
                {
                    MessageBox.Show("You have not selected Port", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    serialPort1.Open();
                    serialPort1.Write("1");
                    Notification.Text = "Notification: successfully connected";
                    state = true;
                    Connect.Text = "Disconnect";
                }
            }
            else if (state == true)
            {
                serialPort1.Write("0");
                serialPort1.Close();
                Notification.Text = "Notification: disconnected";
                state = false;
                Connect.Text = "Connect";
            }
        }
        
        private async void serialPort1_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            sensor_index += 1;
            string Data = serialPort1.ReadLine();
            if (sensor_index == 1)
            {
                DataInput1 = double.Parse(Data);
                //Vref = 3V for real system and Vref = 5V for simulation on proteus
                DataInput1 = DataInput1 * 3 * 100 / 4095;
                await Task.Delay(100);
                temp1.Text = DataInput1.ToString("00.00");
            }
            else if (sensor_index == 2)
            {
                DataInput2 = double.Parse(Data);
                //Vref = 3V for real system and Vref = 5V for simulation on proteus
                DataInput2 = DataInput2 * 3 * 100 / 4095;
                await Task.Delay(100);
                temp2.Text = DataInput2.ToString("00.00");
                sensor_index = 0;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            temp1.Text = DataInput1.ToString("00.00");
            temp2.Text = DataInput2.ToString("00.00");
            if (Alarm1.Checked)
            {
                if (Convert.ToDouble(temp1.Text) > Convert.ToDouble(numericUpDown2.Value))
                {
                    light12.BackColor = Color.Green;
                }
                else if (Convert.ToDouble(temp1.Text) == Convert.ToDouble(numericUpDown2.Value))
                {
                    light12.BackColor = Color.Yellow;
                }    
                else
                {
                    //light2.BackColor = Color.Red;
                    k1 = 1;
                    timer_blink1.Enabled = true;
                }

                if (Convert.ToDouble(temp1.Text) < Convert.ToDouble(numericUpDown1.Value))
                {
                    light11.BackColor = Color.Green;
                }
                else if (Convert.ToDouble(temp1.Text) == Convert.ToDouble(numericUpDown1.Value))
                {
                    light11.BackColor = Color.Yellow;
                }
                else
                {
                    //light1.BackColor = Color.Red;
                    k1 = 2;
                    timer_blink1.Enabled = true;
                }
            }
            else
            {
                light11.BackColor = Color.White;
                light12.BackColor = Color.White;
            }

            if (Alarm2.Checked)
            {
                if (Convert.ToDouble(temp2.Text) > Convert.ToDouble(numericUpDown4.Value))
                {
                    light22.BackColor = Color.Green;
                }
                else if (Convert.ToDouble(temp2.Text) == Convert.ToDouble(numericUpDown4.Value))
                {
                    light22.BackColor = Color.Yellow;
                }
                else
                {
                    //light2.BackColor = Color.Red;
                    k2 = 1;
                    timer_blink2.Enabled = true;
                }

                if (Convert.ToDouble(temp2.Text) < Convert.ToDouble(numericUpDown3.Value))
                {
                    light21.BackColor = Color.Green;
                }
                else if (Convert.ToDouble(temp2.Text) == Convert.ToDouble(numericUpDown3.Value))
                {
                    light21.BackColor = Color.Yellow;
                }
                else
                {
                    //light1.BackColor = Color.Red;
                    k2 = 2;
                    timer_blink2.Enabled = true;
                }
            }
            else
            {
                light21.BackColor = Color.White;
                light22.BackColor = Color.White;
            }

            if (radioButton1.Checked)
            {
                sampletime = 5000;
                enable = true;
            }
            else if (radioButton2.Checked)
            {
                sampletime = 10000;
                enable = true;
            }
            else if (radioButton3.Checked)
            {
                sampletime = 30000;
                enable = true;
            }
            else if (radioButton4.Checked)
            {
                sampletime = 60000;
                enable = true;
            }
            else
            {
                sampletime = 0;
                enable = false;
            }

            if (sampletime != 0)
            {
                timer2.Interval = (int)sampletime;
            }
        }

        private async void timer2_Tick(object sender, EventArgs e)
        {

            //RECORD RECEIVED DATA
            if (state && checkBox1.Checked && enable && path.Text != "")
            {
                light_data.BackColor = Color.Green;
                link = @path.Text.ToString() + @"\DataRecord.txt'";
                System.IO.FileStream DataRecord = new System.IO.FileStream(link, System.IO.FileMode.Append, System.IO.FileAccess.Write, System.IO.FileShare.None);
                StreamWriter sw = new StreamWriter(DataRecord);
                sw.Write("Time: ");
                sw.Write(DateTime.Now.ToString());

                sw.Write(" Temperature Sensor 1: ");
                sw.Write(DataInput1.ToString("00.00"));
                sw.Write(" ");
                sw.Write("[°C]");

                sw.Write(" Temperature Sensor 2: ");
                sw.Write(DataInput2.ToString("00.00"));
                sw.Write(" ");
                sw.WriteLine("[°C]");
                sw.Flush();

                sw.Close();
                DataRecord.Close();
                await Task.Delay(200);
                light_data.BackColor = Color.White;
            }
            else
                light_data.BackColor = Color.White;
        }

        private void Form1_Closing(object sender, FormClosingEventArgs e)
        {
            DialogResult dr;
            dr = MessageBox.Show("Do you want to close the program? All processes would be stopped!", "Notification", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dr == DialogResult.OK)
            {
                serialPort1.Write("0");
                e.Cancel = false;
            }
            else
                e.Cancel = true;
        }

        private void Brower_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog link_browse = new FolderBrowserDialog();
            link_browse.Description = "Select where you want to save temperature record";
            link_browse.ShowNewFolderButton = true;
            if (link_browse.ShowDialog() == DialogResult.OK)
            {
                path.Text = link_browse.SelectedPath.ToString();
            }
        }

        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://docs.google.com/document/d/1ijwW41G3-eAlaB3opVIapp2kdYp8rihJ7g7Whi5zAFk/edit?usp=sharing");
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Window1 form = new Window1();
            form.Show();
        }

        private void timer_blink2_Tick(object sender, EventArgs e)
        {
            if (k2 == 1)
            {
                if (blink2 == 0)
                {
                    light22.BackColor = Color.Red;
                    blink2 = 1;
                }
                else
                {
                    light22.BackColor = Color.White;
                    blink2 = 0;
                }
                k2 = 0;
                timer_blink2.Enabled = false;
            }

            if (k2 == 2)
            {
                if (blink2 == 0)
                {
                    light21.BackColor = Color.Red;
                    blink2 = 1;
                }
                else
                {
                    light21.BackColor = Color.White;
                    blink2 = 0;
                }
                k2 = 0;
                timer_blink2.Enabled = false;
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                MessageBox.Show("Make sure that you filled the path, selected the sample time and checked \"log data to files\"", "Do you want to save data to the record?", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
            }
        }

        private void timer3_Tick(object sender, EventArgs e)
        {
            if (k1 == 1)
            {
                if (blink1 == 0)
                {
                    light12.BackColor = Color.Red;
                    blink1 = 1;
                }
                else
                {
                    light12.BackColor = Color.White;
                    blink1 = 0;
                }
                k1 = 0;
                timer_blink1.Enabled = false;
            }

            if (k1 == 2)
            {
                if (blink1 == 0)
                {
                    light11.BackColor = Color.Red;
                    blink1 = 1;
                }
                else
                {
                    light11.BackColor = Color.White;
                    blink1 = 0;
                }
                k1 = 0;
                timer_blink1.Enabled = false;
            }
        }
    }
        
}
