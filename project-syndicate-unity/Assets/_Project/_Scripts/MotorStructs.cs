using UnityEngine;

/// <summary>
/// Defined points, where (Collision) Raycast should originate form. 
/// </summary>
public struct RaycastOrigin
{
	#region SkinWidth
	const float skinWidth = 0.015f;

	/// <summary>
	/// To avoid weird collision between objects, we start our raycasts from inside our collider.
	/// Keep in mind, that you have to add this value again when using any of the RaycastOrigins.
	/// </summary>
	public float SkinWidth => skinWidth;
	#endregion

	#region Origins
	Vector3 topLeftFront, topLeftBack;
	Vector3 topRightFront, topRightBack;
	Vector3 bottomLeftFront, bottomLeftBack;
	Vector3 bottomRightFront, bottomRightBack;

	public Vector3 TopLeftFront => topLeftFront;
	public Vector3 TopLeftBack => topLeftBack;
	public Vector3 TopRightFront => topRightFront;
	public Vector3 TopRightBack => topRightBack;
	public Vector3 BottomLeftFront => bottomLeftFront;
	public Vector3 BottomLeftBack => bottomLeftBack;
	public Vector3 BottomRightFront => bottomRightFront;
	public Vector3 BottomRightBack => bottomRightBack;

	#endregion

	/// <summary>
	/// Recalculate the origin vectors from the given bounds.
	/// Has to be used every time the character moves.
	/// </summary>
	/// <param name="bounds">Current collider bounds (usually collder.bounds)</param>
	public void Update(Bounds bounds)
	{
		bounds.Expand(skinWidth * -2f); //2 -> both sides

		/*       |======| <- max
		 *      /      /|
		 *     /      / |
		 *    |======|  |
		 *  y |      | /
		 *    |      |/ z
		 *    |======|
		 *  min   x
		 */

		bottomLeftFront = new Vector3(bounds.min.x, bounds.min.y, bounds.min.z);
		bottomLeftBack = new Vector3(bounds.min.x, bounds.min.y, bounds.max.z);
		bottomRightFront = new Vector3(bounds.max.x, bounds.min.y, bounds.min.z);
		bottomRightBack = new Vector3(bounds.max.x, bounds.min.y, bounds.max.z);

		topLeftFront = new Vector3(bounds.min.x, bounds.max.y, bounds.min.z);
		topLeftBack = new Vector3(bounds.min.x, bounds.max.y, bounds.max.z);
		topRightFront = new Vector3(bounds.max.x, bounds.max.y, bounds.min.z);
		topRightBack = new Vector3(bounds.max.x, bounds.max.y, bounds.max.z);
	}
}

/// <summary>
/// Calculate the number and the distance between rays based on the objects collider size.
/// </summary>
public struct RaycastSpacing
{ //NOTE RacastSpacing -> better name for spacing?
	#region Distance between Rays
	const float dstBetweenRays = 0.25f;
	#endregion

	#region RayCount
	int horizontalRayCount;
	int verticalRayCount;
	int depthRayCount;

	public int HorizontalRayCount => horizontalRayCount;
	public int VerticalRayCount => verticalRayCount;
	public int DepthRayCount => depthRayCount;
	#endregion

	#region RaySpacing
	float horizontalRaySpacing;
	float verticalRaySpacing;
	float depthRaySpacing;

	public float HorizontalRaySpacing => horizontalRaySpacing;
	public float VerticalRaySpacing => verticalRaySpacing;
	public float DepthRaySpacing => depthRaySpacing;
	#endregion

	/// <summary>
	/// Calculate number and distance between rays. Call this function, whenever the collider size changes.
	/// </summary>
	/// <param name="bounds">Current collider bounds (usually collder.bounds)</param>
	/// <param name="skinWidth">Thin protection layer so collider don't space out when directly contacting ground/walls.</param>
	public void Set(Bounds bounds, float skinWidth = 0.015f)
	{
		bounds.Expand(skinWidth * -2f);

		float boundsHeight = bounds.size.y;
		float boundsWidth = bounds.size.x;
		float boundsDepth = bounds.size.z;

		horizontalRayCount = Mathf.RoundToInt(boundsHeight / dstBetweenRays);
		verticalRayCount = Mathf.RoundToInt(boundsWidth / dstBetweenRays);
		depthRayCount = Mathf.RoundToInt(boundsDepth / dstBetweenRays);

		horizontalRayCount = Mathf.Clamp(horizontalRayCount, 2, int.MaxValue);
		verticalRayCount = Mathf.Clamp(verticalRayCount, 2, int.MaxValue);
		depthRayCount = Mathf.Clamp(depthRayCount, 2, int.MaxValue);

		horizontalRaySpacing = boundsHeight / (horizontalRayCount - 1);
		verticalRaySpacing = boundsWidth / (verticalRayCount - 1);
		depthRaySpacing = boundsWidth / (depthRayCount - 1);
	}
}

/// <summary>
/// Stores the current collisions directions.
/// </summary>
public struct CollisionInfo
{
	public bool above, below;
	public bool left, right;
	public bool front, back;

	//NOTE CollisionsInfo: are slopeUp/slopeDown needed as Information in other classes?
	public bool slopeUp, slopeDown;

	/// <summary>
	/// Set all the bool to false. Call this every frame.
	/// </summary>
	public void Reset()
	{
		above = below = false;
		left = right = false;
		front = back = false;
		slopeUp = slopeDown = false;
	}
}