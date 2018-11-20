using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace R_173
{
    public static class SimpleLogger
    {
        private static object lockobj = new object();

        public static void Log(string info)
        {
            lock (lockobj)
            {
                File.AppendAllText("log.log", $"{DateTime.Now} {info}{Environment.NewLine}");
            }
        }

        public static void Log(Exception e)
        {
            lock (lockobj)
            {
                File.AppendAllText("log.log", $"{DateTime.Now} {e.ToString()}{Environment.NewLine}");
            }
        }
    }
}
