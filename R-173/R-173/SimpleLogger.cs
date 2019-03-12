using System;
using System.IO;

namespace R_173
{
    public static class SimpleLogger
    {
        private static readonly object Lockobj = new object();

        public static void Log(string info)
        {
            lock (Lockobj)
            {
                File.AppendAllText("log.log", $"{DateTime.Now} {info}{Environment.NewLine}");
            }
        }

        public static void Log(Exception e)
        {
            lock (Lockobj)
            {
                File.AppendAllText("log.log", $"{DateTime.Now} {e}{Environment.NewLine}");
            }
        }
    }
}
