using System.Collections.Generic;
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

        public MenuButtonView ButtonPrefab;

        public List<string> FiltersToLoad;

        public List<MenuButtonView> Buttons;

        public Transform ButtonsRoot;

        private void Awake()
        {
            InitButtons();
        }

        private void InitButtons()
        {
            if (FiltersToLoad == null)
            {
                return;
            }

            for (int fIndex = 0; fIndex < FiltersToLoad.Count; fIndex++)
            {
                var filter = FiltersToLoad[fIndex];

                var buttonInstance = CreateButtonInstance(filter);
                Buttons ??= new List<MenuButtonView>();
                Buttons.Add(buttonInstance);
            }
        }

        private MenuButtonView CreateButtonInstance(string filterName)
        {
            if (ButtonPrefab == null)
            {
                return default;
            }

            var button = Instantiate(ButtonPrefab, ButtonsRoot);
            button.OnClicked?.AddListener(OnFilterButtonClicked);
            button.Title = filterName;

            return button;
        }

        protected virtual void OnFilterButtonClicked(MenuButtonView sender)
        {
            if (sender != null)
            {
                var filterIndex = FiltersToLoad != null ? FiltersToLoad.IndexOf(sender.Title) : 0;
                LoadFilter(filterIndex);
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