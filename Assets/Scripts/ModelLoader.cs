using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TriLib �̎Q��
using TriLibCore;

public class ModelLoader : MonoBehaviour
{
    public Camera Camera;
    public GameObject loadedGameObject;
    public Vector3 playerPos;

    public void FileLoad(string filepath)
    {
        if (loadedGameObject != null)
        {
            Destroy(loadedGameObject.gameObject);
        }
        // ���[�h�̐ݒ�����쐬����i����̓f�t�H���g�ݒ�j
        AssetLoaderOptions assetLoaderOptions = AssetLoader.CreateDefaultLoaderOptions();
        // �t�@�C���V�X�e������̃��[�h�����s����
        AssetLoader.LoadModelFromFile(filepath, OnLoad, OnMaterialsLoad, OnProgress, OnError, null, assetLoaderOptions);
    }

    /// <summary>
    /// <summary>
    /// ���f���̓ǂݍ��݂̐i�s�󋵂��ω������Ƃ��ɌĂяo�����C�x���g
    /// </summary>
    /// <param name="assetLoaderContext"></param>
    /// <param name="progress"></param>
    private void OnProgress(AssetLoaderContext assetLoaderContext, float progress)
    {
    }

    /// <summary>
    /// �G���[�������ɌĂяo�����C�x���g
    /// </summary>
    /// <param name="contextualizedError"></param>
    private void OnError(IContextualizedError contextualizedError)
    {
    }

    /// <summary>
    /// �S�Ă� GameObject ���ǂݍ��܂ꂽ�Ƃ��ɌĂяo�����C�x���g
    /// </summary>
    /// <param name="assetLoaderContext"></param>
    private void OnLoad(AssetLoaderContext assetLoaderContext)
    {
        // ���[�h���ꂽGameObject���擾����
        GameObject loadedGameObject = assetLoaderContext.RootGameObject;

        // ���̎��_�ł̓}�e���A���ƃe�N�X�`���̓ǂݍ��݂��������Ă��Ȃ��\�������邽��
        // �I�u�W�F�N�g���\���ɂ��Ă���
        loadedGameObject.SetActive(false);
    }

    /// <summary>
    /// �}�e���A���ƃe�N�X�`�����܂ޑS�Ă̓ǂݍ��݂����������Ƃ��ɌĂяo�����C�x���g
    /// </summary>
    /// <param name="assetLoaderContext"></param>
    private void OnMaterialsLoad(AssetLoaderContext assetLoaderContext)
    {
        // ���[�h���ꂽGameObject���擾����
        loadedGameObject = assetLoaderContext.RootGameObject;

        // ���̎��_�őS�Ẵ��\�[�X�̓ǂݍ��݂��������Ă���̂ŃI�u�W�F�N�g��\������
        loadedGameObject.SetActive(true);

        //// ������̃��f����ǂݍ��ލہA���̂����f���̍��W���Y�����̂ŏC���@https://www.cgtrader.com/3d-model-collections/1400-people-crowds
        foreach (Transform childTransform in loadedGameObject.transform)
        {
            Debug.Log(childTransform.gameObject.name);
            if (childTransform.gameObject.name != "default.default")
            {
                var child = childTransform.gameObject;
                playerPos = child.GetComponent<Renderer>().bounds.center;
                float x = -playerPos.x;
                float y = -playerPos.y;
                float z = -playerPos.z;
                loadedGameObject.transform.Translate(x, y, z);
            }
            if (loadedGameObject.transform.childCount == 1)
            {
                var child = childTransform.gameObject;
                playerPos = child.GetComponent<Renderer>().bounds.center;
                float x = -playerPos.x;
                float y = -playerPos.y;
                float z = -playerPos.z;
                loadedGameObject.transform.Translate(x, y, z);
            }
        }
    }
}