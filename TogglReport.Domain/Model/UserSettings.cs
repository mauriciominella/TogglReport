using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TogglReport.Domain.Model
{
    public class UserSettings : List<Setting>
    {
        #region Public Methods

        public bool ContainsKey(string key)
        {
            return this.GetKeyIndex(key) > -1;
        }

        public void Add(string key, string value)
        {
            if (!this.ContainsKey(key))
                this.Add(new Setting() { Key = key, Value = value });
            else
                this[this.GetKeyIndex(key)] = new Setting() { Key = key, Value = value };
        }

        public string GetValue(string key)
        {
            string value = String.Empty;

            if (this.ContainsKey(key))
                value = this[this.GetKeyIndex(key)].Value;

            return value;
        }

        #endregion

        #region Private Methods

        private int GetKeyIndex(string key)
        {
            return this.FindIndex(i => i.Key == key);
        }

        #endregion

    }
}
