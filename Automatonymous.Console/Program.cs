using Automatonymous;

var relationship = new Relationship();
var machine = new RelationshipStateMachine();

await machine.RaiseEvent(relationship, machine.Hello);
await machine.RaiseEvent(relationship, machine.PissOff);
await machine.RaiseEvent(relationship, machine.Introduce,new Person() { Name = "furkan"});
await machine.RaiseEvent(relationship, machine.Hello);

if (relationship.CurrentState == machine.Final)
    Console.WriteLine("end");



var instanceLift = machine.CreateInstanceLift(relationship);

class Relationship
{
    public State CurrentState { get; set; }
    public string Name { get; set; }
}

class Person
{
    public string Name { get; set; }
}

class RelationshipStateMachine : AutomatonymousStateMachine<Relationship>
{
    public State Friend { get; private set; }
    public State Enemy { get; private set; }

    public Event Hello { get; private set; }
    public Event PissOff { get; private set; }
    public Event<Person> Introduce { get; private set; }

    public RelationshipStateMachine()
    {
        Event(() => Hello);
        Event(() => PissOff);
        Event(() => Introduce);

        State(() => Friend);
        State(() => Enemy);

        Initially(
            When(Introduce).Then(ctx => ctx.Instance.Name = ctx.Data.Name).TransitionTo(Friend),
            When(PissOff).TransitionTo(Enemy),
            When(Hello).TransitionTo(Initial)
        );

        During(Friend, When(PissOff).TransitionTo(Enemy));
        During(Friend, When(Hello).Finalize());
        During(Friend, When(Introduce).Then(ctx => ctx.Instance.Name = ctx.Data.Name).TransitionTo(Friend));
        During(Enemy,When(Introduce).TransitionTo(Friend));
    }

    
}



