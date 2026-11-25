# 注意事项与已知限制

## 1. 源码编码

`.cs` 中的中文注释为 **GBK** 编码，在按 UTF-8 打开的编辑器中会显示为乱码，
但注释不参与编译，不影响运行。发布包按字节脱敏并保持原编码，请勿整体转码。

## 2. 固定路径与对象名

| 项 | 位置 | 说明 |
|----|------|------|
| `D:/windData/1.txt` | `singleWindArea.csvFilePath` | 分区风场默认数据路径，需按实际修改后重新编译 |
| `Text_path` | `mapRotate` | 主风场 CSV 根目录，由场景 UGUI Text 的 `text` 提供 |
| `centralPoint` / `MainCamera` / `Particle System` / `windPT` / `singleWindArea` | 各脚本 `GameObject.Find()` | 场景对象必须按此命名，否则返回 null 并报错 |

这些是程序约定，非发布者本机私有路径；如需改动，请修改源码或场景对象名后重新构建。

## 3. 数据文件选择不稳定

`mapRotate.GetAllFiles()` 取 `Directory.GetFiles()` 返回数组的 `files[files.Length - 2]`。
该顺序依赖文件系统枚举顺序，并非按时间排序；目录内文件数不足 2 个时会越界。
实际部署建议确保目录内容可控，或改写为按修改时间取最新文件。

## 4. 计算的边界情况

- `wind_speed = |tan(avg_x / avg_y)|`，当 `avg_y` 接近 0 时结果会非常大；
- `rotate_angle = atan(avg_x / avg_y)` 返回**弧度**，却直接赋给 `localEulerAngles`（Unity 使用**角度**），
  源码未做 `Rad2Deg` 转换，旋转量纲存在不一致；如需准确角度请自行补上转换。

## 5. 重复注册回调

`scrollbar_rotate` / `scrollbar_speed` / `scrollbar_yMove` 在 `Update()` 中每帧调用
`onValueChanged.AddListener(...)`，会重复注册监听，可能导致回调被多次触发。
建议把注册移到 `Start()`。

## 6. 依赖与素材

- 本包只含脚本，不含 `.unity` 场景与 ProjectSettings/Packages 配置，无法直接打开即运行；
- 原始地理 DXF、卫星底图与上游风场 CSV 需用户自备；
- `JetBrains.Annotations` 仅用于 `singleWindArea` 的注解，缺失时可移除对应 `using`；
- `CreateTerrain` 需自带高度图纹理，否则 `heightMap.GetPixels32()` 会报错。

## 7. 素材取舍

发布包中的截图已人工核验，不含任何品牌水印；
源目录中含旧品牌水印的截图及大体积录屏均未纳入。
`resources/` 仅保留能说明项目效果的最小素材集。
