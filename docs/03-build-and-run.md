# 运行与部署

## 1. 环境准备

- Unity Hub + **Unity 2022.3.13f1c1**（或兼容的 2022.3.x LTS），新建 **3D** 项目；
- 在 Package Manager 安装 **TextMeshPro**（`com.unity.textmeshpro`）与
  **Visual Scripting**（`com.unity.visualscripting`，多个脚本含 `using Unity.VisualScripting`）；
- `singleWindArea.cs` 含 `using JetBrains.Annotations;`，若未安装该程序集，
  在确认无副作用后可移除该 `using`（仅用于注解，不影响逻辑）。

## 2. 导入源码

将 `src/` 下 20 个 `.cs` 与对应 `.meta` 复制到 `Assets/Scripts/`。
`.meta` 保存 Unity 的 GUID 引用关系，建议保留，否则需重新挂载脚本组件。

> 源码 `.cs` 中的中文注释为 **GBK** 编码，在 UTF-8 编辑器中可能显示乱码，
> 但注释不影响编译；请勿整体转码，详见 [注意事项](06-notes.md)。

## 3. 场景对象命名约定

脚本大量使用 `GameObject.Find()` 按名称查找对象，场景需按下列名称创建：

| 对象名 | 类型 | 需挂载脚本 |
|--------|------|-----------|
| `centralPoint` | GameObject（风场中心） | `mapRotate` |
| `Text_path` | UGUI `Text` | 其 `text` 内容为 CSV 数据目录绝对路径 |
| `MainCamera` | Camera | `camera_move`、`compass`（罗盘子对象） |
| `Particle System` | ParticleSystem | `change_wind`、`force` |
| `windPT` | ParticleSystem 预制对象 | `cloneWind` 的克隆模板 |
| `singleWindArea` | GameObject + ParticleSystem | `singleWindArea` |
| `Text_angle` | `TextMeshProUGUI` | `scrollbar_rotate` 读取 |
| `Text_speed` | `TextMeshProUGUI` | `scrollbar_speed` 读取 |
| `Text_hight` | `TextMeshProUGUI` | `scrollbar_yMove` 读取 |

另外：
- 三个 `Scrollbar` 分别挂载 `scrollbar_rotate` / `scrollbar_speed` / `scrollbar_yMove`；
- 箭头对象放在 **Layer 6**，`showArror` / `hideArror` 通过 `cullingMask` 控制显隐；
- `CreateTerrain` 需挂在带 `MeshRenderer` / `MeshFilter` 的对象上，并给 `heightMap` 赋值纹理；
- `closeWindow.testClick()` 可绑定到退出按钮。

## 4. 准备数据

将上游雷达测风反演产生的风场 CSV 放入一个目录，在 `Text_path` 中填写该目录路径。
`mapRotate.GetAllFiles()` 会取目录中 `GetFiles` 返回列表的**倒数第二个文件**作为数据源，
因此目录内至少应有 2 个文件（见 [注意事项](06-notes.md)）。

## 5. 运行

点击 Play。`mapRotate.Start()` 设置最高分辨率与全屏；
`FixedUpdate()` 周期调用 `ImportCSV()`，更新 `wind_speed` 与 `centralPoint` 旋转角；
`change_wind` 把风速同步到粒子受力与速度上限，形成随数据变化的风场动画。

## 6. 构建发布

Unity 菜单 `File > Build Settings` 选择目标平台（Windows / Mac / Linux），
`Build` 生成可执行文件。运行库由 Unity 随构建一并打包，无需额外部署工具。
地理底图与建筑 DXF 若需在目标机器显示，请将其放入构建数据目录并按场景引用路径配置。
