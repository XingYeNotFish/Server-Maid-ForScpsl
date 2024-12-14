//using CommandSystem;
//using Exiled.API.Extensions;
//using Exiled.API.Features;
//using Exiled.API.Features.Pickups;
//using MEC;
//using PlayerRoles;
//using System;
//using System.Linq;

//namespace Server_Maid
//{
//    [CommandHandler(typeof(RemoteAdminCommandHandler))]
//    public class ServerMaidParent : ParentCommand
//    {
//        public ServerMaidParent()
//        {
//            LoadGeneratedCommands();
//        }

//        public override string Command => "servermaid";

//        public override string[] Aliases { get; } = { "maid" };

//        public override string Description => "ServerMaid's Parent Command";

//        public sealed override void LoadGeneratedCommands()
//        {
//            var commandTranslations = new CommandTranslations();

//            RegisterCommand(commandTranslations.StopClean);
//            RegisterCommand(commandTranslations.ReStartClean);
//            RegisterCommand(commandTranslations.ForceClean);
//        }

//        protected override bool ExecuteParent(ArraySegment<string> arguments, ICommandSender sender,
//            out string response)
//        {
//            var playerSender = Player.Get(sender);
//            if (playerSender == null)
//            {
//                response = "Players don't exist!";
//                return false;
//            }
//            response = "<b><color=yellow>stopclean</color>\n\nDeactivate servermaid for this round.\n<color=yellow>restartclean</color>\n\nRe-enable servermaid for this round.\n<color=yellow>forceclean</color>\n\nImmediate implementation of a clean.</b>";
//            return true;
//        }
//    }
//    [CommandHandler(typeof(RemoteAdminCommandHandler))]
//    public class StopClean : ICommand
//    {
//        public string[] Aliases => new string[] { "stopclean" };
//        public string Description => "Deactivate servermaid for this round.";
//        public string Command => "stopclean";
//        private static XYlikeconfig Config => Plugin.Instance.Config;
//        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
//        {
//            Player p = Player.Get(sender);
//            if (arguments.Count != 0)
//            {
//                response = "Error!";
//                return false;
//            }
//            if (p == null)
//            {
//                response = "Players don't exist!";
//                return false;
//            }
//            if (!Config.IsCleaningModuleEnabled)
//            {
//                response = "Error! ServerMaid is not enabled at all!";
//                return false;
//            }

//            Timing.KillCoroutines(Maid.MaidSystem_Coroutine);
//            Map.Broadcast(8, $"<b><size=25>[<color=#EEEE00>Server Maid</color>]Admin <color=#54FF9F>{p.Nickname}</color> has turned off auto-cleanup for this round</size></b>");
//            response = "Success!";
//            return true;
//        }
//    }
//    [CommandHandler(typeof(RemoteAdminCommandHandler))]
//    public class ReStartClean : ICommand
//    {
//        public string[] Aliases => new string[] { "restartclean" };
//        public string Description => "Re-enable servermaid for this round.";
//        public string Command => "restartclean";
//        private static XYlikeconfig Config => Plugin.Instance.Config;
//        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
//        {
//            Player p = Player.Get(sender);
//            if (arguments.Count != 0)
//            {
//                response = "Error!";
//                return false;
//            }
//            if (p == null)
//            {
//                response = "Players don't exist!";
//                return false;
//            }
//            if (!Config.IsCleaningModuleEnabled)
//            {
//                response = "Error! ServerMaid is not enabled at all!";
//                return false;
//            }

//            if (Timing.IsRunning(Maid.MaidSystem_Coroutine))
//            {
//                Timing.KillCoroutines(Maid.MaidSystem_Coroutine);
//            }

//            Maid.MaidSystem_Coroutine = Timing.RunCoroutine(Maid.MaidSystem());
//            Map.Broadcast(8, $"<b><size=25>[<color=#EEEE00>Server Maid</color>]Admin <color=#54FF9F>{p.Nickname}</color> has restart auto-cleanup for this round</size></b>");
//            response = "Success!";
//            return true;
//        }
//    }
//    [CommandHandler(typeof(RemoteAdminCommandHandler))]
//    public class ForceClean : ICommand
//    {
//        public string[] Aliases => new string[] { "forceclean" };
//        public string Description => "Immediate implementation of a clean.";
//        public string Command => "forceclean";
//        private static XYlikeconfig Config => Plugin.Instance.Config;
//        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
//        {
//            Player p = Player.Get(sender);
//            if (arguments.Count != 0)
//            {
//                response = "Error!";
//                return false;
//            }
//            if (p == null)
//            {
//                response = "Players don't exist!";
//                return false;
//            }
//            if (!Config.IsCleaningModuleEnabled)
//            {
//                response = "Error! ServerMaid is not enabled at all!";
//                return false;
//            }

//            int ragdollnum = 0;
//            int itemnum = 0;

//            foreach (Ragdoll ragdoll in Ragdoll.List.ToHashSet())
//            {
//                if (!Config.IsCleaning0492Ragdolls)
//                {
//                    if (ragdoll.Role == RoleTypeId.Scp0492)
//                    {
//                        continue;
//                    }
//                }
//                ragdoll.Destroy();
//                int num = ragdollnum;
//                ragdollnum = num + 1;
//            }

//            foreach (Pickup item in Pickup.List.ToHashSet())
//            {
//                bool flag = !item.Type.IsScp() && !item.Type.IsKeycard() && !item.Type.IsMedical() && !item.Type.IsThrowable() && item.Type != ItemType.MicroHID && !item.Type.IsWeapon(true);
//                if (flag)
//                {
//                    item.Destroy();
//                    int num = itemnum;
//                    itemnum = num + 1;
//                }
//            }

//            if (Timing.IsRunning(Maid.MaidSystem_Coroutine))
//            {
//                Timing.KillCoroutines(Maid.MaidSystem_Coroutine);
//            }

//            Maid.MaidSystem_Coroutine = Timing.RunCoroutine(Maid.MaidSystem());
//            Log.Warn(string.Format(Config.ServerConsoleMessages, itemnum, ragdollnum));
//            Map.Broadcast(10, $"<b><size=25>[<color=#EEEE00>Server Maid</color>]Admin <color=#54FF9F>{p.Nickname}</color> has run forceclean command. \nCleaning {itemnum} items and {ragdollnum} ragdolls this time!</size></b>");
//            response = "Success!";
//            return true;
//        }
//    }
//}