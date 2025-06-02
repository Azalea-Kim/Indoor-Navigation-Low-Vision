using UnityEngine;
namespace TechJuego.HapticFeedback
{
    public class CallHapticFeedback : MonoBehaviour
    {
        public void HeavyHaptic()
        {
            iOSHapticCall.HeavyHaptic();
        }
        public void MediumHaptic()
        {
            iOSHapticCall.MediumHaptic();
        }
        public void LightHaptic()
        {
            iOSHapticCall.LightHaptic();
        }
        public void RigidHaptic()
        {
            iOSHapticCall.RigidHaptic();

        }
        public void SoftHaptic()
        {
            iOSHapticCall.SoftHaptic();
        }
        public void PerformSuccessFeedback()
        {
            iOSHapticCall.PerformSuccessFeedback();
        }
        public void PerformErrorFeedback()
        {
            iOSHapticCall.PerformErrorFeedback();
        }
        public void PerformWarningFeedback()
        {
            iOSHapticCall.PerformWarningFeedback();
        }
    }

}