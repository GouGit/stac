using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using UnityEngine.EventSystems;

public class RoutePicker : MonoBehaviour, IPointerClickHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Image image;
    public Traveler traveler;
    public Spot spot;
    public List<Sprite> spirteList;

    public void SetOption(Traveler traveler, Spot spot)
    {
        this.traveler = traveler;
        this.spot = spot;
        
    }

    void Start()
    {
        image.sprite = spirteList[(int)spot.sceneOption.type];
        transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        for(int i = 0; i < transform.parent.childCount; i++)
        {
            Destroy(transform.parent.GetChild(i).gameObject);
        }

        SceneLoader.LoadScene("TestScene", spot.sceneOption);

        // traveler.ChangeSpot(spot);    
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        
    }

    public void OnPointerEnter(PointerEventData eventData)
    {

    }

    public void OnPointerExit(PointerEventData eventData)
    {

    }
}
