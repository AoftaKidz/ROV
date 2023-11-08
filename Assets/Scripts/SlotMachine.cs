using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using Newtonsoft.Json;
using Unity.VisualScripting;
//using System.Runtime.InteropServices;

[Serializable]
public class MatchData
{
    public List<int> datas = new List<int>();
    public MatchData(int[] datas)
    {
        if(datas != null)
        this.datas.AddRange(datas);
    }
}
[Serializable]
public class SlotMachineData
{
    public int[] datas = new int[15];
    public List<MatchData> matches = new List<MatchData>();
    public List<int> lines = new List<int>();//Type 1,2,3,..,15 
    public List<float> linesReward = new List<float>();
    public bool isScatterMode = false;
    public int scatterCount = 0;
    public int scatterMultiply = 2;
    public float reward = 0;
    public float totalReward = 0;
    public bool isFiveOfKind = false;
    public float winRatio = 0;
    public int wildSpawnIndex = -1;
}
public class SlotMachine : MonoBehaviour
{
    static public event Action OnSlotColumnPreSpin;
    static public event Action OnSlotColumnSpin;
    static public event Action OnSlotColumnStopSpin;
    static public event Action<int> OnActionGetPuzzle;
    static public event Action OnCreateSlotMachine;

    public List<Puzzle> puzzles;
    public List<SlotColumn> slotColumns;
    static public Puzzle activePuzzle = null;
    static public WildTall activeWildTall = null;
    static public float turbo = 1;

    static public bool isSpinning = false;
    static public bool isTurboMode = false;
    static public bool isAutoMode = false;
    static public bool isFreeSpinMode = false;

    public BetModel slotData = null;
    bool _isInitSlotMachine = true;
    string initSlotResult = "";
    bool isCooldown = true;
    float _cooldownTime = 0;
    public enum SlotMachineID
    {
                Puzzle_1 = 0,
                Puzzle_2,
                Puzzle_3,
                Puzzle_4,
                Puzzle_5,
                Puzzle_A,//A
                Puzzle_K,//K
                Puzzle_Q,//Q
                Puzzle_J,//J
                Puzzle_10,//10
                Puzzle_9,//9
                Puzzle_Wild,
                Puzzle_Scatter,
                Puzzle_Collectable
    }

    public static SlotMachine Instance { get; private set; }

