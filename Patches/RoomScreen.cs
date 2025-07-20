using GorillaNetworking;
using GorillaTagScripts;
using HarmonyLib;
using System;
using KID.Model;
using GorillaGameModes;

namespace BetterRoom.Patches
{
    [HarmonyPatch(typeof(GorillaComputer))]
    [HarmonyPatch("RoomScreen", MethodType.Normal)]
    internal class RoomScreen
    {
        private static void Postfix()
        {
            if (Plugin.instance.playerThing != "")
                GorillaComputer.instance.screenText.Text += " " + Plugin.instance.playerThing;
        }
    }
}
