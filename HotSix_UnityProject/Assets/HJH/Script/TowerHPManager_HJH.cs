using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerHPManager_HJH : MonoBehaviour
{
    [Header("�����̴���")]
    public Slider playerHPSlider;
    public Slider enemyHPSlider;


    public int startPlayerTowerHP; // �÷����� Ÿ�� ���� ü��
    public int startEnemyTowerHP; //�� Ÿ�� ���� ü��

    public int playerMaxHP; //�÷��̾� �ִ� ü��
    public int enemyMaxHP;

    public int playerTowerHP; //�÷��̿� ���� ü��
    public int enemyTowreHP;
    // Start is called before the first frame update
    void Start()
    {
        playerMaxHP = startEnemyTowerHP;
        enemyMaxHP = startEnemyTowerHP;
        playerTowerHP = startPlayerTowerHP;
        enemyTowreHP = startEnemyTowerHP;
    }

    // Update is called once per frame
    void Update()
    {
        playerHPSlider.value = (float)playerTowerHP / (float)playerMaxHP;
        enemyHPSlider.value = (float)enemyTowreHP / (float)enemyMaxHP;
    }
}
