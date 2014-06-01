namespace TypeLess.Helpers
{
    public static class HashHelper
    {
        public static int GetHashCode<T1, T2>(T1 arg1, T2 arg2)
        {
            unchecked
            {
                return 367 * arg1.GetHashCode() + arg2.GetHashCode();
            }
        }

        public static int GetHashCode<T1, T2, T3>(T1 arg1, T2 arg2, T3 arg3)
        {
            unchecked
            {
                int hash = arg1.GetHashCode();
                hash = hash + 31 * arg2.GetHashCode();
                return hash + 37 * arg3.GetHashCode();
            }
        }

        public static int GetHashCode<T1, T2, T3, T4>(T1 arg1, T2 arg2, T3 arg3,
            T4 arg4)
        {
            unchecked
            {
                int hash = arg1.GetHashCode();
                hash = hash + 31 * arg2.GetHashCode();
                hash = hash + 37 * arg3.GetHashCode();
                return hash + 41 * arg4.GetHashCode();
            }
        }

        public static int GetHashCode<T1, T2, T3, T4, T5>(T1 arg1, T2 arg2, T3 arg3,
            T4 arg4, T5 arg5)
        {
            unchecked
            {
                int hash = arg1.GetHashCode();
                hash = hash + 31 * arg2.GetHashCode();
                hash = hash + 37 * arg3.GetHashCode();
                hash = hash + 41 * arg4.GetHashCode();
                return hash + 43 * arg5.GetHashCode();
            }
        }

        public static int GetHashCode<T1, T2, T3, T4, T5, T6>(T1 arg1, T2 arg2, T3 arg3,
            T4 arg4, T5 arg5, T6 arg6)
        {
            unchecked
            {
                int hash = arg1.GetHashCode();
                hash = hash + 31 * arg2.GetHashCode();
                hash = hash + 37 * arg3.GetHashCode();
                hash = hash + 41 * arg4.GetHashCode();
                hash = hash + 43 * arg5.GetHashCode();
                return hash + 47 * arg6.GetHashCode();
            }
        }

        public static int GetHashCode<T1, T2, T3, T4, T5, T6, T7>(T1 arg1, T2 arg2, T3 arg3,
            T4 arg4, T5 arg5, T6 arg6, T7 arg7)
        {
            unchecked
            {
                int hash = arg1.GetHashCode();
                hash = hash + 31 * arg2.GetHashCode();
                hash = hash + 37 * arg3.GetHashCode();
                hash = hash + 41 * arg4.GetHashCode();
                hash = hash + 43 * arg5.GetHashCode();
                hash = hash + 47 * arg6.GetHashCode();
                return hash + 53 * arg7.GetHashCode();
            }
        }

        public static int GetHashCode<T1, T2, T3, T4, T5, T6, T7, T8>(T1 arg1, T2 arg2, T3 arg3,
            T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8)
        {
            unchecked
            {
                int hash = arg1.GetHashCode();
                hash = hash + 31 * arg2.GetHashCode();
                hash = hash + 37 * arg3.GetHashCode();
                hash = hash + 41 * arg4.GetHashCode();
                hash = hash + 43 * arg5.GetHashCode();
                hash = hash + 47 * arg6.GetHashCode();
                hash = hash + 53 * arg7.GetHashCode();
                return hash + 59 * arg8.GetHashCode();
            }
        }

        public static int GetHashCode<T1, T2, T3, T4, T5, T6, T7, T8, T9>(T1 arg1, T2 arg2, T3 arg3,
             T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9)
        {
            unchecked
            {
                int hash = arg1.GetHashCode();
                hash = hash + 31 * arg2.GetHashCode();
                hash = hash + 37 * arg3.GetHashCode();
                hash = hash + 41 * arg4.GetHashCode();
                hash = hash + 43 * arg5.GetHashCode();
                hash = hash + 47 * arg6.GetHashCode();
                hash = hash + 53 * arg7.GetHashCode();
                hash = hash + 59 * arg8.GetHashCode();
                return hash + 61 * arg9.GetHashCode();
            }
        }

        public static int GetHashCode<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(T1 arg1, T2 arg2, T3 arg3,
            T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10)
        {
            unchecked
            {
                int hash = arg1.GetHashCode();
                hash = hash + 31 * arg2.GetHashCode();
                hash = hash + 37 * arg3.GetHashCode();
                hash = hash + 41 * arg4.GetHashCode();
                hash = hash + 43 * arg5.GetHashCode();
                hash = hash + 47 * arg6.GetHashCode();
                hash = hash + 53 * arg7.GetHashCode();
                hash = hash + 59 * arg8.GetHashCode();
                hash = hash + 61 * arg9.GetHashCode();
                return hash + 67 * arg10.GetHashCode();
            }
        }
    }
}
