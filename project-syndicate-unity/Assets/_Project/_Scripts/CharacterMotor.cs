using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//BUG: CharacterMotor: jump doesn't always work when switching from slope up to down

[RequireComponent(typeof(RaycastMotor))]
public class CharacterMotor : RaycastMotor, IMotor
{
	public CollisionInfo collision;
	public float maxSlopeAngle;

	float currentSlopeAngle;
	float lastSlopeAngle;
	Vector2 moveOld; //stored in case we detect a slope up while walking a slope down 

	Color color = new Color(1f, 0f, 0f, 0.5f);

	protected override void Awake()
	{
		base.Awake();
	}

	protected override void Start()
	{
		base.Start();
	}

	/// <summary>
	/// Move character, while also checking for collisions.
	/// </summary>
	/// <param name="move">Direction/Velocity of the Movement</param>
	public void Move(Vector3 move)
	{
		//Reset/recalculate values
		raycastOrigin.Update(boxCollider.bounds);
		collision.Reset();
		lastSlopeAngle = currentSlopeAngle;
		currentSlopeAngle = 0f;
		moveOld = move;

		if (move.y < 0f)
		{
			HandleDescendingSlopes(ref move);
		}

		if (move.x != 0f)
		{
			HandleHorizontalCollisions(ref move);
		}

		if (move.y != 0f)
		{
			HandleVerticalCollisions(ref move);
		}

		transform.Translate(move);
	}

	void HandleHorizontalCollisions(ref Vector3 move)
	{
		float dirX = Mathf.Sign(move.x);
		float rayLength = Mathf.Abs(move.x) + raycastOrigin.SkinWidth;

		for (int i = 0; i < raycastSpacing.HorizontalRayCount; i++)
		{
			Vector3 rayOriginBack = (dirX == -1) ? raycastOrigin.BottomLeftBack : raycastOrigin.BottomRightBack;
			Vector3 rayOriginFront = (dirX == -1) ? raycastOrigin.BottomLeftFront : raycastOrigin.BottomRightFront;
			rayOriginBack += Vector3.up * (raycastSpacing.HorizontalRaySpacing * i);
			rayOriginFront += Vector3.up * (raycastSpacing.HorizontalRaySpacing * i);

			if (drawDebugRays)
			{
				// TODO make it better kekW
				Debug.DrawRay(rayOriginBack, Vector2.right * dirX * rayLength, color);
				Debug.DrawRay(rayOriginFront, Vector2.right * dirX * rayLength, color);
			}

			RaycastHit hit;
			if (Physics.Raycast(rayOriginBack, Vector3.right * dirX, out hit, rayLength, collisionMask)) // also do it for hitFront
			{
				float slopeAngle = Vector3.Angle(hit.normal, Vector3.up);

				//Only check slopes on bottom raycast
				if (i == 0 && slopeAngle <= maxSlopeAngle)
				{

					if (collision.slopeDown)
					{
						collision.slopeDown = false;
						move = moveOld;
					}

					//remove 'hit.distance' when walking on slope (!not standing)
					//hit.distance is usually really small, but you are 'jumping' down
					//on the slope when you stop moving
					float distanceToSlopeStart = 0f;
					if (slopeAngle != lastSlopeAngle)
					{
						//the != occurs when start walking on a slope
						//because the slopeAngleOld is 0f, while slopeAngle is changed
						//it also occurs when you go from a slope to another slope
						distanceToSlopeStart = hit.distance - raycastOrigin.SkinWidth;
						move.x -= distanceToSlopeStart * dirX;
					}
					HandleClimbingSlopes(ref move, slopeAngle);
					move.x += distanceToSlopeStart * dirX;
				}

				//if not on a slope (standard case)
				//OR detecting a "wall" (while also on a slope)
				if (!collision.slopeUp || slopeAngle > maxSlopeAngle)
				{
					move.x = (hit.distance - raycastOrigin.SkinWidth) * dirX;
					rayLength = hit.distance;

					if (collision.slopeUp)
					{
						move.y = Mathf.Tan(currentSlopeAngle * Mathf.Deg2Rad) * Mathf.Abs(move.x);
					}

					collision.left = (dirX == -1);
					collision.right = (dirX == 1);
					collision.back = true;
				}
			}

			if (Physics.Raycast(rayOriginFront, Vector3.right * dirX, out hit, rayLength, collisionMask)) // also do it for hitFront
			{
				float slopeAngle = Vector3.Angle(hit.normal, Vector3.up);

				//Only check slopes on bottom raycast
				if (i == 0 && slopeAngle <= maxSlopeAngle)
				{

					if (collision.slopeDown)
					{
						collision.slopeDown = false;
						move = moveOld;
					}

					//remove 'hit.distance' when walking on slope (!not standing)
					//hit.distance is usually really small, but you are 'jumping' down
					//on the slope when you stop moving
					float distanceToSlopeStart = 0f;
					if (slopeAngle != lastSlopeAngle)
					{
						//the != occurs when start walking on a slope
						//because the slopeAngleOld is 0f, while slopeAngle is changed
						//it also occurs when you go from a slope to another slope
						distanceToSlopeStart = hit.distance - raycastOrigin.SkinWidth;
						move.x -= distanceToSlopeStart * dirX;
					}
					HandleClimbingSlopes(ref move, slopeAngle);
					move.x += distanceToSlopeStart * dirX;
				}

				//if not on a slope (standard case)
				//OR detecting a "wall" (while also on a slope)
				if (!collision.slopeUp || slopeAngle > maxSlopeAngle)
				{
					move.x = (hit.distance - raycastOrigin.SkinWidth) * dirX;
					rayLength = hit.distance;

					if (collision.slopeUp)
					{
						move.y = Mathf.Tan(currentSlopeAngle * Mathf.Deg2Rad) * Mathf.Abs(move.x);
					}

					collision.left = (dirX == -1);
					collision.right = (dirX == 1);
					collision.front = true;
				}
			}
		}
	}

