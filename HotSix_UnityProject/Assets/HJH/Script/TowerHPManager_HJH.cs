using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.XR;

public class TowerHPManager_HJH : MonoBehaviour
{
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

    public float playerMaxHP; //�÷��̾� �ִ� ü��
    public float enemyMaxHP;

    public float playerTowerHP; //�÷��̿� ���� ü��
    public float enemyTowerHP;

    public MoneyManager_HJH moneyManager;

    int towerLevel = 0;
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
        upgradeMoneyText.text = upgradeMoneyList[towerLevel].ToString() + "M";
        playerHPSlider.value = playerTowerHP / playerMaxHP;
        enemyHPSlider.value = enemyTowerHP / enemyMaxHP;
        if(playerTowerHP <= 0)
        {
            Debug.Log("GameOver");
        }
        if(enemyTowerHP <= 0)
        {
            Debug.Log("GameClear");
        }
#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0))
        {
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider.name == "PlayerTower")
                    {
                        upgradeMoneyButton.gameObject.SetActive(true);
                    }
                    else
                    {
                        upgradeMoneyButton.gameObject.SetActive(false);
                    }
                }
            }
        }
#else
        if(Input.GetTouch(0).phase != TouchPhase.Began)
        {
                return;
        }
        if(Input.touchCount > 0)
        {
        if(!EventSystem.current.IsPointerOverGameObject())
	{  
     Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
            RaycastHit hit;
            if(Physics.Raycast(ray, out hit) )
            {
                if(hit.collider.name == "PlayerTower")
                {
                    upgradeMoneyButton.gameObject.SetActive(true);
                }
                                else
                {
                    upgradeMoneyButton.gameObject.SetActive(false);
                }
            }
	}
           
        }
#endif
    }

    public void UpgradeTower()
    {
        if(towerLevel == 2)
        {
            return;
        }
        if(moneyManager.money >= upgradeMoneyList[towerLevel])
        {
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
            playerTower.GetComponent<SpriteRenderer>().sprite = playerTowerSprite[towerLevel];
        }
        
    }
}
