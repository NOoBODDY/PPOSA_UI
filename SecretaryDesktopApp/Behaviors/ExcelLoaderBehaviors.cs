using System;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Input;
using Avalonia.Interactivity;

namespace SecretaryDesktopApp.Behaviors;

public class ExcelLoaderBehaviors
{
     #region TheEvent Attached Avalonia Property
        public static RoutedEvent GetTheEvent(IControl obj)
        {
            return obj.GetValue(TheEventProperty);
        }

        public static void SetTheEvent(IControl obj, RoutedEvent value)
        {
            obj.SetValue(TheEventProperty, value);
        }

        public static readonly AttachedProperty<RoutedEvent> TheEventProperty =
            AvaloniaProperty.RegisterAttached<ExcelLoaderBehaviors, IControl, RoutedEvent>
            (
                "TheEvent"
            );
        #endregion TheEvent Attached Avalonia Property


        #region TargetObject Attached Avalonia Property
        public static object GetTargetObject(IControl obj)
        {
            return obj.GetValue(TargetObjectProperty);
        }

        public static void SetTargetObject(IControl obj, object value)
        {
            obj.SetValue(TargetObjectProperty, value);
        }

        public static readonly AttachedProperty<object> TargetObjectProperty =
            AvaloniaProperty.RegisterAttached<ExcelLoaderBehaviors, IControl, object>
            (
                "TargetObject"
            );
        #endregion TargetObject Attached Avalonia Property


        #region MethodToCall Attached Avalonia Property
        public static string? GetMethodToCall(IControl obj)
        {
            return obj.GetValue(MethodToCallProperty);
        }

        public static void SetMethodToCall(IControl obj, string value)
        {
            obj.SetValue(MethodToCallProperty, value);
        }

        public static readonly AttachedProperty<string> MethodToCallProperty =
            AvaloniaProperty.RegisterAttached<ExcelLoaderBehaviors, IControl, string>
            (
                "MethodToCall"
            );
        #endregion MethodToCall Attached Avalonia Property
        
        
        #region AsyncMethodToCall Attached Avalonia Property
        public static string? GetAsyncMethodToCall(IControl obj)
        {
            return obj.GetValue(AsyncMethodToCallProperty);
        }

        public static void SetAsyncMethodToCall(IControl obj, string value)
        {
            obj.SetValue(AsyncMethodToCallProperty, value);
        }

        public static readonly AttachedProperty<string> AsyncMethodToCallProperty =
            AvaloniaProperty.RegisterAttached<ExcelLoaderBehaviors, IControl, string>
            (
                "AsyncMethodToCall"
            );
        #endregion MethodToCall Attached Avalonia Property

        #region Command Property
        
        /// <summary>
        /// Identifies the <seealso cref="CommandProperty"/> avalonia attached property.
        /// </summary>
        /// <value>Provide an <see cref="ICommand"/> derived object or binding.</value>
        public static readonly AttachedProperty<ICommand> CommandProperty = AvaloniaProperty.RegisterAttached<ExcelLoaderBehaviors, Interactive, ICommand>(
            "Command", default(ICommand), false, BindingMode.OneTime);
        
        
        /// <summary>
        /// Accessor for Attached property <see cref="CommandProperty"/>.
        /// </summary>
        public static void SetCommand(IControl element, ICommand commandValue)
        {
            element.SetValue(CommandProperty, commandValue);
        }

        /// <summary>
        /// Accessor for Attached property <see cref="CommandProperty"/>.
        /// </summary>
        public static ICommand GetCommand(IControl element)
        {
            return element.GetValue(CommandProperty);
        }
    
        
        
        #endregion
        
        
        static ExcelLoaderBehaviors()
        {
            TheEventProperty.Changed.Subscribe(OnEventChanged);
        }

        private static void OnEventChanged(AvaloniaPropertyChangedEventArgs<RoutedEvent> args)
        {
            IControl el = (IControl) args.Sender;

            RoutedEvent? oldRoutedEvent = args.OldValue.Value as RoutedEvent;

            if (oldRoutedEvent != null)
            {
                // remove old event handler from the object (if exists)
                el.RemoveHandler(oldRoutedEvent, (EventHandler<RoutedEventArgs>)HandleRoutedEvent!);
            }


            RoutedEvent newRoutedEvent = args.NewValue.Value as RoutedEvent;

            if (newRoutedEvent != null)
            {
                // add new event handler to the object
                el.AddHandler(newRoutedEvent, (EventHandler<RoutedEventArgs>)HandleRoutedEvent!);
            }
        }

        // handle the routed event when happens on the object
        // by calling the method of name 'methodName' onf the
        // TargetObject
        private static async void HandleRoutedEvent(object sender, RoutedEventArgs e)
        {
            IControl el = (IControl)sender;

            // if TargetObject is not set, use DataContext as the target object
            object? targetObject = GetTargetObject(el) ?? el.DataContext;

            var isAsync = true;

            string? methodName = GetAsyncMethodToCall(el);
            if (methodName == null)
            {
                isAsync = false;
                methodName = GetMethodToCall(el);
            }

            // do not do anything
            if (targetObject == null || methodName == null)
            {
                return;
            }

            MethodInfo? methodInfo =
                targetObject.GetType().GetMethod(methodName);

            if (methodInfo == null)
            {
                return;
            }

            // call the method using reflection
            object? commandParameter = null;
            if (isAsync)
            {
                commandParameter = await (Task<object>)methodInfo.Invoke(targetObject, null);
            }
            else
            {
                commandParameter = methodInfo.Invoke(targetObject, null);
            }

            var command = GetCommand(el);
            if (commandParameter != null && command != null)
            {
                command.Execute(commandParameter);
            }
        }
}