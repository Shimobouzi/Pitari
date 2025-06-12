using UnityEngine;

/// <summary>
/// ƒŒƒCƒ„[İ’è
/// </summary>
public class LayerSetting
{
	public static LayerMask Layer_Obstacle()
	{
		return LayerMask.GetMask("Obstacle");
	}

	public static LayerMask Layer_Background()
	{
		return LayerMask.GetMask("Background");
	}

	public static LayerMask Layer_Gimmick()
	{
		return LayerMask.GetMask("Gimmick");
	}

	public static LayerMask Layer_Player()
	{
		return LayerMask.GetMask("Player");
	}

	public static LayerMask Layer_FlendlyNPC()
	{
		return LayerMask.GetMask("FlendlyNPC");
	}

	public static LayerMask Layer_HostileNPC()
	{
		return LayerMask.GetMask("HostileNPC");
	}
}