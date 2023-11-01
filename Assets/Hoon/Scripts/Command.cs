using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class Command
{
    public int actionCategory;
    public int attackerIdx;
    public int deffenderIdx;
    public string id;

    public Command(int cat, int atk, int def, string id = "")
    {
        actionCategory = cat;
        attackerIdx = atk;
        deffenderIdx = def;
        this.id = id;
    }
}

[Serializable]
public class CommandList
{
    public List<Command> commands;

    public CommandList()
    {
        commands = new List<Command>();
    }

    public void Add(Command com, UnityAction callback)
    {
        commands.Add(com);
        callback();
    }
}