using System;
using System.Collections.Generic;

namespace EnergySystem.Core
{
    public interface IReadOnlyReactiveValue<T>
    {
        T Value { get; }
        IDisposable Subscribe(Action<T> callback, bool invokeImmediately = true);
    }

    public class ReactiveValue<T> : IReadOnlyReactiveValue<T>, IDisposable
    {
        private T _value;
        private readonly List<Action<T>> _callbacks = new List<Action<T>>();
        private bool _disposed;

        public T Value
        {
            get => _value;
            set
            {
                if (EqualityComparer<T>.Default.Equals(_value, value))
                    return;

                _value = value;
                NotifyCallbacks();
            }
        }

        public ReactiveValue(T initialValue = default)
        {
            _value = initialValue;
        }

        public IDisposable Subscribe(Action<T> callback, bool invokeImmediately = true)
        {
            if (callback == null) throw new ArgumentNullException(nameof(callback));
            if (_disposed) throw new ObjectDisposedException(nameof(ReactiveValue<T>));

            _callbacks.Add(callback);

            if (invokeImmediately)
                callback(_value);

            return new Subscription(this, callback);
        }

        private void NotifyCallbacks()
        {
            var callbacks = _callbacks.ToArray();
            foreach (var callback in callbacks)
            {
                callback(_value);
            }
        }

        private void Unsubscribe(Action<T> callback)
        {
            _callbacks.Remove(callback);
        }

        public void Dispose()
        {
            _disposed = true;
            _callbacks.Clear();
        }

        private class Subscription : IDisposable
        {
            private readonly ReactiveValue<T> _owner;
            private readonly Action<T> _callback;
            private bool _disposed;

            public Subscription(ReactiveValue<T> owner, Action<T> callback)
            {
                _owner = owner;
                _callback = callback;
            }

            public void Dispose()
            {
                if (_disposed) return;
                _disposed = true;
                _owner.Unsubscribe(_callback);
            }
        }
    }
}
