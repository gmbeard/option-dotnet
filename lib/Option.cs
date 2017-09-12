using System;
using System.Linq;
using System.Collections.Generic;

namespace Gmb
{
	public class OptionIsEmptyException : Exception
	{ }

	public static class Some
	{
		public static Option<T> Value<T>(T val)
		{
			return new Option<T>(val, true);
		}
	}

	public static class None
	{
		public static Option<T> Value<T>()
		{
			return new Option<T>(default(T), false);
		}
	}

	public struct Option<T>
	{
		private T _value;
		private bool _hasValue;

		internal Option(T val, bool isSet)
		{
			this._value = val;
			this._hasValue = isSet;
		}

		public bool IsNone()
		{
			return !this._hasValue;
		}

		public bool IsSome()
		{
			return this._hasValue;
		}

		public T Unwrap()
		{
			if(this._hasValue) {
				return this._value;
			}

			throw new OptionIsEmptyException();
		}

		public T UnwrapOrDefault(T def)
		{
			return UnwrapOr(() => def);
		}

		public T UnwrapOr(Func<T> f)
		{
			if(this._hasValue) {
				return this._value;
			}
			else {
				return f();
			}
		}

		public Option<U> Map<U>(Func<T, U> f)
		{
			if(this._hasValue) {
				return Some.Value(f(this._value));
			}
			else {
				return None.Value<U>();
			}
		}

		public IEnumerable<T> ToEnumerable()
		{
			if (this._hasValue) {
				return Enumerable.Repeat(this._value, 1);
			}
			
			return Enumerable.Empty<T>();
		}

		public Option<T> AndThen(Func<T, T> f)
		{
			if (IsSome()) {
				return Some.Value(f(this._value));
			}

			return this;
		}
	}
}
