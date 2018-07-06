using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Net;
using System.Configuration;
using System.IO;
using System.Drawing.Drawing2D;
using System.Threading;
using System.Web;
using System.Text.RegularExpressions;


    /// <summary>
    /// ������־
    /// </summary>
    public class Logging
    {

    #region ��־����
    /// <summary>
    /// ������ͨ��־
    /// </summary>
    /// <param name="message"></param>
    public static void WriteWYlog(string filename, string message)
    {
        string logContent = string.Format(message);
        SetFile(filename + @"___΢ҽLog.xml", "�ӿ�����ʱ�䣺" + System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "����������" + logContent);
    }
    /// <summary>
    /// ������ͨ��־
    /// </summary>
    /// <param name="message"></param>
    public static void WriteHISlog(string filename, string message)
    {
        string logContent = string.Format(message);
        SetFile(filename + @"___HISLog.xml", "�ӿ�����ʱ�䣺" + System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "����������" + logContent);
    }
    /// <summary>
    /// ������ͨ��־
    /// </summary>
    /// <param name="message"></param>
    public static void WriteListlog(string filename, List<string[]> message)
        {
            List<string[]> logContent = message;
            SetFile(filename + @"WYLog.xml", Convert.ToString(logContent));
        }

        /// <summary>
        /// ����ؼ���־
        /// </summary>
        /// <param name="message"></param>
        public static void WriteKeylog(string filename,string message)
        {
            var logContent = string.Format(message);
            SetFile(filename + "KeyLog.xml", logContent);
        }
      
       
         /// <summary>
        /// ���������Ϣ��־
        /// </summary>
        /// <param name="ex"></param>
        public static void WriteBuglog(Exception ex)
        {
            var logContent = string.Format("[{0}]�������ڣ�{1}��\r\n ���ݣ�{2}",
                DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"), ex.Source, ex.Message);
            logContent += string.Format("\r\n [{0}] ���٣�{1}", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"),
                ex.StackTrace);
            SetFile(@"BugLog.txt", logContent);
        }
        #endregion

        #region ͨ�ò���
        /// <summary>
        /// ��׼��д����̣��̳�֮����Զ���д������
        /// Ĭ�ϱ�����debugĿ¼��LogĿ¼��
        /// </summary>
        /// <param name="filename">�ļ���</param>
        /// <param name="logContent">д������</param>
        protected static void SetFile(string filename, string logContent)
        {
            Isexist(); // �ж�LogĿ¼�Ƿ����
            string RootPath = GetRootPath();
            string errLogFilePath = RootPath + "\\"+filename.Trim();
            StreamWriter sw;
            if (!File.Exists(errLogFilePath))
            {
                FileStream fs1 = new FileStream(errLogFilePath, FileMode.Create, FileAccess.Write);
                sw = new StreamWriter(fs1);

            }
            else
            {
                sw = new StreamWriter(errLogFilePath, true);
            }
            sw.WriteLine(logContent);
            sw.Flush();
            sw.Close();
        }
        /// <summary>
        /// ȡ����վ��Ŀ¼������·��
        /// </summary>
        /// <returns></returns>
        public static string GetRootPath()
        {
            string AppPath = "";
            HttpContext HttpCurrent = HttpContext.Current;
            if (HttpCurrent != null)
            {
                AppPath = HttpCurrent.Server.MapPath("~");
            }
            else
            {
                AppPath = AppDomain.CurrentDomain.BaseDirectory;
                if (Regex.Match(AppPath, @"\\$", RegexOptions.Compiled).Success)
                    AppPath = AppPath.Substring(0, AppPath.Length - 1);
            }
            return AppPath+"Log";
        }
        // �ж��Ƿ������־�ļ�
        private static void Isexist()
        {
            string path = GetRootPath();
          //  string path = HttpRuntime.AppDomainAppVirtualPath + "Log";
            if (!File.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        
        #endregion
    }
