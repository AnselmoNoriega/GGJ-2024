using UnityEngine;

public class Player : MonoBehaviour
{
    public int Lives = 5;

    public void PlayerReady()
    {
        ServiceLocator.Get<UIManager>().ButtonSetActive(false);
        ServiceLocator.Get<GameLoop>().EndTime();
    }
}
