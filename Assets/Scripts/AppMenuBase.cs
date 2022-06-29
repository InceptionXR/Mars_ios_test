using UnityEngine;

namespace Assets.Scripts
{
    public class AppMenuBase : MonoBehaviour
    {
        [SerializeField]
        private ApplicationController _appController;
        public ApplicationController AppController
        {
            get
            {
                if (_appController == null)
                {
                    _appController = FindObjectOfType<ApplicationController>();
                }

                return _appController;
            }
        }

        public void LoadFilter(int filterIndex)
        {
            Debug.Log($"Load Filter {filterIndex} is requested.");

            var appController = AppController;
            if (appController != null)
            {
                appController.LoadFilter(filterIndex);
            }
        }

        public void NavigateToMainMenu()
        {
            Debug.Log($"Navigate to main menu requested");

            var appController = AppController;
            if (appController != null)
            {
                appController.NavigateToMainMenu();
            }
        }

        public void NavigateToArFilter()
        {
            Debug.Log($"Navigate to AR Filter requested");

            var appController = AppController;
            if (appController != null)
            {
                appController.NavigateToArFilter();
            }
        }

        public void ExitApplication()
        {
            Application.Quit();
        }
    }
}