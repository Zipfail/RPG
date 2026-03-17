using UnityEngine;

abstract public class gn_baz_mele : MonoBehaviour
{
    [SerializeField] protected float damage;
    [SerializeField] protected Animator anim;
    protected bool razr_attac;
    public int count;
    protected float timer;
    [SerializeField]protected float time_atac;

    abstract protected void attac();
    public void razr(bool r)
    {
        razr_attac = r;
    }
}
