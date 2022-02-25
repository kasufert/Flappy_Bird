using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class Mgr : MonoBehaviour
{
    private static Mgr _inst;
    private bool _paused = true;
    public static bool Paused { get => _inst._paused; }
    private System.Random _rand;
    public static float RandNextFloat { get => (float)_inst._rand.NextDouble(); }

    public const float PIPE_DELAY_SECONDS = 1.1f;
    public const int TARGET_FRAMERATE = 144;
    public const int PIPE_DELAY_FRAMES = (int)(PIPE_DELAY_SECONDS * TARGET_FRAMERATE);
    public readonly Vector3 SPAWN_PIPES_AT = new Vector3(7.3f, 0, 0);
    private int _timeUntilNextPipe = PIPE_DELAY_FRAMES;
    private bool _isBirdDead = false;
    [SerializeField] private GameObject _pipe;

    void Awake()
    {
        _inst = this;
        _inst._rand = new System.Random(System.DateTime.Now.Millisecond);
        Application.targetFrameRate = TARGET_FRAMERATE;
        BirdScript.BirdDeath += OnBirdDeath;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            _paused = !_paused;
        }
        if (!_paused && !_isBirdDead)
        {
            _timeUntilNextPipe--;
            if (_timeUntilNextPipe == 0)
            {
                _timeUntilNextPipe = PIPE_DELAY_FRAMES;
                SpawnPipe();
            }
        }

        void SpawnPipe()
        {
            Instantiate(_pipe, SPAWN_PIPES_AT, Quaternion.identity);
        }
    }
    private void OnBirdDeath()
	{
        _isBirdDead = true;
	}
	private void OnGUI()
	{
        if (_isBirdDead)
		{
            GUI.Box(new Rect(100, 100, 300, 300), "You Died\n\n\n\n\n\nPress Space to Restart");
		}
        GUI.Label(new Rect(10, 10, 100, 50), "FPS: " + (int)(1 / Time.smoothDeltaTime));
	}
}
