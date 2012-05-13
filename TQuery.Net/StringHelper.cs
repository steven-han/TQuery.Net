using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace TQuery.Net
{
    public static class StringHelper
    {
        /// <summary>
        /// 字符串截断（基于字节数，一个中文字符＝２字节）
        /// </summary>
        /// <param name="text">源串</param>
        /// <param name="textLength">截断长度</param>
        /// <param name="appendDot">附加省略号</param>
        /// <returns></returns>
        public static string CutStringBytes(string text, int textLength, string appendDot)
        {
            int templ = 0;
            for (int i = 0; i < text.Length; i++)
            {
                char a = text[i];
                if ((int)a > 255)
                {
                    templ += 2;
                }
                else
                {
                    templ++;
                }
                if (templ == textLength)
                    return text.Substring(0, i + 1) + appendDot;
                else if (templ > textLength)
                    return text.Substring(0, i) + appendDot;
            }
            return text;
        }

        /// <summary>
        /// 判断是否为正确的固定电话号码
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        public static bool IsValidPhone(string phone)
        {
            //string pattern = @"^(\d{7,8}|(\(|\（)\d{3,4}(\)|\）)\d{7,8}|\d{3,4}(-?)\d{7,8})(((-|转)\d{1,9})?)$ ";
            if (string.IsNullOrEmpty(phone)) return false;
            string pattern = @"\b\d{7,16}";
            return Regex.Match(phone, pattern, RegexOptions.Compiled).Success;
        }

        /// <summary>
        /// 判断是否为正确的手机号
        /// </summary>
        /// <param name="mobile"></param>
        /// <returns></returns>
        public static bool IsValidMobile(string mobile)
        {
            if (string.IsNullOrEmpty(mobile)) return false;
            string pattern = @"\b1(3|5|8)\d{9}\b";
            return Regex.Match(mobile, pattern, RegexOptions.Compiled).Success;
        }

        /// <summary>
        /// 判断是否为正确的邮政编码
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static bool IsValidPostalCode(string code)
        {
            if (string.IsNullOrEmpty(code)) return false;
            string pattern = @"\b\d{6}\b";
            return Regex.Match(code, pattern, RegexOptions.Compiled).Success;
        }

        /// <summary>
        /// 判断是否为正确的Email地址
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public static bool IsValidEmail(string email)
        {
            if (string.IsNullOrEmpty(email)) return false;
            string pattern = @"\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*";
            //Regex reg = new Regex(pattern);
            //return reg.IsMatch(email);
            return Regex.Match(email, pattern, RegexOptions.Compiled).Success;
        }

        /// <summary>
        /// 是否中文字符串
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsCnString(string str)
        {
            if (string.IsNullOrEmpty(str)) return false;
            string pattern = @"[\u4e00-\uf900]";
            return Regex.Match(str, pattern, RegexOptions.Compiled).Success;
        }

        public static bool IsCnString(char ch)
        {
            return IsCnString(ch.ToString());
        }

        /// <summary>
        /// 是否是正确的IP
        /// </summary>
        /// <returns></returns>
        public static bool IsValidIP(string ip)
        {
            if (string.IsNullOrEmpty(ip)) return false;
            string pattern = @"^(2[0-5]{2}|2[0-4][0-9]|1?[0-9]{1,2}).(2[0-5]{2}|2[0-4][0-9]|1?[0-9]{1,2}).(2[0-5]{2}|2[0-4][0-9]|1?[0-9]{1,2}).(2[0-5]{2}|2[0-4][0-9]|1?[0-9]{1,2})$";
            return Regex.Match(ip, pattern, RegexOptions.Compiled).Success;
        }

        /// <summary>
        /// 验证指定字符串是否包含特殊字符
        /// </summary>
        /// <param name="str">待验证字符串</param>
        /// <returns>True：通过 False：失败</returns>
        public static bool VerifyString(string str)
        {
            try
            {
                if (string.IsNullOrEmpty(str)) return true;
                if (Regex.IsMatch(str.Trim(), "[~!@#$%&*':?/.\\|}{)(=]")) return false;
            }
            catch
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 转全角的函数(SBC case)
        /// </summary>
        /// <param name="input">任意字符串</param>
        /// <returns>全角字符串</returns>
        ///<remarks>
        ///全角空格为12288，半角空格为32
        ///其他字符半角(33-126)与全角(65281-65374)的对应关系是：均相差65248
        ///</remarks>        
        public static string ToSBC(string input)
        {
            if (string.IsNullOrEmpty(input)) return string.Empty;
            //半角转全角：
            char[] c = input.ToCharArray();
            for (int i = 0; i < c.Length; i++)
            {
                if (c[i] == 32)
                {
                    c[i] = (char)12288;
                    continue;
                }
                if (c[i] < 127)
                    c[i] = (char)(c[i] + 65248);
            }
            return new string(c);
        }

        /// <summary>
        /// 转半角的函数(DBC case)
        /// </summary>
        /// <param name="input">任意字符串</param>
        /// <returns>半角字符串</returns>
        ///<remarks>
        ///全角空格为12288，半角空格为32
        ///其他字符半角(33-126)与全角(65281-65374)的对应关系是：均相差65248
        ///</remarks>
        public static string ToDBC(string input)
        {
            if (string.IsNullOrEmpty(input)) return string.Empty;
            char[] c = input.ToCharArray();
            for (int i = 0; i < c.Length; i++)
            {
                if (c[i] == 12288)
                {
                    c[i] = (char)32;
                    continue;
                }
                if (c[i] > 65280 && c[i] < 65375)
                    c[i] = (char)(c[i] - 65248);
            }
            return new string(c);
        }

        private enum ChinaMoneyNumbers { 零, 壹, 贰, 叁, 肆, 伍, 陆, 柒, 捌, 玖 };
        private enum ChinaNumberChars { 个 = 1, 拾, 佰, 仟 }
        private enum ChinaMoneyUnits { 元 = 1, 万, 亿, 兆 };
        private enum ChinaMoneyDecimalUnits { 角, 分 };

        public static string MoneyToChineseCharacter(decimal d)
        {
            decimal flag = 0;
            if (d < 0)
            {
                flag = d;
                d = -d;
            }

            string strChinese = "";
            string nextString = "";

            System.Globalization.NumberFormatInfo fmat = new System.Globalization.NumberFormatInfo();
            fmat.CurrencyDecimalDigits = 2;
            fmat.CurrencySymbol = "";
            fmat.CurrencyGroupSizes = new int[] { 4, 4, 4, 4 };
            fmat.CurrencyGroupSeparator = ",";

            string strx = d.ToString("c", fmat);
            string[] afterArray = strx.ToString().Split('.');

            char[] prePoint = afterArray[0].ToCharArray();
            char[] nextChar = afterArray[1].ToCharArray();

            if (System.Convert.ToDecimal(afterArray[0].ToString()) == 0)
            { strChinese = ""; }
            else
            {
                string[] str = afterArray[0].ToString().Split(',');
                int Num = str.Length;
                //交错数组用来放四个一组的数组
                char[][] part = new char[Num][];
                for (int i = 0; i < str.Length; i++)
                {
                    part[i] = str[i].ToCharArray();
                }
                for (int i = 0; i < Num; i++)
                {
                    for (int j = 0; j < part[i].Length; j++)
                    {
                        //用枚举完成汉字的转换
                        strChinese += ((ChinaMoneyNumbers)int.Parse(part[i][j].ToString())).ToString();
                        //用枚举完成单位: 个 十 百 千 
                        strChinese += ((ChinaNumberChars)(part[i].Length - j)).ToString();
                    }
                    //以下为处理元 万 亿 兆 
                    strChinese += ((ChinaMoneyUnits)(part.Length - i)).ToString();
                }
            }
            //处理点号后面的小数部分        
            if (System.Convert.ToDecimal(afterArray[1].ToString()) == 0 && System.Convert.ToDecimal(afterArray[0].ToString()) != 0)
            {
                nextString = "整";
            }
            else
            {
                for (int i = 0; i < 2; i++)
                {
                    int t = int.Parse(nextChar[i].ToString());
                    nextString += ((ChinaMoneyNumbers)int.Parse(nextChar[i].ToString())).ToString();
                    nextString += ((ChinaMoneyDecimalUnits)(i)).ToString();
                    if (t == 0)
                    {
                        StringBuilder str = new StringBuilder(nextString);
                        nextString = str.Replace("零零", "零").ToString();
                        nextString = str.Replace("零角零分", "零元").ToString();
                    }
                }
            }
            StringBuilder sb = new StringBuilder(strChinese);
            for (int i = 0; i < 4; i++)
            {
                strChinese = sb.Replace("个", "").ToString();
                strChinese = sb.Replace("零元", "元").ToString();
                strChinese = sb.Replace("零万", "万").ToString();
                strChinese = sb.Replace("亿万", "亿").ToString();
                strChinese = sb.Replace("零亿", "亿").ToString();
                strChinese = sb.Replace("零十", "零").ToString();
                strChinese = sb.Replace("零百", "零").ToString();
                strChinese = sb.Replace("零千", "零").ToString();
                strChinese = sb.Replace("零零", "零").ToString();
                strChinese = sb.Replace("零角零分", "整").ToString();
            }
            if (flag >= 0)
            {
                return strChinese + nextString;
            }
            return "负" + strChinese + nextString;
        }

        /// <summary>
        /// 字符串是否数字
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsNumeric(string str)
        {
            Regex reg1 = new Regex(@"^(-?\d+)(\.\d{1,2})?$");
            return reg1.IsMatch(str);
        }
        /// <summary>
        /// 是否为日期型字符串
        /// </summary>
        /// <param name="StrSource">日期字符串(2008-05-08)</param>
        /// <returns></returns>
        public static bool IsDate(string StrSource)
        {
            return Regex.IsMatch(StrSource, @"^((((1[6-9]|[2-9]\d)\d{2})-(0?[13578]|1[02])-(0?[1-9]|[12]\d|3[01]))|(((1[6-9]|[2-9]\d)\d{2})-(0?[13456789]|1[012])-(0?[1-9]|[12]\d|30))|(((1[6-9]|[2-9]\d)\d{2})-0?2-(0?[1-9]|1\d|2[0-9]))|(((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))-0?2-29-))$");
        }

        /// <summary>
        /// 是否为时间型字符串
        /// </summary>
        /// <param name="source">时间字符串(15:00:00)</param>
        /// <returns></returns>
        public static bool IsTime(string StrSource)
        {
            return Regex.IsMatch(StrSource, @"^((20|21|22|23|[0-1]?\d):[0-5]?\d:[0-5]?\d)$");
        }

        /// <summary>
        /// 是否为日期+时间型字符串
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static bool IsDateTime(string StrSource)
        {
            return Regex.IsMatch(StrSource, @"^(((((1[6-9]|[2-9]\d)\d{2})-(0?[13578]|1[02])-(0?[1-9]|[12]\d|3[01]))|(((1[6-9]|[2-9]\d)\d{2})-(0?[13456789]|1[012])-(0?[1-9]|[12]\d|30))|(((1[6-9]|[2-9]\d)\d{2})-0?2-(0?[1-9]|1\d|2[0-8]))|(((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))-0?2-29-)) (20|21|22|23|[0-1]?\d):[0-5]?\d:[0-5]?\d)$ ");
        }

        #region 身份证验证
        /// <summary>
        /// 身份证验证
        /// </summary>
        /// <param name="cardId"></param>
        /// <returns></returns>
        public static bool IsIDCard(string cardId)//创建一个CheckCard方法用于检查身份证号码是否合法
        {
            if (cardId.Length == 18)        //如果身份证号为18位
            {
                return CheckCard18(cardId);//调用CheckCard18方法验证
            }
            else if (cardId.Length == 15)   //如果身份证号为15位
            {
                return CheckCard15(cardId);//调用CheckCard15方法验证
            }
            else
            {
                return false;
            }
        }

        private static bool CheckCard18(string CardId)//CheckCard18方法用于检查18位身份证号码的合法性
        {
            long n = 0;
            bool flag = false;
            if (long.TryParse(CardId.Remove(17), out n) == false || n < Math.Pow(10, 16) || long.TryParse(CardId.Replace('x', '0').Replace('X', '0'), out n) == false)
                return false;//数字验证
            string[] Myaddress = new string[]{ "11","22","35","44","53","12",
                "23","36","45","54","13","31","37","46","61","14","32","41",
                "50","62","15","33","42","51","63","21","34","43","52","64",
                "65","71","81","82","91"};
            for (int kk = 0; kk < Myaddress.Length; kk++)
            {
                if (Myaddress[kk].ToString() == CardId.Remove(2))
                {
                    flag = true;
                }
            }
            if (flag)
            {
                return flag;
            }
            string Mybirth = CardId.Substring(6, 8).Insert(6, "-").Insert(4, "-");
            DateTime Mytime = new DateTime();
            if (DateTime.TryParse(Mybirth, out Mytime) == false)
                return false;//生日验证
            string[] MyVarifyCode = ("1,0,x,9,8,7,6,5,4,3,2").Split(',');
            string[] wi = ("7,9,10,5,8,4,2,1,6,3,7,9,10,5,8,4,2").Split(',');
            char[] ai = CardId.Remove(17).ToCharArray();
            int sum = 0;
            for (int i = 0; i < 17; i++)
                sum += int.Parse(wi[i]) * int.Parse(ai[i].ToString());
            int y = -1;
            Math.DivRem(sum, 11, out y);
            if (MyVarifyCode[y] != CardId.Substring(17, 1).ToLower())
            {
                return false;//校验码验证
            }
            return true;//符合GB11643-1999标准
        }

        private static bool CheckCard15(string CardId)
        {
            long n = 0;
            bool flag = false;
            if (long.TryParse(CardId, out n) == false || n < Math.Pow(10, 14))
                return false;//数字验证
            string[] Myaddress = new string[]{ "11","22","35","44","53","12",
                "23","36","45","54","13","31","37","46","61","14","32","41",
                "50","62","15","33","42","51","63","21","34","43","52","64",
                "65","71","81","82","91"};
            for (int kk = 0; kk < Myaddress.Length; kk++)
            {
                if (Myaddress[kk].ToString() == CardId.Remove(2))
                {
                    flag = true;
                }
            }
            if (flag)
            {
                return flag;
            }
            string Mybirth = CardId.Substring(6, 6).Insert(4, "-").Insert(2, "-");
            DateTime Mytime = new DateTime();
            if (DateTime.TryParse(Mybirth, out Mytime) == false)
            {
                return false;//生日验证
            }
            return true;//符合15位身份证标准
        }
        #endregion
    }
}
