using Unity.MARS.Data;
using Unity.MARS.Providers;
using Unity.XRTools.ModuleLoader;
using UnityEngine;

namespace Assets.Scripts
{
    public class MARSSessionController : MonoBehaviour, IUsesCameraTexture, IUsesFunctionalityInjection
    {
        public IProvidesFunctionalityInjection provider { get; set; }
        IProvidesCameraTexture IFunctionalitySubscriber<IProvidesCameraTexture>.provider { get; set; }

        private void OnEnable()
        {
            this.EnsureFunctionalityInjected();
            this.RequestCameraFacingDirection(CameraFacingDirection.User);
        }
    }
}