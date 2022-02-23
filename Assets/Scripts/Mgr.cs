using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Mgr : MonoBehaviour
{
    private static Mgr _inst;
    private bool _paused = true;
    public static bool Paused { get => _inst._paused; }
    void Awake()
    {
        _inst = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            _paused = !_paused;
        }
    }
}
