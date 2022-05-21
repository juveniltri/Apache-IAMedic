using System;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using System.Threading.Tasks;

namespace ApacheIAMedicClient
{
    public class CommonFuntions : BaseScript
    {
        public static void TriggerMessage(string type, string msg)
        {
            TriggerEvent("chat:addMessage", new
            {
                color = new[] { 255, 0, 0 },
                multiline = true,
                args = new[] { type, msg }
            });

        }

        public static async Task<bool> LoadModel(uint model)
        {
            if (!API.IsModelInCdimage(model))
            {
                Debug.WriteLine($"Invalid model {model} was supplied to LoadModel.");
                return false;
            }
            API.RequestModel(model);
            while (!API.HasModelLoaded(model))
            {
                Debug.WriteLine($"Waiting for model {model} to load");
                await BaseScript.Delay(100);
            }
            return true;
        }
    }
}
