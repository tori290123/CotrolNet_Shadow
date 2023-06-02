using UnityEngine;
using System.IO;
using System.Collections;

public class Camera_Capture : MonoBehaviour
{
    public GameObject ModelCamera;
    public GameObject LightCamera;
    public GameObject DirectionalLight;
    public GameObject model;
    public int captureW;
    public int captureH;

    public bool IsLoading;
    public Vector3 Pos;
    public string outpath;
    public string modelpath;
    public int count;
    public int max_count;
    public int model_count;

    private bool isCapturing; // �B�e�����ǂ������Ǘ�����t���O

    private void Start()
    {
        count = 0;
        model_count = Directory.GetFiles(modelpath, "*", SearchOption.TopDirectoryOnly).Length/2;
        isCapturing = false; // �B�e�t���O��������

        string targetFolderPath = Path.Combine(outpath, "target");
        if (!Directory.Exists(targetFolderPath))
        {
            Directory.CreateDirectory(targetFolderPath);
        }

        string lightFolderPath = Path.Combine(outpath, "light");
        if (!Directory.Exists(lightFolderPath))
        {
            Directory.CreateDirectory(lightFolderPath);
        }
        StartCapture();
    }

    private void StartCapture()
    {
        if (!isCapturing)
        {
            StartCoroutine(RandomCapture());
        }
    }

    bool ModelLoad(string model_name)
    {
        GameObject modelPrefab = Resources.Load<GameObject>(model_name);

        model = Instantiate(modelPrefab);

        //model�̍��W���Y���鎞�̏C���p�B�ʒu�����������Ȃ�����R�����g�A�E�g���Ă݂�
        foreach (Transform childTransform in model.transform)
        {
            if (childTransform.gameObject.name != "default.default")
            {
                var child = childTransform.gameObject;
                Pos = child.GetComponent<Renderer>().bounds.center;
                float x = -Pos.x;
                float y = -Pos.y;
                float z = -Pos.z;
                model.transform.Translate(x, y, z);
            }
            if (model.transform.childCount == 1)
            {
                var child = childTransform.gameObject;
                Pos = child.GetComponent<Renderer>().bounds.center;
                float x = -Pos.x;
                float y = -Pos.y;
                float z = -Pos.z;
                model.transform.Translate(x, y, z);
            }
        }
        IsLoading = true ;
        return IsLoading;
    }

    private IEnumerator RandomCapture()
    {
        isCapturing = true; // �B�e�t���O��ݒ�

        while (count < max_count)
        {
            count++; // count �̒l���C���N�������g

            int model_rnd = UnityEngine.Random.Range(0, model_count);
            string model_name = model_rnd.ToString("0000");

            yield return new WaitUntil(() => ModelLoad(model_name) == true); // ���f���̃��[�h����������܂őҋ@
                                                                              // ���f���̃��[�h������������ɍs�������������ɋL�q����
                                                                              // �����_�������y�у����_���J�����z�u
            float rot_rnd = UnityEngine.Random.Range(-360, 360);
            ModelCamera.transform.RotateAround(Pos, Vector3.up, rot_rnd);
            DirectionalLight.transform.eulerAngles = new Vector3(rot_rnd, 90, 90);


            // �����ŎB�e���s��
            CameraSavePng_Model(count.ToString("00000"));
            CameraSavePng_Light(count.ToString("00000"));

            // ���f���̎B�e������������GameObject��j������
            Destroy(model);

            IsLoading = false;
            if (count >= max_count)
            {
                UnityEditor.EditorApplication.isPlaying = false;
            }
        }

        isCapturing = false; // �B�e�t���O������
    }

    private void CameraSavePng_Model(string count)
    {
        Camera camera = ModelCamera.GetComponent<Camera>();

        RenderTexture rt = new RenderTexture(captureW, captureH, 24);
        RenderTexture prev = camera.targetTexture;
        camera.targetTexture = rt;
        camera.Render();
        camera.targetTexture = prev;
        RenderTexture.active = rt;

        Texture2D capture = new Texture2D(captureW, captureH, TextureFormat.ARGB32, false);
        capture.ReadPixels(new Rect(0, 0, capture.width, capture.height), 0, 0);
        capture.Apply();
        byte[] bytes = capture.EncodeToPNG();
        string path = Path.Combine(outpath, "target", count + ".png");
        if (string.IsNullOrEmpty(path))
        {
            return;
        }
        File.WriteAllBytes(path, bytes);

        RenderTexture.active = null;
        camera.targetTexture = null;
        Destroy(rt);
    }

    private void CameraSavePng_Light(string count)
    {
        Camera camera = LightCamera.GetComponent<Camera>();

        RenderTexture rt = new RenderTexture(captureW, captureH, 24);
        RenderTexture prev = camera.targetTexture;
        camera.targetTexture = rt;
        camera.Render();
        camera.targetTexture = prev;
        RenderTexture.active = rt;

        Texture2D capture = new Texture2D(captureW, captureH, TextureFormat.ARGB32, false);
        capture.ReadPixels(new Rect(0, 0, capture.width, capture.height), 0, 0);
        capture.Apply();
        byte[] bytes = capture.EncodeToPNG();
        string path = Path.Combine(outpath, "light", count + ".png");
        if (string.IsNullOrEmpty(path))
        {
            return;
        }
        File.WriteAllBytes(path, bytes);

        RenderTexture.active = null;
        camera.targetTexture = null;
        Destroy(rt);
    }
}