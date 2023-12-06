using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Spine.Unity;

public class SlotMachineScatterMode : MonoBehaviour
{

   /* static public event Action<int> OnAutoSpin;
    static public event Action OnStopAutoSpin;
    static public event Action<int> OnEndterAutoSpin;*/
   enum ScatterState
    {
        None = 0,
        Start,
        SpawnWildPuzzle,
        SpawnWildTall,
        WaitForSpin,
        Spinning,
        Matching,
        End,
        ClickForSpin
    }
    int _count = 0;
    int _current = 0;
    bool _isStart = false;
    float _time = 0;
    float _delay = 1;
    ScatterState _state = ScatterState.None;

    public static SlotMachineScatterMode Instance { get; private set; }
    public GameObject prefabPuzzle;
    public GameObject prefabWildTall;
    public GameObject spawnPuzzleLocation;
    bool _isSpawnWildPuzzle = false;
    GameObject puzzle = null;
    public bool isWildSpawning = false;
    public GameObject topFX;
    public int lastScatterCount = 0;
    public GameObject spawnWildSaberEffect;
    public GameObject thunder;

    // Start is called before the first frame update
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }
    private void Start()
    {

    }
    private void Update()
    {
        HandleState();
    }
    void HandleState()
    {
        switch (_state)
        {
            case ScatterState.None:
                {
                    break;
                }
            case ScatterState.Start:
                {
                    _time += Time.deltaTime;
                    if(_time > 0.3f)
                    {
                        _time = 0;
                        _state = ScatterState.WaitForSpin;

/*                        if (SlotMachine.Instance.slotData.wildSpawnIndex < 0)
                            _state = ScatterState.WaitForSpin;
                        else
                            _state = ScatterState.SpawnWildPuzzle;*/
                    }
                    break;
                }
            case ScatterState.SpawnWildPuzzle:
                {
                    isWildSpawning = true;
                    if (_isSpawnWildPuzzle)
                    {

                    }
                    else
                    {
                        _time += Time.deltaTime;
                        //Debug.Log("SpawnWildPuzzle : " + _time);
                        if (_time > 1.3f)
                        {
                            //UIRoundRewardPopup.Instance.Hide();
                            //SoundManager.Instance.PlaySFX("IncreaseDecrease");
                            //particleCounting.time = 1;

                            //UIGameplay.Instance.UpdateScateMode(SlotMachine.Instance.slotData.scatterCount, SlotMachine.Instance.slotData.scatterMultiplier);
                            _time = 0;
                            _isSpawnWildPuzzle = true;
                            Vector2 p = spawnPuzzleLocation.transform.position;
                            p.y += 2.5f;
                            puzzle = Instantiate(prefabPuzzle, p, Quaternion.identity);
                            //SlotMachine.Instance.SetActivePuzzle(SlotMachine.Instance.slotData.wildSpawnIndex);
                            Vector3 pos = SlotMachine.Instance.puzzles[SlotMachine.Instance.slotData.wildSpawnIndex].transform.position; //SlotMachine.activePuzzle.transform.position;
                           

                            puzzle.transform.localScale = Vector3.zero;
                            puzzle.transform.DOScale(0, 0.5f).SetEase(Ease.OutElastic).OnComplete(() =>
                            {
                                puzzle.transform.localScale = Vector3.zero;
                                //particleCounting.Play();
                                //particleThunder.Play();
                                AddScatterCount(-1);

                                //particleThunder.transform.position = pos;
                                SoundManager.Instance.PlaySFX("WildShot");
                                
                                {
                                    SoundManager.Instance.PlaySFX("Lightning");
                                    spawnWildSaberEffect.SetActive(true);
                                    thunder.SetActive(true);
                                    Vector3 o = spawnWildSaberEffect.transform.position;
                                    Vector3 p = pos - spawnWildSaberEffect.transform.position;
                                    float deg = 180f + Mathf.Atan2(p.y, p.x) * Mathf.Rad2Deg;
                                    /*if (deg > 0)
                                        deg = -deg;*/
                                    Debug.Log("Degree : " + deg);

                                    Quaternion rot = new Quaternion();
                                    rot.eulerAngles = new Vector3(0, 0, deg);
                                    spawnWildSaberEffect.transform.rotation = rot;
                                    //thunder.transform.rotation = rot;
                                    {
                                        float _s = CalculateThunderScale(pos);
                                        Vector3 s = new Vector3(_s,_s,_s);
                                        thunder.transform.localScale = s;
                                        thunder.GetComponent<SkeletonAnimation>().AnimationState.SetAnimation(0,"Lightning_Activate", false);
                                    }

                                    //spawnWildSaberEffect.transform.DOMove(pos, 0.5f).SetEase(Ease.InQuint).OnComplete(() => {
                                    spawnWildSaberEffect.transform.DOMove(o, 0.5f).SetEase(Ease.InQuint).OnComplete(() => {
                                            spawnWildSaberEffect.SetActive(false);
                                            thunder.SetActive(false);
                                        spawnWildSaberEffect.transform.position = o;

                                        puzzle.transform.DOMove(pos, 0.0f).SetEase(Ease.OutQuint).OnComplete(() =>
                                        {
                                            _state = ScatterState.SpawnWildTall;

                                            SlotMachine.Instance.slotData.data[SlotMachine.Instance.slotData.wildSpawnIndex] = (int)SlotMachine.SlotMachineID.Puzzle_Wild;

                                            SlotMachine.Instance.puzzles[SlotMachine.Instance.slotData.wildSpawnIndex].SetPuzzleData();
                                            SlotMachine.Instance.puzzles[SlotMachine.Instance.slotData.wildSpawnIndex].SetAlpha(1);
                                            SlotMachine.Instance.puzzles[SlotMachine.Instance.slotData.wildSpawnIndex].JellyEffect();
                                            SlotMachine.Instance.puzzles[SlotMachine.Instance.slotData.wildSpawnIndex].WildSpawnFX();

                                            //SlotMachine.activePuzzle.SetPuzzleData();
                                            //SlotMachine.activePuzzle.SetAlpha(1);
                                            SlotColumn slotColumn = SlotColumn.GetSlotColumn(SlotMachine.Instance.puzzles[SlotMachine.Instance.slotData.wildSpawnIndex].columnID);
                                            slotColumn.CreateWildTall(true);
                                        });

                                    }); 
                                }
                                /*puzzle.transform.DOMove(pos, 0.0f).SetEase(Ease.OutQuint).OnComplete(() => {
                                    _state = ScatterState.SpawnWildTall;

                                    SlotMachine.Instance.slotData.data[SlotMachine.Instance.slotData.wildSpawnIndex] = (int)SlotMachine.SlotMachineID.Puzzle_Wild;

                                    SlotMachine.Instance.puzzles[SlotMachine.Instance.slotData.wildSpawnIndex].SetPuzzleData();
                                    SlotMachine.Instance.puzzles[SlotMachine.Instance.slotData.wildSpawnIndex].SetAlpha(1);
                                    SlotMachine.Instance.puzzles[SlotMachine.Instance.slotData.wildSpawnIndex].JellyEffect();

                                    //SlotMachine.activePuzzle.SetPuzzleData();
                                    //SlotMachine.activePuzzle.SetAlpha(1);
                                    SlotColumn slotColumn = SlotColumn.GetSlotColumn(SlotMachine.Instance.puzzles[SlotMachine.Instance.slotData.wildSpawnIndex].columnID);
                                    slotColumn.CreateWildTall(true);
                                });*/
                            });
                            
                        }
                    }
                    
                    break;
                }
            case ScatterState.SpawnWildTall:
                {
                    _time += Time.deltaTime;
                    if (_time > 3f)
                    {
                        _time = 0;
                        _state = ScatterState.WaitForSpin;
                        Destroy(puzzle);
                        isWildSpawning = false;

                        //Update collectible
                        CreateCollectableHeart();
                    }
                    break;
                }
            case ScatterState.WaitForSpin:
                {
                    if (UIKindOfMeowPopup.Instance.Appear()) return;
                    if (UIBigWinPopup.Instance.Appear()) return;
                    if (UIRoundRewardPopup.Instance.Appear()) return;

                    _time += Time.deltaTime;
                    float delay = 1;
                    if (_time > delay)
                    {
                        if (!SlotMachine.Instance.slotData.wildEnded)
                        {
                            if (SlotMachine.isFreeSpinModeAuto)
                            {
                                AutoSpin();
                            }
                            else
                            {
                                _state = ScatterState.ClickForSpin;
                            }
                        }
                        else
                        {
                            //End
                            isWildSpawning = false;
                            _state = ScatterState.End;
                        }
                        _time = 0;
                    }
                    break;
                }
            case ScatterState.ClickForSpin:
                {
                    //Wait user click to spin
                    if (!SlotMachine.Instance.slotData.wildEnded && SlotMachine.isFreeSpinModeAuto)
                    {
                        AutoSpin();
                    }
                    else if (SlotMachine.Instance.slotData.wildEnded)
                    {
                        //End
                        isWildSpawning = false;
                        _state = ScatterState.End;
                    }
                    break;
                }
            case ScatterState.Spinning:
                {
                    if (SlotMachine.isSpinning)
                    {
                        //Spinning
                        Debug.Log("Spinning....");
                    }
                    else
                    {
                        UpdateScatterCount();

                        if (SlotMachine.Instance.slotData.wildSpawnIndex < 0)
                        {
                            _state = ScatterState.WaitForSpin;
                            isWildSpawning = false;
                            CreateCollectableHeart();
                        }
                        else
                        {
                            _state = ScatterState.SpawnWildPuzzle;
                            _isSpawnWildPuzzle = false;
                        }
                    }
                    break;
                }
            case ScatterState.Matching:
                {
                    break;
                }
            case ScatterState.End:
                {
                    UIGameplay.Instance.NormalMode();
                    SlotMachine.isFreeSpinMode = false;
                    SlotMachine.Instance.slotData.isScatterMode = false;
                    Puzzle.isEnableClick = true;
                    _isSpawnWildPuzzle = false;
                    _state = ScatterState.None;
                    UIFreeSpinTotalPopup.Instance.Show();
                    //topFX.SetActive(false);
                    break;
                }
        }
    }

    public void StartScatterMode()
    {
        SlotMachine.isFreeSpinMode = true;
        _current = 0;
        _state = ScatterState.Start;

        UpdateScatterCount(true);

        //OnEndterAutoSpin?.Invoke(count);
    }
    public void UpdateScatterCount(bool playEffect = false)
    {
        int scatterCount = 0;
        int scatterMultiply = 0;
        if (SlotMachine.Instance.slotData.isScatterMode)
        {
            if (SlotMachine.Instance.slotData.wildSpawnIndex < 0)
                scatterCount = SlotMachine.Instance.slotData.scatterCount;
            else
                scatterCount = SlotMachine.Instance.slotData.scatterCount + 1;

            scatterMultiply = SlotMachine.Instance.slotData.scatterMultiplier == 0 ? 2 : SlotMachine.Instance.slotData.scatterMultiplier;

            lastScatterCount = scatterCount;
        }
        else
        {
            scatterCount = 2;
            scatterMultiply = 2;
            lastScatterCount = 2;
        }

        UIGameplay.Instance.UpdateScateMode(lastScatterCount, scatterMultiply);
    }
    public void AddScatterCount(int num)
    {
        lastScatterCount += num;
        UIGameplay.Instance.UpdateScateMode(lastScatterCount, SlotMachine.Instance.slotData.scatterMultiplier);
        UIGameplay.Instance.JellyHeartText();
    }
    public void OnClickSpin()
    {
        if (SlotMachine.isSpinning)
        {
            if (SlotMachine.Instance.Busy()) return;

            SlotMachine.Instance.Spin();
        }
        else
        {
            if (_state != ScatterState.ClickForSpin) return;
            if (UIKindOfMeowPopup.Instance.Appear()) return;
            if (UIBigWinPopup.Instance.Appear()) return;
            if (UIRoundRewardPopup.Instance.Appear()) return;

            AutoSpin();
        }
    }
    public void AutoSpin()
    {
        if (!SlotMachine.isFreeSpinMode) return;
        _state = ScatterState.Spinning;
        SlotMachine.Instance.Spin();
        UIGameplay.Instance.AnimateFreespinSpinButton();

    }
    public void StopAutoSpin()
    {
        _isStart = false;
        _count = 0;
        _current = 0;
        SlotMachine.isAutoMode = false;
    }
    public void CreateCollectableHeart()
    {
        foreach (Puzzle p in SlotMachine.Instance.puzzles)
        {
            if (p.dataID == (int)SlotMachine.SlotMachineID.Puzzle_Collectable)
                p.CreateCollectableHeart();
        }
    }
    float CalculateThunderScale(Vector2 pos)
    {
        Vector2 _d1 = new Vector2(4.26f,6.48f);
        Vector2 _d2 = new Vector2(-2.84f, 3.85f);
        float _dst = (_d2 - _d1).magnitude;
        float _sc = 3.1f;

        float _dst2 = (pos - _d1).magnitude;

        return _dst2 * _sc / _dst;
    }
}
