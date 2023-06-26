using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MathProblem_HJH : MonoBehaviour
{
    [SerializeField] WJ_Connector wj_conn;
    [SerializeField] CurrentStatus currentStatus;
    public CurrentStatus CurrentStatus => currentStatus;

    [Header("Panels")]
    [SerializeField] GameObject panel_diag_chooseDiff;  //���̵� ���� �г�
    [SerializeField] GameObject panel_question;         //���� �г�(����,�н�)

    [SerializeField] Text textDescription;        //���� ���� �ؽ�Ʈ
    [SerializeField] TEXDraw textEquation;           //���� �ؽ�Ʈ(��TextDraw�� ���� �ʿ�)
    [SerializeField] Button[] btAnsr = new Button[4]; //���� ��ư��
    TEXDraw[] textAnsr;                  //���� ��ư�� �ؽ�Ʈ(��TextDraw�� ���� �ʿ�)

    [Header("Status")]
    int currentQuestionIndex;
    bool isSolvingQuestion;
    float questionSolveTime;

    [Header("For Debug")]
    [SerializeField] WJ_DisplayText wj_displayText;         //�ؽ�Ʈ ǥ�ÿ�(�ʼ�X)
    [SerializeField] Button getLearningButton;      //���� �޾ƿ��� ��ư

    [Header("Money")]
    public MoneyManager_HJH moneyManager;
    int wrongTry = 0; //Ʋ�� Ƚ��
    public int firstMoney; //���� �ٷ� ������ �� �޴� ��
    public int reduceMoney; // ���� Ʋ���� �� ���ҵǴ¾�

    public TMP_InputField answerInputField;

    private void Awake()
    {
        //Debug.Log("1");
        textAnsr = new TEXDraw[btAnsr.Length];
        for (int i = 0; i < btAnsr.Length; ++i)

            textAnsr[i] = btAnsr[i].GetComponentInChildren<TEXDraw>();

        wj_displayText.SetState("�����", "", "", "");
    }

    private void OnEnable()
    {
        Setup();
    }

    private void Setup()
    {
        switch (currentStatus)
        {
            case CurrentStatus.WAITING:
                panel_diag_chooseDiff.SetActive(true);
                break;
        }

        if (wj_conn != null)
        {
            wj_conn.onGetDiagnosis.AddListener(() => GetDiagnosis());
            wj_conn.onGetLearning.AddListener(() => GetLearning(0));
        }
        else Debug.LogError("Cannot find Connector");
        answerInputField.onSubmit.AddListener(WriteAnswer);
    }

    private void Update()
    {
        if (isSolvingQuestion) questionSolveTime += Time.deltaTime;
    }

    /// <summary>
    /// ������ ���� �޾ƿ���
    /// </summary>
    private void GetDiagnosis()
    {
        switch (wj_conn.cDiagnotics.data.prgsCd)
        {
            case "W":
                MakeQuestion(wj_conn.cDiagnotics.data.textCn,
                            wj_conn.cDiagnotics.data.qstCn,
                            wj_conn.cDiagnotics.data.qstCransr,
                            wj_conn.cDiagnotics.data.qstWransr);
                wj_displayText.SetState("������ ��", "", "", "");
                break;
            case "E":
                Debug.Log("������ ��! �н� �ܰ�� �Ѿ�ϴ�.");
                wj_displayText.SetState("������ �Ϸ�", "", "", "");
                currentStatus = CurrentStatus.LEARNING;
                getLearningButton.interactable = true;
                wj_conn.Learning_GetQuestion();
                break;
        }
    }

    /// <summary>
    ///  n ��° �н� ���� �޾ƿ���
    /// </summary>
    private void GetLearning(int _index)
    {
        if (_index == 0) currentQuestionIndex = 0;

        MakeQuestion(wj_conn.cLearnSet.data.qsts[_index].textCn,
                    wj_conn.cLearnSet.data.qsts[_index].qstCn,
                    wj_conn.cLearnSet.data.qsts[_index].qstCransr,
                    wj_conn.cLearnSet.data.qsts[_index].qstWransr);
    }

    /// <summary>
    /// �޾ƿ� �����͸� ������ ������ ǥ��
    /// </summary>
    private void MakeQuestion(string textCn, string qstCn, string qstCransr, string qstWransr)
    {
        panel_diag_chooseDiff.SetActive(false);
        panel_question.SetActive(true);

        string correctAnswer;
        string[] wrongAnswers;

        int ran = Random.Range(0, 2);
        for(int i = 0; i<textAnsr.Length; i++)
        {
            
            if (!int.TryParse(textAnsr[i].text, out int result)) //������ ������ �ƴϸ� ������ ��ư���·� ������
            {

                ran = 0; 
                break;
            }
        }
        textDescription.text = textCn;
        textEquation.text = qstCn;
        correctAnswer = qstCransr;
        wrongAnswers = qstWransr.Split(',');
        if (ran == 0)
        {

            int ansrCount = Mathf.Clamp(wrongAnswers.Length, 0, 3) + 1;
            answerInputField.gameObject.SetActive(false);
            for (int i = 0; i < btAnsr.Length; i++)
            {
                if (i < ansrCount)
                    btAnsr[i].gameObject.SetActive(true);
                else
                    btAnsr[i].gameObject.SetActive(false);
            }

            int ansrIndex = Random.Range(0, ansrCount);

            for (int i = 0, q = 0; i < ansrCount; ++i, ++q)
            {
                if (i == ansrIndex)
                {
                    textAnsr[i].text = correctAnswer;
                    --q;
                }
                else
                    textAnsr[i].text = wrongAnswers[q];
            }
            isSolvingQuestion = true;
        }
        else
        {
            for(int i = 0; i< btAnsr.Length; ++i)
            {
                btAnsr[i].gameObject.SetActive(false);
            }
            answerInputField.gameObject.SetActive(true);
            isSolvingQuestion = true;
        }

    }

    //��ǲ�ʵ�� �� �־��� ��
    public void WriteAnswer(string answer)
    {
        bool isCorrect;
        string ansrCwYn = "N";
        switch (currentStatus)
        {
            case CurrentStatus.DIAGNOSIS:
                isCorrect = answer.CompareTo(wj_conn.cDiagnotics.data.qstCransr) == 0 ? true : false;
                ansrCwYn = isCorrect ? "Y" : "N";
                if (isCorrect)
                {
                    moneyManager.money += firstMoney - (reduceMoney * wrongTry);
                    wrongTry = 0;
                    isSolvingQuestion = false;
                    wj_conn.Diagnosis_SelectAnswer(answer, ansrCwYn, (int)(questionSolveTime * 1000));
                    wj_displayText.SetState("������ ��", answer, ansrCwYn, questionSolveTime + " ��");
                    panel_question.SetActive(false);
                    questionSolveTime = 0;
                }
                else
                {
                    wrongTry += 1;
                }
                break;
            case CurrentStatus.LEARNING:
                isCorrect = answer.CompareTo(wj_conn.cLearnSet.data.qsts[currentQuestionIndex].qstCransr) == 0 ? true : false;
                ansrCwYn = isCorrect ? "Y" : "N";

                if (ansrCwYn == "Y")
                {
                    moneyManager.money += firstMoney - (reduceMoney * wrongTry);
                    wrongTry = 0;
                    isSolvingQuestion = false;
                    currentQuestionIndex++;

                    wj_conn.Learning_SelectAnswer(currentQuestionIndex, answer, ansrCwYn, (int)(questionSolveTime * 1000));

                    wj_displayText.SetState("����Ǯ�� ��", answer, ansrCwYn, questionSolveTime + " ��");

                    if (currentQuestionIndex >= 8)
                    {
                        panel_question.SetActive(false);
                        wj_displayText.SetState("����Ǯ�� �Ϸ�", "", "", "");
                        wj_conn.Learning_GetQuestion();
                    }
                    else GetLearning(currentQuestionIndex);

                    questionSolveTime = 0;
                }
                else if (ansrCwYn == "N")
                {
                    wrongTry += 1;
                }
                break;
        }
        answerInputField.text = "";
        
    }

    /// <summary>
    /// ���� ���� �¾Ҵ� �� üũ
    /// </summary>
    public void SelectAnswer(int _idx)
    {
        bool isCorrect;
        string ansrCwYn = "N";

        switch (currentStatus)
        {
            case CurrentStatus.DIAGNOSIS:
                isCorrect = textAnsr[_idx].text.CompareTo(wj_conn.cDiagnotics.data.qstCransr) == 0 ? true : false;
                ansrCwYn = isCorrect ? "Y" : "N";
                if(ansrCwYn == "Y")
                {
                    moneyManager.money += firstMoney - (reduceMoney * wrongTry);
                    wrongTry = 0;
                    isSolvingQuestion = false;
                    wj_conn.Diagnosis_SelectAnswer(textAnsr[_idx].text, ansrCwYn, (int)(questionSolveTime * 1000));
                    wj_displayText.SetState("������ ��", textAnsr[_idx].text, ansrCwYn, questionSolveTime + " ��");
                    panel_question.SetActive(false);
                    questionSolveTime = 0;
                }
                else if(ansrCwYn == "N")
                {
                    wrongTry += 1;
                    btAnsr[_idx].transform.gameObject.SetActive(false);
                }
                break;

            case CurrentStatus.LEARNING:
                isCorrect = textAnsr[_idx].text.CompareTo(wj_conn.cLearnSet.data.qsts[currentQuestionIndex].qstCransr) == 0 ? true : false;
                ansrCwYn = isCorrect ? "Y" : "N";

                if(ansrCwYn == "Y")
                {
                    moneyManager.money += firstMoney - (reduceMoney * wrongTry);
                    wrongTry = 0;
                    isSolvingQuestion = false;
                    currentQuestionIndex++;

                    wj_conn.Learning_SelectAnswer(currentQuestionIndex, textAnsr[_idx].text, ansrCwYn, (int)(questionSolveTime * 1000));

                    wj_displayText.SetState("����Ǯ�� ��", textAnsr[_idx].text, ansrCwYn, questionSolveTime + " ��");

                    if (currentQuestionIndex >= 8)
                    {
                        panel_question.SetActive(false);
                        wj_displayText.SetState("����Ǯ�� �Ϸ�", "", "", "");
                        wj_conn.Learning_GetQuestion();
                    }
                    else GetLearning(currentQuestionIndex);

                    questionSolveTime = 0;
                }
                else if (ansrCwYn == "N")
                {
                    wrongTry += 1;
                    btAnsr[_idx].transform.gameObject.SetActive(false);
                }
                break;
        }
    }

    public void DisplayCurrentState(string state, string myAnswer, string isCorrect, string svTime)
    {
        if (wj_displayText == null) return;

        wj_displayText.SetState(state, myAnswer, isCorrect, svTime);
    }

    #region Unity ButtonEvent
    public void ButtonEvent_ChooseDifficulty(int a)
    {
        currentStatus = CurrentStatus.DIAGNOSIS;
        wj_conn.FirstRun_Diagnosis(a);
    }
    public void ButtonEvent_GetLearning()
    {
        wj_conn.Learning_GetQuestion();
        wj_displayText.SetState("����Ǯ�� ��", "-", "-", "-");
    }
    #endregion
}
