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

    //�÷��̾��� ��ġ�� �ش��ϴ� �׸��� ������Ʈ
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
    /// ����ý��� �и�
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
    {   //7�̰� ������ 2 �϶� �ִ� ũ��� 14 �÷��̾��� ��ġ�� 7�϶� 7 / �÷��̾��� ��ġ�� 4�̸� 2 �÷��̾� ��ġ�� -5�̸� 0.5

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
        // NUM�� �÷��̾��� ��ġ

        if (gridXAxis % 2 == 0)
        {
            float half = gridXAxis / 2;  //x�ε����� ���� (6 : 3) -2.5 -1.5 -0.5 / 0.5 1.5 2.5
                                         // num�� -1.8 �̶�� -2.5 �̻� -1.5���� ������ -1.5�� �����Ƿ� index = 1

            if (num > 0)                 // 0���� ũ�ٸ� 3�̻�
            {
                int result = (int)(Mathf.Round(half + num)); //3 + 1.8 = 4.8(4) / 3 + 2.8 = 5.8(5) / 3 + 0.3 = 3.3(3)
                return result;
            }

            else if (num < 0)               // 0���� �۴ٸ� 3����
            {
                half--;
                int result = (int)(Mathf.Round(half + num));      // 3 + -1.8 = 1.2(1) / 3 + -2.8 = 0.2(0) / 3 + 0.3 = 2.7(2)
                return result;
            }

            else                                // ¦���ε� �� ����� �ɷȴٴ°� ���� �̻��� ��Ȳ
            {
                return (int)half;
            }
        }

        else if (gridXAxis % 2 != 0)
        {
            int half = gridXAxis / 2;   // x�ε����� ���� (7 : 3)
            half = gridXAxis - half;    // �� ���ݿ��� x�ε����� ���� ���� ���� 7 - 3 = 4
            half--;
            if (num < -0.5)     //�÷��̾ 0���� �ڿ� �ִٸ�, �̰� �ε����� 3���� �۴ٴ� �Ҹ�
            {               // -3 -2 -1 0 1 2 3
                            // �ε����� ������ 1�� �ƴ϶� 0
                int result = (int)(Mathf.Round(half + num)); // (4-1) + -3 = 0 / 3 + -2 = 1 / 3 + -1 = 2 /
                return result;
            }

            else if (num > -0.5 && num < 0.5)   //�÷��̾ 0 �̶�� �װ� �ε����� 3 ��°�.
            {
                return half;
            }

            else if (num > 0.5)    //�÷��̾ 0���� �ڿ� �ִٸ�, �̰� �ε����� 3���� ũ�ٴ� �Ҹ�
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
        // NUM�� �÷��̾��� ��ġ



        return (int)num;
    }

    int CalculateZindex(float num)
    {
        // NUM�� �÷��̾��� ��ġ

        if (gridZAxis % 2 == 0)
        {
            float half = gridZAxis / 2;  //x�ε����� ���� (6 : 3) -2.5 -1.5 -0.5 / 0.5 1.5 2.5
                                         // num�� -1.8 �̶�� -2.5 �̻� -1.5���� ������ -1.5�� �����Ƿ� index = 1

            if (num > 0)                 // 0���� ũ�ٸ� 3�̻�
            {
                int result = (int)(Mathf.Round(half + num)); //3 + 1.8 = 4.8(4) / 3 + 2.8 = 5.8(5) / 3 + 0.3 = 3.3(3)
                return result;
            }

            else if (num < 0)               // 0���� �۴ٸ� 3����
            {
                int result = (int)(Mathf.Round(half + num));      // 3 + -1.8 = 1.2(1) / 3 + -2.8 = 0.2(0) / 3 + 0.3 = 2.7(2)
                return result;
            }

            else                                // ¦���ε� �� ����� �ɷȴٴ°� ���� �̻��� ��Ȳ
            {
                return (int)half;
            }
        }

        else if (gridZAxis % 2 != 0)
        {
            int half = gridZAxis / 2;   //x�ε����� ���� (7 : 3)
            half = gridZAxis - half;    //�� ���ݿ��� x�ε����� ���� ���� ���� 7 - 3 = 4
            half--;

            if (num < -0.5)     //�÷��̾ 0���� �ڿ� �ִٸ�, �̰� �ε����� 3���� �۴ٴ� �Ҹ�
            {               // -3 -2 -1 0 1 2 3

                int result = (int)(Mathf.Round(half + num)); // (4-1) + -3 = 0 / 3 + -2 = 1 / 3 + -1 = 2 /
                return result;
            }

            else if (num > -0.5 && num < 0.5)   //�÷��̾ 0 �̶�� �װ� �ε����� 3 ��°�.
            {
                return half;
            }

            else if (num > 0.5)    //�÷��̾ 0���� �ڿ� �ִٸ�, �̰� �ε����� 3���� ũ�ٴ� �Ҹ�
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

    //�ٸ� Ŭ�������� �׸��� ������Ʈ�� �޾ƿ����� �� �Լ��� ����Ұ�
    public GameObject GetGridObject()
    {
        GameObject player = player_position;

        //�÷��̾��� ������ ���ϴ� �κ�
        Vector3 playerLocation = player.transform.position;
        Vector3 playerlook = (player.transform.rotation * (Vector3.back));
        playerlook.y = 0;

        if(Mathf.Abs(playerlook.x)  > Mathf.Abs(playerlook.z))
        {
            //�÷��̾ �ٶ󺸰� �ִ� ������ x��
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
            // �÷��̾ �ٶ󺸰� �ִ� ������ z��
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

    // ������ Object ID ����
    public void SetBuildObjectId(int index)
    {
        buildojbectID = index;
    }
}
