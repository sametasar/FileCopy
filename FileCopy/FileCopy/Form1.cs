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
using  Newtonsoft.Json;

namespace FileCopy
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private string Kaynakdosyaadi = "";

        List<Cls_Text> Liste = new List<Cls_Text>();

        private void textBox1_DoubleClick(object sender, EventArgs e)
        {
            OpenFileDialog file = new OpenFileDialog();
            //file.Filter = "Excel Dosyası |*.xlsx| Excel Dosyası|*.xls";
            //file.FilterIndex = 2;
            file.RestoreDirectory = true;
            file.CheckFileExists = false;
            file.Title = "Dosya seçimi yapın.";

            if (file.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = file.FileName;
                Kaynakdosyaadi = file.SafeFileName;
            }

            Cls_Text t = new Cls_Text();
            t.DosyaYolu = file.FileName;
            

            if (Liste.Count == 0)
            {
                Liste.Add(t);
            }
            else
            {
                Liste[0] = t;
            }
        }

        private void textBox2_DoubleClick(object sender, EventArgs e)
        {
            FolderBrowserDialog Klasor = new FolderBrowserDialog();
            Klasor.ShowDialog();
            textBox2.Text = Klasor.SelectedPath;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                MessageBox.Show("Kaynak dosya yolunu girmelisiniz!");
                return;
                
            }

            if(textBox2.Text == "")
            {
                MessageBox.Show("Hedef dosya yolunu girmelisiniz!");
                return;
            }

            listBox1.Items.Add(textBox2.Text + "\\" + Kaynakdosyaadi);
            Cls_Text t = new Cls_Text();
            t.DosyaYolu = textBox2.Text + "\\" + Kaynakdosyaadi;
            Liste.Add(t);
        }

        private void button2_Click(object sender, EventArgs e)
        {
           
            foreach (var i in listBox1.Items)
            {
                File.Copy(textBox1.Text, i.ToString(), true);
            }

            DOSYAYAZ();
            MessageBox.Show("Dosyaları aktarma işlemi tamamlandı!");
        }

        public void DOSYAOKU()
        {
            FileStream fs = new FileStream(Application.StartupPath + "/FilesInfo.txt", FileMode.Open, FileAccess.Read);
            StreamReader sw = new StreamReader(fs);
            
            try
            {
                Liste = JsonConvert.DeserializeObject<List<Cls_Text>>(sw.ReadLine());
                int i = 0;
                foreach (Cls_Text s in Liste)
                {
                    if (i == 0)
                    {
                        textBox1.Text = s.DosyaYolu;
                        Kaynakdosyaadi = textBox1.Text.Split('\\').LastOrDefault();
                    }
                    else
                    {
                        listBox1.Items.Add(s.DosyaYolu);
                    }

                    i++;
                }
            }
            catch
            {
                MessageBox.Show("Son kopyalanan dosya bilgileri okunamadı. Program içinde bulunan FilesInfo.txt dosyasının bozuk olmadığından emin olun!");
                return;
            }

            sw.Close();
            fs.Close();
        }

        public void DOSYAYAZ()
        {
            FileStream fs = new FileStream(Application.StartupPath + "\\FilesInfo.txt", FileMode.Open, FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs);
            string json = JsonConvert.SerializeObject(Liste);
            sw.WriteLine(json);
            sw.Close();
            fs.Close();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            DOSYAOKU();
        }

        private void BTNsİL_Click(object sender, EventArgs e)
        {
            Liste.Remove(Liste[listBox1.SelectedIndex+1]);
            listBox1.Items.Remove(listBox1.SelectedItem);
        }

        private void btnTemizle_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            Liste.Clear();
            Cls_Text t = new Cls_Text();
            t.DosyaYolu = textBox1.Text;
            Liste.Add(t);
        }
    }
}
