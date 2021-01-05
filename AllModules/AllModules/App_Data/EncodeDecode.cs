using System;
using System.Collections.Generic;
using System.Configuration.Provider;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AllModules
{
    //CLASS IS UNUSED
    public partial class EncodeDecode 
    {
        private MembershipPasswordFormat PasswordFormat;         
        private string ppassPhrase;
        private string psaltValue;
        private string phashAlgorithm;
        private string phashKey;
        private int ppasswordIterations;
        private string pinitVector;
        private int pkeySize;

       
        public string PassPhrase
        {
            get { return ppassPhrase; }
            set { ppassPhrase = value; }
        }

        public string SaltValue
        {
            get { return psaltValue; }
            set { psaltValue = value; }
        }

        public string HashAlgorithm
        {
            get { return phashAlgorithm; }
            set { phashAlgorithm = value; }
        }

        public string HashKey
        {
            get { return phashKey; }
            set { phashKey = value; }
        }

        public string InitVector
        {
            get { return pinitVector; }
            set { pinitVector = value; }
        }

        public int PasswordIterations
        {
            get { return ppasswordIterations; }
            set { ppasswordIterations = value; }
        }

        public int KeySize
        {
            get { return pkeySize; }
            set { pkeySize = value; }
        }
        //protected void Page_Load(object sender, EventArgs e)
        //{
        //}

        public EncodeDecode()
        {
            PasswordFormat = MembershipPasswordFormat.Encrypted;
            HashKey = "AllModules";
            InitVector = "@1B2c3D4e5F6g7H8";
            SaltValue = "AllModules";
            KeySize = 256;
            PassPhrase = "AllModules";
            HashAlgorithm = "SHA1";
            PasswordIterations =  2 ;
            
        }

        public string GetProperConnectionString(string connString)
        {
            //http://weblogs.asp.net/jeevanmurkoth/exception-cannot-start-your-application-the-workgroup-information-file-is-missing-or-opened-exclusively-by-another-user
            int lastIndex = connString.LastIndexOf("Password=");
            lastIndex += 9;         
            string encodedPassword = connString.Substring(lastIndex, connString.Length - 1 - lastIndex);//takes care semicolon at end
            connString = connString.Substring(0, lastIndex);
            connString += UnEncodePassword(encodedPassword);
            return connString;
        }

        #region EncodePassword

        //
        // EncodePassword
        // Encrypts, Hashes, or leaves the password clear based on the PasswordFormat.
        //

        public string EncodePassword(string password)
        {
            string encodedPassword = password;
            if (PasswordFormat == MembershipPasswordFormat.Clear)
            {
            }
            else if (PasswordFormat == MembershipPasswordFormat.Encrypted)
            {
                encodedPassword = Encrypt(password);
            }
            else if (PasswordFormat == MembershipPasswordFormat.Hashed)
            {
                HMACSHA1 hash = new HMACSHA1();
                hash.Key = HexToByte((string)HashKey);
                encodedPassword = (string)(Convert.ToBase64String(hash.ComputeHash(Encoding.Unicode.GetBytes(password))));
            }
            else
            {
                throw (new ProviderException("Unsupported password format."));
            }
            return encodedPassword;
        }

        #endregion

        #region UnEncodePassword

        //
        // UnEncodePassword
        // Decrypts or leaves the password clear based on the PasswordFormat.
        //

        private string UnEncodePassword(string encodedPassword)
        {
            string password = encodedPassword;
            if (PasswordFormat == MembershipPasswordFormat.Clear)
            {
            }
            else if (PasswordFormat == MembershipPasswordFormat.Encrypted)
            {
                password = Decrypt(password);
            }
            else if (PasswordFormat == MembershipPasswordFormat.Hashed)
            {
                throw (new ProviderException("Cannot unencode a hashed password."));
            }
            else
            {
                throw (new ProviderException("Unsupported password format."));
            }
            return password;
        }
        #endregion
        #region Encrypt

        private string Encrypt(string plainText)
    {
        string returnValue = default(string);
        byte[] initVectorBytes = null;
        initVectorBytes = Encoding.ASCII.GetBytes(InitVector);
        byte[] saltValueBytes = null;
        saltValueBytes = Encoding.ASCII.GetBytes(SaltValue);
        byte[] plainTextBytes = null;
        plainTextBytes = Encoding.UTF8.GetBytes(plainText);
        PasswordDeriveBytes password = default(PasswordDeriveBytes);
        password = new PasswordDeriveBytes(PassPhrase, saltValueBytes, HashAlgorithm, PasswordIterations);
        byte[] keyBytes = null;
        keyBytes = password.GetBytes(KeySize / 8);
        RijndaelManaged symmetricKey = default(RijndaelManaged);
        symmetricKey = new RijndaelManaged();
        // It is reasonable to set encryption mode to Cipher Block Chaining
        // (CBC). Use default options for other symmetric key parameters.
        symmetricKey.Mode = CipherMode.CBC;
        // Generate encryptor from the existing key bytes and initialization
        // vector. Key size will be defined based on the number of the key
        // bytes.
        ICryptoTransform encryptor = default(ICryptoTransform);
        encryptor = symmetricKey.CreateEncryptor(keyBytes, initVectorBytes);
        // Define memory stream which will be used to hold encrypted data.
        System.IO.MemoryStream memoryStream = default(System.IO.MemoryStream);
        memoryStream = new System.IO.MemoryStream();
        // Define cryptographic stream (always use Write mode for encryption).
        CryptoStream cryptoStream = default(CryptoStream);
        cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);
        // Start encrypting.
        cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
        // Finish encrypting.
        cryptoStream.FlushFinalBlock();
        // Convert our encrypted data from a memory stream into a byte array.
        byte[] cipherTextBytes = null;
        cipherTextBytes = memoryStream.ToArray();
        // Close both streams.
        memoryStream.Close();
        cryptoStream.Close();
        // Convert encrypted data into a base64-encoded string.
        string cipherText = default(string);
        cipherText = (string)(Convert.ToBase64String(cipherTextBytes));
        // Return encrypted string.
        returnValue = cipherText;
        return returnValue;
    }

    #endregion

    #region Decrypt

    private string Decrypt(string cipherText)
    {
        string returnValue = default(string);
        // Convert strings defining encryption key characteristics into byte
        // arrays. Let us assume that strings only contain ASCII codes.
        // If strings include Unicode characters, use Unicode, UTF7, or UTF8
        // encoding.
        byte[] initVectorBytes = null;
        initVectorBytes = Encoding.ASCII.GetBytes(InitVector);
        byte[] saltValueBytes = null;
        saltValueBytes = Encoding.ASCII.GetBytes(SaltValue);
        // Convert our ciphertext into a byte array.
        byte[] cipherTextBytes = null;
        cipherTextBytes = Convert.FromBase64String(cipherText);
        // First, we must create a password, from which the key will be
        // derived. This password will be generated from the specified
        // passphrase and salt value. The password will be created using
        // the specified hash algorithm. Password creation can be done in
        // several iterations.
        PasswordDeriveBytes password = default(PasswordDeriveBytes);
        password = new PasswordDeriveBytes(PassPhrase, saltValueBytes, HashAlgorithm, PasswordIterations);
        // Use the password to generate pseudo-random bytes for the encryption
        // key. Specify the size of the key in bytes (instead of bits).
        byte[] keyBytes = null;
        keyBytes = password.GetBytes(KeySize / 8);
        // Create uninitialized Rijndael encryption object.
        RijndaelManaged symmetricKey = default(RijndaelManaged);
        symmetricKey = new RijndaelManaged();
        // It is reasonable to set encryption mode to Cipher Block Chaining
        // (CBC). Use default options for other symmetric key parameters.
        symmetricKey.Mode = CipherMode.CBC;
        // Generate decryptor from the existing key bytes and initialization
        // vector. Key size will be defined based on the number of the key
        // bytes.
        ICryptoTransform decryptor = default(ICryptoTransform);
        decryptor = symmetricKey.CreateDecryptor(keyBytes, initVectorBytes);
        // Define memory stream which will be used to hold encrypted data.
        System.IO.MemoryStream memoryStream = default(System.IO.MemoryStream);
        memoryStream = new System.IO.MemoryStream(cipherTextBytes);
        // Define memory stream which will be used to hold encrypted data.
        CryptoStream cryptoStream = default(CryptoStream);
        cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
        // Since at this point we don't know what the size of decrypted data
        // will be, allocate the buffer long enough to hold ciphertext;
        // plaintext is never longer than ciphertext.
        byte[] plainTextBytes = null;
        plainTextBytes = new byte[cipherTextBytes.Length + 1];
        // Start decrypting.
        int decryptedByteCount = default(int);
        decryptedByteCount = System.Convert.ToInt32(cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length));
        // Close both streams.
        memoryStream.Close();
        cryptoStream.Close();
        // Convert decrypted data into a string.
        // Let us assume that the original plaintext string was UTF8-encoded.
        string plainText = default(string);
        plainText = (string)(Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount));
        // Return decrypted string.
        returnValue = plainText;
        return returnValue;
    }
    #endregion

    #region HexToByte

    //
    // HexToByte
    // Converts a hexadecimal string to a byte array. Used to convert encryption
    // key values from the configuration.
    //

    private byte[] HexToByte(string hexString)
    {
        byte[] ReturnBytes = new byte[(hexString.Length / 2) - 1 + 1];
        for (int i = 0; i <= ReturnBytes.Length - 1; i++)
        {
            ReturnBytes[i] = System.Convert.ToByte(Convert.ToByte(hexString.Substring(i * 2, 2), 16));
        }
        return ReturnBytes;
    }

    #endregion
    }
}