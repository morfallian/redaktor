using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Forms;
using Microsoft.Speech.Recognition;
using Microsoft.Speech.Recognition.SrgsGrammar;



namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        string str = "";
        static RichTextBox box;
        static TextBox tbox;

        Event_Methods ev_meth = new Event_Methods();
        public Form1()
        {
            InitializeComponent();
        }

        private void OpenToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            if (openFileDialog1.ShowDialog() == DialogResult.Cancel)
                return;

            string filename = openFileDialog1.FileName;

            string fileText = System.IO.File.ReadAllText(filename);
            richTextBox1.Text = fileText;
            Text = filename;
            str = fileText;
        }
        private void SaveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ev_meth.Save_Txt(richTextBox1);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (richTextBox1.Text == str)
                e.Cancel = false;
            else
                switch (MessageBox.Show(this, "Произошли изменения в файле, закрыть без сохранения?", "Closing", MessageBoxButtons.YesNo))
                {
                    case DialogResult.No:
                        e.Cancel = true;
                        ev_meth.Save_Txt(richTextBox1);
                        str = richTextBox1.Text;
                        break;
                    default:
                        break;
                }
        }
        private void Form1_Shown(object sender, EventArgs e)
        {
            box = richTextBox1;
            tbox = textBox1;

            SpeechRecognition speechRecog = new SpeechRecognition(box, tbox);

            speechRecog.SpechRecog();
        }
//Жмем кнопку "Найти"
        private void button2_Click(object sender, EventArgs e)
        {
            ev_meth.FindText(textBox1.Text, box, tbox);
        }
//запускаем код
        private void button3_Click(object sender, EventArgs e)
        {
            ev_meth.run_code(richTextBox1.Text);
        }
    }
}

