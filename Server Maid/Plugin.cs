
namespace Server_Maid
{   
    using PlayerRoles.Ragdolls;
    using System;
    using System.Collections.Generic;

#if EXILED
    using Exiled.API.Features;
    using Exiled.Events.EventArgs.Player;
    using Exiled.Events.EventArgs.Scp3114;

    public class Plugin : Plugin<XYlikeconfig>
#else
    using LabApi.Features;
    using LabApi.Features.Console;
    using LabApi.Loader.Features.Plugins;
    using LabApi.Events.Arguments.Scp3114Events;
    using LabApi.Events.Handlers;
    using PlayerRoles.PlayableScps.Scp3114;

    public class Plugin : Plugin<XYlikeconfig>
#endif
    {
        public override string Name { get; } = "Server Maid / 服务器女仆";
        public override string Author { get; } = "XingYeNotFish";
        public override Version Version { get; } = new Version(1, 1, 0);

#if LABAPI
        public override string Description { get; } = "A plugin that automatically helps you clean up your server's drops and ragdolls";
        public override Version RequiredApiVersion { get; } = new(LabApiProperties.CompiledVersion);
#endif
        
        public static Plugin Instance { get; private set; }
        public static List<BasicRagdoll> DisguisedRagdolls = [];
#if EXILED
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
#else
        public override void Enable()
        {
            Instance = this;
            Scp3114Events.Disguised += Disguised;
            Scp3114Events.Revealing += Revealing;
            ServerEvents.RoundStarted += Maid.Start;
            ServerEvents.RoundEnded += Maid.End;
            Logger.Info("Plugin has been enabled! / 插件已启用!");
        }

        public override void Disable()
        {
            Instance = null;
            Scp3114Events.Disguised -= Disguised;
            Scp3114Events.Revealing -= Revealing;
            ServerEvents.RoundStarted -= Maid.Start;
            ServerEvents.RoundEnded -= Maid.End;
            Logger.Info("Plugin has been disabled! / 插件已关闭!");
        }

        private void Disguised(Scp3114DisguisedEventArgs e)
        {
            if (e.Player != null && e.Ragdoll != null)
            {
                DisguisedRagdolls.Add(e.Ragdoll.Base);
            }
        }

        private void Revealing(Scp3114RevealingEventArgs e)
        {
            if (e.Player != null)
            {
                if (e.Player.RoleBase is Scp3114Role scp3114)
                {
                    DisguisedRagdolls.Remove(scp3114.Ragdoll);
                }

            }
        }
#endif
    }
}