using System;
using Microsoft.Win32;

namespace RH
{
    public class RegistryHelpers
    {
        public static RegistryKey GetRegistryKey(string hive_HKLM_or_HKCU)
        {
            return GetRegistryKey(null, hive_HKLM_or_HKCU);
        }
        public static RegistryKey GetRegistryKey(string keyPath, string hive_HKLM_or_HKCU)
        {
            RegistryKey MachineRegistry;
                          
            switch (hive_HKLM_or_HKCU.ToUpper())
            {
                case "HKLM":
                    MachineRegistry = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine,
                                      Environment.Is64BitOperatingSystem
                                          ? RegistryView.Registry64
                                          : RegistryView.Registry32);
                    break;
                case "HKCU":
                    MachineRegistry = RegistryKey.OpenBaseKey(RegistryHive.CurrentUser,
                                      Environment.Is64BitOperatingSystem
                                          ? RegistryView.Registry64
                                          : RegistryView.Registry32);
                    break;
                default:
                    throw new InvalidOperationException("parameter registryRoot must be either \"HKLM\" or \"HKCU\"");
            }
        

            return string.IsNullOrEmpty(keyPath)
                ? MachineRegistry
                : MachineRegistry.OpenSubKey(keyPath);
        }

        public static object GetRegistryValue(string keyPath, string keyName, string hive_HKLM_or_HKCU)
        {
            RegistryKey registry = GetRegistryKey(keyPath, hive_HKLM_or_HKCU);
            var valueKind = registry.GetValueKind(keyName);
            if (valueKind == RegistryValueKind.Binary) 
            {
                var value = (byte[])registry.GetValue(keyName);
                return BitConverter.ToString(value);
            }
            return registry.GetValue(keyName);
        }

    }
}
