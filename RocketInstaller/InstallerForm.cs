using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using ICSharpCode.SharpZipLib.Zip;
using MetroFramework;
using MetroFramework.Forms;
using Microsoft.Win32;

namespace RocketInstaller
{
    public partial class InstallerForm : MetroForm
    {
        private static readonly char Sp = Path.DirectorySeparatorChar;
        private const String API_KEY = "18682849-FBC3-4298-A7B6-1E4B71A60633";
        public InstallerForm()
        {
            //Todo: add beta option
            //Todo: make excluding of "Servers" directory optional

            InitializeComponent();
            BrowseButton.Click += BrowseButton_Click;
            InstallButton.Click += InstallButton_Click;
        }

        private void InstallerForm_Load(object sender, EventArgs e)
        {
            String steamPath = GetSteamDirectory();
            if (steamPath != null)
            {
                String unturnedDir = AppendDirectory(steamPath, "SteamApps", "common", "Unturned");
                if (Directory.Exists(unturnedDir) && File.Exists(AppendDirectory(unturnedDir, "Unturned.exe")))
                {
                    InstallDirectory.Text = unturnedDir;
                    StatusSuccess("Unturned found");
                    return;
                }
                StatusError("Unturned not found");
                return;
            }
            StatusError("Steam directory not found");
        }

        private static String AppendDirectory(String path, params String[] dirs)
        {
            if (path == null)
            {
                path = "";
            }

            if (dirs == null || !dirs.Any())
            {
                return path;
            }

            foreach (String s in dirs)
            {
                if (!path.EndsWith(Sp.ToString()))
                {
                    path += Sp;
                }
                path += s;
            }

            return path;
        }

        private static String GetSteamDirectory()
        {
            if (IsLinux())
            {
                if (File.Exists("~/.steam"))
                {
                    return "~/.steam";
                }
                if (File.Exists("~/.local/share/Steam"))
                {
                    return "~/.local/share/Steam";
                }
                return null;
            }

            //Windows
            RegistryKey regKey = Registry.CurrentUser;
            regKey = regKey.OpenSubKey(@"Software\Valve\Steam");

            if (regKey == null || regKey.GetValue("SteamPath") == null) return null;

            String dir=  regKey.GetValue("SteamPath").ToString();
            return Directory.Exists(dir) ? dir.Replace(@"/", Sp.ToString()) : null;
        }

        public static bool IsLinux()
        {
            int p = (int)Environment.OSVersion.Platform;
            return (p == 4) || (p == 6) || (p == 128);
        }

        private void BrowseButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            if (GetSteamDirectory() != null)
            {
                String commonDir = AppendDirectory(GetSteamDirectory(), "SteamApps", "common");
                fileDialog.InitialDirectory = Directory.Exists(commonDir) ? commonDir : GetSteamDirectory();
            }

            fileDialog.Filter = "Unturned (Unturned.exe)|Unturned.exe";
            fileDialog.FilterIndex = 1;
            fileDialog.RestoreDirectory = true;
            if (fileDialog.ShowDialog() != DialogResult.OK) return;
            String file = fileDialog.FileName;
            if (!File.Exists(file)) return;
            InstallDirectory.Text = Directory.GetParent(file).FullName;
        }

        private void InstallButton_Click(object sender, EventArgs e)
        {
            InstallButton.Enabled = false;
            BrowseButton.Enabled = false;
            InstallDirectory.Enabled = false;
            InstallProgress.Visible = true;
            InstallProgress.ProgressBarStyle = ProgressBarStyle.Marquee;
            InstallProgress.Value = 1;
            UseSeperateCheckBox.Enabled = false;

            if (!Directory.Exists(InstallDirectory.Text))
            {
                InstallationFailed("Directory doesn't exists");
                return;
            }


            if (!IsDirectory(InstallDirectory.Text)) 
            {
                InstallationFailed("Given path is not a directory");
                return;
            }

            if (!File.Exists(AppendDirectory(InstallDirectory.Text, "Unturned.exe")))
            {
                InstallationFailed("Unturned.exe not found");
                return;
            }

            try
            {
                Install();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Unexpected exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
                InstallationFailed("Installation failed");
            }
        }

        private String _unturnedDir;
        private void Install()
        {
            String installDir = InstallDirectory.Text;
            if (installDir.EndsWith(Sp.ToString()))
            {
                installDir.Remove(installDir.Length - 1); //Remove last char
            }

            const string serverSuffix = " - Server";
            if (UseSeperateCheckBox.Checked && !installDir.EndsWith(serverSuffix))
            {
                StatusNeutral("Copying files...");
                String copyDir = installDir + serverSuffix;
                CopyDirs(installDir, copyDir);
                return;
            }
            _unturnedDir = installDir;
            StartDownload();
        }

        private async void CopyDirs(String sourceDir, String targetDir)
        {
            if (!Directory.Exists(targetDir))
            {
                Directory.CreateDirectory(targetDir);
            }

            foreach (string dirPath in Directory.GetDirectories(sourceDir, "*", SearchOption.AllDirectories).Where(dirPath => Path.GetDirectoryName(dirPath) != "Servers"))
            {
                Directory.CreateDirectory(dirPath.Replace(sourceDir, targetDir));
                foreach (string filename in Directory.EnumerateFiles(dirPath))
                {
                    using (FileStream sourceStream = File.Open(filename, FileMode.Open))
                    {
                        using (FileStream destinationStream = File.Create(filename.Replace(sourceDir, targetDir)))
                        {
                            StatusNeutral("Copying: " + Path.GetFileName(filename));
                            await sourceStream.CopyToAsync(destinationStream);
                        }
                    }
                }
            }

            foreach (string filename in Directory.EnumerateFiles(sourceDir))
            {
                using (FileStream sourceStream = File.Open(filename, FileMode.Open))
                {
                    using (FileStream destinationStream = File.Create(targetDir + filename.Substring(filename.LastIndexOf('\\'))))
                    {
                        StatusNeutral("Copying: " + Path.GetFileName(filename));
                        await sourceStream.CopyToAsync(destinationStream);
                    }
                }
            }
            _unturnedDir = targetDir;
            StartDownload();
        }

