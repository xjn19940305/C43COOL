using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace C43COOL.Infrastructure
{
    public static class ToolsHelper
    {
        /// <summary>
        /// 进行MD5加密
        /// </summary>
        /// <param name="md5"></param>
        /// <returns></returns>
        public static string Get32Md5(this string md5)
        {
            System.Security.Cryptography.MD5CryptoServiceProvider md = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] value, hash;
            value = System.Text.Encoding.UTF8.GetBytes(md5);
            hash = md.ComputeHash(value);
            md.Clear();
            string temp = "";
            for (int i = 0, len = hash.Length; i < len; i++)
            {
                temp += hash[i].ToString("X").PadLeft(2, '0');
            }
            return temp;
        }

        /// <summary>
        /// md5加密加盐并且可以指定md5几次
        /// </summary>
        /// <param name="content"></param>
        /// <param name="Salt"></param>
        /// <param name="loop"></param>
        /// <returns></returns>
        public static string GetMd5WithSalt(this string content, string Salt, int loop = 1)
        {
            var data = Get32Md5(content + Salt);
            for (var i = 1; i < loop; i++)
            {
                data = Get32Md5(data);
            }
            return data;
        }
        public static string GetDescription(this Enum enumName)
        {
            string description;
            FieldInfo fieldInfo = enumName.GetType().GetField(enumName.ToString());
            DescriptionAttribute[] attributes = fieldInfo.GetCustomAttributes<DescriptionAttribute>().ToArray();
            if (attributes != null && attributes.Length > 0)
                description = attributes[0].Description;
            else
                return string.Empty;
            return description;
        }
    }
}
