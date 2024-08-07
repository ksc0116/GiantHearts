using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformState : MonoBehaviour
{
    [SerializeField] GameObject originObject;
    [SerializeField] GameObject redObject;
    [SerializeField] GameObject greenObject;

    [SerializeField]    public bool isInstallable = false;
    [SerializeField]    public bool isBuild = false;
    [SerializeField]    private int ironCount;
    [SerializeField]    private int copperCount;
    [SerializeField]    private int coalCount;

    private bool isSoundPlay;
    private PlaySound playSound;


    private void Awake()
    {
        gameObject.AddComponent<PlaySound>();
        playSound = gameObject.GetComponent<PlaySound>();
    }

    void Start()
    {
    }

    public void Sell()
    {
        if (Manager.Instance == null)
        {
            Debug.Log("manager null");
        }
        Manager.Instance.gameManager.ingredient.ironCount += ironCount;
        Manager.Instance.gameManager.ingredient.coalCount += coalCount;
        Manager.Instance.gameManager.ingredient.copperCount += copperCount;
    }

    public Vector3 GetItemvalue()
    {
        Vector3 itemvalue = new Vector3(ironCount, copperCount, coalCount);

        return itemvalue;
    }

    void Update()
    {
        if (!isBuild)
        {
            originObject.SetActive(false);
            if (isInstallable)
            {
                greenObject.SetActive(true);
                redObject.SetActive(false);
            }
            else
            {
                greenObject.SetActive(false);
                redObject.SetActive(true);
            }
        }
        else
        {
            if(!isSoundPlay)
            {
                isSoundPlay = true;
                playSound.PlaySE(Manager.Instance.manager_SE.ES_Hit);
            }
            originObject.SetActive(true);
            greenObject.SetActive(false);
            redObject.SetActive(false);
        }
    }

    private void OnDisable()
    {
        isSoundPlay = false;
    }
}
