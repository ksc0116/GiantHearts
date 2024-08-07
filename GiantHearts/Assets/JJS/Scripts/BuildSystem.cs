using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

struct ObjectandState
{
    public GameObject gameObject;          // 임시 오브젝트 공간.
    public PlatformState platformstate;    // 플랫폼의 상태를 기록해 둘 물건
}

public class BuildSystem : MonoBehaviour
{
    public enum CharactorMode //1부터 시작, 0이면 빌드모드 아닌걸로
    {
        none,
        Build1, // 평평플랫폼
        Build2, // 위아래플랫폼
        Build3, // 평평컨베
        Build4, // 위아래컨베
        Build5, // 턴컨베
        Build6, // 분배기
        Build7, // ????
        Destroy // 파괴모드
    }
    public CharactorMode mode;

    public bool isUpPlatform;
    public bool isLeftConveyorBelt;
    public bool isUpConveyorBelt = false;

    public Gridsystem gridsystem;   // 그리드 시스템을 참조하기 위한
    public ObjectPoolSystem poolsystem;     // 오브젝트 풀 시스템을 참조하기 위한
    public GameManager gamemanager;

    /// sign position은 테스트용으로 임시로 넣어둠
    /// 위치가 제대로 잡히는지 등등 확인을 위해
    public GameObject sign_BuildPosition;

    ObjectandState TempObject;          // 임시 오브젝트 공간.
    ObjectandState subTempObject;       // 임시 오브젝트 공간2.
    GameObject pickedGridObject;    // 선택된 위치의 그리드오브젝트
    GameObject[,,] gridObjects;     // 조건 판단등을 위한 그리드 오브젝트 배열
    GridObject gridObject;        //  

    public bool debugMod = false;   //이게 true라면 플랫폼의 가격을 체크하지 않고 설치할 수 있습니다.

    GridObject.ObjectType type;

    LogisticsTransfer tempLogistic;
    LogisticsTransfer subtempLogistic;

    private PlaySound playSound;

    void Start()
    {
        playSound = GetComponent<PlaySound>();
        gridObjects = gridsystem.GetGameObjects();
        if (gridObjects == null)
        {
            Debug.Log("NUll");
        }
    }

