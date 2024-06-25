using Enemies.BasicEnemy.Weapons.Bases;
using UnityEngine;

namespace Utility
{
    public class DamageZoneDrawer : MonoBehaviour
    {
        [SerializeField] private MeleeWeapon weapon;
        [SerializeField] private bool _flip;

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;

            switch (weapon.areaType)
            {
                case AreaType.Area:
                    DrawAreaGizmos();
                    break;
                case AreaType.Circle:
                    DrawCircleGizmos();
                    break;
                case AreaType.Box:
                    DrawRotatedBoxGizmos();
                    break;
            }
        }

        private void DrawAreaGizmos()
        {
            Vector3 position = transform.position;

            Vector3[] points =
            {
                position + (Vector3)weapon.PointA,
                position + new Vector3(weapon.PointB.x, weapon.PointA.y),
                position + new Vector3(weapon.PointB.x, weapon.PointA.y),
                position + (Vector3)weapon.PointB,
                position + (Vector3)weapon.PointB,
                position + new Vector3(weapon.PointA.x, weapon.PointB.y),
                position + new Vector3(weapon.PointA.x, weapon.PointB.y),
                position + (Vector3)weapon.PointA
            };

            Quaternion rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
            
            for (int i = 0; i < points.Length; i++)
            {
                points[i] = rotation * (points[i] - position) + position;
            }
            
            Gizmos.DrawLineList(points);
        }

        private void DrawCircleGizmos()
        {
            float yRotValue = transform.rotation.eulerAngles.y;
            float dirMultiplier = yRotValue == 0 ? 1 : -1;
            
            Vector3 center = transform.position + new Vector3(dirMultiplier * weapon.PointA.x, weapon.PointA.y);
            
            Gizmos.DrawWireSphere(center, weapon.Radius);
        }

        private void DrawRotatedBoxGizmos()
        {
            float yRotValue = transform.rotation.eulerAngles.y;
            float dirMultiplier = yRotValue == 0 ? 1 : -1;
            
            Vector3 center = transform.position + new Vector3(dirMultiplier * weapon.PointA.x, weapon.PointA.y);
            Vector3 size = weapon.Size;
            float angle = weapon.Angle;

            Vector3 halfSize = size * 0.5f;

            // Calculate the four corners of the box
            Vector3[] corners = new Vector3[4];
            corners[0] = new Vector3(-halfSize.x, -halfSize.y);
            corners[1] = new Vector3(halfSize.x, -halfSize.y);
            corners[2] = new Vector3(halfSize.x, halfSize.y);
            corners[3] = new Vector3(-halfSize.x, halfSize.y);

            Quaternion rotation = Quaternion.Euler(0, yRotValue, angle);
            
            for (int i = 0; i < corners.Length; i++)
            {
                corners[i] = rotation * corners[i] + center;
            }

            // Draw the box
            Gizmos.DrawLine(corners[0], corners[1]);
            Gizmos.DrawLine(corners[1], corners[2]);
            Gizmos.DrawLine(corners[2], corners[3]);
            Gizmos.DrawLine(corners[3], corners[0]);
        }

#endif
    }
}