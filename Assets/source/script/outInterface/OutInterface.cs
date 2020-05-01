using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutInterface : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    virtual public void beAttack(float damage){}
    virtual public void beDebuff(string debuff){}

    virtual public bool isDead(){return false;}

}
