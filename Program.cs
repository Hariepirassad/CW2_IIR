using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CW2_IIR
{
    class Program
    {
        private static int maxRules = 50;
        private static int eventNum = 5;
        private static int maxEventActions = 5;

        static void Main(string[] args)
        {
            int actionNumber = findNumberOfActions(maxRules);
            //Console.WriteLine(actionNumber);
            List<Action> actions = creatActionList(actionNumber);
            List<Event> events = creatEventList(eventNum);
            List<Rule> rules = creatRulesList(actions, events, maxRules, maxEventActions);
            
            //List<Rule> rules = userRules();
            
            print(rules);
            checkConflicts(rules);

            Console.ReadKey();
        }

        private static int findNumberOfActions(int maxRulesNumber)
        {
            int result = 0;
            int number = 1;
            while (result < maxRulesNumber)
            {
                number++;
                result = sumCombination(number, maxEventActions);

            }

            return number + 1;
        }

        private static int sumCombination(int number, int maxEventActions)
        {
            int count = 0;

            for (int i = 1; i <= maxEventActions; i++)
            {
                count += combination(number, i);
            }

            return count;
        }

        private static int combination(int N, int K)
        {
            int n = 1;
            int k;

            if (N < K)
                return 0;

            for (k = 1; k <= K; k++)
            {
                n *= N--;
                n /= k;
            }
            return n;
        }

        private static void print(List<Rule> rules)
        {
            for (int i = 0; i < rules.Count; i++)
                Console.WriteLine("rule_" + (i+1) + ": "  + rules[i].toString());
        }

        private static void checkConflicts(List<Rule> rules)
        {

            for (int i = 0; i < rules.Count - 1; i++)
            {
                for (int j = i + 1; j < rules.Count; j++)
                {
                    if (rules[i].e.id == rules[j].e.id)
                    {
                        if (rules[i].actionList.Count == rules[j].actionList.Count)
                        {
                            int redundancyCounter = 0;
                            for (int k = 0; k < rules[i].actionList.Count; k++)
                            {
                                if (rules[i].actionList[k].id == rules[j].actionList[k].id)
                                {
                                    redundancyCounter++;
                                }
                            }
                            if (redundancyCounter == rules[i].actionList.Count)
                            {
                                Console.WriteLine("Redundancy : rule_" + (i + 1) + " and rule_" + (j + 1));
                            }

                        }
                        else
                        {
                            if (rules[i].actionList.Count > rules[j].actionList.Count)
                            {
                                int shadowingCounter = 0;
                                for (int l = 0; l < rules[j].actionList.Count; l++)
                                {
                                    if (rules[i].actionList[l].id == rules[j].actionList[l].id)
                                    {
                                        shadowingCounter++;
                                    }
                                }
                                if (shadowingCounter == rules[j].actionList.Count)
                                {
                                    Console.WriteLine("Shadowing : rule_" + (i + 1) + " and rule_" + (j + 1));
                                }
                            }
                            else
                            {
                                int shadowingCounter = 0;
                                for (int l = 0; l < rules[i].actionList.Count; l++)
                                {
                                    if (rules[i].actionList[l].id == rules[j].actionList[l].id)
                                    {
                                        shadowingCounter++;
                                    }
                                }
                                if (shadowingCounter == rules[i].actionList.Count)
                                {
                                    Console.WriteLine("Shadowing : rule_" + (i + 1) + " and rule_" + (j + 1));
                                }
                            }
                        }
                    }
                }
            }
        }

        private static List<Action> creatActionList(int actionNumber)
        {
            List<Action> actions = new List<Action>();

            for (int i = 1; i <= actionNumber; i++)
            {
                actions.Add(new Action(i));
            }

            return actions;
        }

        private static List<Event> creatEventList(int eventNumber)
        {
            List<Event> events = new List<Event>();

            for (int i = 1; i <= eventNumber; i++)
            {
                events.Add(new Event(i));
            }

            return events;
        }

        private static List<Rule> creatRulesList(List<Action> actions, List<Event> events, int maxRules, int maxEventActions)
        {
            List<Rule> rules = new List<Rule>();
            Random r = new Random();

            for (int i = 1; i <= maxRules; i++)
            {
                Event selectedEvent = events[r.Next(events.Count)];
                List<Action> associatedActions = new List<Action>();
                int actionNum = r.Next(maxEventActions) + 1;
                for (int j = 0; j < actionNum; j++)
                {
                    associatedActions.Add(actions[r.Next(actions.Count)]);      
                }
                associatedActions = removeDuplicateActions(associatedActions);

                rules.Add(new Rule(selectedEvent, associatedActions));
            }

            return rules;
        }

        private static List<Action> removeDuplicateActions(List<Action> actionList)
        {
            int size = actionList.Count;

            if (size > 1)
            {
                for (int i = 0; i < size - 1; i++)
                {
                    for (int j = i+1; j < size; j++)
                    {
                        if (actionList[i].id == actionList[j].id)
                        {
                            actionList.Remove(actionList[j]);
                            size = size - 1;
                        }
                    }
                }
            }
         
            return actionList;
        }

        private static List<Rule> userRules()
        {
            List<Rule> rules = new List<Rule>();
            int numberOfRules;
            int eventNumber;
            int numberOfActions;
            int actionNumber;

            Console.WriteLine("Choose the number of rules : ");
            String ruleNb = Console.ReadLine();

            try
            {
                numberOfRules = Int32.Parse(ruleNb);
                for (int i = 0; i < numberOfRules; i++)
                {
                    Console.WriteLine("Enter the event number for rule_" + (i + 1) + " (exemple -> 1 for event_1) : ");
                    String eventNb = Console.ReadLine();
                    Console.WriteLine("Enter the number of actions : ");
                    String actionNb = Console.ReadLine();

                    try
                    {
                        eventNumber = Int32.Parse(eventNb);
                        numberOfActions = Int32.Parse(actionNb);
                        Event evt = new Event(eventNumber);
                        List<Action> a = new List<Action>();

                        for (int j = 0; j < numberOfActions; j++)
                        {
                            Console.WriteLine("Enter the action number for event_" + eventNumber + " (exemple -> 1 for action_1) : ");
                            String action = Console.ReadLine();

                            try
                            {
                                actionNumber = Int32.Parse(action);
                                Action ac = new Action(actionNumber);
                                a.Add(ac);
                            }
                            catch (FormatException e)
                            {
                                Console.WriteLine(e.Message);
                            }
                        }

                        removeDuplicateActions(a);
                        Rule r = new Rule(evt, a);
                        rules.Add(r);
                    }
                    catch (FormatException e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }

            }
            catch (FormatException e)
            {
                Console.WriteLine(e.Message);
            }

            return rules;

        }

        private class Action
        {
            public String name;
            public int id;

            public Action(int id)
            {
                this.id = id;
                this.name = "action_" + id; 
            }

            public Action(String name, int id)
            {
                this.name = name;
                this.id = id;
            }

            public int compareTo(Action comparedAction)
            {
                return this.id.CompareTo(comparedAction.id);
            }

            public String toString()
            {
                return name;
            }
        }

        private class Event
        {
            public String name;
            public int id;

            public Event(int id)
            {
                this.id = id;
                this.name = "event_" + id;
            }

            public Event(String name, int id)
            {
                this.name = name;
                this.id = id;
            }

            public String toString()
            {
                return name;
            }
        }

        private class Rule
        {
            public Event e;
            public List<Action> actionList;

            public Rule(Event e, List<Action> actionList)
            {
                this.e = e;
                this.actionList = actionList;
            }

            public String toString()
            {
                String returnString = e.toString() + " = ";
                foreach (Action a in actionList) {
                    returnString += "[" + a.toString() + "]";
                }
                return returnString;
            }
        }
    
    }
}
