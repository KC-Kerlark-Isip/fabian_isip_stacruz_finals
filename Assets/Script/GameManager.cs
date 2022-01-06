using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour{

    public GameObject ship1Prefab;
    public Text TextScore;
    public Text TextLife;
    public Text TextLevel;
    public Text TextHighscore;

    public GameObject panelMenu;
    public GameObject panelPlay;
    public GameObject panelLevelCompleted;
    public GameObject panelGameOver;

    public GameObject[] levels;




    public static GameManager Instance { get; private set; }
    public enum State { MENU, INIT, PLAY, LEVELCOMPLETED, LOADLEVEL, GAMEOVER }
    State _state;
    uint numHit = 0;
    GameObject _currentLife;
    GameObject _currentLevel;
    bool _isSwitchingState;


    private int _score;

    public int Score
    {
        get { return _score; }
        set
        {
            _score = value;
            TextScore.text = "SCORE:" + _score;
        }
    }

    private int _level;

    public int Level
    {
        get { return _level; }
        set
        {
            _level = value;
            TextLevel.text = "LEVEL:" + _level;
        }
    }

    private int _life;

    public int Life
    {
        get { return _life; }
        set
        {
            _life = value;
            TextLife.text = "LIFE:" + _life;
        }
    }


    private void Awake()
    {
        
          
        
    }

   
    void Start()
    {
        Instance = this;
        SwitchState(State.MENU);
        //PlayerPrefs.DeleteKey("highscore");
    }

    public void PlayClicked()
    {
        SwitchState(State.INIT);
    }

    // Update is called once per frame
    void Update()
    {
        switch (_state)
        {
            case State.MENU:
                break;
            case State.INIT:
                break;
            case State.PLAY:
                if (_currentLife == null)
                {
                    if (Life > 0)
                    {
                        _currentLife = Instantiate(ship1Prefab);
                    }
                    else
                    {
                        SwitchState(State.GAMEOVER);
                    }
                }
                if (_currentLife != null && numHit == 0 && !_isSwitchingState)
                {
                    SwitchState(State.LEVELCOMPLETED);
                    
                }

                break;
            case State.LEVELCOMPLETED:
                break;
            case State.LOADLEVEL:
                break;
            case State.GAMEOVER:
                if (Input.anyKeyDown)
                {
                    SwitchState(State.MENU);
                }
                break;
        }
    }



    public void AddHit()
    {
        numHit++;
    }

    public void RemoveHit()
    {
        numHit--;

        if(numHit == 0)
        {
           
        }
    }

    IEnumerator SwitchDelay(State newState, float delay = 0)
    {
        _isSwitchingState = true;
        yield return new WaitForSeconds(delay);
        EndState();
        _state = newState;
        BeginState(newState);
        _isSwitchingState = false;
    }

    public void SwitchState(State newState, float delay = 0)
    {
        StartCoroutine(SwitchDelay(newState, delay));
    }

    void BeginState(State newState)
    {
        switch (newState)
        {
            case State.MENU:
                Cursor.visible = true;
                TextHighscore.text = "Highscore: " + PlayerPrefs.GetInt("Highscore");
                panelMenu.SetActive(true);
                break;
            case State.INIT:
                Cursor.visible = false;
                panelPlay.SetActive(true);
                Score = 0;
                Level = 0;
                Life = 3;
                
                SwitchState(State.LOADLEVEL);
                break;
            case State.PLAY:
                break;
            case State.LEVELCOMPLETED:
                Destroy(ship1Prefab.gameObject);
                Destroy(_currentLevel);
                Level++;
                panelLevelCompleted.SetActive(true);
                SwitchState(State.LOADLEVEL, 2F);
                break;
            case State.LOADLEVEL:
                if (Level >= levels.Length)
                {
                    SwitchState(State.GAMEOVER);
                }
                else
                {
                    _currentLevel = Instantiate(levels[Level]);
                    SwitchState(State.PLAY);
                }
                break;
            case State.GAMEOVER:
                if (Score > PlayerPrefs.GetInt("Highscore"))
                {
                    PlayerPrefs.SetInt("Highscore", Score);
                }
                panelGameOver.SetActive(true);
                Destroy(_currentLife);
                Destroy(_currentLevel);
                Level--;
                break;
        }
    }

    void EndState()
    {
        switch (_state)
        {
            case State.MENU:
                panelMenu.SetActive(false);
                break;
            case State.INIT:
                break;
            case State.PLAY:
                break;
            case State.LEVELCOMPLETED:
                panelLevelCompleted.SetActive(false);
                break;
            case State.LOADLEVEL:
                break;
            case State.GAMEOVER:
                panelPlay.SetActive(false);
                panelGameOver.SetActive(false);
                break;
        }
    }


}
