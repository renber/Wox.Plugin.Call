using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Controls;
using Wox.Plugin.Call.Controls;
using Wox.Plugin.Call.Infrastructure;
using Wox.Plugin.Call.ViewModels;

namespace Wox.Plugin.Call
{
    public class CallPlugin : IPlugin, ISettingProvider
    {
        private const string callIcon = "Images/callIcon.png";
        private PluginJsonStorage<CallPluginSettings> storage = new PluginJsonStorage<CallPluginSettings>();
        CallPluginSettings settings;

        public void Init(PluginInitContext context)
        {
            storage = new PluginJsonStorage<CallPluginSettings>();
            settings = storage.Load();
        }

        public List<Result> Query(Query query)
        {
            var lst = new List<Result>();
            lst.Add(new Result
            {
                Title = String.Join(" ", query.Terms.Skip(1)) + " anrufen",
                SubTitle = String.Join(" ", query.Terms.Skip(1)) + " wählen",
                IcoPath = callIcon,
                Action = _ =>
                {
                    DoCall(new Entry("", String.Join(" ", query.Terms)));
                    return true;
                }
            });

            var contactResults = FindMatches(query.Terms.Skip(1).ToArray());
            lst.AddRange(contactResults.Select( entry => new Result
            {
                Title = entry.Name + " anrufen",
                SubTitle = entry.Number + " wählen",                 
                IcoPath = callIcon,
                Action = _ =>
                {
                    DoCall(entry);
                    return true;
                }
            }));
            return lst;
        }

        private IEnumerable<Entry> FindMatches(string[] searchTerms)
        {            
            return settings.Entries.Where(entry => Matches(entry, searchTerms)).OrderBy(x => x.Name).Take(5);
        }

        private bool Matches(Entry entry, string[] searchTerms)
        {            
            foreach(string s in searchTerms)
            {
                if (entry.Name.IndexOf(s, StringComparison.OrdinalIgnoreCase) < 0 && entry.Number.IndexOf(s, StringComparison.OrdinalIgnoreCase) < 0)
                    return false;
            }

            return true;
        }

        private void DoCall(Entry entry)
        {
            string what = settings.CallCommandTemplate;
            string command = what.Replace("{name}", HttpUtility.UrlEncode(entry.Name)).Replace("{number}", HttpUtility.UrlEncode(entry.Number));

            System.Diagnostics.Process.Start(command);
        }

        public Control CreateSettingPanel()
        {
            var control = new SettingsControl() { DataContext = new SettingsViewModel(settings) };
            control.Unloaded += Control_Unloaded;
            return control;
        }

        private void Control_Unloaded(object sender, System.Windows.RoutedEventArgs e)
        {
            (sender as Control).Unloaded -= Control_Unloaded;

            storage.Save();
        }
    }
}
