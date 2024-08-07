using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CheckVisuable : MonoBehaviour
{
    [SerializeField]
    private static Vector3 center = new(0.0f, 7.5f, 0.0f);

    private float fadeSpeed = 10.0f;

    private MeshRenderer newrenderer;
    private List<Material> materials;

    void Start()
    {
        materials = new List<Material>();

        newrenderer = gameObject.GetComponent<MeshRenderer>();

        for (int i = 0; i < newrenderer.materials.Length; ++i)
        {
            materials.Add(newrenderer.materials[i]);
        }
    }

    void Update()
    {
        Camera camera = Camera.main;
        Vector3 cameraLook = center - camera.transform.position;
        

        Vector3 objectOffset = transform.position - center;
        float x = Mathf.Abs(objectOffset.x);
        float y = Mathf.Abs(objectOffset.y);
        float z = Mathf.Abs(objectOffset.z);
        if (x > y && x > z)
        {
            objectOffset.y = 0;
            objectOffset.z = 0;
        }
        else if (y > z && y > x)
        {
            objectOffset.x = 0;
            objectOffset.z = 0;
        }
        else if (z > x && z > y)
        {
            objectOffset.y = 0;
            objectOffset.x = 0;
        }
        else
        {
            // �̻��� �� �������� �����.
            // �������� ����� �ڵ尡 ������
            newrenderer.material.SetFloat("_AlphaValue", 0.0f);
            return;
        }

        objectOffset.Normalize();
        float changeSign = 1.0f;
        float rad = Mathf.Deg2Rad * 45.0f;
        if (Vector3.Dot(cameraLook, objectOffset) < -Mathf.Cos(rad))
        {
            changeSign = -1.0f;
        }

        float deltaAlpha = changeSign * Time.deltaTime * fadeSpeed;

        //���⼭ ���İ� ���ؼ� �Ʒ��� deltaAlpha�� �־��ָ� ��.

        float alphaValue = newrenderer.material.GetFloat("_AlphaValue");
        alphaValue += deltaAlpha;
        alphaValue = Mathf.Clamp(alphaValue, 0.0f, 1.0f);
        //newrenderer.material.SetFloat("_AlphaValue", alphaValue);

        foreach(var i  in materials)
        {
            i.SetFloat("_AlphaValue", alphaValue);
        }
    }
}
