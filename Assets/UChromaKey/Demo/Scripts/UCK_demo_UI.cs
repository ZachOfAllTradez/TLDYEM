using UnityEngine;
using System.Collections;

public class UCK_demo_UI : MonoBehaviour {

	public UChromaKey uck;
	public Texture2D colorPicker;
	public Texture2D whiteSquare;
	public int colorPickerSize;
	public Material normalMaterial;
	public Material chromaKeyMaterial;

	public Renderer[] matObjects;

	private string[] deviceNames;
	private int selectedDevice;
	private int oldSelectedDevice;
	private int selectedMode;
	private int oldSelectedMode;
	private Color guiColor;

	private bool mouseFlag;

	private string[] modes = new string[2] {"Fullscreen","Material"};

	void OnGUI () 
	{
		GUILayout.BeginArea(new Rect (0,0,250,600));
		GUILayout.BeginVertical("box");
		GUILayout.Label("Select device:");
		if (deviceNames != null)
		{
			selectedDevice = GUILayout.SelectionGrid(selectedDevice,deviceNames,1,"toggle");
		}
		GUILayout.EndVertical();
		GUILayout.BeginVertical("box");
		GUILayout.Label("Select mode:");
		if (deviceNames != null)
		{
			selectedMode = GUILayout.SelectionGrid(selectedMode,modes,1,"toggle");
		}
		GUILayout.EndVertical();
		GUILayout.BeginVertical("box");
		uck.flipHorizontal = GUILayout.Toggle(uck.flipHorizontal,"Horizontal flip");
		uck.flipVertical = GUILayout.Toggle(uck.flipVertical,"Vertical flip");
		GUILayout.EndVertical();
		GUILayout.BeginVertical("box");
		GUILayout.Label("Key color selection:");
		GUI.color = uck.SelectedColor;
		GUILayout.Label(whiteSquare);
		GUI.color = guiColor;
		Rect lastRect = GUILayoutUtility.GetLastRect();
		if (GUILayout.RepeatButton(colorPicker,"label",GUILayout.Width(colorPickerSize),GUILayout.Height(colorPickerSize)))
		{
			Vector2 pickpos  = Event.current.mousePosition;
			float aaa  = pickpos.x;
			float bbb  =  pickpos.y - lastRect.yMax;
			int aaa2  = (int)(aaa * (colorPicker.width / (colorPickerSize + 0.0f)));
			int bbb2  =  (int)((colorPickerSize - bbb) * (colorPicker.height / (colorPickerSize + 0.0f)));
			uck.SelectedColor  = colorPicker.GetPixel(aaa2, bbb2);
		}

		GUILayout.BeginHorizontal();
		GUILayout.Label("Range:    ");
		uck.Range = GUILayout.HorizontalSlider(uck.Range, 0.0f, 2.83f,GUILayout.Width(150));
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal();
		GUILayout.Label("Hue Range:");
		uck.HueRange = GUILayout.HorizontalSlider(uck.HueRange, 0.0f, 5.0f,GUILayout.Width(150));
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal();
		GUILayout.Label("Opacity:");
		uck.opacity = GUILayout.HorizontalSlider(uck.opacity, 0.0f, 1.0f,GUILayout.Width(150));
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal();
		GUILayout.Label("Edge sharp:");
		uck.edgeSharpness = GUILayout.HorizontalSlider(uck.edgeSharpness, 1.0f, 20.0f,GUILayout.Width(150));
		GUILayout.EndHorizontal();

		GUILayout.EndVertical();
		GUILayout.EndArea();



	}


	// Use this for initialization
	void Start () 
	{
		guiColor = GUI.color;
		deviceNames = new string[1];
		deviceNames[0] = "Unity RenderTexture";
		uck.SourceType = UChromaKey.ChromaKeySource.Texture;
	}

	
	// Update is called once per frame
	void Update () 
	{
		if (deviceNames.Length == 1 && Application.HasUserAuthorization(UserAuthorization.WebCam)) 	
		{
			WebCamDevice[] devices = WebCamTexture.devices;
			deviceNames = new string[devices.Length+1];
			deviceNames[0] = "Unity RenderTexture";
			for (int i = 0; i < devices.Length; i++)
				deviceNames[i+1] = devices[i].name;
		}
		if (selectedMode == 1)
		{
			chromaKeyMaterial.SetColor("_CKCol",uck.SelectedColor);
			chromaKeyMaterial.SetFloat("_Range",uck.Range);
			chromaKeyMaterial.SetFloat("_HueRange",uck.HueRange);
			chromaKeyMaterial.SetFloat("_EdgeSharp",uck.edgeSharpness);
			chromaKeyMaterial.SetFloat("_Opacity",uck.opacity);
		}
		if (oldSelectedDevice != selectedDevice)
		{
			if (selectedDevice != 0)
			{
				uck.SourceType = UChromaKey.ChromaKeySource.Device;
				uck.DeviceName = deviceNames[selectedDevice];
			}
			else
			{
				uck.SourceType = UChromaKey.ChromaKeySource.Texture;
			}
			oldSelectedDevice = selectedDevice;
		}
		if (oldSelectedMode != selectedMode)
		{
			switch (selectedMode)
			{
			case 1:
				uck.imageEffect = false;
				foreach(Renderer rend in matObjects)
					rend.material = chromaKeyMaterial;
				break;
			case 0:
				uck.imageEffect = true;
				foreach(Renderer rend in matObjects)
					rend.material = normalMaterial;
				break;
			}
			oldSelectedMode = selectedMode;
		}
	}
}
