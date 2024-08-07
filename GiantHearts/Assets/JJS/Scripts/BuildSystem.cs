using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

struct ObjectandState
{
    public GameObject gameObject;          // �ӽ� ������Ʈ ����.
    public PlatformState platformstate;    // �÷����� ���¸� ����� �� ����
}

public class BuildSystem : MonoBehaviour
{
    public enum CharactorMode //1���� ����, 0�̸� ������ �ƴѰɷ�
    {
        none,
        Build1, // �����÷���
        Build2, // ���Ʒ��÷���
        Build3, // ��������
        Build4, // ���Ʒ�����
        Build5, // ������
        Build6, // �й��
        Build7, // ????
        Destroy // �ı����
    }
    public CharactorMode mode;

    public bool isUpPlatform;
    public bool isLeftConveyorBelt;
    public bool isUpConveyorBelt = false;

    public Gridsystem gridsystem;   // �׸��� �ý����� �����ϱ� ����
    public ObjectPoolSystem poolsystem;     // ������Ʈ Ǯ �ý����� �����ϱ� ����
    public GameManager gamemanager;

    /// sign position�� �׽�Ʈ������ �ӽ÷� �־��
    /// ��ġ�� ����� �������� ��� Ȯ���� ����
    public GameObject sign_BuildPosition;

    ObjectandState TempObject;          // �ӽ� ������Ʈ ����.
    ObjectandState subTempObject;       // �ӽ� ������Ʈ ����2.
    GameObject pickedGridObject;    // ���õ� ��ġ�� �׸��������Ʈ
    GameObject[,,] gridObjects;     // ���� �Ǵܵ��� ���� �׸��� ������Ʈ �迭
    GridObject gridObject;        //  

    public bool debugMod = false;   //�̰� true��� �÷����� ������ üũ���� �ʰ� ��ġ�� �� �ֽ��ϴ�.

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
        pickedGridObject = gridsystem.GetGridObject();                  // �� �ΰ����� ��� �޾ƿ;� �Ѵ�.
        if (pickedGridObject == null)
        {
            poolsystem.GiveBackObject(TempObject.gameObject);
            TempObject.platformstate = null;
            TempObject.gameObject = null;
            TempObject = new ObjectandState();
            mode = CharactorMode.none;
            return;
        }

        gridObject = pickedGridObject.GetComponent<GridObject>();     // �÷��̾� ��ġ�� ������� ��� �ٲ�°Ŷ�

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
    /// public �Լ� ����

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

    public void SetDestroyMode()     // QŰ
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

    public void RotateTempObject() // RŰ?
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
    /// private �Լ�����

    void Buildplatform()     // �����̽�Ű
    {
        if (mode == CharactorMode.none || mode == CharactorMode.Destroy) return;
        else if (mode == CharactorMode.Build1 || mode == CharactorMode.Build2)
        {
            if (TempObject.platformstate.isInstallable)    // ���� �������, ��ġ �������� Ȯ��
            {
                gridObject.ItemType = type;                          // tag, Itemtype, platformŸ���� �� ȥ���Ǿ� �ִµ� ���Ϲ�� �ʿ�
                gridObject.platfrom = TempObject.gameObject;         // ���� ������Ʈ�� �׸��� ������Ʈ�� �÷������� �ѱ��

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
            // ��������� ����ġ ���

            Vector3 output = gridObject.platfrom.GetComponent<LogisticsTransfer>().inDirection;
            Vector3 gridpos = new Vector3(gridObject.GetxIndex(), gridObject.GetyIndex(), gridObject.GetzIndex());

            output += gridpos;

            GridObject outposConveyor = gridObjects[(int)output.x, (int)output.y, (int)output.z].GetComponent<GridObject>();
            if (outposConveyor == null) return;
            outposConveyor.platfrom = gridObject.platfrom;
            outposConveyor.ItemType = GridObject.ObjectType.ItemClassifier_out;
            // ��������� ������ġ ���

            LinkToConveyorBelt(outposConveyor);
        }
    }

    void Destroyplatform()   // �����̽� Ű
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

                //poolsystem.GiveBackObject(nextobj.platfrom);       // �׸��� �÷����� ������Ʈ�� �����ְ�
                nextobj.platfrom = null;                           // ���� ����ش�.
                nextobj.ItemType = GridObject.ObjectType.none;     // �̳�Ÿ�Ե� ���� ����ְ�
            }

