using System;
using System.Collections.Generic;
using System.Linq; 
using System.Text;
using System.Management;
using LogicNP.CryptoLicensing;
using System.Configuration;
using AllModules.License;

namespace AllModules
{
    public class CustomCryptoLicense : CryptoLicense
    {
        //Validation key for the project
        // Step How to get this key.
        // Open Crypto license,  
        // Load key from the LicenseTest Folder
        // Get the key from Project, get validation key & code
        private string validationKey = "AMAAMACezEPfdWRrVaaxUkgA80OXmcJULdKwqouhgNYBA621qnj9tiu9BdwIZn2EEWH4hk0DAAEAAQ=="; 
        private string localMachineCode = string.Empty;
        private bool licenseStatus = false;
         

        public string LicenseKey
        {
            get { return LicenseCode; }             
        }
        public bool LicenseValid
        {
            get { return licenseStatus; }
            //set { licenseStatus = value; }
        }

        public string LocalMachineCode
        {
            get { return GetLocalMachineCodeAsString(); }             
        }
        public CustomCryptoLicense()
        {
            //Get lincese key from database license table
            License.LicenseLogic ll = new AllModules.License.LicenseLogic();
            LicenseData ld = ll.GetLicense();
            
            this.ValidationKey = validationKey;
            this.LicenseCode = ld.LicenseKey.Trim();
            //licenseStatus = Status == LicenseStatus.Valid ? true : false;
            if (this.Status == LicenseStatus.Valid) // just licenseStatus Enum check
            {
                 licenseStatus= true;
            }
            else
            {
                 licenseStatus= false;
            }
             
        }
        // Override to provide custom machine code 
        // This example uses a combo of the CPU-ID and motherboard serial as the machine code 
        // The machine is considered matched if either of the two match 
        public override byte[] GetLocalMachineCode()
        {
            byte[] ret = null;
            try
            {
                ret = CombineBuffersWithLength(GetCPUId(), GetMBSerial());
            }
            catch { }

            // Fall back to base implementation if failed 
            if (ret == null || ret.Length == 2)
                ret = base.GetLocalMachineCode();

            return ret;
        }

        public override bool IsMachineCodeValid()
        {
            try
            {
                byte[] embeddedMachineCode = this.MachineCode;
                byte[] localMachineCode = this.GetLocalMachineCode();

                byte[] cpuid1, cpuid2, mbserial1, mbserial2;

                if (SplitBuffer(embeddedMachineCode, out cpuid1, out mbserial1) && SplitBuffer(localMachineCode, out cpuid2, out mbserial2))
                {
                    // Consider valid of either cpu-id or mb-serial matches 
                    if (AreBuffersEqual(cpuid1, cpuid2) && AreBuffersEqual(mbserial1, mbserial2))
                        return true;
                    else
                        return false;
                }
            }
            catch { }

            // Fall back to base implementation if failed 
            return base.IsMachineCodeValid();

        }

        // Gets the CPU-ID of the local machine 
        static internal byte[] GetCPUId()
        {
            try
            {
                ManagementClass mgmt = new ManagementClass("Win32_Processor");
                ManagementObjectCollection objCol = mgmt.GetInstances();
                foreach (ManagementObject obj in objCol)
                {
                    // Only use CPU-ID from the first CPU 
                    string cpuInfo = obj.Properties["ProcessorId"].Value.ToString();
                    if (cpuInfo != null && cpuInfo.Length > 0)
                    {
                        byte[] ret = Encoding.UTF8.GetBytes(cpuInfo);
                        return ret;
                    }
                }
            }
            catch { }

            return new byte[0];
        }

        static internal byte[] GetMBSerial()
        {
            try
            {
                ManagementClass mgmt = new ManagementClass("Win32_BaseBoard");
                ManagementObjectCollection objCol = mgmt.GetInstances();
                foreach (ManagementObject obj in objCol)
                {
                    // Only use CPU-ID from the first CPU 
                    string mbserial = obj.Properties["SerialNumber"].Value.ToString();
                    if (mbserial != null && mbserial.Length > 0)
                    {
                        byte[] ret = Encoding.UTF8.GetBytes(mbserial);
                        return ret;
                    }
                }
            }
            catch { }
            return new byte[0];
        }

        static byte[] CombineBuffersWithLength(byte[] buff1, byte[] buff2)
        {
            // Returned format is: 
            // buff1length-....buff1....-buff2length-...buff2.... 
            byte[] ret = new byte[buff1.Length + buff2.Length + 2];
            ret[0] = (byte)buff1.Length;
            buff1.CopyTo(ret, 1);
            ret[buff1.Length + 1] = (byte)buff2.Length;
            buff2.CopyTo(ret, buff1.Length + 2);

            return ret;
        }

        static bool AreBuffersEqual(byte[] buff1, byte[] buff2)
        {
            try
            {
                if (buff1.Length != buff2.Length)
                    return false;
                for (int i = 0; i < buff1.Length; i++)
                {
                    if (buff1[i] != buff2[i])
                        return false;

                    return true;
                }
            }
            catch { }

            return false;
        }

        // Gets machine code components 
        static bool SplitBuffer(byte[] buff, out byte[] buff1, out byte[] buff2)
        {
            buff1 = buff2 = null;

            try
            {
                byte buff1Length = buff[0];
                buff1 = new byte[buff1Length];
                Buffer.BlockCopy(buff, 1, buff1, 0, buff1Length);

                byte buff2Length = buff[buff1Length + 1];
                buff2 = new byte[buff2Length];
                Buffer.BlockCopy(buff, buff1Length + 2, buff2, 0, buff2Length);

                return true;
            }
            catch
            {
                buff1 = buff2 = null;
            }

            return false;
        }
    } 
}
