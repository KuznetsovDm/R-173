using System;
using System.Collections.Generic;

namespace R_173.BE
{
    public class Message
    {
        public string Header { get; set; }
        public IEnumerable<Message> Messages { get; set; }

        public override string ToString()
        {
            return string.Join(Environment.NewLine, FormatMessage(0));
        }

        private static readonly string _tab = "\t";

        private IEnumerable<string> FormatMessage(int level)
        {
            if(!string.IsNullOrEmpty(Header))
                yield return GetTabs(level) + Header;

            if (Messages == null)
            {
                yield break;
            }

            foreach (var message in Messages)
            {
                foreach (var item in message.FormatMessage(level + 1))
                {
                    yield return item;
                }
            }
        }

        private static string GetTabs(int tabCount)
        {
            var result = string.Empty;
            for (int i = 0; i < tabCount; i++)
            {
                result += _tab;
            }
            return result;
        }
    }
}
