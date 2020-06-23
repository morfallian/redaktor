﻿using System;
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
    class SpeechRecognition
    {
        private RichTextBox box;
        private TextBox tbox;
        Event_Methods ev_meth = new Event_Methods();
        public SpeechRecognition(RichTextBox _box, TextBox _tbox)
        {
            box = _box;
            tbox = _tbox;
        }
        public void SpechRecog()
        {
            SpeechRecognitionEngine sre = new SpeechRecognitionEngine(new System.Globalization.CultureInfo("ru-RU"));

            string grammarPath = @"C:\Users\Morfa\source\repos\WindowsFormsApp1\WindowsFormsApp1\";
            //Компилируем наше грамматическое правило в файл Commands.cfg
            FileStream fs = new FileStream(grammarPath + "Commands.cfg", FileMode.Create);
            SrgsGrammarCompiler.Compile(grammarPath + "Comands.xml", (Stream)fs);
            fs.Close();

            Grammar gr = new Grammar(grammarPath + "Commands.cfg", "Команды");

            //Загружаем скомпилированный файл грамматики
            sre.LoadGrammar(gr);

            //Подписываемся на событие распознавания
            sre.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(sre_SpeechRecognized);

            sre.SetInputToDefaultAudioDevice();

            //Запускаем асинхронно распознаватель
            sre.RecognizeAsync(RecognizeMode.Multiple);
        }

        private void sre_SpeechRecognized(object sender, SpeechRecognizedEventArgs a)
        {
            string Text = a.Result.Text;
            int line;


            if (a.Result.Confidence > 0.6)
            {

                switch (a.Result.Text)
                {
                    case "плюс":
                        box.Focus();
                        box.SelectedText += a.Result.Semantics["plus"].Value.ToString();
                        box.SelectionStart = box.Text.Length;
                        break;
                    case "переменная а":
                        box.Focus();
                        box.SelectedText += "a";
                        box.SelectionStart = box.Text.Length;
                        break;
                    case "новая строка":
                        box.Focus();
                        box.SelectedText = "\n";
                        line = box.Text.Length;
                        box.SelectionStart = line;
                        break;
                    case "табуляция":
                        box.Focus();
                        box.SelectedText = a.Result.Semantics["tab"].Value.ToString();
                        line = box.Text.Length;
                        box.SelectionStart = line;
                        break;
                    case "сделай три пробела":
                        box.Focus();
                        box.SelectedText = "   ";
                        line = box.Text.Length;
                        box.SelectionStart = line;
                        break;
                    case "шаг назад":
                        box.Undo();
                        break;
                    case "поиск":
                        ev_meth.FindText(tbox.Text, box, tbox);
                        break;
                    case "курсор в конец":
                        line = box.Text.Length;
                        box.SelectionStart = line;
                        break;
                    case "запусти":
                        ev_meth.run_code(box.Text);
                        break;
                }
                if (Regex.IsMatch(Text, @"размер шрифта(\w*)", RegexOptions.IgnoreCase))
                {

                    int ab = Convert.ToInt32(a.Result.Semantics["num"].Value);
                    box.SelectionFont = new Font("Tahoma", ab, FontStyle.Bold);
                }


                if (Regex.IsMatch(Text, @"подключи библиотеку(\w*)", RegexOptions.IgnoreCase))
                {
                    box.Focus();
                    box.SelectedText += a.Result.Semantics["lib"].Value.ToString();
                    line = box.Text.Length;
                    box.SelectionStart = line;
                }

                if (Regex.IsMatch(Text, @"переменная(\w*)", RegexOptions.IgnoreCase))
                {
                    box.Focus();
                    box.SelectedText += a.Result.Semantics["var"].Value.ToString();
                    line = box.Text.Length;
                    box.SelectionStart = line;
                }
            }
        }
    }
}
