using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor( typeof(UChromaKey))]
public class UChromaKey_editor : Editor {

	public override void OnInspectorGUI()
	{
		UChromaKey uck = (UChromaKey)target;

		EditorGUILayout.BeginVertical("box");
		uck.SourceType = (UChromaKey.ChromaKeySource)EditorGUILayout.EnumPopup("Chroma Key source",uck.SourceType);
		
		switch (uck.SourceType)
		{
		case UChromaKey.ChromaKeySource.Device:
			int deviceInd = 0;
			WebCamDevice[] devices = WebCamTexture.devices;
			string[] deviceNames = new string[devices.Length];
			for (int i = 0; i < devices.Length; i++)
			{
				deviceNames[i] = devices[i].name;
				if (deviceNames[i] == uck.DeviceName)
					deviceInd = i;
			}
			if (!(uck.autoSetDevice = EditorGUILayout.ToggleLeft("Auto-set first avaliable device",uck.autoSetDevice)))
			{
				deviceInd = EditorGUILayout.Popup("Device", deviceInd, deviceNames); 
				uck.DeviceName = devices[deviceInd].name;
			}
			break;
		case UChromaKey.ChromaKeySource.Texture:
			uck.chromaKeyTexture = (Texture) EditorGUILayout.ObjectField("Texture", uck.chromaKeyTexture, typeof (Texture), false);
			break;
		}
		EditorGUILayout.EndVertical();

		EditorGUILayout.BeginVertical("box");
		if (uck.imageEffect = EditorGUILayout.ToggleLeft("Use fullscreen image effect",uck.imageEffect))
		{
			uck.SelectedColor = EditorGUILayout.ColorField("Chroma key color",uck.SelectedColor);
			uck.shaderModel2 = EditorGUILayout.ToggleLeft("Use shader model 2.0",uck.shaderModel2);
			uck.Range = EditorGUILayout.Slider("RGB Range",uck.Range,0,2.83f);
			if (!uck.shaderModel2)		
				uck.HueRange = EditorGUILayout.Slider("Hue Range",uck.HueRange,0,5);		
			uck.opacity = EditorGUILayout.Slider("Opacity",uck.opacity,0,1);
			uck.edgeSharpness = EditorGUILayout.Slider("Edge sharpness",uck.edgeSharpness,1,20);
		}
		EditorGUILayout.EndVertical();

		EditorGUILayout.BeginVertical("box");
		uck.flipHorizontal = EditorGUILayout.ToggleLeft("Flip horizontally",uck.flipHorizontal);
		uck.flipVertical = EditorGUILayout.ToggleLeft("Flip vertically",uck.flipVertical);
		uck.uvShift = EditorGUILayout.Vector2Field("Screen shift",uck.uvShift);
		uck.uvCoef = EditorGUILayout.Vector2Field("Screen multiplier",uck.uvCoef);
		EditorGUILayout.EndVertical();
	}

}
