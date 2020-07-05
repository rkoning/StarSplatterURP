using System;
using System.Collections.Generic;
using UnityEngine;

public class PriorityQueue<T> {

	private List<QueueItem<T>> contents;

	public PriorityQueue() {
		this.contents = new List<QueueItem<T>>();
	}

	public bool Empty() {
		return contents.Count < 1;
	}

	public void Enqueue(T item, float priority) {
		contents.Add(new QueueItem<T>(item, priority));
		int ci = contents.Count - 1;
		while (ci > 0) {
			int pi = (ci - 1) / 2;
			if (contents[ci].CompareTo(contents[pi]) >= 0) {
				break;
			}
			var tmp = contents[ci];
			contents[ci] = contents[pi];
			contents[pi] = tmp;
			ci = pi;
		}
	}

	public T Dequeue() {
		int end = contents.Count - 1;
		QueueItem<T> front = contents[0];
		contents[0] = contents[end];
		contents.RemoveAt(end);
		end--;

		int pi = 0;
		while(true) {
			int ci = pi * 2 + 1;
			if (ci > end)
				break;
			int rc = ci + 1;
			if (rc <= end && contents[rc].CompareTo(contents[ci]) < 0)
				ci = rc;
			if (contents[pi].CompareTo(contents[ci]) <= 0)
				break;
			var tmp = contents[pi];
			contents[pi] = contents[ci];
			contents[ci] = tmp;
			pi = ci;
		}
		return front.item;
	}

	public T Peek() {
		if (Empty()) {
			return default(T);
		}
		return contents[0].item;
	}

	public void PrintContents() {
		string o = "";
		for (int i = 0; i < contents.Count; i++) {
			o += " " + i + ": " + contents[i].priority;
		}
		Debug.Log(o);
	}

	private class QueueItem<K> : IComparable {
		public K item;
		public float priority;

		public QueueItem(K item, float priority) {
			this.item = item;
			this.priority = priority;
		}

		public int CompareTo(object other) {
			QueueItem<K> otherItem = other as QueueItem<K>;
			if (otherItem != null)
				return priority.CompareTo(otherItem.priority);
			else
				throw new ArgumentException("Object is not a QueueItem!");
		}
	}
}
