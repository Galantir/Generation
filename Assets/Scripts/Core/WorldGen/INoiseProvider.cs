using UnityEngine;
using System.Collections;

public interface INoiseProvider {

	float GetValue(float x, float z);
}
