using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CompareLayer
{
    
    //layermask == (layermask | (1 << layer))
    public static bool IsLayerInLayerMask(int layer, LayerMask layerMask)
    {
        return layerMask == (layerMask | (1 << layer));
    }
}
