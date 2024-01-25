using UnityEngine;

public class Player : MonoBehaviour
{
    public int Lives = 5;

    public void PlayerReady()
    {
        ServiceLocator.Get<GameLoop>().EndTime();
    }
}
