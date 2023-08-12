using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.XR;

public class TowerHPManager_HJH : MonoBehaviour
{
    [Header("���� ����µ� ���°�")]
    public GameObject hpLine;
    public GameObject enemyHpLine;
    [Header("�����̴���")]
    public Slider playerHPSlider;
    public Slider enemyHPSlider;

    public GameObject playerTower; //�÷��̾� Ÿ�� ������Ʈ
    public Sprite[] playerTowerSprite; // �÷��̾� Ÿ�� ��������Ʈ��
    public GameObject enemyTower; // �� Ÿ�� ������Ʈ
    public Sprite[] enemyTowerSprite; // �� Ÿ�� ��������Ʈ��

    public TMP_Text upgradeMoneyText;
    public GameObject upgradeMoneyButton;
    public int[] upgradeMoneyList;


    public float startPlayerTowerHP; // �÷����� Ÿ�� ���� ü��
    public float startEnemyTowerHP; //�� Ÿ�� ���� ü��

    public GameObject playerHpObject;
    public GameObject enemyHpObject;

    public float playerMaxHP; //�÷��̾� �ִ� ü��
    public float enemyMaxHP;

    public float playerTowerHP; //�÷��̿� ���� ü��
    public float enemyTowerHP;

    public MoneyManager_HJH moneyManager;

    public Menu_HJH menu;

    int towerLevel = 0;
    int enemyTowerLevel = 0;

    public MapManager_HJH mapManager;
    // Start is called before the first frame update
    void Start()
    {
        playerMaxHP = startEnemyTowerHP;
        enemyMaxHP = startEnemyTowerHP;
        playerTowerHP = startPlayerTowerHP;
        enemyTowerHP = startEnemyTowerHP;
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
            //Ÿ�� ���׷��̵� �� �� ���� �κ�
            moneyManager.money -= upgradeMoneyList[towerLevel];
            moneyManager.timeMoney += 2;
            moneyManager.maxMoney *= 2;
            moneyManager.answerMoney *= 2;
            moneyManager.reduceMoney *= 2;
            //Ÿ�� ���õ� �κ�
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
        enemyTowerLevel++;
        enemyTowerHP += 10000;
        GetEnemyHpBarChange();
        enemyTower.GetComponent<SpriteRenderer>().sprite = enemyTowerSprite[towerLevel];
    }

    public void GetPlayerHpBarChange()
    {
        float scaleX = 10000f/ playerMaxHP;
        hpLine.GetComponent<HorizontalLayoutGroup>().gameObject.SetActive(false);
        foreach(Transform child in hpLine.transform)
        {
            child.gameObject.transform.localScale = new Vector3(scaleX, 1f, 1f);
        }
        hpLine.GetComponent<HorizontalLayoutGroup>().gameObject.SetActive(true);
    }
    public void GetEnemyHpBarChange()
    {
        float scaleX = 10000f / enemyMaxHP;
        enemyHpLine.GetComponent<HorizontalLayoutGroup>().gameObject.SetActive(false);
        foreach (Transform child in enemyHpLine.transform)
        {
            child.gameObject.transform.localScale = new Vector3(scaleX, 1f, 1f);
        }
        enemyHpLine.GetComponent<HorizontalLayoutGroup>().gameObject.SetActive(true);
    }

}
