using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{
    [Header("GameObject")]
    public Text TimerText;
    public Slider ScoreBar;

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

    void Start()
    {
        Init();

        _gameStart = true;
    }

    void Update()
    {
        //Test
        if (Input.GetKeyUp(KeyCode.Q))
        {
            IncreaseScore(1);
        }

        if (Input.GetKeyUp(KeyCode.W))
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

    private void SetScore(int score, int scoreMax = -1)
    {
        _score = score;
        ScoreBar.value = score;

        if(scoreMax != -1)
        {
            ScoreBar.maxValue = scoreMax;
        }
    }

    private void Init()
    {
        SetScore(0, ScoreMax);

        _time = PresetTime;

        _changeTargetTime = ChangeTargetTime.OrderByDescending(e => e).ToList();
    }
}
