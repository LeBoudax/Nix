using UnityEngine;

public class GameState : MonoBehaviour
{
    [Header("Menus")]
    [SerializeField] private float _timeLimit, _currentTime;
    [SerializeField] private GameObject _pauseMenu, _gameOverMenu;

    public bool IsPlaying;
    public float CurrentTime => _currentTime;

    //Singleton
    public static GameState Instance;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        IsPlaying = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsPlaying)
        {
            return;
        }

        _currentTime -= Time.deltaTime;
        
        if (_currentTime <= 0)
        {
            GameOver();
        }
    }

    public void StartGame()
    {
        _currentTime = _timeLimit;
        IsPlaying = true;
        SoundManager.Instance.ChangeMusic();
    }

    /// <summary>
    /// If the game is playing, pause; else, unpause
    /// </summary>
    public void PauseUnpause()
    {
        IsPlaying = !IsPlaying;
        _pauseMenu.SetActive(!_pauseMenu.activeSelf);
        
        //Baisser / monter le volume
        if (IsPlaying)
        {
            SoundManager.Instance.ChangeVolume(2f);
        }
        else
        {
            SoundManager.Instance.ChangeVolume(0.5f);
        }
    }

    public void GameOver()
    {
        Debug.Log("GAME OVER");
        _gameOverMenu.SetActive(true);
    }

    public void ResetGame()
    {
        IsPlaying = false;
        _currentTime = _timeLimit;
        SoundManager.Instance.PlayIntro();
    }

}
