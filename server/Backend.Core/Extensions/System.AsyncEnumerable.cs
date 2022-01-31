using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace System
{
    public static partial class System
    { 
        public static IAsyncEnumerable<T> ToAsync<T>(this IEnumerable<T> @this) => new EnumerableAdapter<T>(@this);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IAsyncEnumerator<T> ToAsync<T>(this IEnumerator<T> @this) => new EnumeratorAdapter<T>(@this);

        private sealed class EnumerableAdapter<T> : IAsyncEnumerable<T>
        {
            private readonly IEnumerable<T> target;
            public EnumerableAdapter(IEnumerable<T> target) => this.target = target;
            public IAsyncEnumerator<T> GetAsyncEnumerator()
            {
                return this.target.GetEnumerator().ToAsync();
            }
            public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = default)
            {
                return this.target.GetEnumerator().ToAsync();
            }
        }

        private sealed class EnumeratorAdapter<T> : IAsyncEnumerator<T>
        {
            private readonly IEnumerator<T> enumerator;
            public EnumeratorAdapter(IEnumerator<T> enumerator) => this.enumerator = enumerator;

            public ValueTask<bool> MoveNextAsync() => new ValueTask<bool>(this.enumerator.MoveNext());
            public T Current => this.enumerator.Current;
            public ValueTask DisposeAsync()
            {
                this.enumerator.Dispose();
                return new ValueTask();
            }
        }
    }

}
