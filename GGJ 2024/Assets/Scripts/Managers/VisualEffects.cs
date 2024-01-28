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
                    _heatBlur.SetVector2("Distortion", new Vector2(0.54f, 0f));
                }
                return;
            case 1:
                {
                    _heatBlur.SetFloat("Blur", 0.07f);
                    _heatBlur.SetVector2("Distortion", new Vector2(0.54f, 0f));
                }
                return;
            case 0:
                {
                    _heatBlur.SetFloat("Blur", 0.14f);
                    _heatBlur.SetVector2("Distortion", new Vector2(2.22f, 0f));
                }
                return;

        }
    }
}
