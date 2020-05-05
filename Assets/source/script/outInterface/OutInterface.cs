using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;

public class OutInterface : MonoBehaviour
{
    protected lifeManager life;

    void Start()
    {
        life = GetComponent<lifeManager>();
        init();
    }

    public void Update()
    {
        update();
    }
    virtual public void init() { }
    virtual public void update() { }


    virtual public void beAttack(float damage,GameObject atkObj){}
    virtual public void beDebuff(string debuff){}
    

    virtual public bool isDead(){return false;}
    virtual public float getLife() { return life.Nowlife; }
    virtual public Vector3 getBeAtkPos() { return Vector3.zero; }

    virtual public void pushProp(string propStr) { }

}
