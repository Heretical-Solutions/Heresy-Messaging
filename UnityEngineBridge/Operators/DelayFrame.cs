﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UniRx.Operators
{
    internal class DelayFrameObservable<T> : OperatorObservableBase<T>
    {
        readonly IObservable<T> source;
        readonly int frameCount;
        readonly FrameCountType frameCountType;

        public DelayFrameObservable(IObservable<T> source, int frameCount, FrameCountType frameCountType)
            : base(source.IsRequiredSubscribeOnCurrentThread())
        {
            this.source = source;
            this.frameCount = frameCount;
            this.frameCountType = frameCountType;
        }

        protected override IDisposable SubscribeCore(IObserver<T> observer, IDisposable cancel)
        {
            return new DelayFrame(this, observer, cancel).Run();
        }

        class DelayFrame : OperatorObserverBase<T, T>
        {
            readonly DelayFrameObservable<T> parent;
            readonly object gate = new object();
            int runningEnumeratorCount;
            bool readyDrainEnumerator;
            bool running;
            IDisposable sourceSubscription;
            Queue<T> currentQueueReference = new Queue<T>();
            bool calledCompleted;
            bool hasError;
            Exception error;
            BooleanDisposable cancelationToken;

            public DelayFrame(DelayFrameObservable<T> parent, IObserver<T> observer, IDisposable cancel) : base(observer, cancel)
            {
                this.parent = parent;
            }

            public IDisposable Run()
            {
                cancelationToken = new BooleanDisposable();

                var _sourceSubscription = new SingleAssignmentDisposable();
                sourceSubscription = _sourceSubscription;
                _sourceSubscription.Disposable = parent.source.Subscribe(this);

                return StableCompositeDisposable.Create(cancelationToken, sourceSubscription);
            }

            IEnumerator DrainQueue(Queue<T> q, int frameCount)
            {
                lock (gate)
                {
                    readyDrainEnumerator = false; // use next queue.
                    running = false;
                }

                while (!cancelationToken.IsDisposed && frameCount-- != 0)
                {
                    yield return null;
                }

                try
                {
                    if (q != null)
                    {
                        while (q.Count > 0 && !hasError)
                        {
                            if (cancelationToken.IsDisposed) break;

                            lock (gate)
                            {
                                running = true;
                            }

                            var value = q.Dequeue();
                            observer.OnNext(value);

                            lock (gate)
                            {
                                running = false;
                            }
                        }
                    }

                    if (hasError)
                    {
                        if (!cancelationToken.IsDisposed)
                        {
                            cancelationToken.Dispose();

                            try { observer.OnError(error); } finally { Dispose(); }
                        }
                    }
                    else if (calledCompleted)
                    {
                        lock (gate)
                        {
                            // not self only
                            if (runningEnumeratorCount != 1) yield break;
                        }

                        if (!cancelationToken.IsDisposed)
                        {
                            cancelationToken.Dispose();

                            try { observer.OnCompleted(); }
                            finally { Dispose(); }
                        }
                    }
                }
                finally
                {
                    lock (gate)
                    {
                        runningEnumeratorCount--;
                    }
                }
            }

            public override void OnNext(T value)
            {
                Queue<T> targetQueue = null;
                lock (gate)
                {
                    if (!readyDrainEnumerator)
                    {
                        readyDrainEnumerator = true;
                        runningEnumeratorCount++;
                        targetQueue = currentQueueReference = new Queue<T>(5); // Queue's default capacity is 32, it's too large for this usecase
                        targetQueue.Enqueue(value);
                    }
                    else
                    {
                        currentQueueReference.Enqueue(value);
                        return;
                    }
                }

                if (cancelationToken.IsDisposed) return;

                switch (parent.frameCountType)
                {
                    case FrameCountType.Update:
                        MainThreadDispatcher.StartUpdateMicroCoroutine(DrainQueue(targetQueue, parent.frameCount));
                        break;
                    case FrameCountType.FixedUpdate:
                        MainThreadDispatcher.StartFixedUpdateMicroCoroutine(DrainQueue(targetQueue, parent.frameCount));
                        break;
                    case FrameCountType.EndOfFrame:
                        MainThreadDispatcher.StartEndOfFrameMicroCoroutine(DrainQueue(targetQueue, parent.frameCount));
                        break;
                    default:
                        throw new ArgumentException("Invalid FrameCountType:" + parent.frameCountType);
                }
            }

            public override void OnError(Exception error)
            {
                sourceSubscription.Dispose(); // stop subscription

                if (cancelationToken.IsDisposed) return;

                lock (gate)
                {
                    if (running)
                    {
                        hasError = true;
                        this.error = error;
                        return;
                    }
                }

                cancelationToken.Dispose();
                try { base.observer.OnError(error); } finally { Dispose(); }
            }

            public override void OnCompleted()
            {
                sourceSubscription.Dispose(); // stop subscription

                if (cancelationToken.IsDisposed) return;

                lock (gate)
                {
                    calledCompleted = true;

                    if (!readyDrainEnumerator)
                    {
                        readyDrainEnumerator = true;
                        runningEnumeratorCount++;
                    }
                    else
                    {
                        return;
                    }
                }

                switch (parent.frameCountType)
                {
                    case FrameCountType.Update:
                        MainThreadDispatcher.StartUpdateMicroCoroutine(DrainQueue(null, parent.frameCount));
                        break;
                    case FrameCountType.FixedUpdate:
                        MainThreadDispatcher.StartFixedUpdateMicroCoroutine(DrainQueue(null, parent.frameCount));
                        break;
                    case FrameCountType.EndOfFrame:
                        MainThreadDispatcher.StartEndOfFrameMicroCoroutine(DrainQueue(null, parent.frameCount));
                        break;
                    default:
                        throw new ArgumentException("Invalid FrameCountType:" + parent.frameCountType);
                }
            }
        }
    }
}