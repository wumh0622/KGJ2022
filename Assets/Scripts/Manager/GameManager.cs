using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    const int sceneNum = 2;
    [Header("GameObject")]
    public Text TimerText;
    public Image ScoreBar;

    [Header("Setting")]
    public int ScoreMax;
    public int PresetTime;
    public List<float> ChangeTargetTime;

    public GameObject player;

    public AI_Data AiData;
    public AI_Controller aiController;
    public GameObject girl;

    public QG qG;
    public Dialog_Box dialog;

    public List<AudioInfo> Audios;

    private int _score;
    private float _time;
    private bool _gameStart;
    private int _targetIndex = 0;
    private List<float> _changeTargetTime;
    private Dictionary<AudioKey, AudioClip> _audioDict;
    public PARTICLEREACTION pARTICLEREACTION;

    public AudioSource mAudioEffectPlayer = null;
    public GameObject winPanel;
    public GameObject losePanel;
    public Text winText;
    public Text loseText;

    public enum AudioKey
    {
        Attack, //攻擊、被攻擊
        HappyEnd, //結局 好
        BadEnd, //結局 壞
        ReactionBad, //VTuber反應 反感
        ReactionEnterFail, //VTuber反應 錯誤語句
        ReactionGood, //VTuber反應 好感
        ReactionNormal, //VTuber反應 一般
        PickWord, //撿到字
        ButtonEnter, //發送按鍵 
        EnterGood, //輸入好感詞句
        EnterBad, //輸入反感詞句
        EnterFail, //輸入錯誤詞句
    }

    [System.Serializable]
    public class AudioInfo
    {
        public string name;
        public AudioKey AudioKey;
        public AudioClip AudioClips;
    }

    protected override void Awake()
    {
        qG = FindObjectOfType<QG>();
        dialog = FindObjectOfType<Dialog_Box>();
    }

    void Start()
    {
        Time.timeScale = 1.0f;
        Init();

        _gameStart = true;
        MediatorManager<AI_Data>.Instance.Publish(AI_State.SetAI, this, new MediatorArgs<AI_Data>(AiData));
        girl = aiController.mAIData.mAIRoot;

        _audioDict = Audios.ToDictionary(info => info.AudioKey, info => info.AudioClips);
        mAudioEffectPlayer = GetComponent<AudioSource>();
    }

    void Update()
    {
        //Test
        if (Input.GetKeyUp(KeyCode.O))
        {
            IncreaseScore(50);
        }

        if (Input.GetKeyUp(KeyCode.L))
        {
            DecreaseScore(1);
        }
        //Test

        if (!_gameStart)
        {
            Time.timeScale = 0.0f;
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

    public void TryGetRandomAudioClip(AudioKey audioKey/*, out AudioClip audioClip*/)
    {
        AudioClip aAudioClip = null;

        if (!_audioDict.TryGetValue(audioKey, out aAudioClip) || aAudioClip == null)
        {
            return /*false*/;
        }
        mAudioEffectPlayer.PlayOneShot(aAudioClip);
       /* int index = UnityEngine.Random.Range(0, audioClips.Count);

        return audioClips[index];*/
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
        Time.timeScale = 0.0f;
        winPanel.SetActive(true);
        winText.text = dialog.GetEndWord(true);
    }

    private void OnTimeUp()
    {
        _gameStart = false;
        losePanel.SetActive(true);
        loseText.text = dialog.GetEndWord(false);
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
                TryGetRandomAudioClip(AudioKey.EnterGood);
                TryGetRandomAudioClip(AudioKey.ReactionGood);
                MediatorManager<string>.Instance.Publish(AI_State.State_Shy, this, null);
                dialog.ShowRespond(AI_State.State_Shy);
                pARTICLEREACTION.PlayParticle(1);
            }
            else if (score < 0)
            {
                TryGetRandomAudioClip(AudioKey.EnterBad);
                TryGetRandomAudioClip(AudioKey.ReactionBad);
                MediatorManager<string>.Instance.Publish(AI_State.State_Angry, this, null);
                dialog.ShowRespond(AI_State.State_Angry);
            }
            else
            {
                TryGetRandomAudioClip(AudioKey.EnterFail);
                TryGetRandomAudioClip(AudioKey.ReactionEnterFail);
                MediatorManager<string>.Instance.Publish(AI_State.State_Confuse, this, null);

                dialog.ShowRespond(AI_State.State_Confuse);
                pARTICLEREACTION.PlayParticle(0);
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

    public void NextLevel()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(1);
    }

    public void Restart()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(0);
        
    }

    public void EndGame()
    {
        Application.Quit();
    }

}
