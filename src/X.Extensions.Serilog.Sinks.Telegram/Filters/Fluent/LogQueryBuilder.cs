namespace X.Extensions.Serilog.Sinks.Telegram.Filters.Fluent;

public class LogQueryBuilder :
    IMessageRuleBuilder,
    IExceptionRuleBuilder,
    IPropertiesRuleBuilder,
    ILevelRuleBuilder,
    IFilterExecutor
{
    private ConditionGroup _currentGroup = new();
    private readonly List<ConditionGroup> _groups = [];

    public IMessageRuleBuilder Message => this;
    public IExceptionRuleBuilder Exception => this;
    public IPropertiesRuleBuilder Properties => this;
    public ILevelRuleBuilder Level => this;

    private LogQueryBuilder()
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

    public bool Evaluate(LogEvent entry)
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

    private bool EvaluateCondition(object condition, LogEvent entry)
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

    ILogQueryBuilder IMessageRuleBuilder.Contains(string substring)
    {
        var condition = new Condition(e => e.RenderMessage().Contains(substring));
        _currentGroup.Conditions.Add(condition);
        return this;
    }

    ILogQueryBuilder IMessageRuleBuilder.NotContains(string substring)
    {
        var condition = new Condition(e => !e.RenderMessage().Contains(substring));
        _currentGroup.Conditions.Add(condition);
        return this;
    }

    ILogQueryBuilder IMessageRuleBuilder.Equals(string substring, StringComparison comparison)
    {
        var condition = new Condition(e => e.RenderMessage().Equals(substring, comparison));
        _currentGroup.Conditions.Add(condition);
        return this;
    }

    ILogQueryBuilder IMessageRuleBuilder.NotEquals(string substring, StringComparison comparison)
    {
        var condition = new Condition(e => !e.RenderMessage().Equals(substring, comparison));
        _currentGroup.Conditions.Add(condition);
        return this;
    }

    ILogQueryBuilder IMessageRuleBuilder.Null()
    {
        // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
        var condition = new Condition(e => e.RenderMessage() is null);
        _currentGroup.Conditions.Add(condition);
        return this;
    }

    ILogQueryBuilder IMessageRuleBuilder.NotNull()
    {
        // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
        var condition = new Condition(e => e.RenderMessage() is not null);
        _currentGroup.Conditions.Add(condition);
        return this;
    }

    #endregion

    #region IExceptionRuleBuilder

    ILogQueryBuilder IExceptionRuleBuilder.Contains(string substring)
    {
        var condition = new Condition(e => e.Exception?.Message.Contains(substring) ?? false);
        _currentGroup.Conditions.Add(condition);
        return this;
    }

    ILogQueryBuilder IExceptionRuleBuilder.NotContains(string substring)
    {
        var condition = new Condition(e => !e.Exception?.Message.Contains(substring) ?? false);
        _currentGroup.Conditions.Add(condition);
        return this;
    }

    ILogQueryBuilder IExceptionRuleBuilder.Equals(string substring, StringComparison comparison)
    {
        var condition = new Condition(e => e.Exception?.Message.Equals(substring, comparison) ?? false);
        _currentGroup.Conditions.Add(condition);
        return this;
    }

    ILogQueryBuilder IExceptionRuleBuilder.NotEquals(string substring, StringComparison comparison)
    {
        var condition = new Condition(e => !e.Exception?.Message.Equals(substring, comparison) ?? false);
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

    ILogQueryBuilder IPropertiesRuleBuilder.Contains(string substring)
    {
        var condition = new Condition(e => e.Properties.ContainsKey(substring));
        _currentGroup.Conditions.Add(condition);
        return this;
    }

    ILogQueryBuilder IPropertiesRuleBuilder.NotContains(string key)
    {
        var condition = new Condition(e => !e.Properties.ContainsKey(key));
        _currentGroup.Conditions.Add(condition);
        return this;
    }

    ILogQueryBuilder IPropertiesRuleBuilder.Equals(string key, string value, StringComparison comparison)
    {
        var condition = new Condition(
            e => e.Properties.TryGetValue(key, out var prop) &&
                 prop.ToString().Equals(value, comparison));
        _currentGroup.Conditions.Add(condition);
        return this;
    }

    ILogQueryBuilder IPropertiesRuleBuilder.NotEquals(string key, string value, StringComparison comparison)
    {
        var condition = new Condition(
            e => !(e.Properties.TryGetValue(key, out var prop) &&
                   prop.ToString().Equals(value, comparison)));
        _currentGroup.Conditions.Add(condition);
        return this;
    }

    ILogQueryBuilder IPropertiesRuleBuilder.Null()
    {
        // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
        var condition = new Condition(e => e.Properties is null);
        _currentGroup.Conditions.Add(condition);
        return this;
    }

    ILogQueryBuilder IPropertiesRuleBuilder.NotNull()
    {
        // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
        var condition = new Condition(e => e.Properties is not null);
        _currentGroup.Conditions.Add(condition);
        return this;
    }

    #endregion

    #region ILevelRuleBuilder

    ILogQueryBuilder ILevelRuleBuilder.InRange(LogEventLevel min, LogEventLevel max)
    {
        var minInt = (int)min;
        var maxInt = (int)max;

        var condition = new Condition(e =>
        {
            var logLevelInt = (int)e.Level;
            return minInt <= logLevelInt && logLevelInt <= maxInt;
        });

        _currentGroup.Conditions.Add(condition);
        return this;
    }

    ILogQueryBuilder ILevelRuleBuilder.NotInRange(LogEventLevel min, LogEventLevel max)
    {
        var minInt = (int)min;
        var maxInt = (int)max;

        var condition = new Condition(e =>
        {
            var logLevelInt = (int)e.Level;
            return !(logLevelInt >= minInt && logLevelInt <= maxInt);
        });

        _currentGroup.Conditions.Add(condition);
        return this;
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

    public static LogQueryBuilder Create()
    {
        return new LogQueryBuilder();
    }
}