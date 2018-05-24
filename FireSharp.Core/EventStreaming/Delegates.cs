namespace FireSharp.Core.EventStreaming
{
    public delegate void ValueAddedEventHandler(object sender, ValueAddedEventArgs args, object context);

    public delegate void ValueRootAddedEventHandler<T>(object sender, T arg);

    public delegate void ValueChangedEventHandler(object sender, ValueChangedEventArgs args, object context);

    public delegate void ValueRemovedEventHandler(object sender, ValueRemovedEventArgs args, object context);
}