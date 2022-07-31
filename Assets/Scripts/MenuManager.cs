using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    bool isMenuOpen = true;
    public void ToggleMenu()
    {
        Animation anim = GetComponent<Animation>();
        foreach (AnimationState item in anim)
        {
            if (item.name == "PanelOpen" && !isMenuOpen)
            {
                anim.clip = item.clip;
                anim.Play();
                isMenuOpen = true;
                return;
            }
            else if(item.name == "PanelClose" && isMenuOpen)
            {
                anim.clip = item.clip;
                anim.Play();
                isMenuOpen = false;
                return;
            }
        }

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
