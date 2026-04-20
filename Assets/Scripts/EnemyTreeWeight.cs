using UnityEngine;

public class EnemyTreeWeight
{
    private DecisionNode rootNode;
    private DecisionNode returningOrQuestionNode;
    private DecisionNode wanderOrMoveNode;
    private DecisionNode moveOrDecisionNode;

    public void InitTree()
    {
        ActionNode idleNode = new ActionNode(enemy => enemy.Idle());
        ActionNode returnNode = new ActionNode(enemy => enemy.ReturnToPositionOrigin());
        ActionNode moveNode = new ActionNode(enemy => enemy.Move());
        EnemyWeightNode wanderNode = new EnemyWeightNode(new (float, System.Action<EnemyBase>)[]
            {
                (50f, enemy => enemy.Wander()),
                (50f, enemy => enemy.Wait()),
            }
        );
        EnemyWeightNode weightNode = new EnemyWeightNode(
            new (float, System.Action<EnemyBase>)[]
            {
                (33f, enemy => enemy.MoveRandom3()),
                (33f, enemy => enemy.MoveRandom2()),
                (33f, enemy => enemy.MoveRandom1()),
            }
        );

        moveOrDecisionNode = new QuestionNode(context => context._decisionMoment == false, moveNode, weightNode);
        wanderOrMoveNode = new QuestionNode(context => context._isWander == true || context._LOS.IsOnFront(context._selfTransform) == true, wanderNode, moveOrDecisionNode);
        returningOrQuestionNode = new QuestionNode(context => context._returnToOrigin == false, wanderOrMoveNode, returnNode);
        rootNode = new QuestionNode(context => context._isOn == true, returningOrQuestionNode, idleNode);
    }

    public void Evaluate(EnemyBase enemy, EnemyContext context)
    {
        rootNode.Evaluate(enemy, context);
    }
}
