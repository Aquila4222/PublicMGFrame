# PublicMGFrame

## 项目结构

以下是音游判定相关的核心代码：

- `Assets/Scripts/NoteJudgment/Note` ：Note类，每一个实例代表一个Note。
- `Assets/Scripts/NoteJudgment/NotesTrack` ：Note轨道，控制所有Note的判定。
- `Assets/Scripts/TrackController/TrackController` ：单例，控制NoteTrack、音乐播放和时间轴。

制谱器相关的代码在 `Assets/Scripts/ChartMaker` 中，但是写这个的程序几乎没写注释，阅读起来比较困难



## 致谢与许可证

- **源代码**：采用 MIT 许可证，详见 [LICENSE](LICENSE)。
- **美术/模型/音效等**：由 [开发者团队] 创作，保留所有权利，详见 [ASSETS_LICENSE.md](ASSETS_LICENSE.md)。
- **音乐**：由 [曲师] 创作并授权使用，仅限本项目内使用，未经许可不得用于其他用途，详见 [ASSETS_LICENSE.md](ASSETS_LICENSE.md)。
