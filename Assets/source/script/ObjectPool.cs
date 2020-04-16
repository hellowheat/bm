using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool
{
    GameObject preferb;
    private List<GameObject> pools;
    public ObjectPool(GameObject preferb)
    {
        this.preferb = preferb;
        pools = new List<GameObject>();    
    }
    public GameObject create(Transform tsf, Transform parent = null)
    {
        if(pools.Count == 0)
        {
            GameObject gbj = GameObject.Instantiate(preferb, tsf.position, tsf.rotation);
            gbj.SetActive(true);
            gbj.transform.SetParent(parent);
            return gbj;
        }
        else
        {
            GameObject gbj = pools[0];
            pools.RemoveAt(0);
            gbj.transform.SetPositionAndRotation(tsf.position, tsf.rotation);
            gbj.SetActive(true);
            gbj.transform.SetParent(parent);
            return gbj;
        }
    }

    public GameObject createNotShow(Transform tsf, Transform parent = null)
    {
        if (pools.Count == 0)
        {
            GameObject gbj = GameObject.Instantiate(preferb, tsf.position, tsf.rotation);
            gbj.transform.SetParent(parent);
            return gbj;
        }
        else
        {
            GameObject gbj = pools[0];
            pools.RemoveAt(0);
            gbj.transform.SetPositionAndRotation(tsf.position, tsf.rotation);
            gbj.transform.SetParent(parent);
            return gbj;
        }
    }

    public void destroy(GameObject gbj)
    {
        gbj.SetActive(false);
        pools.Add(gbj);
    }
}
