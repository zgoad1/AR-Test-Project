using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour {

    public Animator anim;

    public static Menu nextMenu;

    private static Stack<Menu> menuStack = new Stack<Menu>();

    private void Reset() {
        anim = GetComponent<Animator>();
    }

    private void Update() {
        if(Application.platform == RuntimePlatform.Android) {
            if(Input.GetKeyDown(KeyCode.Escape)) {
                Debug.Log("Escape pressed!");
                EnterPreviousMenu();
            }
        } else {
            if(Input.GetMouseButtonDown(1)) {
                Debug.Log("Escape pressed! (mouse)");
                EnterPreviousMenu();
            }
        }
    }

    private void OnEnable() {
        if(menuStack.Count == 0) {
            menuStack.Push(this);
        }
    }

    public static void EnterNewMenu(Menu newMenu) {
        menuStack.Peek().Exit();
        nextMenu = newMenu;
        menuStack.Push(newMenu);
    }

    public void EnterPreviousMenu() {
        menuStack.Pop();
        if(menuStack.Count > 0) {
            nextMenu = menuStack.Peek();
        }
        Exit();
    }

    public void Exit() {
        anim.SetBool("Active", false);
    }

    public void OnExit() {
        gameObject.SetActive(false);
        if(nextMenu) {
            nextMenu.gameObject.SetActive(true);
            nextMenu = null;
        } else {
            Debug.Log("Exiting application");
            Application.Quit(0);
        }
        //} else {
        //    menuStack.Pop();
        //    if(menuStack.Count > 0) {
        //        menuStack.Peek().gameObject.SetActive(true);
        //    } else {
        //        Debug.Log("Exiting application");
        //        Application.Quit(0);
        //    }
        //}
    }

    public static void ClearMenuStack() {
        menuStack.Clear();
    }

}
