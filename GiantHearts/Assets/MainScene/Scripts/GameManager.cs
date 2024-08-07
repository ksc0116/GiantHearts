using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class GameManager : MonoBehaviour
{
    public struct Ingredient
    {
        public int ironCount;
        public int copperCount;
        public int coalCount;
    }

    public NavMeshAgent playerNav;

    public RenderTexture renderTexture;

    // ���� �÷��̾ ������ �ִ� ���
    public Ingredient ingredient;

    // ġƮŰ Ȱ��ȭ ������ ������ �ִ� ��ȭ
    public Ingredient prevIngredient;

    // ���� ��������
    public int curStage;

    // �ִ� �������� ����
    public int maxStage;

    // �� �������� �� ���޵Ǵ� ���
    public Ingredient[] stagePaymentIngredient;

    // �������� �� �� ���� ���� ����ó�� �Ǿ����� Ȯ���ϴ� bool
    public Dictionary<int, bool[]> isStageClear;

    public bool isRealStageClear;

    public bool isGameClear;

    private bool isStageUp;

    private PlaySound playSound;

    public BuildSystem buildSystem;

    public Transform playerTransform;

    public bool isCheatMode;

    public void SetStageIngredient(int stage)
    {
        ingredient.ironCount = 0;
        ingredient.copperCount = 0;
        ingredient.coalCount = 0;

        for (int i = 0; i <= stage; i++)
        {
            ingredient.ironCount += stagePaymentIngredient[i].ironCount;
            ingredient.copperCount += stagePaymentIngredient[i].copperCount;
            ingredient.coalCount += stagePaymentIngredient[i].coalCount;
        }
    }

    private void SetIngredient()
    {
        stagePaymentIngredient = new Ingredient[4];

        stagePaymentIngredient[0].ironCount = 30;
        stagePaymentIngredient[0].copperCount = 0;
        stagePaymentIngredient[0].coalCount = 0;

        stagePaymentIngredient[1].ironCount = 30;
        stagePaymentIngredient[1].copperCount = 10;
        stagePaymentIngredient[1].coalCount = 0;

        stagePaymentIngredient[2].ironCount = 50;
        stagePaymentIngredient[2].copperCount = 40;
        stagePaymentIngredient[2].coalCount = 3;

        stagePaymentIngredient[3].ironCount = 15;
        stagePaymentIngredient[3].copperCount = 5;
        stagePaymentIngredient[3].coalCount = 0;
    }

    public void NextStage()
    {
        curStage++;

        if(curStage<maxStage)
        {
            // �� ������ �Ҹ�
            playSound.PlaySE(Manager.Instance.manager_SE.ES_hatch_open);

            ingredient.ironCount += stagePaymentIngredient[curStage].ironCount;
            ingredient.copperCount += stagePaymentIngredient[curStage].copperCount;
            ingredient.coalCount += stagePaymentIngredient[curStage].coalCount;

            isStageUp = false;
        }
        else
        {
            isGameClear = true;
        }

        // �������� �� BGM
        if (curStage == 0)
        {
            Manager.Instance.uiManager.ChangeInGame();
            Manager.Instance.manager_BGM.bgmAudio.clip = Manager.Instance.manager_BGM.BG_GH_stage1;
        }
        else if (curStage == 1)
        {
            Manager.Instance.manager_BGM.bgmAudio.clip = Manager.Instance.manager_BGM.BG_GH_stage2;
        }
        else if (curStage == 2)
        {
            Manager.Instance.manager_BGM.bgmAudio.clip = Manager.Instance.manager_BGM.BG_GH_stage3;
        }
        else if(curStage == 3)
        {
            Manager.Instance.manager_BGM.bgmAudio.clip = Manager.Instance.manager_BGM.BG_GH_stage4;
        }
        Manager.Instance.manager_BGM.bgmAudio.Play();
    }

    private void CheckClear()
    {
        for (int i = 0; i <= curStage; ++i)
        {
            bool[] tempBool;
            isStageClear.TryGetValue(i, out tempBool);

            //Debug.Log("curStage " + curStage);
            //Debug.Log("i Index " + i);

            for (int j = 0; j < tempBool.Length; j++)
            {
                if (!tempBool[j])
                {
                    // �ϳ��� Ŭ���� �ȵǾ� ������ Ŭ���� �ƴ�
                    isRealStageClear = false;
                    return;
                }
            }
        }
        isRealStageClear = true;
    }

    private void SetCorrectArr()
    {
        isStageClear = new Dictionary<int, bool[]>();
        isStageClear.Add(0, new bool[1]);
        isStageClear.Add(1, new bool[1]);
        isStageClear.Add(2, new bool[2]);
        isStageClear.Add(3, new bool[1]);
    }

    void Start()
    {
        RenderTexture.active = renderTexture;
        GL.Clear(true,true,new Color(0,0,0));
        playSound = GetComponent<PlaySound>();
        SetCorrectArr();
        SetIngredient();
    }

    void Update()
    {
        if ((Input.GetKey(KeyCode.LeftControl)) && (Input.GetKeyDown(KeyCode.M)))
        {
            if (isCheatMode)
            {
                isCheatMode = false;
                ingredient.ironCount = prevIngredient.ironCount;
                ingredient.copperCount = prevIngredient.copperCount;
                ingredient.coalCount = prevIngredient.coalCount;
            }
            else
            {
                isCheatMode = true;
                prevIngredient.ironCount = ingredient.ironCount;
                prevIngredient.copperCount = ingredient.copperCount;
                prevIngredient.coalCount = ingredient.coalCount;
            }
        }

        if(isCheatMode)
        {
            ingredient.ironCount = 999;
            ingredient.copperCount = 999;
            ingredient.coalCount = 999;
        }

        //// ���� �÷��̾� ����� ��ŵŰ ������ ��ŵ
        //if((Manager.Instance.uiManager.videoPlayer.isPlaying )&&
        //    (Input.GetKey(KeyCode.LeftControl) && ((Input.GetKey(KeyCode.F12)))))
        //{
        //    Manager.Instance.uiManager.GetComponent<FadeInOut>().FadeOut(Manager.Instance.uiManager.TitleButtonEvent, 2);
        //}

        if(curStage < maxStage)
        {
            CheckClear();
        }

        if (isRealStageClear && !isGameClear && !isStageUp && curStage >= 0)
        {
            isRealStageClear = false;

            isStageUp = true;

            // �������� Ŭ���� ���� ���������� ���� ��� ����
            if(curStage < maxStage-1)
            {
                Manager.Instance.uiManager.GetComponent<FadeInOut>().FadeOut(NextStage, 2);
            }
            else
            {
                Manager.Instance.uiManager.GetComponent<FadeInOut>().FadeOut(EndScene, 2);
            }
        }

        CheatKeyUpdate();
    }

    private void EndScene()
    {
        //Manager.Instance.uiManager.inGameUI.SetActive(false);
        Manager.Instance.uiManager.rawImage.SetActive(true);
        Manager.Instance.uiManager.videoPlayer.enabled = true;
        Manager.Instance.uiManager.videoPlayer.Play();

        Invoke("GoToTitle", 18.0f);
    }

    private void GoToTitle()
    {
        Manager.Instance.uiManager.GetComponent<FadeInOut>().FadeOut(Manager.Instance.uiManager.TitleButtonEvent,2);
    }

    private void CheatKeyUpdate()
    {
        if ((Input.GetKey(KeyCode.LeftControl)) && (Input.GetKeyDown(KeyCode.F1)))
        {
            Manager.Instance.uiManager.GetComponent<FadeInOut>().FadeOutINT(JumpToStage, 0, 2);
        }

        if ((Input.GetKey(KeyCode.LeftControl)) && (Input.GetKeyDown(KeyCode.F2)))
        {
            Manager.Instance.uiManager.GetComponent<FadeInOut>().FadeOutINT(JumpToStage, 1, 2);
        }

        if ((Input.GetKey(KeyCode.LeftControl)) && (Input.GetKeyDown(KeyCode.F3)))
        {
            Manager.Instance.uiManager.GetComponent<FadeInOut>().FadeOutINT(JumpToStage, 2, 2);
        }

        if ((Input.GetKey(KeyCode.LeftControl)) && (Input.GetKeyDown(KeyCode.F4)))
        {
            Manager.Instance.uiManager.GetComponent<FadeInOut>().FadeOutINT(JumpToStage, 3, 2);
        }
    }

    private void JumpToStage(int jumpStage)
    {
        playerNav.enabled = false;
        playerTransform.position = new Vector3(0.0f, 1.2f, 0.0f);
        playerNav.enabled = true;

        SetStageIngredient(jumpStage);

        buildSystem.DeleteAllGrid();

        // ���� �׸��忡 ��ġ�� ��� �÷��� ���� �ʿ� ��ȭ�� �����־�� ��

        // ==========================================================

        // �� �ݱ�
        Manager.Instance.doorManager.DoorClose();

        curStage = jumpStage;

        // �� ������ �Ҹ�
        playSound.PlaySE(Manager.Instance.manager_SE.ES_hatch_open);

        // BGM �ٲٱ�
        if (curStage == 0)
        {
            Manager.Instance.uiManager.ChangeInGame();
            Manager.Instance.manager_BGM.bgmAudio.clip = Manager.Instance.manager_BGM.BG_GH_stage1;
        }
        else if (curStage == 1)
        {
            Manager.Instance.manager_BGM.bgmAudio.clip = Manager.Instance.manager_BGM.BG_GH_stage2;
        }
        else if (curStage == 2)
        {
            Manager.Instance.manager_BGM.bgmAudio.clip = Manager.Instance.manager_BGM.BG_GH_stage3;
        }
        else
        {
            Manager.Instance.manager_BGM.bgmAudio.clip = Manager.Instance.manager_BGM.BG_GH_stage4;
        }
        Manager.Instance.manager_BGM.bgmAudio.Play();
    }

    // ���� ���� �ʱ�ȭ
    public void ResetGame()
    {
        playerNav.enabled = false;
        playerTransform.position = new Vector3(0.0f, 1.2f, 0.0f);
        playerNav.enabled = true;

        // ���� �������� �ʱ�ȭ
        curStage = -1;

        // ��� Ÿ��Ʋ ȭ�� ������� �ٲ�
        Manager.Instance.manager_BGM.bgmAudio.clip = Manager.Instance.manager_BGM.BG_GH_Title;
        Manager.Instance.manager_BGM.bgmAudio.Play();

        // ��ȭ �ʱ�ȭ
        ingredient.ironCount = 0;
        ingredient.copperCount = 0;
        ingredient.coalCount = 0;

        // ���� �÷��̾� �ʱ�ȭ
        Manager.Instance.uiManager.rawImage.SetActive(false);
        Manager.Instance.uiManager.videoPlayer.enabled = false;

        buildSystem.DeleteAllGrid();

       

        // ���� �׸��忡 ��ġ�� ��� �÷��� ���� �ʿ� ��ȭ�� �����־�� ��

        // ==========================================================
    }

    // ���� �������� �����
    public void RestartStage()
    {
        Manager.Instance.doorManager.DoorClose();

        playerNav.enabled = false;
        playerTransform.position = new Vector3(0.0f, 1.2f, 0.0f);
        playerNav.enabled = true;

        SetStageIngredient(curStage);

        buildSystem.DeleteAllGrid();

        // ���� �׸��忡 ��ġ�� ��� �÷��� ���� �ʿ� ��ȭ�� �����־�� ��

        // ==========================================================


    }
}
