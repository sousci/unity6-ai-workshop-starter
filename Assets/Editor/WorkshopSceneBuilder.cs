using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class WorkshopSceneBuilder
{
    private const string ScenePath = "Assets/Scenes/PhysicsBowling.unity";

    [MenuItem("Workshop/Build Physics Bowling Scene")]
    public static void Build()
    {
        EnsureFolder("Assets/Scenes");
        EnsureFolder("Assets/Materials");
        EnsureFolder("Assets/PhysicsMaterials");

        var groundMaterial = CreateMaterial("Assets/Materials/Ground.mat", new Color(0.17f, 0.42f, 0.28f));
        var laneMaterial = CreateMaterial("Assets/Materials/Lane.mat", new Color(0.78f, 0.62f, 0.38f));
        var ballMaterial = CreateMaterial("Assets/Materials/Ball.mat", new Color(0.08f, 0.38f, 0.88f));
        var blockBlue = CreateMaterial("Assets/Materials/BlockBlue.mat", new Color(0.13f, 0.65f, 0.88f));
        var blockOrange = CreateMaterial("Assets/Materials/BlockOrange.mat", new Color(1.0f, 0.38f, 0.18f));
        var bouncyMaterial = CreatePhysicsMaterial("Assets/PhysicsMaterials/BouncyBall.physicMaterial");

        var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
        scene.name = "PhysicsBowling";

        CreateCamera();
        CreateLighting();

        var environment = new GameObject("Environment");
        CreateCube("Ground", new Vector3(0f, -0.5f, 1f), new Vector3(16f, 1f, 22f), groundMaterial, environment.transform);
        CreateCube("Lane", new Vector3(0f, 0.03f, 1f), new Vector3(7f, 0.08f, 19f), laneMaterial, environment.transform);
        CreateCube("Left Rail", new Vector3(-3.65f, 0.45f, 1f), new Vector3(0.3f, 0.9f, 19f), groundMaterial, environment.transform);
        CreateCube("Right Rail", new Vector3(3.65f, 0.45f, 1f), new Vector3(0.3f, 0.9f, 19f), groundMaterial, environment.transform);
        CreateCube("Back Wall", new Vector3(0f, 1.25f, 10.35f), new Vector3(7.6f, 2.5f, 0.3f), groundMaterial, environment.transform);

        var ball = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        ball.name = "Ball - Edit BallLauncher.cs";
        ball.transform.position = new Vector3(0f, 0.65f, -6.8f);
        ball.transform.localScale = Vector3.one * 1.2f;
        ball.GetComponent<Renderer>().sharedMaterial = ballMaterial;
        ball.GetComponent<SphereCollider>().sharedMaterial = bouncyMaterial;
        var ballBody = ball.AddComponent<Rigidbody>();
        ballBody.mass = 1.5f;
        ballBody.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        ball.AddComponent<BallLauncher>();

        var blocks = new GameObject("Blocks");
        for (var row = 0; row < 3; row++)
        {
            for (var column = 0; column < 3; column++)
            {
                var position = new Vector3((column - 1) * 1.15f, 0.55f + row * 1.1f, 4.7f);
                var block = CreateCube($"Block {row + 1}-{column + 1}", position, new Vector3(1f, 1f, 0.8f),
                    (row + column) % 2 == 0 ? blockBlue : blockOrange, blocks.transform);
                var body = block.AddComponent<Rigidbody>();
                body.mass = 0.75f;
            }
        }

        var instructions = new GameObject("README - Edit Assets/Scripts/BallLauncher.cs");
        instructions.transform.SetAsFirstSibling();

        Selection.activeGameObject = ball;
        EditorSceneManager.SaveScene(scene, ScenePath);
        EditorBuildSettings.scenes = new[] { new EditorBuildSettingsScene(ScenePath, true) };

        PlayerSettings.companyName = "sousci";
        PlayerSettings.productName = "Unity 6 AI Physics Workshop";
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log($"Workshop scene created: {ScenePath}");
    }

    private static void CreateCamera()
    {
        var cameraObject = new GameObject("Main Camera");
        cameraObject.tag = "MainCamera";
        var camera = cameraObject.AddComponent<Camera>();
        camera.clearFlags = CameraClearFlags.Skybox;
        camera.fieldOfView = 55f;
        cameraObject.AddComponent<AudioListener>();
        cameraObject.transform.position = new Vector3(0f, 6.2f, -12.5f);
        cameraObject.transform.LookAt(new Vector3(0f, 1.1f, 2.2f));
    }

    private static void CreateLighting()
    {
        var lightObject = new GameObject("Directional Light");
        var light = lightObject.AddComponent<Light>();
        light.type = LightType.Directional;
        light.intensity = 1.25f;
        lightObject.transform.rotation = Quaternion.Euler(48f, -32f, 0f);

        RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Trilight;
        RenderSettings.ambientSkyColor = new Color(0.45f, 0.58f, 0.75f);
        RenderSettings.ambientEquatorColor = new Color(0.28f, 0.32f, 0.4f);
        RenderSettings.ambientGroundColor = new Color(0.12f, 0.14f, 0.18f);
    }

    private static GameObject CreateCube(string name, Vector3 position, Vector3 scale, Material material, Transform parent)
    {
        var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.name = name;
        cube.transform.SetParent(parent);
        cube.transform.position = position;
        cube.transform.localScale = scale;
        cube.GetComponent<Renderer>().sharedMaterial = material;
        return cube;
    }

    private static Material CreateMaterial(string path, Color color)
    {
        var existing = AssetDatabase.LoadAssetAtPath<Material>(path);
        if (existing != null)
        {
            existing.color = color;
            return existing;
        }

        var shader = Shader.Find("Standard") ?? Shader.Find("Universal Render Pipeline/Lit");
        var material = new Material(shader) { color = color };
        AssetDatabase.CreateAsset(material, path);
        return material;
    }

    private static PhysicsMaterial CreatePhysicsMaterial(string path)
    {
        var existing = AssetDatabase.LoadAssetAtPath<PhysicsMaterial>(path);
        if (existing != null) return existing;

        var material = new PhysicsMaterial("Bouncy Ball")
        {
            bounciness = 0.72f,
            dynamicFriction = 0.18f,
            staticFriction = 0.18f,
            bounceCombine = PhysicsMaterialCombine.Maximum,
            frictionCombine = PhysicsMaterialCombine.Average,
        };
        AssetDatabase.CreateAsset(material, path);
        return material;
    }

    private static void EnsureFolder(string path)
    {
        if (!Directory.Exists(path)) Directory.CreateDirectory(path);
    }
}

