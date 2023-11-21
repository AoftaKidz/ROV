using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineManager : MonoBehaviour
{
    enum LineManagerState{
        None = 0,
        Create,
        Animate
    }
    public static LineManager Instance = null;
    public GameObject prefabDrawLine;
    List<GameObject> _lines = new List<GameObject>();
    int _currLine = -1;
    float _currTime = 0;
    float _duration = 2;
    float _delay = 0.3f;
    int _state = 0;
    float _turbo = 1;
    private void Awake()
    {
        Instance = this;
    }
    void Update()
    {
        switch (_state)
        {
            case (int)LineManagerState.None:
                {
                    break;
                }
            case (int)LineManagerState.Create:
                {
                    if (SlotMachineScatterMode.Instance.isWildSpawning) return;

                    _currTime += Time.deltaTime * _turbo;
                    if(_currTime > _delay)
                    {
                        _state = (int)LineManagerState.Animate;
                        _currTime = 0;

                        SlotMachine slot = SlotMachine.Instance;
                        BetModel slotData = slot.slotData;
                        int c = 0;
                        foreach (int type in slot.slotData.lines)
                        {
                            GameObject prefab = Instantiate(prefabDrawLine, Vector3.zero, Quaternion.identity);
                            List<int> line = CreateLineByType(type + 1);
                            prefab.GetComponent<DrawLine>().number = type + 1;
                            prefab.GetComponent<DrawLine>().reward = slotData.lineRewards[c];
                            prefab.GetComponent<DrawLine>().CreateLine(line);
                            if (slot.slotData.lines.Count == 1)
                                prefab.GetComponent<DrawLine>().ShowReward(true);
                            prefab.transform.parent = transform;
                            _lines.Add(prefab);
                            c++;
                        }

                        //Show Puzzle Match
                        for (int i = 0; i < 15; i++)
                        {
                            SlotMachine.Instance.puzzles[i].ShowMatch();
                        }

                        if (SlotMachine.Instance.slotData.reward > 0 || (slot.slotData.isScatterMode == false && slot.slotData.comingFreeSpinCount > 0))
                        {
                            SoundManager.Instance.PlaySFX("Match");
                            //FreeSpinAvatar.Instance.PlayWin();
                        }
                    }
                   break;
                }
            case (int)LineManagerState.Animate:
                {
                    if(!SlotMachine.Instance.IsComingScatterMode())
                        _currTime += Time.deltaTime;
                    if(_currTime > _duration)
                    {
                        _currTime = 0;

                        //Next Line
                        _currLine++;
                        if (_currLine >= _lines.Count)
                            _currLine = -1;

                        if(_currLine < 0)
                        {
                            //All line
                            
                            foreach (GameObject d in _lines)
                            {
                                d.SetActive(true);
                                d.GetComponent<DrawLine>().ShowReward(false);
                                if (_lines.Count == 1)
                                    d.GetComponent<DrawLine>().ShowReward(true);
                            }

                            //Set Puzzle alpha
                            if(_lines.Count > 0)
                            {
                                BetModel slot = SlotMachine.Instance.slotData;
                                for (int i = 0; i < 15; i++)
                                {
                                    bool isMatch = false;
                                    foreach (var match in slot.matches)
                                    {
                                        foreach (int d in match)
                                        {
                                            if (d == i)
                                            {
                                                isMatch = true;
                                                break;
                                            }
                                        }
                                        if (isMatch) break;
                                    }

                                    if (isMatch)
                                    {
                                        SlotMachine.Instance.puzzles[i].SetAlpha(1);
                                        /*SlotMachine.Instance.SetActivePuzzle(i);
                                        SlotMachine.activePuzzle.SetAlpha(1);*/
                                    }
                                    else
                                    {
                                        /*SlotMachine.Instance.SetActivePuzzle(i);
                                        SlotMachine.activePuzzle.SetAlpha(0.5f);*/
                                        SlotMachine.Instance.puzzles[i].SetAlpha(0.5f);
                                    }

                                }
                            }
                        }
                        else
                        {
                            foreach (GameObject d in _lines)
                            {
                                d.SetActive(false);
                            }

                            _lines[_currLine].SetActive(true);
                            _lines[_currLine].GetComponent<DrawLine>().ShowReward(true);

                            //Set Puzzle alpha
                            BetModel slot = SlotMachine.Instance.slotData;
                            List<int> match = slot.matches[_currLine];
                            for(int i = 0; i < 15; i++)
                            {
                                bool isMatch = false;
                                for(int k = 0; k < match.Count; k++)
                                {
                                    if (i == match[k])
                                    {
                                        isMatch = true;
                                        break;
                                    }
                                }

                                if (isMatch)
                                {
                                    /*SlotMachine.Instance.SetActivePuzzle(i);
                                    SlotMachine.activePuzzle.SetAlpha(1);*/
                                    SlotMachine.Instance.puzzles[i].SetAlpha(1);

                                }
                                else
                                {
                     /*               SlotMachine.Instance.SetActivePuzzle(i);
                                    SlotMachine.activePuzzle.SetAlpha(0.5f);*/
                                    SlotMachine.Instance.puzzles[i].SetAlpha(0.5f);

                                }

                            }
                        }
                    }

                   break;
                }
        }
    }
    private void OnEnable()
    {
        //Subscribe event
        SlotMachine.OnSlotColumnSpin += ClearLine;
        SlotMachine.OnSlotColumnStopSpin += Stop;
    }

    private void OnDisable()
    {
        //Unsubscribe event
        SlotMachine.OnSlotColumnSpin -= ClearLine;
        SlotMachine.OnSlotColumnStopSpin -= Stop;
    }
    public void CreateLine()
    {
        if (SlotMachine.isTurboMode)
            _turbo = 4;
        else
            _turbo = 1;
        _state = (int)LineManagerState.Create;
        _currTime = 0;

        //Destryo line
        foreach (GameObject d in _lines)
        {
            Destroy(d);
        }
        _lines.Clear();
    }
    public void ClearLine()
    {
        _state = (int)LineManagerState.None;
        _currTime = 0;

        //Destryo line
        foreach (GameObject d in _lines)
        {
            Destroy(d);
        }
        _lines.Clear();
    }
    void Stop() 
    {
        _currTime = _delay - 0.5f;
    }
    List<int> CreateLineByType(int type)
    {
        //Type 1,2,...,15
        switch (type)
        {
            case 1:
                {
                    return new List<int>() { 1,4,7,10,13};
                }
            case 2:
                {
                    return new List<int>() { 0, 3, 6, 9, 12 };
                }
            case 3:
                {
                    return new List<int>() { 2, 5, 8, 11, 14 };
                }
            case 4:
                {
                    return new List<int>() { 0, 4, 8, 10, 12 };
                }
            case 5:
                {
                    return new List<int>() { 2, 4, 6, 10, 14 };
                }
            case 6:
                {
                    return new List<int>() { 0, 3, 7, 9, 12 };
                }
            case 7:
                {
                    return new List<int>() { 2, 5, 7, 11, 14 };
                }
            case 8:
                {
                    return new List<int>() { 1, 5, 8, 11, 13 };
                }
            case 9:
                {
                    return new List<int>() { 1, 3, 6, 9, 13 };
                }
            case 10:
                {
                    return new List<int>() { 0, 4, 7, 10, 12 };
                }
            case 11:
                {
                    return new List<int>() { 2, 4, 7, 10, 14 };
                }
            case 12:
                {
                    return new List<int>() { 1, 4, 6, 10, 13 };
                }
            case 13:
                {
                    return new List<int>() { 1, 4, 8, 10, 13 };
                }
            case 14:
                {
                    return new List<int>() { 0, 5, 8, 11, 12 };
                }
            case 15:
                {
                    return new List<int>() { 2, 3, 6, 9, 14 };
                }
            default:
                {
                    return new List<int>() {0};
                }
        }
    }
}
