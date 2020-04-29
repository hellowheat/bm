using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lifeManager : MonoBehaviour
{
    public float lifeLimit;
    private float nowlife;

    public float Nowlife { get => nowlife;}

    // Start is called before the first frame update
    void Start()
    {
        nowlife = lifeLimit;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void beAttack(float damage)
    {

    }

    public void beDebuff(float debuff)
    {

    }
}
