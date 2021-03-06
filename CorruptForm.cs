﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Corruption;

namespace Corruption
{
    public partial class CorruptForm : System.Windows.Forms.Form
    {
        string filePetaPath = "";
        string filePopulationPath = "";

        public CorruptForm()
        {
            InitializeComponent();
        }

        private void numPicker_ValueChanged(object sender, EventArgs e)
        {
              // do nothing
        }

        private async void submitButton_Click(object sender, EventArgs e)
        {
            try
            {
                int time = (int)numPicker.Value;
                for(int i = 0; i <= time; ++i)
                {
                    Graph g = new Graph();
                    g.loadFromFile(population: this.filePopulationPath, peta: this.filePetaPath);
                    g.BFS(i);
                    g.PrintInfectionPath();
                    g.PrintInfo();
                    mainPanel.Controls.Clear();
                    mainPanel.Controls.Add(g.Visualize(i));
                    pathBox.Text = "==[ Day " + i.ToString()+" ]==" + Environment.NewLine;
                    pathBox.Text += g.PrintInfectionPath();
                    await Task.Delay(50);
                }
            } catch (Exception error)
            {
                pathBox.Text = "";
                signPeta.Text = "-";
                popLabel.Text = "-";

                filePetaPath = "";
                filePopulationPath = "";

                numPicker.Value = 0;

                mainPanel.Controls.Clear();
                string message = error.Message;
                string title = "Error happened";
                MessageBox.Show(message, title);
            }            
        }

        private void labelText_Click(object sender, EventArgs e)
        {
            // do nothing
        }

        private void folderBrowserDialog1_HelpRequest(object sender, EventArgs e)
        {
            

        }

        private void button3_Click(object sender, EventArgs e)
        {
            signPeta.Text = "-";
            popLabel.Text = "-";
            pathBox.Text = "";
            filePetaPath = "";
            filePopulationPath = "";

            numPicker.Value = 0;

            mainPanel.Controls.Clear();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            DialogResult result = fileDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                this.filePetaPath = fileDialog.FileName;
                string[] text = this.filePetaPath.Split('\\');
                signPeta.Text = text[text.Length - 1];
                Console.WriteLine("File Peta: " + this.filePetaPath);
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void popBtnBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            DialogResult result = fileDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                this.filePopulationPath = fileDialog.FileName;
                string[] text = this.filePopulationPath.Split('\\');
                popLabel.Text = text[text.Length - 1];
                Console.WriteLine("File Populasi : " + this.filePopulationPath);
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void CorruptForm_Load(object sender, EventArgs e)
        {

        }

        private void signPeta_Click(object sender, EventArgs e)
        {

        }

        private void labelBrowse2_Click(object sender, EventArgs e)
        {

        }

        private void labelBrowse1_Click(object sender, EventArgs e)
        {

        }


        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }
    }
}
