using Unity.MARS.Data;
using Unity.MARS.Providers;
using Unity.XRTools.ModuleLoader;
using UnityEngine;

namespace Assets.Scripts
{
    public class MARSSessionController : MonoBehaviour, IUsesCameraTexture
    {
        IProvidesCameraTexture IFunctionalitySubscriber<IProvidesCameraTexture>.provider { get; set; }

        private void Awake()
        {
            this.RequestCameraFacingDirection(CameraFacingDirection.User);
        }
    }
}