    void Update()
    {
        pickedGridObject = gridsystem.GetGridObject();                  // 이 두가지는 계속 받아와야 한다.
        if (pickedGridObject == null)
        {
            poolsystem.GiveBackObject(TempObject.gameObject);
            TempObject.platformstate = null;
            TempObject.gameObject = null;
            TempObject = new ObjectandState();
            mode = CharactorMode.none;
            return;
        }

        gridObject = pickedGridObject.GetComponent<GridObject>();     // 플레이어 위치나 방향따라 계속 바뀌는거라

        //if (gridObject.ItemType != GridObject.ObjectType.none) return;
        if (gridObject.GetxIndex() < 0 || gridObject.GetxIndex() > gridsystem.gridXAxis) return;
        if (gridObject.GetzIndex() < 0 || gridObject.GetzIndex() > gridsystem.gridZAxis) return;

        if (gridObject.GetyIndex() < 0 || gridObject.GetyIndex() > gridsystem.gridYAxis) return;


        switch (mode)
        {
            case CharactorMode.none:
                {
                    if (TempObject.gameObject != null)
                    {
                        poolsystem.GiveBackObject(TempObject.gameObject);
                        TempObject.platformstate = null;
                        TempObject.gameObject = null;
                        TempObject = new ObjectandState();
                    }
                    return;
                }
                break;
            case CharactorMode.Build1:
                {
                    SettingTempObject("Player_Common", ref TempObject);
                    TempObject.platformstate.isBuild = CheckBuilded();
                    TempObject.platformstate.isInstallable = CheckInstallable();
                    sign_BuildPosition.SetActive(false);
                    type = GridObject.ObjectType.PlayerWay;
                }
                break;
            case CharactorMode.Build2:
                {
                    if (isUpPlatform)
                    {
                        SettingTempObject("Player_UP_Platform", ref TempObject);
                    }
                    else
                    {
                        if (gridObject.GetyIndex() - 1 < 0)
                        {
                            isUpPlatform = true;
                            return;
                        }
                        SettingTempObject("Player_UP_Platform", ref TempObject);
                        Vector3 gridpos = new Vector3(gridObject.GetxIndex(), gridObject.GetyIndex(), gridObject.GetzIndex());

                        GameObject inverseObject = gridObjects[(int)gridpos.x, (int)gridpos.y - 1, (int)gridpos.z];
                        pickedGridObject = inverseObject;
                        TempObject.gameObject.transform.position = pickedGridObject.transform.position;
                        gridObject = pickedGridObject.GetComponent<GridObject>();
                    }
                    TempObject.platformstate.isBuild = CheckBuilded();
                    TempObject.platformstate.isInstallable = CheckInstallable();
                    sign_BuildPosition.SetActive(false);
                    type = GridObject.ObjectType.PlayerSlopeWay;

                    Vector3 playerLook = gridsystem.player_position.transform.rotation * Vector3.back;

                    if (Mathf.Abs(playerLook.x) > Mathf.Abs(playerLook.z))
                    {
                        if (playerLook.x > 0)
                        {
                            playerLook = new Vector3(1, 0, 0);
                        }
                        else
                        {
                            playerLook = new Vector3(-1, 0, 0);
                        }
                    }
                    else
                    {
                        if (playerLook.z > 0)
                        {
                            playerLook = new Vector3(0, 0, 1);
                        }
                        else
                        {
                            playerLook = new Vector3(0, 0, -1);
                        }
                    }

                    Quaternion upPlatformOffset = Quaternion.Euler(0, -90, 0);
                    if (!isUpPlatform)
                    {
                        upPlatformOffset = Quaternion.Euler(0, 90, 0);
                    }

                    TempObject.gameObject.transform.rotation = Quaternion.LookRotation(playerLook);
                    TempObject.gameObject.transform.rotation *= upPlatformOffset;
                }
                break;
            case CharactorMode.Build3:
                {
                    SettingTempObject("Conveyor_stright", ref TempObject);
                    TempObject.platformstate.isBuild = CheckBuilded();
                    TempObject.platformstate.isInstallable = CheckInstallable();

                    sign_BuildPosition.SetActive(false);
                    type = GridObject.ObjectType.ItemWay;

                    if (tempLogistic != null)
                        tempLogistic.AlignNextBelt(TempObject.gameObject.GetComponent<LogisticsTransfer>());
                }
                break;
            case CharactorMode.Build4:
                {
                    if (isLeftConveyorBelt)
                    {
                        SettingTempObject("Conveyor_Turn_L", ref TempObject);
                    }
                    else
                    {
                        SettingTempObject("Conveyor_Turn_R", ref TempObject);
                    }
                    TempObject.platformstate.isBuild = CheckBuilded();
                    TempObject.platformstate.isInstallable = CheckInstallable();
                    sign_BuildPosition.SetActive(false);
                    type = GridObject.ObjectType.ItemTurnWay;

                    if (tempLogistic != null)
                        tempLogistic.AlignNextBelt(TempObject.gameObject.GetComponent<LogisticsTransfer>());
                }
                break;
            case CharactorMode.Build5:
                {
                    if (isUpConveyorBelt)
                    {
                        SettingTempObject("Conveyor_UP", ref TempObject);
                    }
                    else
                    {
                        //                         if (gridObject.GetyIndex() - 1 < 0)
                        //                         {
                        //                             isUpPlatform = true;
                        //                             return;
                        //                         }
                        SettingTempObject("Conveyor_Down", ref TempObject);
                    }
                    TempObject.platformstate.isBuild = CheckBuilded();
                    TempObject.platformstate.isInstallable = CheckInstallable();
                    sign_BuildPosition.SetActive(false);
                    type = GridObject.ObjectType.ItemSlopeWay;

                    if (tempLogistic != null)
                        tempLogistic.AlignNextBelt(TempObject.gameObject.GetComponent<LogisticsTransfer>());
                }
                break;
            case CharactorMode.Build6:
                {
                    SettingTempObject("Splitter", ref TempObject);
                    TempObject.platformstate.isBuild = CheckBuilded();
                    TempObject.platformstate.isInstallable = CheckInstallable();

                    sign_BuildPosition.SetActive(false);
                    type = GridObject.ObjectType.ItemWay;

                    if (tempLogistic != null)
                        tempLogistic.AlignNextBelt(TempObject.gameObject.GetComponent<LogisticsTransfer>());

                    sign_BuildPosition.SetActive(false);
                    type = GridObject.ObjectType.ItemClassifier_in;
                }
                break;
            case CharactorMode.Build7:
                {
                    sign_BuildPosition.SetActive(false);
                }
                break;
            case CharactorMode.Destroy:
                {
                    sign_BuildPosition.transform.position = pickedGridObject.transform.position;

                    if (TempObject.gameObject == null) return;
                    TempObject.gameObject.SetActive(false);
                }
                break;
            default:
                break;
        }
    }

