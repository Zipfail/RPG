using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class vr_move : MonoBehaviour
{
    [SerializeField] private Transform target;
    public Transform player;
    [SerializeField] private Transform[] targets;
    [SerializeField] private float time_stop;
    [SerializeField] private float distance_hit;
    [SerializeField] private float min_ray;
    [SerializeField] private float max_ray;
    [SerializeField] private float distance_stop;
    [SerializeField] private Transform obnur;
    private float timer;
    private int count;
    private NavMeshAgent agent =>transform.GetComponent<NavMeshAgent>();
    [SerializeField] private int sost = 0;
    //[SerializeField]private float speed_povorot;
    private float distance;
    private float distance_baza;

    
    void Start()
    {
        obnur.rotation = Quaternion.Euler(obnur.eulerAngles*-1);
        distance_baza = agent.stoppingDistance;
        player = GameObject.FindGameObjectWithTag("player").transform;
    }
    
    // Update is called once per frame
    void Update()
    {
        look();



        Vector3 direction = target.position - transform.position;
        direction.y = 0f; // ограничение по оси Y
        
        switch (sost)
        {
            case 0: //за таргетом
                distance = 0;
                agent.stoppingDistance = 0;
                if (Vector3.Distance(target.position, transform.position) <= distance+1)
                {
                    
                    if (timer > time_stop)
                    {
                        timer = 0;
                        if(count < targets.Count()-1)
                        {
                            count++;
                            print(count);
                        }
                        else
                        {
                            count = 0;
                        }
                    }
                    else {
                        timer += Time.fixedDeltaTime;
                        transform.Rotate(new Vector3(0,Time.fixedDeltaTime*10,0));
                    }
                    
                }
                target = targets[count];
                break;
            case 1: //за игроком
                distance = distance_baza;
                agent.stoppingDistance = distance_baza;
                target = player;
                if (Vector3.Distance(target.position, transform.position) <= distance)
                {
                    Quaternion targetRotation = Quaternion.LookRotation(direction);
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, agent.angularSpeed * Time.deltaTime);
                }
                break;
        }
        agent.SetDestination(target.position);
    }



    private void look()
    {
        obnur.LookAt(player.position);
        //obnur.rotation = Quaternion.Euler(new Vector3(0, Mathf.Clamp(obnur.forward.y, min_ray,max_ray), 0));
        RaycastHit hit;
        if (Physics.Raycast(obnur.position, obnur.forward, out hit, distance_hit))
        {
            if (hit.transform.tag == "player")
            {
                sost = 1;
                return;
            }
        }
        if (Vector3.Distance(transform.position,target.position) >= distance_stop)
        {
            sost = 0;
            return;
        }
    }

}
