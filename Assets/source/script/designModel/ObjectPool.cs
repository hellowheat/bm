using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool
{
    GameObject preferb;
    Transform parentObj;
    private List<GameObject> pools;
    public ObjectPool(GameObject preferb,Transform parentObj)
    {
        this.preferb = preferb;
        this.parentObj = parentObj;
        pools = new List<GameObject>();    
    }
    public GameObject create(Transform tsf)
    {
        if(pools.Count == 0)
        {
            GameObject gbj = GameObject.Instantiate(preferb, tsf.position, tsf.rotation);
            gbj.SetActive(true);
            gbj.transform.SetParent(parentObj.transform);
            return gbj;
        }
        else
        {
            GameObject gbj = pools[0];
            pools.RemoveAt(0);
            gbj.transform.SetPositionAndRotation(tsf.position, tsf.rotation);
            gbj.SetActive(true);
            return gbj;
        }
    }

    public GameObject createNotShow(Transform tsf)
    {
        if (pools.Count == 0)
        {
            GameObject gbj = GameObject.Instantiate(preferb, tsf.position, tsf.rotation);
            gbj.transform.SetParent(parentObj.transform);
            return gbj;
        }
        else
        {
            GameObject gbj = pools[0];
            pools.RemoveAt(0);
            gbj.transform.SetPositionAndRotation(tsf.position, tsf.rotation);
            return gbj;
        }
    }

    public void destroy(GameObject gbj)
    {
        gbj.SetActive(false);
        pools.Add(gbj);
    }
}
