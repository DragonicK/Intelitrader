using System;
using System.Linq;
using System.Globalization;

namespace HelloWorld {
    public enum Action {
        Insert,
        Modify,
        Delete
    }

    public class Item {
        public double Value { get; set; }
        public int Quantity { get; set; }

        public override string ToString() {
            return $"{Value},{Quantity}";
        }
    }

    public class Inventory {
        private readonly Item[] items;

        public Inventory(int size) => items = new Item[size];

        public void Modify(int index, Input input) {
            var item = items[index];

            item ??= new Item();

            if (input.Value != 0) {
                item.Value = input.Value;
            }

            if (input.Quantity != 0) {
                item.Quantity = input.Quantity;
            }

            items[index] = item;
        }

        public void Delete(int index) {
            items[index] = null;

            Shift(index);
        }

        public Item[] ToArray() => items.Where(x => x != null).ToArray();

        private void Shift(int index) {
            for (var i = index; i < items.Length - 1; i++) {
                (items[i], items[i + 1]) = (items[i + 1], items[i]);
            }
        }
    }

    public abstract class Input {
        public int Position { get; set; }
        public double Value { get; set; }
        public int Quantity { get; set; }
        public abstract void Execute(Inventory inventory);
    }

    public class InsertInput : Input {
        public override void Execute(Inventory inventory) {
            inventory.Modify(Position, this);
        }
    }

    public class ModifyInput : Input {
        public override void Execute(Inventory inventory) {
            inventory.Modify(Position, this);
        }
    }

    public class DeleteInput : Input {
        public override void Execute(Inventory inventory) {
            inventory.Delete(Position);
        }
    }

    public class InvalidInputException : Exception {
        public InvalidInputException(string message) : base(message) { }
    }

    public class InvalidParameterException : InvalidInputException {
        public InvalidParameterException(string paramName) : base($"Parameter '{paramName}' is not a valid number!") { }
    }

    public static class InputStream {
        private static readonly Type[] inputs = new Type[] {
                    typeof(InsertInput),
                    typeof(ModifyInput),
                    typeof(DeleteInput)
            };

        public static Input[] Read() {
            if (!int.TryParse(Console.ReadLine(), out var count)) {
                throw new InvalidInputException("Failed to parse first line (count).");
            }

            var culture = new CultureInfo("en-US", false);

            var states = new Input[count];

            for (var i = 0; i < count; ++i) {
                var line = Console.ReadLine();

                if (line == null) {
                    break;
                }

                var parts = line.Split(',');

                if (parts.Length != 4) {
                    throw new InvalidInputException($"Invalid input! Expecting 4 parameters, found '{parts.Length}'!");
                }

                if (!int.TryParse(parts[0], out var position)) {
                    throw new InvalidParameterException("Position");
                }

                if (!Enum.TryParse(parts[1], out Action action)) {
                    throw new InvalidParameterException("Action");
                }

                if (!double.TryParse(parts[2], culture, out var value)) {
                    throw new InvalidParameterException("Value");
                }

                if (!int.TryParse(parts[3], out var quantity)) {
                    throw new InvalidParameterException("Quantity");
                }

                var input = (Input)Activator.CreateInstance(inputs[(int)action])!;

                input.Position = position - 1;
                input.Value = value;
                input.Quantity = quantity;

                states[i] = input;
            }

            return states;
        }
    }

    public class Program {
        public static void Main(string[] args) {
            var states = InputStream.Read();
            var inventory = new Inventory(states.Length);

            foreach (var state in states) {
                state.Execute(inventory);
            }

            var index = 0;

            foreach (var item in inventory.ToArray()) {
                Console.WriteLine($"{++index},{item}");
            }
        }
    }
}