	void HandleVerticalCollisions(ref Vector3 move)
	{
		float dirY = Mathf.Sign(move.y);
		float rayLength = Mathf.Abs(move.y) + raycastOrigin.SkinWidth;

		for (int i = 0; i < raycastSpacing.VerticalRayCount; i++)
		{
			Vector3 rayOriginBack = (dirY == -1) ? raycastOrigin.BottomLeftBack : raycastOrigin.TopLeftBack;
			rayOriginBack += Vector3.right * (raycastSpacing.VerticalRaySpacing * i + move.x);
			Vector3 rayOriginFront = (dirY == -1) ? raycastOrigin.BottomLeftFront : raycastOrigin.TopLeftFront;
			rayOriginFront += Vector3.right * (raycastSpacing.VerticalRaySpacing * i + move.x);

			if (drawDebugRays)
			{
				Debug.DrawRay(rayOriginBack, Vector2.up * dirY * rayLength, color);
			}

			RaycastHit hit;
			if (Physics.Raycast(rayOriginBack, Vector3.up * dirY, out hit, rayLength, collisionMask))
			{
				move.y = (hit.distance - raycastOrigin.SkinWidth) * dirY;
				rayLength = hit.distance;

				//if climbingSlope
				//tan(a) = y / x <=> x = y / tan(a)
				//velocity.x = velocity.y / tan(slopeAngle * deg2Rad) * dirY

				if (collision.slopeUp)
				{
					move.x = move.y / Mathf.Tan(currentSlopeAngle * Mathf.Deg2Rad) * Mathf.Sign(move.x);
				}

				collision.below = (dirY == -1);
				collision.above = (dirY == 1);
				collision.back = true;
			}

			if (Physics.Raycast(rayOriginFront, Vector3.up * dirY, out hit, rayLength, collisionMask))
			{
				move.y = (hit.distance - raycastOrigin.SkinWidth) * dirY;
				rayLength = hit.distance;

				//if climbingSlope
				//tan(a) = y / x <=> x = y / tan(a)
				//velocity.x = velocity.y / tan(slopeAngle * deg2Rad) * dirY

				if (collision.slopeUp)
				{
					move.x = move.y / Mathf.Tan(currentSlopeAngle * Mathf.Deg2Rad) * Mathf.Sign(move.x);
				}

				collision.below = (dirY == -1);
				collision.above = (dirY == 1);
				collision.front = true;
			}
		}

		//a bug occurs when we switch between two slopes, so we are 'stuck' for a frame in a platform
		//so we try to detect other slopes beforehand and adjust our character accordingly
		if (collision.slopeUp)
		{
			float dirX = Mathf.Sign(move.x);
			rayLength = Mathf.Abs(move.x) + raycastOrigin.SkinWidth;
			//case the ray from the new height
			Vector3 rayOriginBack = ((dirX == -1) ? raycastOrigin.BottomLeftBack : raycastOrigin.BottomRightBack) + Vector3.up * move.y;
			Vector3 rayOriginFront = ((dirX == -1) ? raycastOrigin.BottomLeftFront : raycastOrigin.BottomRightFront) + Vector3.up * move.y;

			RaycastHit hit;
			if (Physics.Raycast(rayOriginBack, Vector3.right * dirX, out hit, rayLength, collisionMask))
			{
				float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
				//if new slope is not our current slope
				if (slopeAngle != currentSlopeAngle)
				{
					move.x = (hit.distance - raycastOrigin.SkinWidth) * dirX;
					currentSlopeAngle = slopeAngle;
				}
			}

			if (Physics.Raycast(rayOriginFront, Vector3.right * dirX, out hit, rayLength, collisionMask))
			{
				float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
				//if new slope is not our current slope
				if (slopeAngle != currentSlopeAngle)
				{
					move.x = (hit.distance - raycastOrigin.SkinWidth) * dirX;
					currentSlopeAngle = slopeAngle;
				}
			}
		}
	}

