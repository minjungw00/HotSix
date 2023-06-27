using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerHPManager_HJH : MonoBehaviour
{
    [Header("�����̴���")]
    public Slider playerHPSlider;
    public Slider enemyHPSlider;


    public float startPlayerTowerHP; // �÷����� Ÿ�� ���� ü��
    public float startEnemyTowerHP; //�� Ÿ�� ���� ü��

    public float playerMaxHP; //�÷��̾� �ִ� ü��
    public float enemyMaxHP;

    public float playerTowerHP; //�÷��̿� ���� ü��
    public float enemyTowerHP;
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
        playerHPSlider.value = playerTowerHP / playerMaxHP;
        enemyHPSlider.value = enemyTowerHP / enemyMaxHP;
        if(playerTowerHP <= 0)
        {
            Debug.Log("GameOVer");
        }
        if(enemyTowerHP <= 0)
        {
            Debug.Log("GaemClear");
        }
    }
}
