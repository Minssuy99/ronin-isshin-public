using System;
using UnityEngine;

public class Game_Mng : Singleton<MonoBehaviour>
{
    protected override void Awake()
    {
        base.Awake();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Start()
    {
        Sound_Mng.Instance.PlayBGM("Zipang");
    }
}