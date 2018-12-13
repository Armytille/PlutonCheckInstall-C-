using System;
using System.IO.Ports;
using System.Collections.ObjectModel;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Diagnostics;
using System.Text;
using System.Management;
using Microsoft.Win32;
using BIOS;
using Motherboard;
using System.IO;
using System.Collections.Generic;

namespace OperatingSystemInfo1 {
    public class SystemInfo {
        public List<string> getOperatingSystemInfo(){
            List<string> ls = new List<string>();
            ManagementObjectSearcher mos = new ManagementObjectSearcher("select * from Win32_OperatingSystem");
            foreach (ManagementObject managementObject in mos.Get()){
                if (managementObject["Locale"] != null)
                    if (managementObject["Locale"].ToString() == "040c")
                    {
                        ls.Add("Langue de la machine"); ls.Add("Français");
                    }
                    //else
                    //    ls.Add("La langue de la machine est : " + managementObject["Locale"].ToString());
                if (managementObject["Caption"] != null && managementObject["Version"] != null)
                {
                    ls.Add("Version de Windows"); ls.Add(managementObject["Caption"].ToString() + " " + managementObject["Version"].ToString());
                }
                 

                if (managementObject["OSArchitecture"] != null)
                {
                    ls.Add("Architecture de l'OS"); ls.Add(managementObject["OSArchitecture"].ToString());
                }
            }
            if (mos != null)
                mos.Dispose();
            
            return ls;
        }

        public string getProcessorInfo(){
            RegistryKey processor_name = Registry.LocalMachine.OpenSubKey(@"Hardware\Description\System\CentralProcessor\0", RegistryKeyPermissionCheck.ReadSubTree);   //This registry entry contains entry for processor info.

            if (processor_name != null)
                if (processor_name.GetValue("ProcessorNameString") != null)
                    return processor_name.GetValue("ProcessorNameString").ToString();
            return "Le processeur n'a pas de nom...";
        }

        public String CommandToString(string command, string arguments){
            ProcessStartInfo startInfo = new ProcessStartInfo(command);
            startInfo.Arguments = arguments;
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardOutput = true;
            startInfo.CreateNoWindow = true;
            string s = "";
            try{
                 s = Process.Start(startInfo).StandardOutput.ReadToEnd();
            }
            catch (Exception e){
                Console.WriteLine("The process failed: {0}", e.ToString());
            }

            return s;
           
        }
        public string RunScript(string scriptText){
            Runspace runspace = RunspaceFactory.CreateRunspace();

            runspace.Open();

            Pipeline pipeline = runspace.CreatePipeline();
            pipeline.Commands.AddScript(scriptText);

            pipeline.Commands.Add("Out-String");
            Collection < PSObject > results = pipeline.Invoke();

            runspace.Close();

            StringBuilder stringBuilder = new StringBuilder();
            foreach (PSObject obj in results)
                stringBuilder.AppendLine(obj.ToString());

            return stringBuilder.ToString();
        }
        public string CheckBiosPassword()
        {
            using (PowerShell PowerShellInstance = PowerShell.Create())
            {
                PowerShellInstance.AddScript(@"(gwmi -Class Lenovo_BiosPasswordSettings -Namespace root\wmi).PasswordState");
                Collection<PSObject> PSOutput = PowerShellInstance.Invoke();
                foreach (PSObject outputItem in PSOutput)
                    if (outputItem != null)
                        return outputItem.BaseObject.ToString();   
                 
            }
            return "Impossible de déterminer si le Bios a un mot de passe";
        }

        public bool CheckRegistre(string keyName, string type){
            try{
                RegistryKey test = RH.RegistryHelpers.GetRegistryKey(keyName, type);
                if (test == null)
                    return false;
                else
                    return true;

            }
            catch (Exception)
            {
                return false;
            }
        }
        public static object GetRegistryValue(string registryRoot, string valueName, string hive_HKLM_or_HKCU){
            RegistryKey root;

            root = RH.RegistryHelpers.GetRegistryKey(registryRoot,hive_HKLM_or_HKCU);

