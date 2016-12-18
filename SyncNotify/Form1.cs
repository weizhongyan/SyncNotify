using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace SyncNotify
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            startSyncthing();
            MessageBox.Show("Syncthing is loading...", "SyncNotify");
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            this.Hide();
        }

        public void startSyncthing()
        {
            try
            {
                //Ejecutamos synchting antes de iniciar la aplicacion
                Process startInfo = new Process();
                startInfo.StartInfo.FileName = "syncthing.exe";
                startInfo.StartInfo.Arguments = "-no-console";
                startInfo.Start();
            }
            catch (Exception e)
            {
                MessageBox.Show("We couldn't find Syncthing.exe", "SyncNotify");
                this.notifyIcon1.Dispose();
                System.Diagnostics.Process.GetCurrentProcess().Kill();
            }
            
        }

        private void openWebInterfaceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ProcessStartInfo sInfo = new ProcessStartInfo("http://127.0.0.1:8384/");
            Process.Start(sInfo);
        }

        private void notifyIcon1_DoubleClick(object sender, EventArgs e)
        {
            ProcessStartInfo sInfo = new ProcessStartInfo("http://127.0.0.1:8384/");
            Process.Start(sInfo);
        }


        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Tomamos todos los procesos con nombre "syncthing"
            Process[] procs = Process.GetProcessesByName("syncthing");

            //Recorremos el vector matando proceso por proceso
            foreach (Process proc in procs)
            {
                proc.Kill();
            }

            this.notifyIcon1.Dispose();
            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }

    }
}
