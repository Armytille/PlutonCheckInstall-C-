using System;
using System.Management;

namespace BIOS
{
    static public class BIOSInfo
    {
        private static ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_BIOS");

        static public string BiosCharacteristics
        {
            get
            {
                try
                {
                    foreach (ManagementObject queryObj in searcher.Get())
                    {
                        return GetBiosCharacteristics(int.Parse(queryObj["BiosCharacteristics"].ToString()));
                    }
                    return "";
                }
                catch (Exception)
                {
                    return "";
                }
            }
        }

        static public string BIOSVersion
        {
            get
            {
                try
                {
                    foreach (ManagementObject queryObj in searcher.Get())
                    {
                        return queryObj["BIOSVersion"].ToString();
                    }
                    return "";
                }
                catch (Exception)
                {
                    return "";
                }
            }
        }

        static public string BuildNumber
        {
            get
            {
                try
                {
                    foreach (ManagementObject queryObj in searcher.Get())
                    {
                        return queryObj["BuildNumber"].ToString();
                    }
                    return "";
                }
                catch (Exception)
                {
                    return "";
                }
            }
        }

        static public string Caption
        {
            get
            {
                try
                {
                    foreach (ManagementObject queryObj in searcher.Get())
                    {
                        return queryObj["Caption"].ToString();
                    }
                    return "";
                }
                catch (Exception)
                {
                    return "";
                }
            }
        }

        static public string CurrentLanguage
        {
            get
            {
                try
                {
                    foreach (ManagementObject queryObj in searcher.Get())
                    {
                        return queryObj["CurrentLanguage"].ToString();
                    }
                    return "";
                }
                catch (Exception)
                {
                    return "";
                }
            }
        }

        static public string Description
        {
            get
            {
                try
                {
                    foreach (ManagementObject queryObj in searcher.Get())
                    {
                        return queryObj["Description"].ToString();
                    }
                    return "";
                }
                catch (Exception)
                {
                    return "";
                }
            }
        }

        static public int InstallableLanguages
        {
            get
            {
                try
                {
                    foreach (ManagementObject queryObj in searcher.Get())
                    {
                        return int.Parse(queryObj["InstallableLanguages"].ToString());
                    }
                    return 0;
                }
                catch (Exception)
                {
                    return 0;
                }
            }
        }

        static public string InstallDate
        {
            get
            {
                try
                {
                    foreach (ManagementObject queryObj in searcher.Get())
                    {
                        return ConvertToDateTime(queryObj["InstallDate"].ToString());
                    }
                    return "";
                }
                catch (Exception)
                {
                    return "";
                }
            }
        }

        static public string LanguageEdition
        {
            get
            {
                try
                {
                    foreach (ManagementObject queryObj in searcher.Get())
                    {
                        return queryObj["LanguageEdition"].ToString();
                    }
                    return "";
                }
                catch (Exception)
                {
                    return "";
                }
            }
        }

        static public string Manufacturer
        {
            get
            {
                try
                {
                    foreach (ManagementObject queryObj in searcher.Get())
                    {
                        return queryObj["Manufacturer"].ToString();
                    }
                    return "";
                }
                catch (Exception)
                {
                    return "";
                }
            }
        }

        static public string Name
        {
            get
            {
                try
                {
                    foreach (ManagementObject queryObj in searcher.Get())
                    {
                        return queryObj["Name"].ToString();
                    }
                    return "";
                }
                catch (Exception)
                {
                    return "";
                }
            }
        }

