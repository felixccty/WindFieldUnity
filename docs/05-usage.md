# 使用示例

## 1. CSV 数据格式

`mapRotate.ImportCSV()` 与 `singleWindArea.ImportCSV()` 读取同一类格式：
每行由若干字段组成，字段间以**逗号**分隔，每个字段是 `x:y` 形式的矢量分量。

```csv
0.512:0.833,0.497:0.812,-999.0:-999.0,0.531:0.845
0.508:0.821,0.502:0.806,0.521:0.839,0.518:0.830
0.487:0.795,0.495:0.810,0.503:0.822,0.499:0.818
```

处理规则（与源码一致）：
1. 每行先替换 `-999.0:-999.0` 为空串（无效观测标记）；
2. `line.Length <= 80` 的行被跳过；
3. 按 `,` 拆字段，跳过空字段；
4. 每字段按 `:` 拆分，取 `[0]` 为 x、`[1]` 为 y，`float.Parse()` 后累加；
5. 求平均后得到 `avg_x` / `avg_y`。

## 2. 路径配置

- **主风场（mapRotate）**：路径来自场景对象 `Text_path` 的 `Text.text`，
  `GetAllFiles()` 对目录做 `Directory.GetFiles(path, "*.*", SearchOption.AllDirectories)`，
  返回 `files[files.Length - 2]`。
- **分区风场（singleWindArea）**：源码中固定为 `D:/windData/1.txt`，
  部署时请改为实际数据文件路径后重新编译。

## 3. 典型操作流程

1. 启动场景：`mapRotate.Start()` 读取 `Text_path`，设置全屏。
2. `FixedUpdate()` 周期读取最新 CSV 并调用 `ImportCSV()`。
3. 观察 `centralPoint` 随 `rotate_angle` 旋转，`Text_path` 对应的风场随之转向。
4. 拖动 `scrollbar_speed` 手动覆盖风速（`value * 100` m/s）；
   拖动 `scrollbar_rotate` 手动设置旋转角（`value * 360` 度）；
   拖动 `scrollbar_yMove` 调整 `centralPoint` 高度（`100 - value*100`）。
5. WASD / 鼠标右键 / 滚轮 / 中键操作相机漫游，罗盘随相机朝向指示方位。
6. 用 `showArror` / `hideArror` 切换箭头层显示。
7. 退出按钮绑定 `closeWindow.testClick()`（调用 `Application.Quit()`）。

## 4. 分区风场

`cloneWindArea` 以 `singleWindArea` 为模板克隆 5×5=25 个区域，
`cloneWind` 以 `windPT` 为模板克隆 150 个粒子点沿 x 轴排布。
进入 `singleWindArea` 触发区的粒子被染为红色，退出时染为绿色（`OnParticleTrigger()`）。

## 5. 地形

给挂载 `CreateTerrain` 的对象指定 `heightMap` 纹理后，
`Generate()` 按红色通道生成高度（`r/255 * height`），网格分辨率 `N=200`，
高度图采样分辨率 `NMap=512`。修改 `N` / `height` / `heightMap` 后可在 Inspector 调整并重新运行。
