using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HumanData;

public class Human 
{
    //Custom
    public Human(Age ageGroup, Occupation occupation, int age)
    {
        this.ageGroup = ageGroup;
        this.occupation = occupation;
        this.age = age;
    }
    //Default
    public Human()
    {
        this.ageGroup = Age.adult;
        this.occupation = Occupation.unoccupied;
        this.age = 35;
    }

    public Age ageGroup;
    public Occupation occupation;

    public int age;

    public bool isInfected;
    public bool isCured;
    public bool isStatic;
}
