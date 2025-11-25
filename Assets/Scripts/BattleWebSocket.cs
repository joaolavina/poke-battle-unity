using System;
using System.Threading.Tasks;
using System.Text;
using UnityEngine;
using NativeWebSocket;
using UnityEngine.Events;


public class BattleWebSocket : SingletonMonoBehaviour<BattleWebSocket>
{
    private WebSocket websocket;
    private string _wsUrl = "ws://localhost:8080/ws/battle";
    private int _userId;
    public event Action<BattleMessageDTO> OnBattleMessage;

    public event UnityAction<BattleMessageDTO> BattleStart;
    public event UnityAction<BattleMessageDTO> PlayerAction;
    public event UnityAction<BattleMessageDTO> TurnResult;
    public event UnityAction<BattleMessageDTO> BattleEnd;


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
                var raw = JsonUtility.FromJson<RawBattleMessage>(msg);
                var dto = BattleMessageConverter.FromRaw(raw);
                OnBattleMessage?.Invoke(dto);

                switch (dto.type)
                {
                    case MessageType.BATTLE_START:
                        BattleStart?.Invoke(dto);
                        break;

                    case MessageType.PLAYER_ACTION:
                        PlayerAction?.Invoke(dto);
                        break;

                    case MessageType.TURN_RESULT:
                        TurnResult?.Invoke(dto);
                        break;

                    case MessageType.BATTLE_END:
                        BattleEnd?.Invoke(dto);
                        break;
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