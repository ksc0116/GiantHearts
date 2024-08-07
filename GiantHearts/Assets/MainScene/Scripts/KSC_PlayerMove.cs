using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KSC_PlayerMove : MonoBehaviour
{
    [SerializeField]
    private GameObject mainMenuUI;

    [SerializeField]
    private float moveSpeed;
    private Camera cam;
    private Vector3 playerFront;
    private Vector3 curForward;
    private Animator anim;

    [SerializeField]
    private GameObject spanner;

    [SerializeField]
    private BuildSystem buildSystem;

    private bool isMoveAble = true;

    private PlaySound playSound;


    public void FootSound()
    {
        // 플레이어 걷는 소리
        playSound.PlaySE(Manager.Instance.manager_SE.ES_Footstep);
    }

    void Start()
    {
        playSound = GetComponent<PlaySound>();
        anim = GetComponent<Animator>();
        cam = Camera.main;
    }

    void Update()
    {
        if (mainMenuUI.activeSelf == true)
        {
            return;
        }

        BuildAndDestroyAnim();
        IdleAnim();
        if (isMoveAble)
        {
            Move();
        }
        //SelectPlatform();
    }

    private void IdleAnim()
    {
        if (Input.GetKeyUp(KeyCode.A))
        {
            anim.SetBool("isMove", false);
        }

        if (Input.GetKeyUp(KeyCode.D))
        {
            anim.SetBool("isMove", false);
        }

        if (Input.GetKeyUp(KeyCode.W))
        {
            anim.SetBool("isMove", false);
        }

        if (Input.GetKeyUp(KeyCode.S))
        {
            anim.SetBool("isMove", false);
        }
    }

    private void BuildAndDestroyAnim()
    {
        if ((Input.GetKeyDown(KeyCode.Space))
            && (
                        (buildSystem.mode == BuildSystem.CharactorMode.Destroy)
                        || (buildSystem.mode == BuildSystem.CharactorMode.Build1)
                        || (buildSystem.mode == BuildSystem.CharactorMode.Build2)
                        || (buildSystem.mode == BuildSystem.CharactorMode.Build3)
                        || (buildSystem.mode == BuildSystem.CharactorMode.Build4)
                        || (buildSystem.mode == BuildSystem.CharactorMode.Build5)
                        || (buildSystem.mode == BuildSystem.CharactorMode.Build6)
                        || (buildSystem.mode == BuildSystem.CharactorMode.Build7)
               ))
        {
            isMoveAble = false;
            spanner.SetActive(true);
            anim.SetBool("isSpanner",true);
        }
    }

    private void EndBuildAndDestroyAnim()
    {
        Debug.Log("애니메이션 끝남");
        isMoveAble = true;
        spanner.SetActive(false);
        anim.SetBool("isSpanner", false);
        buildSystem.BuildorDestroy();
    }

    private void Move()
    {
        Vector3 tempDir = new Vector3(0, 0, 0);

        Vector3 tempCamForward = new Vector3(cam.transform.forward.x, 0, cam.transform.forward.z);
        playerFront = Vector3.Normalize(tempCamForward);

        Vector3 right = Vector3.Cross(Vector3.up, playerFront);
        right = Vector3.Normalize(right);

        if (Input.GetKey(KeyCode.A))
        {
            anim.SetBool("isMove", true);
            tempDir -= moveSpeed * Time.deltaTime * right;
        }

        if (Input.GetKey(KeyCode.D))
        {
            anim.SetBool("isMove", true);
            tempDir += moveSpeed * Time.deltaTime * right;
        }

        if (Input.GetKey(KeyCode.W))
        {
            anim.SetBool("isMove", true);
            tempDir += moveSpeed * Time.deltaTime * playerFront;
        }

        if (Input.GetKey(KeyCode.S))
        {
            anim.SetBool("isMove", true);
            tempDir -= moveSpeed * Time.deltaTime * playerFront;
        }

        transform.position += (tempDir);
        curForward = tempDir;

        if (Vector3.zero != tempDir)
        {
            transform.forward = -curForward;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Roof")
        {
            Debug.Log("천장이다");
        }
    }
}
