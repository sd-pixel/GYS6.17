using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

// A behaviour that is attached to a playable
public class TimelinePlayableBehaviour : PlayableBehaviour
{
    public PlayableDirector director;
    public float loopStartTime;

    // ��ӵ�е�ͼ�ο�ʼ����ʱ����
    public override void OnGraphStart(Playable playable)
    {
        
    }

    // ��ӵ�е�ͼ��ֹͣ����ʱ����
    public override void OnGraphStop(Playable playable)
    {
        
    }

    // ���ɲ��Ŷ����״̬����Ϊ����ʱ����
    public override void OnBehaviourPlay(Playable playable, FrameData info)
    {
        director.time = loopStartTime;
    }

    // ������״̬����Ϊ��ͣʱ����
    public override void OnBehaviourPause(Playable playable, FrameData info)
    {
        
    }

    // ��״̬����Ϊ����ʱ������ÿһ֡
    public override void PrepareFrame(Playable playable, FrameData info)
    {

    }
}
