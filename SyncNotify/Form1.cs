using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace SyncNotify
{
    public partial class Form1 : Form
    {
        public static String AppDataFolder;
        public static String SyncthingDataFolder;
        public static String BackupFolder;

        public Form1()
        {
            InitializeComponent();
            this.CenterToScreen();

            //Get the localAppdata Folder
            AppDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            //Add the Syncthing Appdatafolder to the localAppdata
            SyncthingDataFolder = Path.Combine(AppDataFolder, "Syncthing");
            //Declare the backup Folder
            BackupFolder = "BackupFolder";

            //Create the Backup Folder
            if (!System.IO.Directory.Exists(BackupFolder))
            {
                System.IO.Directory.CreateDirectory(BackupFolder);
            } 

            startSyncthing();
            MessageBox.Show("Syncthing is loading ...", "SyncNotify");
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

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (AboutBox1 box = new AboutBox1())
            {
                box.ShowDialog(this);
            }
        }

        //Import Settings
        private void importToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult dialogResult = MessageBox.Show("Are you sure you want to Restore the Backup?\nAll your settings will be overwritten", 
                                                            "SyncNotify", MessageBoxButtons.YesNo);

                if (dialogResult == DialogResult.Yes)
                {
                    string[] files = System.IO.Directory.GetFiles(BackupFolder);

                    // Copy the files and overwrite destination files if they already exist.
                    foreach (string s in files)
                    {
                        // Use static Path methods to extract only the file name from the path.
                        String fileName = Path.GetFileName(s);
                        String destFile = Path.Combine(SyncthingDataFolder, fileName);
                        File.Copy(s, destFile, true);
                    }

                    MessageBox.Show(this, "Backup Restored, Please Restart SyncNotify", "SyncNotify");
                    this.notifyIcon1.Dispose();
                    System.Diagnostics.Process.GetCurrentProcess().Kill();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "SyncNotify");
            }
           
        }

        //Export Settings
        private void exportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                //Get all the files from that folder
                string[] files = System.IO.Directory.GetFiles(SyncthingDataFolder);

                foreach (string s in files)
                {
                    String fileName = Path.GetFileName(s);

                    if (fileName == "cert.pem" || fileName == "key.pem" || fileName == "config.xml")
                    {
                        // Use static Path methods to extract only the file name from the path.
                        String destFile = Path.Combine(BackupFolder, fileName);
                        File.Copy(s, destFile, true);
                    }

                }

                MessageBox.Show(this, "Backup Performed", "SyncNotify");
                Process.Start(BackupFolder);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "SyncNotify");
            }
        }

    }
}
