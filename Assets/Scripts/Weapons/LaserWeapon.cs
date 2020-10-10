using UnityEngine;

public class LaserWeapon : Weapon
{
    [Header("Laser Variables")]
    public LineRenderer lineRenderer;
    public float lineRendererTimeToLive;
    protected override void ShootLogic()
    {
        base.ShootLogic();

        RaycastHit2D[] hits = Physics2D.RaycastAll(firingPoint.transform.position, player.gunOriginTransform.right, range);

        if (lineRenderer != null)
        {
            LineRenderer currentLineRenderer = Instantiate(lineRenderer, transform.position + (firingPoint.localPosition * transform.right.x * transform.right.y), Quaternion.identity).GetComponent<LineRenderer>();
            currentLineRenderer.SetPosition(0, firingPoint.transform.position);
            currentLineRenderer.SetPosition(1, firingPoint.transform.position + ((range * transform.right)));

            Destroy(currentLineRenderer.gameObject, lineRendererTimeToLive);
        }
        bool shouldAddHitStat = false;
        if (hits != null)
        {
            for (int i = 0; i < hits.Length; i++)
            {
                if (hits[i].collider.GetComponent<PlayerHealth>())
                {
                    hits[i].collider.GetComponent<PlayerHealth>().HitByPlayer(player.playerNumber);
                    shouldAddHitStat = true;
                }
                else if (hits[i].collider.GetComponent<Dinosaur>())
                {
                    hits[i].collider.GetComponent<Dinosaur>().TakeDamage(player.playerNumber, damage);
                    shouldAddHitStat = true;
                }
                else if (hits[i].collider.GetComponent<EnvironmentalObjectHealth>())
                {
                    hits[i].collider.GetComponent<EnvironmentalObjectHealth>().TakeDamage(damage, player.playerNumber);
                    shouldAddHitStat = true;
                }
            }

            if (shouldAddHitStat)
            {
                if (GameManager.instance.SelectedGamemode != null)
                {
                    GameManager.instance.SelectedGamemode.AddToStats(player.playerNumber, StatTypes.BulletsHit, 1);
                }
            }
        }

        if (GameManager.instance.SelectedGamemode != null)
        {
            GameManager.instance.SelectedGamemode.AddToStats(player.playerNumber, StatTypes.BulletsFired, 1);
        }
    }
}
