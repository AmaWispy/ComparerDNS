using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Threading;

namespace ComparerDNS
{
    public partial class Form1 : Form
    {
        public const string t = ".txt";
        public string[] file2Text = new string[127];
        public string[] file1Text = new string[127];
        public StreamReader[] file = new StreamReader[3];
        public int countFile = 0;
        public Thread transferToStringThr;
        public delegate void TxtMaker(string getText);
        TxtMaker txtMaker;
        public bool stopThr = false;

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "" && textBox2.Text != "" && textBox1.Text != "Имя файла для анализа 1" && textBox2.Text != "Имя файла для анализа 2")
            {
                if (File.Exists(textBox1.Text + t) && File.Exists(textBox2.Text + t))
                {
                    txtMaker = new TxtMaker(TxtMakerMethod);
                    countFile = 0;
                    file[1] = new StreamReader(textBox1.Text + t);
                    file[2] = new StreamReader(textBox2.Text + t);
                    countFile++;
                    transferToStringThr = new Thread(new ThreadStart(TransferToString));
                    transferToStringThr.Start();
                    Thread thrWaiterThr = new Thread(new ThreadStart(ThrWaiter));
                    thrWaiterThr.Start();
                }
                else { MessageBox.Show("Указанных вами файлов не существует в папке с программой","System.Error"); }
            }
            else { MessageBox.Show("Введите имя файлов без расширения", "System.Error"); }
        }

        void TransferToString()
        {
            int i = 0;
            if (countFile == 1)
            {
                while (!file[countFile].EndOfStream)
                {
                    file1Text[i] = file[countFile].ReadLine();
                    Invoke(txtMaker, file1Text[i]);
                    //textBox3.Text = (fileText[saverCountFile][i] + Environment.NewLine);
                    i++;
                }
            }
            else
            {
                while (!file[countFile].EndOfStream)
                {
                    file2Text[i] = file[countFile].ReadLine();
                    Invoke(txtMaker, file2Text[i]);
                    //textBox3.Text = (fileText[saverCountFile][i] + Environment.NewLine);
                    i++;
                }
            }
            file[countFile].Close();
            MessageBox.Show("Первый кончился");
        }

        void ThrWaiter()
        {
            while (transferToStringThr.IsAlive && !stopThr)
            {
                stopThr = true;
                countFile++;
                transferToStringThr = new Thread(new ThreadStart(TransferToString));
                transferToStringThr.Start();
            }
        }
        void TxtMakerMethod(string getText)
        {
            textBox3.AppendText(getText + Environment.NewLine);
        }

        private void textBox1_MouseDown(object sender, MouseEventArgs e)
        {
            textBox1.Text = null;
        }

        private void textBox2_MouseDown(object sender, MouseEventArgs e)
        {
            textBox2.Text = null;
        }
    }
}
