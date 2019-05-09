using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace arbolesb
{
    public class Node<E>
    {
        private List<E> keys;
        private List<Object> pointers;
        private Node<E> next;
        private Node<E> prev;
        bool isleaf;
        int nodeSize;
        public Node(int nodeSize, Boolean isLeaf)
        {
            keys = new List<E>();
            pointers = new List<Object>();
            this.nodeSize = nodeSize;
            this.isleaf = isLeaf;
        }
        public List<E> getKeys()
        {
            return keys;
        }
        public void setKeys(List<E> keys)
        {
            this.keys = keys;
        }
        public List<Object> getPointers()
        {
            return pointers;
        }
        public void setPointers(List<Object> pointers)
        {
            this.pointers = pointers;
        }
        public Node<E> getNext()
        {
            return next;
        }
        public void setNext(Node<E> next)
        {
            this.next = next;
        }
        public Node<E> getPrev()
        {
            return prev;
        }
        public void setPrev(Node<E> prev)
        {
            this.prev = prev;
        }
        public bool isLeaf()
        {
            return isleaf;
        }
        public void setLeaf(bool isLeaf)
        {
            this.isleaf = isLeaf;
        }
        public int getNodeSize()
        {
            return nodeSize;
        }
        public void setNodeSize(int nodeSize)
        {
            this.nodeSize = nodeSize;
        }
    }
    public class NativeMethodss<E> where E : IComparable
    {
        public void sortedInsert(E key, Object record, Node<E> N)
        {
            List<E> data = N.getKeys();
            List<Object> pointers = N.getPointers();
            if (data.Count == 0)
            {
                data.Add(key);
                pointers.Add(record);
                return;
            }
            else
            {
                if (key.CompareTo(data[0]) < 0)
                {
                    data.Insert(0, key);
                    pointers.Insert(0, record);
                }
                else
                {
                    Boolean flag = true;
                    for (int i = 0; i < data.Count; i++)
                    {
                        if (key.CompareTo(data[i]) < 0)
                        {
                            flag = false;
                            data.Insert(i, key);
                            pointers.Insert(i, record);
                            break;
                        }
                    }
                    if (flag)
                    {
                        data.Add(key);
                        pointers.Add(record);
                    }

                }

            }

        }
        public void sortedInsertInternal(E key, Object record, Node<E> N)
        {
            List<E> data = N.getKeys();
            List<Object> pointers = N.getPointers();
            if (key.CompareTo(data[0]) < 0)
            {
                data.Insert(0, key);
                pointers.Insert(1, record);
            }
            else
            {
                Boolean flag = true;
                for (int i = 0; i < data.Count; i++)
                {
                    if (key.CompareTo(data[i]) < 0)
                    {
                        data.Insert(i, key);
                        pointers.Insert(i + 1, record);
                        flag = false;
                        break;
                    }
                }
                if (flag)
                {
                    data.Add(key);
                    pointers.Add(record);
                }
            }
        }

        public void deleteNode(Node<E> n, E k)
        {
            for (int i = 0; i < n.getKeys().Count; i++)
            {
                if (k.CompareTo(n.getKeys()[i]) == 0)
                {
                    n.getKeys().RemoveAt(i);
                    n.getPointers().RemoveAt(i);
                }
            }
        }

        public void internalDelet(E key, Node<E> n, Node<E> temp)
        {
            for (int i = 0; i < n.getKeys().Count; i++)
            {
                if (n.getKeys()[i].CompareTo(key) == 0)
                {
                    n.getKeys().RemoveAt(i);
                    n.getPointers().RemoveAt(i + 1);
                }
            }
        }
        public int sameParent(Node<E> n, Node<E> parent, int size)
        {
            List<E> keys = parent.getKeys();
            Boolean _next = false;
            Boolean _prev = false;
            Node<E> next = n.getNext();
            Node<E> prev = n.getPrev();
            if (sameParent2(parent, n))
            {
                for (int i = 0; i < parent.getPointers().Count; i++)
                {
                    if (next == parent.getPointers()[i])
                    {
                        _next = true;
                        break;
                    }
                }
            }

            if (!sameParent2(parent, n))
            {
                for (int i = 0; i < parent.getPointers().Count; i++)
                {
                    if (prev == parent.getPointers()[i])
                    {
                        _prev = true;
                        break;
                    }
                }
            }
            if (_next && next.getKeys().Count - 1 >= Math.Ceiling(size / 2.0))
            {
                return 1;
            }
            else if (_prev && next.getKeys().Count - 1 >= Math.Ceiling(size / 2.0))
            {
                return 2;
            }
            else
            {
                return 0;
            }
        }
        public Boolean sameParent2(Node<E> parent, Node<E> n)
        {
            Boolean _next = false;
            Boolean _prev = false;
            Node<E> next = n.getNext();
            Node<E> prev = n.getPrev();
            if (next != null)
            {
                for (int i = 0; i < parent.getPointers().Count; i++)
                {
                    if (next == parent.getPointers()[i])
                    {
                        _next = true;
                        break;
                    }
                }
            }
            if (prev != null)
            {
                for (int i = 0; i < parent.getPointers().Count; i++)
                {
                    if (prev == parent.getPointers()[i])
                    {
                        _prev = true;
                        break;
                    }
                }
            }
            if (_next)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public int nexOrprev(Node<E> n, Node<E> parent, int size)
        {
            Boolean _next = false;
            Boolean _prev = false;
            Node<E> next = n.getNext();
            Node<E> prev = n.getPrev();
            if (next != null)
            {
                for (int i = 0; i < parent.getPointers().Count; i++)
                {
                    if (next == parent.getPointers()[i])
                    {
                        _next = true;
                        break;
                    }
                }
            }
            if (prev != null)
            {
                for (int i = 0; i < parent.getPointers().Count; i++)
                {
                    if (prev == parent.getPointers()[i])
                    {
                        _prev = true;
                        break;
                    }
                }
            }
            if (next != null && _next && next.getKeys().Count - 1 >= 1)
            {
                return 1;
            }
            else if (prev != null && _prev && prev.getKeys().Count - 1 >= 1)
            {
                return 2;
            }
            else
            {
                return 0;
            }
        }
    }
    public class BPTree<E> where E : IComparable
    {
        private int leafSize;

        private int internalSize;

        private Node<E> root;

        private int size;

        private NativeMethodss<E> nm;

        private List<Node<E>> buffer;

        private int bufferboolSize;



        public BPTree(int leafSize, int internalSize, int bufferbool)

        {

            this.leafSize = leafSize;

            this.internalSize = internalSize;

            this.root = new Node<E>(leafSize, true);

            nm = new NativeMethodss<E>();

            buffer = new List<Node<E>>();

            this.bufferboolSize = bufferbool;

        }

        public void insetNode(E key, Object data)

        {

            // stack to hold parent

            LinkedList<Node<E>> stack = new LinkedList<Node<E>>();

            Node<E> n = root;

            //sezrching fo the element

            while (!n.isLeaf())

            {

                stack.AddFirst(n);

                // ===================================================

                if (key.CompareTo(n.getKeys()[(0)]) < 0)

                {  // if in first pointer

                    n = (Node<E>)n.getPointers()[(0)];

                }

                else if (key.CompareTo(n.getKeys()[(n.getKeys().Count - 1)]) >= 0)

                {// if in last pointer

                    n = (Node<E>)n.getPointers()[(n.getPointers().Count - 1)];

                }

                else

                {

                    for (int i = 0; i < n.getKeys().Count - 1; i++)

                    { // general case

                        if (n.getKeys().Count > 1 && key.CompareTo(n.getKeys()[(i)]) >= 0 && key.CompareTo(n.getKeys()[(i + 1)]) < 0)

                        {

                            n = (Node<E>)n.getPointers()[(i + 1)];

                            break;

                        }

                    }

                }

            }

            // check if the elemnet in the node or not

            for (int i = 0; i < n.getKeys().Count; i++)

            {

                if (key.Equals(n.getKeys()[i]))

                {

                    return;

                }

            }

            // if node is not full

            if (n.getKeys().Count < leafSize)

            {

                nm.sortedInsert(key, data, n);

            }

            else

            {

                ///    spliting two leaf nodes

                // copying all current node contents in temp node then insert the new element on it

                Node<E> temp = new Node<E>(leafSize, true);

                temp.setKeys(new List<E>(n.getKeys()));

                temp.setPointers(new List<Object>(n.getPointers()));

                nm.sortedInsert(key, data, temp);

                Node<E> newNode = new Node<E>(leafSize, true);
                int j = 0;
                if (n.getPointers().Count % 2 == 0)
                {
                    j = (int)Math.Ceiling((n.getPointers().Count / (double)2));
                }
                else
                {
                    j = (int)Math.Ceiling((n.getPointers().Count / (double)2)-1);
                }

                //take the first half of the temp nde in current node

                n.setKeys(new List<E>(temp.getKeys().GetRange(0, j)));

                n.setPointers(new List<Object>(temp.getPointers().GetRange(0, j)));

                // next and prev

                if (n.getNext() != null)

                {

                    n.getNext().setPrev(newNode);

                }

                newNode.setNext(n.getNext());

                n.setNext(newNode);

                // copying the rest of temp node in new node

                newNode.setPrev(n);

                newNode.setKeys(new List<E>(temp.getKeys().GetRange(j, temp.getKeys().Count - j)));

                newNode.setPointers(new List<Object>(temp.getPointers().GetRange(j, temp.getPointers().Count - j)));

                // keeping the key that will be inserting in parent node

                key = temp.getKeys()[j];

                bool finished = false;

                do

                {

                    // if the parent is null (root case)

                    if (stack.Count == 0)

                    {

                        root = new Node<E>(internalSize, false);

                        List<Object> point = new List<Object>();

                        point.Add(n);

                        point.Add(newNode);

                        List<E> keys_ = new List<E>();

                        keys_.Add(key);

                        root.setKeys(keys_);

                        root.setPointers(point);

                        finished = true;

                    }

                    else

                    {

                        // if there's parent

                        n = stack.First();

                        stack.RemoveFirst();

                        // if there's no need for splitting internal

                        if (n.getKeys().Count < internalSize)

                        {

                            nm.sortedInsertInternal(key, newNode, n);

                            finished = true;

                        }

                        else

                        {

                            /* splitting two internal nodes by copying them into new node and insert

                            new elemnet in the temp node then divide it betwwen current node and new node

                             */

                            temp.setLeaf(false);

                            temp.setKeys(new List<E>(n.getKeys()));

                            temp.setPointers(new List<Object>(n.getPointers()));



                            nm.sortedInsertInternal(key, newNode, temp);

                            newNode = new Node<E>(internalSize, false);

                            j = (int)Math.Ceiling(temp.getPointers().Count / (double)2);



                            n.setKeys(new List<E>(temp.getKeys().GetRange(0, j - 1)));

                            n.setPointers(new List<Object>(temp.getPointers().GetRange(0, j)));

                            if (n.getNext() != null)

                            {

                                n.getNext().setPrev(newNode);

                            }

                            newNode.setNext(n.getNext());

                            n.setNext(newNode);

                            newNode.setPrev(n);

                            newNode.setKeys(new List<E>(temp.getKeys().GetRange(j, temp.getKeys().Count)));

                            newNode.setPointers(new List<Object>(temp.getPointers().GetRange(j, temp.getPointers().Count)));



                            key = temp.getKeys()[j - 1];

                        }

                    }

                } while (!finished);

            }

        }

        public void insertBulk(List<E> keys, List<Object> records)

        {

            E key;

            bool firstInsert = true;

            int first = 0;

            int second = 0;

            for (int i = 0; i < Math.Ceiling(keys.Count / (double)leafSize); i++)

            {

                // stack to hold parent

                LinkedList<Node<E>> stack = new LinkedList<Node<E>>();

                Node<E> n = root;

                first = second;

                second = second + leafSize;

                if (second > keys.Count)

                {

                    second = keys.Count;

                }

                List<E> newKeys = new List<E>(keys.GetRange(first, second));

                List<Object> newRecords = new List<Object>(records.GetRange(first, second));

                // getting the right most elemnet in the tree

                while (!n.isLeaf())

                {

                    stack.AddFirst(n);

                    n = (Node<E>)n.getPointers()[n.getPointers().Count - 1];

                }

                // if its the first insert

                if (firstInsert)

                {

                    root.setKeys(newKeys);

                    root.setPointers(newRecords);

                    firstInsert = false;

                }

                else

                {

                    //    spliting two leaf nodes

                    // copying all current node contents in temp node then insert the new element on it

                    Node<E> temp = new Node<E>(leafSize, true);

                    temp.setKeys(new List<E>(n.getKeys()));

                    temp.setPointers(new List<Object>(n.getPointers()));

                    temp.getKeys().AddRange(newKeys);

                    temp.getPointers().AddRange(newRecords);

                    Node<E> newNode = new Node<E>(leafSize, true);

                    int j = (int)Math.Ceiling(temp.getPointers().Count / (double)2);

                    //take the first half of the temp nde in current node

                    n.setKeys(new List<E>(temp.getKeys().GetRange(0, j)));

                    n.setPointers(new List<Object>(temp.getPointers().GetRange(0, j)));

                    if (n.getNext() != null)

                    {

                        n.getNext().setPrev(newNode);

                    }

                    newNode.setNext(n.getNext());

                    n.setNext(newNode);

                    // copying other elements

                    newNode.setPrev(n);

                    newNode.setKeys(new List<E>(temp.getKeys().GetRange(j, temp.getKeys().Count)));

                    newNode.setPointers(new List<Object>(temp.getPointers().GetRange(j, temp.getPointers().Count)));

                    key = temp.getKeys()[j];

                    bool finished = false;

                    // keeping the key that will be inserting in parent node

                    do

                    {

                        // if the parent is null (root case)

                        if (stack.Count == 0)

                        {

                            root = new Node<E>(internalSize, false);

                            List<Object> point = new List<Object>();

                            point.Add(n);

                            point.Add(newNode);

                            List<E> keys_ = new List<E>();

                            keys_.Add(key);

                            root.setKeys(keys_);

                            root.setPointers(point);

                            finished = true;

                        }

                        else

                        {

                            // if there's parent

                            n = stack.First();

                            stack.RemoveFirst();

                            // if there's no need for splitting internal

                            if (n.getKeys().Count < internalSize)

                            {

                                nm.sortedInsertInternal(key, newNode, n);

                                finished = true;

                            }

                            else

                            {

                                /* splitting two internal nodes by copying them into new node and insert

                                new elemnet in the temp node then divide it betwwen current node and new node

                                 */

                                temp.setLeaf(false);

                                temp.setKeys(new List<E>(n.getKeys()));

                                temp.setPointers(new List<Object>(n.getPointers()));

                                nm.sortedInsertInternal(key, newNode, temp);

                                newNode = new Node<E>(internalSize, false);

                                j = (int)Math.Ceiling(temp.getPointers().Count / (double)2);

                                n.setKeys(new List<E>(temp.getKeys().GetRange(0, j - 1)));

                                n.setPointers(new List<Object>(temp.getPointers().GetRange(0, j)));

                                if (n.getNext() != null)

                                {

                                    n.getNext().setPrev(newNode);

                                }

                                newNode.setNext(n.getNext());

                                n.setNext(newNode);

                                newNode.setPrev(n);

                                newNode.setKeys(new List<E>(temp.getKeys().GetRange(j, temp.getKeys().Count)));

                                newNode.setPointers(new List<Object>(temp.getPointers().GetRange(j, temp.getPointers().Count)));

                                key = temp.getKeys()[j - 1];

                            }

                        }

                    } while (!finished);

                }

            }

        }

        public Node<E> search(E key)

        {

            for (int i = 0; i < buffer.Count; i++)

            {

                List<E> find = buffer[i].getKeys();

                if (find.Contains(key))

                {

                    return buffer[i];

                }

            }

            Node<E> n = root;

            while (!(n.isLeaf()))

            {

                //sezrching fo the element

                if (key.CompareTo(n.getKeys()[0]) < 0)

                {// if in the first pointer

                    n = (Node<E>)n.getPointers()[0];

                }

                else if (key.CompareTo(n.getKeys()[n.getKeys().Count - 1]) >= 0)

                {// if in the last pointer

                    n = (Node<E>)n.getPointers()[n.getPointers().Count - 1];

                }

                else

                {

                    for (int i = 0; i < n.getKeys().Count - 1; i++)

                    {

                        if (n.getKeys().Count > 1 && key.CompareTo(n.getKeys()[i]) >= 0 && key.CompareTo(n.getKeys()[i + 1]) < 0)

                        {// general case

                            n = (Node<E>)n.getPointers()[i + 1];

                            break;

                        }

                    }

                }

            }

            for (int i = 0; i < n.getKeys().Count; i++)

            {

                if (key.CompareTo(n.getKeys()[i]) == 0)

                {

                    if (buffer.Count == bufferboolSize)

                    {

                        buffer.RemoveAt(0);

                        buffer.Add(n);

                    }

                    else

                    {

                        buffer.Add(n);

                    }

                    return n;

                }

            }

            return null;

        }

        public void delete(E key)
        {
            LinkedList<Node<E>> stack = new LinkedList<Node<E>>();
            Node<E> n = root;
            //secrching for the required node
            while (!n.isLeaf())
            {
                stack.AddFirst(n);
                // ===================================================
                if (key.CompareTo(n.getKeys()[0]) < 0)
                {
                    n = (Node<E>)n.getPointers()[(0)];
                }
                else if (key.CompareTo(n.getKeys()[(n.getKeys().Count - 1)]) >= 0)
                {
                    n = (Node<E>)n.getPointers()[(n.getPointers().Count - 1)];
                }
                else
                {
                    for (int i = 0; i < n.getKeys().Count; i++)
                    { 
                        if (key.CompareTo(n.getKeys()[(i)]) >= 0 && key.CompareTo(n.getKeys()[(i + 1)]) < 0)

                        {

                            n = (Node<E>)n.getPointers()[(i + 1)];

                            break;

                        }

                    }

                }

            }

            // END OF WHILE

            bool flag = false;

            for (int i = 0; i < n.getKeys().Count; i++)

            {

                if (n == root && key.Equals(n.getKeys()[i]))

                {

                    nm.deleteNode(n, key);

                    return;

                }

                else if (key.Equals(n.getKeys()[i]))

                {

                    flag = true;

                    break;

                }

            }

            //searching to determine if the element is found in leaf node or not

            if (flag)

            {

                //if the node isn't under flow

                if (n.getKeys().Count - 1 >= Math.Ceiling(leafSize / 2.0))

                {

                    nm.deleteNode(n, key);

                    Node<E> parent = stack.First();

                    for (int i = 0; i < parent.getKeys().Count; i++)

                    {

                        if (key.CompareTo(parent.getKeys()[i]) == 0)

                        {

                            parent.getKeys().RemoveAt(i);

                            parent.getKeys().Insert(i, n.getKeys()[(0)]);

                            break;

                        }

                    }

                }

                else

                {

                    // if node is in underflow

                    Node<E> parent = stack.First();

                    // determin if the next node is from the same parent or not to borrow from it

                    int deter = nm.sameParent(n, stack.First(), leafSize);

                    // if next from the same parent

                    if (deter == 1)

                    {

                        // delete the node

                        nm.deleteNode(n, key);

                        // borrow from the next leaf node

                        E element = n.getNext().getKeys()[(0)];

                        n.getNext().getKeys().RemoveAt(0);

                        Object obj = n.getNext().getPointers().Remove(0);

                        n.getKeys().Add(element);

                        n.getPointers().Add(obj);

                        for (int i = 0; i < parent.getKeys().Count; i++)

                        {

                            if (element.CompareTo(parent.getKeys()[i]) == 0)

                            {

                                parent.getKeys().RemoveAt(i);

                                parent.getKeys().Insert(i, n.getNext().getKeys()[0]);

                                break;

                            }

                        }

                        for (int i = 0; i < parent.getKeys().Count; i++)

                        {

                            if (key.CompareTo(parent.getKeys()[(i)]) == 0)

                            {

                                parent.getKeys().RemoveAt(i);

                                parent.getKeys().Insert(i, n.getKeys()[(0)]);

                                break;

                            }

                        }

                        return;

                    }

                    else if (deter == 2)

                    {

                        // borrow from the previous node

                        nm.deleteNode(n, key);

                        E element = n.getPrev().getKeys()[n.getPrev().getKeys().Count - 1];

                        n.getPrev().getKeys().RemoveAt(n.getPrev().getKeys().Count - 1);

                        Object obj = n.getPrev().getPointers().Remove(n.getPrev().getPointers().Count - 1);

                        n.getKeys().Insert(0, element);

                        n.getPointers().Insert(0, obj);

                        for (int i = 0; i < parent.getKeys().Count; i++)

                        {

                            if (element.CompareTo(parent.getKeys()[(i)]) == 0)

                            {

                                parent.getKeys().RemoveAt(i);

                                parent.getKeys().Insert(i, n.getPrev().getKeys()[(n.getPrev().getKeys().Count - 1)]);

                                break;

                            }

                        }

                        for (int i = 0; i < parent.getKeys().Count; i++)

                        {

                            if (key.CompareTo(parent.getKeys()[(i)]) == 0)

                            {

                                parent.getKeys().RemoveAt(i);

                                parent.getKeys().Insert(i, n.getKeys()[0]);

                                break;

                            }

                        }

                        return;

                    }

                    else

                    {

                        // there will be merging for internal nodes

                        bool prevB = true;

                        if (key.Equals(n.getKeys()[(0)]))

                        {

                            prevB = false;

                        }



                        nm.deleteNode(n, key);

                        int tempKey = 0;

                        int tempPointer = 0;

                        // if the merging will be with the next node

                        // then copying all elemnts of the current node in the next node

                        // dalete the first element from the next node in the parent node

                        if (nm.sameParent2(parent, n))

                        {

                            Node<E> next = n.getNext();

                            if (n.getPrev() != null)

                            {

                                n.getPrev().setNext(next);

                            }

                            if (next != null)

                            {

                                next.setPrev(n.getPrev());

                            }

                            n.setNext(null);

                            n.setPrev(null);

                            next.getKeys().InsertRange(0, n.getKeys());

                            next.getPointers().InsertRange(0, n.getPointers());

                            for (int i = 0; i < parent.getKeys().Count; i++)

                            {

                                if (next.getKeys()[(n.getKeys().Count)].CompareTo(parent.getKeys()[(i)]) == 0)

                                {

                                    tempKey = i;

                                    tempPointer = i;

                                    break;

                                }

                            }

                            if (tempKey > 0 && parent.getKeys()[(tempKey - 1)].Equals(key))

                            {

                                parent.getKeys().RemoveAt(tempKey - 1);

                                parent.getKeys().Insert(tempKey - 1, next.getKeys()[0]);

                            }

                        }

                        else

                        {

                            // if the merging will be with the prev node

                            // then copying all elemnts of the node in the prev node

                            // dalete the first element from the current node in the parent node

                            Node<E> prev = n.getPrev();

                            if (prev != null)

                            {

                                prev.setNext(n.getNext());

                            }

                            if (n.getNext() != null)

                            {

                                n.getNext().setPrev(prev);

                            }

                            n.setNext(null);

                            n.setPrev(null);

                            prev.getKeys().AddRange(n.getKeys());

                            prev.getPointers().AddRange(n.getPointers());

                            if (prevB)

                            {

                                for (int i = 0; i < parent.getKeys().Count; i++)

                                {

                                    if (n.getKeys()[0].CompareTo(parent.getKeys()[(i)]) == 0)

                                    {

                                        tempKey = i;

                                        tempPointer = i + 1;

                                        break;

                                    }

                                }

                            }

                            else

                            {

                                for (int i = 0; i < parent.getKeys().Count; i++)

                                {

                                    if (key.CompareTo(parent.getKeys()[i]) == 0)

                                    {

                                        tempKey = i;

                                        tempPointer = i + 1;

                                        break;

                                    }

                                }

                            }

                        }

                        bool finished = false;

                        do

                        {

                            // if we get the root

                            if (stack.Count == 0)

                            {

                                root.getKeys().RemoveAt(tempKey);

                                root.getPointers().RemoveAt(tempPointer);

                                finished = true;

                            }

                            else

                            {

                                n = stack.First();

                                stack.RemoveFirst();

                                //try borrowing from the cebeling

                                if (n.getKeys().Count - 1 >= 1)

                                {

                                    n.getKeys().RemoveAt(tempKey);

                                    n.getPointers().RemoveAt(tempPointer);

                                    finished = true;

                                }

                                else

                                {

                                    // if the root have one cebeling

                                    // the tree level will decrease

                                    if (n == root)

                                    {

                                        n.getKeys().RemoveAt(tempKey);

                                        n.getPointers().RemoveAt(tempPointer);

                                        if (n.getPointers().Count == 1)

                                        {

                                            root = (Node<E>)n.getPointers()[0];

                                        }

                                        finished = true;

                                    }

                                    else

                                    {

                                        n.getKeys().RemoveAt(tempKey);

                                        n.getPointers().RemoveAt(tempPointer);

                                        deter = nm.nexOrprev(n, stack.First(), internalSize);

                                        parent = stack.First();

                                        // borrowing from next internal node

                                        if (deter == 1)

                                        {
                                            int index = -1;
                                            for (int i = 0; i < parent.getPointers().Count; i++)
                                            {
                                                if (parent.getPointers()[i] == n.getNext())
                                                {
                                                    index = i;
                                                    break;
                                                }
                                            }
                                            E tempkey = parent.getKeys()[index - 1];
                                            parent.getKeys().RemoveAt(index - 1);
                                            n.getKeys().Add(tempkey);
                                            Node<E> tempNext = (Node<E>)n.getNext().getPointers()[0];
                                            n.getNext().getPointers().RemoveAt(0);
                                            E nextKey = n.getNext().getKeys()[0];
                                            n.getNext().getKeys().RemoveAt(0);
                                            n.getPointers().Add(tempNext);
                                            parent.getKeys().Insert(index - 1, nextKey);
                                            finished = true;
                                            // boorwing form prev internal node
                                        }
                                        else if (deter == 2)
                                        {
                                            int index = -1;
                                            for (int i = 0; i < parent.getPointers().Count; i++)
                                            {
                                                if (parent.getPointers()[i] == n)
                                                {
                                                    index = i;
                                                    break;
                                                }
                                            }
                                            E tempkey = parent.getKeys()[index - 1];
                                            parent.getKeys().RemoveAt(index - 1);
                                            n.getKeys().Insert(0, tempkey);
                                            Node<E> tempPrev = (Node<E>)n.getPrev().getPointers()[(n.getPrev().getPointers().Count - 1)];
                                            n.getPrev().getPointers().RemoveAt(n.getPrev().getPointers().Count - 1);
                                            E prevKey = n.getPrev().getKeys()[(n.getPrev().getKeys().Count - 1)];
                                            n.getPrev().getKeys().RemoveAt(n.getPrev().getKeys().Count - 1);
                                            n.getPointers().Insert(0, tempPrev);
                                            parent.getKeys().Insert(index - 1, prevKey);
                                            finished = true;
                                        }
                                        else
                                        {
                                            // mergae two internal nodes
                                            if (nm.sameParent2(parent, n))
                                            {
                                                for (int i = 0; i < parent.getPointers().Count; i++)
                                                {
                                                    if (n == parent.getPointers()[i])
                                                    {
                                                        tempKey = i;
                                                        tempPointer = i;
                                                        break;
                                                    }
                                                }
                                                Node<E> next = n.getNext();
                                                if (n.getPrev() != null)
                                                {
                                                    n.getPrev().setNext(next);
                                                }
                                                if (next != null)
                                                {
                                                    next.setPrev(n.getPrev());
                                                }
                                                next.getKeys().Insert(0, parent.getKeys()[(tempKey)]);
                                                next.getKeys().InsertRange(0, n.getKeys());
                                                next.getPointers().InsertRange(0, n.getPointers());
                                            }
                                            else
                                            {
                                                for (int i = 0; i < parent.getPointers().Count; i++)
                                                {
                                                    if (n == parent.getPointers()[i])
                                                    {
                                                        tempKey = i - 1;
                                                        tempPointer = i;
                                                        break;
                                                    }
                                                }
                                                Node<E> prev = n.getPrev();
                                                if (prev != null)
                                                {
                                                    prev.setNext(n.getNext());
                                                }
                                                if (n.getNext() != null)
                                                {
                                                    n.getNext().setPrev(prev);
                                                }
                                                prev.getKeys().Add(parent.getKeys()[(tempKey)]);
                                                prev.getKeys().AddRange(n.getKeys());
                                                prev.getPointers().AddRange(n.getPointers());
                                            }
                                        }
                                    }
                                }
                            }
                        } while (!finished);
                    }
                }
            }
            else
            { // if the elemnet isn't found
                return;
            }
        }
        public int getLeafSize()
        {
            return leafSize;
        }
        public void setLeafSize(int leafSize)
        {
            this.leafSize = leafSize;
        }
        public int getInternalSize()
        {
            return internalSize;
        }
        public void setInternalSize(int internalSize)
        {
            this.internalSize = internalSize;
        }
        public Node<E> getRoot()
        {
            return root;
        }
        public void setRoot(Node<E> root)
        {
            this.root = root;
        }
        public int getSize()
        {
            return size;
        }
        public void setSize(int size)
        {
            this.size = size;
        }
    }
}
