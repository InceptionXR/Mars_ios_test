using AssetSupplier;
using NetworkFramework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class TestView : MonoBehaviour
{
    public string AssetBundleName;

    public string PrefabAssetName = "root";

    public List<string> Errors;

    [Tooltip("If true, will load AssetBundle from StreamingAssets, not from CDN")]
    public bool IsLoadFromStreamingAssets;

    public GameObject FilterPrefab;

    [Tooltip("Filter Prefab will be instantiated under this Transform")]
    public Transform FilterRoot;

    [Tooltip("If true, Enables instantiated GameObject")]
    public bool IsEnableAfterInstantiatePrefab;

    public Vector3 InitialFilterPosition = Vector3.zero;
    public Quaternion InitialFilterRotation = Quaternion.identity;
    public Vector3 InitialFilterScale = Vector3.one;

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

            if (_filterInstance != null)
            {
                _filterInstance.transform.position = InitialFilterPosition;
                _filterInstance.transform.rotation = InitialFilterRotation;
                _filterInstance.transform.localScale = InitialFilterScale;

                if (IsEnableAfterInstantiatePrefab)
                {
                    _filterInstance.SetActive(true);
                }
            }
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

        AssetBundle assetBundle;
        if (IsLoadFromStreamingAssets)
        {
            var assetBundleName = $"{Application.streamingAssetsPath}/{AssetBundleName}";
            assetBundle = AssetBundle.LoadFromFile(assetBundleName);
            Errors.Add($"Loaded AssetBundle from path {assetBundleName}: {assetBundle}");
        }
        else
        {
            assetBundle = await NetworkAssets.GetAssetBundle(AssetBundleName).GetAsset();
            Errors.Add($"Loaded AssetBundle from CDN with name {AssetBundleName}: {assetBundle}");
        }

        // validate asset bundle
        if (assetBundle == null)
        {
            Errors.Add("No AssetBundle downloaded!");
            return;
        }

        // download prefab
        FilterPrefab = assetBundle.LoadAsset<UnityEngine.GameObject>(PrefabAssetName);

        if (FilterPrefab == null)
        {
            Errors.Add($"Filter Prefab has been not found in AssetBundle {AssetBundleName} by name {PrefabAssetName}");

            // list what we have in asset bundle
            var assetNames = assetBundle.GetAllAssetNames();
            if (assetNames == null)
            {
                Errors.Add($"No assets found in AssetBundle {AssetBundleName}");
            }
            else
            {
                for (int i = 0; i < assetNames.Length; i++)
                {
                    var assetName = assetNames[i];
                    var asset = assetBundle.LoadAsset(assetName);
                    var assetTypeName = "NULL";
                    if (asset != null)
                    {
                        assetTypeName = asset.GetType().Name;
                    }
                    Errors.Add($"Found Asset {assetName} ({assetTypeName})");
                }
            }
        }
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