            else if (gridObject.ItemType == GridObject.ObjectType.ItemClassifier_out)
            {
                Vector3 gridpos = new Vector3(gridObject.GetxIndex(), gridObject.GetyIndex(), gridObject.GetzIndex());
                gridpos -= gridObject.platfrom.GetComponent<LogisticsTransfer>().inDirection;
                GridObject nextobj = gridObjects[(int)gridpos.x, (int)gridpos.y, (int)gridpos.z].GetComponent<GridObject>();

                //poolsystem.GiveBackObject(nextobj.platfrom);       // �׸��� �÷����� ������Ʈ�� �����ְ�
                nextobj.platfrom = null;                           // ���� ����ش�.
                nextobj.ItemType = GridObject.ObjectType.none;     // �̳�Ÿ�Ե� ���� ����ְ�
            }

            playSound.PlaySE(Manager.Instance.manager_SE.ES_Delete);
            poolsystem.GiveBackObject(gridObject.platfrom);       // �׸��� �÷����� ������Ʈ�� �����ְ�
            gridObject.platfrom.GetComponent<PlatformState>().Sell();

            LogisticsTransfer outputobj = gridObject.platfrom.GetComponent<LogisticsTransfer>();
            if (outputobj != null)
            {
                outputobj = outputobj.GetInPutObject();
                Splitter splitterobj = outputobj.gameObject.GetComponent<Splitter>();
                
                if (splitterobj != null) splitterobj.RightSideObjectMakeNull();
                if (outputobj != null ) outputobj.OutPutObjectMakeNull();
            }

            gridObject.platfrom = null;                           // ���� ����ش�.
            gridObject.ItemType = GridObject.ObjectType.none;     // �̳�Ÿ�Ե� ���� ����ְ�
        }
    }

    bool CheckBuilded() //�̹� �Ǽ��Ǿ� �ִ��� Ȯ�� �ϴ� �Լ�
    {
        //���� �ۼ�
        return false;
    }

    bool CheckInstallable() // ��ġ �������� Ȯ�� �ϴ� �Լ�
    {
        if (gamemanager.ingredient.ironCount - (int)TempObject.platformstate.GetItemvalue().x < 0) return false;
        if (gamemanager.ingredient.copperCount - (int)TempObject.platformstate.GetItemvalue().y < 0) return false;
        if (gamemanager.ingredient.coalCount - (int)TempObject.platformstate.GetItemvalue().z < 0) return false;

        if (gridObject.ItemType == GridObject.ObjectType.Wall) return false;        // �ش� ��ġ�� ���� �ִ°�?
        else if (gridObject.ItemType != GridObject.ObjectType.none) return false;   // ���ٸ� �ٸ� ���𰡰� �ֳ�?

        if (mode == CharactorMode.Build1 && gridObject.GetyIndex() == 0) return false;
        else if (mode == CharactorMode.Build3 || mode == CharactorMode.Build4 || mode == CharactorMode.Build5 || mode == CharactorMode.Build6)
        //������ ����� �����̾Ʈ�ΰ�?
        {
            // ����üũ�κ�. ���õ� ������Ʈ�� y-1 �����¿�,�����¿�,y+1 �����¿츦 ��� Ȯ���غ���
            // �ش� ������ output �Ǵ� ������Ʈ�� �ϳ��ۿ� ���ٸ� true�� ��ȯ.
            tempLogistic = null;
            subtempLogistic = null;

            Vector3 tempvec = Vector3.zero;

            //y �� 0
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

            //���������� ���Ҵµ� TempLogistic�� ���ٸ�? �����̾� ��Ʈ ��ü�� ���°Ŵ�.
            if (tempLogistic == null) return false;
            else if (mode != CharactorMode.Build6) return true;

            //--------------------�����̾� ��Ʈ �� �߰� ���� Ž����------------------------------
            if (mode == CharactorMode.Build6) // ���� �Ǽ��Ϸ��°� ���ø��Ͷ��
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
            newobject.gameObject = poolsystem.TakeOutObject(Tag); // ���� ���õ� ������Ʈ�� ���� ������Ʈ�� ���
            newobject.platformstate = newobject.gameObject.GetComponent<PlatformState>();
        }

        if (TempObject.gameObject.tag != Tag)
        {
            poolsystem.GiveBackObject(TempObject.gameObject);
            TempObject.gameObject = null;

            newobject.gameObject = poolsystem.TakeOutObject(Tag); // ���� ���õ� ������Ʈ�� ���� ������Ʈ�� ���
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
    /// �ƿ�ǲ�� ��ġ�� �׸��尡 �ִ��� Ȯ���ϰ� �ִٸ� �������ִ� �Լ�.
    /// �Լ��� �ߵ��Ҷ� �׸��������ǿ� �����̾Ʈ �з����� ������Ʈ�� ���õǾ��ٰ� �����ϰ� �ۼ�
    /// </summary>
    /// <param name="gridObject">�Ǽ��� �׸��� ������Ʈ�� ��ġ</param>
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
