using GorillaNetworking;
using HarmonyLib;
using System;

namespace BetterRoom.Patches
{
    [HarmonyPatch(typeof(GorillaComputer))]
    [HarmonyPatch("ProcessRoomState", MethodType.Normal)]
    internal class ProcessRoomState
    {
        private static bool Prefix(GorillaKeyboardBindings buttonPressed) =>
            Plugin.instance.ProcessRoomState(buttonPressed);
    }
}
