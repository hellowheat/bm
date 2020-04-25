using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class meshCombine : MonoBehaviour
{
    delegate void DeleFunc(GameObject gb);

    void Start()
    {
        DeleFunc deleFunc = haidGB;
        MeshCombine();
        dealChildren(gameObject, deleFunc);
    }

    void MeshCombine()
    {
        MeshFilter[] filters = GetComponentsInChildren<MeshFilter>();
        CombineInstance[] combineInstance = new CombineInstance[filters.Length];
        for(int i = 0; i < filters.Length; i++)
        {
            combineInstance[i].mesh = filters[i].sharedMesh;
            combineInstance[i].transform = filters[i].transform.localToWorldMatrix;
        }

        transform.position = Vector3.zero;
        Mesh finalMesh = new Mesh();
        finalMesh.CombineMeshes(combineInstance);
        gameObject.GetComponent<MeshFilter>().sharedMesh = finalMesh;
    }

    void dealChildren(GameObject gb,DeleFunc deleFunc)
    {
        for(int i = 0; i < gb.transform.childCount; i++)
        {
            deleFunc(gb.transform.GetChild(i).gameObject);
        }
    }

    void haidGB(GameObject gb)
    {
        gb.SetActive(false);
    }
}
