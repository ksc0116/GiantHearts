using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class Gridsystem : MonoBehaviour
{
    [SerializeField]
    private GameObject gridParent;
    [SerializeField]
    private GameObject wall;

    public int gridXAxis;
    public int gridYAxis;
    public int gridZAxis;

    public float gridsize;

    public float hieghtPoint;

    public GameObject gridDot;
    public GameObject player_position;
    

    GameObject[,,] gridObjects;

    //플레이어의 위치에 해당하는 그리드 오브젝트
    GameObject frontGridObject;

    int buildojbectID;

    void Awake()
    {
        gridObjects = new GameObject[gridXAxis, gridYAxis, gridZAxis];

        if (gridObjects == null)
        {
            Debug.Log("null");
        }


        for (int i = 0; i < gridXAxis; i++)
        {
            for (int j = 0; j < gridYAxis; j++)
            {
                for (int l = 0; l < gridZAxis; l++)
                {
                    gridObjects[i, j, l] = Instantiate(gridDot);
                    gridObjects[i, j, l].name = "grid x:" + i + "_y:" + j + "_z:" + l;

                    gridObjects[i, j, l].transform.SetParent(gridParent.transform);

                    float xpos = GetXPos(i);
                    float ypos = GetYPos(j);
                    float zpos = GetZPos(l);

                    gridObjects[i, j, l].transform.position = new Vector3(xpos * gridsize, (ypos * gridsize) + hieghtPoint, zpos * gridsize);

                    gridObjects[i, j, l].GetComponent<GridObject>().SetxIndex(i);
                    gridObjects[i, j, l].GetComponent<GridObject>().SetyIndex(j);
                    gridObjects[i, j, l].GetComponent<GridObject>().SetzIndex(l);

                    if (i == 0 || i == 8)
                    {
                        if (gridObjects[i, j, l].GetComponent<GridObject>().ItemType == GridObject.ObjectType.none)
                        {
                            //gridObjects[i, j, l].GetComponent<GridObject>().platfrom = Instantiate(wall);
                            //gridObjects[i, j, l].GetComponent<GridObject>().platfrom.transform.position = gridObjects[i, j, l].transform.position;
                            //gridObjects[i, j, l].GetComponent<GridObject>().platfrom.transform.SetParent(gridObjects[i, j, l].transform);
                            gridObjects[i, j, l].GetComponent<GridObject>().ItemType = GridObject.ObjectType.Wall;
                        }
                    }

                    if (l == 0 || l == 8)
                    {
                        if (gridObjects[i, j, l].GetComponent<GridObject>().ItemType == GridObject.ObjectType.none)
                        {
//                             gridObjects[i, j, l].GetComponent<GridObject>().platfrom = Instantiate(wall);
//                             gridObjects[i, j, l].GetComponent<GridObject>().platfrom.transform.position = gridObjects[i, j, l].transform.position;
//                             gridObjects[i, j, l].GetComponent<GridObject>().platfrom.transform.SetParent(gridObjects[i, j, l].transform);
                            gridObjects[i, j, l].GetComponent<GridObject>().ItemType = GridObject.ObjectType.Wall;
                        }
                    }

                }
            }
        }
    }

    /// <summary>
    /// 빌드시스템 분리
    /// </summary>
    void Update()
    {
        frontGridObject = GetGridObject();
        if(frontGridObject == null) return;
    }

    float GetXPos(int num)
    {
        float result = 0;

        if (gridXAxis % 2 != 0)
        {
            int half = gridXAxis / 2;
            half = gridXAxis - half;
            num++;

            if (num > half)
            {
                result = (num - half);
                return result;
            }

            else if (num < half)
            {
                result = (half - num) * -1;
                return result;
            }
        }

        else
        {
            float half = gridXAxis / 2;
            half += 0.5f;
            num++;

            if (num > half)
            {
                result = (num - half);
                return result;
            }

            else if (num < half)
            {
                result = (half - num) * -1;
                return result;
            }
        }

        return 0;
    }
    float GetYPos(int num)
    {
        float result = 0;

        if (gridYAxis % 2 != 0)
        {
            int half = gridYAxis / 2;
            half = gridYAxis - half;
            num++;

            if (num > half)
            {
                result = (num - half);
                return result;
            }

            else if (num < half)
            {
                result = (half - num) * -1;
                return result;
            }
        }

        else
        {
            float half = gridYAxis / 2;
            half += 0.5f;
            num++;

            if (num > half)
            {
                result = (num - half);
                return result;
            }

            else if (num < half)
            {
                result = (half - num) * -1;
                return result;
            }
        }

        return result;
    }
    float GetZPos(int num)
    {
        float result = 0;

        if (gridZAxis % 2 != 0)
        {
            int half = gridZAxis / 2;
            half = gridZAxis - half;
            num++;

            if (num > half)
            {
                result = (num - half);
                return result;
            }

            else if (num < half)
            {
                result = (half - num) * -1;
                return result;
            }
        }

        else
        {
            float half = gridZAxis / 2;
            half += 0.5f;
            num++;

            if (num > half)
            {
                result = (num - half);
                return result;
            }

            else if (num < half)
            {
                result = (half - num) * -1;
                return result;
            }
        }

        return 0;
    }

    GameObject CalculateWorldLocation(Vector3 worldLocation)
    {   //7이고 사이즈 2 일때 최대 크기는 14 플레이어의 위치가 7일때 7 / 플레이어의 위치가 4이면 2 플레이어 위치가 -5이면 0.5

        float pl_xpos = (worldLocation.x) / gridsize;
        float pl_ypos = (worldLocation.y) / gridsize;
        float pl_zpos = (worldLocation.z) / gridsize;

        pl_xpos = CalculateXindex(pl_xpos);
        pl_ypos = CalculateYindex(pl_ypos);
        pl_zpos = CalculateZindex(pl_zpos);

        if (pl_xpos < 0 || pl_xpos > gridXAxis-1 ) return null;
        if(pl_ypos < 0 || pl_ypos > gridYAxis -1) return null;
        if(pl_zpos < 0 || pl_zpos > gridZAxis -1) return null;

        GameObject frontobject = gridObjects[(int)pl_xpos, (int)pl_ypos, (int)pl_zpos ];
        if (frontGridObject != frontobject)
        {
            //Debug.Log(frontobject.name);
        }

        return frontobject;
    }

    int CalculateXindex(float num)
    {
        // NUM은 플레이어의 위치

        if (gridXAxis % 2 == 0)
        {
            float half = gridXAxis / 2;  //x인덱스의 절반 (6 : 3) -2.5 -1.5 -0.5 / 0.5 1.5 2.5
                                         // num이 -1.8 이라면 -2.5 이상 -1.5이하 이지만 -1.5에 가까우므로 index = 1

            if (num > 0)                 // 0보다 크다면 3이상
            {
                int result = (int)(Mathf.Round(half + num)); //3 + 1.8 = 4.8(4) / 3 + 2.8 = 5.8(5) / 3 + 0.3 = 3.3(3)
                return result;
            }

            else if (num < 0)               // 0보다 작다면 3이하
            {
                half--;
                int result = (int)(Mathf.Round(half + num));      // 3 + -1.8 = 1.2(1) / 3 + -2.8 = 0.2(0) / 3 + 0.3 = 2.7(2)
                return result;
            }

            else                                // 짝수인데 정 가운데가 걸렸다는건 조금 이상한 상황
            {
                return (int)half;
            }
        }

        else if (gridXAxis % 2 != 0)
        {
            int half = gridXAxis / 2;   // x인덱스의 절반 (7 : 3)
            half = gridXAxis - half;    // 그 절반에서 x인덱스를 빼면 나온 숫자 7 - 3 = 4
            half--;
            if (num < -0.5)     //플레이어가 0보다 뒤에 있다면, 이건 인덱스가 3보다 작다는 소리
            {               // -3 -2 -1 0 1 2 3
                            // 인덱스의 시작은 1이 아니라 0
                int result = (int)(Mathf.Round(half + num)); // (4-1) + -3 = 0 / 3 + -2 = 1 / 3 + -1 = 2 /
                return result;
            }

            else if (num > -0.5 && num < 0.5)   //플레이어가 0 이라면 그건 인데스가 3 라는것.
            {
                return half;
            }

            else if (num > 0.5)    //플레이어가 0보다 뒤에 있다면, 이건 인덱스가 3보다 크다는 소리
            {                   // 3 + 1 = 4 / 3 + 2 = 5 / 3 + 3 = 6

                int result = (int)(Mathf.Round(half + num));
                return result;
            }
        }

        return 0;
    }

    int CalculateYindex(float num)
    {
        //num--;
        // NUM은 플레이어의 위치



        return (int)num;
    }

    int CalculateZindex(float num)
    {
        // NUM은 플레이어의 위치

        if (gridZAxis % 2 == 0)
        {
            float half = gridZAxis / 2;  //x인덱스의 절반 (6 : 3) -2.5 -1.5 -0.5 / 0.5 1.5 2.5
                                         // num이 -1.8 이라면 -2.5 이상 -1.5이하 이지만 -1.5에 가까우므로 index = 1

            if (num > 0)                 // 0보다 크다면 3이상
            {
                int result = (int)(Mathf.Round(half + num)); //3 + 1.8 = 4.8(4) / 3 + 2.8 = 5.8(5) / 3 + 0.3 = 3.3(3)
                return result;
            }

            else if (num < 0)               // 0보다 작다면 3이하
            {
                int result = (int)(Mathf.Round(half + num));      // 3 + -1.8 = 1.2(1) / 3 + -2.8 = 0.2(0) / 3 + 0.3 = 2.7(2)
                return result;
            }

            else                                // 짝수인데 정 가운데가 걸렸다는건 조금 이상한 상황
            {
                return (int)half;
            }
        }

        else if (gridZAxis % 2 != 0)
        {
            int half = gridZAxis / 2;   //x인덱스의 절반 (7 : 3)
            half = gridZAxis - half;    //그 절반에서 x인덱스를 빼면 나온 숫자 7 - 3 = 4
            half--;

            if (num < -0.5)     //플레이어가 0보다 뒤에 있다면, 이건 인덱스가 3보다 작다는 소리
            {               // -3 -2 -1 0 1 2 3

                int result = (int)(Mathf.Round(half + num)); // (4-1) + -3 = 0 / 3 + -2 = 1 / 3 + -1 = 2 /
                return result;
            }

            else if (num > -0.5 && num < 0.5)   //플레이어가 0 이라면 그건 인데스가 3 라는것.
            {
                return half;
            }

            else if (num > 0.5)    //플레이어가 0보다 뒤에 있다면, 이건 인덱스가 3보다 크다는 소리
            {                   // 3 + 1 = 4 / 3 + 2 = 5 / 3 + 3 = 6
                int result = (int)(Mathf.Round(half + num));
                return result;
            }
        }

        return 0;
    }

    public GameObject[,,] GetGameObjects()
    {
        return gridObjects;
    }

    //다른 클래스에서 그리드 오브젝트를 받아오려면 이 함수를 사용할것
    public GameObject GetGridObject()
    {
        GameObject player = player_position;

        //플레이어의 정면을 구하는 부분
        Vector3 playerLocation = player.transform.position;
        Vector3 playerlook = (player.transform.rotation * (Vector3.back));
        playerlook.y = 0;

        if(Mathf.Abs(playerlook.x)  > Mathf.Abs(playerlook.z))
        {
            //플레이어가 바라보고 있는 방향은 x축
            if(playerlook.x > 0)
            {
                playerLocation += new Vector3(2, 0, 0);
                return CalculateWorldLocation(playerLocation);
            }

            else
            {
                playerLocation += new Vector3(-2, 0, 0);
                return CalculateWorldLocation(playerLocation);
            }
        }

        else
        {
            // 플레이어가 바라보고 있는 방향은 z축
            if (playerlook.z > 0)
            {
                playerLocation += new Vector3(0, 0, 2);
                return CalculateWorldLocation(playerLocation);
            }

            else
            {
                playerLocation += new Vector3(0, 0, -2);
                return CalculateWorldLocation(playerLocation);
            }
        }

//         Vector3 playerforward = player.transform.rotation * (Vector3.back * gridsize);
//         playerforward += player.transform.position;
//         if (playerforward.y > 1)
//         {
//             playerforward.y -= gridsize;
//         }
// 
//         GameObject gridpostion = CalculateWorldLocation(playerforward);
//         return gridpostion;
    }

    public GameObject GetGridObject(Vector3 worldLocation)
    {
        return CalculateWorldLocation(worldLocation);
    }

    public float GetGridsize()
    {
        return 1.0f;
    }

    // 빌드모드 Object ID 설정
    public void SetBuildObjectId(int index)
    {
        buildojbectID = index;
    }
}
