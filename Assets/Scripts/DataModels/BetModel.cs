using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
/*{ "statusCode":401,"message":"Unauthorized"}*/
[Serializable]
public class ErrorModel
{
    public int statusCode;
    public string message;
    public static ErrorModel FromJSON(string jsonString)
    {
        return JsonUtility.FromJson<ErrorModel>(jsonString);
    }
}
[Serializable]
public class BetModel
{
    public string user { get; set; }
    public float v { get; set; }
    public List<int> data { get; set; }
    public bool isScatterMode { get; set; }
    public int scatterCount { get; set; }
    public int comingFreeSpinCount { get; set; }
    public int combo { get; set; }
    public float totalReward { get; set; }
    public string matchId { get; set; }
    public List<List<int>> matches { get; set; }
    public List<int> lines { get; set; }
    public List<float> lineRewards { get; set; }
    public bool isFiveOfKind { get; set; }
    public float reward { get; set; }
    public float winRatio { get; set; }
    public double userBalance { get; set; }
    public bool wildEnded { get; set; }
    public int scatterMultiplier { get; set; }
    public int wildSpawnIndex { get; set; }
    public int ballColor { get; set; }
    public bool wildCleared { get; set; }

}