    //---------------------------------------
    /// public 함수 모음

    public void BuildorDestroy()
    {
        if (mode == CharactorMode.Destroy)
        {
            if (pickedGridObject.GetComponent<GridObject>().ItemType != GridObject.ObjectType.none && pickedGridObject.GetComponent<GridObject>().ItemType != GridObject.ObjectType.Wall)
            {
                GameObject smoke = poolsystem.TakeOutObject("Particle_Smoke");
                smoke.transform.position = sign_BuildPosition.transform.position + new Vector3(0, -1, 0);
                smoke.SetActive(true);
                Destroyplatform();
            }
        }

        else if (mode != CharactorMode.none)
        {
            if (TempObject.platformstate.isInstallable == true)
            {
                GameObject smoke = poolsystem.TakeOutObject("Particle_Smoke");
                smoke.transform.position = TempObject.gameObject.transform.position + new Vector3(0, -1, 0); ;
                smoke.SetActive(true);
                Buildplatform();
            }
        }
    }

    public void SetDestroyMode()     // Q키
    {
        if (mode != CharactorMode.Destroy)
        {
            mode = CharactorMode.Destroy;
            sign_BuildPosition.SetActive(true);
        }
        else
        {
            mode = CharactorMode.none;
            sign_BuildPosition.SetActive(false);
        }
    }

    public void RotateTempObject() // R키?
    {
        //if (TempObject.gameObject == null) return;

        TempObject.gameObject.transform.Rotate(0, 90, 0);
    }

    public void DeleteAllGrid()
    {
        if (gridObjects == null)
        {
            Debug.Log("null");
        }

        for (int i = 1; i < gridsystem.gridXAxis - 1; i++)
        {
            for (int j = 0; j < gridsystem.gridYAxis; j++)
            {
                for (int l = 1; l < gridsystem.gridZAxis - 1; l++)
                {
                    GridObject gridobj = gridObjects[i, j, l].GetComponent<GridObject>();
                    if (gridobj.platfrom != null)
                    {
                        poolsystem.GiveBackObject(gridobj.platfrom);
                        gridobj.platfrom = null;
                    }
                    gridobj.ItemType = GridObject.ObjectType.none;
                }
            }
        }
    }

    //---------------------------------------------------------------------------
    /// private 함수모음

