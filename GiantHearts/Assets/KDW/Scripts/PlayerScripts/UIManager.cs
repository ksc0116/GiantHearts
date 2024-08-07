using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class UIManager : MonoBehaviour
{
    public GameObject rawImage;
    public VideoPlayer videoPlayer;

    private PlaySound playSound;

    public GameObject inGameUI;
    public GameObject mainMeunUI;
    public GameObject CreditUI;
    public VideoPlay creditVideo;

    // player 정보
    public GameObject m_PlayerObject;

    // Manager 정보
    public GameManager m_MamagerObject;
    public BuildSystem m_BuildSystemObject;
    public Manager_SE m_Manager_SEObject;
    public Manager_BGM m_Manager_BGM;

    // UI 정보
    private GameObject m_currentUIObject;     // 현재 UI 상태

    // Main Meun 정보
    [Header("[Main Meun]")]
    public Button m_StateButton;
    public Button m_EndButton;

    private Navigation m_navigation;

    [Header("[InGame]")]
    public GameObject[] m_PlatformListImage = new GameObject[6];
    public GameObject m_InfoPlatform;
    [SerializeField]
    private Sprite[] m_InfoPlatformSpriteObject;     // 플랫폼 정보 창 Sprite 정보
    [SerializeField]
    private Image m_InfoPlatformImage;               // 플랫폼 정보 창 Image Object
    [SerializeField]
    private Image m_HelpInfoButtonObject;           // info button object
    [SerializeField]
    private Sprite[] m_HelpInfoSpriteButton;        // info button Sprite
    [SerializeField]
    private GameObject m_HelpWindowObject;           // 도움창 Obejct
    [SerializeField]
    private Image m_HowToImageObject;                // 게임 설명창 Object
    [SerializeField]
    private Sprite[] m_HowToSprite;                  // 게임 설명창 Sprite
    [SerializeField]
    private Sprite m_HowToRightSpriteButton, m_HowToLiftSpriteButton;   // 게임 설명창 Page 넘기기 Button Sprite
    [SerializeField]
    private Sprite m_HowToExitSpriteButton;          // 게임 설명창 나가기 Button Sprite
    private static int m_HowToCount = 1;

    [Header("[InfoPlatform Size or Color]")]
    [SerializeField]
    float m_InfoPlatformHeight;
    [SerializeField]
    float m_InfoPlatformWidth;
    [SerializeField]
    float m_InfoInterval;
    [SerializeField]
    float m_InfoColor_r, m_InfoColor_g, m_InfoColor_b;
    [SerializeField]
    float m_InfoOutLineSizeX, m_InfoOutLineSizeY;

    [Header("[Window]")]
    [SerializeField]
    private Image m_WindowImageObject;
    [SerializeField]
    private Sprite[] m_WindowSprite;

    [Header("[Cost]")]
    [SerializeField]
    private TMP_Text[] m_CostText;

    [Header("[CurrentGoal]")]
    [SerializeField]
    private Sprite[] m_CurrentGoalSpriteObject;
    [SerializeField]
    private Image m_CurrentGoalImage;

    private static int m_currentPlatformBox;        // 현재 선택중인 플랫폼박스 번호
    private TextMeshProUGUI m_platformBoxText;      // 플랫폼박스 Text 박스
    private Outline[] m_platformImageOutLine;       // 플랫폼박스 윤곽선 담을 변수
    private RectTransform m_platformRectTransform;  // 플랫폼 정보 Transform 정보

    private bool m_isPlatform = false;
    private int m_isPlatformNum = 0;
    void Start()
    {
        videoPlayer.time = 0.0f;

        playSound = gameObject.GetComponent<PlaySound>();
        m_platformRectTransform = m_InfoPlatform.GetComponent<RectTransform>();

        mainMeunUI = GameObject.Find("MainMenuUI");
        //CreditUI = GameObject.Find("CreditImage");
        //CreditUI.SetActive(false);

        m_currentUIObject = GameObject.Find("Canvas");
        m_StateButton.onClick.AddListener(StartButtonClickEnvent);

        m_currentUIObject = GameObject.Find("BuildModeUI");
        m_currentUIObject.transform.Find("InfoPlatform").gameObject.SetActive(false);

        m_currentUIObject = GameObject.Find("InGame");
        m_currentUIObject.transform.Find("BuildModeUI").gameObject.SetActive(false);
        m_currentUIObject.transform.Find("CurrentGoal").gameObject.SetActive(false);
        m_currentUIObject.transform.Find("Possessions").gameObject.SetActive(false);
        m_currentUIObject.transform.Find("Help").gameObject.SetActive(false);
        m_currentUIObject.transform.Find("Exit").gameObject.SetActive(false);

        m_currentUIObject = GameObject.Find("StageWindow");
        m_currentUIObject.transform.Find("Window").gameObject.SetActive(false);

        m_currentUIObject = GameObject.Find("Canvas");
        m_currentUIObject.transform.Find("OptionUI").gameObject.SetActive(false);
        m_currentUIObject.transform.Find("InGame").gameObject.SetActive(false);

        m_platformImageOutLine = new Outline[m_PlatformListImage.Length];

        // Button navigation 초기화
        //m_navigation = m_StateButton.navigation;
        //m_navigation.mode = Navigation.Mode.Explicit;
        //m_navigation.selectOnDown = m_CreditsButton;
        //m_StateButton.navigation = m_navigation;

        //m_Manager_BGM.TilteBGMPlay();       // Title BGM 시작
    }

    void Update()
    {
        m_navigation = m_StateButton.navigation;
        m_navigation.mode = Navigation.Mode.Explicit;

        // Manager에서 자원 정보를 받아와 Text로 표시
        if (inGameUI.activeSelf == true)
        {
            WindowStageChage();

            m_CostText[0].text = m_MamagerObject.ingredient.coalCount.ToString();
            m_CostText[1].text = m_MamagerObject.ingredient.copperCount.ToString();
            m_CostText[2].text = m_MamagerObject.ingredient.ironCount.ToString();

            m_CurrentGoalImage.sprite = m_CurrentGoalSpriteObject[m_MamagerObject.curStage];
        }
    }

    /// Main UI
    // Start Button 클릭 시
    public void StartButtonClickEnvent()
    {
        playSound.PlaySE(Manager.Instance.manager_SE.ES_Click);

        Manager.Instance.uiManager.GetComponent<FadeInOut>().FadeOut(
            Manager.Instance.gameManager.NextStage, 2);

        //m_Manager_SEObject.SE_StartButtonCilck();       // 사운드
        //m_Manager_BGM.BGMEnd();
    }

    // 메인메뉴 Start 버튼 클릭 시
    public void ChangeInGame()
    {
        // UI 상태 변경
        m_currentUIObject = GameObject.Find("Canvas");
        m_currentUIObject.transform.Find("MainMenuUI").gameObject.SetActive(false);
        m_currentUIObject.transform.Find("InGame").gameObject.SetActive(true);

        m_currentUIObject = GameObject.Find("InGame");
        m_currentUIObject.transform.Find("BuildModeUI").gameObject.SetActive(true);
        m_currentUIObject.transform.Find("CurrentGoal").gameObject.SetActive(true);
        m_currentUIObject.transform.Find("Possessions").gameObject.SetActive(true);
        m_currentUIObject.transform.Find("Help").gameObject.SetActive(true);
        m_currentUIObject.transform.Find("Exit").gameObject.SetActive(true);

        m_currentUIObject = GameObject.Find("StageWindow");
        m_currentUIObject.transform.Find("Window").gameObject.SetActive(true);

        // InGame 동작 시작
        m_PlayerObject.SetActive(true);     // Player Active 활성화
        m_PlayerObject.GetComponent<KSC_PlayerMove>().enabled = true;



        // 게임 시작하면 curStage가 -1이기 때문에 여기서 +1을 해서 0으로 바꿔준다
        //Manager.Instance.gameManager.curStage++;
    }

    // Credit Button Cilck 시
    public void CreditButtonCilckEnvent()
    {
        CreditVideoPlay();
    }

    // Credit 영상 시작
    private void CreditVideoPlay()
    {
        playSound.PlaySE(Manager.Instance.manager_SE.ES_Click);
        rawImage.SetActive(true);
        videoPlayer.enabled = true;
        videoPlayer.Play();

        Invoke("CreditToTitle", 18);

    }
    private void CreditToTitle()
    {
        GetComponent<FadeInOut>().FadeOut(CreditEnd, 2);
    }

    public void CreditEnd()
    {
        rawImage.SetActive(false);
        videoPlayer.enabled = false;
    }

    // Game Exit Button Cilck 시
    public void EndButtonClickEvnet()
    {
#if UNITY_EDITOR
        playSound.PlaySE(Manager.Instance.manager_SE.ES_Click);
        UnityEditor.EditorApplication.isPlaying = false;    // play 모드를 false로.
#else
        Application.Quit();
#endif
    }

    /// InGame UI


    // 건설모드 On Off
    public void BuildModeUIActiveSelfSetting()
    {
        ///일단 제외
        // 메인화면일땐 안켜지게 예외처리
        m_currentUIObject = GameObject.Find("Canvas");
        if (m_currentUIObject.transform.Find("MainMenuUI").gameObject.activeSelf == true)
        {
            return;
        }

        m_currentUIObject = GameObject.Find("InGame");
        if (m_currentUIObject.transform.Find("BuildModeUI").gameObject.activeSelf == true)
        {
            m_currentUIObject.transform.Find("BuildModeUI").gameObject.SetActive(false);
        }
        else if (m_currentUIObject.transform.Find("BuildModeUI").gameObject.activeSelf == false)
        {
            m_currentUIObject.transform.Find("BuildModeUI").gameObject.SetActive(true);
        }
    }

    // 템플릿 모드에서 key 눌렀을 경우
    public void PlatformListSelection(int count)
    {
        m_currentPlatformBox = count - 1;

        switch (count)
        {
            case 1:
                m_InfoPlatformImage.sprite = m_InfoPlatformSpriteObject[0];     // Sprite 이미지 변경
                m_platformRectTransform.anchoredPosition =
                    new Vector3(m_InfoPlatformHeight, m_InfoPlatformWidth, 0f);

                if (m_isPlatform == false || m_isPlatformNum == count)
                {
                    PlatformInfoOnOff();
                }

                OutLineChenge(m_currentPlatformBox);

                m_isPlatformNum = 1;
                break;
            case 2:
                m_InfoPlatformImage.sprite = m_InfoPlatformSpriteObject[1];
                m_platformRectTransform.anchoredPosition =
                    new Vector3(m_InfoPlatformHeight, m_InfoPlatformWidth - m_InfoInterval, 0f);

                if (m_isPlatform == false || m_isPlatformNum == count)
                {
                    PlatformInfoOnOff();
                }

                OutLineChenge(m_currentPlatformBox);

                m_isPlatformNum = 2;
                break;
            case 3:
                m_InfoPlatformImage.sprite = m_InfoPlatformSpriteObject[2];
                m_platformRectTransform.anchoredPosition =
                    new Vector3(m_InfoPlatformHeight, m_InfoPlatformWidth - (m_InfoInterval * 2), 0f);

                if (m_isPlatform == false || m_isPlatformNum == count)
                {
                    PlatformInfoOnOff();
                }

                OutLineChenge(m_currentPlatformBox);

                m_isPlatformNum = 3;
                break;
            case 4:
                m_InfoPlatformImage.sprite = m_InfoPlatformSpriteObject[3];
                m_platformRectTransform.anchoredPosition =
                    new Vector3(m_InfoPlatformHeight, m_InfoPlatformWidth - (m_InfoInterval * 3), 0f);

                if (m_isPlatform == false || m_isPlatformNum == count)
                {
                    PlatformInfoOnOff();
                }

                OutLineChenge(m_currentPlatformBox);

                m_isPlatformNum = 4;
                break;
            case 5:
                m_InfoPlatformImage.sprite = m_InfoPlatformSpriteObject[4];
                m_platformRectTransform.anchoredPosition =
                    new Vector3(m_InfoPlatformHeight, m_InfoPlatformWidth - (m_InfoInterval * 4), 0f);

                if (m_isPlatform == false || m_isPlatformNum == count)
                {
                    PlatformInfoOnOff();
                }

                OutLineChenge(m_currentPlatformBox);

                m_isPlatformNum = 5;
                break;
            case 6:
                m_InfoPlatformImage.sprite = m_InfoPlatformSpriteObject[5];
                m_platformRectTransform.anchoredPosition =
                    new Vector3(m_InfoPlatformHeight, m_InfoPlatformWidth - (m_InfoInterval * 5), 0f);

                if (m_isPlatform == false || m_isPlatformNum == count)
                {
                    PlatformInfoOnOff();
                }

                OutLineChenge(m_currentPlatformBox);

                m_isPlatformNum = 6;
                break;
            case 7:
                m_InfoPlatformImage.sprite = m_InfoPlatformSpriteObject[6];
                m_platformRectTransform.anchoredPosition =
                    new Vector3(m_InfoPlatformHeight, m_InfoPlatformWidth - (m_InfoInterval * 6), 0f);

                if (m_isPlatform == false || m_isPlatformNum == count)
                {
                    PlatformInfoOnOff();
                }

                OutLineChenge(m_currentPlatformBox);

                m_isPlatformNum = 7;
                break;
        }
    }

    // 플래폼 목록 윤곽선 처리
    private void OutLineChenge(int count)
    {
        for (int i = 0; i < 6; i++)
        {
            m_platformImageOutLine[i] = m_PlatformListImage[i].GetComponent<Outline>();
            if (i == count)
            {
                if (m_isPlatform == true)
                {
                    m_platformImageOutLine[count].enabled = true;
                    m_platformImageOutLine[count].effectColor = new Color(m_InfoColor_r / 255f, m_InfoColor_g / 255f, m_InfoColor_b / 255f);
                    m_platformImageOutLine[count].effectDistance = new Vector2(m_InfoOutLineSizeX, -m_InfoOutLineSizeY);
                }
                else 
                {
                    m_platformImageOutLine[count].enabled = false;
                }
            }
            else
            {
                m_platformImageOutLine[i].enabled = false;
            }
        }
    }

    // 플랫폼 정보창 On Off
    private void PlatformInfoOnOff()
    {
        m_currentUIObject = GameObject.Find("BuildModeUI");

        if (m_isPlatform == false)
        {
            m_currentUIObject.transform.Find("InfoPlatform").gameObject.SetActive(true);
            m_isPlatform = true;
        }
        else if (m_isPlatform == true)
        {
            m_currentUIObject.transform.Find("InfoPlatform").gameObject.SetActive(false);
            m_isPlatform = false;
        }
    }

    // 옵션창
    public void OptionUIOnOff()
    {
        playSound.PlaySE(Manager.Instance.manager_SE.ES_Click);

        m_currentUIObject = GameObject.Find("Canvas");
        if (m_currentUIObject.transform.Find("MainMenuUI").gameObject.activeSelf == true)
        {
            return;
        }

        if (m_currentUIObject.transform.Find("OptionUI").gameObject.activeSelf == true)
        {
            m_currentUIObject.transform.Find("OptionUI").gameObject.SetActive(false);
            m_PlayerObject.GetComponent<KSC_PlayerMove>().enabled = true;
        }
        else if (m_currentUIObject.transform.Find("OptionUI").gameObject.activeSelf == false)
        {
            m_currentUIObject.transform.Find("OptionUI").gameObject.SetActive(true);
            m_PlayerObject.GetComponent<KSC_PlayerMove>().enabled = false;  // player 정지
        }
    }

    // 재시작 버튼
    public void ReStartButtonEvent()
    {
        OptionUIOnOff();

        playSound.PlaySE(Manager.Instance.manager_SE.ES_Click);

        GetComponent<FadeInOut>().FadeOut(
            Manager.Instance.gameManager.RestartStage, 2);
    }

    // 타이틀로
    public void TitleButtonEvent()
    {
        playSound.PlaySE(Manager.Instance.manager_SE.ES_Click);

        Manager.Instance.gameManager.ResetGame();

        //m_isBuildMode = m_isBuildMode == true ? false : true;
        m_isPlatform = true;

        m_currentUIObject = GameObject.Find("Canvas");
        m_currentUIObject.transform.Find("OptionUI").gameObject.SetActive(false);

        m_currentUIObject.transform.Find("MainMenuUI").gameObject.SetActive(true);

        m_currentUIObject = GameObject.Find("BuildModeUI");
        m_currentUIObject.transform.Find("InfoPlatform").gameObject.SetActive(false);
        

        m_currentUIObject = GameObject.Find("StageWindow");
        m_currentUIObject.transform.Find("Window").gameObject.SetActive(false);
        
        m_currentUIObject = GameObject.Find("InGame");
        m_currentUIObject.transform.Find("BuildModeUI").gameObject.SetActive(false);
        m_currentUIObject.transform.Find("CurrentGoal").gameObject.SetActive(false);
        m_currentUIObject.transform.Find("Possessions").gameObject.SetActive(false);
        m_currentUIObject.transform.Find("Exit").gameObject.SetActive(false);

        m_HelpInfoButtonObject.sprite = m_HelpInfoSpriteButton[0];
        m_currentUIObject.transform.Find("HelpWindow").gameObject.SetActive(false);
        m_HowToCount = 1;

        m_currentUIObject.SetActive(false);

        // Player 끄기
        m_PlayerObject.SetActive(false);

        //m_BuildSystemObject.BuildMode();

        //m_Manager_BGM.TilteBGMPlay();       // Title BGM 시작
    }

    // 도움창 ON OFF
    public void HelpWindowOnOff()
    {
        playSound.PlaySE(Manager.Instance.manager_SE.ES_Click);

        if (m_HelpInfoButtonObject.sprite == m_HelpInfoSpriteButton[0])
        {
            m_currentUIObject = GameObject.Find("InGame");
            if (m_currentUIObject.transform.Find("HelpWindow").gameObject.activeSelf == true)
            {
                m_currentUIObject.transform.Find("HelpWindow").gameObject.SetActive(false);
            }
            else if (m_currentUIObject.transform.Find("HelpWindow").gameObject.activeSelf == false)
            {
                m_currentUIObject.transform.Find("HelpWindow").gameObject.SetActive(true);

                m_currentUIObject = GameObject.Find("HelpWindow");
                m_currentUIObject.transform.Find("HowtoLeftPage").gameObject.SetActive(false);
                m_currentUIObject.transform.Find("HowtoRightPage").gameObject.SetActive(true);
                m_currentUIObject.transform.Find("HowtoWindoExit").gameObject.SetActive(false);

                m_HelpInfoButtonObject.sprite = m_HelpInfoSpriteButton[1];

                m_HowToImageObject.sprite = m_HowToSprite[0];
            }
        }
    }

    // HowTo Page 넘기기 Button 클릭 이벤트
    public void HowToPageButtonClickEvent()
    {
        playSound.PlaySE(Manager.Instance.manager_SE.ES_Click);

        m_currentUIObject = GameObject.Find("HelpWindow");

        if (m_HowToCount == 1)
        {
            m_HowToImageObject.sprite = m_HowToSprite[1];
            if(m_HowToSprite[1] == null)
            {
                //Debug.Log("sprite null");
            }
            m_currentUIObject.transform.Find("HowtoLeftPage").gameObject.SetActive(true);
            m_currentUIObject.transform.Find("HowtoRightPage").gameObject.SetActive(false);
            m_currentUIObject.transform.Find("HowtoWindoExit").gameObject.SetActive(true);

            m_HowToCount = 2;
        }
        else if (m_HowToCount == 2)
        {
            m_HowToImageObject.sprite = m_HowToSprite[0];
            m_currentUIObject.transform.Find("HowtoLeftPage").gameObject.SetActive(false);
            m_currentUIObject.transform.Find("HowtoRightPage").gameObject.SetActive(true);
            m_currentUIObject.transform.Find("HowtoWindoExit").gameObject.SetActive(false);

            m_HowToCount = 1;
        }
    }

    // HowTo Exit Button 클릭 이벤트
    public void HowToExitButtonClick()
    {
        playSound.PlaySE(Manager.Instance.manager_SE.ES_Click);

        m_currentUIObject = GameObject.Find("HelpWindow");
        m_currentUIObject.transform.Find("HowtoLeftPage").gameObject.SetActive(false);
        m_currentUIObject.transform.Find("HowtoRightPage").gameObject.SetActive(false);
        m_currentUIObject.transform.Find("HowtoWindoExit").gameObject.SetActive(false);

        m_currentUIObject = GameObject.Find("InGame");
        m_currentUIObject.transform.Find("HelpWindow").gameObject.SetActive(false);

        m_HelpInfoButtonObject.sprite = m_HelpInfoSpriteButton[0];

        m_HowToCount = 1;
    }

    // Window UI 전환
    public void WindowStageChage()
    {
        m_currentUIObject = GameObject.Find("Canvas");
        if (m_currentUIObject.transform.Find("FadeImage").gameObject.activeSelf == true)
        {
            m_HelpInfoButtonObject.sprite = m_HelpInfoSpriteButton[0];
            m_HowToCount = 1;

            // 도움창 UI 끄기
            m_currentUIObject = GameObject.Find("InGame");
            m_currentUIObject.transform.Find("HelpWindow").gameObject.SetActive(false);

            // 플랫폼 UI 끄기
            if (m_isPlatform == false)
            {
                m_currentUIObject = GameObject.Find("BuildModeUI");
                m_currentUIObject.transform.Find("InfoPlatform").gameObject.SetActive(false);
                m_isPlatform = false;
            }
        }

        switch (m_MamagerObject.curStage)
        {
            case 0:
                m_WindowImageObject.sprite = m_WindowSprite[0];
                break;
            case 1:
                m_WindowImageObject.sprite = m_WindowSprite[1];
                break;
            case 2:
                m_WindowImageObject.sprite = m_WindowSprite[2];
                break;
            case 3:
                m_WindowImageObject.sprite = m_WindowSprite[3];
                break;
            default:
                break;
        }
    }
}
