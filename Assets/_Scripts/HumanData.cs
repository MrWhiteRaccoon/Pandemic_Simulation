using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HumanData
{
    public enum Age { kid,young,adult,old };
    public enum Occupation { quarantine, unoccupied, healthcare, politics, industry, student, education_0, education_1, leisure};

    public enum FamilyTrait { son, father, elder}

    public struct FamilyUnit
    {
        public List<Human> parent;
        public List<Human> son;
        public List<Human> elder;

        int members;

        public FamilyUnit(Human[] parent, Human[] son, Human[] elder)
        {
            this.parent = new List<Human>();
            this.son = new List<Human>();
            this.elder = new List<Human>();

            this.parent.AddRange(parent);
            this.son.AddRange(son);
            this.elder.AddRange(elder);

            members = parent.Length + son.Length + elder.Length;
        }

        public FamilyUnit(Human[] members)
        {
            this.parent = new List<Human>();
            this.son = new List<Human>();
            this.elder = new List<Human>();

            this.members = members.Length;

            foreach (Human human in members)
            {
                if (parent.Count < 2 && human.ageGroup==Age.adult)
                {
                    parent.Add(human);
                }
                else if (human.ageGroup == Age.old)
                {
                    elder.Add(human);
                }
                else if (human.ageGroup == Age.kid)
                {
                    son.Add(human);
                }
                else
                {
                    parent.Add(human);
                }
            }
        }
    }
}
