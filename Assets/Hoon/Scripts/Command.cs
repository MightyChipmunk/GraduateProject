using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Command
{
    public int actionCategory;
    public int attackerIdx;
    public int deffenderIdx;
    public int team;

    public Command(int cat, int atk, int def, int team)
    {
        actionCategory = cat;
        attackerIdx = atk;
        deffenderIdx = def;
        this.team = team;
    }
}
