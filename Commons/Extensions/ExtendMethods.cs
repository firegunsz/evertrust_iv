using DocumentFormat.OpenXml.Office2010.ExcelAc;
using EvoPdf;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace OnlineManager.Commons.Extensions
{
    public class ExtendMethods       
    {
        private readonly string AES256Key;
        private readonly IConfiguration conf;
        public ExtendMethods(IConfiguration configuration) 
        {
            conf = configuration;
            AES256Key = GetMD5(configuration.GetValue<string>("AES:Key"));
        }

        public string AES256Encrypt(string strData)
        {
            return AES256Encrypt(AES256Key, strData);
        }
    
        public static string GetMD5(string original)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] b = md5.ComputeHash(Encoding.UTF8.GetBytes(original));
            return BitConverter.ToString(b).Replace("-", string.Empty).ToLower();
        }
        public string AES256Encrypt(string strKey, string strData)
        {
            byte[] sourceBytes = UTF8Encoding.UTF8.GetBytes(strData);
            byte[] byte_pwdMD5 = UTF8Encoding.UTF8.GetBytes(strKey);


            RijndaelManaged rDel = new RijndaelManaged();
            rDel.KeySize = 256;
            rDel.BlockSize = 128;
            rDel.Key = byte_pwdMD5;
            rDel.Mode = CipherMode.ECB;
            rDel.Padding = PaddingMode.PKCS7;
            ICryptoTransform cTransform = rDel.CreateEncryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(sourceBytes, 0, sourceBytes.Length);
            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }
        /// <summary>
        /// 將民國年轉成西元年格式
        /// </summary>    
        /// <returns>string</returns>
        public static string YearMToC(string date)
        {
            if (string.IsNullOrEmpty(date)) return date;
            date = date.Trim();
            try
            {
                if (date.Length < 3) return date;
                string yy = "";
                string tmp = "";
                yy = date.Substring(0, 3);
                tmp = date.Substring(3);
                date = Convert.ToString(Convert.ToInt32(yy) + 1911) + tmp;
                return date;

            }
            catch (Exception e)
            {
                return date;
            }

        }
        /// <summary>
        /// 將西元年轉成民國年格式
        /// </summary>    
        /// <returns>string</returns>
        public static string YearCToM(string date)
        {
            date = date.Trim();
            if (date == "") return date;
            try
            {
                if (date.Length < 4) return date;
                string yy = "";
                string tmp = "";
                yy = date.Substring(0, 4);
                tmp = date.Substring(4);
                date = Convert.ToString(Convert.ToInt32(yy) - 1911).PadLeft(3, '0') + tmp;
                return date;
            }
            catch (Exception e)
            {
                return date;
            }


        }
        /// <summary>
        /// 移除特定字元
        /// </summary>    
        /// <param name="sp">要移除的字元</param>
        /// <returns>string</returns>
        public static string StripSp(string date, string sp)
        {
            date = date.Trim();
            if (date == "") return date;
            try
            {
                while (date.IndexOf(sp) >= 0)
                {
                    date = date.Remove(date.IndexOf(sp), sp.Length);
                }
                return date;
            }
            catch (Exception e)
            {
                return date;
            }


        }
        /// <summary>
        /// 年份格式化
        /// </summary>    
        /// <param name="sp">分隔符號</param>
        /// <param name="type">民國年或西元年</param>
        /// <returns>string</returns>
        public static string toYearFormat(string date, string sp, string type)
        {
            if (string.IsNullOrEmpty(sp)) return date;
            if (string.IsNullOrEmpty(date)) return date;
            date = date.Trim();
            try
            {
                if (type == "M")
                {
                    if (date.Length == 6)
                    {
                        date = date.Insert(2, sp);
                        date = date.Insert(5, sp);
                    }
                    else if (date.Length == 7)
                    {
                        date = date.Insert(3, sp);
                        date = date.Insert(6, sp);
                    }
                    else return date;
                }
                else if (type == "C")
                {
                    if (date.Length >= 8)
                    {
                        date = date.Insert(4, sp);
                        date = date.Insert(7, sp);
                    }
                }
                return date;
            }
            catch (Exception e)
            {
                return date;
            }

        }

        public  string CreatePDF(string mailcontent, string pwd = "")
        {
            string tmpFolder = Path.Combine(Path.Combine(AppDomain.CurrentDomain.BaseDirectory,conf.GetSection("TmpDir:CreatePDFDir").Value ), DateTime.Now.ToString("ss"));
            string tmpfile = tmpFolder + DateTime.Now.ToString("ss") + ".tmp";

            Margins mgs = new Margins(20f, 20f, 0f, 5f);


            if (System.IO.File.Exists(tmpfile))
                System.IO.File.Delete(tmpfile);

            if (!System.IO.Directory.Exists(tmpFolder))
                System.IO.Directory.CreateDirectory(tmpFolder);


            //http://www.html-to-pdf.net/ExpertPDF-HtmlToPdf-Converter.aspx

            Document document = null;
            try
            {
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    document = new Document();
                    if (pwd != null && pwd != "")
                        document.Security.UserPassword = pwd;
                    document.LicenseKey = "rSMwIjcyIjM6IjQsMiIxMywzMCw7Ozs7IjI =";
                    //document.LicenseKey = "tp2EloWFloeDhpaEmIaWhYeYh4SYj4+Pjw==";

                    //document.CompressionLevel = 1;
                    document.Margins = new Margins(10f, 10f, 0f, 0f);
                    document.DocumentInformation.Author = "CathaySite";
                    document.ViewerPreferences.HideToolbar = false;
                    //document.AddFont(

                    PdfPage pdfPage = document.Pages.AddNewPage(PdfPageSize.A4, mgs, 0);
                    //------------------------------------------------------------------------                    
                    HtmlToPdfElement htmlToPdfElement = new HtmlToPdfElement(mailcontent, null);
                    htmlToPdfElement.FitWidth = true;
                    htmlToPdfElement.JavaScriptEnabled = true;
                    htmlToPdfElement.EmbedFonts = true;
                    htmlToPdfElement.LiveUrlsEnabled = true;
                    htmlToPdfElement.NavigationTimeout = 120000;
                    AddElementResult addElementResult = pdfPage.AddElement(htmlToPdfElement);
                    document.Save(tmpfile);
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                if (document != null)
                {
                    document.Close();
                }
            }
            return tmpfile;
        }
        /// <summary>
        /// CheckBox選取項目數值
        /// </summary>    
        /// <param name="removelastsp">移除最後一分隔號</param>
        /// <returns>string</returns>
        public static string CheckListVal(List<string> ck, Boolean removelastsp = false)
        {
            try
            {
                string val = "";
                if (ck == null) return val;
                if (ck.Count > 0)
                {
                    foreach (var s in ck)
                    {
                        var _s = s;
                        if(s == null) { _s = ""; }
                        if (removelastsp)
                        {
                            if (val != "") val += ",";
                            val += _s.Trim();
                        }
                        else
                        {
                            val += _s.Trim() + ",";
                        }
                    }
                }
                return val;
            }
            catch
            {
                return "";
            }


        }
        /// <summary>
        /// 格式化Currency
        /// </summary>    
        /// <param name="num">小數位數</param>
        /// <returns>string</returns>
        public static string CurrencyFormat(string currency, int num = 0)
        {
            currency = currency.Trim();
            if (currency == "") return currency;
            try
            {
                string strNumCount = string.Empty;
                for (int i = 0; i < num; i++)
                    strNumCount += "0";
                currency = String.Format("{0:#,0." + strNumCount + "}", Convert.ToDecimal(currency));
                //string scurrency = String.Format("{0:N" + Convert.ToString(num) + "}", Convert.ToDecimal(currency));
            }
            catch
            {

            }


            return currency;
        }
        /// <summary>
        /// 取得現在日期
        /// </summary>    
        /// <param name="format">西元年或民國年</param>
        /// <param name="sp">分隔符號</param>
        /// <returns>string</returns>
        public static string GetDate(string date, string format = "C", string sp = "")
        {
            DateTime dt = DateTime.Now;
            try
            {
                if (format == "C")
                {
                    date = dt.Year.ToString() + sp + dt.Month.ToString().PadLeft(2, '0') + sp + dt.Day.ToString().PadLeft(2, '0');
                }
                else if (format == "M")
                {
                    date = Convert.ToString(dt.Year - 1911).PadLeft(3, '0') + sp + dt.Month.ToString().PadLeft(2, '0') + sp + dt.Day.ToString().PadLeft(2, '0');
                }

                return date;
            }
            catch (Exception e)
            {
                return date;
            }

        }
    }
}
