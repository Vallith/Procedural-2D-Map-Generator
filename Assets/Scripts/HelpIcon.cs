using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class HelpIcon : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public string tooltipText;
    public GameObject tooltipPrefab;
    Transform tooltipLocation;
    GameObject instantiatedPrefab;

    private void Awake()
    {
        tooltipLocation = transform.GetChild(0).transform;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        instantiatedPrefab = Instantiate(tooltipPrefab, transform);
        instantiatedPrefab.transform.SetParent(GameObject.Find("Menu").transform);
        instantiatedPrefab.transform.position = tooltipLocation.position;
        instantiatedPrefab.GetComponentInChildren<TMP_InputField>().text = tooltipText;
        Animation anim = instantiatedPrefab.GetComponent<Animation>();
        anim.Play();

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Animation anim = instantiatedPrefab.GetComponent<Animation>();
        anim[anim.clip.name].time = anim.clip.length;
        anim[anim.clip.name].speed = -1f;
        anim.Play();
        Destroy(instantiatedPrefab, anim.clip.length);
    }
}
