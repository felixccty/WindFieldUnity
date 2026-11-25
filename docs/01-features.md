# 功能说明

本文档逐项说明 `WindFieldUnity` 的实际功能，类名/函数名与源码一致。

## 1. 风场 CSV 读取与解析

核心脚本 `mapRotate`（挂载在 `centralPoint`）：

| 函数 | 作用 |
|------|------|
| `Start()` | 从 UI 对象 `Text_path` 读取根目录文本；设置最高分辨率并全屏；初始化 `wind_speed = 10` |
| `FixedUpdate()` | 周期重新读取路径、`GetAllFiles()` 取最新文件、`ImportCSV()` 解析 |
| `GetAllFiles()` | `Directory.GetFiles(rootFilePath_text, "*.*", AllDirectories)`，返回 `files[files.Length - 2]` |
| `GetFilePath(path)` | 列出子目录名并返回最后一个（辅助函数） |
| `ImportCSV()` | 解析 CSV，计算平均分量、风向角与风速，并设置 `transform.localEulerAngles` |

`ImportCSV()` 关键处理：
- 每行先 `line.Replace("-999.0:-999.0", "")` 去除无效值标记；
- `line.Length <= 80` 的行跳过；
- 按 `,` 拆分字段，再按 `:` 拆为 `data_xy[0]`（x）与 `data_xy[1]`（y），累加 `x_total` / `y_total` / `total_count`；
- 计算 `avg_x_total = x_total / total_count`、`avg_y_total = y_total / total_count`；
- `wind_speed = Mathf.Abs(Mathf.Tan(avg_x_total / avg_y_total))`；
- `rotate_angle = Mathf.Atan(avg_x_total / avg_y_total)`，赋给 `transform.localEulerAngles = new Vector3(0, rotate_angle, 0)`。

## 2. 风速驱动粒子

- `change_wind.Update()`：读取 `readDataScript.wind_speed`（即 `mapRotate` 的 `wind_speed`），
  设置 `ParticleSystem.forceOverLifetime.z = wind_speed + 2f` 与
  `limitVelocityOverLifetime.limit = wind_speed + 2f`。
- `force.Update()`：构造 `AnimationCurve`（`0.0 -> 0.1`、`0.75 -> 1.0`），
  以 `new ParticleSystem.MinMaxCurve(1.5f, curve)` 设置 `forceOverLifetime.x`。
- `particleSys.Update()`：`GetParticles(m_Particles)` 取存活粒子，
  逐个 `position += Vector3.right * windStrength * Time.deltaTime`，再 `SetParticles()` 写回。
- `ParticleMove.Update()`：`transform.Translate(0, 0, 0.1f)` 沿 z 轴匀速平移。

## 3. 分区风场

| 脚本 | 作用 |
|------|------|
| `singleWindArea` | `ImportCSV()` 解析 `csvFilePath`（默认 `D:/windData/1.txt`）求 `global_wind_dir_x/y`；`OnParticleTrigger()` 用 `GetTriggerParticles` / `SetTriggerParticles` 把进入触发器粒子染红、退出染绿 |
| `cloneWindArea` | 以 `singleWindArea` 为模板克隆 5×5 网格（间距 60，带 `Euler(i*20, j*20, k*20)` 旋转） |
| `cloneWind` | 以 `windPT` 为模板克隆 150 个点，位置 `(i*4)-300, 100, (k*2)-400` |
| `windAreaTrig` | `OnTriggerStay()` 对进入的刚体 `AddForce(Vector3.up * 100)` |

## 4. 相机与罗盘

- `camera_move`（cameraMove.cs）：
  - WASD 键平移（`Vector3.forward/back/left/right * 2 * Time.deltaTime`）；
  - 鼠标右键拖拽旋转（`xSpeed=250`、`ySpeed=120`、`yMinLimit=-360`、`yMaxLimit=360`）；
  - 滚轮缩放（`MouseWheelSensitivity=300`、`MouseZoomMin=1`、`MouseZoomMax=2000`、初始 `Distance=1200`）；
  - 鼠标中键屏幕平移（`WorldToScreenPoint` / `cameraX` / `cameraY`）；
  - `ClampAngle()` 把角度限制在 `[min, max]`。
- `compass`（rotateCompass.cs）：取 `MainCamera.forward` 并清零 y 后归一化，
  减去参考向量 `kReferenceVector = (0,0,1)`，`Atan2(x, z) * Rad2Deg - 90` 再乘 2，
  以 `Quaternion.AngleAxis(_mTempAngle, kReferenceVector)` 旋转罗盘。

## 5. 地形与对象生成

- `CreateTerrain.Generate()`：`ClearMeshData()` 后 `AddMeshData()`，
  从 `heightMap.GetPixels32()` 的红色通道取高度（`colors[zz * NMap + xx].r / 255f * height`），
  按 `N=200` 网格生成顶点、三角形索引，`RecalculateNormals()` / `RecalculateBounds()` 后赋给 `meshFilter.mesh`。
- `createObject.Start()`（CreateWindField.cs）：三层 10×10×10 循环创建 `PrimitiveType.Cylinder`，
  `localScale = new Vector3(1,8,1) / 30`，按 `(i*10, j*10, k*10)` 设置旋转。

## 6. UI 交互

| 脚本 | 作用 |
|------|------|
| `scrollbar_rotate` | `value * 360` 度，`Quaternion.Lerp` 平滑旋转 `centralPoint`，`TextMeshProUGUI` 显示角度 |
| `scrollbar_speed` | `value * 100` 写入 `readDataScript.wind_speed`，显示 `m/s` |
| `scrollbar_yMove` | `(int)(value * 100)`，设置 `centralPoint.position = (0, 100 - change, 0)` |
| `winderDirSiderCtr` | Scrollbar + InputField 双向绑定，`Init(min,max,cur,decimal)` 与 `SetValue()` 含 `Mathf.Clamp` 与小数位截断 |
| `showArror` / `hideArror` | `Camera.main.cullingMask = -1` 显示第 6 层；`&= ~(1 << 6)` 隐藏 |
| `closeWindow` | `testClick()` 调用 `Application.Quit()` |
