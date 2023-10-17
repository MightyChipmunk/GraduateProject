using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[Serializable]
public class TeamPacket 
{
    public ushort id = 1;

    public Team content;

    public TeamPacket(Team team)
    {
        content = team;
    }
}

[Serializable]
public class CommandPacket
{
    public ushort id = 0;

    public Command content;

    public CommandPacket(Command command)
    {
        content = command;
    }
}