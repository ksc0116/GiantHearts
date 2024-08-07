using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnCube : MonoBehaviour
{
    public GameObject m_CubeObject;
    public GameObject m_player;
    
    private Vector3 m_playerPos;
    private int m_index;

    // Start is called before the first frame update
    void Start()
    {
        m_playerPos = transform.position;
        m_index = gameObject.name.IndexOf("(Clone)");
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    public void CubeInstalling()
    {
        m_playerPos.x = GameObject.FindWithTag("Player").transform.position.x + 1;
        m_playerPos.y = GameObject.FindWithTag("Player").transform.position.y + 3;
        m_playerPos.z = GameObject.FindWithTag("Player").transform.position.z;
        //gameObject = Instantiate(gameObject, playerPos, Quaternion.identity);

        //if (index > 0)
        //    gameObject.name.Substring(0, index);

        Debug.Log("test");
    }
}
