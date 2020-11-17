using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.ResourceLocations;

public static class AddressableLocationLoader {

    public static async Task GetAllByLabel(string label, IList<IResourceLocation> loadedLocations) {

        var unloadedLocations = await Addressables.LoadResourceLocationsAsync(label).Task;

        foreach(var location in unloadedLocations) {
            loadedLocations.Add(location);
        }
    }

}
