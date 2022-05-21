using CitizenFX.Core;
using System;
using System.Collections.Generic;
using static CitizenFX.Core.Native.API;

namespace ApacheIAMedicServer
{
    public class Server : BaseScript
    {
        private dynamic ESX;
        private static PlayerList playerList = new PlayerList();
        private static int offDutyEms;

        public Server()
        {
            TriggerEvent("esx:getSharedObject", new object[] {
                new Action<dynamic>(esx =>
                {
                    this.ESX = esx;
                })
            });

            this.EventHandlers.Add("IAMedic:Server:CheckServerEms", new Action<string>(this.CheckServerEms));
        }

        private void CheckServerEms(string jobName)
        {

            foreach (var pls in playerList)
            {
                dynamic player = this.ESX.GetPlayerFromId(pls.Handle);
                dynamic job = player != null ? player.getJob() : null;

                if ( job.name == jobName)
                {
                    offDutyEms ++;
                }
            }
            TriggerClientEvent("ApacheIAMedic:StartHealth", offDutyEms);
        }
    }
}
