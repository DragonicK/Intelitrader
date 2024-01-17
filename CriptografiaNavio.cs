using System;
using System.Collections.Generic;

namespace ConsoleApp1 {
    public class Program {
        public static void Main(string[] args) {
            var messages = new List<int>() {
                Convert.ToInt32("10010110", 2),
                Convert.ToInt32("11110111", 2),
                Convert.ToInt32("01010110", 2),
                Convert.ToInt32("00000001", 2),
                Convert.ToInt32("00010111", 2),
                Convert.ToInt32("00100110", 2),
                Convert.ToInt32("01010111", 2),
                Convert.ToInt32("00000001", 2),
                Convert.ToInt32("00010111", 2),
                Convert.ToInt32("01110110", 2),
                Convert.ToInt32("01010111", 2),
                Convert.ToInt32("00110110", 2),
                Convert.ToInt32("11110111", 2),
                Convert.ToInt32("11010111", 2),
                Convert.ToInt32("01010111", 2),
                Convert.ToInt32("00000011", 2)
            };

            messages.ForEach(x => {
                Console.Write(Decode(x));
            });
        }

        private static char Decode(int x) {
            int last = x & 0x1;
            int penult = x >> 1 & 0x1;

            x = last == 0 ? ReverseBit(x, 0, true) : ReverseBit(x, 0, false);

            x = penult == 0 ? ReverseBit(x, 1, true) : ReverseBit(x, 1, false);

            return (char)SwapNibble(x);
        }

        private static int SwapNibble(int value) {
            return (value & 0x0F) << 4 | (value & 0xF0) >> 4;
        }

        private static int ReverseBit(int value, int position, bool enabled) {
            return enabled ? value |= (1 << position) : value &= ~(1 << position);
        }
    }
}