using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifePanel : MonoBehaviour
{
    public Transform gridRoot;
    public GameObject lifePrefab;

    public void DrawLife(int life)
    {
        ClearGrid();
        if (!gridRoot) return;
        for (int i = 0; i < life; i++)
        {
            var lifeClone = Instantiate(lifePrefab, Vector3.zero, Quaternion.identity);
            lifeClone.transform.SetParent(gridRoot);
            lifeClone.transform.localScale = Vector3.one;
            lifeClone.transform.localPosition = Vector3.zero;
        }
    }
    public void ClearGrid()
    {
        if (!gridRoot) return;
        int childNum = gridRoot.childCount;
        for (int i = 0; i < childNum; i++)
        {
            var child = gridRoot.GetChild(i);
            if (!child) continue;
            Destroy(child.gameObject);
        }
    }
}
