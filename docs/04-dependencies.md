# 依赖配置

## 语言与引擎

| 类别 | 版本/组件 | 说明 |
|------|-----------|------|
| 语言 | C# | Unity MonoBehaviour 脚本 |
| 引擎 | Unity 2022.3.13f1c1（或 2022.3.x LTS） | 录屏中显示的版本 |
| UI | UGUI（`UnityEngine.UI`）+ TextMeshPro（`TMPro`） | `Text`、`Scrollbar`、`TextMeshProUGUI`、`InputField` |

## 命名空间 / 包

| 命名空间 | 来源 | 使用脚本 |
|----------|------|----------|
| `UnityEngine` | Unity 内置 | 全部脚本 |
| `UnityEngine.UI` | UGUI | `mapRotate`、`scrollbar_*`、`winderDirSiderCtr`、`closeWindow` |
| `UnityEngine.UIElements` | Unity UI Elements | `cloneWindArea`、`scrollbar_*` |
| `TMPro` | TextMeshPro 包 | `CreateWindField`、`scrollbar_*` |
| `Unity.VisualScripting` | Visual Scripting 包 | `CreateTerrain`、`mapRotate`、`ParticleMove`、`force`、`singleWindArea` |
| `JetBrains.Annotations` | 第三方（NuGet/手动引用） | `singleWindArea`（仅注解用） |
| `System.IO` | .NET | `mapRotate`、`singleWindArea`（`StreamReader` / `Directory`） |

## 运行期外部依赖

- **风场 CSV**：由上游雷达测风反演程序产生，格式为逗号分隔的 `x:y` 矢量分量；
- **CSV 数据目录**：路径由场景对象 `Text_path` 的文本提供；
- **地理/建筑数据**：`resources/geomap.png` 仅为示例底图；原始 cadmapper DXF 与外卖卫星底图需用户自备；
- **高度图纹理**：`CreateTerrain.heightMap` 需在 Inspector 中赋值。

## 系统要求

- Windows / macOS / Linux，满足 Unity 2022.3 LTS 的最低要求；
- 建议独立显卡以流畅运行粒子系统（粒子数量由 `cloneWind` 决定，约 150 个点 × 各自粒子）；
- 需对 CSV 数据目录具有读取权限。

## 重要说明

本仓库**只含核心脚本**，不含完整 Unity 工程（`.unity` 场景、ProjectSettings、Packages 清单）、
第三方 SDK、地理源数据与录屏。请按 [运行与部署](03-build-and-run.md) 自行搭建场景。
