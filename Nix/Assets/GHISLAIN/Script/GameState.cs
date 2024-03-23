using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class GameState : MonoBehaviour
{
    [SerializeField] private float _timeLimit, _currentTime;

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
    }

    public void GameOver()
    {
        Debug.Log("GAME OVER");
    }

}
