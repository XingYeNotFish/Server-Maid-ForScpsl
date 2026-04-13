namespace Server_Maid
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using static Server_Maid.Maid;

#if EXILED
    using Exiled.API.Interfaces;
    public class XYlikeconfig : IConfig
#else
    public class XYlikeconfig
#endif
    {
        [Description("Do you want to enable the plugin? / 是否开启此插件?")]
        public bool IsEnabled { get; set; } = true;
        public bool Debug { get; set; } = false;

        [Description("Cleaning module settings / 清理模块设置")]
        public bool IsCleaningModuleEnabled { get; set; } = true;

        [Description("Pickups Cleaning type: Category/Whitelist/Blacklist / 掉落物清理类型: 物品种类/白名单/黑名单")]
        public CleaningType CleaningType { get; set; } = CleaningType.Category;
        public List<ItemCategory> Categories { get; set; } = [ItemCategory.Ammo, ItemCategory.Armor, ItemCategory.Keycard, ItemCategory.None, ItemCategory.Radio];
        public List<ItemType> WhiteList { get; set; } = [];
        public List<ItemType> BlackList { get; set; } = [];

        public string CleaningModuleEnabledServerConsoleMessages { get; set; } = "Cleaning module has been enable in this round!";

        [Description("Cleaning interval time Unit: seconds/ 清理间隔时间 单位: 秒")]
        public float CleaningInterval { get; set; } = 300;
        [Description("Cleanup ended displaying content {0} represents the number of items cleared {1} is the player's ragdolls/ 清理结束显示内容 {0} 代表清理的物品数量 {1}为玩家尸体")]
        public string ServerConsoleMessages { get; set; } = "Cleanup successful! Cleaning {0} items and {1} ragdolls this time!";
        public string BroadcastMessages { get; set; } = "<b><size=25>[<color=#EEEE00>Server Maid</color>]Cleanup successful! Cleaning {0} items and {1} ragdolls this time!</size></b>";
    }
}