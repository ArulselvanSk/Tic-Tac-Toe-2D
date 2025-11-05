using System;
using Unity.Netcode;
using UnityEngine;

public class GameManager : NetworkBehaviour
{
    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Multiple Instance GameManager Found");
        }
        Instance = this;
    }

    public enum PlayerType
    {
        none,
        Circle,
        Cross
    }

    private PlayerType LocalPlayerType;
    private NetworkVariable<PlayerType> CurrentPlayablePlayerType = new();

    public event EventHandler<OnClickedOnGridPositionEventArgs> OnClickedOnGridPosition;

    public event EventHandler OnGameStarted;

    public event EventHandler OnCurrentPlayerChanged;

    public class OnClickedOnGridPositionEventArgs : EventArgs
    {
        public int x;
        public int y;
        public PlayerType playerType;
    }

    public override void OnNetworkSpawn()
    {
        ulong value = NetworkManager.Singleton.LocalClientId;
        if (value ==0)
        {
            LocalPlayerType = PlayerType.Circle;
        }
        else
        {
            LocalPlayerType = PlayerType.Cross;
        }

        if(IsServer)
        {
            NetworkManager.Singleton.OnClientConnectedCallback += NetworkManager_OnClientConnectedCallback; 
        }

        CurrentPlayablePlayerType.OnValueChanged += (PlayerType oldPlayerType, PlayerType newPlayerType) =>
        {
            OnCurrentPlayerChanged?.Invoke(this, EventArgs.Empty);
        };
    }

    private void NetworkManager_OnClientConnectedCallback(ulong obj)
    {
        if(NetworkManager.Singleton.ConnectedClientsList.Count == 2)
        {
            CurrentPlayablePlayerType.Value = PlayerType.Circle;
            //Game Start
            TriggerOnGameStartedRpc();
        }
    }

    [Rpc(SendTo.ClientsAndHost)]
    private void TriggerOnGameStartedRpc()
    {
        OnGameStarted?.Invoke(this, EventArgs.Empty);
    }

    public PlayerType GetPlayerType()
    {
        return LocalPlayerType;
    }
    public PlayerType GetCurrentPlayablePlayerType()
    {
        return CurrentPlayablePlayerType.Value;
    }

    [Rpc(SendTo.Server)]
    public void OnGridSelectedHandlerRpc(int x, int y, PlayerType playerType)
    {
        if (CurrentPlayablePlayerType.Value != playerType)
        {
            return;
        }
        Debug.Log($"Grid axis ({ x},{ y})");
        OnClickedOnGridPosition?.Invoke(this, new OnClickedOnGridPositionEventArgs { x = x , y = y , playerType = playerType});
        switch (CurrentPlayablePlayerType.Value){
            default:
            case PlayerType.Circle:
                CurrentPlayablePlayerType.Value = PlayerType.Cross;
                break;
            case PlayerType.Cross:
                CurrentPlayablePlayerType.Value = PlayerType.Circle;
                break;
        }
    }
}