    void Buildplatform()     // 스페이스키
    {
        if (mode == CharactorMode.none || mode == CharactorMode.Destroy) return;
        else if (mode == CharactorMode.Build1 || mode == CharactorMode.Build2)
        {
            if (TempObject.platformstate.isInstallable)    // 빌드 모드인지, 설치 가능한지 확인
            {
                gridObject.ItemType = type;                          // tag, Itemtype, platform타입이 막 혼제되어 있는데 통일방안 필요
                gridObject.platfrom = TempObject.gameObject;         // 템프 오브젝트를 그리드 오브젝트의 플랫폼으로 넘기고

                if (TempObject.gameObject.GetComponent<NavMeshSourceTag>() != null)
                    TempObject.gameObject.GetComponent<NavMeshSourceTag>().enabled = true;
                else if (TempObject.gameObject.transform.GetChild(0).GetComponent<NavMeshSourceTag>() != null)
                    TempObject.gameObject.transform.GetChild(0).GetComponent<NavMeshSourceTag>().enabled = true;
                else { }

                gamemanager.ingredient.ironCount -= (int)TempObject.platformstate.GetItemvalue().x;
                gamemanager.ingredient.copperCount -= (int)TempObject.platformstate.GetItemvalue().y;
                gamemanager.ingredient.coalCount -= (int)TempObject.platformstate.GetItemvalue().z;
                TempObject.platformstate.isBuild = true;
                TempObject.platformstate = null;
                TempObject.gameObject = null;
                TempObject = new ObjectandState();

            }
        }
        else if (mode == CharactorMode.Build3
    || mode == CharactorMode.Build4
    || mode == CharactorMode.Build5)
        {
            gridObject.ItemType = type;
            gridObject.platfrom = TempObject.gameObject;
            Splitter splitter = tempLogistic.gameObject.GetComponent<Splitter>();
            if (splitter != null && splitter.GetRightSideObject() == null)
            {
                splitter.ConnectNextBeltToRightOut(TempObject.gameObject.GetComponent<LogisticsTransfer>());
            }
            else
            {
                tempLogistic.ConnectNextBelt(TempObject.gameObject.GetComponent<LogisticsTransfer>());
            }
            gamemanager.ingredient.ironCount -= (int)TempObject.platformstate.GetItemvalue().x;
            gamemanager.ingredient.copperCount -= (int)TempObject.platformstate.GetItemvalue().y;
            gamemanager.ingredient.coalCount -= (int)TempObject.platformstate.GetItemvalue().z;
            TempObject.platformstate.isBuild = true;
            TempObject.platformstate = null;
            TempObject.gameObject = null;
            TempObject = new ObjectandState();
            LinkToConveyorBelt(gridObject);
        }
        else if (mode == CharactorMode.Build6)
        {
            gridObject.ItemType = type;
            gridObject.platfrom = TempObject.gameObject;
            Splitter splitter = tempLogistic.gameObject.GetComponent<Splitter>();
            if (splitter != null && splitter.GetRightSideObject() == null)
            {
                splitter.ConnectNextBeltToRightOut(TempObject.gameObject.GetComponent<LogisticsTransfer>());
            }
            else
            {
                tempLogistic.ConnectNextBelt(TempObject.gameObject.GetComponent<LogisticsTransfer>());
            }

            gamemanager.ingredient.ironCount -= (int)TempObject.platformstate.GetItemvalue().x;
            gamemanager.ingredient.copperCount -= (int)TempObject.platformstate.GetItemvalue().y;
            gamemanager.ingredient.coalCount -= (int)TempObject.platformstate.GetItemvalue().z;

            TempObject.platformstate.isBuild = true;
            TempObject.platformstate = null;
            TempObject.gameObject = null;
            TempObject = new ObjectandState();
            // 여기까지가 원위치 등록

            Vector3 output = gridObject.platfrom.GetComponent<LogisticsTransfer>().inDirection;
            Vector3 gridpos = new Vector3(gridObject.GetxIndex(), gridObject.GetyIndex(), gridObject.GetzIndex());

            output += gridpos;

            GridObject outposConveyor = gridObjects[(int)output.x, (int)output.y, (int)output.z].GetComponent<GridObject>();
            if (outposConveyor == null) return;
            outposConveyor.platfrom = gridObject.platfrom;
            outposConveyor.ItemType = GridObject.ObjectType.ItemClassifier_out;
            // 여기까지가 다음위치 등록

            LinkToConveyorBelt(outposConveyor);
        }
    }

