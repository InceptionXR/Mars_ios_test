using AssetSupplier;
using NetworkFramework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class TestView : MonoBehaviour
{
    public string AssetBundleName;

    public string PrefabAssetName = "root";

    public List<string> Errors;

    public GameObject FilterPrefab;

    [Tooltip("Filter Prefab will be instantiated under this Transform")]
    public Transform FilterRoot;

    private GameObject _filterInstance;

    private GUIStyle _errorsStyle;

    private NetworkBundleCache _assetBundleCache = new NetworkBundleCache();


    // Start is called before the first frame update
    void Start()
    {
        Errors ??= new List<string>();
        Errors.Clear();

        LoadAndCreateArFilter();
    }

    private void OnDestroy()
    {
        if (_filterInstance != null)
        {
            Destroy(_filterInstance);
        }
    }

    private async Task LoadAndCreateArFilter()
    {
        // load and cache prefab
        await LoadArFilterPrefab();

        // instantiate prefab
        if (FilterPrefab != null)
        {
            _filterInstance = Instantiate(FilterPrefab, FilterRoot);
        }
        else
        {
            Errors.Add("No Filter Prefab to Instantiate!");
        }
    }

    private async Task LoadArFilterPrefab()
    {
        if (string.IsNullOrEmpty(AssetBundleName))
        {
            Errors.Add("AssetBundleName is empty!");
            return;
        }

        if (string.IsNullOrEmpty(PrefabAssetName))
        {
            Errors.Add($"PrefabName is empty!");
            return;
        }

        var assetBundle = await NetworkAssets.GetAssetBundle(AssetBundleName).GetAsset();
        if (assetBundle == null)
        {
            Errors.Add("No AssetBundle downloaded!");
            return;
        }

        // download prefab
        FilterPrefab = assetBundle.LoadAsset<UnityEngine.GameObject>(PrefabAssetName);
        //FilterPrefab = await _assetBundleCache.GetAssetFromBundle<GameObject>(AssetBundleName, PrefabAssetName);
    }

    private void OnGUI()
    {
        if (_errorsStyle == null)
        {
            _errorsStyle = new GUIStyle(GUI.skin.label);
            _errorsStyle.normal.textColor = Color.red;
        }

        if (Errors != null)
        {
            for(int i = 0; i < Errors.Count; i++)
            {
                GUILayout.Label($"{Errors[i]};", _errorsStyle);
            }
        }
    }
}