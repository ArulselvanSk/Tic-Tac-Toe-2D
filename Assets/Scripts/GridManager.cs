using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] private int x = 0;
    [SerializeField] private int y = 0;

    private void OnMouseDown()
    {
        Debug.Log($"Click on the tile : ({x},{y})");
        GameManager.Instance.OnGridSelectedHandlerRpc(x, y, GameManager.Instance.GetPlayerType());
    }
}
