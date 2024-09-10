using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.API.Features.Pickups;
using Exiled.Events.EventArgs.Server;
using MEC;
using System.Collections.Generic;
using System.Linq;
using PlayerRoles;
using PlayerRoles.PlayableScps.Scp3114;
using Map = Exiled.API.Features.Map;

namespace Server_Maid
{
    public class Maid
    {
        public static CoroutineHandle MaidSystem_Coroutine;
        private static XYlikeconfig Config => Plugin.Instance.Config;
        public static void Start()
        {
            if (Config.IsCleaningModuleEnabled)
            {
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

                foreach (Ragdoll ragdoll in Ragdoll.List.ToHashSet())
                {
                    if (!Config.IsCleaning0492Ragdolls)
                    {
                        if (ragdoll.Role == RoleTypeId.Scp0492)
                        {
                            continue;
                        }
                    }

                    if (Plugin.DisguisedRagdolls.Contains(ragdoll))
                    {
                        continue;
                    }

                    if (!ragdoll.IsExpired)
                    {
                        continue;
                    }
                    
                    ragdoll.Destroy();
                    int num = ragdollnum;
                    ragdollnum = num + 1;
                }

                foreach (Pickup item in Pickup.List.ToHashSet())
                {
                    bool flag = !item.Type.IsScp() && !item.Type.IsKeycard() && !item.Type.IsMedical() && !item.Type.IsThrowable() && item.Type != ItemType.MicroHID && !item.Type.IsWeapon(true);
                    if (flag)
                    {
                        item.Destroy();
                        int num = itemnum;
                        itemnum = num + 1;
                    }
                }

                Log.Warn(string.Format(Config.ServerConsoleMessages, itemnum, ragdollnum));

                Timing.CallDelayed(3f, delegate ()
                {
                    Map.Broadcast(10, string.Format(Config.BroadcastMessages, itemnum, ragdollnum), 0, true);
                });

                yield return Timing.WaitForSeconds(Config.CleaningInterval);
            }
        }
    }
}