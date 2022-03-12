using System;
using System.Collections.Generic;
using System.Linq;

namespace Solutions
{
    public class ListNode
    {
        public int val;
        public ListNode next;

        public ListNode(ListNode pNext = null, int x = 0)
        {
            next = pNext;
            val = x;
        }

        public class DoubleListNode
        {
            public int val;
            public DoubleListNode prev;
            public DoubleListNode next;

            public DoubleListNode(DoubleListNode pPrev = null, DoubleListNode pNext = null, int x = 0)
            {
                prev = pPrev;
                next = pNext;
                val = x;
            }
        }

        public class Solution
        {
            public ListNode ReverseList(ListNode pHead)
            {
                // write code here
                ListNode prev = pHead;
                ListNode cur;
                if (prev == null || (cur = prev.next) == null)
                    return prev;

                ListNode tmp;
                while (cur != null)
                {
                    tmp = cur.next;
                    cur.next = prev;
                    prev = cur;
                    cur = tmp;
                    // Console.Write(cur.val);
                }

                return prev;
            }

            public int[] BinaryCount(int[] nums)
            {
                int[] counts = new int[32]; //from 0 to 31

                int count = 0;
                for (int i = 0; i < nums.Length; i++)
                {
                    //Count number of 1's in num
                    count = 0;
                    while (nums[i] > 0)
                    {
                        //Eliminate the last one
                        nums[i] %= (nums[i] - 1);
                        count++;
                    }

                    //Add count to the group
                    counts[count]++;
                }

                return counts;
            }

            /// <summary>
            ///
            /// </summary>
            /// <param name="m">capacity</param>
            /// <param name="t">time</param>
            /// <param name="m1">in-capacity</param>
            /// <param name="t1">in-switch time</param>
            /// <param name="m2">out-capacity</param>
            /// <param name="t2">out-switch time</param>
            /// <returns></returns>
            public int WaterRemaining(int m, int t, int m1, int t1, int m2, int t2)
            {
                bool inOn, outOn;
                inOn = outOn = false;
                int curWater = 0;   //Current water in the pool

                for (int i = 0; i < t; i++)
                {
                    //Switch state of the in and out
                    if (i % t1 == 0)
                        inOn = !inOn;
                    if (i % t2 == 0)
                        outOn = !outOn;

                    if (inOn)
                        curWater += m1;
                    if (outOn)
                        curWater -= m2;

                    //Clamp to [0, m]
                    if (curWater > m)
                        curWater = m;
                    else if (curWater < 0)
                        curWater = 0;
                }

                return curWater;
            }

            public int[] FloodSplit(int[] floods, int[] rooms)
            {
                int[] res = new int[floods.Length];

                for (int i = 0; i < res.Length; i++)
                    res[i] = 1;

                int curFlood;
                bool distroyed, lastDistroyed, allDistroyed;

                for (int i = 0; i < floods.Length; i++)
                {
                    curFlood = floods[i];
                    lastDistroyed = false;
                    allDistroyed = true;

                    for (int j = 0; j < rooms.Length - 1; j++)
                    {
                        distroyed = rooms[j] <= curFlood;
                        allDistroyed = allDistroyed && distroyed;
                        if (!lastDistroyed && distroyed)
                            res[i]++;
                    }

                    //If all cities are flooded, returns zero
                    allDistroyed = allDistroyed && rooms.Last() <= curFlood;
                    if (allDistroyed)
                        res[i] = 0;
                }
                return res;
            }

            public int FirstMissingPositive(int[] nums)
            {
                int mis = 1;

                //Clamp non-positive and greater than nums.Length into [1, nums.Length]
                bool foundOne = false;
                for (int i = 0; i < nums.Length; i++)
                {
                    if (!foundOne)
                        foundOne = nums[i] == 1;

                    if (nums[i] <= 0 || nums[i] > nums.Length)
                        nums[i] = 1;
                }
                if (!foundOne)
                    return 1;

                int index;
                int val;
                for (int i = 0; i < nums.Length; i++)
                {
                    val = Math.Abs(nums[i]);

                    if (nums[index = val - 1] > 0)
                        nums[index] = -nums[index];
                }

                for (mis = 1; mis <= nums.Length; mis++)
                    if (nums[mis - 1] > 0)
                        return mis;

                return mis;
            }

