# Server Maid 🧹  
**SCP: Secret Laboratory 服务器清洁插件**

一个基于 EXILED 的轻量级插件，帮你自动清理玩家尸体和无用掉落物,让服务器时刻保持整洁.

[![Downloads](https://img.shields.io/github/downloads/XingYeNotFish/Server-Maid-ForScpsl/total?color=brown&label=Downloads&style=for-the-badge) ](https://github.com/XingYeNotFish/Server-Maid-ForScpsl/releases)

---

## ✨ 主要功能
- **全自动清理**: 每回合按可配置的时间间隔自动运行.
- **SCP-3114 友好**: 3114正在伪装的玩家尸体**永远不会**被误删导致提前暴露.
- **多种过滤方式**:
  - 按**物品类别** (弹药、护甲、钥匙卡…)
  - **白名单** (仅保留指定物品)
  - **黑名单** (仅删除指定物品)
- **双语提示**：内置中文 & 英,支持完全自定义.
- **零性能负担**：单协程执行,极轻量.

---

## 🚀 安装步骤
1. 前往 [Releases](https://github.com/XingYeNotFish/Server-Maid-ForScpsl/releases) 页面下载最新版.
2. 将 `Server-Maid.dll` 放入服务器的 `EXILED/Plugins` 文件夹.
3. 重启或重载服务器.
4. （可选）在 `EXILED\Configs\Plugins\server_maid\{端口号}.yml` 中修改生成的配置文件.

---

## ⚙️ 配置说明
所有值都可直接编辑 YAML 文件.

| 键名 | 类型 | 默认值 | 说明 |
|------|------|--------|------|
| `IsEnabled` | bool | `true` | 插件总开关 |
| `IsCleaningModuleEnabled` | bool | `true` | 清理模块独立开关 |
| `CleaningType` | enum | `Category` | 清理模式: `Category` (按类别)、`WhiteList` (白名单)、`BlackList` (黑名单) |
| `Categories` | list | `[Ammo, Armor, Keycard, None, Radio]` | 仅当 `CleaningType = Category` 时生效. 列表内的物品类别会被清理 |
| `WhiteList` | list | `[]` | 仅当 `CleaningType = WhiteList` 时生效. 列表以外的物品才会被清理 |
| `BlackList` | list | `[]` | 仅当 `CleaningType = BlackList` 时生效. 列表内的物品会被清理 |
| `CleaningInterval` | float | `300` | 每次清理的间隔时间(秒) |
| `ServerConsoleMessages` | string | `"Cleanup successful! Cleaning {0} items and {1} ragdolls this time!"` | 清理成功后服务器控制台提示 |
| `BroadcastMessages` | string | `"<b><size=25>[<color=#EEEE00>Server Maid</color>] ... </size></b>"` | 清理成功后游戏内广播内容 |

---

## 📖 示例配置（中文）
```yml
# 是否开启此插件?
is_enabled: true
debug: false

# 清理模块设置
is_cleaning_module_enabled: true

# 掉落物清理类型: Category/Whitelist/Blacklist
cleaning_type: Category

categories:
- Ammo        # 弹药
- Armor       # 护甲
- Keycard     # 钥匙卡
- None        # 无分类物品
- Radio       # 对讲机

white_list: []
black_list: []

# 清理间隔时间（秒）
cleaning_interval: 180

# 清理结束后在服务器控制台的提示
server_console_messages: '🧽 Server Maid 已清理 {0} 件垃圾并送 {1} 位伤员去医院!'

# 清理结束后在游戏内的广播
broadcast_messages: '<b><size=25>[<color=#EEEE00>服务器女仆</color>] 已打扫 {0} 件垃圾，并送 {1} 位伤员去医院!</size></b>'
```

---

## 🤝 参与贡献
欢迎提交 Pull Request、Bug 报告与功能建议！  
如计划进行重大修改，请先开一个 [Issue](https://github.com/XingYeNotFish/Server-Maid-ForScpsl/issues) 讨论。

---

用爱制作 ❤️ by **XingYeNotFish**  
“好服务器值得拥有一位好女仆。”