        static public bool PrimaryBIOS
        {
            get
            {
                try
                {
                    foreach (ManagementObject queryObj in searcher.Get())
                    {
                        if (queryObj["PrimaryBIOS"].ToString() == "True")
                            return true;
                        else
                            return false;
                    }
                    return false;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }

        static public string ReleaseDate
        {
            get
            {
                try
                {
                    foreach (ManagementObject queryObj in searcher.Get())
                    {
                        return ConvertToDateTime(queryObj["ReleaseDate"].ToString());
                    }
                    return "";
                }
                catch (Exception)
                {
                    return "";
                }
            }
        }

        static public string SerialNumber
        {
            get
            {
                try
                {
                    foreach (ManagementObject queryObj in searcher.Get())
                    {
                        return queryObj["SerialNumber"].ToString();
                    }
                    return "";
                }
                catch (Exception)
                {
                    return "";
                }
            }
        }

        static public string SMBIOSBIOSVersion
        {
            get
            {
                try
                {
                    foreach (ManagementObject queryObj in searcher.Get())
                    {
                        return queryObj["SMBIOSBIOSVersion"].ToString();
                    }
                    return "";
                }
                catch (Exception)
                {
                    return "";
                }
            }
        }

        static public int SMBIOSMajorVersion
        {
            get
            {
                try
                {
                    foreach (ManagementObject queryObj in searcher.Get())
                    {
                        return int.Parse(queryObj["SMBIOSMajorVersion"].ToString());
                    }
                    return 0;
                }
                catch (Exception)
                {
                    return 0;
                }
            }
        }

        static public int SMBIOSMinorVersion
        {
            get
            {
                try
                {
                    foreach (ManagementObject queryObj in searcher.Get())
                    {
                        return int.Parse(queryObj["SMBIOSMinorVersion"].ToString());
                    }
                    return 0;
                }
                catch (Exception)
                {
                    return 0;
                }
            }
        }

        static public bool SMBIOSPresent
        {
            get
            {
                try
                {
                    foreach (ManagementObject queryObj in searcher.Get())
                    {
                        if (queryObj["SMBIOSPresent"].ToString() == "True")
                            return true;
                        else
                            return false;
                    }
                    return false;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }

        static public string SoftwareElementID
        {
            get
            {
                try
                {
                    foreach (ManagementObject queryObj in searcher.Get())
                    {
                        return queryObj["SoftwareElementID"].ToString();
                    }
                    return "";
                }
                catch (Exception)
                {
                    return "";
                }
            }
        }

        static public string SoftwareElementState
        {
            get
            {
                try
                {
                    foreach (ManagementObject queryObj in searcher.Get())
                    {
                        return GetSoftwareElementState(int.Parse(queryObj["SoftwareElementState"].ToString()));
                    }
                    return "";
                }
                catch (Exception)
                {
                    return "";
                }
            }
        }

        static public string Status
        {
            get
            {
                try
                {
                    foreach (ManagementObject queryObj in searcher.Get())
                    {
                        return queryObj["Status"].ToString();
                    }
                    return "";
                }
                catch (Exception)
                {
                    return "";
                }
            }
        }

        static public string TargetOperatingSystem
        {
            get
            {
                try
                {
                    foreach (ManagementObject queryObj in searcher.Get())
                    {
                        return GetTargetOperatingSystem(int.Parse(queryObj["TargetOperatingSystem"].ToString()));
                    }
                    return "";
                }
                catch (Exception)
                {
                    return "";
                }
            }
        }

        static public string Version
        {
            get
            {
                try
                {
                    foreach (ManagementObject queryObj in searcher.Get())
                    {
                        return queryObj["Version"].ToString();
                    }
                    return "";
                }
                catch (Exception)
                {
                    return "";
                }
            }
        }

        private static string GetBiosCharacteristics(int charac)
        {
            switch (charac)
            {
                case 0: return "Reserved";
                case 1: return "Reserved";
                case 2: return "Unknown";
                case 3: return "BIOS Characteristics Not Supported";
                case 4: return "ISA is supported";
                case 5: return "MCA is supported";
                case 6: return "EISA is supported";
                case 7: return "PCI is supported";
                case 8: return "PC Card (PCMCIA) is supported";
                case 9: return "Plug and Play is supported";
                case 10: return "APM is supported";
                case 11: return "BIOS is Upgradable (Flash)";
                case 12: return "BIOS shadowing is allowed";
                case 13: return "VL-VESA is supported";
                case 14: return "ESCD support is available";
                case 15: return "Boot from CS is supported";
                case 16: return "Selectable Boot is supported";
                case 17: return "BIOS ROM is socketed";
                case 18: return "Boot From PC Card (PCMCIA) is supported";
                case 19: return "EDD (Enhanced Disk Drive) Specification is supported";
                case 20: return "Int 13h - Japanese Floppy for NEC 9800 1.2mb (3.5, 1k Bytes/Sector, 360RPM) is supported";
                case 21: return "Int 13h - Japanese Floppy for Toshiba 1.2mb (3.5, 360RPM) is supported";
                case 22: return "Int 13h - 5.25/360KB Floppy Services are supported";
                case 23: return "Int 13h - 5.25/1.2MB Floppy Services are supported";
                case 24: return "Int 13h - 3.5/720KB Floppy Services are supported";
                case 25: return "Int 13h - 3.5/2.88MB Floppy Services are supported";
                case 26: return "Int 5h, Print Screen Service is supported";
                case 27: return "Int 9h, 8042 Keyboard Services are supported";
                case 28: return "Int 14h, Serial Services are supported";
                case 29: return "Int 17h, Printer Services are supported";
                case 30: return "Int 10h, CGA/Mono Video Services are supported";
                case 31: return "NEC PC-98";
                case 32: return "ACPI is supported";
                case 33: return "USB Legacy is supported";
                case 34: return "AGP is supported";
                case 35: return "I2O boot is supported";
                case 36: return "LS-120 boot is supported";
                case 37: return "ATAPI ZIP Drive boot is supported";
                case 38: return "1394 boot is supported";
                case 39: return "Smart Battery is supported";
                case 40: return "Reserved for BIOS vendor";
                case 41: return "Reserved for BIOS vendor";
                case 42: return "Reserved for BIOS vendor";
                case 43: return "Reserved for BIOS vendor";
                case 44: return "Reserved for BIOS vendor";
                case 45: return "Reserved for BIOS vendor";
                case 46: return "Reserved for BIOS vendor";
                case 47: return "Reserved for BIOS vendor";
                case 48: return "Reserved for system vendor";
                case 49: return "Reserved for system vendor";
                case 50: return "Reserved for system vendor";
                case 51: return "Reserved for system vendor";
                case 52: return "Reserved for system vendor";
                case 53: return "Reserved for system vendor";
                case 54: return "Reserved for system vendor";
                case 55: return "Reserved for system vendor";
                case 56: return "Reserved for system vendor";
                case 57: return "Reserved for system vendor";
                case 58: return "Reserved for system vendor";
                case 59: return "Reserved for system vendor";
                case 60: return "Reserved for system vendor";
                case 61: return "Reserved for system vendor";
                case 62: return "Reserved for system vendor";
                case 63: return "Reserved for system vendor";
                default: return "Unknown";
            }
        }

        private static string ConvertToDateTime(string unconvertedTime)
        {
            string convertedTime = "";
            int year = int.Parse(unconvertedTime.Substring(0, 4));
            int month = int.Parse(unconvertedTime.Substring(4, 2));
            int date = int.Parse(unconvertedTime.Substring(6, 2));
            int hours = int.Parse(unconvertedTime.Substring(8, 2));
            int minutes = int.Parse(unconvertedTime.Substring(10, 2));
            int seconds = int.Parse(unconvertedTime.Substring(12, 2));
            string meridian = "AM";
            /*if (hours &gt; 12)
            {
                hours -= 12;
                meridian = "PM";
            }
            */
            convertedTime = date.ToString() + "/" + month.ToString() + "/" + year.ToString() + " " +
                hours.ToString() + ":" + minutes.ToString() + ":" + seconds.ToString() + " " + meridian;
            return convertedTime;
        }

        private static string GetSoftwareElementState(int ses)
        {
            switch (ses)
            {
                case 0: return "Deployable";
                case 1: return "Installable";
                case 2: return "Executable";
                case 3: return "Running";
                default: return "Unknown";
            }
        }

        private static string GetTargetOperatingSystem(int tos)
        {
            switch (tos)
            {
                case 0: return "Unknown";
                case 1: return "Other";
                case 2: return "MACOS";
                case 3: return "ATTUNIX";
                case 4: return "DGUX";
                case 5: return "DECNT";
                case 6: return "Digital Unix";
                case 7: return "OpenVMS";
                case 8: return "HPUX";
                case 9: return "AIX";
                case 10: return "MVS";
                case 11: return "OS400";
                case 12: return "OS/2";
                case 13: return "JavaVM";
                case 14: return "MSDOS";
                case 15: return "WIN3x";
                case 16: return "WIN95";
                case 17: return "WIN98";
                case 18: return "WINNT";
                case 19: return "WINCE";
                case 20: return "NCR3000";
                case 21: return "NetWare";
                case 22: return "OSF";
                case 23: return "DC/OS";
                case 24: return "Reliant UNIX";
                case 25: return "SCO UnixWare";
                case 26: return "SCO OpenServer";
                case 27: return "Sequent";
                case 28: return "IRIX";
                case 29: return "Solaris";
                case 30: return "SunOS";
                case 31: return "U6000";
                case 32: return "ASERIES";
                case 33: return "TandemNSK";
                case 34: return "TandemNT";
                case 35: return "BS2000";
                case 36: return "LINUX";
                case 37: return "Lynx";
                case 38: return "XENIX";
                case 39: return "VM/ESA";
                case 40: return "Interactive UNIX";
                case 41: return "BSDUNIX";
                case 42: return "FreeBSD";
                case 43: return "NetBSD";
                case 44: return "GNU Hurd";
                case 45: return "OS9";
                case 46: return "MACH Kernel";
                case 47: return "Inferno";
                case 48: return "QNX";
                case 49: return "EPOC";
                case 50: return "IxWorks";
                case 51: return "VxWorks";
                case 52: return "MiNT";
                case 53: return "BeOS";
                case 54: return "HP MPE";
                case 55: return "NextStep";
                case 56: return "PalmPilot";
                case 57: return "Rhapsody";
                case 58: return "Windows 2000";
                case 59: return "Dedicated";
                case 60: return "VSE";
                case 61: return "TPF";
                default: return "Unknown";
            }
        }
    }
}