using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

[RequireComponent(typeof(BoxCollider))]
public class PokemonSpawner : MonoBehaviour {

    public string[] names;
    public GameObject[] prefabs;
    public Animator anim;

    public static List<PokemonSpawner> trackedCards = new List<PokemonSpawner>();
    private static PokemonSpawner activeCard;
    public static PokemonSpawner ActiveCard {
        get {
            return activeCard;
        }
        set {
            activeCard = value;
            foreach(PokemonSpawner s in trackedCards) {
                if(activeCard == null) {
                    Debug.Log("Setting activeCard to null, enabling all animators");
                    s.anim.SetBool("Enabled", true);
                } else if(s != activeCard) {
                    s.anim.SetBool("Enabled", false);
                    Debug.Log("(ActiveCard Set) " + s.pokemonName + " card animator: Enabled: " + s.anim.GetBool("Enabled"));
                }
            }
        }
    }

    private string pokemonName;
    private static Dictionary<string, GameObject> recognizedPokemon;
    private bool pokemonOut = false;
    private ARNavigator myPoke;
    private new BoxCollider collider;
    private bool tracking = false;
    private bool initialized = false;

    private void Reset() {
        anim = GetComponentInChildren<Animator>();
    }

    private void OnEnable() {
        if(recognizedPokemon == null) {
            recognizedPokemon = new Dictionary<string, GameObject>();
            try {
                int i;
                for(i = 0; i < names.Length; i++) {
                    recognizedPokemon.Add(names[i], prefabs[i]);
                }
                if(i < prefabs.Length) {
                    Debug.LogError("Prefabs list longer than names list", gameObject);
                }
            } catch {
                Debug.LogError("Adding keys and values to dictionary failed. Are the arrays the same length?");
            }
        }
        collider = GetComponent<BoxCollider>();
        collider.enabled = false;
        anim.SetBool("Enabled", !(ActiveCard && ActiveCard != this));
        Debug.Log("(OnEnable) " + pokemonName + " card animator: Enabled: " + anim.GetBool("Enabled"));
    }

    private void Update() {
        if(initialized) {
            if(GetComponent<ARTrackedImage>().trackingState == UnityEngine.XR.ARSubsystems.TrackingState.Tracking) {
                if(!tracking) {
                    Debug.Log("Found card again! " + pokemonName);
                    Activate(pokemonName);
                }
            }
        }
    }

    public void Activate(string name) {
        if(!initialized) {
            trackedCards.Add(this);
        }
        initialized = true;
        tracking = true;
        pokemonName = name;
        StartCoroutine(ActivateCR());
    }

    public void OnTouch() {
        if(pokemonOut) {
            ARNavigator.Exit(transform.position);
        } else {
            if(ActiveCard == null) SpawnPokemon();
        }
    }

    public void SpawnPokemon() {
        if(myPoke != null) {
            myPoke.gameObject.SetActive(true);
            anim.SetBool("Contains Pokemon", false);
            pokemonOut = true;
            ActiveCard = this;
            return;
        }
        try {
            Debug.Log("Attempting to spawn " + pokemonName + " at " + transform.position);
            Transform newPoke = Instantiate(recognizedPokemon[pokemonName], transform).transform;

            ARAnchorManager manager = FindObjectOfType<ARAnchorManager>();
            ARAnchor newAnchor = manager.AddAnchor(new Pose(transform.position, transform.rotation));
            newPoke.parent = newAnchor.transform;
            newPoke.localPosition = Vector3.zero;
            newPoke.localRotation = Quaternion.identity;
            myPoke = newPoke.GetComponent<ARNavigator>();
            myPoke.mySpawner = this;
            anim.SetBool("Contains Pokemon", false);
            pokemonOut = true;
            ActiveCard = this;
        } catch {
            Debug.LogError("Card has invalid name: " + pokemonName);
        }
    }

    private IEnumerator ActivateCR() {

        while(GetComponent<ARTrackedImage>().trackingState != UnityEngine.XR.ARSubsystems.TrackingState.Tracking) {
            Debug.Log("Not tracking card yet, waiting");
            yield return null;
        }
        Debug.Log("Tracking card!");
        anim.SetBool("Visible", true);
        collider.enabled = true;

        while(GetComponent<ARTrackedImage>().trackingState == UnityEngine.XR.ARSubsystems.TrackingState.Tracking) {
            yield return null;
        }
        Debug.Log("Lost tracking on card!");
        anim.SetBool("Visible", false);
        collider.enabled = false;
        tracking = false;
    }

    public void OnPokemonRecalled() {
        Debug.Log(pokemonName + " recalled!");
        anim.SetBool("Contains Pokemon", true);
        pokemonOut = false;
        ActiveCard = null;
    }
}
