namespace FireSharp.EventStreaming
{
    public delegate void ValueAddedEventHandler(object sender, ValueAddedEventArgs args);

    public delegate void ValueChangedEventHandler(object sender, ValueChangedEventArgs args);

    public delegate void ValueRemovedEventHandler(object sender, ValueRemovedEventArgs args);
}