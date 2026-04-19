using System;

public class EnemyTree
{
    private DecisionNode rootNode;
    private DecisionNode moveOrWaitNode;
    private DecisionNode isReturningNode;

    public void InitializeNodes()
    {
        ActionNode iddleNode = new ActionNode(EnemyAction => EnemyAction.Iddle());
        ActionNode ReturnNode = new ActionNode(EnemyAction => EnemyAction.ReturnToPositionOrigin());
        ActionNode MoveNode = new ActionNode(EnemyAction => EnemyAction.Move());
        ActionNode WaitNode = new ActionNode(EnemyAction => EnemyAction.StartWait());


        moveOrWaitNode = new QuestionNode(context => !context._LOS.IsOnFront(context._selfTransform), MoveNode, WaitNode);
        isReturningNode = new QuestionNode(context => context._returnToOrigin, ReturnNode, moveOrWaitNode);
        rootNode = new QuestionNode(context => context._isOn == true, isReturningNode, iddleNode );
    }

    public void Evaluate (EnemyBase enemy, EnemyContext context)
    {
        rootNode.Evaluate(enemy, context);
    }
}

public class ActionNode : DecisionNode
{
    private Action <EnemyBase> action;

    public ActionNode(Action <EnemyBase> actionToPass)
    {
        action = actionToPass;
    }

    public override void Evaluate (EnemyBase enemy, EnemyContext context)
    {
        action(enemy);
    }
}
