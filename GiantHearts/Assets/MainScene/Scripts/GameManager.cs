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

    // 현재 플레이어가 가지고 있는 재료
    public Ingredient ingredient;

    // 치트키 활성화 이전에 가지고 있던 재화
    public Ingredient prevIngredient;

    // 현재 스테이지
    public int curStage;

    // 최대 스테이지 갯수
    public int maxStage;

    // 각 스테이지 당 지급되는 재료
    public Ingredient[] stagePaymentIngredient;

    // 스테이지 당 각 문이 현재 정답처리 되었는지 확인하는 bool
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
            // 문 열리는 소리
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

        // 스테이지 별 BGM
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
                    // 하나라도 클리어 안되어 있으면 클리어 아님
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

        //// 비디오 플레이어 재생중 스킵키 누르면 스킵
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

            // 스테이지 클리어 다음 스테이지에 대한 재료 공급
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

        // 현재 그리드에 설치된 모든 플랫폼 제거 필요 재화를 돌려주어야 함

        // ==========================================================

        // 문 닫기
        Manager.Instance.doorManager.DoorClose();

        curStage = jumpStage;

        // 문 열리는 소리
        playSound.PlaySE(Manager.Instance.manager_SE.ES_hatch_open);

        // BGM 바꾸기
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

    // 게임 완전 초기화
    public void ResetGame()
    {
        playerNav.enabled = false;
        playerTransform.position = new Vector3(0.0f, 1.2f, 0.0f);
        playerNav.enabled = true;

        // 현재 스테이지 초기화
        curStage = -1;

        // 브금 타이틀 화면 브금으로 바꿈
        Manager.Instance.manager_BGM.bgmAudio.clip = Manager.Instance.manager_BGM.BG_GH_Title;
        Manager.Instance.manager_BGM.bgmAudio.Play();

        // 재화 초기화
        ingredient.ironCount = 0;
        ingredient.copperCount = 0;
        ingredient.coalCount = 0;

        // 비디오 플레이어 초기화
        Manager.Instance.uiManager.rawImage.SetActive(false);
        Manager.Instance.uiManager.videoPlayer.enabled = false;

        buildSystem.DeleteAllGrid();

       

        // 현재 그리드에 설치된 모든 플랫폼 제거 필요 재화를 돌려주어야 함

        // ==========================================================
    }

    // 현재 스테이지 재시작
    public void RestartStage()
    {
        Manager.Instance.doorManager.DoorClose();

        playerNav.enabled = false;
        playerTransform.position = new Vector3(0.0f, 1.2f, 0.0f);
        playerNav.enabled = true;

        SetStageIngredient(curStage);

        buildSystem.DeleteAllGrid();

        // 현재 그리드에 설치된 모든 플랫폼 제거 필요 재화를 돌려주어야 함

        // ==========================================================


    }
}
