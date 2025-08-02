<div align="center"><img height=256 src="https://www.helloimg.com/delete/rCCiKp/75d3d126b512d3a791d5e5de4181a557"/></div>

<div align="center"><h3>IOLab</h3></div>

<div align="center">Full-Stack Experimental Learning Platform</div>

> [!NOTE]
>
> Project under development, does not represent final quality
>
> Dissemination of code copies is prohibited before patent protection is obtained

### Definition

「Cheng Long」(in Chinese, 成龙)  is spelled as such to reflect the developer's name, symbolizing to integrate parts into a complete system and vigorous advancement (as implied by the Chinese idioms "配套成龙" and "鱼跃龙门"). The English name of the project is **IOLab**.

「Cheng Long」 Smart Laboratory (IOLab) , English name IO Experiment, is a learning platform architecture based on experimental data. It features a unique "Explore-Practice-Summarize" experimental learning approach through hardware-software integration, along with powerful "community" functionality.

Go to -> [中文](https://github.com/f13T2ach/IOLab/blob/master/README_en.md) [Q&A](https://github.com/f13T2ach/IOLab/blob/master/IOLab_QA.md) | [Folder Structure](https://github.com/f13T2ach/IOLab/blob/master/README_FolderStructure.md) [Quick Start](https://github.com/f13T2ach/IOLab/blob/master/IOLab_Quick_Guide.md) 

### Features

- Upper and lower computer collaborative operation
- Innovative teaching practice methodology
- AI assistant for intelligent experiment summarization
- Cloud-based super<sup>[1]</sup> experimental learning platform
- Deep system UI integration with smooth interactive experience
- Low-coupling, high-cohesion software kernel for easy extension
- Sustainability and eco-friendliness

### Experimental Subjects

- Electrical physics experiments and partial mechanical physics experiments
- Experimental investigations related to chemical reaction data changes
- Partial biological experiments

### 「Explore-Practice-Summarize」

**Pending modification**

The Explore-Experiment-Summarize approach is the core teaching methodology advocated by this project. Guided by the new curriculum standards, the teaching application of experiments has transcended traditional classroom boundaries, becoming a crucial pathway for cultivating students' inquiry abilities, innovative spirit, and practical skills. This shift reflects not only the importance placed on experimental technology innovation in modern science education but also the growing demand for comprehensive student competency development.

In current educational trends, experiments are no longer merely tools for knowledge verification but have become central to the student learning process. Through exploration, students can spark curiosity and actively seek answers; through experimentation, they can operate hands-on, observe phenomena, and validate hypotheses; through summarization, they can synthesize experimental results, form scientific concepts, and develop thinking skills.

Under the influence of this educational trend, we pioneered the development of the 「Cheng Long」 Smart Laboratory kit. This kit aims to provide students with a complete growth environment for exploration, experimentation, and summarization. Students can conduct various electrical, mechanical, chemical, and biological experiments in the 「Cheng Long」 Smart Laboratory, personally experiencing the joy of scientific exploration. Simultaneously, the community feature allows students to share experiments, further promoting growth and progress.

In summary, the "Explore-Practice-Summarize" teaching methodology not only aligns with the new curriculum concepts but also adapts to the development trends of modern science education. Through the application of the 「Cheng Long」 Smart Laboratory kit, we anticipate students' continuous growth under this teaching approach, becoming future scientists and innovators with inquiry spirit and practical capabilities.

### Architecture

「Cheng Long」 Smart Laboratory consists of three parts: IONet, IOLab, and IOCore. It is primarily developed using C# and C++.

#### IONet

IONet is the interface implementation for community services (IOLab Community). Developed using C# and C++, the C# component implements all community functionalities, while the C++ component handles functions like firmware updates. IONet is a crucial manifestation of the creativity and extensibility of 「Cheng Long」 Smart Laboratory.

The online component of IONet includes a **blacklist module** that matches users' serial numbers. When users abuse functionalities, their serial numbers can be added to the blacklist.

When this project goes public, the official IONet server will terminate services to avoid potential content review risks.

**Note**: IONet uses a pseudo-database implemented through code hosting platform APIs. This storage method is constrained by the respective platforms, so IONet strictly reviews text content in user-uploaded experiments and limits the number of online experiments (capped at 1000).

#### IOLab

IOLab is the upper-computer software for driving data acquisition instruments, developed using the Windows Presentation Foundation (WPF) framework. It consists of three sections: "Experiments," "Monitor," and "Devices," with additional AI assistant functionality that analyzes completed records to assist with experiment summarization.

**AI Assistant**  
The AI assistant is based on Alibaba Cloud's Tongyi Qianwen large language model. The program automatically aggregates data into query prompts to intelligently summarize experiments.

##### Experiments
IOLab randomly recommends experiments to users from the experiment library. Here, "experiments" refer to collections including observation subjects, relevant chemical reaction equations (or ionic equations), and reaction principles.

![](https://imgos.cn/2024/08/13/66bb64826e616.png)

##### Monitor
The monitor plots charts from data acquired by instruments.

![](https://imgos.cn/2024/08/13/66bb6482773c6.png)

##### Devices
The devices section is the management page for data acquisition instruments.

![](https://imgos.cn/2024/08/13/66bb648251c6b.png)

#### IOCore

IOCore is the firmware for lower-computer devices, developed in C++. It establishes connections with IOLab through serial communication. IOCore can be deployed on the following MCUs:
- Arduino (not recommended, lacks network functionality)
- Espressif 8266 or 32 series development boards (NodeMCU development board as standard)

### Installation Requirements

#### IOLab
**Recommended configuration**: Minimum 512MB disk space, minimum 4GB available RAM, 64-bit architecture processor, dedicated graphics card, Windows 10 operating system.

#### IOCore
**Recommended configuration**: Minimum 2MB Flash space Espressif-32E

### Precautions and Notes

#### Precautions
1. Technical difficulty ranking: Compiling IOLab < Building IONet < Assembling acquisition instruments. Instrument assembly falls under electronic DIY. To add new sensors, manually write drivers (refer to sample_sensor.c)
2. The 「Cheng Long」 Smart Laboratory software is completely free, but physical devices and network features relying on paid servers may incur charges
3. After project completion, it will be open-sourced under GPL3.0

#### Notes
<sup>[1]</sup>Cloud-based super: Refers to the experiment package database hosted on platforms supporting large-scale data embedding
