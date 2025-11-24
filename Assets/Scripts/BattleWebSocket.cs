using System;
using System.Threading.Tasks;
using System.Text;
using UnityEngine;
using NativeWebSocket;
using UnityEngine.Events;

[Serializable]
public class BattleMessageDTO
{
    public string type;
    public string battleId;
    public string instanceId;
    public User user;
    public string opponentName;
    public int damage;
    public string battleLog;
}

public class BattleWebSocket : SingletonMonoBehaviour<BattleWebSocket>
{
    private WebSocket websocket;
    private string _wsUrl = "ws://localhost:8080/ws/battle";
    private int _userId;
    public event Action<BattleMessageDTO> OnBattleMessage;

    public event UnityAction<BattleMessageDTO> BattleStart;
    public event UnityAction<BattleMessageDTO> PlayerAction;


    public async Task Connect()
    {
        _userId = UserService.GetInstance().CurrentUser.id;
        websocket = new WebSocket(_wsUrl);

        websocket.OnOpen += () =>
        {
            Debug.Log("WS conectado");
            // envia o userId para registrar a sessão no servidor
            websocket.SendText(_userId.ToString());
        };

        websocket.OnError += (e) =>
        {
            Debug.LogError("WS Erro: " + e);
        };

        websocket.OnClose += (e) =>
        {
            Debug.LogWarning("WS Fechado: " + e);
        };

        websocket.OnMessage += (bytes) =>
        {
            var msg = Encoding.UTF8.GetString(bytes);
            Debug.Log("WS Mensagem: " + msg);

            try
            {
                var dto = JsonUtility.FromJson<BattleMessageDTO>(msg);
                OnBattleMessage?.Invoke(dto);

                switch (dto.type)
                {
                    case "BATTLE_START":
                        BattleStart?.Invoke(dto);
                        break;

                    case "PLAYER_ACTION":
                        PlayerAction?.Invoke(dto);
                        break;

                        // case "TURN_RESULT":
                        //     HandleTurnResult(dto);
                        //     break;

                        // case "BATTLE_END":
                        //     HandleBattleEnd(dto);
                        //     break;
                }
            }
            catch (Exception ex)
            {
                Debug.LogError("Erro ao desserializar mensagem WS: " + ex);
            }
        };

        await websocket.Connect();
    }

    private void Update()
    {
#if !UNITY_WEBGL || UNITY_EDITOR
        websocket?.DispatchMessageQueue();
#endif
    }

    private async void OnApplicationQuit()
    {
        if (websocket != null)
            await websocket.Close();
    }
}