using System;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using System.Threading.Tasks;
using ApacheIAMedicClient.Models;
using Newtonsoft.Json;

namespace ApacheIAMedicClient
{
    public class Client : BaseScript
    {
        public static ConfigurationModel _configFile;
        public static LanguageModel _langFile;

        private static int gTimer;

        public Client()
        {
            LoadConfig();
            API.RegisterCommand("paramedic", new Action(this.CheckEMS), false);
            API.RegisterCommand("test", new Action(this.Test), false);

            Tick += OnTick;
        }

        private void CheckEMS()
        {
            TriggerServerEvent("IAMedic:Server:CheckServerEms", _configFile.EMSJobName);
        }

        private void LoadConfig()
        {
            try
            {
                _configFile = JsonConvert.DeserializeObject<ConfigurationModel>(API.LoadResourceFile(API.GetCurrentResourceName(), "Config/config.json"));
                _langFile = JsonConvert.DeserializeObject<LanguageModel>(API.LoadResourceFile(API.GetCurrentResourceName(), $"Config/{_configFile.Lang}.json"));
            }
            catch (Exception e)
            {

                Debug.WriteLine($"An error occurred while loading the config and lang file, error description: {e.Message}.");
            }
        }

        private async Task OnTick()
        {
            if (API.GetGameTimer() - gTimer >= 1000)
            {
                gTimer = API.GetGameTimer();

                API.EnableDispatchService(5, false);
                IAMedic.Loop();
            }
        }

        [EventHandler("ApacheIAMedic:StartHealth")]
        private void StartHealth(int maxEMS)
        {
            Debug.WriteLine(maxEMS.ToString() + _configFile.NumOfEMS);

            if (Game.Player.Character.Health < 0 && maxEMS < _configFile.NumOfEMS)
            {
                IAMedic.Summon();
            }
            else
            {
                CommonFuntions.TriggerMessage(_langFile.type, _langFile.notDead);
            }
            
        }

        private void Test()
        {
            Debug.WriteLine(Game.Player.Character.Health.ToString());
        }
    }
}