            public int MissingNumber(int[] nums)
            {
                //Check if the ans is 0 or 1
                bool foundZero, foundOne;
                foundZero = foundOne = false;
                for (int i = 0; i < nums.Length; i++)
                {
                    if (foundZero && foundOne)
                        break;

                    if (!foundZero && nums[i] == 0)
                        foundZero = true;
                    if (!foundOne && nums[i] == 1)
                        foundOne = true;
                }

                if (!foundZero)
                    return 0;
                if (!foundOne)
                    return 1;

                for (int i = 0; i < nums.Length; i++)
                    if (nums[i] == 0)
                        nums[i] = 1;

                int val;
                for (int i = 0; i < nums.Length; i++)
                {
                    val = Math.Abs(nums[i]);
                    if (nums[val - 1] > 0)
                        nums[val - 1] = -nums[val - 1];
                }

                for (int i = 0; i < nums.Length; i++)
                    if (nums[i] > 0)
                        return i + 1;

                return -1;
            }

            public string LongestCommonPrefix(string[] strs)
            {
                string prefix = "";
                int minLength = strs.Aggregate(0, (curMin, curStr) => Math.Min(curMin, curStr.Length));

                int end = 0;
                for (; end < minLength; end++)
                {
                    char curC = strs[0][end];
                    if (strs.Any(str => str[end] != curC))
                        break;
                }

                if (end > 0)
                    prefix = strs[0].Substring(0, end);
                return prefix;
            }

            public int MaxProfitAssignment(int[] difficulty, int[] profit, int[] worker)
            {
                int profitTol = 0;
                Array.Sort(worker);
                Array.Sort(profit, difficulty);

                int jobIndex = profit.Length - 1;
                for (int i = worker.Length - 1; i >= 0; i--)
                {
                    //Find the Best job for the worker
                    while (difficulty[jobIndex] > worker[i])
                    {
                        jobIndex--;
                    }

                    profitTol += profit[jobIndex];
                }

                return profitTol;
            }

            public int SubarraySum(int[] nums, int k)
            {
                int sum, count;
                sum = count = 0;

                Dictionary<int, int> set = new Dictionary<int, int>();
                set[0] = 1;
                for (int i = 0; i < nums.Length; i++)
                {
                    sum += nums[i];

                    if (set.ContainsKey(k - sum))
                        count += set[k - sum];

                    if (set.ContainsKey(sum))
                        set[sum] += 1;
                    else
                        set[sum] = 1;
                }

                return count;
            }

            public int NumSubarrayProductLessThanK(int[] nums, int k)
            {
                uint[] prodSums = new uint[nums.Length + 1];

                uint prod = 1;
                prodSums[0] = prod;
                for (int i = 0; i < nums.Length; i++)
                {
                    prodSums[i + 1] = (prod *= (uint)nums[i]);
                }
                int start, end, count;
                start = count = 0;
                end = 1;
                while (end < prodSums.Length && start < end)
                {
                    uint subProd = prodSums[end] / prodSums[start];
                    if (subProd < k)
                    {
                        count += end - start;
                        end++;
                    }
                    else
                        start++;
                }

                return count;
            }

            public int NumSubarrayProductLessThanK2(int[] nums, int k)
            {
                int count = 0;

                int start = 0;
                int prodDiff = 1;
                for (int end = 0; end < nums.Length; end++)
                {
                    //Adjust start position
                    prodDiff *= nums[end];
                    while (start <= end && prodDiff >= k)
                    {
                        prodDiff /= nums[start++];
                    }
                    count += end - start + 1;
                }

                return count;
            }

            public int[] MaxSlidingWindow(int[] nums, int k)
            {
                int[] res = new int[nums.Length - k + 1];

                int max = int.MinValue;
                Dictionary<int, int> dic = new Dictionary<int, int>();
                for (int i = 0; i < nums.Length; i++)
                {
                    max = (max > nums[i]) ? max : nums[i];
                }

                return res;
            }

