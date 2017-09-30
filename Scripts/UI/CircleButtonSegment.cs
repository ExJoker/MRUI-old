﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace MRUI
{

    /// <summary>
    /// create circle segment mesh for a round button group
    /// this script only creates the mesh, you need the other scripts to make it an interactable button
    /// </summary>
    [RequireComponent(typeof(MeshRenderer))]
    [RequireComponent(typeof(MeshFilter))]

    [ExecuteInEditMode]
    public class CircleButtonSegment : MonoBehaviour
    {
        [Tooltip("Number of segments. Defaults to 2 for a half-circle")]
        [Range(1, 9)]
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

        [HideInInspector]
        public UnityEvent OnAngleChanged;

        private Mesh mesh;
        private Vector3[] vertices;
        private Vector3[] normals;
        private MeshCollider meshCollider;

        private void Awake()
        {
            if (OnAngleChanged == null)
            {
                OnAngleChanged = new UnityEvent();
            }
        }

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
            transform.localEulerAngles = new Vector3(0, 0, angle);
            this.angle = angle;
            // Debug.Log("Set Angle to " + angle);
            if (OnAngleChanged != null)
            {
                OnAngleChanged.Invoke();
            }
        }

        public void updateData()
        {
            if (mesh == null)
            {
                return;
            }
            mesh.Clear();
            mesh.name = "MRUI Circle Segment";

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
            // see https://docs.unity3d.com/Manual/Example-CreatingaBillboardPlane.html
            triangles[0 + offset] = bl;
            triangles[1 + offset] = tl;
            triangles[2 + offset] = br;
            
            triangles[3 + offset] = tl;
            triangles[4 + offset] = tr;
            triangles[5 + offset] = br;
        }

        private void genRoundSegment(float degreeStart)
        {
            // 4 per segment
            vertices = new Vector3[4 * (parts / segments) + 4];
            normals = new Vector3[vertices.Length];
            // create vertices
            for (int i = 0; i < vertices.Length; i += 4)
            {
                float angle = i * 1f / parts / 2;
                // lower front
                vertices[i] = createVertice(angle, innerRadius, -width / 2);
                normals[i] = -Vector3.forward;
                //normals[i] = createVertice(angle, innerRadius / 2, -width);

                // lower back
                vertices[i + 1] = createVertice(angle, innerRadius, width / 2);
                normals[i + 1] = Vector3.forward;
                //normals[i + 1] = createVertice(angle, innerRadius / 2, width);

                // upper front
                vertices[i + 2] = createVertice(angle, outerRadius, -width / 2);
                normals[i + 2] = -Vector3.forward;
                //normals[i + 2] = -createVertice(angle, outerRadius * 1.5f, -width);

                // upper back
                vertices[i + 3] = createVertice(angle, outerRadius, width / 2);
                normals[i + 3] = Vector3.forward;
                //normals[i + 3] = createVertice(angle, outerRadius * 1.5f, width);
            }
            
            int startEnd = 0;
            if (segments >= 1)
            {
                startEnd = 12;
            }
            int[] triangles = new int[(vertices.Length / 4) * 24 + startEnd];
            // start and end
            if (segments > 1)
            {
                createRect(triangles, 0, 3, 2, 1, 0);
                createRect(triangles, triangles.Length - 6, 
                    vertices.Length - 4, vertices.Length - 2, vertices.Length - 3, vertices.Length - 1);
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
                normals[i] = normals[i].normalized;
            }

            mesh.vertices = vertices;
            mesh.normals = normals;
            
            mesh.triangles = triangles;
            //mesh.RecalculateNormals();

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