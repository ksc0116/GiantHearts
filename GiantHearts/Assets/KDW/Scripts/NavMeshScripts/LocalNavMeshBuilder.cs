using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LocalNavMeshBuilder : MonoBehaviour
{

    // The center of the build
    public Transform m_Tracked;

    // The size of the build bounds
    public Vector3 m_Size = new Vector3(80.0f, 20.0f, 80.0f);

    NavMeshData m_NavMesh;
    AsyncOperation m_Operation;
    NavMeshDataInstance m_Instance;

    /// �׺� �޽� ���忡 ���� Source
    List<NavMeshBuildSource> m_Sources = new List<NavMeshBuildSource>();
    List<NavMeshBuildSource> m_Sources1 = new List<NavMeshBuildSource>();

    NavMeshSourceTag m_navMeshSourceTag;

    /// <summary>
    /// ���۰� �Բ� �񵿱�� �׺�޽ø� ��� ������Ʈ���ش�.
    /// </summary>
    /// <returns></returns>
    IEnumerator Start()
    {
        while (true)
        {
            UpdateNavMesh(true);
            yield return m_Operation;
        }
    }

    void OnEnable()
    {
        // Construct and add navmesh
        m_NavMesh = new NavMeshData();
        m_Instance = NavMesh.AddNavMeshData(m_NavMesh);
        if (m_Tracked == null)
            m_Tracked = transform;
        UpdateNavMesh(false);
    }

    void OnDisable()
    {
        // Unload navmesh and clear handle
        m_Instance.Remove();
    }

    /// <summary>
    /// �׺� �޽ø� ����/�񵿱�� ������Ʈ�Ѵ�.
    /// </summary>
    /// <param name="asyncUpdate">�񵿱� ������Ʈ�� ����� �� ����</param>
    void UpdateNavMesh(bool asyncUpdate = false)
    {
        NavMeshSourceTag.Collect(ref m_Sources);
        NavMeshSourceBuildModeTag.Collect(ref m_Sources1);

        var defaultBuildSettings = NavMesh.GetSettingsByID(0);
        var bounds = QuantizedBounds();

        if (asyncUpdate)
        {
            // �񵿱�� �׺�޽ø� ������Ʈ�Ѵ�.
            m_Operation = NavMeshBuilder.UpdateNavMeshDataAsync(m_NavMesh, defaultBuildSettings, m_Sources, bounds);
            //Debug.Log("�񵿱� ");
            //m_Operation = NavMeshBuilder.UpdateNavMeshDataAsync(m_NavMesh, defaultBuildSettings, m_Sources1, bounds);
        }
        else
        {
            // �ش� �����ӿ� ��� �׺�޽ø� ������Ʈ�Ѵ�.
            NavMeshBuilder.UpdateNavMeshData(m_NavMesh, defaultBuildSettings, m_Sources, bounds);
            //Debug.Log("���� ");
            //NavMeshBuilder.UpdateNavMeshData(m_NavMesh, defaultBuildSettings, m_Sources1, bounds);
        }
    }

    /// <summary>
    /// �ٿ�带 ���ϱ� ���� �Լ�
    /// </summary>
    /// <param name="v"></param>
    /// <param name="quant"></param>
    /// <returns></returns>
    static Vector3 Quantize(Vector3 v, Vector3 quant)
    {
        float x = quant.x * Mathf.Floor(v.x / quant.x);
        float y = quant.y * Mathf.Floor(v.y / quant.y);
        float z = quant.z * Mathf.Floor(v.z / quant.z);
        return new Vector3(x, y, z);
    }

    /// <summary>
    /// �׺�޽� ������ �ٿ�带 ���Ѵ�.
    /// </summary>
    /// <returns></returns>
    Bounds QuantizedBounds()
    {
        // Quantize the bounds to update only when theres a 10% change in size
        var center = m_Tracked ? m_Tracked.position : transform.position;
        return new Bounds(Quantize(center, 0.1f * m_Size), m_Size);
    }

    /// <summary>
    /// NavMesh ������ �ٿ�� ������ �׷��ش�.
    /// </summary>
    void OnDrawGizmosSelected()
    {
        if (m_NavMesh)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(m_NavMesh.sourceBounds.center, m_NavMesh.sourceBounds.size);
        }

        Gizmos.color = Color.yellow;
        var bounds = QuantizedBounds();
        Gizmos.DrawWireCube(bounds.center, bounds.size);

        Gizmos.color = Color.green;
        var center = m_Tracked ? m_Tracked.position : transform.position;
        Gizmos.DrawWireCube(center, m_Size);
    }
}
