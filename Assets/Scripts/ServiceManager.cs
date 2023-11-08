using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.Networking;

public class ServiceManager : MonoBehaviour
{
    enum Environment
    {
        PROD = 0,
        STAGING
    }
    public string DOMAIN_PROD = "https://9zhs3ikjkx.ap-southeast-1.awsapprunner.com";
    public string DOMAIN_STAGING = "https://9zhs3ikjkx.ap-southeast-1.awsapprunner.com";
    public string TOKEN = "";
    public string WALLET = "";
    string DOMAIN = "";
    int _ENV = 1;//STAGING
    public bool IS_DEPLOY_PROD = false;

    public static bool BYPASS = true;
    SlotMachineData slotData = new SlotMachineData();
    public Dictionary<string, string> url_params = new Dictionary<string, string>();

    static public ServiceManager Instance = null;
    public delegate void ServiceCallback(string json); // declare delegate type
    // Start is called before the first frame update
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
        }

        if (IS_DEPLOY_PROD)
        {
            DOMAIN = DOMAIN_PROD;
            BYPASS = false;
        }
        else
        {
            DOMAIN = DOMAIN_STAGING;
            BYPASS = true;
        }
    }
    void Start()
    {



    }
    public void GetURLParameter()
    {
        TOKEN = "";
        string url = Application.absoluteURL;
        var arr = url.Split("?");
        if(arr.Length > 1)
        {
            var list = arr[1].Split("&");
            if(list.Length > 0)
            {
                //Find Token
                foreach (var param in list)
                {
                    if (param.Contains("t"))
                    {
                        //Found Token
                        var t = param.Split("=");
                        if(t.Length > 1)
                            TOKEN = t[1];
                    }

                    var tt = param.Split("=");
                    url_params.Add(tt[0], tt[1]);
                }
            }
        }
    }
    public IEnumerator GetAuthenToken(string user,string pass,ServiceCallback success, ServiceCallback fail)
    {
        string uri = DOMAIN + "/api/v1/account/sign"; //Service api function 
        WWWForm form = new WWWForm();
        //Add Parameter
        //form.AddField("walletAddress", WALLET);
        //POST
        if (user == "" || pass == "")
        {
            form.AddField("username", "devbumbo000004");
            form.AddField("password", "Aa239315");
            /*
            form.AddField("username", "devbumbo000005");
            form.AddField("password", "Aa935023");
            */
        }
        else
        {
            form.AddField("username", user);
            form.AddField("password", pass);
        }

        /*        form.AddField("gameID", "6");
                form.AddField("partnerID", "2");
                form.AddField("balance", "5000");*/
        form.AddField("displayName", "devbumbo000004");
        UnityWebRequest request = UnityWebRequest.Post(uri, form);

        //Header
        //request.SetRequestHeader("api-key", "03338E46B7D7C28F95E1B62F7AE26FD245D860B609A608F0DA8D15BFA8589CEB");
        request.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");
        request.SetRequestHeader("Access-Control-Allow-Origin", "*");

        yield return request.SendWebRequest();
        if (request.result != UnityWebRequest.Result.Success)
        {
            if (fail != null)
            {
                fail(request.downloadHandler.text);
            }
        }
        else
        {
            if (success != null)
            {
                success(request.downloadHandler.text);
            }
        }
    }
    public IEnumerator GetAccount(string token,ServiceCallback success, ServiceCallback fail)
    {
        string uri = DOMAIN + "/api/v1/account/" + token;
        UnityWebRequest request = UnityWebRequest.Get(uri);

        request.SetRequestHeader("Access-Control-Allow-Origin", "*");
        yield return request.SendWebRequest();
        if (request.result != UnityWebRequest.Result.Success)
        {
            if (fail != null)
            {
                fail(request.downloadHandler.text);
            }
        }
        else
        {
            if (success != null)
            {
                success(request.downloadHandler.text);
            }
        }
    }
    public IEnumerator GetHistory(string token, string startDate, string endDate, float page, float size, ServiceCallback success, ServiceCallback fail)
    {
        string query = "?start=" + startDate + "&end=" + endDate + "&page=" + page + "&size=" + size;
        string uri = DOMAIN + "/api/v1/games/history" + query;
        Debug.Log("API GET HISTORY" + uri);
        UnityWebRequest request = UnityWebRequest.Get(uri);

        request.SetRequestHeader("Authorization", "Bearer " + token);
        request.SetRequestHeader("Access-Control-Allow-Origin", "*");
        yield return request.SendWebRequest();
        if (request.result != UnityWebRequest.Result.Success)
        {
            if (fail != null)
            {
                fail(request.downloadHandler.text);
            }
        }
        else
        {
            if (success != null)
            {
                success(request.downloadHandler.text);
            }
        }


    }
    public IEnumerator Bet(float bet,string token,ServiceCallback success, ServiceCallback fail)
    {
        string uri = DOMAIN + "/api/v1/games/bet";
        WWWForm form = new WWWForm();
        form.AddField("v", ""+bet);

        UnityWebRequest request = UnityWebRequest.Post(uri, form);

        //Header
        request.SetRequestHeader("Authorization", "Bearer " + token);
        request.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");
        request.SetRequestHeader("Access-Control-Allow-Origin", "*");

        yield return request.SendWebRequest();
        if (request.result != UnityWebRequest.Result.Success)
        {
            if (fail != null)
            {
                fail(request.downloadHandler.text);
            }
        }
        else
        {
            if (success != null)
            {
                success(request.downloadHandler.text);
            }
        }

        
    }
    public IEnumerator GetSetting(string token, ServiceCallback success, ServiceCallback fail)
    {
        string uri = DOMAIN + "/api/v1/users/settings/";
        UnityWebRequest request = UnityWebRequest.Get(uri);

        request.SetRequestHeader("Authorization", "Bearer " + token);
        request.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");
        request.SetRequestHeader("Access-Control-Allow-Origin", "*");
        yield return request.SendWebRequest();
        if (request.result != UnityWebRequest.Result.Success)
        {
            if (fail != null)
            {
                fail(request.downloadHandler.text);
            }
        }
        else
        {
            if (success != null)
            {
                success(request.downloadHandler.text);
            }
        }
    }
    public IEnumerator Setting(string token, ServiceCallback success, ServiceCallback fail)
    {
        string uri = DOMAIN + "/api/v1/users/settings/" + UserProfile.Instance.settingID;
        
        WWWForm form = new WWWForm();
        form.AddField("language", "" + UserProfile.Instance.language);
        form.AddField("bgm", UserProfile.Instance.isOnEffect ? "true" : "false");
        form.AddField("effect", UserProfile.Instance.isOnEffect ? "true" : "false");

        UnityWebRequest request = UnityWebRequest.Post(uri, form);

        request.SetRequestHeader("Authorization", "Bearer " + token);
        request.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");
        request.SetRequestHeader("Access-Control-Allow-Origin", "*");
        yield return request.SendWebRequest();
        if (request.result != UnityWebRequest.Result.Success)
        {
            if (fail != null)
            {
                fail(request.downloadHandler.text);
            }
        }
        else
        {
            if (success != null)
            {
                success(request.downloadHandler.text);
            }
        }
    }
    public IEnumerator Spin(ServiceCallback success, ServiceCallback fail,bool isCreate=false)
    {
        yield return null;
        string json = MockupSpin(isCreate);
        if (success != null)
        {
            success(json);
        }
    }
    
    string MockupSpin(bool isCreate = false)
    {

        //Match
        slotData.matches.Clear();
        {
            int[] match = { 0, 4, 5, 10, 11, 14 };
            //slotData.matches.Add(new MatchData(match));
        }

        {
            int[] match = { 1, 3, 6, 10, 13 };
            //slotData.matches.Add(new MatchData(match));
        }

        {
            int[] match = { 2, 4, 7, 9, 12 };
            //slotData.matches.Add(new MatchData(match));
        }

        float totalReward = 0;
        //Line
        slotData.lines.Clear();
        slotData.linesReward.Clear();
        {
            {
                float reward = (float)UnityEngine.Random.Range(0, 30) + (float)UnityEngine.Random.Range(0, 100) / 100.0f;
                slotData.lines.Add(1);
                slotData.linesReward.Add(reward);
                totalReward += reward;
            }

            {
                float reward = (float)UnityEngine.Random.Range(0, 30) + (float)UnityEngine.Random.Range(0, 100) / 100.0f;
                slotData.lines.Add(2);
                slotData.linesReward.Add(reward);
                totalReward += reward;
            }
            {
                float reward = (float)UnityEngine.Random.Range(0, 30) + (float)UnityEngine.Random.Range(0, 100) / 100.0f;
                slotData.lines.Add(5);
                slotData.linesReward.Add(reward);
                totalReward += reward;
            }
        }

        //Reward
        slotData.reward = totalReward;//(float)UnityEngine.Random.Range(0, 100) + (float)UnityEngine.Random.Range(0, 100)/100.0f;

        if (UserProfile.Instance)
            slotData.winRatio = totalReward / UserProfile.Instance.betTotal;
        else
            slotData.winRatio = 0;
        slotData.isFiveOfKind = UnityEngine.Random.Range(0, 100) > 80 ? true : false;

        if (slotData.isScatterMode)
        {

            if(slotData.scatterCount == 2)//First round
            {
                WildMove();

                slotData.scatterCount--;
                for (int i = 0; i < 15; i++)
                {
                    if (slotData.datas[i] != (int)SlotMachine.SlotMachineID.Puzzle_Wild)
                        slotData.datas[i] = RandomItem(true);//(int)UnityEngine.Random.Range(0, 14);
                }

                //Wild Spawn
                //int rnd = Random.Range(0, 15);
                if(slotData.wildSpawnIndex >= 0)
                slotData.datas[slotData.wildSpawnIndex] = (int)SlotMachine.SlotMachineID.Puzzle_Wild;
                slotData.wildSpawnIndex = -1;
            }
            else//Next round
            {
                if (CheckWildAndCollectible())
                {
                    WildMove();

                    for (int i = 0; i < 15; i++)
                    {
                        if (slotData.datas[i] != (int)SlotMachine.SlotMachineID.Puzzle_Wild)
                            slotData.datas[i] = RandomItem(true);//(int)UnityEngine.Random.Range(0, 14);
                    }

                    if (slotData.scatterCount <= 0)
                    {
                        if (!CheckWildAndCollectible())
                        {
                            slotData.isScatterMode = false;
                        }
                    }
                    else
                    {
                        if (!CheckWildAndCollectible())
                        {
                            //slotData.wildSpawnIndex = Random.Range(0, 15);
                            RandomWildIndex();
                        }
                    }
                }
                else
                {
                    WildMove();

                    slotData.scatterCount--;
                    for (int i = 0; i < 15; i++)
                    {
                        if (slotData.datas[i] != (int)SlotMachine.SlotMachineID.Puzzle_Wild)
                            slotData.datas[i] = RandomItem(true);//(int)UnityEngine.Random.Range(0, 14);
                    }

                    //Wild Spawn
                    //int rnd = Random.Range(0, 15);
                    //slotData.datas[rnd] = (int)SlotMachine.SlotMachineID.Puzzle_Wild;
                    if (slotData.wildSpawnIndex >= 0)
                        slotData.datas[slotData.wildSpawnIndex] = (int)SlotMachine.SlotMachineID.Puzzle_Wild;
                    slotData.wildSpawnIndex = -1;
                }
            }
            
            slotData.totalReward += totalReward;
        }
        else
        {
            
            //For test Scatter
            if (isCreate)
            {
                for (int i = 0; i < 15; i++)
                {
                    slotData.datas[i] = CreateRandomItem(); //(int)UnityEngine.Random.Range(0, 13);
                }
                slotData.isScatterMode = false;//UnityEngine.Random.Range(0,100) > 50 ? true : false;
            }
            else
            {
                WildMove();

                for (int i = 0; i < 15; i++)
                {
                    if(slotData.datas[i] != (int)SlotMachine.SlotMachineID.Puzzle_Wild)
                        slotData.datas[i] = RandomItem(); //(int)UnityEngine.Random.Range(0, 13);
                }
                slotData.isScatterMode = Random.Range(0, 100) > 70 ? true : false;
                if (slotData.isScatterMode)
                {
                    //slotData.wildSpawnIndex = Random.Range(0, 15);
                    RandomWildIndex();
                }
                else
                {
                    slotData.wildSpawnIndex = -1;
                }
            }

            slotData.scatterCount = 2;
            slotData.totalReward = totalReward;
        }

        return JsonUtility.ToJson(slotData);
    }
    int CreateRandomItem()
    {
        int item = Random.Range(0, 11);
        return item;
    }
    int RandomItem(bool isCollectible = false)
    {
        float normalItem = 98;
        float r = Random.Range(0, 100);
        if(r <= normalItem)
        {
            int item = Random.Range(0, 11);
            return item;
        }
        else
        {
            if (isCollectible)
            {
                int item = Random.Range(11, 14); ;
                return item;
            }
            else
            {
                int item = Random.Range(11, 13); ;
                return item;
            }
         
        }
    }
    void WildMove()
    {
        for(int i = 0; i < 15; i++)
        {
            if(slotData.datas[i] == (int)SlotMachine.SlotMachineID.Puzzle_Wild)
            {
                int index = i - 3;
                if (index < 0)
                    slotData.datas[i] = 0;
                else
                {
                    slotData.datas[i] = 0;
                    slotData.datas[index] = (int)SlotMachine.SlotMachineID.Puzzle_Wild;
                }
            }
        }
    }
    bool CheckWildAndCollectible()
    {
        for(int i = 0; i < slotData.datas.Length; i++)
        {
            if (slotData.datas[i] == (int)SlotMachine.SlotMachineID.Puzzle_Wild || slotData.datas[i] == (int)SlotMachine.SlotMachineID.Puzzle_Collectable)
                return true;
        }

        return false;
    }
    void RandomWildIndex()
    {
        bool _isCheck = true;
        while (_isCheck)
        {
            int index = Random.Range(0, 15);
            int column = (int)Mathf.Floor(index / 3) + 1;
            bool _isSameColumn = false;
            for (int i = 0; i < slotData.datas.Length; i++)
            {
                if (slotData.datas[i] == (int)SlotMachine.SlotMachineID.Puzzle_Wild)
                {
                    int c = (int)Mathf.Floor(i / 3) + 1;
                    if(c == column)
                    {
                        _isSameColumn = true;
                        break;
                    }
                }
            }
            if(!_isSameColumn)
            {
                _isCheck = false;
                slotData.wildSpawnIndex = index;
            }
        }
    }
}
