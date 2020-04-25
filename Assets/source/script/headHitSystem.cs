using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class headHitSystem : MonoBehaviour
{
    public Transform startPos;
    public Transform endPos;
    public GameObject hitPref;
    public float showCD;
    private float lastShowTime;
    public enum HitType{
        boomerangAdd
    };

    private List<HitType> hitList;
    private ObjectPool htiPoolhtiPool;

    // Start is called before the first frame update
    void Start()
    {
        hitList = new List<HitType>();
    }

    // Update is called once per frame
    void Update()
    {
        if (lastShowTime > showCD)
        {
            if (hitList.Count != 0)
            {
                lastShowTime = 0;
                StartCoroutine(showHit(hitList[0]));
                hitList.RemoveAt(0);
            }
        } else lastShowTime += Time.deltaTime;
    }

    public void Push(HitType hitType)
    {
        hitList.Add(hitType);
    }

    IEnumerator showHit(HitType hitType)
    {
        yield return 0;
    }
}
