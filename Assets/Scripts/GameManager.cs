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
    private PlayerType CurrentPlayablePlayerType;

    public event EventHandler<OnClickedOnGridPositionEventArgs> OnClickedOnGridPosition;
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
            CurrentPlayablePlayerType = PlayerType.Circle;
        }
    }

    public PlayerType GetPlayerType()
    {
        return LocalPlayerType;
    }

    [Rpc(SendTo.Server)]
    public void OnGridSelectedHandlerRpc(int x, int y, PlayerType playerType)
    {
        if (CurrentPlayablePlayerType != playerType)
        {
            return;
        }
        Debug.Log($"Grid axis ({ x},{ y})");
        OnClickedOnGridPosition?.Invoke(this, new OnClickedOnGridPositionEventArgs { x = x , y = y , playerType = playerType});
        switch (CurrentPlayablePlayerType){
            default:
            case PlayerType.Circle:
                CurrentPlayablePlayerType = PlayerType.Cross;
                break;
            case PlayerType.Cross:
                CurrentPlayablePlayerType = PlayerType.Circle;
                break;
        }
    }
}
