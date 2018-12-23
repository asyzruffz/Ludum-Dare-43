using UnityEngine;

public class PawnShape : MonoBehaviour {

    public bool randomizeColour;

    [SerializeField]
    Material headMaterial, bodyMaterial;

    [SerializeField]
    MeshRenderer headRender, bodyRender;

    void Start () {
        headRender.material = headMaterial;
        bodyRender.material = bodyMaterial;

        if (randomizeColour) {
            headRender.material.color = Random.ColorHSV ();
            bodyRender.material.color = Random.ColorHSV ();
        }
    }
}
