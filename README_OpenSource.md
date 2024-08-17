<div align="center"><img height=256 src="https://imgos.cn/2024/08/13/66bb66b870fa7.png"/></div>

<div align="center"><h3>IOExperiment</h3></div>

<div align="center">全栈实验学习平台</div>

### 定义

IO Experiment，是以软硬件联合的方式、独出一辙的“探索 实践 总结”实验学习路线和强大的“社区”功能构建基于实验数据的学习平台架构。

IOExperiment的旧称是成龙智慧实验室，如果你需要查看成龙智慧实验室是竞赛用README.md，阅读本目录下的README_Edu.md

### 构建教程

### 架构

「成龙」智慧实验室分为三个部分：IONet、IOLab、IOCore。「成龙」智慧实验室主要使用C#和C语言开发。

#### IOLab

IOLab 是驱动采集仪器的上位机，由Windows Presentation Foundation (WPF)框架开发，由“实验”、“监视”和“设备”三个部分组成。另有AI助手功能，可以解析已完成的记录，以帮助进行实验总结。

**AI 助手**

AI 助手基于阿里云研发的通义千问大语言模型，程序自动汇总数据为提问词，智能进行实验总结。

##### 实验

IOLab 会向用户从实验库中随机推荐一个实验。这里的“实验”是监视对象、涉及的化学反应方程式（或离子方程式）、反应原理组成的集合；

![](https://imgos.cn/2024/08/13/66bb64826e616.png)

##### 监视

监视能从采集仪器的数据中绘制图表；

![](https://imgos.cn/2024/08/13/66bb6482773c6.png)

##### 设备

设备是管理采集仪器的页面；

![](https://imgos.cn/2024/08/13/66bb648251c6b.png)

#### IOCore

IOCore是上位机的固件，由C语言开发而成，采用串口通信的方式与 IOLab 建立连接。可以在以下MCU部署 IOCore

- Arduino（不推荐，无法使用联网功能）
- Espressif 8266 或 32 系列开发板（以NodeMCU开发板为标准）

### 注释

<sup>[1]</sup>云端超级：指寄托在托管平台的支持大型数据嵌入的实验包数据库
