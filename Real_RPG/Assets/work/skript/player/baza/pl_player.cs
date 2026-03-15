using UnityEngine;

public class pl_player : entiti
{
    private int yr;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();
    }

    protected override void death()
    {
        print("player death");
    }
}
