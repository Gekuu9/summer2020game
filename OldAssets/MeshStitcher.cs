using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshStitcher : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public BlockGrid blockGrid;

    public void StitchMesh() {
        Block currBlock;

        for (int z = 0; z < blockGrid.xSize; z++) {
            for (int y = 0; y < blockGrid.ySize; y++) {
                for (int x = 0; x < blockGrid.zSize; x++) {
                    Vector3 position = new Vector3(x, y, z);
                    currBlock = blockGrid.getBlock(x, y, z);
                    Matrix4x4 transform = Matrix4x4.Translate(position);

                    CombineInstance blockCombineInstance = new CombineInstance();
                    blockCombineInstance.transform = transform;
                    blockCombineInstance.mesh = currBlock.mesh;
                    blockCombineInstance.subMeshIndex = x + y * blockGrid.xSize + z * blockGrid.xSize * blockGrid.ySize;


                    

                    Mesh temp = new Mesh();
                    temp.vertices = currBlock.mesh.vertices;

                    for (int i = 0; i < temp.vertices.Length; i++) {
                        temp.vertices[i].x += x;
                        temp.vertices[i].y += y;
                        temp.vertices[i].z += z;
                    }

                    temp.triangles = currBlock.mesh.triangles;
                    temp.uv = currBlock.mesh.uv;
                    temp.normals = currBlock.mesh.normals;
                    temp.tangents = currBlock.mesh.tangents;
                    temp.colors = currBlock.mesh.colors;

                    newMesh.CombineMeshes()
                }
            }
        }
    }
}
