using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopulationManager : MonoBehaviour
{
    public int totalPopulation;

    public GameObject humanObject;

    public Vector2 mapLimits;
    public GraphTest graph;

    public List<Human> everyHuman = new List<Human>();
    public List<Human> infectedHuman = new List<Human>();

    List<float> infectionData=new List<float>();
    
    public float refreshTime;
    private float timeCount;

    private void Start()
    {
        timeCount = refreshTime;
        everyHuman = new List<Human>();
        infectedHuman = new List<Human>();
        GeneratePopulation();
        FindObjectsOfType<HumanBehaviour>()[Random.Range(0, FindObjectsOfType<HumanBehaviour>().Length-1)].Infect();
    }

    private void Update()
    {
        timeCount -= Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.O))
        {
            Time.timeScale *= 2;
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            Time.timeScale /= 2;
        }

        if (timeCount<=0)
        {
            timeCount = refreshTime;

            infectionData.Add(infectedHuman.Count);
            graph.RefreshData(infectionData.ToArray(), new Vector2(0, totalPopulation));
        }

        Debug.Log("Infected: " + infectedHuman.Count);
    }

    //Fix this!! the for loop should go from 0 to total population
    void GeneratePopulation()
    {
        for (int i = 0; i < totalPopulation; i++)
        {
            int coordX = -(int)mapLimits.x;
            int coordY = -(int)mapLimits.y;
            
            coordY += 2*(i/(int)(mapLimits.x));
            coordX += 2*(i - (i / (int)(mapLimits.x)) * (int)(mapLimits.x));

            HumanBehaviour human = Instantiate(humanObject, new Vector3(coordX, 0, coordY), Quaternion.identity, this.transform).GetComponent<HumanBehaviour>();
            Human humanData=new Human();
            human.data = humanData;
            everyHuman.Add(humanData);
        }
    }

    public void InfectHuman(Human human)
    {
        infectedHuman.Add(human);
    }

    public void RecoverHuman(Human human)
    {
        infectedHuman.Remove(human);
    }
}
