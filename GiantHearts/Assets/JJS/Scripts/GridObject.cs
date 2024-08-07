using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridObject : MonoBehaviour
{
    public enum ObjectType
    {
        none,                   // 초기화용
        PlayerWay,              // 이동용 길
        PlayerSlopeWay,         // 이동용 길(경사)
        ItemWay,                // 아이템 운송로
        ItemSlopeWay,           // 아이템 운송로(경사)
        ItemTurnWay,            // 아이템 운송로(회전)
        ItemClassifier_in,      // 아이템 분류기(투입)
        ItemClassifier_out,     // 아이템 분류기(방출)
        Wall,                   // 벽    
        WallInputWay,           // 벽출구
        WallOutputWay           // 벽입구

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
