﻿using System;
using System.Security.Cryptography;
using System.Text;
using System.IO;



/// <summary>
/// Summary description for EncryptionUtility
/// </summary>
namespace EncryptionUtility
{


    public class Encryption
    {
        private static byte[] key = { };
        private static byte[] IV = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };
        private static string EncryptionKey = "!5623a#dervwe2585644&%$$()";

    public static string Decrypt(string Input)
    {
        Byte[] inputByteArray = new Byte[Input.Length];

        try
        {
            key = System.Text.Encoding.UTF8.GetBytes
            (EncryptionKey.Substring(0, 8));
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            inputByteArray = Convert.FromBase64String(Input);
          
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(key, IV), CryptoStreamMode.Write);

            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();

            Encoding encoding = Encoding.UTF8;

            return encoding.GetString(ms.ToArray());



        }

        catch (Exception ex)
        {
            return "";
        }

    }

    public static string Encrypt(string Input)
    {
        try
        {
            key = System.Text.Encoding.UTF8.GetBytes
            (EncryptionKey.Substring(0, 8));
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            Byte[] inputByteArray = Encoding.UTF8.GetBytes(Input);
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream
            (ms, des.CreateEncryptor(key, IV),CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            return Convert.ToBase64String(ms.ToArray());
            //return Convert.ToBase64String(ms.ToArray(), Base64FormattingOptions.None);
        }

        catch (Exception ex)
        {
            return "";
        }


    }
}



}
