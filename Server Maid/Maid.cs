namespace Server_Maid
{
    using MEC;
    using System;
    using System.Collections.Generic;

#if EXILED
    using Exiled.API.Features;
    using Exiled.API.Features.Pickups;
    using Exiled.Events.EventArgs.Server;
    using Exiled.CustomItems.API.Features;
    using System.Linq;
#else
    using LabApi.Features.Wrappers;
    using LabApi.Features.Console;
    using LabApi.Events.Arguments.ServerEvents;
#endif
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

                if (Timing.IsRunning(MaidSystem_Coroutine))
                    Timing.KillCoroutines(MaidSystem_Coroutine);

                MaidSystem_Coroutine = Timing.RunCoroutine(MaidSystem());
#if EXILED
                Log.Warn(Config.CleaningModuleEnabledServerConsoleMessages);
#else
                Logger.Warn(Config.CleaningModuleEnabledServerConsoleMessages);
#endif
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

#if EXILED
                    if (!ragdoll.IsExpired)
#else
                    if (!(ragdoll.Base.NetworkInfo.ExistenceTime > 12f))
#endif
                        continue;

                    ragdoll.Destroy();
                    ragdollnum++;
                }

                foreach (Pickup item in Pickup.List)
                {
                    if (ShouldDestroyItem(item))
                    {
#if EXILED
                        if (!Config.ShouldDestroyCustomItems)
                        {
                            CustomItem custom = CustomItem.Registered.FirstOrDefault(x => x.Check(item));
                            if (custom != null)
                                continue;
                        }
#endif

                        item.Destroy();
                        itemnum++;
                    }
                }

                try
                {
#if EXILED
                    Log.Warn(string.Format(Config.ServerConsoleMessages, itemnum, ragdollnum));
                    Map.Broadcast(10, string.Format(Config.BroadcastMessages, itemnum, ragdollnum), 0, true);
#else
                    Logger.Warn(string.Format(Config.ServerConsoleMessages, itemnum, ragdollnum));
                    Server.SendBroadcast(string.Format(Config.BroadcastMessages, itemnum, ragdollnum), 10, 0, true);
#endif
                    
                }
                catch (Exception e)
                {
#if EXILED
                    Log.Error("Error in MaidSystem: " + e.Message);
#else
                    Logger.Error("Error in MaidSystem: " + e.Message);
#endif
                }
                yield return Timing.WaitForSeconds(Config.CleaningInterval);
            }
        }
    }
}