using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetToMove : MonoBehaviour
{
    public Transform targetTransform;
    public float moveSpeed;
    [SerializeField]
    bool isMove = false;

    private void CheckMoveEnd()
    {
        if(transform.position == targetTransform.position)
        {
            Debug.Log("�̵� ��");
            isMove = false;
        }
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CheckMoveEnd();

        if(Input.GetKeyDown(KeyCode.UpArrow))
        {
            isMove = true;
        }

        if (isMove)
        {
            Debug.Log("�̵���");
            transform.position = Vector3.Lerp(transform.position, targetTransform.position, moveSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetTransform.rotation, moveSpeed * Time.deltaTime);
        }
    }
}
