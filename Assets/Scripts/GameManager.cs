using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : Singleton<GameManager>
{
    [SerializeField]
    private GameObject _victoryUI;

    [SerializeField]
    private GameObject _gameOverUI;

    [SerializeField]
    private GameObject _timerUI;

    [SerializeField]
    private TextMeshProUGUI _timerText;

    [SerializeField]
    private Player _player;

    [SerializeField]
    private Transform _floorBody;

    [SerializeField]
    private float _gameDuration = 30f;

    private float _timer;
    private bool _isPaused;

    public bool IsPaused
    {
        get
        {
           return _isPaused;
        }
    }

    public float TimeLeft01
    {
        get
        {
            return _timer / _gameDuration;
        }
    }

    public Vector3 FloorSize
    {
       get
        {
            return _floorBody.localScale;
        }
    }

    private void Start()
    {
        Restart();
    }

    private void Update()
    {
        if (!_isPaused)
        {
            _timer -= Time.deltaTime;
            _timer = Mathf.Clamp(_timer, 0, _gameDuration);
            _timerText.text = "Time left: " + _timer.ToString("F2");

            if (_timer == 0)
            {
                Victory();
            }
        }
    }

    public void Victory()
    {
        _isPaused = true;
        _victoryUI.SetActive(true);
        _timerUI.SetActive(false);
        _player.Reset();
    }

    public void GameOver()
    {
        _isPaused = true;
        _gameOverUI.SetActive(true);
        _timerUI.SetActive(false);
    }

    public void Restart()
    {
        _timer = _gameDuration;
        _gameOverUI.SetActive(false);
        _victoryUI.SetActive(false);
        _timerUI.SetActive(true);
        _player.Reset();
        _isPaused = false;
    }
}
