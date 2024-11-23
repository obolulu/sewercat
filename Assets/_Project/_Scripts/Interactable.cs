using System;
using UnityEngine;
namespace _Project._Scripts
{
    public abstract class Interactable : MonoBehaviour, IInteractable
    {
        [SerializeField] private Material[]   originalMaterials;
        [SerializeField] private Material[]   materialsWithOutline;
        
        [SerializeField]private MeshRenderer meshRenderer;
        
        [SerializeField] private Material outlineShader;

        private void Start()
        {
        
            // Store original materials
            originalMaterials = meshRenderer.materials;
        
            // Create array for materials with outline
            materialsWithOutline = new Material[originalMaterials.Length + 1];
            originalMaterials.CopyTo(materialsWithOutline, 0);
            materialsWithOutline[^1] = outlineShader;
        
            // Start with outline disabled
            DisableOutline();
        }

        public void EnableOutline()
        {
            meshRenderer.materials = materialsWithOutline;
        }

        public void DisableOutline()
        {
            meshRenderer.materials = originalMaterials;
        }

        public abstract            void onInteract();
        
        
        /*
        private void OnDrawGizmos()
        {
            Draw.Line(transform.position, transform.position + transform.forward * 2, Color.red);
        }
        */
        /*
        public void Highlight()
        {
            Draw.Line(transform.position, transform.position + transform.forward * 2, Color.green);
            using (Draw.Command(Camera.main))
            {
                // Draw in world space
                Draw.Matrix = Matrix4x4.identity;
                Draw.Line(transform.position, transform.position + transform.forward * 2, Color.green);
            }
        
        }
        */

        public void Highlight()
        {
            EnableOutline();
        }

        public void EndHighlight()
        {
            DisableOutline();        }

        private void Test()
        {
            Debug.Log(" test");
        }
        
    }
    }