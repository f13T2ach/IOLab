## IOLab 存档与实验结构

### 实验

每个实验是单个json文件，存储在Assets/LocalExperiments下

#### 实例

```json
{
  "name": "新制氯水的变质速率探究",
  "experiment": "liquid_cl2_modification",
  "tags": [ "长时实验" ],
  "services": [100001],
  "formula": "",
  "steps": {
    "一、准备": "准备新制氯水溶液、配套设备、烧杯",
    "二、取样": "取 50mL 新制氯水溶液到烧杯中",
    "三、采集": "放置探头到液面以下，采集24小时",
    "四、分析": "分析图象，讨论这段时间后是否达到化学平衡"
  },
  "checkpoints": [
    {
      "name": "三、采集",
      "freeze": [ 100001 ],
      "require": [ 100001 ]
    }
  ],
  "repeatPins": false,
  "description": "通过 pH 传感器探究新制氯水的变质"
}
```

#### 解析

**name**：实验名称（实际展示的名称会带有标签）

**experiment**：实验包名，须与文件名一致

**tags**：标签名称

**services**：需要使用的传感器

**formula**：化学方程式，可不填，但随机推荐不会推荐此项

**steps**：步骤字典

**checkpoints**：检查点数组

**checkpoints[index].name**：检查点对应的步骤

**checkpoints[index].freeze**：此检查点完成后冻结的表格

**checkpoints[index].require**：此检查点完成后检查的表格

**repeatPins**：检查点是否会require重复的表，如果是，则每回溯一次就删除上一步被冻结的表

**description**：简介，可不填，会根据需要的传感器自动生成

### 采集器

所有采集器都在一个json文件内，存储在Assets/local_services.json内

#### 实例

```json
[
  {
    "name": "氧气传感器",
    "serviceID": 100001,
    "type": "I2C",
    "yrow": "ppm%"
  },
  {
    "name": "pH传感器",
    "serviceID": 100002,
    "type": "ALG",
    "yrow": "pH"
  }
]
```



**[index].name**：传感器名称，用于检索

**[index].serviceID**：传感器的唯一标识符

**[index].type**：传感器的接口类型

**{index}.yrow**：单位，**不能**是中文

### 记录

暂未制作