    //[DllImport("__Internal")]
    //private static extern void closewindow();
    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.

        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
        //

    }
    private void Start()
    {
        RandomTestData();
    }
    private void OnEnable()
    {
        //Subscribe event
        //UIEventManager.OnSlotMachineSpin += Spin;
        UIGameplay.OnUIGameplaySpinAction += Spin;
    }

    private void OnDisable()
    {
        //Unsubscribe event
        //UIEventManager.OnSlotMachineSpin -= Spin;
        UIGameplay.OnUIGameplaySpinAction -= Spin;
    }
    public void Spin()
    {
        try
        {
            GameObject[] wilds = GameObject.FindGameObjectsWithTag("WildTall");
            foreach (GameObject w in wilds)
            {
                if (w.GetComponent<WildTall>().Busy()) return;
            }
        }
        catch 
        {
            //Debug.Log("Not found WildTall.");
        }

        PuzzleInfo.Instance.Hide();

        if (isSpinning)
        {
            OnSlotColumnStopSpin?.Invoke();
        }
        else
        {
            RandomTestData();
        }

        //UIGameplay.Instance.ShowSexyGirlSpin();
    }
    public string GetSpineAnimationIdle(int index)
    {
        switch (index)
        {
             /*Puzzle_1 = 0,
                Puzzle_2,
                Puzzle_3,
                Puzzle_4,
                Puzzle_5,
                Puzzle_A,//A
                Puzzle_K,//K
                Puzzle_Q,//Q
                Puzzle_J,//J
                Puzzle_10,//10
                Puzzle_9,//9
                Puzzle_Wild,
                Puzzle_Scatter,
                Puzzle_Collectable*/

            case (int)SlotMachineID.Puzzle_A: return "Item_A";
            case (int)SlotMachineID.Puzzle_K: return "Item_K";
            case (int)SlotMachineID.Puzzle_Q: return "Item_Q";
            case (int)SlotMachineID.Puzzle_J: return "Item_J";
            case (int)SlotMachineID.Puzzle_10: return "Item_10";
            case (int)SlotMachineID.Puzzle_9: return "Item_9";
            case (int)SlotMachineID.Puzzle_1: return "Item_1";
            case (int)SlotMachineID.Puzzle_2: return "Item_2";
            case (int)SlotMachineID.Puzzle_3: return "Item_3";
            case (int)SlotMachineID.Puzzle_4: return "Item_4";
            case (int)SlotMachineID.Puzzle_5: return "Item_5";
            case (int)SlotMachineID.Puzzle_Scatter: return "Scatter";
            case (int)SlotMachineID.Puzzle_Wild: return "Wild";
            case (int)SlotMachineID.Puzzle_Collectable: return "Collectable";
        }
        return "";
    }

    public string GetSpineAnimationMatch(int index)
    {
        switch (index)
        {
            case (int)SlotMachineID.Puzzle_A: return "Item_A_Match";
            case (int)SlotMachineID.Puzzle_K: return "Item_K_Match";
            case (int)SlotMachineID.Puzzle_Q: return "Item_Q_Match";
            case (int)SlotMachineID.Puzzle_J: return "Item_J_Match";
            case (int)SlotMachineID.Puzzle_10: return "Item_10_Match";
            case (int)SlotMachineID.Puzzle_9: return "Item_9_Match";
            case (int)SlotMachineID.Puzzle_1: return "Item_1_Match";
            case (int)SlotMachineID.Puzzle_2: return "Item_2_Match";
            case (int)SlotMachineID.Puzzle_3: return "Item_3_Match";
            case (int)SlotMachineID.Puzzle_4: return "Item_4_Match";
            case (int)SlotMachineID.Puzzle_5: return "Item_5_Match";
            case (int)SlotMachineID.Puzzle_Scatter: return "Scatter_Match";
            case (int)SlotMachineID.Puzzle_Wild: return "Wild_Match";
            case (int)SlotMachineID.Puzzle_Collectable: return "Collectable_Match";

        }
        return "";
    }
    public static string GetPuzzleName(int index)
    {
        switch (index)
        {
            case (int)SlotMachineID.Puzzle_A: return "Item_A";
            case (int)SlotMachineID.Puzzle_K: return "Item_K";
            case (int)SlotMachineID.Puzzle_Q: return "Item_Q";
            case (int)SlotMachineID.Puzzle_J: return "Item_J";
            case (int)SlotMachineID.Puzzle_10: return "Item_10";
            case (int)SlotMachineID.Puzzle_9: return "Item_9";
            case (int)SlotMachineID.Puzzle_1: return "Item_1";
            case (int)SlotMachineID.Puzzle_2: return "Item_2";
            case (int)SlotMachineID.Puzzle_3: return "Item_3";
            case (int)SlotMachineID.Puzzle_4: return "Item_4";
            case (int)SlotMachineID.Puzzle_5: return "Item_5";
            case (int)SlotMachineID.Puzzle_Scatter: return "Scatter";
            case (int)SlotMachineID.Puzzle_Wild: return "Wild";
            case (int)SlotMachineID.Puzzle_Collectable: return "Collectable";
        }
        return "";
    }
    void API_Spin()
    {
        //UILoading.Instance.Show();
        if (_isInitSlotMachine)
        {
            StartCoroutine(ServiceManager.Instance.Bet(UserProfile.Instance.betTotal,UserProfile.Instance.token,SpinSuccess, SpinFailed));
        }
        else
        {
            return;

            isSpinning = true;
            OnSlotColumnPreSpin?.Invoke();
            //StartCoroutine(ServiceManager.Instance.Spin(SpinSuccess, SpinFailed));
            if (initSlotResult != "")
                SpinSuccess(initSlotResult);
            else
                StartCoroutine(ServiceManager.Instance.Bet(UserProfile.Instance.betTotal, UserProfile.Instance.token, SpinSuccess, SpinFailed));
        }
    }
    static bool mock_scatter_mode = false;
    static int mock_coming_free_spin = 1;
    static int mock_scatter_count = 0;

    void MockData(ref BetModel data)
    {
        return;
        var r = new System.Random();
        data.ballColor = r.Next(3);
        data.winRatio = 0;
        data.reward = 0;
        data.isFiveOfKind = false;
        data.isScatterMode = false;
        data.comingFreeSpinCount = 0;
        //data.data = new List<int>() { 0,1,2,3,2,5,6,7,2,2,10,11,0,2,4};
        return;
        if (mock_scatter_mode == false && mock_coming_free_spin > 0)
        {
            data.data[0] = 12;
            data.data[4] = 12;
            data.data[8] = 12;
            data.data[11] = 12;
            data.comingFreeSpinCount = 2;
            data.isScatterMode = mock_scatter_mode;
            data.scatterCount = 2;
            mock_scatter_mode = true;
            mock_scatter_count = 2;
            data.scatterMultiplier = 2;
        }
        else
        {
            data.comingFreeSpinCount = 0;
            data.isScatterMode = mock_scatter_mode;
            data.scatterCount = mock_scatter_count;
            data.scatterMultiplier = 2;

            mock_scatter_count--;
            if(mock_scatter_count < 0)
            {
                data.wildEnded = true;
                mock_scatter_mode = false;
                mock_coming_free_spin = 1;
                mock_scatter_count = 0;
            }
            data.data[5] = (int)SlotMachine.SlotMachineID.Puzzle_Collectable;

        }

        //data.matches.Add(new List<int>() { 0,4,9,111});
    }
    void SpinSuccess(string result)
    {
        //UILoading.Instance.Hide();
        //Debug.Log(result);
        //var mm = JsonConvert.DeserializeObject<BetModel>(result);
        UIBetPopup.lastBetValue = UserProfile.Instance.betTotal;
        slotData = JsonConvert.DeserializeObject<BetModel>(result);//JsonUtility.FromJson<BetModel>(result);
                                                                   //Debug.Log("scatter multipier : " + slotData.scatterMultiplier);
        if (slotData.wildCleared)
            ClearWild();

        if (slotData.data.Count == 0)
        {
            API_Spin();
            return;
        }

        if (!_isInitSlotMachine)
        {
            MockData(ref slotData);

            //GachaMachine.Instance.GachaBallMove();
            UIGameplay.Instance.AnimateButtonSpin();
            //UIGameplay.Instance.CoinInsert();
            SoundManager.Instance.PlaySFX("SlotSpin", true);
            SoundManager.Instance.PlaySFX("Spin_Button");

            //Update wallet 
            if (!slotData.isScatterMode)
            {
                double _b = UserProfile.Instance.wallet - UserProfile.Instance.betTotal;
                UserProfile.Instance.wallet = _b;
                UserProfile.Instance.CallUpdateUserProfile();
            }

            initSlotResult = "";
            /*            if (isFreeSpinMode)
                            UIGameplay.Instance.UpdateScateMode(slotData.scatterCount, slotData.scatterMultiply);*/
 /*           slotData.isScatterMode = false;
            slotData.comingFreeSpinCount = 5;
            slotData.wildSpawnIndex = UnityEngine.Random.Range(0, 14);*/
            if (slotData.isScatterMode == false && slotData.comingFreeSpinCount > 0)
            {
                //Start Scatter mode
                //Show UI Start Scatter 
                //UIFreeSpinPopup.Instance.Show(slotData.scatterCount);
                //slotData.isStartScatter = true;
                OnSlotColumnSpin?.Invoke();

            }
            else if(slotData.isScatterMode)
            {
                //Continue scatter mode

                OnSlotColumnSpin?.Invoke();
                //UIGameplay.Instance.UpdateScateMode(SlotMachine.Instance.slotData.scatterCount, SlotMachine.Instance.slotData.scatterMultiplier);

            }
            else
            {
                //Normal mode
                OnSlotColumnSpin?.Invoke();

            }
        }
        else
        {
            MockData(ref slotData);

            //OnCreateSlotMachine?.Invoke();
            _isInitSlotMachine = false;
            initSlotResult = result;
/*            slotData.isScatterMode = true;
            slotData.comingFreeSpinCount = 5;
            slotData.wildSpawnIndex = UnityEngine.Random.Range(0, 14);*/
          /*  if (slotData.isScatterMode)
            {
                int scatterCount = 0;
                //Continue scatter mode
                if (slotData.wildSpawnIndex < 0)
                    scatterCount = slotData.scatterCount;
                else
                    scatterCount = slotData.scatterCount + 1;

                    int num = 0;
                    foreach (int d in slotData.data)
                    {
                        if (d == (int)SlotMachineID.Puzzle_Collectable)
                            num++;
                    }

                scatterCount = scatterCount + num;
                
                UIFreeSpinPopup.Instance.Show(scatterCount);
            }*/
        }
    }
    void SpinFailed(string result)
    {
        UILoading.Instance.Hide();
        //API_Spin();
        //Debug.Log("Spin Failed : " + result);
        ErrorModel error = JsonUtility.FromJson<ErrorModel>(result);
        if(error.statusCode == 401)
        {
            UIAlertMessage.Instance.Show(error.message);
        }
        else
        {
            UIAlertMessage.Instance.Show(error.message);
        }
    }
    void GotoTitleScene()
    {
        //SceneManager.LoadScene("TitleScene");
        Application.Quit();
        //closewindow();
    }
    void RandomTestData()
    {
        API_Spin();
    }
    public void SetActivePuzzle(int index)
    {
        OnActionGetPuzzle?.Invoke(index);
    }
    public bool IsComingScatterMode()
    {
        int c = 0;
        foreach(int i in slotData.data)
        {
            if(i == (int)SlotMachineID.Puzzle_Scatter)
            {
                c++;
            }
        }
        if (c >= 3) return true;

        return false;
    }
    public bool CheckWildActive()
    {
        if (slotData == null) return false;
        if (slotData.data == null) return false;

        foreach (var d in slotData.data)
        {
            if (d == (int)SlotMachine.SlotMachineID.Puzzle_Wild)
                return true;
        }
        return false;
    }
    public void ClearWild()
    {
        GameObject[] wilds = GameObject.FindGameObjectsWithTag("WildTall");
        foreach (GameObject w in wilds)
        {
            Destroy(w);
        }
    }
    public bool Busy()
    {
        if (isCooldown) return true;

        if (isSpinning)
        {
            if (slotColumns[0].GetSpinTime() < 0.5f)
                return true;
            else
                return false;
        }
        else
        {
            return false;
        }
    }
    public bool WildSpawning()
    {
        GameObject[] wilds = GameObject.FindGameObjectsWithTag("WildTall");
        foreach (GameObject w in wilds)
        {
            WildTall wt = w.GetComponent<WildTall>();
            if (wt.Busy())
                return true;
        }
        return false;
    }
    public void CoolDown()
    {
        isCooldown = true;
        _cooldownTime = 0;
    }
    private void Update()
    {
        if (isCooldown)
        {
            _cooldownTime += Time.deltaTime;
            if (_cooldownTime > 0.5f)
            {
                _cooldownTime = 0;
                isCooldown = false;
            }
        }
    }
}
