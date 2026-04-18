using System.Collections;
using UnityEngine;

public class Tron : EnemyBase
{
    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Update()
    {
        base.Update();
    }

    public override void Move()
    {
        base.Move();
    }

    public override void Iddle()
    {
        base.Iddle();
    }

    public override void StartWait()
    {
        base.StartWait();
    }

    public override IEnumerator Wait()
    {
        return base.Wait();
    }

    public override void ReturnToOrigin()
    {
        base.ReturnToOrigin();
    }
}
