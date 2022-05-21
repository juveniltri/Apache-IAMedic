using CitizenFX.Core;
using CitizenFX.Core.Native;
using CitizenFX.Core.UI;
using System;
using System.Threading.Tasks;

namespace ApacheIAMedicClient
{
    class IAMedic : BaseScript
    {
        private static Vehicle cAmbulance;
        private static Ped cParamedic1;
        private static Ped cParamedic2;
        private static int cIAMedicBlip;

        private static Vector3 targetLocation = new Vector3();
        private static Vector3 spawnLocation = new Vector3();
        private static float spawnHeading = 0F;
        private static int unusedVar = 0;

        private static bool eventSpawned = false;
        private static bool eventOnScene;

        public async static void Loop()
        {
            Ped player = Game.Player.Character;

            if (!eventOnScene && API.GetDistanceBetweenCoords(cAmbulance.Position.X, cAmbulance.Position.Y, cAmbulance.Position.Z, targetLocation.X, targetLocation.Y, targetLocation.Z, true) < 10F)
            {
                eventOnScene = true;

                cParamedic1.Task.ClearAllImmediately();
                API.SetVehicleForwardSpeed(cAmbulance.Handle, 0F);
                API.TaskLeaveVehicle(cParamedic1.Handle, cAmbulance.Handle, 0);
                API.TaskLeaveVehicle(cParamedic2.Handle, cAmbulance.Handle, 0);

                await Delay(2500);

                API.TaskGoToEntity(cParamedic1.Handle, player.Handle, -1, 5F, 3F, 1073741824, 0);
                API.TaskGoToEntity(cParamedic2.Handle, player.Handle, -1, 5F, 3F, 1073741824, 0);

                while (API.GetDistanceBetweenCoords(cParamedic2.Position.X, cParamedic2.Position.Y, cParamedic2.Position.Z,player.Position.X, player.Position.Y, player.Position.Z, true) > 5F)
                {

                    await Delay(250);
                }
                API.TaskStartScenarioAtPosition(cParamedic2.Handle, "CODE_HUMAN_MEDIC_TIME_OF_DEATH", cParamedic2.Position.X, cParamedic2.Position.Y, cParamedic2.Position.Z, cParamedic2.Heading, -1, false, false);
                API.TaskStartScenarioAtPosition(cParamedic1.Handle, "CODE_HUMAN_MEDIC_TEND_TO_DEAD", player.Position.X, player.Position.Y, player.Position.Z, cParamedic2.Heading, -1, false, false);

                await Delay(15000);
                TriggerEvent("esx_ambulancejob:revive");

                API.ClearPedTasksImmediately(cParamedic1.Handle);
                API.ClearPedTasksImmediately(cParamedic2.Handle);


                cParamedic1.SetIntoVehicle(cAmbulance, VehicleSeat.Driver);
                cParamedic2.SetIntoVehicle(cAmbulance, VehicleSeat.Passenger);

                API.GetNthClosestVehicleNodeWithHeading(player.Position.X, player.Position.Y, player.Position.Z, 80, ref spawnLocation, ref spawnHeading, ref unusedVar, 9, 3.0F, 2.5F);
                cParamedic1.Task.DriveTo(cAmbulance, spawnLocation, 10F, 20F, 262972);

                await Delay(25000);

                cAmbulance.Delete();
                cParamedic1.Delete();
                cParamedic2.Delete();
            }
        }

        public async static void Summon()
        {
            Ped player = Game.Player.Character;

            // Generate the vehicle
            float spawnHeading = 0F;
            int unusedVar = 0;
            API.GetNthClosestVehicleNodeWithHeading(player.Position.X, player.Position.Y, player.Position.Z, 80, ref spawnLocation, ref spawnHeading, ref unusedVar, 9, 3.0F, 2.5F);
            await CommonFuntions.LoadModel((uint)VehicleHash.Ambulance);
            cAmbulance = await World.CreateVehicle(VehicleHash.Ambulance, spawnLocation, spawnHeading);
            cAmbulance.Mods.LicensePlate = "ApacheSC";
            cAmbulance.Mods.LicensePlateStyle = LicensePlateStyle.BlueOnWhite3;
            API.SetVehicleSiren(cAmbulance.Handle, true);

            // Van Blip
            cIAMedicBlip = API.AddBlipForEntity(cAmbulance.Handle);
            API.SetBlipColour(cIAMedicBlip, Client._configFile.BlipColor);
            API.BeginTextCommandSetBlipName("STRING");
            API.AddTextComponentString(Client._configFile.BlipName);
            API.EndTextCommandSetBlipName(cIAMedicBlip);

            // Driver
            await CommonFuntions.LoadModel((uint)PedHash.Doctor01SMM);
            cParamedic1 = await World.CreatePed(PedHash.Doctor01SMM, spawnLocation);
            cParamedic1.SetIntoVehicle(cAmbulance, VehicleSeat.Driver);
            cParamedic1.CanBeTargetted = false;

            // Passenger
            await CommonFuntions.LoadModel((uint)PedHash.Scientist01SMM);
            cParamedic2 = await World.CreatePed(PedHash.Paramedic01SMM, spawnLocation);
            cParamedic2.SetIntoVehicle(cAmbulance, VehicleSeat.Passenger);
            cParamedic2.CanBeTargetted = false;

            // Configuration
            float targetHeading = 0F;
            API.GetClosestVehicleNodeWithHeading(player.Position.X, player.Position.Y, player.Position.Z, ref targetLocation, ref targetHeading, 1, 3.0F, 0);
            cParamedic1.Task.DriveTo(cAmbulance, targetLocation, 10F, 20F, 262972);
            eventSpawned = true;

        }

    }
}
