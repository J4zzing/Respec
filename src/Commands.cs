using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Extensions;

namespace MBRespec {
    public static class Commands
    {
        public static string Manual = String.Join("\n",
            "MANUAL (Before making any changes, backup your save first!)",
            "respec.help",
            "  Print this manual.",
            "",
            "respec.attribute <attribute name> [hero name]",
            "  Reset a attribute to zero, return those points as free.",
            "respec.focus_point <skill name> [hero name]",
            "  Reset focus point of a skill to zero, return those points as free.",
            "respec.all_attributes [hero name]",
            "respec.all_focus_points [hero name]",
            "respec.hero [hero name]",
            "  Reset all attributes and focus points of a hero.",
            "",
            "ARGUMENTS",
            "<attribute name> - Use the names in the ouput of respec.show_all_attributes to refer attributes, case insensetive.",
            "<skill name> - Use the names in the ouput of respec.show_all_skills to refer skills, case insensetive.",
            "[hero name] - Use the names in the output of respec.heroes to refer heroes, specify name with underscores or spaces. If omitted, commands applies to the player by default.",
            "  Note: Seems like non-english characters in Console can not display correctlly. If you are using a non-english language, use \"localization.change_language english\" then repec",
            "  You can also use the in-game option to change language, after you make changes, use the in-game option to change it back.",
            "",
            "EXAMPLES",
            "respec.all_attribute",
            "respec.all_focus_points joseph",
            "respec.attribute vigor Fetheir the ill-Starred",
            "respec.focus_point smithing",
            "respec.focus_point One_handed halgard",
            "==shorthand commands==",
            "You can use initial letters of a hero/attribute/skill name ONLY if there is no ambiguity, see examples below.",
            "respec.focus_point leardership fetheir",
            "respec.focus_point leader feth",
            "respec.focus_point l f",
            "",
            "Other commands not in a respec sense, this may helps some special need.",
            "respec.skill <skill name> [hero name]",
            "  Reset skill level, xp and focus points to zero, perks will be reset after the save is saved and reloaded.",
            "  visit the tournament master beforehand if this quirky behavior is unwated.",
            "  You can use respec.show_learnt_perks_for_skill to check whether the perks match the skill level.",
            "respec.all_skills [hero name]"
            // "respec.set_skill [skill name] [skill level] [hero name]",
            // "  Set skill to a desired level, I only use it to verify other command's result, use this command with care."
        );

        // Commands can only accept type List<string> as arg and can only be of return type string or void.
        [CommandLineFunctionality.CommandLineArgumentFunction("help", "respec")]
        public static string Help(List<string> arguments)
        {
            return Manual;
        }

        [CommandLineFunctionality.CommandLineArgumentFunction("show_all_attributes", "respec")]
        public static string saa(List<string> args) => CommandWrapper(ShowAllAttributes)(args);
        public static string ShowAllAttributes(List<string> arguments)
        {
            return Respec.showAllAttributes();
        }

        [CommandLineFunctionality.CommandLineArgumentFunction("show_all_skills", "respec")]
        public static string sas(List<string> args) => CommandWrapper(ShowAllSkills)(args);
        public static string ShowAllSkills(List<string> arguments)
        {
            return Respec.ShowAllSkills();
        }

        [CommandLineFunctionality.CommandLineArgumentFunction("show_heroes", "respec")]
        public static string spch(List<string> args) => CommandWrapper(ShowPlayerClanHeroes)(args);
        public static string ShowPlayerClanHeroes(List<string> arguments)
        {
            return Respec.ShowPlayerClanHeroes();
        }

        [CommandLineFunctionality.CommandLineArgumentFunction("show_learnt_perks_for_skill", "respec")]
        public static string slpfs(List<string> args) => CommandWrapper(ShowLearntPerksForSkill, 1)(args);
        // public static CommandLineFunction slpfs = CommandWrapper(ShowPerksForSkill, 1);
        public static string ShowLearntPerksForSkill(List<string> arguments)
        {
            var hero = GetHeroFromCommandLineArguments(arguments, 1);
            var skill = GetSkillByName(arguments[0]);
            return Respec.ShowLearntPerksForSkill(hero, skill);
        }

