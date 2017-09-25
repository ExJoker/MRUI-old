using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MRUi
{
    /// <summary>
    /// create circle segment mesh for a round button group
    /// this script only creates the mesh, you need the other scripts to make it an interactable button
    /// </summary>
    [ExecuteInEditMode]
    public class MRUiCircleButtonSegment : MonoBehaviour
    {
        [Tooltip("Number of segments. Defaults to 2 for a half-circle")]
        [Range(1, 8)]
        public int segments = 2;

        [Tooltip("number of segments the whole circle consists of. this defines the amount of vertices/triangles used")]
        public int parts = 36;

        [Tooltip("radius from center to circle")]
        public float innerRadius = .04f;

        [Tooltip("radius from center to outer circle")]
        public float outerRadius = .1f;

        [Tooltip("width of the circle")]
        public float width = .04f;

        public float angle;

        private Mesh mesh;
        private Vector3[] vertices;
        private Vector3[] normals;
        private MeshCollider meshCollider;

        public void Update()
        {
            if (mesh == null)
            {
                GetComponent<MeshFilter>().mesh = mesh = new Mesh(); ;
                meshCollider = GetComponent<MeshCollider>();
                mesh.name = "MRUiSegmentMesh";
                updateData();
            }
        }

        public void setAngle(float angle)
        {
            transform.eulerAngles= new Vector3(0, 0, angle);
            this.angle = angle;
            // Debug.Log("Set Angle to " + angle);
        }

        public void updateData()
        {
            if (mesh == null)
            {
                return;
            }
            mesh.Clear();
            mesh.name = "MRUi Circle Segment";

            genRoundSegment(0);
        }

        private Vector3 createVertice(float angle, float radius, float zPos)
        {
            float cx = angle * Mathf.PI;
            float cy = cx;
            cx = radius * Mathf.Cos(cx);
            cy = radius * Mathf.Sin(cy);
            return new Vector3(cx, cy, zPos);
        }

        private void createRect(int[] triangles, int offset, int bl, int br, int tl, int tr)
        {
            triangles[0 + offset] = br;
            triangles[1 + offset] = tl;
            triangles[2 + offset] = bl;
            
            triangles[3 + offset] = br;
            triangles[4 + offset] = tr;
            triangles[5 + offset] = tl;
        }

        private void genRoundSegment(float degreeStart)
        {
            // 4 per segment
            vertices = new Vector3[4 * (parts / segments) + 4];
            // create vertices
            for (int i = 0; i < vertices.Length; i += 4)
            {
                float angle = i * 1f / parts / 2;
                vertices[i] = createVertice(angle, innerRadius, width / 2);
                vertices[i+1] = createVertice(angle, innerRadius, -width / 2);
                vertices[i+2] = createVertice(angle, outerRadius, width / 2);
                vertices[i+3] = createVertice(angle, outerRadius, -width / 2);
            }

            normals = new Vector3[vertices.Length];

            int startEnd = 0;
            if (segments >= 1)
            {
                startEnd = 12;
            }
            int[] triangles = new int[(vertices.Length / 4) * 24 + startEnd];
            normals = new Vector3[vertices.Length];
            // start and end
            if (segments > 1)
            {
                createRect(triangles, 0, 0, 1, 2, 3);
                createRect(triangles, triangles.Length - 6, 
                    vertices.Length - 1, vertices.Length - 3, vertices.Length - 2, vertices.Length - 4);
            }

            // in between
            int t = 6;
            
            for (int i = 0; i < vertices.Length-4; i+=4)
            {
                // bottom
                createRect(triangles, t, i + 4, i + 5, i, i + 1);
                t += 6;

                // left
                createRect(triangles, t, i + 4, i, i + 6, i + 2);
                t += 6;
                
                // right
                createRect(triangles, t, i + 1, i + 5, i + 3, i + 7);
                t += 6;
                
                // top
                createRect(triangles, t, i + 2, i + 3, i + 6, i + 7);
                t += 6;
            }

            
            for (int i = 0; i < vertices.Length; i++)
            {
                normals[i] = vertices[i].normalized;
            }

            mesh.vertices = vertices;
            mesh.normals = normals;
            mesh.triangles = triangles;

            if (meshCollider != null)
            {
                meshCollider.sharedMesh = null;
                meshCollider.sharedMesh = mesh;
            }
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            parts = Mathf.Max(parts, 16);
            if (parts % segments != 0)
            {
                parts = ((int)(parts / segments) + 1) * segments;
            }
            updateData();
            setAngle(angle);
        }
#endif
    }
}