    void Destroyplatform()   // 스페이스 키
    {
        if (mode != CharactorMode.Destroy)
        {
            return;
        }

        if (gridObject.ItemType != GridObject.ObjectType.none && gridObject.ItemType != GridObject.ObjectType.Wall)
        {
            if (gridObject.ItemType == GridObject.ObjectType.ItemClassifier_in)
            {
                Vector3 gridpos = new Vector3(gridObject.GetxIndex(), gridObject.GetyIndex(), gridObject.GetzIndex());
                gridpos += gridObject.platfrom.GetComponent<LogisticsTransfer>().inDirection;
                GridObject nextobj = gridObjects[(int)gridpos.x, (int)gridpos.y, (int)gridpos.z].GetComponent<GridObject>();

                //poolsystem.GiveBackObject(nextobj.platfrom);       // 그리드 플랫폼의 오브젝트를 돌려주고
                nextobj.platfrom = null;                           // 안을 비워준다.
                nextobj.ItemType = GridObject.ObjectType.none;     // 이넘타입도 같이 비워주고
            }

            else if (gridObject.ItemType == GridObject.ObjectType.ItemClassifier_out)
            {
                Vector3 gridpos = new Vector3(gridObject.GetxIndex(), gridObject.GetyIndex(), gridObject.GetzIndex());
                gridpos -= gridObject.platfrom.GetComponent<LogisticsTransfer>().inDirection;
                GridObject nextobj = gridObjects[(int)gridpos.x, (int)gridpos.y, (int)gridpos.z].GetComponent<GridObject>();

                //poolsystem.GiveBackObject(nextobj.platfrom);       // 그리드 플랫폼의 오브젝트를 돌려주고
                nextobj.platfrom = null;                           // 안을 비워준다.
                nextobj.ItemType = GridObject.ObjectType.none;     // 이넘타입도 같이 비워주고
            }

            playSound.PlaySE(Manager.Instance.manager_SE.ES_Delete);
            poolsystem.GiveBackObject(gridObject.platfrom);       // 그리드 플랫폼의 오브젝트를 돌려주고
            gridObject.platfrom.GetComponent<PlatformState>().Sell();

            LogisticsTransfer outputobj = gridObject.platfrom.GetComponent<LogisticsTransfer>();
            if (outputobj != null)
            {
                outputobj = outputobj.GetInPutObject();
                Splitter splitterobj = outputobj.gameObject.GetComponent<Splitter>();
                
                if (splitterobj != null) splitterobj.RightSideObjectMakeNull();
                if (outputobj != null ) outputobj.OutPutObjectMakeNull();
            }

            gridObject.platfrom = null;                           // 안을 비워준다.
            gridObject.ItemType = GridObject.ObjectType.none;     // 이넘타입도 같이 비워주고
        }
    }

    bool CheckBuilded() //이미 건설되어 있는지 확인 하는 함수
    {
        //내용 작성
        return false;
    }

