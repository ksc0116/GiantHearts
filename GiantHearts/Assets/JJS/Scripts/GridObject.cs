using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridObject : MonoBehaviour
{
    public enum ObjectType
    {
        none,                   // �ʱ�ȭ��
        PlayerWay,              // �̵��� ��
        PlayerSlopeWay,         // �̵��� ��(���)
        ItemWay,                // ������ ��۷�
        ItemSlopeWay,           // ������ ��۷�(���)
        ItemTurnWay,            // ������ ��۷�(ȸ��)
        ItemClassifier_in,      // ������ �з���(����)
        ItemClassifier_out,     // ������ �з���(����)
        Wall,                   // ��    
        WallInputWay,           // ���ⱸ
        WallOutputWay           // ���Ա�

    }

    int xIndex;    
    int yIndex;    
    int zIndex;

    public GameObject platfrom;
    public ObjectType ItemType = ObjectType.none;

    void Start()
    {
        
    }

    
    void Update()
    {
        
    }

    public void SetxIndex(int i)
    {
        xIndex = i;
    }
    public void SetyIndex(int i)
    {
        yIndex = i;
    }
    public void SetzIndex(int i)
    {
        zIndex = i;
    }

    public int GetxIndex()
    {
        return xIndex;
    }

    public int GetyIndex()
    {
        return yIndex;
    }

    public int GetzIndex()
    {
        return zIndex;
    }
}
