using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuSelectButton : MonoBehaviour {

    public Menu myMenu;         // The menu this button is part of
    public Menu selectedMenu;   // The menu this button selects

    public void OnClick() {
        Menu.EnterNewMenu(selectedMenu);
    }

}