    bool CheckInstallable() // 설치 가능한지 확인 하는 함수
    {
        if (gamemanager.ingredient.ironCount - (int)TempObject.platformstate.GetItemvalue().x < 0) return false;
        if (gamemanager.ingredient.copperCount - (int)TempObject.platformstate.GetItemvalue().y < 0) return false;
        if (gamemanager.ingredient.coalCount - (int)TempObject.platformstate.GetItemvalue().z < 0) return false;

        if (gridObject.ItemType == GridObject.ObjectType.Wall) return false;        // 해당 위치에 벽이 있는가?
        else if (gridObject.ItemType != GridObject.ObjectType.none) return false;   // 없다면 다른 무언가가 있나?

        if (mode == CharactorMode.Build1 && gridObject.GetyIndex() == 0) return false;
        else if (mode == CharactorMode.Build3 || mode == CharactorMode.Build4 || mode == CharactorMode.Build5 || mode == CharactorMode.Build6)
        //선택한 블록이 컨베이어벨트인가?
        {
            // 공통체크부분. 선택된 오브젝트의 y-1 전후좌우,전후좌우,y+1 전후좌우를 모두 확인해보고
            // 해당 블럭으로 output 되는 오브젝트가 하나밖에 없다면 true를 반환.
            tempLogistic = null;
            subtempLogistic = null;

            Vector3 tempvec = Vector3.zero;

            //y ± 0
            CheckConvayoroutput(tempvec + Vector3.forward);
            CheckConvayoroutput(tempvec + Vector3.back);
            if (tempLogistic != null && subtempLogistic != null) return false;
            CheckConvayoroutput(tempvec + Vector3.left);
            if (tempLogistic != null && subtempLogistic != null) return false;
            CheckConvayoroutput(tempvec + Vector3.right);
            if (tempLogistic != null && subtempLogistic != null) return false;

            //y - 1
            tempvec.y = 1;
            CheckConvayoroutput(tempvec + Vector3.forward);
            if (tempLogistic != null && subtempLogistic != null) return false;
            CheckConvayoroutput(tempvec + Vector3.back);
            if (tempLogistic != null && subtempLogistic != null) return false;
            CheckConvayoroutput(tempvec + Vector3.left);
            if (tempLogistic != null && subtempLogistic != null) return false;
            CheckConvayoroutput(tempvec + Vector3.right);
            if (tempLogistic != null && subtempLogistic != null) return false;

            // y + 1
            tempvec.y = -1;
            CheckConvayoroutput(tempvec + Vector3.forward);
            if (tempLogistic != null && subtempLogistic != null) return false;
            CheckConvayoroutput(tempvec + Vector3.back);
            if (tempLogistic != null && subtempLogistic != null) return false;
            CheckConvayoroutput(tempvec + Vector3.left);
            if (tempLogistic != null && subtempLogistic != null) return false;
            CheckConvayoroutput(tempvec + Vector3.right);
            if (tempLogistic != null && subtempLogistic != null) return false;

            //마지막까지 돌았는데 TempLogistic이 없다면? 컨베이어 벨트 자체가 없는거다.
            if (tempLogistic == null) return false;
            else if (mode != CharactorMode.Build6) return true;

            //--------------------컨베이어 벨트 별 추가 조건 탐색기------------------------------
            if (mode == CharactorMode.Build6) // 만약 건설하려는게 스플리터라면
            {
                Splitter tempsplitter = TempObject.gameObject.GetComponent<Splitter>();
                if (tempsplitter == null) return false;

                Vector3 gridpos = new Vector3(gridObject.GetxIndex(), gridObject.GetyIndex() + 1, gridObject.GetzIndex());
                gridpos += tempsplitter.inDirection;

                GridObject nextgridObject = gridObjects[(int)gridpos.x, (int)gridpos.y, (int)gridpos.z].GetComponent<GridObject>();
                if (nextgridObject == null) return false;

                if (nextgridObject.ItemType == GridObject.ObjectType.none) return true;
            }
            //---------------------------------------------------
        }

        else
        {
            return true;
        }
        return false;
    }

    void SettingTempObject(string Tag, ref ObjectandState newobject)
    {

        if (newobject.gameObject == null)
        {
            newobject.gameObject = poolsystem.TakeOutObject(Tag); // 지금 선택된 오브젝트를 템프 오브젝트로 등록
            newobject.platformstate = newobject.gameObject.GetComponent<PlatformState>();
        }

        if (TempObject.gameObject.tag != Tag)
        {
            poolsystem.GiveBackObject(TempObject.gameObject);
            TempObject.gameObject = null;

            newobject.gameObject = poolsystem.TakeOutObject(Tag); // 지금 선택된 오브젝트를 템프 오브젝트로 등록
            newobject.platformstate = newobject.gameObject.GetComponent<PlatformState>();
        }

        newobject.gameObject.SetActive(true);
        newobject.gameObject.transform.position = pickedGridObject.transform.position;
    }

