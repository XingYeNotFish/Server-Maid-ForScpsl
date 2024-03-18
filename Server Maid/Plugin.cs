using Exiled.API.Features;
using System;

namespace Server_Maid
{
    public class Plugin : Plugin<XYlikeconfig>
    {
        public override string Name { get; } = "Server Maid / 服务器女仆";
        public override string Author { get; } = "XingYeNotFish";
        public override Version Version { get; } = new Version(1, 0, 0);

        private static readonly Lazy<Plugin> LazyInstance = new Lazy<Plugin>(() => new Plugin());
        public static Plugin Instance => LazyInstance.Value;

        public override void OnEnabled()
        {
            base.OnEnabled();
            Exiled.Events.Handlers.Server.RoundStarted += Maid.Start;
            Exiled.Events.Handlers.Server.RoundEnded += Maid.End;
            Log.Info("Plugin has been enabled! / 插件已启用!");
        }

        public override void OnDisabled()
        {
            base.OnDisabled();
            Exiled.Events.Handlers.Server.RoundStarted -= Maid.Start;
            Exiled.Events.Handlers.Server.RoundEnded += Maid.End;
            Log.Info("Plugin has been disabled! / 插件已关闭!");
        }
    }
}