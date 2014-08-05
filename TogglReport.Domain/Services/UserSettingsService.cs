using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using TogglReport.Domain.Model;

namespace TogglReport.Domain.Services
{
    public class UserSettingsService
    {
        #region Members

        private string _settingsFilePath = String.Empty;
        private const string _togglReportDataFile = "togglReportSettings.xml";
        private UserSettings _userSettings = new UserSettings();

        private const string ApiTokenKey = "ApiToken";

        #endregion

        #region Public Methods

        public void SaveApiToken(string apiToken)
        {
            SetSettingsFilePath();
            LoadUserSettings();

            AddApiTokenToUserSettings(apiToken);

            SaveUserSettings();
        }

        public string GetApiToken()
        {
            string apiToken = string.Empty;

            SetSettingsFilePath();
            LoadUserSettings();

            if (_userSettings != null && _userSettings.ContainsKey(ApiTokenKey))
                apiToken = _userSettings.GetValue(ApiTokenKey);

            return apiToken;
        }

        #endregion

        #region Private Methods

        private void SetSettingsFilePath()
        {
            string applicationDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            this._settingsFilePath = Path.Combine(applicationDataPath, _togglReportDataFile);
        }

        private void SaveUserSettings()
        {
            if (_userSettings != null && _userSettings.Count > 0)
            {
                using (Stream fs = new FileStream(_settingsFilePath, FileMode.Create))
                {
                    System.Xml.XmlWriter writer = new System.Xml.XmlTextWriter(fs, new System.Text.UTF8Encoding());
                    XmlSerializer xmlSerializer = new XmlSerializer(typeof(UserSettings));
                    xmlSerializer.Serialize(writer, _userSettings);
                    writer.Close();
                }
            }
        }

        private void AddApiTokenToUserSettings(string apiTokenValue)
        {
            if (_userSettings != null)
                _userSettings.Add(ApiTokenKey, apiTokenValue);
        }

        private void LoadUserSettings()
        {
            if (File.Exists(_settingsFilePath))
            {
                try
                {
                    XmlSerializer xmlSerializer = new XmlSerializer(typeof(UserSettings));
                    using (FileStream fileStream = new FileStream(_settingsFilePath, FileMode.Open))
                    {
                        _userSettings = (UserSettings)xmlSerializer.Deserialize(fileStream);
                    }
                    
                }
                catch (Exception)
                {
                }

            }
        }

        #endregion

    }
}
