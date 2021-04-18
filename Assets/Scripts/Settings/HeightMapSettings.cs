using UnityEngine;

namespace Settings {

	/** Control the values of the parameters of the Noise function through the editor */
	[CreateAssetMenu]
	public class HeightMapSettings : UpdatableSettings {
		/** Generic settings used by Perlin */
		public NoiseSettings noiseSettings;
		/** The Height Variation */
		public AnimationCurve heightCurve;
		
	}
}