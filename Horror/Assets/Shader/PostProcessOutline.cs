
using System;
using System.IO;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

[Serializable]
[PostProcess(typeof(PostProcessOutlineRenderer), PostProcessEvent.BeforeStack, "Post Process Outline")]
public sealed class PostProcessOutline : PostProcessEffectSettings
{
    // Add to the PostProcessOutline class.
    public IntParameter scale = new IntParameter { value = 1 };
    public FloatParameter lowCutOff = new FloatParameter { value = 0.08f };
    public FloatParameter fadeOutPower = new FloatParameter { value = 1.7f };
    public FloatParameter fadeOutDistance = new FloatParameter { value = 2000f };
    public FloatParameter brightnessClamp = new FloatParameter { value = 0.5f };
    public FloatParameter brightnessScale = new FloatParameter { value = 2f };
}

public sealed class PostProcessOutlineRenderer : PostProcessEffectRenderer<PostProcessOutline>
{
    public override void Render(PostProcessRenderContext context)
    {
        var sheet = context.propertySheets.Get(Shader.Find("Hidden/Outline Post Process"));
        // Add to the Render method in the PostProcessOutlineRenderer class, just below var sheet declaration.
        sheet.properties.SetFloat("_Scale", settings.scale);
        Matrix4x4 clipToView = GL.GetGPUProjectionMatrix(context.camera.projectionMatrix, true).inverse;
        sheet.properties.SetMatrix("_ClipToView", clipToView);
        context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
        sheet.properties.SetFloat("_LowCutOff", settings.lowCutOff);
        sheet.properties.SetFloat("_FadeOutPower", settings.fadeOutPower);
        sheet.properties.SetFloat("_FadeOutDistance", settings.fadeOutDistance);
        sheet.properties.SetFloat("_BrightnessClamp", settings.brightnessClamp);
        sheet.properties.SetFloat("_BrightnessScale", settings.brightnessScale);
    }
}