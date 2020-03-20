using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HumanData;
using UnityEngine.AI;

public class HumanBehaviour : MonoBehaviour
{
    public float distanceThreshold;
    public Human data;
    public SpriteRenderer spriteRenderer;

    [HideInInspector]
    public bool isInfected = false;

    float temporalInfection;
    NavMeshAgent agent;
    PopulationManager manager;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        manager = FindObjectOfType<PopulationManager>();
        temporalInfection = Random.Range(5f, 30f);
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time >= temporalInfection && !isInfected)
        {
            isInfected = true;
            manager.InfectHuman(data);
            spriteRenderer.color = Color.red;
        }
        if (agent.remainingDistance > distanceThreshold)
        {
            return;
        }
        else
        {
            SetRandomTarget(5f);
        }
    }

    public void SetRandomTarget(float radius = 1f)
    {
        Vector3 target = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f));
        //Debug.Log("Mod " + (target.normalized * radius).magnitude);

        target = transform.position + target.normalized * radius;
        target.x = Mathf.Clamp(target.x, -manager.mapLimits.x+1, manager.mapLimits.x-1);
        target.z = Mathf.Clamp(target.z, -manager.mapLimits.y, manager.mapLimits.y-1);

        agent.SetDestination(target);
    }
}
