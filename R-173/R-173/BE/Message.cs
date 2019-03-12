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

	    private const string Tab = "\t";

	    private IEnumerable<string> FormatMessage(int level)
        {
            if(!string.IsNullOrEmpty(Header))
                yield return GetTabs(level++) + Header;

            if (Messages == null)
            {
                yield break;
            }

            foreach (var message in Messages)
            {
                foreach (var item in message.FormatMessage(level))
                {
                    yield return item;
                }
            }
        }

        private static string GetTabs(int tabCount)
        {
            var result = string.Empty;
            for (var i = 0; i < tabCount; i++)
            {
                result += Tab;
            }
            return result;
        }
    }
}