        private async void CopyFile(String source, String target)
        {
            using (FileStream sourceStream = File.Open(source, FileMode.Open))
            {
                using (FileStream destinationStream = File.Create(target))
                {
                    await sourceStream.CopyToAsync(destinationStream);
                }
            }
        }

        private static readonly String TmpDir = AppendDirectory(Path.GetTempPath(), "RocketInstaller"); 
        private static readonly String RocketZipFile = AppendDirectory(TmpDir, "Rocket.zip");
        private static readonly String ExtractedDir = AppendDirectory(TmpDir, "Rocket");

        private void StartDownload()
        {
            StatusNeutral("Starting download");
            if (!Directory.Exists(TmpDir))
            {
                Directory.CreateDirectory(TmpDir);
            }
            InstallProgress.ProgressBarStyle = ProgressBarStyle.Blocks;
            InstallProgress.Value = 0;
            WebClient client = new WebClient();
            client.DownloadProgressChanged += DownloadProgressChanged;
            client.DownloadFileCompleted += DownloadFileCompleted;
            if (IsLinux())
            {
                client.DownloadFileAsync(new Uri("http://api.rocket.foundation/linux-beta/latest/" + API_KEY), RocketZipFile);
                return;
            }
            client.DownloadFileAsync(new Uri("http://api.rocket.foundation/beta/latest/" + API_KEY), RocketZipFile);
        }

        private void DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            InstallProgress.ProgressBarStyle = ProgressBarStyle.Marquee;
            StatusNeutral("Download complete, extracting...");
            Decompress(RocketZipFile, ExtractedDir);
            StatusNeutral("Installing files...");
            String managedDir = AppendDirectory(_unturnedDir, "Unturned_Data", "Managed");
            String[] files = Directory.GetFiles(ExtractedDir);
            foreach (String file in files)
            {
                String fileName = Path.GetFileName(file);
                if (fileName == null || !fileName.EndsWith(".dll")) continue;
                StatusNeutral("Installing: " + fileName);
                CopyFile(file, AppendDirectory(managedDir, fileName));
            }
            String extractedScriptsDir = AppendDirectory(ExtractedDir, "Scripts");
            String untScriptsDir = AppendDirectory(managedDir, "Scripts");
            if (Directory.Exists(extractedScriptsDir ))
            {
                if (!Directory.Exists(untScriptsDir))
                {
                    Directory.CreateDirectory(untScriptsDir);
                }
                files = Directory.GetFiles(extractedScriptsDir );
                foreach (String file in files)
                {
                    String fileName = Path.GetFileName(file);
                    if (fileName == null) continue;

                    StatusNeutral("Installing script: " + fileName);
                    CopyFile(file, AppendDirectory(untScriptsDir, fileName));
                }
            }

            InstallProgress.ProgressBarStyle = ProgressBarStyle.Blocks;
            InstallProgress.Value = 100;

            StatusSuccess("Installation was successful");

            DialogResult dialogResult = MessageBox.Show("Installation was successful. Open server directory?", "Installation successful",
                MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk);
            if (dialogResult == DialogResult.Yes)
            {
                Process.Start ("file:///" + _unturnedDir);
            }
        } 

        private void DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            double bytesIn = double.Parse(e.BytesReceived.ToString());
            double totalBytes = double.Parse(e.TotalBytesToReceive.ToString());
            double percentage = bytesIn / totalBytes * 100;
            StatusNeutral("Downloaded " + e.BytesReceived + " of " + e.TotalBytesToReceive + "(" +(int) percentage + "%)");
            InstallProgress.Value = int.Parse(Math.Truncate(percentage).ToString(CultureInfo.CurrentCulture));
        }

        private static bool IsDirectory(String s)
        {
            FileAttributes attr = File.GetAttributes(s);
            return (attr & FileAttributes.Directory) == FileAttributes.Directory;
        }

        public static void Decompress(String file, String targetDir, String fileFilter = null)
        {
            FastZip fastZip = new FastZip();
            fastZip.ExtractZip(file, targetDir, fileFilter);
        }

        private void InstallationFailed(String reason)
        {
            InstallButton.Enabled = true;
            BrowseButton.Enabled = true;
            InstallDirectory.Enabled = true;
            InstallProgress.Visible = false;
            StatusError("Failed: " + reason);
        }

        private void StatusSuccess(String text)
        {
            StatusLabel.Text = "Status: " + text;
            StatusLabel.Style = MetroColorStyle.Green;
        }

        private void StatusError(String text)
        {
            StatusLabel.Text = "Status: " + text;
            StatusLabel.Style = MetroColorStyle.Red;
        }

        private void StatusNeutral(string text)
        {
            StatusLabel.Text = "Status: " + text;
            StatusLabel.Style = MetroColorStyle.Black;
        }
    }
}
