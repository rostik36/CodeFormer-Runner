﻿using System.Threading;
using System.Threading.Tasks;

namespace unblur_upscale.Commands
{
    public static class CommonHelper
    {
        struct Unit { }

        public static Task AsTask(this CancellationToken @this)
        {
            var tcs = new TaskCompletionSource<Unit>();

            @this.Register(() => tcs.SetResult(default(Unit)));

            return tcs.Task;
        }
    }
}
