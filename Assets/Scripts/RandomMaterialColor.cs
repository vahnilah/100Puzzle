using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RandomMaterialColor : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Object[] allColoredMaterials = Resources.LoadAll("LightColor", typeof(Material));
        var randomColor = allColoredMaterials[Random.Range(0, allColoredMaterials.Length)];

        // Get the MeshRenderer of each gameblock within a gamepiece
        MeshRenderer[] allColoredBlocks = this.gameObject.GetComponentsInChildren<MeshRenderer>();

        // Set a random color to all the gameblocks
        foreach (var block in allColoredBlocks) {
            Material[] meshMats = block.materials;
            meshMats[0] = (Material)randomColor;
            block.materials = meshMats;
        }
    }
}
