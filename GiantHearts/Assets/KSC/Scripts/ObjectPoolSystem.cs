using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObjectPoolSystem : MonoBehaviour
{
    [SerializeField]
    private GameObject[] prefabs;
    [SerializeField]
    private int createCount;
    [SerializeField]
    private Transform parentTransform;

    Dictionary<string, Stack<GameObject>> pools;

    public void AllResourceDissolve()
    {
       
        var values = pools.Values.ToList();

        for(int i = 0; i < pools.Count; i++)
        {
            Stack<GameObject> value = values[i];

            for(int j = 0;j<value.Count;j++)
            {
                GameObject temp = value.Pop();
                if (temp.layer == 8)
                {
                    if(temp.activeSelf)
                    {
                        Debug.Log("광물 디졸브");
                        temp.GetComponent<Mineral_Dissolve>().Dissolve();
                        continue;
                    }
                }
                value.Push(temp);
            }
        }
    }

    void Start()
    {
        pools = new Dictionary<string, Stack<GameObject>>();
        CreatePrefabs();
    }

    public void GiveBackObject(GameObject giveBackObject)
    {
        if(giveBackObject == null)return;
        if(!pools.ContainsKey(giveBackObject.tag))
        {
            Debug.Log("오브젝트풀에 다음과 같은 Tag가 없어 돌려줄 수 없습니다.");
            return;
        }

        giveBackObject.SetActive(false);

        pools[giveBackObject.tag].Push(giveBackObject);
    }

    public GameObject TakeOutObject(string objectTag)
    {
        if(!pools.ContainsKey(objectTag))
        {
            Debug.Log("오브젝트풀에 등록되지 않은 Tag입니다.");
            return new GameObject();
        }

        if(pools[objectTag].Count != 0)
        {
            return pools[objectTag].Pop();
        }

        for(int i = 0; i< prefabs.Length; i++)
        {
            if(objectTag == prefabs[i].tag)
            {
                GameObject temp = Instantiate(prefabs[i]);
                temp.transform.parent = parentTransform;
                pools[objectTag].Push(temp);
            }
        }

        return pools[objectTag].Pop();
    }

    private void CreatePrefabs()
    {
        for (int i = 0; i < prefabs.Length; i++)
        {
            for (int j = 0; j < createCount; j++)
            {
                GameObject temp = Instantiate(prefabs[i]);
                temp.transform.parent = parentTransform;
                temp.transform.position = new Vector3(100, 100, 100);

                // 나중에 문제 생기면 어케든 고치기
                // temp.SetActive(false);
                // =============================

                if (pools.ContainsKey(temp.tag))
                {
                    pools[temp.tag].Push(temp);
                }
                else
                {
                    pools.Add(temp.tag, new Stack<GameObject>());
                    pools[temp.tag].Push(temp);
                }
            }
        }
    }
}
