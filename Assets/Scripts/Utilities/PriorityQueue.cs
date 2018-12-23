using System;
using System.Collections.Generic;

namespace Ruffz.Utilities {

    public class PriorityQueue<T> where T : IComparable<T> {

        private List<T> data;

        public PriorityQueue () {
            this.data = new List<T> ();
        }

        public void Enqueue (T item) {
            data.Add (item);
            int ci = data.Count - 1; // child index; start at end
            while (ci > 0) {
                int pi = (ci - 1) / 2; // parent index
                if (data[ci].CompareTo (data[pi]) >= 0)
                    break; // child item is larger than (or equal) parent so we're done
                T tmp = data[ci];
                data[ci] = data[pi];
                data[pi] = tmp;
                ci = pi;
            }
        }

        public T Dequeue () {
            // assumes pq is not empty; up to calling code
            int li = data.Count - 1; // last index (before removal)
            T frontItem = data[0];   // fetch the front
            data[0] = data[li];
            data.RemoveAt (li);

            --li; // last index (after removal)
            int pi = 0; // parent index. start at front of pq
            while (true) {
                int ci = pi * 2 + 1; // left child index of parent
                if (ci > li)
                    break;  // no children so done
                int rc = ci + 1;     // right child
                if (rc <= li && data[rc].CompareTo (data[ci]) < 0) // if there is a rc (ci + 1), and it is smaller than left child, use the rc instead
                    ci = rc;
                if (data[pi].CompareTo (data[ci]) <= 0)
                    break; // parent is smaller than (or equal to) smallest child so done
                T tmp = data[pi];
                data[pi] = data[ci];
                data[ci] = tmp; // swap parent and child
                pi = ci;
            }
            return frontItem;
        }

        public T Peek () {
            T frontItem = data[0];
            return frontItem;
        }

        public void RemoveAt (int index) {
            if (index < 0 || index >= data.Count) return;
            if (index == 0) { // Removing root, just use Dequeue()
                Dequeue ();
                return;
            }

            int li = data.Count - 1; // last index (before removal)
            data[index] = data[li];  // remove at index by overwriting it with the last element
            data.RemoveAt (li);
            --li; // last index (after removal)

            if (li < 0) return; // empty, we're done here
            if (li <= index && index != 0) return; // Removed last or 2nd last child, no need to heapify unless index is root

            int ki = index; // k-th index
            int pi = (ki - 1) / 2; // parent index
            int comp = data[ki].CompareTo (data[pi]); // compare index value with its parent

            if (comp < 0) { // heapify upwards

                // loop until there is no more parent on top
                while (pi > 0) { 
                    pi = (ki - 1) / 2;
                    if (data[ki].CompareTo (data[pi]) < 0) {
                        T tmp = data[pi];
                        data[pi] = data[ki];
                        data[ki] = tmp; // swap parent and k
                        ki = pi;
                    } else {
                        break; // k is larger than (or equal to) parent so done
                    }
                }
            } else if (comp > 0) { // heapify downwards

                // loop while k has at least 1 child
                while (2 * ki + 1 < data.Count) { 
                    int lc = 2 * ki + 1; // left child
                    int rc = 2 * ki + 2; // right child

                    if (rc < data.Count) { // k has 2 children
                        if (data[ki].CompareTo (data[lc]) < 0 && data[ki].CompareTo (data[rc]) > 0) {
                            break; // k is smaller than both children so done
                        } else {
                            if (data[lc].CompareTo (data[rc]) < 0) {
                                // left child is smaller than right, swap k with left
                                T tmp = data[lc];
                                data[lc] = data[ki];
                                data[ki] = tmp; // swap child and k
                                ki = lc;
                            } else {
                                // right child is smaller than (or equal to) left, swap k with right
                                T tmp = data[rc];
                                data[rc] = data[ki];
                                data[ki] = tmp; // swap child and k
                                ki = rc;
                            }
                        }
                    } else { // k has 1 child
                        if (data[ki].CompareTo (data[lc]) > 0) {
                            T tmp = data[lc];
                            data[lc] = data[ki];
                            data[ki] = tmp; // swap child and k
                            ki = lc;
                        } else {
                            break; // k is smaller than (or equal to) child so done
                        }
                    }
                }
            }
        }

        public int ContainsAt (T item) {
            for (int i = 0; i < data.Count; i++) {
                if (data[i].Equals (item)) {
                    return i;
                }
            }
            return -1;
        }

        public T this[int i] {
            get { return data[i]; }
            set { data[i] = value; }
        }

        public int Count {
            get {
                return data.Count;
            }
        }

        public void Clear () {
            data.Clear ();
        }

        public override string ToString () {
            string s = "";
            for (int i = 0; i < data.Count; ++i)
                s += data[i].ToString () + " ";
            s += "count = " + data.Count;
            return s;
        }

        public bool IsConsistent () {
            // is the heap property true for all data?
            if (data.Count == 0)
                return true;
            int li = data.Count - 1; // last index
            for (int pi = 0; pi < data.Count; ++pi) { // each parent index
                int lci = 2 * pi + 1; // left child index
                int rci = 2 * pi + 2; // right child index

                if (lci <= li && data[pi].CompareTo (data[lci]) > 0)
                    return false; // if lc exists and it's greater than parent then bad.
                if (rci <= li && data[pi].CompareTo (data[rci]) > 0)
                    return false; // check the right child too.
            }
            return true; // passed all checks
        }
    }
}
