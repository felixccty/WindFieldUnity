# WindFieldUnity

> 基于 Unity 2022.3 / C# 的区域风场粒子可视化仿真系统。

[![Platform](https://img.shields.io/badge/platform-Windows%20%7C%20Mac%20%7C%20Linux-blue.svg)](https://unity.com/)
[![Language](https://img.shields.io/badge/language-C%23-purple.svg)](https://docs.microsoft.com/dotnet/csharp/)
[![Framework](https://img.shields.io/badge/framework-Unity%202022.3-orange.svg)](https://unity.com/)
[![License](https://img.shields.io/badge/license-MIT-blue.svg)](LICENSE)

`WindFieldUnity` 是一套基于 **Unity 2022.3** 的**区域风场粒子可视化仿真脚本集**。它把雷达测风反演得到的 CSV 矢量分量（`x:y` 形式）解析为平均风向与风速，驱动场景中心的旋转、粒子系统的受力与速度上限，从而直观展示建筑群/地理环境中的风场运动。

核心脚本 `mapRotate` 挂载于 `centralPoint`，在 `FixedUpdate()` 中周期扫描数据目录、读取最新 CSV 并更新风向风速；`change_wind` 把该风速实时映射到粒子系统的 `forceOverLifetime.z` 与 `limitVelocityOverLifetime.limit`。配套提供相机漫游、罗盘指向、分区风场克隆、地形高度图生成及 UGUI 滑条调参等模块。

---

## 核心特性

- **风场数据读取**：`mapRotate.GetAllFiles()` 扫描数据目录取最新文件，`ImportCSV()` 解析逗号分隔、冒号分隔的 `x:y` 矢量分量，过滤 `-999.0:-999.0` 无效值与过短行。
- **风向风速计算**：`ImportCSV()` 累加分量求均值，`wind_speed = |tan(avg_x / avg_y)|`，`rotate_angle = atan(avg_x / avg_y)` 驱动 `centralPoint` 旋转。
- **粒子受力驱动**：`change_wind.Update()` 把 `mapRotate.wind_speed` 写入粒子系统 `forceOverLifetime.z` 与 `limitVelocityOverLifetime.limit`（均 +2f）。
- **粒子运动**：`particleSys` 按 `windStrength` 逐帧位移存活粒子；`force` 用 `AnimationCurve` 控制 x 方向受力；`ParticleMove` 沿 z 轴平移。
- **分区风场**：`singleWindArea.ImportCSV()` 计算单个区域平均风向，`OnParticleTrigger()` 按进入/退出触发器把粒子染为红/绿；`cloneWindArea` 克隆 5×5 区域，`cloneWind` 克隆 150 个粒子点。
- **相机漫游**：`camera_move`（cameraMove.cs）支持 WASD 平移、右键拖拽旋转、滚轮缩放（1–2000）、中键屏幕平移，`ClampAngle()` 限制俯仰角。
- **罗盘指向**：`compass`（rotateCompass.cs）依据主相机 forward 与参考向量 `(0,0,1)` 计算方位角，绕参考轴旋转罗盘。
- **地形生成**：`CreateTerrain.Generate()` 读取高度图红色通道，按 `N=200` 网格、`NMap=512` 采样、`height=30` 生成地形 Mesh 并重算法线。
- **对象批量创建**：`createObject`（CreateWindField.cs）生成 10×10×10 个圆柱体阵列。
- **UI 参数调节**：`scrollbar_rotate` / `scrollbar_speed` / `scrollbar_yMove` 分别控制旋转角、风速与高度；`winderDirSiderCtr` 实现 Scrollbar + InputField 双向联动。
- **可视化开关与退出**：`showArror` / `hideArror` 通过 `Camera.main.cullingMask` 切换第 6 层箭头；`closeWindow.testClick()` 调用 `Application.Quit()`。

---

## 技术栈

| 类别 | 选型 |
|------|------|
| 语言 | C#（Unity MonoBehaviour） |
| 引擎 | Unity 2022.3.13f1c1（录屏版本） |
| UI | UGUI + TextMeshPro（TMPro） |
| 附加模块 | Unity Visual Scripting、JetBrains.Annotations |
| 数据源 | CSV（逗号分隔的 `x:y` 矢量分量） |
| 地理数据 | cadmapper DXF / 卫星底图（用户自备） |
| 目标平台 | Windows / Mac / Linux（Unity 跨平台） |

---

## 快速开始

```bash
git clone https://github.com/felixccty/WindFieldUnity.git
cd WindFieldUnity
```

1. 安装 Unity Hub 与 Unity 2022.3.13f1c1（3D 模板）。
2. 将 `src/` 下全部 `.cs` 与 `.meta` 复制到 `Assets/Scripts/`；保留 `.meta` 以维持 GUID 引用关系。
3. 在 Package Manager 安装 TextMeshPro 与 Visual Scripting；`singleWindArea.cs` 引用的 `JetBrains.Annotations` 需自行获取，确认后可移除该 `using`。
4. 按 [运行与部署](docs/03-build-and-run.md) 的命名约定搭建场景（`centralPoint`、`Text_path`、`MainCamera`、`Particle System` 等）。
5. 在 `Text_path` 中填入 CSV 数据目录路径后运行；`mapRotate` 在 `FixedUpdate()` 中自动读取最新 CSV 并驱动风场。

完整步骤见 [运行与部署](docs/03-build-and-run.md) 与 [依赖配置](docs/04-dependencies.md)。

---

## 目录结构

```
WindFieldUnity/
├── README.md
├── LICENSE / CHANGELOG.md / CONTRIBUTING.md / .gitignore
├── src/                           # 核心 C# 脚本（20 个 .cs + 对应 .meta）
│   ├── mapRotate.cs / .meta       # CSV 读取、风向风速计算与场景旋转（核心）
│   ├── change_wind.cs / .meta     # 风速 -> 粒子受力/速度上限
│   ├── singleWindArea.cs / .meta  # 分区风场 CSV 解析与粒子触发器染色
│   ├── cloneWind.cs / .meta       # 克隆粒子点阵列
│   ├── cloneWindArea.cs / .meta   # 克隆分区风场
│   ├── particleSys.cs / .meta     # 逐粒子位移
│   ├── force.cs / .meta           # 粒子受力 AnimationCurve
│   ├── ParticleMove.cs / .meta    # 简单平移
│   ├── cameraMove.cs / .meta      # 相机漫游（WASD/右键/滚轮/中键）
│   ├── rotateCompass.cs / .meta   # 罗盘指向
│   ├── CreateTerrain.cs / .meta   # 高度图 -> 地形 Mesh
│   ├── CreateWindField.cs / .meta # 批量创建圆柱体阵列
│   ├── scrollbar_rotate.cs / .meta
│   ├── scrollbar_speed.cs / .meta
│   ├── scrollbar_yMove.cs / .meta
│   ├── winderDirSiderCtr.cs / .meta
│   ├── showArror.cs / .meta
│   ├── hideArror.cs / .meta
│   ├── windAreaTrig.cs / .meta
│   └── closeWindow.cs / .meta
├── resources/                     # 关键截图与地理底图（已核验无品牌水印）
│   ├── demo-streamlines.png
│   └── geomap.png
├── docs/                          # 本文档体系 + images/ 示意图
└── (本地按需) CSV 数据目录 / cadmapper DXF / 卫星底图
```

---

## 文档索引

| 文档 | 内容 |
|------|------|
| [功能说明](docs/01-features.md) | CSV 风场解析、风速风向计算、粒子驱动、相机/罗盘/分区/UI 控制 |
| [目录结构](docs/02-structure.md) | 20 个脚本职责、类名映射与数据流动 |
| [运行与部署](docs/03-build-and-run.md) | Unity 版本、场景对象命名约定、编译与运行 |
| [依赖配置](docs/04-dependencies.md) | Unity 包、命名空间、数据源与系统要求 |
| [使用示例](docs/05-usage.md) | CSV 矢量分量格式、路径配置与典型操作 |
| [注意事项](docs/06-notes.md) | 编码、固定路径、已知限制与取舍说明 |
| [架构与流程](docs/07-architecture.md) | 分层架构、关键流程与数据流原理图 |

## 数据来源说明

本项目只提供**可视化与场景驱动脚本**。风场 CSV 由上游雷达测风反演程序产生，按 `x:y` 矢量分量格式（逗号分隔）放置到配置目录后由 `mapRotate` 读取；地理底图与建筑 DXF 需用户自行准备。详见 [使用示例](docs/05-usage.md)。

---

## 许可与作者

- **许可**：[MIT License](LICENSE)
- **作者 / 项目归属**：Felix
- **仓库**：https://github.com/felixccty/WindFieldUnity

本项目以学习和工程交流为目的开源，欢迎 Issue 与 Pull Request。使用前请自行评估其在目标场景下的适用性与安全性。
