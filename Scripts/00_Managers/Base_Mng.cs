using System;
using UnityEngine;

public class Base_Mng : MonoBehaviour
{
    public static Base_Mng Instance { get; private set; }

    private UI_Mng s_UI_Mng = new UI_Mng();
    public UI_Mng UI { get { return s_UI_Mng; } }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}