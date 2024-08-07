using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LineGrid : MonoBehaviour
{
    [SerializeField]
    int rowCount = 7;
    [SerializeField]
    int colCount = 7;
    [SerializeField]
    int heightCount = 1;
    [SerializeField]
    float gridSize = 2.0f;

    private LineRenderer lineRenderer;
    private List<Vector3> posList;

    private void InitLineRenderer()
    {
        lineRenderer = this.GetComponent<LineRenderer>();
        lineRenderer.startWidth = 0.01f;
        lineRenderer.endWidth = 0.01f;
        lineRenderer.positionCount = ((rowCount + 1) + (colCount + 1)) * heightCount;
    }

    private void MakeGrid()
    {
        int toggle = 1;
        float curXPos = 0;
        float curYPos = 0;
        float curZPos = 0;

        for (int k = 1; k < heightCount + 1; k++)
        {
            curXPos = 0;
            curZPos = 0;
            posList.Add(new Vector3(0, curYPos, 0));
            for (int i = 1; i < colCount + 1; i++)
            {
                // |
                if (toggle > 0)
                {
                    posList.Add(new Vector3(curXPos, curYPos, toggle * gridSize * colCount));
                }
                else
                {
                    posList.Add(new Vector3(curXPos, curYPos, 0));
                }

                curXPos += gridSize;

                // - 
                if (toggle > 0)
                {
                    posList.Add(new Vector3((curXPos), curYPos, toggle * gridSize * colCount));
                }
                else
                {
                    posList.Add(new Vector3((curXPos), curYPos, 0));
                }

                toggle *= -1;
            }
            posList.Add(new Vector3((curXPos), curYPos, 0));
            // ==============================================================================================================
           
            toggle = -1;
            for (int i = 1; i < rowCount + 1; i++)
            {
                // -
                if (toggle < 0)
                {
                    posList.Add(new Vector3(0, curYPos, curZPos));
                }
                else
                {
                    posList.Add(new Vector3(toggle * gridSize * rowCount, curYPos, curZPos));
                }

                curZPos += gridSize;

                // |
                if (toggle < 0)
                {
                    posList.Add(new Vector3(0, curYPos, curZPos));
                }
                else
                {
                    posList.Add(new Vector3((toggle * gridSize * rowCount), curYPos, curZPos));
                }

                toggle *= -1;
            }
            posList.Add(new Vector3((toggle * gridSize * rowCount), curYPos, curZPos));

            curYPos += gridSize;
        }

        lineRenderer.positionCount = posList.Count;
        lineRenderer.SetPositions(posList.ToArray());
    }

    void Awake()
    {
        posList = new List<Vector3>();
        InitLineRenderer();
        MakeGrid();
    }
}
