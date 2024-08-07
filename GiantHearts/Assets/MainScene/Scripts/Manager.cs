using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    public static Manager Instance;

    [Header("Manager")]
    public ObjectPoolSystem objectPool;
    public GameManager gameManager;
    public Manager_SE manager_SE;
    public Manager_BGM manager_BGM;
    public UIManager uiManager;
    public DoorManager doorManager;

    private void Awake()
    {
        if (Instance != this)
            Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
