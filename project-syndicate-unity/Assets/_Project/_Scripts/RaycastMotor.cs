using UnityEngine;


//NOTE maybe rearrange RaycastMotor to be a component instead of a base class
//RaycastMotor add functionality for not just BoxColliders

/// <summary>
/// Loads raycast data inside the RaycastOrigin struct.
/// Use this as base class whenever you work with Collider2D.
/// </summary>
[RequireComponent(typeof(BoxCollider))]
public class RaycastMotor : MonoBehaviour
{
	public LayerMask collisionMask;
	public bool drawDebugRays = false;

	[HideInInspector] public BoxCollider boxCollider;
	protected RaycastOrigin raycastOrigin;
	protected RaycastSpacing raycastSpacing;

	protected virtual void Awake()
	{
		boxCollider = GetComponent<BoxCollider>();
		raycastOrigin = new RaycastOrigin();
		raycastSpacing = new RaycastSpacing();
	}

	protected virtual void Start()
	{
		raycastOrigin.Update(boxCollider.bounds);
		raycastSpacing.Set(boxCollider.bounds, raycastOrigin.SkinWidth);
	}

	protected virtual void Update()
	{
		if (drawDebugRays)
		{
			DebugRaycastOriginCountSpacing();
		}
	}

	/// <summary>
	/// Draw RaycastOrigin and RaycastCount with RaycastSpacing.
	/// </summary>
	protected void DebugRaycastOriginCountSpacing()
	{
		Color color = new Color(0f, 1f, 0f, 0.5f);
		color = Color.yellow;

		// Bottom Square
		//Debug.DrawLine(raycastOrigin.BottomLeftFront + Vector3.down / 2, raycastOrigin.BottomLeftBack + Vector3.up / 2, color);
		//Debug.DrawLine(raycastOrigin.BottomLeftBack + Vector3.down / 2, raycastOrigin.BottomRightBack + Vector3.up / 2, color);
		//Debug.DrawLine(raycastOrigin.BottomRightBack + Vector3.down / 2, raycastOrigin.BottomRightFront + Vector3.up / 2, color);
		//Debug.DrawLine(raycastOrigin.BottomRightFront + Vector3.down / 2, raycastOrigin.BottomLeftFront + Vector3.up / 2, color);

		//// Top Square
		//Debug.DrawLine(raycastOrigin.TopLeftFront + Vector3.down / 2, raycastOrigin.TopLeftBack + Vector3.up / 2, color);
		//Debug.DrawLine(raycastOrigin.TopLeftBack + Vector3.down / 2, raycastOrigin.TopRightBack + Vector3.up / 2, color);
		//Debug.DrawLine(raycastOrigin.TopRightBack + Vector3.down / 2, raycastOrigin.TopRightFront + Vector3.up / 2, color);
		//Debug.DrawLine(raycastOrigin.TopRightFront + Vector3.down / 2, raycastOrigin.TopLeftFront + Vector3.up / 2, color);

		////Horizontal Lines
		//Debug.DrawLine(raycastOrigin.BottomLeftFront + Vector3.down / 2, raycastOrigin.TopLeftFront + Vector3.up / 2, color);
		//Debug.DrawLine(raycastOrigin.BottomLeftBack + Vector3.down / 2, raycastOrigin.TopLeftBack + Vector3.up / 2, color);
		//Debug.DrawLine(raycastOrigin.BottomRightBack + Vector3.down / 2, raycastOrigin.TopRightBack + Vector3.up / 2, color);
		//Debug.DrawLine(raycastOrigin.BottomRightFront + Vector3.down / 2, raycastOrigin.TopRightFront + Vector3.up / 2, color);

		// Bottom Square
		Debug.DrawLine(raycastOrigin.BottomLeftFront, raycastOrigin.BottomLeftBack, color);
		Debug.DrawLine(raycastOrigin.BottomLeftBack, raycastOrigin.BottomRightBack, color);
		Debug.DrawLine(raycastOrigin.BottomRightBack, raycastOrigin.BottomRightFront, color);
		Debug.DrawLine(raycastOrigin.BottomRightFront, raycastOrigin.BottomLeftFront, color);

		// Top Square
		Debug.DrawLine(raycastOrigin.TopLeftFront, raycastOrigin.TopLeftBack, color);
		Debug.DrawLine(raycastOrigin.TopLeftBack, raycastOrigin.TopRightBack, color);
		Debug.DrawLine(raycastOrigin.TopRightBack, raycastOrigin.TopRightFront , color);
		Debug.DrawLine(raycastOrigin.TopRightFront, raycastOrigin.TopLeftFront, color);

		//Horizontal Lines
		Debug.DrawLine(raycastOrigin.BottomLeftFront, raycastOrigin.TopLeftFront, color);
		Debug.DrawLine(raycastOrigin.BottomLeftBack, raycastOrigin.TopLeftBack, color);
		Debug.DrawLine(raycastOrigin.BottomRightBack, raycastOrigin.TopRightBack, color);
		Debug.DrawLine(raycastOrigin.BottomRightFront, raycastOrigin.TopRightFront, color);

		color = new Color(0f, 1f, 0f, 0.35f);

		//Vertical RaycastCount + RaycastSpacing
		//for (int i = 1; i < raycastSpacing.VerticalRayCount - 1; i++)
		//{
		//	Debug.DrawRay(raycastOrigin.BottomLeft + Vector2.right * (raycastSpacing.VerticalRaySpacing * i), Vector2.down / 4, color);
		//	Debug.DrawRay(raycastOrigin.TopLeft + Vector2.right * (raycastSpacing.VerticalRaySpacing * i), Vector2.up / 4, color);
		//}
		////Horizontal RaycastCount + RaycastSpacing
		//for (int i = 1; i < raycastSpacing.HorizontalRayCount - 1; i++)
		//{
		//	Debug.DrawRay(raycastOrigin.BottomLeft + Vector2.up * (raycastSpacing.HorizontalRaySpacing * i), Vector2.left / 4, color);
		//	Debug.DrawRay(raycastOrigin.BottomRight + Vector2.up * (raycastSpacing.HorizontalRaySpacing * i), Vector2.right / 4, color);
		//}
	}
}