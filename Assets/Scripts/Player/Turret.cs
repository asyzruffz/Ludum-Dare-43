using UnityEngine;
using DG.Tweening;

public class Turret : MonoBehaviour {

    [SerializeField]
    MissilePool missilePool;

    [SerializeField]
    Transform aimTarget;

    [Space]
    [SerializeField]
    Vector3 closePosition;
    [SerializeField]
    Vector3 closeRotation;

    [Space]
    [SerializeField]
    Vector3 openPosition;
    [SerializeField]
    Vector3 openRotation;

    [Space]
    [SerializeField]
    float transitionTime = 1;

    Sequence readySequence;
    Missile tempMissile;
    bool busy = false;
    
    public void ReadyTurret () {
        if (busy) return; // Don't start opening when currently is opening
        busy = true;

        readySequence = DOTween.Sequence ();
        readySequence.Append (transform.DOLocalMove (openPosition, transitionTime).SetEase (Ease.Linear));
        readySequence.Join (transform.DOLocalRotate (openRotation, transitionTime).SetEase (Ease.Linear));

        readySequence.AppendInterval (0.5f);
        readySequence.OnComplete (LaunchMissile);
    }

    public void CloseTurret () {
        busy = false;
        if (readySequence != null && readySequence.IsActive()) {
            readySequence.Kill (); // Prevent the turret from shooting (remnant from ready sequence)
        }

        Sequence closeSequence = DOTween.Sequence ();
        closeSequence.Append (transform.DOLocalMove (closePosition, transitionTime).SetEase (Ease.Linear));
        closeSequence.Join (transform.DOLocalRotate (closeRotation, transitionTime).SetEase (Ease.Linear));
    }

    void LaunchMissile () {
        tempMissile = missilePool.GetAvailable ();
        tempMissile.transform.position = transform.position;
        tempMissile.transform.rotation = transform.rotation;

        Vector3 mov = (Vector3.forward.RotateBy (transform.rotation)) * 6;
        Vector3 rot = Quaternion.LookRotation (aimTarget.position - (tempMissile.transform.position + mov), Vector3.up).eulerAngles;

        Sequence missileSequence = DOTween.Sequence ();
        missileSequence.Append (tempMissile.transform.DOMove (mov, 1f).SetRelative ().SetEase (Ease.OutCubic));
        missileSequence.Insert (0.3f, tempMissile.transform.DORotate (rot, 0.5f).SetEase (Ease.OutCubic));
        missileSequence.OnComplete (MissileRelease);
    }

    void MissileRelease () {
        tempMissile.GoTo (aimTarget.position);
        tempMissile = null;
        busy = false;
    }
}
