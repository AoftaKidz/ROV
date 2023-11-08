using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization;

public class UISettingPopup : MonoBehaviour
{
    [SerializeField] GameObject group;
    [SerializeField] GameObject fade;
    [SerializeField] GameObject content;
    public static UISettingPopup Instance = null;
    [SerializeField] Button btnSpeaker;
    [SerializeField] Button btnEffect;
    [SerializeField] Button []btnLanguages;
    string[] languageName = { "th","en","jp","cn"};

    // Start is called before the first frame update
    void Start()
    {
        if (Instance == null)
            Instance = this;
        //LoadLocale2("th");
    }
    public void OnConfirm()
    {
        StartCoroutine(ServiceManager.Instance.Setting(UserProfile.Instance.token,SettingSuccess,SettingFail));
        UILoading.Instance.Show();
    }
    void SettingSuccess(string result)
    {
        Debug.Log(result);
        Hide();
        UILoading.Instance.Hide();
    }
    void SettingFail(string result)
    {
        Hide();
        UILoading.Instance.Hide();
        Debug.Log(result);
        ErrorModel error = JsonUtility.FromJson<ErrorModel>(result);
        UIAlertMessage.Instance.Show(error.message);
    }
    public void OnClose()
    {
        OnConfirm();
    }
    public void OnClickSpeaker()
    {
        UserProfile.Instance.isOnSpeaker = !UserProfile.Instance.isOnSpeaker;
        if (UserProfile.Instance.isOnSpeaker)
            btnSpeaker.GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/Popup/Music_ON");
        else
            btnSpeaker.GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/Popup/Music_OFF");
        if (UserProfile.Instance.isOnSpeaker)
            SoundManager.Instance.UnmuteBGM();
        else
            SoundManager.Instance.MuteBGM();
    }
    public void OnClickEffect()
    {
        UserProfile.Instance.isOnEffect = !UserProfile.Instance.isOnEffect;
        if (UserProfile.Instance.isOnEffect)
            btnEffect.GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/Popup/SFX_ON");
        else
            btnEffect.GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/Popup/SFX_OFF");
    }
    void UpdateDataFromUserProfile()
    {
        //Speaker
        if (UserProfile.Instance.isOnSpeaker)
            btnSpeaker.GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/Popup/Music_ON");
        else
            btnSpeaker.GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/Popup/Music_OFF");
        //Effect
        if (UserProfile.Instance.isOnEffect)
            btnEffect.GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/Popup/SFX_ON");
        else
            btnEffect.GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/Popup/SFX_OFF");

        SetLanguageByName(UserProfile.Instance.language);
    }
    public void OnSelectLanguage(int tag)
    {
        if (tag > 3)
            tag = 0;
        SetLanguageByName(languageName[tag]);
    }
    void SetLanguageByName(string lang)
    {
        string[] disableFileNames = { "Btn_Language_TH", "Btn_Language_EN", "Btn_Language_JP", "Btn_Language_CN" };
        string[] activeFileNames = { "Btn_Language_Active_TH", "Btn_Language_Active_EN", "Btn_Language_Active_JP", "Btn_Language_Active_CN" };

        //Disable all 
        for(int i = 0; i < 4;i++)
        {
            string fileName = "UI/Popup/" + "th" + "/Language/" + disableFileNames[i];
            btnLanguages[i].GetComponent<Image>().sprite = Resources.Load<Sprite>(fileName);
        }

        if(lang == "th")
        {
            string fileName = "UI/Popup/" + "th" + "/Language/" + activeFileNames[0];
            btnLanguages[0].GetComponent<Image>().sprite = Resources.Load<Sprite>(fileName);
            LoadLocale("th");
        }
        else if(lang == "en")
        {
            string fileName = "UI/Popup/" + "th" + "/Language/" + activeFileNames[1];
            btnLanguages[1].GetComponent<Image>().sprite = Resources.Load<Sprite>(fileName);
            LoadLocale("en");
        }
        else if (lang == "jp")
        {
            string fileName = "UI/Popup/" + "th" + "/Language/" + activeFileNames[2];
            btnLanguages[2].GetComponent<Image>().sprite = Resources.Load<Sprite>(fileName);
            LoadLocale("ja");
        }
        else if (lang == "cn")
        {
            string fileName = "UI/Popup/" + "th" + "/Language/" + activeFileNames[3];
            btnLanguages[3].GetComponent<Image>().sprite = Resources.Load<Sprite>(fileName);
            LoadLocale("zh-CN");
        }

        UserProfile.Instance.language = lang;
    }
    public void LoadLocale(string languageIdentifier)
    {
        LocalizationSettings settings = LocalizationSettings.Instance;
        LocaleIdentifier localeCode = new LocaleIdentifier(languageIdentifier);//can be "en" "de" "ja" etc.
        for (int i = 0; i < LocalizationSettings.AvailableLocales.Locales.Count; i++)
        {
            Locale aLocale = LocalizationSettings.AvailableLocales.Locales[i];
            LocaleIdentifier anIdentifier = aLocale.Identifier;
            if (anIdentifier == localeCode)
            {
                LocalizationSettings.SelectedLocale = aLocale;
            }
        }
    }
    public static void LoadLocale2(string languageIdentifier) 
    { 
        LocalizationSettings.SelectedLocale.Identifier = languageIdentifier; 
    }
    public void Show()
    {
        content.SetActive(true);
        Puzzle.isEnableClick = false;
        UpdateDataFromUserProfile();
        fade.SetActive(true);
        content.transform.DOLocalMoveY(1117, 0.5f).SetEase(Ease.OutQuint);
    }
    public void Hide()
    {
        SoundManager.Instance.PlaySFX("Close");
        Puzzle.isEnableClick = true;
        fade.SetActive(false);
        content.transform.DOLocalMoveY(-2778, 0.3f).SetEase(Ease.InQuint).OnComplete(() => {
            content.SetActive(false);
        });
    }
}
