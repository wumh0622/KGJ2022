using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{
    [Header("GameObject")]
    public Text TimerText;
    public Image ScoreBar;

    [Header("Setting")]
    public int ScoreMax;
    public int PresetTime;
    public List<float> ChangeTargetTime;

    private int _score;
    public float _time;
    private List<float> _changeTargetTime;

    private bool _gameStart;
    private int _targetIndex = 0;

    public GameObject player;

    public AI_Data AiData;
    public AI_Controller aiController;
    public GameObject girl;

    public QG qG;
    public Dialog_Box dialog;


    protected override void Awake()
    {
        qG = FindObjectOfType<QG>();
        dialog = FindObjectOfType<Dialog_Box>();
    }

    void Start()
    {
        Init();

        _gameStart = true;
        MediatorManager<AI_Data>.Instance.Publish(AI_State.SetAI, this, new MediatorArgs<AI_Data>(AiData));
        girl = aiController.mAIData.mAIRoot;

    }

    void Update()
    {
        //Test
        if (Input.GetKeyUp(KeyCode.O))
        {
            IncreaseScore(1);
        }

        if (Input.GetKeyUp(KeyCode.L))
        {
            DecreaseScore(1);
        }
        //Test

        if (!_gameStart)
        {
            print("Game Over");
            return;
        }

        Timer();
    }

    public void IncreaseScore(int value)
    {
        int score = _score + value;
        if (score >= ScoreMax)
        {
            score = ScoreMax;
            OnScoreMax();
        }

        if (score <= 0)
        {
            score = 0;
            OnScoreZero();
        }

        SetScore(score);
    }

    public void DecreaseScore(int value)
	{
        int score = _score - value;
        if(score <= 0)
        {
            score = 0;
            OnScoreZero();
        }

        SetScore(score);
    }

    private int GetCurrentTimeIndex(float time)
    {
        for (int i = 0; i < _changeTargetTime.Count; i++)
        {
            if(time > _changeTargetTime[i])
            {
                return i;
            }
        }

        return _changeTargetTime.Count;
    }

    private void Timer()
    {
        if (_time > 0)
        {
            _time -= Time.deltaTime;
            int index = GetCurrentTimeIndex(_time);
            if (_targetIndex != index)
            {
                _targetIndex = index;
                OnChangeTarget(index);
            }
        }

        if (_time <= 0)
        {
            _time = 0;
            OnTimeUp();
        }

        if (TimerText != null)
        {
            TimerText.text = _time.ToString("0.00");
        }
    }

    private void OnScoreZero()
    {
        print("Score Zero");
    }

    private void OnScoreMax()
    {
        print("Score Max");
    }

    private void OnTimeUp()
    {
        _gameStart = false;
        print("Time Up");
    }

    private void OnChangeTarget(int targetIndex)
    {
        print(targetIndex);

        SetScore(0);
    }

    private void SetScore(int score)
    {
        _score = score;
        ScoreBar.fillAmount = (float)score / (float)ScoreMax;
    }

    private void Init()
    {
        SetScore(0);

        _time = PresetTime;

        _changeTargetTime = ChangeTargetTime.OrderByDescending(e => e).ToList();
    }

    public void ConfirmWord()
    {
        if(dialog.HaveWord())
        {
            int score = dialog.CheckWord();
            qG.HideAllWord(true);
            if (score > 0)
            {
                MediatorManager<string>.Instance.Publish(AI_State.State_Shy, this, null);
                dialog.ShowRespond(AI_State.State_Shy);
            }
            else if (score < 0)
            {
                MediatorManager<string>.Instance.Publish(AI_State.State_Angry, this, null);
                dialog.ShowRespond(AI_State.State_Angry);
            }
            else
            {
                MediatorManager<string>.Instance.Publish(AI_State.State_Nothing, this, null);

                dialog.ShowRespond(AI_State.State_Nothing);
            }

            IncreaseScore(score);


            SimpleTimerManager.Instance.RunTimer(NextStep, 3.0f);
        }
        
    }

    public void NextStep()
    {
        MediatorManager<string>.Instance.Publish(AI_State.State_Nothing, this, null);
        qG.createQuestion();
        dialog.ClearRespond();
    }

}
