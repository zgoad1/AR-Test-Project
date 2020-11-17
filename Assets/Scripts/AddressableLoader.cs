using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ResourceManagement;
using UnityEngine.AddressableAssets;
using System.Threading.Tasks;

public class AddressableLoader : MonoBehaviour {

    public Vector3 position;
    public Quaternion rotation;

    [SerializeField] private string _label;

    private void Start() {
        Debug.Log("ADDRESSABLE LOADER SPAWNED");
        Get(_label);
    }

    public void LoadAsset(string name, Vector3 position, Quaternion rotation) {
        //Addressables.LoadAssetAsync<GameObject>(name).Completed += OnLoadDone;
        try {
            Debug.Log("STARTING LOAD ASSET");
            Addressables.LoadAssetAsync<GameObject>(name).Completed += OnLoadDone;
        } catch {
            Debug.LogError("ERROR - Failed to start async task!");
        }
        this.position = position;
        this.rotation = rotation;
    }

    private void OnLoadDone(UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle<GameObject> obj) {
        try {
            Debug.Log("FINISHED LOAD ASSET");
            obj.Result.transform.position = position;
            obj.Result.transform.rotation = rotation;
        } catch {
            Debug.LogError("ERROR - Spawned a null object!");
        }
    }

    private async Task Get(string label) {
        var locations = await Addressables.LoadResourceLocationsAsync(label).Task;
        foreach(var location in locations) {
            await Addressables.InstantiateAsync(location).Task;
        }
    }


}
