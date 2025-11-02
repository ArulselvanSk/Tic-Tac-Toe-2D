using UnityEngine;

public class GameVisualManager : MonoBehaviour
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
        Instantiate(CircleTile, GetGridWorldPositions(e.x, e.y), Quaternion.identity);
    }
    private Vector2 GetGridWorldPositions(int x, int y)
    {
        Vector2 pos = new Vector2(-GRID_SIZE + x * GRID_SIZE, -GRID_SIZE + y * GRID_SIZE);
        return pos;
    }
}
