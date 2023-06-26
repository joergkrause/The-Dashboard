using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using TheDashboard.Ui.Attributes;
using TheDashboard.Ui.ValidationAttributes;

namespace TheDashboard.Ui;

public sealed class AutoFormField<TModel>
{
  private static readonly MethodInfo s_eventCallbackFactoryCreate = GetEventCallbackFactoryCreate();

  private readonly AutoForm<TModel> _form;
  private RenderFragment? _editorTemplate;
  private RenderFragment? _fieldValidationTemplate;

  private bool _hasDependentFields;
  private string _dependsOnProperty;

  public bool Visible = true;

  public event EventHandler? ValueChanged;

  private AutoFormField(AutoForm<TModel> form, PropertyInfo propertyInfo)
  {
    _form = form;
    Property = propertyInfo;
  }

  internal static List<AutoFormField<TModel>> Create(AutoForm<TModel> form)
  {
    var result = new List<AutoFormField<TModel>>();
    var fieldDependencies = new Dictionary<string, string>();
    var properties = typeof(TModel).GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy);
    foreach (var prop in properties)
    {
      // Skip readonly properties
      if (prop.SetMethod == null || prop.GetCustomAttribute<HiddenAttribute>() is not null)
        continue;

      if (prop.GetCustomAttribute<EditableAttribute>() is { } editor && !editor.AllowEdit)
        continue;

      var field = new AutoFormField<TModel>(form, prop);

      // this property's field depends on somebody else
      if (prop.GetCustomAttribute<ShowIfDependentAttribute>() is not null)
      {
        field._dependsOnProperty = prop.GetCustomAttribute<ShowIfDependentAttribute>().CheckProperty;
      }

      if (typeof(TModel).GetCustomAttribute<CategoryAttribute>() is not null)
      {
        field.ShowHelp = typeof(TModel).GetCustomAttribute<CategoryAttribute>().Category != "Tablet";
      }

      result.Add(field);

    }
    // mark the field that changes influence others
    foreach (var field in result)
    {
      field._hasDependentFields = result.Any(f => f._dependsOnProperty == field.Property.Name);
    }
    // set initial state of dependent fields    
    return result;
  }

  public bool Enabled { get; set; }

  public bool ShowHelp { get; set; } = true;

  public PropertyInfo Property { get; }
  public string EditorId
  {
    get
    {
      return $"{_form.BaseEditorId}_{Property.Name}";
    }
  }
  public TModel Owner => _form.Model;

  public int Order
  {
    get
    {
      var displayAttribute = Property.GetCustomAttribute<DisplayAttribute>();
      if (displayAttribute != null)
      {
        return displayAttribute.GetOrder() ?? default;
      }

      return default;
    }
  }

  public string DisplayName
  {
    get
    {
      var displayAttribute = Property.GetCustomAttribute<DisplayAttribute>();
      if (displayAttribute != null)
      {
        var displayName = displayAttribute.GetName();
        if (!string.IsNullOrEmpty(displayName))
          return displayName;
      }

      var displayNameAttribute = Property.GetCustomAttribute<DisplayNameAttribute>();
      if (displayNameAttribute != null)
      {
        var displayName = displayNameAttribute.DisplayName;
        if (!string.IsNullOrEmpty(displayName))
          return displayName;
      }

      return Property.Name;
    }
  }

  public string? Description
  {
    get
    {
      var displayAttribute = Property.GetCustomAttribute<DisplayAttribute>();
      if (displayAttribute != null)
      {
        var description = displayAttribute.GetDescription();
        if (!string.IsNullOrEmpty(description))
          return description;
      }

      var descriptionAttribute = Property.GetCustomAttribute<DescriptionAttribute>();
      if (descriptionAttribute != null)
      {
        var description = descriptionAttribute.Description;
        if (!string.IsNullOrEmpty(description))
          return description;
      }

      return null;
    }
  }

  public Type PropertyType => Property.PropertyType;

  public object Value
  {
    get => Property.GetValue(Owner);
    set
    {
      if (Property.SetMethod != null && !Equals(Value, value))
      {
        Property.SetValue(Owner, value);
        ValueChanged?.Invoke(this, EventArgs.Empty);
        // inform other that they need to render
        if (_hasDependentFields)
        {
          CheckDependencies(_form, Property.Name);
        }
      }
    }
  }

  internal static void CheckDependencies(AutoForm<TModel> form, string propertyName)
  {
    try
    {
      // has another prop an attribute hat points to this field?
      var dependentants = form.fields?.Where(f => f.Property.GetCustomAttribute<ShowIfDependentAttribute>()?.CheckProperty == propertyName);
      if (dependentants != null)
      {
        var c = dependentants.Count();
        dependentants.ToList().ForEach(d =>
        {
          var propAttr = d.Property.GetCustomAttribute<ShowIfDependentAttribute>();
          var negate = propAttr.ExcludeCompareValue;
          var checkProp = typeof(TModel).GetProperty(propAttr.CheckProperty);
          var depValue = checkProp.GetValue(form.Model);
          // make others visible if conditions apply
          d.Visible = depValue switch
          {
            var dv when !negate && Convert.ToBoolean(dv) == Convert.ToBoolean(propAttr.CompareToValue) => true,
            var dv when !negate && propAttr.CompareToValue.Equals(dv) => true,
            var dv when !negate && propAttr.CompareToValue.Equals(dv) => false,
            var dv when negate && Convert.ToBoolean(dv) != Convert.ToBoolean(propAttr.CompareToValue) => true,
            var dv when negate && !propAttr.CompareToValue.Equals(dv) => true,
            var dv when negate && !propAttr.CompareToValue.Equals(dv) => false,
            _ => false
          };
        });
        form.Rerender();
      }
    }
    catch (Exception ex)
    {
      Debug.WriteLine(ex.Message);
    }
  }

  public RenderFragment EditorTemplate
  {
    get
    {
      if (_editorTemplate != null)
        return _editorTemplate;

      // () => Owner.Property
      var access = Expression.Property(Expression.Constant(Owner, typeof(TModel)), Property);
      var lambda = Expression.Lambda(typeof(Func<>).MakeGenericType(PropertyType), access);

      // Create(object receiver, Action<object> callback
      var method = s_eventCallbackFactoryCreate.MakeGenericMethod(PropertyType);

      // value => Field.Value = value;
      var changeHandlerParameter = Expression.Parameter(PropertyType);
      var body = Expression.Assign(Expression.Property(Expression.Constant(this), nameof(Value)), Expression.Convert(changeHandlerParameter, typeof(object)));
      var changeHandlerLambda = Expression.Lambda(typeof(Action<>).MakeGenericType(PropertyType), body, changeHandlerParameter);
      var changeHandler = method.Invoke(EventCallback.Factory, new object[] { this, changeHandlerLambda.Compile() });

      var uiAdditions = Property.GetCustomAttributes<UIHintAttribute>().SelectMany(ui => ui.ControlParameters);

      return _editorTemplate ??= builder =>
      {
        var (componentType, classes, additionalAttributes) = GetEditorType(Property);
        builder.OpenComponent(0, componentType);
        builder.AddAttribute(1, "Value", Value);
        builder.AddAttribute(2, "ValueChanged", changeHandler);
        builder.AddAttribute(3, "ValueExpression", lambda);
        builder.AddAttribute(4, "id", EditorId);
        builder.AddAttribute(5, "class", classes);
        if (!Enabled)
        {
          builder.AddAttribute(6, "disabled", "disabled");
        }
        builder.AddMultipleAttributes(7, additionalAttributes?.Union(uiAdditions));
        builder.CloseComponent();
      };
    }
  }

  public RenderFragment? FieldValidationTemplate
  {
    get
    {
      if (!_form.EnableFieldValidation || !Visible)
        return null;

      return _fieldValidationTemplate ??= builder =>
      {
        // () => Owner.Property
        var access = Expression.Property(Expression.Constant(Owner, typeof(TModel)), Property);
        var lambda = Expression.Lambda(typeof(Func<>).MakeGenericType(PropertyType), access);

        builder.OpenComponent(0, typeof(ValidationMessage<>).MakeGenericType(PropertyType));
        builder.AddAttribute(1, "For", lambda);
        builder.CloseComponent();
      };
    }
  }

  private (Type ComponentType, string? EditorClass, IEnumerable<KeyValuePair<string, object>> AdditionalAttributes) GetEditorType(PropertyInfo property)
  {
    var editorAttributes = property.GetCustomAttributes<EditorAttribute>();
    foreach (var editorAttribute in editorAttributes)
    {
      if (editorAttribute.EditorBaseTypeName == typeof(InputBase<>).AssemblyQualifiedName)
        return (Type.GetType(editorAttribute.EditorTypeName, throwOnError: true)!, _form.EditorClass, new Dictionary<string, object>());
    }

    if (property.PropertyType == typeof(bool))
      return (typeof(InputCheck), _form.CheckboxClass, new Dictionary<string, object>());

    if (property.PropertyType == typeof(string))
    {
      var dataType = property.GetCustomAttributes<DataTypeAttribute>().SingleOrDefault();
      if (dataType != null)
      {
        if (dataType.DataType == DataType.Date)
          return (typeof(InputDateTime), _form.EditorClass, new[] { KeyValuePair.Create<string, object>("type", "date") });

        if (dataType.DataType == DataType.DateTime)
          return (typeof(InputDateTime), _form.EditorClass, new[] { KeyValuePair.Create<string, object>("type", "datetime-local") });

        if (dataType.DataType == DataType.EmailAddress)
          return (typeof(InputText), _form.EditorClass, new[] { KeyValuePair.Create<string, object>("type", "email") });

        if (dataType.DataType == DataType.MultilineText)
          return (typeof(InputBigText), _form.EditorClass, new Dictionary<string, object>());

        if (dataType.DataType == DataType.Password)
          return (typeof(InputText), _form.EditorClass, new[] { KeyValuePair.Create<string, object>("type", "password") });

        if (dataType.DataType == DataType.PhoneNumber)
          return (typeof(InputText), _form.EditorClass, new[] { KeyValuePair.Create<string, object>("type", "tel") });

        if (dataType.DataType == DataType.Time)
          return (typeof(InputText), _form.EditorClass, new[] { KeyValuePair.Create<string, object>("type", "time") });

        if (dataType.DataType == DataType.Url)
          return (typeof(InputText), _form.EditorClass, new[] { KeyValuePair.Create<string, object>("type", "url") });

      }
      var valueList = property.GetCustomAttributes<ValueListAttribute>().SingleOrDefault();
      if (valueList != null)
      {
        return (typeof(InputDropDown<>).MakeGenericType(property.PropertyType), _form.SelectClass, new Dictionary<string, object>());
      }

      return (typeof(InputText), _form.EditorClass, new Dictionary<string, object>());
    }

    if (property.PropertyType == typeof(IList<IBrowserFile>))
    {
      var dataType = property.GetCustomAttributes<DataTypeAttribute>().SingleOrDefault();
      if (dataType != null && dataType.DataType == DataType.Upload)
      {
        return (typeof(InputFile), _form.EditorClass, new[] { KeyValuePair.Create<string, object>("multiple", "multiple") });
      }
    }

    if (property.PropertyType == typeof(short))
      return (typeof(InputNumber<short>), _form.EditorClass, new Dictionary<string, object>());

    if (property.PropertyType == typeof(int))
      return (typeof(InputNumber<int>), _form.EditorClass, new Dictionary<string, object>());

    if (property.PropertyType == typeof(long))
      return (typeof(InputNumber<long>), _form.EditorClass, new Dictionary<string, object>());

    if (property.PropertyType == typeof(float))
      return (typeof(InputNumber<float>), _form.EditorClass, new Dictionary<string, object>());

    if (property.PropertyType == typeof(double))
      return (typeof(InputNumber<double>), _form.EditorClass, new Dictionary<string, object>());

    if (property.PropertyType == typeof(decimal))
      return (typeof(InputNumber<decimal>), _form.EditorClass, new Dictionary<string, object>());

    if (property.PropertyType == typeof(DateTime))
    {
      var dataType = property.GetCustomAttribute<DataTypeAttribute>();
      if (dataType != null && dataType.DataType == DataType.Date)
        return (typeof(InputDateTime), _form.EditorClass, new[] { KeyValuePair.Create<string, object>("type", "date") });
      if (dataType != null && dataType.DataType == DataType.DateTime)
        return (typeof(InputDateTime), _form.EditorClass, new[] { KeyValuePair.Create<string, object>("type", "datetime-local") });
    }

    if (property.PropertyType == typeof(DateTime?))
    {
      var dataType = property.GetCustomAttribute<DataTypeAttribute>();
      if (dataType != null && dataType.DataType == DataType.Date)
        return (typeof(InputDateTimeNullable), _form.EditorClass, new[] { KeyValuePair.Create<string, object>("type", "date") });
      if (dataType != null && dataType.DataType == DataType.Date)
        return (typeof(InputDateTimeNullable), _form.EditorClass, new[] { KeyValuePair.Create<string, object>("type", "datetime-local") });
    }

    if (property.PropertyType == typeof(DateTimeOffset))
    {
      var dataType = property.GetCustomAttribute<DataTypeAttribute>();
      if (dataType != null && dataType.DataType == DataType.Date)
        return (typeof(InputDate<DateTimeOffset>), _form.EditorClass, new Dictionary<string, object>());

      return (typeof(InputDateTime), _form.EditorClass, new Dictionary<string, object>());
    }

    if (property.PropertyType.IsEnum)
    {
      if (!property.PropertyType.IsDefined(typeof(FlagsAttribute), inherit: true))
        return (typeof(InputEnumSelect<>).MakeGenericType(property.PropertyType), _form.SelectClass, new Dictionary<string, object>());
    }

    // nullable Enum
    if (property.PropertyType.IsGenericType && property.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>) && property.PropertyType.GetGenericArguments()[0].IsEnum)
    {
      return (typeof(InputEnumSelect<>).MakeGenericType(property.PropertyType), _form.SelectClass, new Dictionary<string, object>());
    }

    return (typeof(InputText), _form.EditorClass, new Dictionary<string, object>());
  }

  private static MethodInfo GetEventCallbackFactoryCreate()
  {
    return typeof(EventCallbackFactory).GetMethods()
        .Single(m =>
        {
          if (m.Name != "Create" || !m.IsPublic || m.IsStatic || !m.IsGenericMethod)
            return false;

          var generic = m.GetGenericArguments();
          if (generic.Length != 1)
            return false;

          var args = m.GetParameters();
          return args.Length == 2 && args[0].ParameterType == typeof(object) && args[1].ParameterType.IsGenericType && args[1].ParameterType.GetGenericTypeDefinition() == typeof(Action<>);
        });
  }

  private record SimpleCallback(Action Callback) : IHandleEvent
  {
    public static Action Create(Action callback) => new SimpleCallback(callback).Invoke;
    public static Func<Task> Create(Func<Task> callback) => new SimpleAsyncCallback(callback).Invoke;

    public void Invoke() => Callback();
    public Task HandleEventAsync(EventCallbackWorkItem item, object arg) => item.InvokeAsync(arg);
  }

  private record SimpleAsyncCallback(Func<Task> Callback) : IHandleEvent
  {
    public Task Invoke() => Callback();
    public Task HandleEventAsync(EventCallbackWorkItem item, object arg) => item.InvokeAsync(arg);
  }

}