            public string Tictactoe(int[][] moves)
            {
                int n = 3;
                if (moves.Length < 5)
                    return "Pending";

                int[] rows, cols;
                rows = cols = new int[n];
                int diag, antiDiag;
                diag = antiDiag = 0;

                int player = 1;
                foreach (var move in moves)
                {
                    rows[move[0]] += player;
                    cols[move[0]] += player;
                    if (move[0] == move[1])
                        diag += player;
                    if (n - move[0] - 1 == move[1])
                        antiDiag += player;

                    if (rows[move[0]] == player * n || cols[move[1]] == player * n || diag == player * n || antiDiag == player * n)
                        return (player == 1) ? "A" : "B";

                    player = -player;
                }

                return moves.Length == 9 ? "Draw" : "Pending";
            }

            public class TicTacToe
            {
                private int n;

                private int[] rows;
                private int[] cols;
                private int diag, antiDiag;

                private int winner = -1;
                public int Winner => winner;
                public bool IsEnd => winner != -1;

                public TicTacToe(int n)
                {
                    this.n = n;
                    rows = new int[n];
                    cols = new int[n];
                    diag = antiDiag = 0;
                }

                public int Move(int row, int col, int player)
                {
                    if (IsEnd)
                        return winner;
                    int win = (player % 2 == 1) ? 1 : -1;

                    rows[row] += win;
                    cols[col] += win;

                    if (row == col)
                        diag += win;
                    if (n - row - 1 == col)
                        antiDiag += win;

                    if (rows[row] == win * n || cols[col] == win * n || diag == win * n || antiDiag == win * n)
                    {
                        winner = player;
                        return player;
                    }
                    return 0;
                }
            }

            public int[][] Merge(int[][] intervals)
            {
                if (intervals.Length == 1)
                    return intervals;

                Array.Sort(intervals, CompareInverval);
                List<int[]> res = new List<int[]>();

                int start, end;
                start = 0;
                end = 1;

                int curMin, curMax;
                curMin = intervals[0][0];
                curMax = intervals[0][1];
                while (end < intervals.Length)
                {
                    if (intervals[end][0] > curMax)
                    {
                        //Merge sub intervals
                        res.Add(new int[] { curMin, curMax });

                        //Update start index
                        start = end;
                        curMin = intervals[start][0];
                        curMax = intervals[start][1];
                    }
                    else
                    {
                        curMax = Math.Max(curMax, intervals[end][1]);
                        end++;
                    }
                }

                res.Add(new int[] { curMin, curMax });
                return res.ToArray();
            }

            private int CompareInverval(int[] interval1, int[] interval2)
            {
                if (interval1[0] > interval2[0])
                    return 1;

                if (interval1[0] == interval2[0])
                {
                    if (interval1[1] == interval2[1])
                        return 0;
                    return (interval1[1] < interval2[1]) ? -1 : 1;
                }

                return -1;
            }

            #region Math

            public int Reverse(int x)
            {
                int sign = Math.Sign(x);

                if (x == int.MinValue)
                    return 0;

                x = Math.Abs(x);
                int res = 0;
                int deg = 1;
                int y = x;
                while ((y /= 10) > 0)
                    deg *= 10;

                while (x > 0)
                {
                    try
                    {
                        res = checked(res + (x % 10) * deg);
                    }
                    catch (System.OverflowException)
                    {
                        return 0;
                    }
                    deg /= 10;
                    x /= 10;
                }

                return res * sign;
            }

            public bool IsPalindrome(int x)
            {
                if (x < 0)
                    return false;

                return x == Reverse(x);
            }

            #endregion Math

            #region Dynamic Programming

            public int Fib(int n)
            {
                if (n < 2)
                    return n;

                int cur, prev0, prev1;
                cur = 0;
                prev0 = 0;
                prev1 = 1;
                for (int i = 2; i <= n; i++)
                {
                    cur = prev0 + prev1;
                    prev0 = prev1;
                    prev1 = cur;
                }

                return cur;
            }

