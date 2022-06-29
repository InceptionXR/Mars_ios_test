using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts
{
    public class ApplicationController : MonoBehaviour
    {
        public string ARSceneName;

        public string MainMenuSceneName;

        public List<string> FilterNames;

        public int CurrentFilterIndex = -1;

        public GameObject CurrentFilter;

        public string CurrentRouting = MainMenuRoutingName;

        public const string MainMenuRoutingName = "MainMenu";
        public const string ArFilterRoutingName = "AR Filter";

        protected AsyncOperation LoadingSceneOperation;
        protected int RequestedFilterIndex;

        private void Awake()
        {
            DontDestroyOnLoad(this);
        }

        private void Start()
        {
            NavigateToMainMenu();
        }

        public void LoadFilter(int filterIndex)
        {
            Debug.Log($"Load Filter {filterIndex} is requested");

            if (CurrentRouting != ArFilterRoutingName)
            {
                // switch to the AR Filter scene
                RequestedFilterIndex = filterIndex;
                NavigateToArFilter();

                Debug.Log($"Requested to load filter of index {filterIndex}, but AR Scene is not loading. Loading AR Scene");
                return;
            }

            if (FilterNames == null)
            {
                Debug.LogWarning($"Can't load filter {filterIndex}: No Filter Names!");
                return;
            }

            if (filterIndex < 0 || filterIndex >= FilterNames.Count)
            {
                Debug.LogWarning($"Requested to load filter with index {filterIndex}, but we have only {FilterNames.Count} filter names!");
                return;
            }

            var filterName = FilterNames[filterIndex];
            if (string.IsNullOrEmpty(filterName))
            {
                UnloadCurrentFilter();
                return;
            }

            // load and instantiate filter
            var prefabPath = $"{filterName}";
            var filterPrefab = Resources.Load<GameObject>(prefabPath);
            if (filterPrefab == null)
            {
                Debug.LogWarning($"Can't load prefab at path {prefabPath}");
                return;
            }

            // unload already loaded filter
            UnloadCurrentFilter();

            CurrentFilter = Instantiate(filterPrefab);
        }

        public void UnloadCurrentFilter()
        {
            if (CurrentFilter != null)
            {
                Destroy(CurrentFilter);
            }
        }

        public void NavigateToMainMenu()
        {
            if (LoadingSceneOperation != null && !LoadingSceneOperation.isDone)
            {
                Debug.Log($"Trying to navigate to MainMenu scene, but we're in Scene loading state now!");
                return;
            }

            CurrentRouting = MainMenuRoutingName;

            LoadingSceneOperation = SceneManager.LoadSceneAsync(MainMenuSceneName, LoadSceneMode.Single);

            LoadingSceneOperation.completed += OnLoadingSceneCompleted;
        }

        public void NavigateToArFilter()
        {
            if (LoadingSceneOperation != null && !LoadingSceneOperation.isDone)
            {
                Debug.Log($"Trying to navigate to ARFilter scene, but we're in Scene loading state now!");
                return;
            }

            CurrentRouting = ArFilterRoutingName;

            LoadingSceneOperation = SceneManager.LoadSceneAsync(ARSceneName, LoadSceneMode.Single);

            LoadingSceneOperation.completed += OnLoadingSceneCompleted;
        }

        private void OnLoadingSceneCompleted(AsyncOperation obj)
        {
            if (CurrentRouting == ArFilterRoutingName && RequestedFilterIndex >= 0)
            {
                var filterIndex = RequestedFilterIndex;
                RequestedFilterIndex = -1;
                LoadFilter(filterIndex);
            }
        }

        private IEnumerator WaitAndLoadFilter(int filterIndex)
        {
            yield return new WaitForSeconds(5);

            LoadFilter(filterIndex);
        }
    }
}