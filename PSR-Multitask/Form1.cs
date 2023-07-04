using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PSR_Multitask
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        string path = System.Reflection.Assembly.GetExecutingAssembly()
               .Location + @"\..\..\..\..\PSR-Multitask\Resources\";

        private async void button1_Click(object sender, EventArgs e)
        {
            if (numlineas.Text == String.Empty)
            {
                MessageBox.Show(
                    "Please fill in the blank with the desired\nnumber of lines you want.",
                    "Error",
                    MessageBoxButtons.OK
                    );
            }
            else
            {
                int cant = int.Parse(numlineas.Text);

                await Write(cant);
            }
        }

        private async Task Write(int cant)
        {
            FileStream fs;

            string filename = DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + ".txt";

            // MessageBox.Show($"{filename}");

            try
            {
                fs = File.Create(path + filename);

                progressBar1.Maximum = cant;


                Thread writetxt = new Thread(async () =>
                {
                    try
                    {
                        using (StreamWriter sw = new StreamWriter(fs))
                        {
                            for (int i = 1; i <= cant; i++)
                            {
                                sw.WriteLine($"Line {i.ToString()}");
                                progressBar1.Invoke((MethodInvoker)(() => progressBar1.Value = i));
                            }
                        }

                        MsgBox(cant);

                        Thread.Sleep(2500);

                        Process.Start(path);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Critical error\n{ex}");
                    }
                });

                writetxt.Start();

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Critical error\n{ex}");
            }
        }

        private void MsgBox(int cant)
        {
            Thread msg = new Thread((() =>
            {
                MessageBox.Show($"Text file with {cant} lines created successfully.\nOpening folder..");
            }));

            msg.Start();
        }

        private void numlineas_KeyPress(object sender, KeyPressEventArgs e)
        {
                e.Handled = !Regex.Match(e.KeyChar.ToString(), "^(\\d+)$").Success;
        }
    }
}
