using UnityEngine;

abstract public class gn_baz_mele : sound
{
    [SerializeField] protected float damage;
    [SerializeField] protected Animator anim;
    protected bool razr_attac;
    public int count;
    protected float timer;
    [SerializeField]protected float time_atac;
    [SerializeField]public Vector3 posUp;
    [SerializeField]public Vector3 rotation;

    abstract protected void attac();
    public void razr(bool r)
    {
        razr_attac = r;
    }
}