            public int Tribonacci(int n)
            {
                if (n < 2)
                    return n;
                if (n == 2)
                    return 1;

                int cur, prev0, prev1, prev2;
                cur = 0;
                prev0 = 0;
                prev1 = prev2 = 1;
                for (int i = 3; i <= n; i++)
                {
                    cur = prev0 + prev1 + prev2;
                    prev0 = prev1;
                    prev1 = prev2;
                    prev2 = cur;
                }

                return cur;
            }

            #endregion Dynamic Programming

            #region String

            public int LengthOfLongestSubstring(string s)
            {
                Dictionary<char, int> set = new Dictionary<char, int>();
                int longest = 0;
                int start, end;
                start = end = 0;

                while (end < s.Length)
                {
                    if (!set.ContainsKey(s[end]) || set[s[end]] < start)
                        longest = Math.Max(longest, end - start + 1);
                    else
                        start = set[s[end]] + 1;
                    set[s[end]] = end++;
                }

                return longest;
            }

            #endregion String

            #region Linked List

            /// <summary>
            /// If a linked list is a palindrome
            /// </summary>
            /// <param name="head"></param>
            /// <returns></returns>
            public bool IsPalindrome(ListNode head)
            {
                int cnt = 0;
                ListNode cur = head;
                while (cur != null)
                {
                    cur = cur.next;
                    cnt++;
                }

                ListNode prev;
                prev = null;
                cur = head;

                for (int i = 0; i < cnt / 2; i++)
                    cur = cur.next;

                //Reverse Half List
                while (cur != null)
                {
                    ListNode next = cur.next;
                    cur.next = prev;
                    prev = cur;
                    cur = next;
                }

                for (int i = 0; i < cnt / 2; i++)
                {
                    if (head.val != prev.val)
                        return false;
                    prev = prev.next;
                    head = head.next;
                }

                return true;
            }

            /// <summary>
            /// Find the max value of pair nodes on the two sides of the linked list
            /// </summary>
            /// <param name="head"></param>
            /// <returns></returns>
            public int PairSum(ListNode head)
            {
                int cnt = 0;
                ListNode cur = head;
                while (cur != null)
                {
                    cur = cur.next;
                    cnt++;
                }

                ListNode prev;
                prev = null;
                cur = head;

                for (int i = 0; i < cnt / 2; i++)
                    cur = cur.next;

                //Reverse Half List
                while (cur != null)
                {
                    ListNode next = cur.next;
                    cur.next = prev;
                    prev = cur;
                    cur = next;
                }

                int max = int.MinValue;

                for (int i = 0; i < cnt / 2; i++)
                {
                    max = Math.Max(max, head.val + prev.val);
                    prev = prev.next;
                    head = head.next;
                }

                return max;
            }

            /// <summary>
            /// Reverse a double linked list in-place
            /// </summary>
            /// <param name="head"></param>
            public DoubleListNode ReverseDoubleLinkedList(DoubleListNode head)
            {
                DoubleListNode tmp;
                while (head != null)
                {
                    tmp = head.next;
                    head.next = head.prev;
                    head.prev = tmp;
                    head = head.prev;
                }

                return head;
            }

            public ListNode SwapPairs(ListNode head)
            {
                ListNode dummy = new ListNode(-1, head);
                ListNode prev, cur;
                prev = dummy;
                cur = head;

                while (cur != null && cur.next != null)
                {
                    ListNode first, second;
                    first = cur.next;
                    second = first.next;

                    prev.next = first;
                    first.next = cur;
                    cur.next = second;

                    prev = cur;
                    cur = cur.next;
                }

                return dummy.next;
            }

            public ListNode SwapNodes(ListNode head, int k)
            {
                ListNode dummy = new ListNode();
                dummy.next = head;

                //Find nodes just before the desired ones
                ListNode headKBefore, tailKBefore;
                headKBefore = tailKBefore = dummy;


            }

            #endregion Linked List

            public static void Main()
            {
                Solution sol = new Solution();
                #region Init Double Linked Lists
                DoubleListNode list1 = new DoubleListNode(null, 1);
            }
        }
    }
}