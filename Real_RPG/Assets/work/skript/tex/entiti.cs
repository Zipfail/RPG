using UnityEngine;

abstract public class entiti : MonoBehaviour
{
    [SerializeField]protected float xp;
    protected float max_xp;
    [SerializeField]protected float armor;
    public void Start()
    {
        max_xp = xp;
    }
    protected void Update()
    {
        if(xp <= 0)
        {
            death();
        }
    }
    public void _damag(float dam)
    {
        if (xp - dam > 0)
        {
            xp -= dam;
        }
        else
        {
            xp = 0;
        }
    }
    public void _regen(float reg)
    {
        if (xp + reg < max_xp)
        {
            xp += reg;
        }
        else
        {
            xp = max_xp;
        }
    }
    abstract protected void death();
}
