using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;
using Unity.VisualScripting;
using System;
#region Class_HJH
[System.Serializable]
public class UserData_HJH
{

    public int porfileImg = 0;
    public string userName = "nullName"; //유저 이름
    public int stageProgress = 0; // 스테이지 진행도
    public int winCount = 0; //승리 횟수
    public int loseCount = 0; //패배 횟수
    public float stageClearTime = 0; //스테이지 클리어 하는데 걸린 시간의 총합
    public int solveCount = 0; //푼 수학 문제 수
    public int tryCount = 0; //문제 풀이 시도 횟수
    public int mathCoinAmount = 0; //얻은 전체 메스 코인량
    public int userLevel = 0;
    public int langaugeSet = 1;
    public bool vibration = true;
    [Serializable]
    public class stageStars
    {
        public bool[] stageStar = new bool[3];
        public stageStars()
        {
            stageStar = new bool[3];
        }
    }
    public stageStars[] stageStar = new stageStars[13]; //스테이지 당 별
    public UserData_HJH() 
    {
        porfileImg = 0;
        userName = string.Empty;
        stageProgress = 0;
        winCount = 0;
        loseCount = 0;
        stageClearTime = 0;
        solveCount = 0;
        mathCoinAmount = 0;
        tryCount = 0;
        userLevel = 0;
        langaugeSet = 1;
        vibration = true;
        stageStar = new stageStars[13];
    }
}
[System.Serializable]
public class RewardData_HJH
{
    public int[] startCardAmount;
    public int firstClearCard;
    public bool random; // 랜덤으로 주는지 하나는 확정인지
    public int confirmedUnitIdx; //확정인 유닛 인덱스

}

[System.Serializable]
public class StarSystem_HJH
{
    public int[] whatIsCondition = new int[3]; //어떤 조건으로 할지
    public bool gameClear = false; //게임이 클리어 했을 때
    public int gameClearTime = 0; // 시간 제한
    public int mathCoinAmount = 0; // 사용한 돈 제한
}
#endregion
public class GameManager : MonoBehaviour
{

    public List<StarSystem_HJH> starCondition;
    public List<RewardData_HJH> rewardData;
    public static GameManager instance = null;
    public int stage = 0;

    public Sprite[] starImage;

    public UserData_HJH userData;
    
    [SerializeField]
    float bgmVolume;
    [SerializeField]
    float soundEffectVolume;

    public GameState gameState;
    public enum GameState
    {
        GamePlay,
        GameStop,
    }

    public float BgmVolume
    {
        get
        {
            return bgmVolume;
        }
        set
        { 
            bgmVolume = value; 
            if(bgmVolume < 0)
            {
                bgmVolume = 0;
            }
            else if(bgmVolume > 1)
            {
                bgmVolume = 1;
            }
            bgm.volume = bgmVolume;
        }
    }

    public float SoundEffectVolume
    {
        get
        {
            return soundEffectVolume;
        }
        set
        {
            soundEffectVolume = value;
            if (soundEffectVolume < 0)
            {
                soundEffectVolume = 0;
            }
            else if (soundEffectVolume > 1)
            {
                soundEffectVolume = 1;
            }
            for(int i = 0; i<soundEffects.Count; i++)
            {
                soundEffects[i].volume = soundEffectVolume;
            }
        }
    }

    public AudioSource bgm;
    public AudioClip[] bgmSources;

    public List<AudioSource> soundEffects;
    public Sprite[] unitImage;


    #region properties_MJW

    public TextAsset playerUnitDatabase;
    public TextAsset enemyUnitDatabase;
    public UnitTable playerUnitTable;
    public UnitTable enemyUnitTable;
    public UserInfo_MJW userInfo;
    public Deck_MJW currentDeck;
    public int? currentStage = null;

    public UnitPrefabManager_MJW unitPrefabManager;
    private string filePath;

    #endregion


    #region methods_MJW

