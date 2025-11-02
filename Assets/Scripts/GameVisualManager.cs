using UnityEngine;
using Unity.Netcode;

public class GameVisualManager : NetworkBehaviour
{
    [SerializeField] private Transform CircleTile;
    [SerializeField] private Transform CrossTile;

    private const float GRID_SIZE = 3f;

    private void Start()
    {
        GameManager.Instance.OnClickedOnGridPosition += GameManager_OnClickedOnGridPosition;
    }

    private void GameManager_OnClickedOnGridPosition(object sender, GameManager.OnClickedOnGridPositionEventArgs e)
    {
        SpawnTileObjectRpc(e.x, e.y);
    }

    [Rpc(SendTo.Server)]
    private void SpawnTileObjectRpc(int x, int y)
    {
        Transform SpawnedObject = Instantiate(CircleTile, GetGridWorldPositions(x, y), Quaternion.identity);
        SpawnedObject.GetComponent<NetworkObject>().Spawn(true);
        SpawnedObject.position = GetGridWorldPositions(x, y);
    }

    private Vector2 GetGridWorldPositions(int x, int y)
    {
        Vector2 pos = new Vector2(-GRID_SIZE + x * GRID_SIZE, -GRID_SIZE + y * GRID_SIZE);
        return pos;
    }
}