    bool CheckConvayoroutput(Vector3 addsize)
    {
        Vector3 gridpos = new Vector3(0, 0, 0);
        if (mode == CharactorMode.Build5 && !isUpConveyorBelt)
            gridpos = new Vector3(gridObject.GetxIndex(), gridObject.GetyIndex() + 1, gridObject.GetzIndex());
        else
            gridpos = new Vector3(gridObject.GetxIndex(), gridObject.GetyIndex(), gridObject.GetzIndex());

        gridpos += addsize;

        if (gridpos.x < 0 || gridpos.x > gridsystem.gridXAxis - 1) return false;
        if (gridpos.y < 0 || gridpos.y > gridsystem.gridYAxis - 1) return false;
        if (gridpos.z < 0 || gridpos.z > gridsystem.gridZAxis - 1) return false;

        GridObject inverseObject = gridObjects[(int)gridpos.x, (int)gridpos.y, (int)gridpos.z].GetComponent<GridObject>();

        if (inverseObject.ItemType == GridObject.ObjectType.none
            || inverseObject.ItemType == GridObject.ObjectType.none) return false;
        if (inverseObject.platfrom == null) return false;

        LogisticsTransfer lgstTrnf = inverseObject.platfrom.GetComponent<LogisticsTransfer>();
        if (lgstTrnf != null)
        {
            Splitter sp_tempLogistic = inverseObject.platfrom.gameObject.GetComponent<Splitter>();
            if (sp_tempLogistic != null && sp_tempLogistic.GetRightSideObject() == null)
            {
                addsize = sp_tempLogistic.rightOutDirection;
            }
            else
            {
                addsize = lgstTrnf.outDirection;
            }

            Vector3 newgridpos = new Vector3(inverseObject.GetxIndex(), inverseObject.GetyIndex(), inverseObject.GetzIndex());
            newgridpos += addsize;
            if (newgridpos.x < 0 || newgridpos.x > gridsystem.gridXAxis - 1) return false;
            if (newgridpos.y < 0 || newgridpos.y > gridsystem.gridYAxis - 1) return false;
            if (newgridpos.z < 0 || newgridpos.z > gridsystem.gridZAxis - 1) return false;


            if ((isUpConveyorBelt || mode != CharactorMode.Build5) && gridObject == gridObjects[(int)newgridpos.x, (int)newgridpos.y, (int)newgridpos.z].GetComponent<GridObject>())
            {
                if (tempLogistic == null)
                    tempLogistic = lgstTrnf;
                else
                    subtempLogistic = lgstTrnf;
                return true;
            }

            if (newgridpos.y - 1 < 0) return false;

            else if ((!isUpConveyorBelt && mode == CharactorMode.Build5)
                && gridObject == gridObjects[(int)newgridpos.x, (int)newgridpos.y - 1, (int)newgridpos.z].GetComponent<GridObject>())
            {

                if (tempLogistic == null)
                    tempLogistic = lgstTrnf;
                else
                    subtempLogistic = lgstTrnf;
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// 아웃풋의 위치에 그리드가 있는지 확인하고 있다면 연결해주는 함수.
    /// 함수가 발동할때 그리드포지션에 컨베이어벨트 분류군의 오브젝트가 선택되었다고 전재하고 작성
    /// </summary>
    /// <param name="gridObject">건설된 그리드 오브젝트의 위치</param>
    void LinkToConveyorBelt(GridObject gridObject)
    {
        if (gridObject.platfrom == null || gridObject.platfrom.GetComponent<LogisticsTransfer>() == null) return;
        Vector3 output = gridObject.platfrom.GetComponent<LogisticsTransfer>().outDirection;
        Vector3 gridpos = new Vector3(gridObject.GetxIndex(), gridObject.GetyIndex(), gridObject.GetzIndex());

        output += gridpos;

        GridObject outposConveyor = gridObjects[(int)output.x, (int)output.y, (int)output.z].GetComponent<GridObject>();

        if (outposConveyor == null || outposConveyor.platfrom == null) return;
        LogisticsTransfer lgstTrnf = outposConveyor.platfrom.GetComponent<LogisticsTransfer>();
        if (lgstTrnf == null) return;
        gridObject.platfrom.GetComponent<LogisticsTransfer>().ConnectNextBelt(lgstTrnf);
    }

}
