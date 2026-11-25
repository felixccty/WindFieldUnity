# 目录结构

## 发布包目录

```
WindFieldUnity/
├── README.md / LICENSE / CHANGELOG.md / CONTRIBUTING.md / .gitignore
├── src/                    # 20 个 C# 脚本 + 20 个 .meta（保持原编码）
├── resources/              # demo-streamlines.png / geomap.png
├── docs/                   # 本体系 + images/ 示意图
└── (本地按需) CSV 数据 / DXF / 卫星底图
```

## 源文件职责与类名映射

| 文件 | 类名 | 职责 |
|------|------|------|
| `mapRotate.cs` | `mapRotate` | 核心：扫描 CSV、求平均风向/风速、旋转 `centralPoint`、全屏设置 |
| `change_wind.cs` | `change_wind` | 把风速写入粒子 `forceOverLifetime.z` 与速度上限 |
| `singleWindArea.cs` | `singleWindArea` | 分区 CSV 解析、粒子触发器染色 |
| `cloneWind.cs` | `cloneWind` | 克隆 150 个粒子点 |
| `cloneWindArea.cs` | `cloneWindArea` | 克隆 5×5 分区风场 |
| `particleSys.cs` | `particleSys` | 逐粒子位移 |
| `force.cs` | `force` | x 方向受力 AnimationCurve |
| `ParticleMove.cs` | `ParticleMove` | z 轴匀速平移 |
| `cameraMove.cs` | `camera_move` | 相机 WASD/右键/滚轮/中键漫游 |
| `rotateCompass.cs` | `compass` | 由相机朝向计算罗盘方位 |
| `CreateTerrain.cs` | `CreateTerrain` | 高度图转地形 Mesh |
| `CreateWindField.cs` | `createObject` | 批量创建圆柱体阵列 |
| `scrollbar_rotate.cs` | `scrollbar_rotate` | 旋转角滑条 |
| `scrollbar_speed.cs` | `scrollbar_speed` | 风速滑条 |
| `scrollbar_yMove.cs` | `scrollbar_yMove` | 高度滑条 |
| `winderDirSiderCtr.cs` | `winderDirSiderCtr` | 风向盘 Scrollbar + InputField 联动 |
| `showArror.cs` | `showArror` | 显示箭头层（cullingMask = -1） |
| `hideArror.cs` | `hideArror` | 隐藏箭头层（cullingMask &= ~(1<<6)） |
| `windAreaTrig.cs` | `windAreaTrig` | 触发区施加向上力 |
| `closeWindow.cs` | `closeWindow` | 退出应用 |

> 注意：部分文件名与类名不一致（如 `cameraMove.cs` -> `camera_move`、
> `CreateWindField.cs` -> `createObject`、`rotateCompass.cs` -> `compass`），
> 挂载脚本时以**类名**为准。

## 数据依赖

| 依赖 | 来源 | 使用方 |
|------|------|--------|
| `wind_speed` | `mapRotate` 字段 | `change_wind`、`scrollbar_speed` |
| `Text_path` 文本 | UGUI Text 对象 | `mapRotate.GetAllFiles()` |
| `centralPoint` 对象 | 场景对象 | `mapRotate`、`scrollbar_*` |
| `Particle System` 对象 | 场景对象 | `change_wind`、`force` |
| `windPT` 对象 | 场景对象 | `cloneWind` |
| `singleWindArea` 对象 | 场景对象 | `cloneWindArea` |
| `MainCamera` 对象 | 场景相机 | `camera_move`、`compass` |
| `Text_angle` / `Text_speed` / `Text_hight` | TextMeshProUGUI | 三个 scrollbar 脚本 |

## 数据流

```
CSV 数据目录（Text_path 指定）
        │  Directory.GetFiles(..., AllDirectories) -> files[Length-2]
        ▼
mapRotate.ImportCSV()
   │  逐行：去 -999.0:-999.0 → 跳过 length<=80 → 按 ',' 与 ':' 拆分 → 累加 x/y
   ▼
avg_x = x_total/count , avg_y = y_total/count
   ├── rotate_angle = atan(avg_x/avg_y) ──> centralPoint.localEulerAngles
   └── wind_speed = |tan(avg_x/avg_y)|
            │
            ▼
change_wind.Update() ──> forceOverLifetime.z / limitVelocityOverLifetime.limit = wind_speed + 2
            │
            ▼
ParticleSystem 粒子受力/限速 ──> 风场可视化
```

`singleWindArea` 走独立的 `csvFilePath`（默认 `D:/windData/1.txt`）解析路径，
结果写入 `global_wind_dir_x/y`。
