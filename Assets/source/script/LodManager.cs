using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LodManager : MonoBehaviour
{
    [Header("lodModel_1")]
    public float lowDis;
    public float cullDis;
    public Transform[] lodObjList;

    [Header("lodModel_2")]
    public float cullDis_2;
    public Transform[] lodObjList_2;

    // Start is called before the first frame update
    private delegate void MDeal(LODGroup lodg);
    MDeal mDeal1, mDeal2;
    void Start()
    {
        mDeal1 = new MDeal(this.dealModel1);
        mDeal2 = new MDeal(this.dealModel2);
        foreach (Transform t in lodObjList)
        {
            findAllChild(t, mDeal1);
        }

        foreach (Transform t in lodObjList_2)
        {
            findAllChild(t, mDeal2);
        }

    }

    // Update is called once per frame
    void Update()
    {

    }

    void findAllChild(Transform obj,MDeal delFunc)
    {
        LODGroup lodg = obj.GetComponent<LODGroup>();
        if (lodg != null)
        {
            delFunc(lodg);
        }

        for (int i = 0; i < obj.childCount; i++)
        {
            findAllChild(obj.GetChild(i),delFunc);
        }

    }

    void dealModel1(LODGroup lodg)
    {
        LOD[] lods = lodg.GetLODs();
        Debug.Log(lods[0].screenRelativeTransitionHeight);
        Debug.Log(lods[1].screenRelativeTransitionHeight);
        lods[0].screenRelativeTransitionHeight = lowDis;
        lods[1].screenRelativeTransitionHeight = cullDis;
        lodg.SetLODs(lods);
    }
    void dealModel2(LODGroup lodg)
    {
        LOD[] lods = lodg.GetLODs();
        lods[0].screenRelativeTransitionHeight = cullDis_2;
        lodg.SetLODs(lods);
    }
}