	void HandleClimbingSlopes(ref Vector3 move, float slopeAngle)
	{
		float distance = Mathf.Abs(move.x);

		float slopeMoveY = distance * Mathf.Sin(slopeAngle * Mathf.Deg2Rad);

		//if y is smaller then calculated move then change it
		//if y is higher, that the player is jumping (or otherwise changing the y move)
		if (move.y <= slopeMoveY)
		{
			move.y = slopeMoveY;
			move.x = Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * distance * Mathf.Sign(move.x);

			collision.below = true; //we can assume, that when we walk on a slope we are grounded
			collision.slopeUp = true;
			currentSlopeAngle = slopeAngle;
		}
	}

	void HandleDescendingSlopes(ref Vector3 move)
	{
		float dirX = Mathf.Sign(move.x);
		Vector2 rayOriginBack = (dirX == -1) ? raycastOrigin.BottomRightBack : raycastOrigin.BottomLeftBack;
		Vector2 rayOriginFront = (dirX == -1) ? raycastOrigin.BottomRightFront : raycastOrigin.BottomLeftFront;

		if (drawDebugRays)
		{
			Debug.DrawRay(rayOriginBack, Vector2.down, color);
			Debug.DrawRay(rayOriginFront, Vector2.down, color);
		}

		RaycastHit hit;
		if (Physics.Raycast(rayOriginBack, Vector3.down, out hit, Mathf.Infinity, collisionMask))
		{
			float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
			//maybe bug occurs when faliing on a angled platform? 
			if (slopeAngle != 0f && slopeAngle <= maxSlopeAngle)
			{
				//are you actually walking downwards? (
				if (Mathf.Sign(hit.normal.x) == dirX)
				{
					//if our detected ground is smaller/egual than our actual move (calculated thourgh y = tan(a) * x)
					if (hit.distance - raycastOrigin.SkinWidth <= Mathf.Tan(slopeAngle * Mathf.Deg2Rad) * Mathf.Abs(move.x))
					{
						float distance = Mathf.Abs(move.x);

						move.x = Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * distance * dirX;
						move.y -= distance * Mathf.Sin(slopeAngle * Mathf.Deg2Rad);

						currentSlopeAngle = slopeAngle;
						collision.slopeDown = true;
						collision.below = true;
						collision.back = true;
					}
				}
			}
		}

		if (Physics.Raycast(rayOriginFront, Vector3.down, out hit, Mathf.Infinity, collisionMask))
		{
			float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
			//maybe bug occurs when faliing on a angled platform? 
			if (slopeAngle != 0f && slopeAngle <= maxSlopeAngle)
			{
				//are you actually walking downwards? (
				if (Mathf.Sign(hit.normal.x) == dirX)
				{
					//if our detected ground is smaller/egual than our actual move (calculated thourgh y = tan(a) * x)
					if (hit.distance - raycastOrigin.SkinWidth <= Mathf.Tan(slopeAngle * Mathf.Deg2Rad) * Mathf.Abs(move.x))
					{
						float distance = Mathf.Abs(move.x);

						move.x = Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * distance * dirX;
						move.y -= distance * Mathf.Sin(slopeAngle * Mathf.Deg2Rad);

						currentSlopeAngle = slopeAngle;
						collision.slopeDown = true;
						collision.below = true;
						collision.front = true;
					}
				}
			}
		}
	}
}