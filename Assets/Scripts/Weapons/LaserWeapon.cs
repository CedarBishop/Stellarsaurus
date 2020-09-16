using UnityEngine;

public class LaserWeapon : Weapon
{
    public LineRenderer lineRenderer;
    public float lineRendererTimeToLive;
    protected override void ShootLogic()
    {
        base.ShootLogic();

        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position + (firingPoint.localPosition * transform.right.x * transform.right.y), playerShoot.gunOriginTransform.right, range);

        if (lineRenderer != null)
        {
            LineRenderer currentLineRenderer = Instantiate(lineRenderer, transform.position + (firingPoint.localPosition * transform.right.x * transform.right.y), Quaternion.identity).GetComponent<LineRenderer>();
            currentLineRenderer.SetPosition(0, transform.position + (firingPoint.localPosition * transform.right.x * transform.right.y));
            currentLineRenderer.SetPosition(1, transform.position + ((firingPoint.localPosition * transform.right.x * transform.right.y) * range));

            Destroy(currentLineRenderer.gameObject, lineRendererTimeToLive);
        }
        bool shouldAddHitStat = false;
        if (hits != null)
        {
            for (int i = 0; i < hits.Length; i++)
            {
                if (hits[i].collider.GetComponent<PlayerHealth>())
                {
                    hits[i].collider.GetComponent<PlayerHealth>().HitByPlayer(playerShoot.playerNumber);
                    shouldAddHitStat = true;
                }
                else if (hits[i].collider.GetComponent<Dinosaur>())
                {
                    hits[i].collider.GetComponent<Dinosaur>().TakeDamage(playerShoot.playerNumber, damage);
                    shouldAddHitStat = true;
                }
                else if (hits[i].collider.GetComponent<EnvironmentalObjectHealth>())
                {
                    hits[i].collider.GetComponent<EnvironmentalObjectHealth>().TakeDamage(damage, playerShoot.playerNumber);
                    shouldAddHitStat = true;
                }
            }

            if (shouldAddHitStat)
            {
                if (GameManager.instance.SelectedGamemode != null)
                {
                    GameManager.instance.SelectedGamemode.AddToStats(playerShoot.playerNumber, StatTypes.BulletsHit, 1);
                }
            }
        }

        if (GameManager.instance.SelectedGamemode != null)
        {
            GameManager.instance.SelectedGamemode.AddToStats(playerShoot.playerNumber, StatTypes.BulletsFired, 1);
        }
    }
}
