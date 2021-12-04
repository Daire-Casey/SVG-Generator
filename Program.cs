// OS: Windows 10
// IDE: VS Code
// Pattern: Command

using System;
using System.Collections.Generic;
using System.IO;

namespace a4 {
    public class Program {

        static void Main(string[] args) {
            Canvas canvas = new Canvas(); // create a new canvas
            User user = new User(); // create a new user

            Console.WriteLine("Welcome! Please enter an operation. Enter H to see a list of operations.");

            bool running = true; // tells the program to keep waiting for user inputs
            while(running) {
                string op = Console.ReadLine(); // read user input
                if(op.Length > 0 && (op.Length == 1 || op[0] == 'A')) { // checks input format is valid
                    switch(op[0]) {
                        // the help operation shows a list of valid operations
                        case 'H': Console.WriteLine("\nH	 	 Help	 - displays this message\nA <shape>        Add	 <shape> to canvas\nU	 	 Undo	 last operation\nR	 	 Redo	 last operation\nD	 	 Display canvas \nC	 	 Clear	 canvas\nS	 	 Save	 canvas \nQ	 	 Quit	 application\n");
                            break;
                        // the add operation allows the adding of shapes to the canvas
                        case 'A': try {
                                Add(op.Split(' ')[1], canvas, user); // retrieves the parameter given and calls the add operation
                            } catch (Exception) {
                                Console.WriteLine("ERROR: Incorrect use of the add operation: A <shape>."); // if no space is put between A and the parameter
                            }
                            break;
                        // the undo operation removes the most recent shape from the canvas
                        case 'U': user.Undo(); // calls the undo method from the invoker
                            break;
                        // the redo operation adds undone shapes back to the canvas
                        case 'R': user.Redo(); // calls the redo method from the invoker
                            break;
                        // the display operation shows the SVG form of the current canvas
                        case 'D': Console.WriteLine("\n{0}\n", canvas.GetSVG());
                            break;
                        // the clear operation wipes the canvas clean
                        case 'C': canvas.canvas.Clear();
                            user.Reset(); // reset the undo and redo stacks to avoid inconsistencies and errors
                            Console.WriteLine("Canvas cleared.");
                            break;
                        // the save operation exports the current canvas to an SVG file
                        case 'S': canvas.Export();
                            break;
                        // the quit operation ends the program
                        case 'Q': running = false; // the program will not wait for user input after this, therefore ending
                            Console.WriteLine("Goodbye!");
                            break;
                        // if the formatting was correct but the character given is not a valid one
                        default: Console.WriteLine("Please enter a valid operation. Enter H to see a list of valid operation.");
                            break;
                    }
                } else { // if the formatting was incorrect
                    Console.WriteLine("Please enter a valid operation. Enter H to see a list of valid operation.");
                }
            }
        }
        
        // the add method is called when a user enters "A <shape>", and adds a shape to the canvas
        public static void Add(string input, Canvas canvas, User user) {
            switch(input) { // adds a different type of shape depending on the given string
                case "circle": user.Action(new AddShapeCommand(new Circle(), canvas)); // calls the action method with the type AddShapeCommand, for a circle
                    break;
                case "rectangle": user.Action(new AddShapeCommand(new Rectangle(), canvas));
                    break;
                case "ellipse": user.Action(new AddShapeCommand(new Ellipse(), canvas));
                    break;
                case "line": user.Action(new AddShapeCommand(new Line(), canvas));
                    break;
                case "polyline": user.Action(new AddShapeCommand(new Polyline(), canvas));
                    break;
                case "polygon": user.Action(new AddShapeCommand(new Polygon(), canvas));
                    break;
                default: Console.WriteLine("ERROR: {0} is not a valid shape.", input); // if the given string is not a valid shape
                    break;
            }
        }
    }

    // this class is the receiver class, it has a stack to which shapes are added
    public class Canvas {
        public Stack<Shape> canvas = new Stack<Shape>(); // this is the stack where we put shapes

        public void AddShape(Shape shp) // this method adds a shape to the canvas
        {
            canvas.Push(shp);
            Console.WriteLine("Shape added to the canvas.");
        }

        public Shape DelShape() // this method deletes a shape from the canvas
        {
            Shape shp = canvas.Pop();
            Console.WriteLine("Shape deleted from the canvas.");
            return shp;
        }

        public string GetSVG() { // returns the SVG code of the current canvas
            string svg = @"<svg height=""900"" width=""1400"" xmlns=""http://www.w3.org/2000/svg"">" + Environment.NewLine;
            
            foreach(Shape shp in canvas) // gets the SVG for each shape
            {
                svg += shp.ToString() + Environment.NewLine;
            }

            svg += @"</svg>";

            return svg;
        }

        public void Export() // exports the current canvas to an SVG file
        {
            string svg = GetSVG(); // get the SVG code of the current canvas

            File.WriteAllTextAsync("svgFile.svg", svg); // writes to the file
            Console.WriteLine("Canvas exported to SVG file.");
        }
    }

    // the invoker class
    public class User
    {
        private Stack<Command> undo; // commands that can be undone are stored here
        private Stack<Command> redo; // commands that can be redone are stored here

        public int UndoCount { get => undo.Count; } // used to see if an undo operation is possible
        public int RedoCount { get => redo.Count; } // used to see if a redo operation is possible
        public User() // constructor
        {
            Reset();
            Console.WriteLine("Created a new User!"); Console.WriteLine();
        }
        public void Reset() // sets the stacks to 2 new empty ones
        {
            undo = new Stack<Command>();
            redo = new Stack<Command>();
        }

        public void Action(Command command)
        {
            undo.Push(command); // add command to undo stack
            redo.Clear(); // undone commands cannot be redone after new ones have been executed, as is standard for undo redo functionality

            Type t = command.GetType(); // gets the type of command
            if (t.Equals(typeof(AddShapeCommand))) // in our case there is only one type
            {
                Console.WriteLine("Command Received: Add new Shape!" + Environment.NewLine);
                command.Do();
            }
        }

        public void Undo() // undo method
        {
            if (undo.Count > 0) // check if the operation is possible
            {
                Console.WriteLine("Undoing operation:");
                Command c = undo.Pop(); c.Undo(); redo.Push(c); // undo command and save it to redo stack
            } else {
                Console.WriteLine("ERROR: Nothing to undo.");
            }
        }

        public void Redo() // redo method
        {
            if (redo.Count > 0) // check if the operation is possible
            {
                Console.WriteLine("Redoing operation:");
                Command c = redo.Pop(); c.Do(); undo.Push(c); // redo command and save it to undo stack
            } else {
                Console.WriteLine("ERROR: Nothing to redo.");
            }
        }
    }

    public abstract class Command // class inherited by concrete commands
    {
        public abstract void Do(); // what happens when we execute
        public abstract void Undo(); // what happens when we unexecute
    }

    public class AddShapeCommand : Command // a concrete command class. adds shape as the do action
    {
        Shape shape;
        Canvas canvas;

        public AddShapeCommand(Shape s, Canvas c)
        {
            shape = s;
            canvas = c;
        }

        public override void Do() // adds shape as do action
        {
            canvas.AddShape(shape);
        }

        public override void Undo() // removes shape as undo action
        {
            shape = canvas.DelShape();
        }

    }
}