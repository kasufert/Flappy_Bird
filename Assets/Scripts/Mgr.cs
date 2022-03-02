using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class Mgr : MonoBehaviour
{
	private static Mgr _inst;
	[SerializeField] private bool _paused = false;
	public static bool Paused { get => _inst._paused; }
	private System.Random _rand;
	public static float RandNextFloat { get => (float)_inst._rand.NextDouble(); }

	public const float PIPE_DELAY_SECONDS = .64f;
	public const int TARGET_FRAMERATE = 144;
	public const int PIPE_DELAY_FRAMES = (int)(PIPE_DELAY_SECONDS * TARGET_FRAMERATE);
	public const int MOVING_AVG_FRAMES = 72;
	public readonly Vector3 SPAWN_PIPES_AT = new Vector3(7.3f, 0, 0);
	[SerializeField] private int _timeUntilNextPipe = PIPE_DELAY_FRAMES;
	[SerializeField] private bool _isBirdDead = false;
	[SerializeField] private GameObject _pipe;
	public static event Action RestartGame;
	private bool _started = false;
	private ushort _score = 0;
	private ushort _highScore = 0;
	private float _frameRateSample;
	private int _currentFrameRate;

	public static void IncScore()
	{
		_inst._score++;
		if (_inst._score > _inst._highScore)
		{
			_inst._highScore = _inst._score;
		}
	}
	private void OnBirdDeath()
	{
		_isBirdDead = true;
	}
	private IEnumerator SpawnPipes_Coroutine(GameObject pipe, float delay)
	{
		while (true)
		{
			while (_paused || _isBirdDead)
			{
				yield return null;
			}
			Instantiate(_pipe, SPAWN_PIPES_AT, Quaternion.identity);
			yield return new WaitForSeconds(delay);
		}
	}
	void Awake()
	{
		_inst = this;
		_inst._rand = new System.Random(System.DateTime.Now.Millisecond);
		//Application.targetFrameRate = TARGET_FRAMERATE;
		BirdScript.BirdDeath += OnBirdDeath;
	}
	private void Start()
	{
		StartCoroutine(SpawnPipes_Coroutine(_pipe, PIPE_DELAY_SECONDS));
	}
	// Update is called once per frame
	void Update()
	{
		_frameRateSample += Time.unscaledDeltaTime;
		if (Time.frameCount % MOVING_AVG_FRAMES == 0)
		{
			_currentFrameRate = (int)(MOVING_AVG_FRAMES / _frameRateSample);
			_frameRateSample = 0;
		}
		if (!_started)
		{
			if (Input.GetKeyDown(KeyCode.Space))
			{
				_started = true;
				_paused = false;
				return;
			}
			return;
		}
		if (Input.GetKeyDown(KeyCode.P))
		{
			_paused = !_paused;
			PausePhysics();
			Debug.Log("Pause toggling...");
		}
		if (_isBirdDead && Input.GetKeyDown(KeyCode.Space))
		{
			Debug.Log("Restarting game...");
			RestartGame();
			_score = 0;
			_isBirdDead = false;
		}

		void PausePhysics()
		{
			if (_paused)
			{
				Time.timeScale = 0;
			}
			if (!_paused)
			{
				Time.timeScale = 1;
			}
		}
	}
	private void OnGUI()
	{
		if (_isBirdDead)
		{
			GUI.Box(new Rect(100, 100, 300, 300), "You Died\n\n\n\n\n\nPre ss Space to Restart");
		}
		if (!_started)
		{
			GUI.Box(new Rect(100, 100, 300, 300), "Flappy Bird\n\n\n\n\n\nPress Space to Start");
		}
		GUI.Box(new Rect(10, 10, 200, 100),
			"FPS: " + (int)(1 / Time.unscaledDeltaTime) +"\tScore: " + _score + "\n" +
			"Smoothed FPS: " + _currentFrameRate + "\n" +
			"High Score: " + _highScore);

	}
}
