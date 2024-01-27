using UnityEngine;
using UnityEngine.VFX;

public class VisualEffects : MonoBehaviour
{
    [SerializeField] private VisualEffect _heatBlur;

    public void SetBlur(int health)
    {
        switch (health)
        {
            case 3:
                {
                    _heatBlur.SetFloat("Blur", 0);
                    _heatBlur.SetVector2("Distortion", new Vector2(8.5f, -0.3f));
                }
                return;
            case 1:
                {
                    _heatBlur.SetFloat("Blur", 0.07f);
                    _heatBlur.SetVector2("Distortion", new Vector2(10.3f, 0.24f));
                }
                return;
            case 0:
                {
                    _heatBlur.SetFloat("Blur", -0.09f);
                    _heatBlur.SetVector2("Distortion", new Vector2(17.5f, 12.87f));
                }
                return;

        }
    }
}
