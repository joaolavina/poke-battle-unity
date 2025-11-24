using System;
using System.Collections.Generic;

[Serializable]
public class StadiumResponse
{
    public bool success;
    public List<StadiumInfo> stadiums;
    public int count;
}

[Serializable]
public class StadiumInfo
{
    public string instanceId;
    public string name;
    public string queueName;
    public int waitingPlayers;
    public int activeBattles;
}
