using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class HighlightLevel : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler  //highlight level text
{
    public TextMeshProUGUI levelText;                   //get the assigned text 
    public void Start()                                //disable it 
    {
        levelText.enabled = false; 
    }
    public void OnPointerEnter(PointerEventData enterData) //whenever the mouse is over the level
    {
        levelText.enabled = true;                          //display it
    }

    public void OnPointerExit(PointerEventData exitData)   //when it leaves the button
    {
        levelText.enabled = false;                         //hide it
    }
}
