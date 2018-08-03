﻿namespace Net4Log.WpfExtennsions
{
    using System.Windows;
    using System.Windows.Input;
    using System.Windows.Interactivity;

    public sealed class EventCommand : TriggerAction<DependencyObject>
    {
        public static readonly DependencyProperty CommandParameterProperty =
            DependencyProperty.Register("CommandParameter", typeof(object), typeof(EventCommand), null);

        public static readonly DependencyProperty CommandProperty = DependencyProperty.Register(
            "Command", typeof(ICommand), typeof(EventCommand), null);

        public static readonly DependencyProperty InvokeParameterProperty = DependencyProperty.Register(
            "InvokeParameter", typeof(object), typeof(EventCommand), null);

        private string commandName;

        public object InvokeParameter
        {
            get => this.GetValue(InvokeParameterProperty);
            set => this.SetValue(InvokeParameterProperty, value);
        }

        public ICommand Command
        {
            get => (ICommand)this.GetValue(CommandProperty);
            set => this.SetValue(CommandProperty, value);
        }

        public string CommandName
        {
            get
            {
                return this.commandName;
            }

            set
            {
                if (this.CommandName != value)
                {
                    this.commandName = value;
                }
            }
        }

        public object CommandParameter
        {
            get => this.GetValue(CommandParameterProperty);

            set => this.SetValue(CommandParameterProperty, value);
        }

        public object Sender { get; set; }

        protected override void Invoke(object parameter)
        {
            this.InvokeParameter = parameter;
            if (this.AssociatedObject != null)
            {
                var command = this.Command;
                if ((command != null) && command.CanExecute(this.CommandParameter))
                {
                    command.Execute(this.CommandParameter);
                }
            }
        }
    }
}