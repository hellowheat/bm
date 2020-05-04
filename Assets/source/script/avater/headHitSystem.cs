using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class headHitSystem : MonoBehaviour
{
    public Transform startPos;
    public float offsetUp;

    public Transform hitBox;
    public GameObject[] hitPref;
    public float showCD;
    public float showLength;
    private float lastShowTime;

    private List<ObjectPool> pools;
    private List<int> waitShowHit;
    private ObjectPool htiPoolhtiPool;

    // Start is called before the first frame update
    void Start()
    {
        waitShowHit = new List<int>();
        pools = new List<ObjectPool>();
        for (int i = 0; i < hitPref.Length; i++)
        {
            pools.Add(new ObjectPool(hitPref[i], hitBox));
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (lastShowTime > showCD)
        {
            if (waitShowHit.Count != 0)
            {
                lastShowTime = 0;
                show(waitShowHit[0]);
                waitShowHit.RemoveAt(0);
            }
        } else lastShowTime += Time.deltaTime;
    }

    public void Push(int hitType)
    {
        waitShowHit.Add(hitType);
    }

    private void show(int hitType)
    {
        GameObject gb = pools[hitType].create(startPos);
        MeshRenderer renderer = gb.GetComponent<MeshRenderer>();
        renderer.material.color = new Color(1, 1, 1, 0);
        renderer.material.DOColor(Color.white, showLength).SetEase(Ease.OutQuart);
        gb.transform.DOMoveY(gb.transform.position.y + offsetUp, showLength).SetEase(Ease.OutQuart).OnComplete(delegate {
            Debug.Log(gb);
            pools[hitType].destroy(gb);
        });
    }
}
