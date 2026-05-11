using System;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using System.IO;
using System.Linq;

namespace SunshineFreezer
{
    public class HistoryRecord
    {
        public string Time { get; set; }
        public string ProcessName { get; set; }
        public int Pid { get; set; }
        public string Action { get; set; } // "冻结" 或 "解冻"
    }

    public class AppSettings
    {
        // 原有配置项
        public string text1 { get; set; } = "ctrl+b";
        public string text2 { get; set; } = "ctrl+m";
        public string text3 { get; set; } = "48000";
        public string text4 { get; set; } = "3";
        public string text5 { get; set; } = "0";
        public string text6 { get; set; } = "0";
        public string text7 { get; set; } = "0";
        public string text8 { get; set; } = "120";
        public string text9 { get; set; } = "0";
        public string text10 { get; set; } = "0";
        public string text11 { get; set; } = "输入冻结时的命令";
        public string text12 { get; set; } = "输入解冻时的命令";
        public string text13 { get; set; } = "1";
        public string text14 { get; set; } = "0";
        public string text15 { get; set; } = "0";

        // 新增：历史记录（最多30条）
        public List<HistoryRecord> History { get; set; } = new List<HistoryRecord>();

        // 新增：白名单（进程名列表）
        public List<string> Whitelist { get; set; } = new List<string> { "explorer", "svchost", "csrss", "lsass", "services", "winlogon", "SunshineFreezer", "StartMenuExperienceHost" };

        private const int MaxHistoryCount = 30;
        private const string ConfigFile = "1.json";

        public static AppSettings Load()
        {
            if (File.Exists(ConfigFile))
            {
                try
                {
                    string json = File.ReadAllText(ConfigFile);
                    var serializer = new JavaScriptSerializer();
                    return serializer.Deserialize<AppSettings>(json) ?? new AppSettings();
                }
                catch
                {
                    return new AppSettings();
                }
            }
            return new AppSettings();
        }

        public void Save()
        {
            var serializer = new JavaScriptSerializer();
            string json = serializer.Serialize(this);
            File.WriteAllText(ConfigFile, json);
        }

        public void AddHistoryRecord(string processName, int pid, string action)
        {
            History.Add(new HistoryRecord
            {
                Time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                ProcessName = processName,
                Pid = pid,
                Action = action
            });

            // 保持最多30条记录
            if (History.Count > MaxHistoryCount)
            {
                History = History.Skip(History.Count - MaxHistoryCount).ToList();
            }

            Save();
        }

        public bool IsInWhitelist(string processName)
        {
            if (string.IsNullOrEmpty(processName))
                return false;

            return Whitelist.Any(w => w.Equals(processName, StringComparison.OrdinalIgnoreCase));
        }

        public void AddToWhitelist(string processName)
        {
            if (!string.IsNullOrEmpty(processName) && !IsInWhitelist(processName))
            {
                Whitelist.Add(processName);
                Save();
            }
        }

        public void RemoveFromWhitelist(string processName)
        {
            Whitelist.RemoveAll(w => w.Equals(processName, StringComparison.OrdinalIgnoreCase));
            Save();
        }
    }
}
