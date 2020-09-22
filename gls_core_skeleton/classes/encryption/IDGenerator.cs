using System;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
//using System.Management;

namespace appglobal
{
    public class IDGenerator
    {
        string HardwareID;
        string licenseID;
        string SerialNumber;
        String ActivationNumber;

        public string GenerateHWID()
        {
            string MB = getSystemSN("baseboard");
            string BS = getSystemSN("bios");
            string HWID = string.Empty;
            HWID = MB + BS;
            return HWID.Trim();
        }

        public void WriteLicenseToEncrypted(string LicenseID)
        {
            string location = AppGlobal.get_working_directory() + @"\note.dat";
            string content = GenerateHWID() + "\r\n" + LicenseID;
            CoreEncryptionHandler.ProcessTextToFile(content, location, "GLSystem", "Encrypt");
        }

        public void WriteSerialToEncrypted(string SerialNumber)
        {
            string location = AppGlobal.get_working_directory() + @"\seal.dat";
            string content = SerialNumber;
            CoreEncryptionHandler.ProcessTextToFile(content, location, "GLSystem", "Encrypt");
        }

        public string GetHWidFromFile()
        {
            string location = AppGlobal.get_working_directory() + @"\note.dat";
            string data;
            string[] ID;
            string curFile = AppGlobal.get_working_directory() + @"\\note.dat";  //Your path
            data = (File.Exists(curFile) ? "File exists." : "File does not exist.");
            if (data == "File exists.")
            {
                string decryptfile = CoreEncryptionHandler.ProcessFileToText(location, "GLSystem", "Decrypt");
                ID = Regex.Split(decryptfile, "\r\n");
                HardwareID = ID[0];
            }
            return HardwareID;
        }
        public string GetLicenseidFromFile()
        {
            string location = AppGlobal.get_working_directory() + @"\note.dat";
            string data;
            string[] ID;
            string curFile = AppGlobal.get_working_directory() + @"\\note.dat";  //Your path
            data = (File.Exists(curFile) ? "File exists." : "File does not exist.");
            if (data == "File exists.")
            {
                string decryptfile = CoreEncryptionHandler.ProcessFileToText(location, "GLSystem", "Decrypt");
                ID = Regex.Split(decryptfile, "\r\n");
                licenseID = ID[1];
            }
            return licenseID;
        }
        public string GetSerialNumberFromFile()
        {
            string location = AppGlobal.get_working_directory() + @"\seal.dat";
            string data;
            string curFile = AppGlobal.get_working_directory() + @"\\seal.dat";  //Your path
            data = (File.Exists(curFile) ? "File exists." : "File does not exist.");
            if (data == "File exists.")
            {
                string decryptfile = CoreEncryptionHandler.ProcessFileToText(location, "GLSystem", "Decrypt");
                SerialNumber = decryptfile;
            }
            return SerialNumber;
        }

        public string CalculateMD5Hash(string input)
        {
            // step 1, calculate MD5 hash from input
            MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
            byte[] hash = md5.ComputeHash(inputBytes);
            // step 2, convert byte array to hex string
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }
            return sb.ToString();
        }

        public String MD5SerialNumber()
        {
            string HWID = CalculateMD5Hash(GenerateHWID());
            string LCID = CalculateMD5Hash(GetLicenseidFromFile());
            string KodeAktivation = (HWID.Substring(0, 5)) + "-" + (LCID.Substring(0, 5)) + "-" + (HWID.Substring(6, 5)) + "-" + (LCID.Substring(6, 5));
            ActivationNumber = KodeAktivation;
            return ActivationNumber;
        }

        public string getSystemSN(string target = "bios")
        {
            string hasil = "";
            string command = "wmic " + target + " get serialnumber";
            System.Diagnostics.ProcessStartInfo procStartInfo = new System.Diagnostics.ProcessStartInfo("cmd", "/c " + command);

            procStartInfo.RedirectStandardOutput = true;
            procStartInfo.UseShellExecute = false;
            procStartInfo.CreateNoWindow = true;

            // wrap IDisposable into using (in order to release hProcess) 
            using (System.Diagnostics.Process process = new System.Diagnostics.Process())
            {
                process.StartInfo = procStartInfo;
                process.Start();

                // Add this: wait until process does its work
                process.WaitForExit();

                // and only then read the result
                hasil = process.StandardOutput.ReadToEnd();
            }

            return hasil.Replace("SerialNumber", "").Trim();
        }
    }
}