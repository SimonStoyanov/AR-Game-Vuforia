using UnityEngine;
using Vuforia;

public class MarkerDetectionScript : MonoBehaviour,	ITrackableEventHandler
{
	private TrackableBehaviour mTrackableBehaviour;
	bool markerFound = false;

    private EventSystem event_system = null;

	void Start()
	{
		mTrackableBehaviour = GetComponent<TrackableBehaviour>();
		if (mTrackableBehaviour)
		{
			mTrackableBehaviour.RegisterTrackableEventHandler(this);
		}
	}

    public void SetEventSystem(EventSystem es)
    {
        event_system = es;
    }


	public void OnTrackableStateChanged( TrackableBehaviour.Status previousStatus,
										 TrackableBehaviour.Status newStatus)
	{
		if (newStatus == TrackableBehaviour.Status.DETECTED ||
			newStatus == TrackableBehaviour.Status.TRACKED ||
			newStatus == TrackableBehaviour.Status.EXTENDED_TRACKED)
		{
			OnTrackingFound();
		}
		else
		{
			OnTrackingLost();
		}
	}


	private void OnTrackingFound()
	{
        EventSystem.Event ev = new EventSystem.Event(EventSystem.EventType.TRACKER_FOUND);
        ev.tracker_found.tracker_go = gameObject;

        if (event_system != null)
            event_system.SendEvent(ev);

		markerFound = true;	
	}


	private void OnTrackingLost()
	{
        EventSystem.Event ev = new EventSystem.Event(EventSystem.EventType.TRACKER_LOST);
        ev.tracker_lost.tracker_go = gameObject;

        if (event_system != null)
            event_system.SendEvent(ev);

        markerFound = false;
	}

	public bool markerDetected()
	{
		return markerFound;
	}

}
