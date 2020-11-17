using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using System.Threading.Tasks;
using System;

public class CreatedAssets : MonoBehaviour {

    private LoadedAddressableLocations _loadedLocations;

    [SerializeField] private List<GameObject> Assets { get; } = new List<GameObject>();

    private void Start() {
        CreateAndWaitUntilCompleted();
    }

    private async Task CreateAndWaitUntilCompleted() {
        _loadedLocations = GetComponent<LoadedAddressableLocations>();
        await Task.Delay(TimeSpan.FromSeconds(1));  // To ensure LoadedAddressableLocations has had time to initialize if we're calling from Start()
                                                    // (or we could just change it in the script execution order, but this is what he did)
        await CreateAddressablesLoader.ByLoadedAddress(_loadedLocations.AssetLocations, Assets);

        foreach(var asset in Assets) {
            Debug.Log(asset.name);
        }
    }

}
