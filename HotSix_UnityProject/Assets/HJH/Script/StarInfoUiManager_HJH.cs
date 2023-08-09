using TMPro;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

public class StarInfoUiManager_HJH : MonoBehaviour
{
    public GameObject[] starParents;
    // Start is called before the first frame update
    void Start()
    {

    }

    private void OnEnable()
    {
        int stageNum = (int)GameManager.instance.currentStage;
        for (int i = 0; i < 3; i++)
        {
            bool clear = GameManager.instance.userData.stageStar[stageNum].stageStar[i];
            int condition = GameManager.instance.starCondition[stageNum].whatIsCondition[i];
            SetStarCondition(stageNum,i, condition, clear);
        }
    }

    public void SetStarCondition(int stage,int what, int condition, bool clear) //what = ���° ���ӿ�����Ʈ(�ؿ��� ���� 0,1,2) condition = � ��������, clear = ������ star�� ����� ������ 
    {
        if (clear)
        {
            starParents[what].transform.GetChild(0).gameObject.GetComponent<Image>().sprite = GameManager.instance.starImage[1];
        }
        else
        {
            starParents[what].transform.GetChild(0).gameObject.GetComponent<Image>().sprite = GameManager.instance.starImage[0];
        }
        TMP_Text text = starParents[what].transform.GetChild(1).GetComponent<TMP_Text>();
        switch (condition)
        {
            case 0:
                if (LocalizationSettings.SelectedLocale == LocalizationSettings.AvailableLocales.Locales[0]) //����
                {
                    text.text = "Stage Clear";
                }
                else
                {
                    text.text = "�������� Ŭ����";
                }
                break;
            case 1:
                if (LocalizationSettings.SelectedLocale == LocalizationSettings.AvailableLocales.Locales[0]) //����
                {
                    text.text = "Clear in " + GameManager.instance.starCondition[stage].gameClearTime / 60 + ":" + GameManager.instance.starCondition[stage].gameClearTime % 60 + "sec";
                }
                else
                {
                    text.text = GameManager.instance.starCondition[stage].gameClearTime/60 +":" + GameManager.instance.starCondition[stage].gameClearTime%60 + "�� �̸�";
                }
                break;
            case 2:
                if (LocalizationSettings.SelectedLocale == LocalizationSettings.AvailableLocales.Locales[0]) //����
                {
                    text.text = "Use less than " + GameManager.instance.starCondition[stage].gameClearTime + "mathcoin";
                }
                else
                {
                    text.text = "�޽�����"+GameManager.instance.starCondition[stage].mathCoinAmount + "�� �̸� ���";
                }
                break;
        }


    }


    // Update is called once per frame
    void Update()
    {

    }
}
