using BIOS;
using Motherboard;
using OperatingSystemInfo1;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Management;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Data;


namespace PlutonCheckInstall
{
    public class ValueColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var str = value as string;
            if (str == null) return null;

            if (str.Contains("KO") || str.Contains("Pas") || str.Contains("non")) return System.Windows.Media.Brushes.Red;
            else
                if (str.Contains("OK")) return System.Windows.Media.Brushes.Green;
            return System.Windows.Media.Brushes.White;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public partial class MainWindow : Window
    {
        [DllImport("gdi32.dll", EntryPoint = "DeleteObject")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool DeleteObject([In] IntPtr hObject);


        public class GPO
        {
            public string Nom { get; set; }
            public string Statut { get; set; }
            public string Valeur { get; set; }
            public string Attendue { get; set; }
        }

        public class OS
        {
            public string Nom { get; set; }
            public string Statut { get; set; }
        }

        public static string currentDirectory = Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);
        public static SystemInfo si = new SystemInfo();
        public static Boolean os = false;
        public static Boolean sql = false;
        public static Boolean bios = false;
        public static List<string> ls = new List<string>();
        public static List<OS> SaturneInfo = new List<OS>();
        public static List<GPO> BIOS_info = new List<GPO>();


        public MainWindow()
        {
            InitializeComponent();
            dataGrid.RowHeight = 30;
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //dataGrid.Visibility = Visibility.Hidden;
            //textBlock.Visibility = Visibility.Visible;
            
            if (!os)
            {
                foreach (string s in si.CheckHKEY_Logiciel())
                    ls.Add(s + "\n");
                ls.Add("MobiControl"); ls.Add(si.CheckMobiControl());
                ls.Add("Port Euridis");  ls.Add(si.CheckCOM());
                ls.Add("Chiffrage du disque"); ls.Add(si.CheckDisk());
                foreach (string s in si.getOperatingSystemInfo())
                    ls.Add(s + "\n");
                string test = si.CommandToString("powercfg", "query db310065-829b-4671-9647-2261c00e86ef sub_sleep hibernateidle");
                int index =test.IndexOf("continu");
                test = test.Substring(index+12).Remove(8, 4);
                //si.CommandToString("powercfg", "query scheme_balanced sub_sleep hibernateidle").Split(':')[12].Substring(3).Remove(8, 4);
                //(Convert.ToUInt32(si.CommandToString("powercfg", "query scheme_balanced sub_sleep hibernateidle").Split(':')[12].Substring(3).Remove(8, 4), 16)/60).ToString()+" minutes";
                ls.Add("Mise en veille"); ls.Add((Convert.ToUInt32(test, 16) / 60).ToString() + " minutes");
                ls.Add("Forçage du préchargement"); ls.Add(si.CheckScriptSCCM());
                ls.Add("Processeur"); ls.Add(si.getProcessorInfo() + "\n");
                ls.Add("Centre d'appartenance"); ls.Add(si.CheckCentre() + "\n");
                ls.Add("BIOS ReleaseDate"); ls.Add(BIOSInfo.ReleaseDate);
                ls.Add("BIOS SerialNumber"); ls.Add(BIOSInfo.SerialNumber);
                ls.Add("BIOS Version"); ls.Add(BIOSInfo.Version);
                ls.Add("Constructeur CM"); ls.Add(MotherboardInfo.Manufacturer);
                ls.Add("Produit"); ls.Add(MotherboardInfo.Product);
                ls.Add("Numéro de série CM"); ls.Add(MotherboardInfo.SerialNumber);
                ls.Add("Nom système"); ls.Add(MotherboardInfo.SystemName);
                ls.Add("Version CM"); ls.Add(MotherboardInfo.Version);
                os = true;
            }

            dataGrid.ItemsSource = List_OS();
        }
        private List<OS> List_OS()
        {
            List<OS> osi = new List<OS>();
            int compteur = 0;
            string[] s = new string[2];
            foreach (string line in ls)
            {
                if ((compteur % 2 == 0) && s[1] != null)
                    osi.Add(new OS()
                    {
                        Nom = s[0],
                        Statut = s[1],
                    });
                s[compteur % 2] = line;
                compteur++;
            }
            return osi;
        }

        private List<GPO> List_GPO()
        {
            List<GPO> gp = new List<GPO>();
            int compteur = 0;
            string[] s = new string[4];
            List<string> hkey = si.CheckHKEY();
            hkey.Add("\n"); //rajout artificiel d'une ligne
            foreach (string line in hkey)
            {
                if ((compteur % 4 == 0) && s[3] != null)
                    gp.Add(new GPO(){
                        Nom = s[0],
                        Statut = s[1],
                        Valeur = s[2].Length < 15 ? s[2] : s[2].Substring(0, 15)+"...",
                        Attendue = s[3].Length < 15 ? s[3] : s[3].Substring(0, 15) + "..."
                    });
                s[compteur % 4] = line;  
                compteur++;
            }
            return gp;
        }
        public string SetCOMSQL()
        {
            string com = si.CheckCOM();

            if (com.Equals("Coupleur Euridis non détecté"))
                return com;


            string path = currentDirectory + @"\SqlCeCmd.exe";
            byte[] SqlCeCmd = Properties.Resources.SqlCeCmd;

            if (!File.Exists(path)){
                if (SqlCeCmd == null)
                    return "Erreur !";
                File.WriteAllBytes(path, SqlCeCmd);
                File.SetAttributes(path, File.GetAttributes(path) | FileAttributes.Hidden);
            }
            Process SqlCeCmdexe = new Process();
            SqlCeCmdexe.StartInfo.FileName = path;
            SqlCeCmdexe.StartInfo.Arguments = "-d \"Data Source = C:\\Asais\\Saturne Mobile\\base\\basetsp.sdf\" -q \"UPDATE SE_OPTIONS_TSP SET OPTT_VALEUR = '" + com + "' WHERE OPTT_CODE = \'PortEuridis\'\"";
            SqlCeCmdexe.StartInfo.UseShellExecute = false;
            SqlCeCmdexe.StartInfo.CreateNoWindow = true;
            try
            {
                SqlCeCmdexe.Start();
                SqlCeCmdexe.WaitForExit();
                File.Delete(path);
                return "Port " + com + " modifié avec succès !";
            }
            catch (Exception e)
            {
                return e.ToString();
            }
        }
        public List<OS> SQL_Query()
        {
            List<string> SQLQuery = si.Get_SQL_Query();
            List<OS> SQL = new List<OS>();
            string path = currentDirectory + @"\SqlCeCmd.exe";
            string[] t = new string[2];
            byte[] SqlCeCmd = Properties.Resources.SqlCeCmd;
            int compteur = 0;


            if (!File.Exists(path)){
                if (SqlCeCmd == null)
                    return SQL;
                File.WriteAllBytes(path, SqlCeCmd);
                File.SetAttributes(path, File.GetAttributes(path) | FileAttributes.Hidden);
            }

            Process SqlCeCmdexe = new Process();
            SqlCeCmdexe.StartInfo.FileName = path;
            SqlCeCmdexe.StartInfo.UseShellExecute = false;
            SqlCeCmdexe.StartInfo.CreateNoWindow = true;
            SqlCeCmdexe.StartInfo.RedirectStandardOutput = true;

            foreach (string line in SQLQuery)
            {
                    try
                    {
                        if ((compteur % 2 == 0) && t[1] != null){
                            SqlCeCmdexe.StartInfo.Arguments = "-d \"Data Source = C:\\Asais\\Saturne Mobile\\base\\basetsp.sdf\" -q" + "\"" + t[1] + "\" -h 0 -W";
                            t[1] = Process.Start(SqlCeCmdexe.StartInfo).StandardOutput.ReadToEnd().Split('\r')[0];
                            SQL.Add(new OS(){
                                Nom = t[0],
                                Statut = t[1]
                            });
                    }
                    t[compteur % 2] = line;
                        compteur++;
                    }
                    catch (Exception){}
            }
            try{
                File.Delete(path);
            }
            catch (Exception) {}
            return SQL;
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            dataGrid.ItemsSource = List_GPO();
            dataGrid.Visibility = Visibility.Visible;
            textBlock.Visibility = Visibility.Hidden;
        }
        private void Button_Click2(object sender, RoutedEventArgs e){
            MessageBox.Show(SetCOMSQL());
        }
        private void Button_Click3(object sender, RoutedEventArgs e)
        {
            if (!sql)
            {
                SaturneInfo = SQL_Query();
                dataGrid.Visibility = Visibility.Visible;
                sql = true;
            }
            dataGrid.ItemsSource = SaturneInfo;
        }

        private List<GPO> PanasonicWMI(){
            ManagementObjectSearcher Panasonic = new ManagementObjectSearcher("root\\PanasonicPC", "SELECT * FROM SetBIOS4Conf");
            string[] t = new string[3];
            int compteur = 0;
            List<string> BIOS_Query = si.Get_WMI_BIOS();
            BIOS_Query.Add("\n");//rajout d'une ligne artificielle
            List<GPO> test = new List<GPO>();
            foreach (string line in BIOS_Query){
                try{
                    if ((compteur % 3 == 0) && t[2] != null){
                        foreach (ManagementObject queryObj in Panasonic.Get()){
                            t[1] = queryObj[t[1]].ToString();
                        }
                        test.Add(new GPO(){
                            Nom = t[0],
                            Statut = (t[2]!="") ? ((t[1] == t[2]) ? "OK" : "KO") :"",
                            Valeur = t[1],
                            Attendue = t[2]
                        });
                    }
                    t[compteur % 3] = line;
                    compteur++;
                }
                catch (Exception){}
            }
            return test;
        }
        /*
        private void Panasonis_Bios_Install()
        {
            string[] paths = {   currentDirectory + @"\PNSNProv.dll",
                                currentDirectory + @"\PNSNProv.mof",
                                currentDirectory + @"\SetBIOS4Conf.dll",
                                currentDirectory + @"\SetBIOS4Conf.mof"
                             };
            string[] t = new string[2];
            byte[][] files = {  Properties.Resources.PNSNProv,
                                Properties.Resources.PNSNProv1,
                                Properties.Resources.SetBIOS4Conf,
                                Properties.Resources.SetBIOS4Conf1
                             };
            int compteur = 0;

            foreach (string path in paths){
                if (!File.Exists(path)) {
                    File.WriteAllBytes(path, files[compteur]);
                    File.SetAttributes(path, File.GetAttributes(path) | FileAttributes.Hidden);
                }
                compteur++;
            }
            File.WriteAllText(currentDirectory + @"\Setup_SetBIOS.bat", Properties.Resources.Setup_SetBIOS);

            Process p = new Process();
            p.StartInfo.FileName = "powershell.exe";
            p.StartInfo.Arguments = currentDirectory + @"\Setup_SetBIOS.bat";
            p.StartInfo.CreateNoWindow = false;
            p.Start();
            p.WaitForExit();

            foreach (string path in paths)
                if (File.Exists(path))
                    File.Delete(path);
            if (File.Exists(currentDirectory + @"\Setup_SetBIOS.bat"))
                File.Delete(currentDirectory + @"\Setup_SetBIOS.bat");
        }
        */
        private void Button_Click4(object sender, RoutedEventArgs e){

            if (!bios){
                //Panasonis_Bios_Install();
                BIOS_info = PanasonicWMI();
                dataGrid.Visibility = Visibility.Visible;
                bios = true;
            }
            dataGrid.ItemsSource = BIOS_info;
        }
    }
}
