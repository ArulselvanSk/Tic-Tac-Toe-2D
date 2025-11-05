using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private GameObject CrossArrowObject;
    [SerializeField] private GameObject CircleArrowObject;
    [SerializeField] private GameObject CrossYouTextObject;
    [SerializeField] private GameObject CircleYouTextObject;

    private void Awake()
    {
        CrossArrowObject.SetActive(false);
        CircleArrowObject.SetActive(false);
        CrossYouTextObject.SetActive(false);
        CircleYouTextObject.SetActive(false);
    }

    private void Start()
    {
        GameManager.Instance.OnGameStarted += OnGameStarted_Handler;
        GameManager.Instance.OnCurrentPlayerChanged += GameManager_OnCurrentPlayerChanged;
    }

    private void GameManager_OnCurrentPlayerChanged(object sender, System.EventArgs e)
    {
        UpdateCurrentArrow();
    }

    private void OnGameStarted_Handler(object sender, System.EventArgs e)
    {
        if(GameManager.Instance.GetPlayerType() == GameManager.PlayerType.Cross)
        {
            CrossYouTextObject.SetActive(true);
        }
        else
        {
            CircleYouTextObject.SetActive(true);
        }

        UpdateCurrentArrow(); 
    }

    public void UpdateCurrentArrow()
    {
        if (GameManager.Instance.GetCurrentPlayablePlayerType() == GameManager.PlayerType.Cross)
        {
            CircleArrowObject.SetActive(false);
            CrossArrowObject.SetActive(true);
        }
        else
        {
            CrossArrowObject.SetActive(false);
            CircleArrowObject.SetActive(true);
        }
    }
}
