using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Text TimerText;

    public int ScoreMax;
    public int PreSetTime;
    public List<float> ChangeTargetTimeTrigger;

    private int _score;
    public float _time;
    private List<float> _changeTargetTrigger;

    private bool _gameStart = true;
    private int _targetIndex = 0;

    void Start()
    {
        Init();
    }

    void Update()
    {
        if(!_gameStart)
        {
            return;
        }

        if (Input.GetKeyUp(KeyCode.Q))
        {
            IncreaseScore(1);
        }

        if (Input.GetKeyUp(KeyCode.W))
        {
            DecreaseScore(1);
        }

        Timer();
    }

    public void IncreaseScore(int value)
    {
        _score += value;
        print(_score);
        if(_score >= ScoreMax)
        {
            _score = ScoreMax;
            OnScoreMax();
        }
    }

    public void DecreaseScore(int value)
	{
        _score -= value;
        print(_score);
        if (_score <= 0)
		{
            _score = 0;
            OnScoreZero();
        }
    }

    private int GetCurrentTargetIndex(float time)
    {
        for (int i = 0; i < _changeTargetTrigger.Count; i++)
        {
            if(time > _changeTargetTrigger[i])
            {
                return i;
            }
        }

        return _changeTargetTrigger.Count;
    }

    private void Timer()
    {
        if (_time > 0)
        {
            _time -= Time.deltaTime;
            int index = GetCurrentTargetIndex(_time);
            if (_targetIndex != index)
            {
                OnChangeTarget(index);
            }
        }

        if (_time <= 0)
        {
            _time = 0;
            OnTimeUp();
        }

        TimerText.text = _time.ToString("0.00");
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
        _targetIndex = targetIndex;
        print(_targetIndex);
    }

    private void Init()
    {
        _score = 0;
        _time = PreSetTime;

        _changeTargetTrigger = ChangeTargetTimeTrigger.OrderByDescending(e => e).ToList();
    }
}
