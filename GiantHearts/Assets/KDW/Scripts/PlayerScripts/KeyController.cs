using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class KeyController : MonoBehaviour
{
    public Animator playerAnim;

    public UIManager m_UIObject;
    public BuildSystem m_BuildSystem;
    public GameObject mainMeunUIObject;     // 메인메뉴 UI Object

    bool uiActive = false;

    // Start is called before the first frame update
    void Start()
    {
    }

    void Update()
    {
        if (Input.inputString != " ")
        {
            if (!uiActive)
            {
                m_UIObject.BuildModeUIActiveSelfSetting();
                uiActive = true;
            }

            if ((Input.GetKeyDown(KeyCode.Alpha1)) && (GameObject.Find("PlatformListImage1") != null)
                && (!playerAnim.GetBool("isSpanner")))
            {
                if (GameObject.Find("PlatformListImage1").gameObject.activeSelf == true)
                {
                    m_UIObject.PlatformListSelection(1);
                    
                    if(m_BuildSystem.mode == BuildSystem.CharactorMode.Build1)
                    {
                        m_BuildSystem.mode = BuildSystem.CharactorMode.none;
                    }
                    else
                    {
                        m_BuildSystem.mode = BuildSystem.CharactorMode.Build1;
                    }
                }
            }
            else if ((Input.GetKeyDown(KeyCode.Alpha2)) && (GameObject.Find("PlatformListImage2") != null)
                        && (!playerAnim.GetBool("isSpanner")))
            {
                if (GameObject.Find("PlatformListImage2").gameObject.activeSelf == true)
                {
                    m_UIObject.PlatformListSelection(2);

                    if (m_BuildSystem.mode == BuildSystem.CharactorMode.Build2)
                    {
                        m_BuildSystem.mode = BuildSystem.CharactorMode.none;
                    }
                    else
                    {
                        m_BuildSystem.mode = BuildSystem.CharactorMode.Build2;
                    }
                }
            }
            else if ((Input.GetKeyDown(KeyCode.Alpha3)) && (GameObject.Find("PlatformListImage3") != null)
                        && (!playerAnim.GetBool("isSpanner")))
            {
                if (GameObject.Find("PlatformListImage3").gameObject.activeSelf == true)
                {
                    m_UIObject.PlatformListSelection(3);
                    if (m_BuildSystem.mode == BuildSystem.CharactorMode.Build3)
                    {
                        m_BuildSystem.mode = BuildSystem.CharactorMode.none;
                    }
                    else
                    {
                        m_BuildSystem.mode = BuildSystem.CharactorMode.Build3;
                    }
                }
            }
            else if ((Input.GetKeyDown(KeyCode.Alpha4)) && (GameObject.Find("PlatformListImage4") != null)
                 && (!playerAnim.GetBool("isSpanner")))
            {
                if (GameObject.Find("PlatformListImage4").gameObject.activeSelf == true)
                {
                    m_UIObject.PlatformListSelection(4);
                    if (m_BuildSystem.mode == BuildSystem.CharactorMode.Build4)
                    {
                        m_BuildSystem.mode = BuildSystem.CharactorMode.none;
                    }
                    else
                    {
                        m_BuildSystem.mode = BuildSystem.CharactorMode.Build4;
                    }
                }
            }
            else if ((Input.GetKeyDown(KeyCode.Alpha5)) && (GameObject.Find("PlatformListImage5") != null)
                && (!playerAnim.GetBool("isSpanner")))
            {
                if (GameObject.Find("PlatformListImage5").gameObject.activeSelf == true)
                {
                    m_UIObject.PlatformListSelection(5);
                    if (m_BuildSystem.mode == BuildSystem.CharactorMode.Build5)
                    {
                        m_BuildSystem.mode = BuildSystem.CharactorMode.none;
                    }
                    else
                    {
                        m_BuildSystem.mode = BuildSystem.CharactorMode.Build5;
                    }
                }
            }
            else if ((Input.GetKeyDown(KeyCode.Alpha6)) && (GameObject.Find("PlatformListImage6") != null)
                && (!playerAnim.GetBool("isSpanner")))
            {
                if (GameObject.Find("PlatformListImage6").gameObject.activeSelf == true)
                {
                    m_UIObject.PlatformListSelection(6);
                    if (m_BuildSystem.mode == BuildSystem.CharactorMode.Build6)
                    {
                        m_BuildSystem.mode = BuildSystem.CharactorMode.none;
                    }
                    else
                    {
                        m_BuildSystem.mode = BuildSystem.CharactorMode.Build6;
                    }
                }
            }
            else if ((Input.GetKeyDown(KeyCode.Alpha7)) && (GameObject.Find("PlatformListImage7") != null)
                && (!playerAnim.GetBool("isSpanner")))
            {
                if (GameObject.Find("PlatformListImage7").gameObject.activeSelf == true)
                {
                    m_UIObject.PlatformListSelection(7);
                    if (m_BuildSystem.mode == BuildSystem.CharactorMode.Build7)
                    {
                        m_BuildSystem.mode = BuildSystem.CharactorMode.none;
                    }
                    else
                    {
                        m_BuildSystem.mode = BuildSystem.CharactorMode.Build7;
                    }
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            m_UIObject.OptionUIOnOff();
        }

        if (mainMeunUIObject.activeSelf == false)
        {
            if ((Input.GetKeyDown(KeyCode.Q)) && (!playerAnim.GetBool("isSpanner")))
            {
                m_BuildSystem.SetDestroyMode();
            }
            else if (Input.GetKeyDown(KeyCode.R))
            {
                switch(m_BuildSystem.mode)
                {
                    case BuildSystem.CharactorMode.Build2:
                        m_BuildSystem.isUpPlatform = !m_BuildSystem.isUpPlatform;
                        break;
                        case BuildSystem.CharactorMode.Build4:
                        m_BuildSystem.isLeftConveyorBelt = !m_BuildSystem.isLeftConveyorBelt;
                        break;
                    case BuildSystem.CharactorMode.Build5:
                        m_BuildSystem.isUpConveyorBelt = !m_BuildSystem.isUpConveyorBelt;
                        break;

                }
            }
        }
    }
}