    public void ParseUnitTable(TextAsset database, UnitTable table){
        string[] line = database.text.Substring(0, database.text.Length - 1).Split('\n');
        for(int i = 0; i < line.Length; ++i){
            string[] row = line[i].Split('\t');

            UnitData data = table.unitData[i];

            data.unitInfos.id = int.Parse(row[0]);
            data.unitInfos.e_name = row[1];
            data.unitInfos.k_name = row[2];
            data.unitInfos.e_information = row[3];
            data.unitInfos.k_information = row[4];

            data.unitStats.maxHP = float.Parse(row[5]);
            data.unitStats.attackDamage = float.Parse(row[6]);
            data.unitStats.attackRange = float.Parse(row[7]);
            data.unitStats.attackSpeed = float.Parse(row[8]);
            data.unitStats.defensive = float.Parse(row[9]);
            data.unitStats.moveSpeed = float.Parse(row[10]);
            data.unitStats.cost = int.Parse(row[11]);
            data.unitStats.cooldown = float.Parse(row[12]);

            data.upgradeStats.uMaxHP = float.Parse(row[13]);
            data.upgradeStats.uAttackDamage = float.Parse(row[14]);
            data.upgradeStats.uAttackRange = float.Parse(row[15]);
            data.upgradeStats.uAttackSpeed = float.Parse(row[16]);
            data.upgradeStats.uDefensive = float.Parse(row[17]);
            data.upgradeStats.uMoveSpeed = float.Parse(row[18]);
            data.upgradeStats.upgradeCost = int.Parse(row[19]);

            table.unitData[i] = data;
        }
    }

    public void InitData(){
        userInfo = new UserInfo_MJW();
        currentDeck = userInfo.GetSelectedDeck();
        SaveData();
        LoadData();
    }

    public void SaveData(){
        string jdata = JsonUtility.ToJson(userInfo);

        File.WriteAllText(filePath + "/UserData.txt", jdata);
    }

    public void LoadData(){
        if(!File.Exists(filePath + "/UserData.txt")){InitData(); return;}

        string jdata = File.ReadAllText(filePath + "/UserData.txt");
        userInfo = JsonUtility.FromJson<UserInfo_MJW>(jdata);
        currentDeck = userInfo.GetSelectedDeck();
    }

    #endregion

    private void Awake()
    {
        
        DontDestroyOnLoad(gameObject);
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        bgmVolume = bgm.volume;

        SceneManager.sceneLoaded += OnSceneLoaded;

        filePath = Application.persistentDataPath;
        Debug.Log("" + filePath);

        // 전체 유닛 리스트 불러오기
        ParseUnitTable(playerUnitDatabase, playerUnitTable);
        ParseUnitTable(enemyUnitDatabase, enemyUnitTable);
        unitPrefabManager.LinkPrefabs(playerUnitTable, enemyUnitTable);

        LoadData();
    }
    
    // Start is called before the first frame update
    void Start()
    {
        //PlayerPrefs.DeleteAll();
        string data = PlayerPrefs.GetString("UserData");
        if (data.Length >1)
        {
            userData = JsonUtility.FromJson<UserData_HJH>(data);
        }
        else
        {
            userData = new UserData_HJH();
            if (userData.stageStar[0] == null)
            {
                for (int i = 0; i < userData.stageStar.Length; i++)
                {
                    userData.stageStar[i] = new UserData_HJH.stageStars();
                }
            }
        }
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[userData.langaugeSet];
        //Debug.Log(userData.porfileImg);
        //Debug.Log(LocalizationSettings.SelectedLocale.ToString());
    }

    // Update is called once per frame
    void Update()
    {
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        gameState = GameState.GamePlay;
        if(scene.name == "StageScene")
        {
            bgm.clip = bgmSources[0];
            bgm.Play();
        }
        else if(scene.name == "GameScene")
        {
            bgm.clip = bgmSources[1];
            bgm.Play();
        }
        FindAudioSource();

    }
    private void FindAudioSource()
    {
        List<AudioSource> audioSources = new List<AudioSource>();
        GameObject[] all = FindObjectsOfType<GameObject>();
        AudioSource myAudio = GetComponent<AudioSource>();
        foreach (GameObject obj in all)
        {
            AudioSource audio;
            if (obj.TryGetComponent<AudioSource>(out audio))
            {
                if (audio != myAudio)
                {
                    audioSources.Add(audio);
                    audio.volume = soundEffectVolume;
                }
            }
        }
        soundEffects = audioSources;
    }

    public void SaveUserData()
    {
        for(int i =0; i< LocalizationSettings.AvailableLocales.Locales.Count; i++)
        {
            if(LocalizationSettings.SelectedLocale == LocalizationSettings.AvailableLocales.Locales[i])
            {
                userData.langaugeSet = i;
            }
        }
        string data = JsonUtility.ToJson(userData,true);
        //Debug.Log(data);
        PlayerPrefs.SetString("UserData", data);
    }

    public void Vibrate()
    {
        if (userData.vibration)
        {
            Handheld.Vibrate();
        }
    }
}
