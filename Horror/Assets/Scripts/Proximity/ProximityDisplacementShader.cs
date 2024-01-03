using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum Axis
{
    X,
    Y,
    Z
}

[ExecuteInEditMode]
public class ProximityDisplacementShader : MonoBehaviour
{
    private MaterialPropertyBlock mat_prop_block;
    private Vector3 player_position;
    private MeshRenderer mesh_renderer;
    private Material material;
    
    [SerializeField] private Color shader_color;
    [SerializeField] private float displacement_strength = 8f;
    [SerializeField] private Axis axis;
    
    // Start is called before the first frame update
    void Awake()
    {
        Debug.Log("AWAKE CALLED IN EDITOR");
        mesh_renderer = GetComponentInChildren<MeshRenderer>();
        mat_prop_block = new MaterialPropertyBlock();
        mesh_renderer.GetPropertyBlock(mat_prop_block);
        
        material = mesh_renderer.sharedMaterial;

        // in the editor, the player instance is not assigned and is null
        try
        {
            player_position = PlayerInstance.GetCameraPosition();
        }
        catch (Exception _e)
        {
            player_position = Vector3.zero;
        }
        
        mat_prop_block.SetVector("_playerPosition", player_position);
        mat_prop_block.SetFloat("_strength", displacement_strength);
        mat_prop_block.SetColor("_Color", shader_color);
        // get ID of property _DISPLACEMENTAXIS
        int displacement_axis_id = Shader.PropertyToID("_DISPLACEMENTAXIS");
        // set value of property _DISPLACEMENTAXIS
        switch (axis)
        {
            case Axis.X:
                mat_prop_block.SetFloat(displacement_axis_id, 0);
                break;
            case Axis.Y:
                mat_prop_block.SetFloat(displacement_axis_id, 1);
                break;
            case Axis.Z:
                mat_prop_block.SetFloat(displacement_axis_id, 2);
                break;
        }
        
        mesh_renderer.SetPropertyBlock(mat_prop_block);
    }

    // Update is called once per frame
    // execute only in play mode
    
    void Update()
    {
            mat_prop_block = new MaterialPropertyBlock();
            mesh_renderer.GetPropertyBlock(mat_prop_block);
        
            try
            {
                player_position = PlayerInstance.GetCameraPosition();
            }
            catch (Exception e)
            {
                player_position = Vector3.zero;
            }
        
            mat_prop_block.SetVector("_playerPosition", player_position);

#if UNITY_EDITOR
            mat_prop_block.SetColor("_Color", shader_color);

            switch (axis)
            {
                case Axis.X:
                    mat_prop_block.SetFloat("_DISPLACEMENTAXIS", 0f);
                    break;
                case Axis.Y:
                    mat_prop_block.SetFloat("_DISPLACEMENTAXIS", 1f);
                    break;
                case Axis.Z:
                    mat_prop_block.SetFloat("_DISPLACEMENTAXIS", 2f);
                    break;
            }
#endif
            
            mesh_renderer.SetPropertyBlock(mat_prop_block);
    }
}