            if (root != null){
                try
                {
                 var valueKind = root.GetValueKind(valueName);
                 if (valueKind == RegistryValueKind.Binary){
                    var value = (byte[])root.GetValue(valueName);
                    return BitConverter.ToString(value);
                 }
                 else
                    return root.GetValue(valueName);
                }
                catch(System.IO.IOException)
                {
                    return null;
                }
            }  
            return null;
        }
        public string CheckLastBootUpTime() { return this.CommandToString(@"WMIC", "OS Get LastBootUpTime"); }
        public string CheckDate() { return DateTime.UtcNow.Date.ToString("d"); }
        public string CheckDisk(){ return ((this.CommandToString(@"C:\Windows\Sysnative\manage-bde.exe", "-status").Contains("Intégralement chiffré") ? "Intégralement chiffré" : "Pas intégralement chiffré")); }
        public string CheckMBAM(){ return ("MICROSOFT BITLOCKER ADMINISTRATION MONITORING " + (this.CheckRegistre(@"SOFTWARE\WOW6432Node\Souche\Inventaire\logiciels\MICROSOFT BITLOCKER ADMINISTRATION MONITORING 2.5 SP1", "HKLM") ? "Installé" : "Pas installé")); }
        public string CheckSCCM(){ return ("SCCM " + (this.CheckRegistre(@"SOFTWARE\WOW6432Node\Souche\Inventaire\logiciels\CLIENT SCCM CB1702", "HKLM")  ? "Installé" : "Pas installé"));}
        public string CheckScriptSCCM()
        {
            if (File.Exists(@"C:\Temp\LogScriptSCCM.txt"))
                return "OK";
            return "KO : Le script n'a pas été lancé !";
        }
        public string CheckCentre()
        {
        
            object rv = GetRegistryValue(@"SOFTWARE\Souche\Inventaire\Identification\OUSite", "Info", "HKLM");
            if (rv != null){
                string v = rv.ToString();
                return 
                    (v.Contains("HT") ? "REUNION" :
                    (v.Contains("HU") ? "MARTINIQUE" :
                    (v.Contains("HV") ? "GUADELOUPE" :
                    (v.Contains("HW") ? "GUYANE" :
                    (v.Contains("HZ") ? "CORSE" :
                    (v.Contains("HX") ? "NANTERRE" : "Aucun centre"))))));
            }
            return "Impossible de récupérer la valeur de la clef de registre SitePOSTE";
        }

        public void CheckPKI(){
            string SM = this.CommandToString(@"C:\Windows\Sysnative\cmdkey.exe", "/list:saturne-mobile.edf.fr");
            Console.WriteLine("Résultat de la commande WHOAMI : " + this.CommandToString(@"C:\Windows\Sysnative\whoami.exe",""));
            Console.WriteLine("Association PKI :\n" + (SM.Contains("AUCUN") ? "installation manuelle en cours... " +
                (this.CommandToString(@"C:\Windows\Sysnative\cmdkey.exe", "/add:saturne-mobile.edf.fr /smartcard").Contains("Aucun") ? "ERREUR : Insérer la clef PKI !" : "Clef PKI OK") : SM));
        }
        public string CheckCOM(){
            ManagementClass processClass = new ManagementClass("Win32_PnPEntity");
            string name;

            ManagementObjectCollection Ports = processClass.GetInstances();
            foreach (ManagementObject property in Ports)
            {

                if (property.GetPropertyValue("Name") != null)
                {
                    name = property.GetPropertyValue("Name").ToString();
                    if (name.Contains("USB") && name.Contains("COM"))
                        if (property.GetPropertyValue("Manufacturer").ToString().Contains("FTDI"))
                            if (property.GetPropertyValue("PNPDeviceID").ToString().Contains("A5004"))
                                return (name.Split('(')[1]).Replace(")", string.Empty);
                }
            }
             return "Coupleur Euridis non détecté";
        }

