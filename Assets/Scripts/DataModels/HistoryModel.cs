using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class HistoryModelCombo
{
    public string id { get; set; }
    public List<int> data { get; set; }
    public int bet { get; set; }
    public List<List<int>> matches { get; set; }
    public List<int> lines { get; set; }
    public List<double> lineRewards { get; set; }
    public bool isScatterMode { get; set; }
    public double reward { get; set; }
    public double totalReward { get; set; }
    public bool isFiveOfKind { get; set; }
    public double winRatio { get; set; }
    public int comingFreeSpinCount { get; set; }
    public string created { get; set; }
    public int maxCombo;
}

[Serializable]
public class HistoryModelData
{
    public int page { get; set; }
    public int size { get; set; }
    public int total { get; set; }
    public List<HistoryModelTransaction> transactions { get; set; }
}

[Serializable]
public class HistoryModel
{
    public bool success { get; set; }
    public HistoryModelData data { get; set; }
}

[Serializable]
public class HistoryModelTransaction
{
    public string _id { get; set; }
    public int maxCombo { get; set; }
    public string start { get; set; }
    public List<HistoryModelCombo> combo { get; set; }
}
