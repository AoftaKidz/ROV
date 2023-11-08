using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class UITitleScene : MonoBehaviour
{
    public GameObject slotmachine;
    public GameObject uiGameplay;
    public GameObject uiTitle;
    public GameObject btnStart;
    public GameObject btnStartActive;

    public GameObject loading;
    public Image imgLoadingGauge;
    public TextMeshProUGUI txtProgress;
    bool isStart = false;

    // Start is called before the first frame update
    void Start()
    {
        SoundManager.Instance.PlayBGM("BGM Title");
        //LoadGameplayScene();
        //btnStart.SetActive(false);
        //btnStartActive.SetActive(false);

        {
            //Check token from url
            ServiceManager.Instance.GetURLParameter();
            if (ServiceManager.Instance.TOKEN == "")
            {
                string a = ServiceManager.Instance.url_params.ContainsKey("u") ? ServiceManager.Instance.url_params["u"]:"";
                string b = ServiceManager.Instance.url_params.ContainsKey("p") ? ServiceManager.Instance.url_params["p"] : "";
                if (ServiceManager.BYPASS)
                    StartCoroutine(ServiceManager.Instance.GetAuthenToken(a,b,GetAuthenTokenSuccess, GetAuthenTokenFail));
                else
                    UIAlertMessage.Instance.Show("Unauthorized.", false);
            }
            else
            {
                StartCoroutine(ServiceManager.Instance.GetAccount(ServiceManager.Instance.TOKEN, GetAccountSuccess, GetAccountFail));
            }
        }
    }
    void GetAuthenTokenSuccess(string result)
    {
        var t = result.Split("=");
        Debug.Log(result);

        StartCoroutine(ServiceManager.Instance.GetAccount(t[1], GetAccountSuccess, GetAccountFail));

    }
    void GetAuthenTokenFail(string result)
    {
        Debug.Log(result);
        ErrorModel error = JsonUtility.FromJson<ErrorModel>(result);
        UIAlertMessage.Instance.Show(error.message);

    }
    void GetAccountSuccess(string result)
    {
        var data = AccountModel.FromJSON(result);
        Debug.Log(result);
        UserProfile.Instance.token = data.data.access_token;
        UserProfile.Instance.wallet = data.data.balance;
        UserProfile.Instance.username = data.data.username;
        UserProfile.Instance.CallUpdateUserProfile();
        StartCoroutine(ServiceManager.Instance.GetSetting(UserProfile.Instance.token, Settinguccess, SettingFail));

    }
    void GetAccountFail(string result)
    {
        if (result == "")
        {
            UIAlertMessage.Instance.Show("Unauthorized.", false);
        }
        else
        {
            Debug.Log(result);
            try
            {
                ErrorModel error = JsonUtility.FromJson<ErrorModel>(result);
                if (error != null)
                    UIAlertMessage.Instance.Show(error.message, false);
                else
                    UIAlertMessage.Instance.Show("Unauthorized.", false);
            }
            catch
            {
                UIAlertMessage.Instance.Show("Unauthorized.", false);

            }

        }
    }
    void Settinguccess(string result)
    {
        var data = SettingModel.FromJSON(result);
        UserProfile.Instance.language = data.language;
        UserProfile.Instance.isOnSpeaker = data.bgm;
        UserProfile.Instance.isOnEffect = data.effect;
        UserProfile.Instance.wallet = data.balance;
        UserProfile.Instance.settingID = data._id;
        if(!UserProfile.Instance.isOnSpeaker)
            SoundManager.Instance.MuteBGM();
        else
            SoundManager.Instance.UnmuteBGM();
        Debug.Log(result);

    }
    void SettingFail(string result)
    {
        Debug.Log(result);
        ErrorModel error = JsonUtility.FromJson<ErrorModel>(result);
        if (error != null)
            UIAlertMessage.Instance.Show(error.message, false);
        else
            UIAlertMessage.Instance.Show("Unauthorized.", false);
    }
    private void OnEnable()
    {
        FadeManager.OnFadeOutComplete += FadeOutComplete;
    }
    private void OnDisable()
    {
        FadeManager.OnFadeOutComplete -= FadeOutComplete;
    }
    void FadeOutComplete()
    {
        //isStart = true;
        //SceneManager.LoadScene("GameplayScene", LoadSceneMode.Single);
        uiGameplay.gameObject.SetActive(true);
        slotmachine.gameObject.SetActive(true);
        uiTitle.gameObject.SetActive(false);
        FadeManager.Instance.StartFadeIn();
        SoundManager.Instance.PlayBGM("BGM Gameplay");

    }
    public void StartGame()
    {
        SlotMachine.isAutoMode = false;
        SlotMachine.isFreeSpinMode = false;
        SlotMachine.isSpinning = false;
        //btnStart.GetComponent<Image>().sprite = Resources.Load<Sprite>("Title/Btn_Start_Blank_Active");
        SoundManager.Instance.PlaySFX("Start_Btn");
        FadeManager.Instance.StartFadeOut();

    }
    void LoadGameplayScene()
    {
        //StartCoroutine(LoadSceneAsync(SceneIndex.GameplayScene.ToString()));
    }
    IEnumerator LoadSceneAsync(string sceneName)
    {
        AsyncOperation oper = SceneManager.LoadSceneAsync(sceneName);
        oper.allowSceneActivation = false;
        while (!oper.isDone)
        {
            float progress = Mathf.Clamp01(oper.progress / 0.9f);
            imgLoadingGauge.transform.localScale = new Vector2(progress,1);
            txtProgress.text = string.Format("{0}%",(progress * 100.0f) );
            if (progress >= 1.0f)
            {
                yield return new WaitForSeconds(0.6f);
                btnStart.SetActive(true);
                this.loading.SetActive(false);
                if (isStart)
                    oper.allowSceneActivation = true;
            }

            yield return null;
        }
    }
}
