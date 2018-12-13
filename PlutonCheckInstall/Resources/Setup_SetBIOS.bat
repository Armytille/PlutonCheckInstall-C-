xcopy SetBIOS4Conf.dll %SYSTEMROOT%\system32\ /Y
xcopy PNSNProv.dll %SYSTEMROOT%\system32\ /Y
xcopy .\*.mof %SYSTEMROOT%\system32\ /Y
mofcomp %SYSTEMROOT%\system32\PNSNProv.mof
mofcomp %SYSTEMROOT%\system32\SetBIOS4Conf.mof
regsvr32 %SYSTEMROOT%\system32\PNSNProv.dll