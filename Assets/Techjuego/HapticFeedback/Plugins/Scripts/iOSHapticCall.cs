namespace TechJuego
{
    public class iOSHapticCall
    {
        public static void HeavyHaptic()
        {
#if UNITY_IOS
        iOSPlugins.PerformUIImpactFeedbackStyleHeavy();
#endif
        }
        public static void MediumHaptic()
        {
#if UNITY_IOS
        iOSPlugins.PerformUIImpactFeedbackStyleMedium();
#endif
        }
        public static void LightHaptic()
        {
#if UNITY_IOS
        iOSPlugins.PerformUIImpactFeedbackStyleLight();
#endif
        }
        public static void RigidHaptic()
        {
#if UNITY_IOS
        iOSPlugins.PerformUIImpactFeedbackStyleRigid();
#endif
        }
        public static void SoftHaptic()
        {
#if UNITY_IOS
        iOSPlugins.PerformUIImpactFeedbackStyleSoft();
#endif
        }
        public static void PerformSuccessFeedback()
        {
#if UNITY_IOS
        iOSPlugins.PerformUINotificationFeedbackTypeSuccess();
#endif
        }
        public static void PerformErrorFeedback()
        {
#if UNITY_IOS
        iOSPlugins.PerformErrorFeedback();
#endif
        }
        public static void PerformWarningFeedback()
        {
#if UNITY_IOS
        iOSPlugins.PerformUINotificationFeedbackTypeWarning();
#endif
        }
    }
}