using UnityEngine;
using System.Collections;

[AddComponentMenu("Image Effects/UChromaKey")]
public class UChromaKey : MonoBehaviour {

	public bool imageEffect = true;
	public Color SelectedColor;
	public float Range = 0.1f;
	public float HueRange = 0.1f;
	public float opacity = 1;
	public float edgeSharpness = 20;
	public bool shaderModel2 = false;
	public Vector2 uvShift = Vector2.zero;
	public Vector2 uvCoef = Vector2.one;
	public bool flipHorizontal;
	public bool flipVertical;

	public bool autoSetDevice;


	public Texture chromaKeyTexture;

	private bool oldShaderModel;
	private Material curMaterial;
	public WebCamTexture webCamTexture;
	[SerializeField]
	private string devName;
	public bool devicesExist = false;

	[SerializeField]
	private ChromaKeySource srcType;


	public enum ChromaKeySource
	{
		Device = 0,
		Texture = 1
	}
	
	Material material
	{
		get
		{
			if(curMaterial == null || oldShaderModel != shaderModel2)
			{
				if(curMaterial != null)
				{
					DestroyImmediate(curMaterial);
				}
				if (shaderModel2)
					curMaterial = new Material(Shader.Find("Hidden/UChromaKey_mobile"));
				else
					curMaterial = new Material(Shader.Find("Hidden/UChromaKey"));
				curMaterial.hideFlags = HideFlags.HideAndDontSave;	
			}
			return curMaterial;
		}
	}

	public string DeviceName
	{
		get
		{
			return devName;
		}
		set
		{
			if (value != devName)
			{
				devName = value;
				if (srcType == ChromaKeySource.Device && Application.isPlaying)
					SetTexture();	
			}
		}
	}

	public ChromaKeySource SourceType
	{
		get
		{
			return srcType;
		}
		set
		{
			if (value != srcType)
			{
				srcType = value;
				if (srcType == ChromaKeySource.Texture && Application.isPlaying && webCamTexture != null && webCamTexture.isPlaying)
					webCamTexture.Stop();
				if (srcType == ChromaKeySource.Device && Application.isPlaying)
				{
					SetTexture();
					if (!webCamTexture.isPlaying)
						webCamTexture.Play();
				}
			}
		}
	}

	[ContextMenu("SetFirstDevice")]
	public void SetFirstDevice()
	{
		WebCamDevice[] devices = WebCamTexture.devices;
		if (devices.Length > 0)
			DeviceName = devices[0].name;
		else
			Debug.LogError("No devices found.");
	}



	public void SetTexture()
	{
		if (webCamTexture != null && webCamTexture.isPlaying)
		{
			webCamTexture.Stop();
		}
		if (webCamTexture != null)
			webCamTexture.deviceName = devName;
		else
			webCamTexture = new WebCamTexture(devName);
		webCamTexture.Play();
	}


	void OnRenderImage (RenderTexture sourceTexture, RenderTexture destTexture) //sourceTexture is the source texture, destTexture is the final image that gets to the screen
	{
		if (imageEffect)
		{
			material.SetColor("_PatCol",SelectedColor);
			material.SetFloat("_Range", Range);
			if (!shaderModel2)
				material.SetFloat("_HueRange", HueRange);
			material.SetFloat("_opacity",opacity);
			material.SetFloat("_smoothing",edgeSharpness);

			Graphics.Blit(sourceTexture, destTexture, curMaterial);
			oldShaderModel = shaderModel2;
		}
		else
		{
			Graphics.Blit(sourceTexture,destTexture);
		}
	}

	// Use this for initialization
	IEnumerator Start () {
		if(!SystemInfo.supportsImageEffects)
		{
			enabled = false;
			return false;
		}
		devicesExist = true;
		oldShaderModel = shaderModel2;
		if( Application.platform == RuntimePlatform.OSXWebPlayer
		   || Application.platform == RuntimePlatform.WindowsWebPlayer )
		{
			yield return Application.RequestUserAuthorization(UserAuthorization.WebCam);
			if (!Application.HasUserAuthorization(UserAuthorization.WebCam)) 						
			{
				devicesExist = false;
				return false;
			}
		}
		WebCamDevice[] devices = WebCamTexture.devices;
		if (devices.Length == 0)
			devicesExist = false;
		if (SourceType == ChromaKeySource.Device && autoSetDevice && devicesExist)
		{
			DeviceName = devices[0].name;
		}
	}

	void Update () 
	{
		switch (srcType)
		{
		case ChromaKeySource.Device:
			Shader.SetGlobalTexture("_UChromaKeyTex", webCamTexture);
			break;
		case ChromaKeySource.Texture:
			Shader.SetGlobalTexture("_UChromaKeyTex", chromaKeyTexture);
			break;
		}

		if (flipHorizontal)
		{
			Shader.SetGlobalFloat("_uvDefX",1.0f + uvShift.x);
			Shader.SetGlobalFloat("_uvCoefX",-1.0f / uvCoef.x);
		}
		else
		{
			Shader.SetGlobalFloat("_uvDefX",0.0f - uvShift.x);
			Shader.SetGlobalFloat("_uvCoefX",1.0f / uvCoef.x);
		}
		if (flipVertical)
		{
			Shader.SetGlobalFloat("_uvDefY",1.0f + uvShift.y);
			Shader.SetGlobalFloat("_uvCoefY",-1.0f / uvCoef.y);
		}
		else
		{
			Shader.SetGlobalFloat("_uvDefY",0.0f - uvShift.y);
			Shader.SetGlobalFloat("_uvCoefY",1.0f / uvCoef.y);
		}
	}


	void OnEnable ()
	{
		if (SourceType == ChromaKeySource.Device)
			SetTexture();
	}

	//When we disable or delete the effect.....
	void OnDisable ()
	{
		if(curMaterial != null)
		{
			DestroyImmediate(curMaterial);	//Destroys the material when not used so it won't cause leaks
		}
		if (webCamTexture != null)
		{
			if (webCamTexture.isPlaying)
				webCamTexture.Stop();
			DestroyImmediate(webCamTexture);
		}
	}

}
