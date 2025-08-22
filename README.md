# Server Maid 🧹  
[中文自述文件](ReadMeFile/README_CN.md)

**SCP: Secret Laboratory** 

Exiled plugin that keeps your server tidy – automatically removes expired ragdolls and unwanted item pickups every round.

[![Downloads](https://img.shields.io/github/downloads/XingYeNotFish/Server-Maid-ForScpsl/total?color=brown&label=Downloads&style=for-the-badge    )](https://github.com/XingYeNotFish/Server-Maid-ForScpsl/releases    )

---

## ✨ Features
* **Automatic cleanup** – runs on a configurable timer.  
* **SCP-3114 friendly** – ragdolls used by SCP-3114 for disguise are **never** deleted.  
* **Flexible filtering** – decide what gets removed by:
  * Item **category** (ammo, armor, key-cards …)  
  * **Whitelist** (keep only listed items)  
  * **Blacklist** (remove only listed items)  
* **Multi-language messages** – English & Chinese built-in, fully customizable.  
* **Zero performance worries** – executed in a single lightweight coroutine.

---

## 🚀 Installation
1. Download the latest release from the [Releases](https://github.com/XingYeNotFish/Server-Maid/releases    ) page.  
2. Drop `Server-Maid.dll` into your `EXILED/Plugins` folder.  
3. Restart or reload your server.  
4. (Optional) edit the auto-generated config file at  
   `EXILED\Configs\Plugins\server_maid\{Your_Port}.yml`.

---

## ⚙️ Configuration
All values can be changed directly in the YAML file.

| Key | Type | Default | Description |
|-----|------|---------|-------------|
| `IsEnabled` | bool | `true` | Master switch for the entire plugin. |
| `IsCleaningModuleEnabled` | bool | `true` | Toggle the cleanup system itself. |
| `CleaningType` | enum | `Category` | `Category`, `WhiteList`, or `BlackList`. |
| `Categories` | list | `[Ammo, Armor, Keycard, None, Radio]` | Only used when `CleaningType = Category`. Only clean up dropped items whose `ItemCategory` are in `Categories`.|
| `WhiteList` | list | `[]` | Only used when `CleaningType = WhiteList`. Only clean up pickups that are not in `WhiteList`.|
| `BlackList` | list | `[]` | Only used when `CleaningType = BlackList`. Only clean up pickups from `BlackList`.|
| `CleaningInterval` | float | `300` | Seconds between each cleanup cycle. |
| `ServerConsoleMessages` | string | `"Cleanup successful! Cleaning {0} items and {1} ragdolls this time!"` | Server console message after cleanup. |
| `BroadcastMessages` | string | `"<b><size=25>[<color=#EEEE00>Server Maid</color>] ... </size></b>"` | In-game broadcast message after cleanup. |

---

## 📖 Example Config
```yml
# Do you want to enable the plugin? / 是否开启此插件?
is_enabled: true
debug: false
# Cleaning module settings / 清理模块设置
is_cleaning_module_enabled: true
# Pickups Cleaning type: Category/Whitelist/Blacklist / 掉落物清理类型: 物品种类/白名单/黑名单
cleaning_type: Category
categories:
- Ammo
- Armor
- Keycard
- None
- Radio
white_list: []
black_list: []
cleaning_module_enabled_server_console_messages: 'Cleaning module has been enable in this round!'
# Cleaning interval time Unit: seconds/ 清理间隔时间 单位: 秒
cleaning_interval: 180
# Cleanup ended displaying content {0} represents the number of items cleared {1} is the player's ragdolls/ 清理结束显示内容 {0} 代表清理的物品数量 {1}为玩家尸体
server_console_messages: '🧽 Server Maid wiped {0} trash and {1} injured persons.'
broadcast_messages: '<b><size=25>[<color=#EEEE00>Server Maid</color>]Cleanup successful! Cleaning {0} trash and take the {1} injured persons to the hospital!</size></b>'
```

---

## 🤝 Contributing
Pull requests, bug reports and suggestions are welcome!  
Please open an [Issue](https://github.com/XingYeNotFish/Server-Maid/issues    ) first for any major changes.

---

Made with ❤️ by **XingYeNotFish**  
*“Because every good server deserves a good maid.”*
