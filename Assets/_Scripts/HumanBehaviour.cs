using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HumanData;
using UnityEngine.AI;

public class HumanBehaviour : MonoBehaviour
{
    [Header("Disease Settings")]
    public float recoveryTimeAv;
    public float recoveryTimeDisp;

    [Header("Behaviour Settings")]
    public float distanceThreshold;
    public float moveability;
    public float speed;

    public Human data;
    public SpriteRenderer spriteRenderer;

    NavMeshAgent agent;
    PopulationManager manager;
    float recoveryTime;

    // Start is called before the first frame update
    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        manager = FindObjectOfType<PopulationManager>();
        recoveryTime = Random.Range(recoveryTimeAv-recoveryTimeDisp, recoveryTimeAv+recoveryTimeDisp);
        spriteRenderer.color = manager.healthyColor;
        agent.speed = speed;
    }

    // Update is called once per frame
    void Update()
    {
        if (data.isInfected)
        {
            recoveryTime -= Time.deltaTime;
            if (recoveryTime <= 0)
            {
                Cure();
            }
        }
        if (data.isStatic)
        {
            return;
        }
        if (agent.remainingDistance > distanceThreshold)
        {
            return;
        }
        else
        {
            SetRandomTarget(moveability);
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

    public void Infect()
    {
        data.isInfected = true;
        manager.InfectHuman(data);
        spriteRenderer.color = manager.infectedColor;
    }

    public void Cure()
    {
        data.isInfected = false;
        data.isCured = true;
        manager.RecoverHuman(data);
        spriteRenderer.color = manager.curedColor;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (data.isCured || data.isInfected)
        {
            return;
        }
        HumanBehaviour otherHuman = other.GetComponent<HumanBehaviour>();
        if (otherHuman.data.isInfected)
        {
            Infect();
        }
        
    }
}
