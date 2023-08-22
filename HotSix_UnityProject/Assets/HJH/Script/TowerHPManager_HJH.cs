using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.XR;

public class TowerHPManager_HJH : MonoBehaviour
{
    public AudioSource towerHpSound;
    int playerSoundIdx = 1; //체력 까이는거 소리 용도로 쓰는거. 
    int enemySoundIdx = 1;
    public float towerHpUnit;
    public int towerUnitAmount; 

    [Header("눈금 만드는데 쓰는거")]
    public GameObject hpLine;
    public GameObject enemyHpLine;
    [Header("슬라이더들")]
    public Slider playerHPSlider;
    public Slider enemyHPSlider;

    public GameObject playerTower; //플레이어 타워 오브젝트
    public Sprite[] playerTowerSprite; // 플레이어 타워 스프라이트들
    public GameObject enemyTower; // 적 타워 오브젝트
    public Sprite[] enemyTowerSprite; // 적 타워 스프라이트들

    public TMP_Text upgradeMoneyText;
    public GameObject upgradeMoneyButton;
    public int[] upgradeMoneyList;


    public float startPlayerTowerHP; // 플레이터 타워 최초 체력
    public float startEnemyTowerHP; //적 타워 최초 체력

    public GameObject playerHpObject;
    public GameObject enemyHpObject;

    public float playerMaxHP; //플레이어 최대 체력
    public float enemyMaxHP;

    public float playerTowerHP; //플레이여 현재 체력
    public float enemyTowerHP;

    public MoneyManager_HJH moneyManager;

    public Menu_HJH menu;

    public int towerLevel = 0;
    public int enemyTowerLevel = 0;

    public MapManager_HJH mapManager;
    // Start is called before the first frame update
    void Start()
    {
        playerMaxHP = startEnemyTowerHP;
        enemyMaxHP = startEnemyTowerHP;
        playerTowerHP = startPlayerTowerHP;
        enemyTowerHP = startEnemyTowerHP;
        GameManager.instance.soundEffects.Add(towerHpSound);
    }

    // Update is called once per frame
    void Update()
    {
        if(towerLevel < 2)
        {
            upgradeMoneyText.text = "["+upgradeMoneyList[towerLevel].ToString() + "]";
        }
        else
        {
            upgradeMoneyText.text = "[MAX]";
        }
        playerHPSlider.value = playerTowerHP / playerMaxHP;
        enemyHPSlider.value = enemyTowerHP / enemyMaxHP;
        if(playerTowerHP <= 0)
        {
            playerHpObject.SetActive(false);
            menu.GameOver();
        }
        else
        {
            playerHpObject.SetActive(true);
        }
        if(enemyTowerHP <= 0)
        {
            enemyHpObject.SetActive(false);
            menu.GameClear();
        }
        else
        {
            enemyHpObject.SetActive(true);
        }
        if(playerTowerHP < playerMaxHP - (playerSoundIdx * towerHpUnit))
        {
            towerHpSound.Play();
            playerSoundIdx++;
        }
        if(enemyTowerHP < enemyMaxHP - (enemySoundIdx * towerHpUnit))
        {
            towerHpSound.Play();
            enemySoundIdx++;
        }
//#if UNITY_EDITOR
//        if (Input.GetMouseButtonDown(0))
//        {
//            if (!EventSystem.current.IsPointerOverGameObject())
//            {
//                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
//                RaycastHit hit;
//                if (Physics.Raycast(ray, out hit))
//                {
//                    if (hit.collider.name == "PlayerTower")
//                    {
//                        upgradeMoneyButton.gameObject.SetActive(true);
//                    }
//                    else
//                    {
//                        upgradeMoneyButton.gameObject.SetActive(false);
//                    }
//                }
//            }
//        }
//#else
//        if(Input.GetTouch(0).phase != TouchPhase.Began)
//        {
//                return;
//        }
//        if(Input.touchCount > 0)
//        {
//        if(!EventSystem.current.IsPointerOverGameObject())
//	{  
//     Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
//            RaycastHit hit;
//            if(Physics.Raycast(ray, out hit) )
//            {
//                if(hit.collider.name == "PlayerTower")
//                {
//                    upgradeMoneyButton.gameObject.SetActive(true);
//                }
//                                else
//                {
//                    upgradeMoneyButton.gameObject.SetActive(false);
//                }
//            }
//	}
           
////        }
//#endif
    }

    public void UpgradeTower()
    {
        if(towerLevel == 2)
        {
            return;
        }
        if(moneyManager.money >= upgradeMoneyList[towerLevel])
        {
            if(towerLevel == 1)
            {
                mapManager.MovePlayerTower();
            }
            playerSoundIdx = 1;
            //타워 업그레이드 시 돈 관련 부분
            moneyManager.money -= upgradeMoneyList[towerLevel];
            moneyManager.timeMoney += 2;
            moneyManager.maxMoney *= 2;
            moneyManager.answerMoney *= 2;
            moneyManager.reduceMoney *= 2;
            //타워 관련된 부분
            towerLevel++;
            playerMaxHP += 10000;
            playerTowerHP = playerMaxHP;
            GetPlayerHpBarChange();
            playerTower.GetComponent<SpriteRenderer>().sprite = playerTowerSprite[towerLevel];
        }
        
    }
    public void EnemyTowerUpgrade()
    {
        if(enemyTowerLevel == 2)
        {
            return;
        }
        if(enemyTowerLevel == 1)
        {
            mapManager.MoveEnemyTower();
        }
        enemySoundIdx = 1;
        enemyTowerLevel++;
        enemyMaxHP += 10000;
        enemyTowerHP = enemyMaxHP;
        GetEnemyHpBarChange();
        enemyTower.GetComponent<SpriteRenderer>().sprite = enemyTowerSprite[enemyTowerLevel];
    }

    public void GetPlayerHpBarChange()
    {
        float scaleX = (towerHpUnit * towerUnitAmount)/ playerMaxHP;
        hpLine.GetComponent<HorizontalLayoutGroup>().gameObject.SetActive(false);
        foreach(Transform child in hpLine.transform)
        {
            child.gameObject.transform.localScale = new Vector3(scaleX, 1f, 1f);
        }
        hpLine.GetComponent<HorizontalLayoutGroup>().gameObject.SetActive(true);
    }
    public void GetEnemyHpBarChange()
    {
        float scaleX = (towerHpUnit * towerUnitAmount) / enemyMaxHP;
        enemyHpLine.GetComponent<HorizontalLayoutGroup>().gameObject.SetActive(false);
        foreach (Transform child in enemyHpLine.transform)
        {
            child.gameObject.transform.localScale = new Vector3(scaleX, 1f, 1f);
        }
        enemyHpLine.GetComponent<HorizontalLayoutGroup>().gameObject.SetActive(true);
    }

}
