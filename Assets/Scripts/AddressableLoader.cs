using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ResourceManagement;
using UnityEngine.AddressableAssets;

public class AddressableLoader : MonoBehaviour {

    public Vector3 position;
    public Quaternion rotation;

    private void Start() {
        Debug.Log("ADDRESSABLE LOADER SPAWNED");
    }

    public void LoadAsset(string name, Vector3 position, Quaternion rotation) {
        try {
            Addressables.LoadAssetAsync<GameObject>(name).Completed += OnLoadDone;
        } catch {
            Debug.LogError("ERROR - Failed to start async task!");
        }
        this.position = position;
        this.rotation = rotation;
    }

    private void OnLoadDone(UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle<GameObject> obj) {
        try {
            obj.Result.transform.position = position;
            obj.Result.transform.rotation = rotation;
        } catch {
            Debug.LogError("ERROR - Spawned a null object!");
        }
    }


}
