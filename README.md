# Unity-Work

## 项目概览

- 引擎版本：Unity 2022.3.46f1c1
- 项目类型：2D 横版动作平台游戏
- 核心场景：MainMenu、Scene 1

## 项目亮点

### 1. 玩家控制系统完整，强调动作手感
实现较完整的角色能力组合，包括移动、跳跃、冲刺、翻滚、墙体交互、攻击以及子弹时间机制，能够支撑较丰富的横版动作玩法。

### 2. 使用有限状态机组织角色与敌人逻辑
分别为玩家与敌人实现状态机：

- 玩家侧通过 FiniteStateMachine 管理多种动作状态切换
- 敌人侧通过 EnemyStateMachine 管理 Idle、Move、Attack、Dead 等行为

### 3. 存档系统
实现了基于接口收集的存档机制：

- 统一由 SaveManager 扫描场景内实现 ISaveManager 的对象
- 聚合角色位置、血量、收集物等关键数据到 GameData
- 使用 JsonUtility 序列化为 savefile.json

### 4. 子弹时间与冻结效果增强战斗反馈
实现了子弹时间系统，通过动态调整 Time.timeScale 和 Time.fixedDeltaTime 控制全局时间流速，并结合 UI 读条与输入退出条件。

### 5. 音频系统采用对象池思路管理 AudioSource
AudioManager 在初始化阶段预创建多个 AudioSource，并通过队列进行复用，避免频繁创建和销毁带来的性能浪费。

### 6. 丰富的关卡交互元素
- 弹跳板、弹跳平台
- 可破坏平台、可破坏墙体
- 移动平台、隐藏区域
- 宝石收集与 UI 展示
- 关卡出口、提示区域、房间触发器
- 陷阱、落石、上升气流等环境机制