        [CommandLineFunctionality.CommandLineArgumentFunction("show_xp", "respec")]
        public static string _xp(List<string> args) => CommandWrapper(Xp)(args);
        public static string Xp(List<string> arguments)
        {
            Hero hero = GetHeroFromCommandLineArguments(arguments, 0);

            var heroD = hero.HeroDeveloper as PropertyOwnerF<PropertyObject>;
            var skillXp = Skills.All.Sum(skill => heroD.GetPropertyValue(skill));

            var xp = hero.HeroDeveloper.TotalXp;
            return String.Join("\n",
                    $"Character total xp: {xp}, total skill xp: {skillXp}",
                    $"Character heath points {hero.HitPoints}/{hero.MaxHitPoints}"
            );
        }

        [CommandLineFunctionality.CommandLineArgumentFunction("hero", "respec")]
        public static string rh(List<string> args) => CommandWrapper(RespecHero)(args);
        public static string RespecHero(List<string> arguments)
        {
            var hero = GetHeroFromCommandLineArguments(arguments, 0);
            return Respec.RespecHero(hero);
        }
        [CommandLineFunctionality.CommandLineArgumentFunction("attribute", "respec")]
        public static string ra(List<string> args) => CommandWrapper(RespecAttribute, 1)(args);
        public static string RespecAttribute(List<string> arguments)
        {
            var hero = GetHeroFromCommandLineArguments(arguments, 1);
            var attribute = GetAttributeByName(arguments[0]);
            return Respec.RespecAttribute(hero, attribute);
        }

        [CommandLineFunctionality.CommandLineArgumentFunction("all_attributes", "respec")]
        public static string raa(List<string> args) => CommandWrapper(RespecAllAttributes)(args);
        public static string RespecAllAttributes(List<string> arguments)
        {
            Hero hero = GetHeroFromCommandLineArguments(arguments, 0);
            return Respec.RespecAllAttributes(hero);
        }

        [CommandLineFunctionality.CommandLineArgumentFunction("focus_point", "respec")]
        public static string rfp(List<string> args) => CommandWrapper(RespecFocusPoint, 1)(args);
        public static string RespecFocusPoint(List<string> arguments)
        {
            var hero = GetHeroFromCommandLineArguments(arguments, 1);
            var skill = GetSkillByName(arguments[0]);
            return Respec.RespecFocusPoint(hero, skill);
        }

        [CommandLineFunctionality.CommandLineArgumentFunction("all_focus_points", "respec")]
        public static string rafp(List<string> args) => CommandWrapper(RespecAllFocusPoints)(args);
        public static string RespecAllFocusPoints(List<string> arguments)
        {
            Hero hero = GetHeroFromCommandLineArguments(arguments, 0);
            return Respec.RespecAllFocusPoints(hero);
        }

        [CommandLineFunctionality.CommandLineArgumentFunction("skill", "respec")]
        public static string rs(List<string> args) => CommandWrapper(RespecSkill, 1)(args);
        public static string RespecSkill(List<string> arguments)
        {
            var hero = GetHeroFromCommandLineArguments(arguments, 1);
            var skill = GetSkillByName(arguments[0]);
            return Respec.ResetSkill(hero, skill);
        }

