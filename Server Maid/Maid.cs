using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.API.Features.Pickups;
using Exiled.Events.EventArgs.Server;
using MEC;
using System.Collections.Generic;
using System.Linq;

namespace Server_Maid
{
    public class Maid
    {
        public static CoroutineHandle MaidSystem_Coroutine;
        private static XYlikeconfig config => Plugin.Instance.Config;
        public static void Start()
        {
            if (config.Is_cleaning_module_enabled)
            {
                MaidSystem_Coroutine = Timing.RunCoroutine(MaidSystem());
                Log.Warn(config.Cleaning_module_enabled_server_console_messages);
            }
        }

        public static void End(RoundEndedEventArgs e)
        {
            if (config.Is_cleaning_module_enabled)
            {
                Timing.KillCoroutines(MaidSystem_Coroutine);
            }
        }

        public static IEnumerator<float> MaidSystem()
        {

            yield return Timing.WaitForSeconds(config.Cleaning_interval);
            for (; ; )
            {
                int ragdollnum = 0;
                int itemnum = 0;

                foreach (Ragdoll ragdoll in Ragdoll.List.ToHashSet())
                {
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

                Log.Warn(string.Format(config.Server_console_messages, itemnum, ragdollnum));

                Timing.CallDelayed(5f, delegate ()
                {
                    Map.Broadcast(10, string.Format(config.Broadcast_messages, itemnum, ragdollnum), 0, true);
                });

                yield return Timing.WaitForSeconds(config.Cleaning_interval);
            }
        }
    }
}