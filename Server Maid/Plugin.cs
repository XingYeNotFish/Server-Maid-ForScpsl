using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using Exiled.Events.EventArgs.Scp3114;
using PlayerRoles.Ragdolls;
using System;
using System.Collections.Generic;

namespace Server_Maid
{
    public class Plugin : Plugin<XYlikeconfig>
    {
        public override string Name { get; } = "Server Maid / 服务器女仆";
        public override string Author { get; } = "XingYeNotFish";
        public override Version Version { get; } = new Version(1, 0, 5);

        public static Plugin Instance;
        public static List<BasicRagdoll> DisguisedRagdolls = [];

        public override void OnEnabled()
        {
            Instance = this;
            base.OnEnabled();
            Exiled.Events.Handlers.Server.RoundStarted += Maid.Start;
            Exiled.Events.Handlers.Server.RoundEnded += Maid.End;
            Exiled.Events.Handlers.Scp3114.Disguised += Disguised;
            Exiled.Events.Handlers.Scp3114.Revealing += Revealing;
            Log.Info("Plugin has been enabled! / 插件已启用!");
        }

        public override void OnDisabled()
        {
            Instance = null;
            base.OnDisabled();
            Exiled.Events.Handlers.Server.RoundStarted -= Maid.Start;
            Exiled.Events.Handlers.Server.RoundEnded -= Maid.End;
            Exiled.Events.Handlers.Scp3114.Disguised -= Disguised;
            Exiled.Events.Handlers.Scp3114.Revealing -= Revealing;
            Log.Info("Plugin has been disabled! / 插件已关闭!");
        }

        private void Disguised(DisguisedEventArgs e)
        {
            if (e.Player != null && e.Ragdoll != null)
            {
                DisguisedRagdolls.Add(e.Ragdoll.Base);
            }
        }

        private void Revealing(RevealingEventArgs e)
        {
            if (e.Player != null && e.Scp3114 != null)
            {
                DisguisedRagdolls.Remove(e.Scp3114.Ragdoll);
            }
        }
    }
}