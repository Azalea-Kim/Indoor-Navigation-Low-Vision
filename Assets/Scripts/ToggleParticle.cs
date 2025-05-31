using UnityEngine;
using UnityEngine.UI;

public class ToggleParticle : MonoBehaviour
{
    [SerializeField] private Toggle toggle;
    [SerializeField] private RippleParticleEffect rippleEffect;

    private void Start()
    {
        if (toggle != null && rippleEffect != null)
        {
            // Set initial toggle state
            toggle.isOn = rippleEffect.GetRippleVisibility();

            // Add listener for toggle changes
            toggle.onValueChanged.AddListener(OnToggleChanged);
        }
    }

    private void OnToggleChanged(bool isVisible)
    {
        if (rippleEffect != null)
        {
            rippleEffect.SetRippleVisibility(isVisible);
        }
    }

}