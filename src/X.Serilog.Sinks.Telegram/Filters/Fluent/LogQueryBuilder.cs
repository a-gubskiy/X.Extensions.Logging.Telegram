namespace X.Serilog.Sinks.Telegram.Filters.Fluent;

public class LogQueryBuilder :
    IMessageRuleBuilder,
    IExceptionRuleBuilder,
    IPropertiesRuleBuilder,
    ILevelRuleBuilder
{
    private ConditionGroup _currentGroup = new();
    private readonly List<ConditionGroup> _groups = [];

    public IMessageRuleBuilder Message => this;
    public IExceptionRuleBuilder Exception => this;
    public IPropertiesRuleBuilder Properties => this;
    public ILevelRuleBuilder Level => this;

    public LogQueryBuilder()
    {
        _groups.Add(_currentGroup);
    }

    public ILogQueryBuilder And() => this;

    public ILogQueryBuilder Or()
    {
        _currentGroup = new ConditionGroup(isOrGroup: true);
        _groups.Add(_currentGroup);
        return this;
    }

    // public LogQueryBuilder And(LogEntryPredicate predicate)
    // {
    //     _currentGroup.Conditions.Add(new Condition(predicate));
    //     return this;
    // }

    // public LogQueryBuilder Or(LogEntryPredicate predicate)
    // {
    //     _currentGroup = new ConditionGroup(isOrGroup: true);
    //     _groups.Add(_currentGroup);
    //     _currentGroup.Conditions.Add(new Condition(predicate));
    //     return this;
    // }

    public bool Evaluate(LogEntry entry)
    {
        foreach (var group in _groups)
        {
            if (group.IsOrGroup)
            {
                if (group.Conditions.Any(condition => EvaluateCondition(condition, entry)))
                    return true;
            }
            else
            {
                if (group.Conditions.All(condition => EvaluateCondition(condition, entry)))
                    return true;
            }
        }

        return false;
    }

    private bool EvaluateCondition(object condition, LogEntry entry)
    {
        return condition switch
        {
            Condition simpleCondition => simpleCondition.Predicate(entry),
            ConditionGroup group => group.IsOrGroup
                ? group.Conditions.Any(c => EvaluateCondition(c, entry))
                : group.Conditions.All(c => EvaluateCondition(c, entry)),
            _ => false
        };
    }

    #region IMessageRuleBuilder

    ILogQueryBuilder IMessageRuleBuilder.Contains(string key)
    {
        var condition = new Condition(e => e.RenderedMessage?.Contains(key) ?? false);
        _currentGroup.Conditions.Add(condition);
        return this;
    }

    ILogQueryBuilder IMessageRuleBuilder.NotContains(string substring)
    {
        var condition = new Condition(e => !e.RenderedMessage?.Contains(substring) ?? false);
        _currentGroup.Conditions.Add(condition);
        return this;
    }

    ILogQueryBuilder IMessageRuleBuilder.Equals(string substring, StringComparison comparison)
    {
        var condition = new Condition(e => e.RenderedMessage?.Equals(substring, comparison) ?? false);
        _currentGroup.Conditions.Add(condition);
        return this;
    }

    ILogQueryBuilder IMessageRuleBuilder.NotEquals(string substring, StringComparison comparison)
    {
        var condition = new Condition(e => !e.RenderedMessage?.Equals(substring, comparison) ?? false);
        _currentGroup.Conditions.Add(condition);
        return this;
    }

    ILogQueryBuilder IMessageRuleBuilder.Null()
    {
        var condition = new Condition(e => e.RenderedMessage is null);
        _currentGroup.Conditions.Add(condition);
        return this;
    }

    ILogQueryBuilder IMessageRuleBuilder.NotNull()
    {
        var condition = new Condition(e => e.RenderedMessage is not null);
        _currentGroup.Conditions.Add(condition);
        return this;
    }

    #endregion

    #region IExceptionRuleBuilder

    ILogQueryBuilder IExceptionRuleBuilder.Contains(string key)
    {
        var condition = new Condition(e => e.Exception?.Contains(key) ?? false);
        _currentGroup.Conditions.Add(condition);
        return this;
    }

    ILogQueryBuilder IExceptionRuleBuilder.NotContains(string substring)
    {
        var condition = new Condition(e => !e.Exception?.Contains(substring) ?? false);
        _currentGroup.Conditions.Add(condition);
        return this;
    }

    ILogQueryBuilder IExceptionRuleBuilder.Equals(string substring, StringComparison comparison)
    {
        var condition = new Condition(e => e.Exception?.Equals(substring, comparison) ?? false);
        _currentGroup.Conditions.Add(condition);
        return this;
    }

    ILogQueryBuilder IExceptionRuleBuilder.NotEquals(string substring, StringComparison comparison)
    {
        var condition = new Condition(e => !e.Exception?.Equals(substring, comparison) ?? false);
        _currentGroup.Conditions.Add(condition);
        return this;
    }

    ILogQueryBuilder IExceptionRuleBuilder.Null()
    {
        var condition = new Condition(e => e.Exception is null);
        _currentGroup.Conditions.Add(condition);
        return this;
    }

    ILogQueryBuilder IExceptionRuleBuilder.NotNull()
    {
        var condition = new Condition(e => e.Exception is not null);
        _currentGroup.Conditions.Add(condition);
        return this;
    }

    #endregion

    #region IPropertiesRuleBuilder

    ILogQueryBuilder IPropertiesRuleBuilder.Contains(string key)
    {
        var condition = new Condition(e => e.Properties?.ContainsKey(key) ?? false);
        _currentGroup.Conditions.Add(condition);
        return this;
    }

    ILogQueryBuilder IPropertiesRuleBuilder.NotContains(string key)
    {
        var condition = new Condition(e => !e.Properties?.ContainsKey(key) ?? false);
        _currentGroup.Conditions.Add(condition);
        return this;
    }

    ILogQueryBuilder IPropertiesRuleBuilder.Equals(string key, string value, StringComparison comparison)
    {
        var condition = new Condition(e =>
        {
            if (!e.Properties?.ContainsKey(key) ?? true)
            {
                return false;
            }

            return e.Properties![key].Equals(value, comparison);
        });
        _currentGroup.Conditions.Add(condition);
        return this;
    }

    ILogQueryBuilder IPropertiesRuleBuilder.NotEquals(string key, string value, StringComparison comparison)
    {
        var condition = new Condition(e =>
        {
            if (!e.Properties?.ContainsKey(key) ?? true)
            {
                return false;
            }

            return e.Properties![key].Equals(value, comparison);
        });
        _currentGroup.Conditions.Add(condition);
        return this;
    }

    ILogQueryBuilder IPropertiesRuleBuilder.Null()
    {
        var condition = new Condition(e => e.Properties is null);
        _currentGroup.Conditions.Add(condition);
        return this;
    }

    ILogQueryBuilder IPropertiesRuleBuilder.NotNull()
    {
        var condition = new Condition(e => e.Properties is not null);
        _currentGroup.Conditions.Add(condition);
        return this;
    }

    #endregion

    #region ILevelRuleBuilder

    ILogQueryBuilder ILevelRuleBuilder.InRange(LogEventLevel min, LogEventLevel max)
    {
        throw new NotImplementedException();
    }

    ILogQueryBuilder ILevelRuleBuilder.NotInRange(LogEventLevel min, LogEventLevel max)
    {
        throw new NotImplementedException();
    }

    ILogQueryBuilder ILevelRuleBuilder.Equals(LogEventLevel level)
    {
        var condition = new Condition(e => e.Level.Equals(level));
        _currentGroup.Conditions.Add(condition);
        return this;
    }

    ILogQueryBuilder ILevelRuleBuilder.NotEquals(LogEventLevel level)
    {
        var condition = new Condition(e => !e.Level.Equals(level));
        _currentGroup.Conditions.Add(condition);
        return this;
    }

    #endregion
}