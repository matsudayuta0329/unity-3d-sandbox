using UnityEngine;

public class MeshGanerator : MonoBehaviour
{
    [SerializeField]private SkinnedMeshRenderer meshRenderer;
    [SerializeField]private Transform boneParent;
    [Space(20)]

    [SerializeField]private float length = 1;
    [SerializeField]private float thickness = 1;
    [SerializeField]private float width = 1;

    void Start()
    {
        GanerateMesh();
    }

    public void GanerateMesh()
    {
        Vector3 offset = new Vector3(-length / 2, -thickness / 2, -width / 2);
        int cutNum = (int)length;
        int faceNum = (cutNum + 1) * 4 + 2;

        Vector3[] vertices = new Vector3[faceNum + 2];
        int[] triangles = new int[faceNum * 6];//四角面の数 * 一つの四角面のポリゴン数 * ポリゴンの頂点数

        //lengthをx方向の長さとしてカットされた各yz平面を走査
        //x,0,0をスタートとしてx,0,z -> x,y,0 -> x,y,zの順に頂点を追加する
        //(一周させる形で生成することも考えたがyz値をインデックス値一致で判定する必要が出たため使用していない)
        for(int i = 0; i < vertices.Length / 4; i++)
        {
            int topIndex = i * 4;
            //y,z方向に0の頂点の位置
            //カットはyz面で切るためここにthickness,widthを足すと他3点の位置になる
            Vector3 basisPos = new Vector3(i * length / (cutNum + 1), 0f, 0f);

            /*
            for(int j = 0; j < 4; j++)
            {
                Vector3 vertPos = basisPos;
                vertPos.y = Mathf.Floor(j / 2) * thickness;
                vertPos.z = j % 2 * width;

                vertices[topIndex + j] = vertPos;
            }
            */

            Vector3 vertPos = basisPos;
            vertices[topIndex] = vertPos;

            vertPos.y = 0;
            vertPos.z = width;
            vertices[topIndex + 1] = vertPos;
            
            vertPos.y = thickness;
            vertPos.z = 0;
            vertices[topIndex + 2] = vertPos;
            
            vertPos.y = thickness;
            vertPos.z = width;
            vertices[topIndex + 3] = vertPos;
        }


        int[] basisTri = {
            0, 2, 4,
            2, 6, 4,
            2, 3, 6,
            3, 7, 6,
            3, 1, 7,
            1, 5, 7,
            1, 0, 5,
            0, 4, 5,
        };

        int[] capTris = {
            3, 1, 0,
            0, 2, 3
        };

        for(int i = 0; i < (triangles.Length / 3) - 4; i++)//ふたの4ポリゴンは別途計算
        {
            int indexOffset = (int)(i / 8) * 4;

            for(int j = 0; j < 3; j++)
            {
                int triIndex = i * 3 + j;
                triangles[triIndex + 6] = basisTri[triIndex % 24]  + indexOffset;
            }
        }

        for(int i = 0; i < 6; i++)
        {
            triangles[i] = capTris[i];
        }
        for(int i = 0; i < 6; i++)
        {
            triangles[triangles.Length - 6 + i] = capTris[i] + (cutNum + 1) * 4;
        }

		Mesh mesh = new Mesh();
		mesh.Clear ();
		mesh.vertices = vertices;
		mesh.triangles = triangles;
		mesh.Optimize ();
		mesh.RecalculateNormals ();

        meshRenderer.sharedMesh = mesh;
    }
}
