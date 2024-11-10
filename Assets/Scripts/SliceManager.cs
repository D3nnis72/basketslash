using UnityEngine;
using EzySlice;

public class SliceManager : MonoBehaviour
{
    public Material slicedMaterial;
    public Transform sword;

    private const float SliceForce = 2000f;
    private const float DestructionDelay = 1f;

    public void SliceObject(GameObject slicedGameObject, Collision collision)
    {
        if (!CanSlice(slicedGameObject))
        {
            Debug.LogWarning("slicedGameObject does not have a MeshFilter or MeshCollider. Slicing will fail.");
            return;
        }

        Vector3 collisionNormal = collision.contacts[0].normal;
        Vector3 slicePosition = collision.contacts[0].point;
        Vector3 swordDirection = sword.forward;

        float alignment = Vector3.Dot(sword.forward, collisionNormal);
        if (Mathf.Abs(alignment) > 0.9f)
        {
            slicePosition = Vector3.up;
        }

        SlicedHull slicedObject = Slice(slicedGameObject, slicePosition, swordDirection);
        if (slicedObject == null)
        {
            Debug.LogError("Slicing failed: slicedObject is null. Check mesh settings and slice parameters.");
            return;
        }

        HandleSlicedObject(slicedGameObject, slicedObject, slicePosition, swordDirection);
    }

    private bool CanSlice(GameObject slicedGameObject)
    {
        return slicedGameObject.GetComponent<MeshFilter>() != null && slicedGameObject.GetComponent<MeshCollider>() != null;
    }

    private SlicedHull Slice(GameObject target, Vector3 position, Vector3 direction)
    {
        return target.Slice(position, direction, slicedMaterial);
    }

    private void HandleSlicedObject(GameObject originalObject, SlicedHull slicedObject, Vector3 slicePosition, Vector3 swordDirection)
    {
        GameObject upperHull = CreateHull(slicedObject.CreateUpperHull(originalObject, slicedMaterial));
        GameObject lowerHull = CreateHull(slicedObject.CreateLowerHull(originalObject, slicedMaterial));

        ApplyForces(upperHull, lowerHull, slicePosition, swordDirection);
        Destroy(originalObject);
        ScheduleDestruction(upperHull);
        ScheduleDestruction(lowerHull);
    }

    private GameObject CreateHull(GameObject hull)
    {
        if (hull == null) return null;

        hull.AddComponent<Rigidbody>();
        hull.AddComponent<BoxCollider>();
        return hull;
    }

    private void ApplyForces(GameObject upperHull, GameObject lowerHull, Vector3 slicePosition, Vector3 swordDirection)
    {
        Vector3 perpendicularDir = Vector3.Cross(swordDirection, Vector3.up).normalized;

        Rigidbody upperRb = upperHull.GetComponent<Rigidbody>();
        Rigidbody lowerRb = lowerHull.GetComponent<Rigidbody>();

        upperRb.AddForce(perpendicularDir * SliceForce);
        lowerRb.AddForce(-perpendicularDir * SliceForce);
    }

    private void ScheduleDestruction(GameObject hull)
    {
        if (hull != null)
        {
            Destroy(hull, DestructionDelay);
        }
    }
}
