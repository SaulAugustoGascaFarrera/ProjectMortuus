using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu()]
public class AnimationDataListSO : ScriptableObject
{
    public List<AnimationDataSO> animationDataSOList;

    public AnimationDataSO GetAnimationDataSO(AnimationDataSO.AnimationType animationType)
    {
        foreach(AnimationDataSO animaionDataSO in animationDataSOList)
        {
            if (animaionDataSO.animationType == animationType)
            {
                return animaionDataSO;
            }
        }

        Debug.LogError("Could not found AnimationDataSO for AnimationType " + animationType);

        return null;
    }

}
