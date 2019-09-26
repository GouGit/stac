using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardText : MonoBehaviour
{
    public string sortingLayerName;
    public int sortingOrder;
    private string cost;
    private TextMesh textMesh;

    void Start () 
    {
        MeshRenderer mesh = GetComponent<MeshRenderer> ();
        textMesh = GetComponent<TextMesh>();
        mesh.sortingLayerName = sortingLayerName;
        mesh.sortingOrder = sortingOrder;
        ShowCard show = transform.parent.GetComponent<ShowCard>();
        if(show.level > 0)
            textMesh.text = "+" + show.level;
        else
        {
            textMesh.text = "";
        }
        transform.position += Vector3.back;
    }
}
