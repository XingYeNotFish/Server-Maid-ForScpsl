using Exiled.API.Features;
using Exiled.API.Features.Pickups;
using Exiled.Events.EventArgs.Server;
using MEC;
using System;
using System.Collections.Generic;

namespace Server_Maid
{
    public class Maid
    {
        public enum CleaningType
        {
            Category,
            WhiteList,
            BlackList
        }

        public static CoroutineHandle MaidSystem_Coroutine;
        private static XYlikeconfig Config => Plugin.Instance.Config;
        private static Func<Pickup, bool> ShouldDestroyItem;

        public static void Start()
        {
            if (Config.IsCleaningModuleEnabled)
            {
                ShouldDestroyItem = Config.CleaningType switch
                {
                    CleaningType.Category => p => Config.Categories.Contains(p.Category),
                    CleaningType.WhiteList => p => !Config.WhiteList.Contains(p.Type),
                    CleaningType.BlackList => p => Config.BlackList.Contains(p.Type),
                    _ => _ => false
                };

                MaidSystem_Coroutine = Timing.RunCoroutine(MaidSystem());
                Log.Warn(Config.CleaningModuleEnabledServerConsoleMessages);
                Plugin.DisguisedRagdolls.Clear();
            }
        }

        public static void End(RoundEndedEventArgs e)
        {
            if (Config.IsCleaningModuleEnabled)
            {
                Timing.KillCoroutines(MaidSystem_Coroutine);
                Plugin.DisguisedRagdolls.Clear();
            }
        }

        public static IEnumerator<float> MaidSystem()
        {
            yield return Timing.WaitForSeconds(Config.CleaningInterval);
            for (; ; )
            {
                int ragdollnum = 0;
                int itemnum = 0;

                foreach (Ragdoll ragdoll in Ragdoll.List)
                {
                    if (Plugin.DisguisedRagdolls.Contains(ragdoll.Base))
                        continue;

                    if (!ragdoll.IsExpired)
                        continue;

                    ragdoll.Destroy();
                    ragdollnum++;
                }

                foreach (Pickup item in Pickup.List)
                {
                    if (ShouldDestroyItem(item))
                    {
                        item.Destroy();
                        itemnum++;
                    }
                }

                try
                {
                    Log.Warn(string.Format(Config.ServerConsoleMessages, itemnum, ragdollnum));
                    Map.Broadcast(10, string.Format(Config.BroadcastMessages, itemnum, ragdollnum), 0, true);
                }
                catch (Exception e)
                {
                    Log.Error("Error in MaidSystem: " + e.Message);
                }
                yield return Timing.WaitForSeconds(Config.CleaningInterval);
            }
        }
    }
}