        [CommandLineFunctionality.CommandLineArgumentFunction("all_skills", "respec")]
        public static string ras(List<string> args) => CommandWrapper(RespecAllSkills)(args);
        public static string RespecAllSkills(List<string> arguments)
        {
            Hero hero = GetHeroFromCommandLineArguments(arguments, 0);
            return Respec.ResetAllSkills(hero);
        }
#if DEBUG
        [CommandLineFunctionality.CommandLineArgumentFunction("set_skill", "respec")]
        public static string ss(List<string> args) => CommandWrapper(SetSkill, 2)(args);
        public static string SetSkill(List<string> arguments)
        {
            Hero hero = GetHeroFromCommandLineArguments(arguments, 2);
            var skill = GetSkillByName(arguments[0]);
            if (!Int32.TryParse(arguments[1], out int level)) throw new CommandException($"Can not parse skill level \"{arguments[1]}\".");
            return Respec.SetSkillLevel(hero, skill, level);
        }
#endif
        internal static void VerifyHero(Hero hero)
        {
            var name = hero.Name;
            if (hero.IsDead)
            {
                throw new CommandException($"Hero \"{name}\" is DEAD.");
            }
            if (hero.Clan != Clan.PlayerClan)
            {
                throw new CommandException($"Hero \"{name}\" is not in your clan/family.");
            }
            if (hero.IsChild)
            {
                throw new CommandException($"Hero \"{name}\" is only a child.");
            }
        }

        public static Hero GetHeroFromCommandLineArguments(List<string> arguments, int index)
        {
            // defaults to player character
            Hero hero = Hero.MainHero;
            string name = "player";
            if (arguments.Count > index)
            {
                name = arguments[index];
                // supports both hero name using underscores and spaces
                name = name.Replace("_", " ");
                if (arguments.Count - index > 1) name = String.Join(" ", arguments.GetRange(index, arguments.Count - index));
                // Beware of name conflicts, consider making a UI version.
                hero = Hero.MainHero.Clan.Heroes.SingleOrDefault(h => h.Name.ToString().StartsWith(name, StringComparison.InvariantCultureIgnoreCase));
            }
            if (hero == null) throw new CommandException($"Hero \"{name}\" not found.");

            VerifyHero(hero);
            return hero;
        }
        public static CharacterAttribute GetAttributeByName(string attributeName)
        {
            var attribute = Attributes.All.SingleOrDefault(attri => attri.StringId.StartsWith(attributeName, StringComparison.InvariantCultureIgnoreCase)
                                                                || attri.Name.ToString().StartsWith(attributeName, StringComparison.InvariantCultureIgnoreCase));
            if (attribute == null) throw new CommandException($"Attribute \"{attributeName}\" not found");
            return attribute;
        }

        public static SkillObject GetSkillByName(string skillName)
        {
            skillName.Replace("_", " ");
            var skill = Skills.All.SingleOrDefault(s => s.StringId.StartsWith(skillName, StringComparison.InvariantCultureIgnoreCase)
                                                    || s.Name.ToString().StartsWith(skillName, StringComparison.InvariantCultureIgnoreCase));
            if (skill == null) throw new CommandException($"Skill \"{skillName}\" not found.");
            return skill;
        }

        
        public static void CheckCurrentCampaign()
        {
            if (Campaign.Current == null) throw new CommandException("Campaign is null. You're probably in the main menu, load a save or start a new game.");
        }

        public delegate string CommandLineFunction(List<string> arguments);
        public static CommandLineFunction CommandWrapper(CommandLineFunction func, int SyntaxRequiredArguments = 0) {
            CommandLineFunction wrapper = (arguments) => {
                try
                {
                    CheckCurrentCampaign();
                    if (arguments.Count < SyntaxRequiredArguments) throw SyntaxErrorException;
                    return func(arguments);
                }
                catch (CommandException ex)
                {
                    return ex.Message;
                }
                catch (Exception ex) {
                    return UnexpectedException + ex.ToString();
                }
            };
            return wrapper;
        }

        public static string UnexpectedException = "Unexpected error. Load your save and inform this error to the author please.\n";
        public static CommandException SyntaxErrorException = new CommandException("Syntax Error. use \"respect.help\" command to show manual.");
        public static CommandException HeroNotFoundException = new CommandException("Hero not found.");
        public static CommandException AttributeNotFoundException = new CommandException("Attribute not found.");
        public static CommandException SkillNotFoundException = new CommandException("Skill not found.");

        public class CommandException : Exception
        {
            public CommandException(string message) : base(message) { }
        }
    }
}