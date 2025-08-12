using BepInEx;
using GorillaNetworking;
using Photon.Pun;
using PlayFab.ClientModels;
using PlayFab;
using System;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using HarmonyLib;

namespace BetterRoom
{
    [BepInPlugin(PluginInfo.GUID, PluginInfo.Name, PluginInfo.Version)]
    public class Plugin : BaseUnityPlugin
    {
        public static Plugin instance = null;
        void Awake()
        {
            instance = this;
            HarmonyPatches.ApplyHarmonyPatches();
        }

        public bool ProcessRoomState(GorillaKeyboardBindings buttonPressed)
        {
            playerThing = "";

            switch (buttonPressed)
            {
                case GorillaKeyboardBindings.option2:
                    StartCoroutine(GetPlayersInRoom(GorillaComputer.instance.roomToJoin));
                    return false;
            }

            return true;
        }

        public bool running;
        public string playerThing = "";
        public IEnumerator GetPlayersInRoom(string room)
        {
            if (running)
                yield break;

            playerThing = "";
            running = true;

            GorillaComputer.instance.roomFull = false;

            Dictionary<string, string> cosmetics = new Dictionary<string, string> { { "LBAAD.", "ADMINISTRATOR BADGE" }, { "LBAAK.", "MOD STICK" }, { "LBADE.", "FINGER PAINTER BADGE" }, { "LBAGS.", "ILLUSTRATOR BADGE" }, { "LMAPY.", "FOREST GUIDE MOD STICK" } };

            GorillaComputer.instance.UpdateScreen();
            foreach (string region in new[] {"US", "USW", "EU" })
            {
                PlayFabClientAPI.GetSharedGroupData(new PlayFab.ClientModels.GetSharedGroupDataRequest
                {
                    SharedGroupId = room + region
                }, delegate (GetSharedGroupDataResult result)
                {
                    playerThing += region + " " + result.Data.Count;

                    if (result.Data.Count > 0)
                    {
                        foreach (KeyValuePair<string, PlayFab.ClientModels.SharedGroupDataRecord> data in result.Data)
                        {
                            foreach (KeyValuePair<string, string> cosmetic in cosmetics)
                            {
                                if (data.Value.Value.Contains(cosmetic.Key))
                                    playerThing += " <color=green>" + cosmetic.Value + "</color>";
                            }
                        }
                    }

                    playerThing += " ";
                    GorillaComputer.instance.UpdateScreen();
                }, null, null, null);
                yield return new WaitForSeconds(0.1f);
            }

            running = false;
        }
    }
}
