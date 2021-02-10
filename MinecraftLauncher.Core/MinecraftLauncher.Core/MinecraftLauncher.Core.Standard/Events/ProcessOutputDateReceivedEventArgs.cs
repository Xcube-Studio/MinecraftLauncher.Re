using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MinecraftLauncher.Core.Standard.Enums;
using System.Text.RegularExpressions;

namespace MinecraftLauncher.Core.Standard.Events
{
    public class ProcessOutputDateReceivedEventArgs
    {
        public string Data { get; set; }

        public ProcessOutputDateType ProcessOutputDateType { get; set; }

        public ProcessOutputDateReceivedEventArgs() { }

        public ProcessOutputDateReceivedEventArgs(string data)
        {
            this.Data = data;
            this.ProcessOutputDateType = GetType(data);
        }

        public ProcessOutputDateType GetType(string data)
        {
            if (data == null)
                return ProcessOutputDateType.Info;

            Regex regex = new Regex(@"(?i)(?<=\[)(.*)(?=\])");
            foreach (Match result in regex.Matches(data))
            {
                if (result.Value.ToLower().Contains("info"))
                    return ProcessOutputDateType.Info;
                else if (result.Value.ToLower().Contains("fatal"))
                    return ProcessOutputDateType.Fatal;
                else if (result.Value.ToLower().Contains("debug"))
                    return ProcessOutputDateType.Debug;
                else if (result.Value.ToLower().Contains("warn"))
                    return ProcessOutputDateType.Warn;
                else if (result.Value.ToLower().Contains("error"))
                    return ProcessOutputDateType.Error;
            }

            return ProcessOutputDateType.Last;
        }
    }
}
