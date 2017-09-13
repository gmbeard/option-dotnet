using System;

namespace Gmb
{
    public static class Ok
    {
        public static Result<T, E> Value<T, E>(T val)
            where E: Exception
        {
            return new Result<T, E>(val, default(E), true);
        }
    }

    public static class Err
    {
        public static Result<T, E> Value<T, E>(E err)
            where E: Exception
        {
            return new Result<T, E>(default(T), err, false);
        }
    }

    public static class Result
    {
        public static Result<T, E> For<T, E>(Func<T> f)
            where E: Exception
        {
            try {
                return Ok.Value<T, E>(f());
            }
            catch(E e) {
                return Err.Value<T, E>(e);
            }
        }
    }

    public struct Result<T, E> 
        where E: Exception
    {
        private bool _isOk;
        private T _val;
        private E _err;

        internal Result(T val, E err, bool isOk)
        {
            this._val = val;
            this._err = err;
            this._isOk = isOk;
        }

        public bool IsOk()
        {
            return this._isOk;
        }

        public bool IsErr()
        {
            return !IsOk();
        }

        public T Unwrap()
        {
            if (!IsOk()) {
                throw this._err;
            }

            return this._val;
        }

        public T UnwrapOrDefault(T def)
        {
            return MaybeOk().UnwrapOrDefault(def);
        }

        public T UnwrapOr(Func<T> f)
        {
            return MaybeOk().UnwrapOr(f);
        }

        public Option<T> MaybeOk()
        {
            if (!IsOk()) {
                return None.Value<T>();
            }

            return Some.Value(this._val);
        }

        public Option<E> MaybeErr()
        {
            if (IsOk()) {
                return None.Value<E>();
            }

            return Some.Value(this._err);
        }

        public Result<U, E> Map<U>(Func<T, U> f)
        {
            if (!IsOk()) {
                return Err.Value<U, E>(this._err);
            }

            return Ok.Value<U, E>(f(this._val));
        }

        public Result<T, U> MapErr<U>(Func<E, U> f)
            where U: Exception
        {
            if (IsOk()) {
                return Ok.Value<T, U>(this._val);
            }

            return Err.Value<T, U>(f(this._err));
        }
    }
}