        public List<string> CnWHKEY(string message, string HKEY, object value, string type, string vulg){

            object v = GetRegistryValue(HKEY, message,type);
            List<string> s = new List<String>();
            if (v != null){
                v = v.ToString();
                s.Add(vulg);
                s.Add(v.Equals(value) ? "OK" : "KO");
                s.Add(v.ToString());
                s.Add(value.ToString());
                return s;
            }
            else {
                s.Add(vulg + "\n");
                s.Add("KO\n");
                s.Add("n'existe pas\n");
                s.Add(value.ToString() + "\n");
                return s;
            }          
        }

        public bool IsNumeric(object Expression)
        {
            double retNum;

            bool isNum = Double.TryParse(Convert.ToString(Expression), System.Globalization.NumberStyles.Any, System.Globalization.NumberFormatInfo.InvariantInfo, out retNum);
            return isNum;
        }

        public List <string> CheckHKEY(){
            string currentDirectory = Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);
            string path = currentDirectory + @"\HK_check_config.txt";
            List<Object> param = new List<Object>();
            List<string> ls = new List<string>();
            if (File.Exists(path)){
                Object[] text = File.ReadAllText(path).Split('"');
          
                foreach (object mot in text)
                    param.Add(mot);

                for (int i = 0; i < param.Count / 10; i++)
                    foreach (string line in CnWHKEY(param[1 + (10 * i)].ToString(), param[3 + (10 * i)].ToString(), param[5 + (10 * i)], param[7 + (10 * i)].ToString(), param[9 + (10 * i)].ToString()))
                        ls.Add(line + "\n");
            }
            else
            {
                ls.Add("Présence du fichier " + path);
                ls.Add("KO");
            }
            return ls;
        }
        public string CheckLogiciel(string message, string HK_path, string HKLM_HKCU) { return (/*message + " " +*/ (this.CheckRegistre(HK_path, HKLM_HKCU) ? "Installé" : "Pas installé")); }

        public string CheckMobiControl()
        {
            if (File.Exists(@"C:\Program Files (x86)\SOTI\MobiControl\Install.bat"))
                return "Installé";
            return "Pas installé";
        }
        public List<string> CheckHKEY_Logiciel()
        {
            string currentDirectory = Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);
            string path = currentDirectory + @"\HK_Logiciels.txt";
            List<Object> param = new List<Object>();
            List<string> ls = new List<string>();
            if (File.Exists(path))
            {
                Object[] text = File.ReadAllText(path).Split('"');

                foreach (object mot in text)
                    param.Add(mot);

                for (int i = 0; i < param.Count / 6; i++) {
                    ls.Add(param[1 + (6 * i)].ToString());
                    ls.Add(CheckLogiciel(param[1 + (6 * i)].ToString(), param[3 + (6 * i)].ToString(), param[5 + (6 * i)].ToString()));
                }
            }
            else
            {
                ls.Add("Présence du fichier " + path);
                ls.Add("KO");
            }
                
            return ls;
        }
        public List<string> Get_SQL_Query(){
            string currentDirectory = Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);
            string path = currentDirectory + @"\SaturneSQL.txt";
            List<string> ls = new List<string>();
            List<string> param = new List<string>();

            if (File.Exists(path))
            {
                string[] text = File.ReadAllText(path).Split('"');
                foreach (string line in text)
                    param.Add(line);
                for (int i = 0; i < param.Count / 2; i++)
                    ls.Add(param[1 + (2 * i)].ToString());
            }
            else
            {
                ls.Add("Présence du fichier " + path);
                ls.Add("KO");
            }
            return ls;
        }
        public List<string> Get_WMI_BIOS()
        {
            string currentDirectory = Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);
            string path = currentDirectory + @"\WMI_BIOS_Conf.txt";
            List<string> ls = new List<string>();
            List<string> param = new List<string>();

            if (File.Exists(path))
            {
                string[] text = File.ReadAllText(path).Split('"');
                foreach (string line in text)
                    param.Add(line);
                for (int i = 0; i < param.Count / 2; i++)
                    ls.Add(param[1 + (2 * i)].ToString());
            }
            else
            {
                ls.Add("Présence du fichier " + path);
                ls.Add("KO");
            }
            return ls;
        }

    }
}
