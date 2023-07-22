using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class ProfileManager_HJH : MonoBehaviour
{
    public Image profileImage;
    public Sprite[] images;
    public TMP_Text nameText;
    public TMP_Text stageProgress;
    public TMP_Text stageWinRate;
    public TMP_Text stageClearTimeAVG;
    public TMP_Text stageSolveMathProblem;
    public TMP_Text problemCorrectRate;
    public TMP_Text mathCoinAmount;
    public GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.instance;
        Debug.Log(profileImage.name);
        Debug.Log(gameManager.userData.porfileImg);
        profileImage.sprite = images[gameManager.userData.porfileImg];
        nameText.text = gameManager.userData.userName;
        stageProgress.text = "�������� ���൵ : " + gameManager.userData.stageProgress.ToString();
        if ((float)(gameManager.userData.loseCount + gameManager.userData.winCount) > 0)
        {
            stageWinRate.text = "�������� �·� : " + ((float)gameManager.userData.winCount / (float)(gameManager.userData.loseCount + gameManager.userData.winCount) * 100).ToString() + "%";
        }
        else
        {
            stageWinRate.text = "�������� �·� : 0%";
        }
        if (gameManager.userData.winCount > 0)
        {
            int time = Mathf.CeilToInt((gameManager.userData.stageClearTime / (float)(gameManager.userData.winCount)));
            int min = time/60;
            int sec = time%60;
            stageClearTimeAVG.text = "��� Ŭ����ð� : " + min +" �� " + sec +" ��" ;
        }
        else
        {
            stageClearTimeAVG.text = "��� Ŭ����ð� : 0";
        }
        stageSolveMathProblem.text = "Ǭ ��ü ���й��� �� : " + gameManager.userData.solveCount.ToString();
        if (gameManager.userData.tryCount > 0)
        {
            problemCorrectRate.text = "��� ���� Ǯ�� ����� : " + Mathf.Floor(((float)gameManager.userData.solveCount / (float)gameManager.userData.tryCount) * 10000)/100+ "%";
        }
        else
        {
            problemCorrectRate.text = "��� ���� Ǯ�� ����� : 0%";
        }
        mathCoinAmount.text = "�� ȹ�� �޽����� : " + gameManager.userData.mathCoinAmount.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            MoveScene();
        }
    }

    public void MoveScene()
    {
        SceneManager.LoadScene("StageScene");
    }

}
