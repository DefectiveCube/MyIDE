using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Input;

namespace IDE_WPF
{
    public class InputManager
    {
        public List<Key> KeysPressed { get; set; }

        public ModifierKeys ModifierKeys { get; set; }

        public InteractionMode Mode { get; set; }

        private Dictionary<ModifierKeys, Dictionary<Predicate<Key>, Action>> Bindings;

        public InputManager()
        {
            KeysPressed = new List<Key>();
            ModifierKeys = ModifierKeys.None;

            Bindings = new Dictionary<ModifierKeys, Dictionary<Predicate<Key>, Action>>();
        }

        public void Add(Action action, ModifierKeys mod, Predicate<Key> kFunc)
        {
            if (Bindings.ContainsKey(mod))
            {
                if (Bindings[mod].ContainsKey(kFunc))
                {
                    Bindings[mod][kFunc] = action;
                }
                else
                {
                    Bindings[mod].Add(kFunc, action);
                }
            }
            else
            {
                Bindings.Add(mod, new Dictionary<Predicate<Key>, Action>() { { kFunc, action } });
            }
        }

        public void Add(Action action, Predicate<Key> func)
        {
            Add(action, ModifierKeys.None, func);
        }

        public void Add(Action action, ModifierKeys mod, Key key)
        {
            Add(action, mod, k => k == key);
        }

        public void Add(Action action, Key key)
        {
            Add(action, k => k == key);
        }

        public void KeyUp(object sender, KeyEventArgs e)
        {
            KeyUp(e.Key);
        }

        public void KeyUp(Key key)
        {
            if (!key.IsModifier())
            {
                KeysPressed.Remove(key);
            }
            else
            {
                ModifierKeys |= key.ToModifierKey();
                ModifierKeys ^= key.ToModifierKey();
            }
        }

        public void KeyDown(object sender, KeyEventArgs e)
        {
            KeyDown(e.Key);

            e.Handled = true;
        }

        public void KeyDown(Key key)
        {
            if (!key.IsModifier())
            {
                KeysPressed.Add(key);
            }
            else
            {
                ModifierKeys |= key.ToModifierKey();
            }

            EvaluateCombination();
        }

        private void EvaluateCombination()
        {
            // Evaluate the predicates for the modifier keys. It is possible to have multiple results returned! (which means bindings need to be fixed!)
            if(KeysPressed.Count() == 0)
            {
                return;
            }

            bool passed = true;

            List<Action> actions = new List<Action>();

            // ModifierKeys is a Flags enum (makes this much easier!)
            var m = ModifierKeys;

            if (Bindings.ContainsKey(m))
            {
                // Test each predicate to see if it matchs the keys pressed
                foreach (var func in Bindings[m].Keys)
                {
                    passed = true;

                    foreach (var k in KeysPressed)
                    {
                        if (!func(k))
                        {
                            passed = false;
                            break;
                        }
                    }

                    if (passed)
                    {
                        actions.Add(Bindings[m][func]);
                    }
                }
            }

            if (actions.Count() > 0)
            {
                actions.First().Invoke();

                if (actions.Count() > 1)
                {
                    Debug.WriteLine("WARNING! Multiple actions are bound to the same input!");
                }
            }
        }

        /*private void Type()
        {
            if (Typed != null)
            {
                Typed(KeysPressed.Last(), IsShiftDown());
            }
        }
        
           private void Move()
        {
            var line = 0;
            var column = 0;

            KeysPressed.ToArray();
            if (Key.Left.IsDown())
            {
                column--;
            }

            if (Key.Right.IsDown())
            {
                column++;
            }

            if (Key.Up.IsDown())
            {
                line--;
            }

            if (Key.Down.IsDown())
            {
                line++;
            }

            if((line != 0 || column != 0) && Moved != null) 
            {
                Moved(line, column);
            }
        }*/

        public static bool IsModifierDown()
        {
            return GetModifierKeys() != ModifierKeys.None;
        }

        public static bool IsCtrlDown()
        {
            return Key.LeftCtrl.IsDown() || Key.RightCtrl.IsDown();
        }

        public static bool IsShiftDown()
        {
            return Key.LeftShift.IsDown() || Key.RightShift.IsDown();
        }

        public static bool IsAltDown()
        {
            return Key.LeftAlt.IsDown() || Key.RightAlt.IsDown();
        }

        public static ModifierKeys GetModifierKeys()
        {
            var mod = ModifierKeys.None;

            if (IsShiftDown())
            {
                mod |= ModifierKeys.Shift;
            }

            if (IsCtrlDown())
            {
                mod |= ModifierKeys.Control;
            }

            if (IsAltDown())
            {
                mod |= ModifierKeys.Alt;
            }

            return mod;
        }
    }
}