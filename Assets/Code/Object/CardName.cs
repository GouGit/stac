using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardName : MonoBehaviour
{
    public string sortingLayerName;
    public int sortingOrder;
    private new string name;
    private TextMesh textMesh;

    void Start () 
    {
        MeshRenderer mesh = GetComponent<MeshRenderer> ();
        textMesh = GetComponent<TextMesh>();
        mesh.sortingLayerName = sortingLayerName;
        mesh.sortingOrder = sortingOrder;
        ShowCard show = transform.parent.GetComponent<ShowCard>();
        name = show.card.name;
        textMesh.text = name;
    }
}
