using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopulationManager : MonoBehaviour
{
    [Header("Quarrantine Settings")]
    public bool socialDistancing;
    [Range(0, 100)] public float socialDistancingPercentage;

    [Header("SimulationSettings")]
    public int totalPopulation;
    public float secondsPerDay;

    [Header("Colors")]
    public Color healthyColor;
    public Color infectedColor;
    public Color curedColor;
    public Color deathColor;
    public Color backgroundColor;

    [Header("UI")]
    public Text healthyText;
    public Text infectedText;
    public Text curedText;
    public Text populationText;
    public Text densityText;
    public Text dayText;

    [Header("Refs")]
    public GameObject humanObject;
    public Vector2 mapLimits;
    public GraphTest graph;

    public List<Human> everyHuman = new List<Human>();
    public List<Human> infectedHuman = new List<Human>();
    public List<Human> curedHuman = new List<Human>();

    List<float> infectionData=new List<float>();
    List<float> curedData = new List<float>();
    List<float> healthyData = new List<float>();
    
    public float refreshTime;
    private float timeCount;

    private void Start()
    {
        timeCount = refreshTime;

        //GeneratePopulationGrid();
        GeneratePopulationRandom();

        FindObjectsOfType<HumanBehaviour>()[Random.Range(0, FindObjectsOfType<HumanBehaviour>().Length-1)].Infect();

        UISetup();

        graph.healthyColor = healthyColor;
        graph.infectedColor = infectedColor;
        graph.curedColor = curedColor;
        graph.deathColor = deathColor;
        graph.backgroundColor = backgroundColor;
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
            curedData.Add(curedHuman.Count);
            healthyData.Add(totalPopulation - infectedHuman.Count - curedHuman.Count);

            graph.RefreshData(infectionData.ToArray(),curedData.ToArray(),healthyData.ToArray(), new Vector2(0, totalPopulation+100));
        }
        UpdateUI();
        if (infectedHuman.Count == 0)
        {
            Debug.Break();
        }
    }

    void UISetup()
    {
        healthyText.color = healthyColor;
        infectedText.color = infectedColor;
        curedText.color = curedColor;
        populationText.text = "Total population: " + totalPopulation;
        densityText.text = "Population density: " + (totalPopulation) / (mapLimits.x * mapLimits.y) + " (p/Km^2)";
    }

    void UpdateUI()
    {
        healthyText.text = "Healthy: " + (totalPopulation - infectedHuman.Count - curedHuman.Count) + "  " + (float)(totalPopulation - infectedHuman.Count - curedHuman.Count) * 100 / totalPopulation + "%";
        infectedText.text = "Infected: " + infectedHuman.Count + "  " + (float)infectedHuman.Count * 100 / totalPopulation + "%";
        curedText.text = "Cured: " + curedHuman.Count + "  " + (float)curedHuman.Count * 100 / totalPopulation + "%";

        dayText.text = "Day: " + (int)(Time.time / secondsPerDay);
    }

    void GeneratePopulationGrid()
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

    void GeneratePopulationRandom()
    {
        for (int i = 0; i < totalPopulation; i++)
        {
            float coordX = Random.Range(-mapLimits.x + 1, mapLimits.x - 1);
            float coordY = Random.Range(-mapLimits.y + 1, mapLimits.y - 1);


            HumanBehaviour human = Instantiate(humanObject, new Vector3(coordX, 0, coordY), Quaternion.identity, this.transform).GetComponent<HumanBehaviour>();
            Human humanData = new Human();
            human.data = humanData;
            everyHuman.Add(humanData);
            if (Random.Range(0, 100) < socialDistancingPercentage)
            {
                humanData.isStatic = true;
            }
        }
    }

    public void InfectHuman(Human human)
    {
        infectedHuman.Add(human);
    }

    public void RecoverHuman(Human human)
    {
        infectedHuman.Remove(human);
        curedHuman.Add(human);
    }
}
