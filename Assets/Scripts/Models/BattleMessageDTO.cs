using System;
using UnityEngine;

// Raw class: tudo que pode ser "problemático" vira string (enums, nullables)
[Serializable]
public class RawBattleMessage
{
    public string type;            // "BATTLE_START" etc.
    public string from;
    public string to;
    public User user;              // sua classe User já existente
    public string battleId;
    public string opponentName;
    public StadiumStatusDTO stadium; // pode ser null -> JsonUtility aceita null para objetos?
    public string target;          // pode vir "null" -> string permite detectar
    public string action;          // "ATTACK" ou null
    public string damage;          // pode vir null
    public string battleLog;
    public string instanceId;
}

// Seu DTO final (já tinha)
[Serializable]
public class BattleMessageDTO
{
    public MessageType type;
    public string from;
    public string to;
    public User user;
    public string battleId;
    public string opponentName;
    public StadiumStatusDTO stadium;
    public int? target;
    public BattleAction? action;
    public int? damage;
    public string battleLog;
    public string instanceId;
}

public enum MessageType
{
    LOGIN,
    BATTLE_START,
    PLAYER_ACTION,
    TURN_RESULT,
    BATTLE_END,
    BATTLE_STATE
}

public enum BattleAction
{
    ATTACK,
    SWITCH_POKEMON,
    FLEE
}

// CONVERSOR: Raw -> DTO
public static class BattleMessageConverter
{
    public static BattleMessageDTO FromRaw(RawBattleMessage raw)
    {
        if (raw == null) return null;

        var dto = new BattleMessageDTO();
        // type (string -> enum)
        if (!string.IsNullOrEmpty(raw.type) && Enum.TryParse<MessageType>(raw.type, out var mt))
            dto.type = mt;
        else
            dto.type = MessageType.LOGIN; // ou outro default que você prefira

        dto.from = raw.from;
        dto.to = raw.to;
        dto.user = raw.user;
        dto.battleId = raw.battleId;
        dto.opponentName = raw.opponentName;
        dto.stadium = raw.stadium;
        dto.instanceId = raw.instanceId;
        dto.battleLog = raw.battleLog;

        // target (string -> int?)
        if (!string.IsNullOrEmpty(raw.target) && int.TryParse(raw.target, out var t))
            dto.target = t;
        else
            dto.target = null;

        // damage
        if (!string.IsNullOrEmpty(raw.damage) && int.TryParse(raw.damage, out var d))
            dto.damage = d;
        else
            dto.damage = null;

        // action (string -> enum?)
        if (!string.IsNullOrEmpty(raw.action) && Enum.TryParse<BattleAction>(raw.action, out var ba))
            dto.action = ba;
        else
            dto.action = null;

        return dto;
    }
}
