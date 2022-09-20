using System;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Extensions;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.CampaignBehaviors;

namespace MBRespec {
    public static class Respec
    {
        public static string showAllAttributes()
        {
            return String.Join(", ", Attributes.All.Select(attribute => attribute.StringId));
        }

        public static string ShowAllSkills()
        {
            return String.Join(", ", Skills.All.Select(skill => skill.StringId));
        }

        public static string ShowPlayerClanHeroes()
        {
            return String.Join(", ", Clan.PlayerClan.Heroes);
        }

        public static string RespecAttribute(Hero hero, CharacterAttribute attribute)
        {
            var heroD = hero.HeroDeveloper;
            int points = hero.GetAttributeValue(attribute) - RewardedPointsFromPerks(hero, attribute);
            heroD.RemoveAttribute(attribute, points);
            heroD.UnspentAttributePoints += points;
            return "Success.";
        }

        public static string RespecAllAttributes(Hero hero)
        {
            foreach (var attribute in Attributes.All)
            {
                Respec.RespecAttribute(hero, attribute);
            }
            return "Success.";
        }

        public static string RespecFocusPoint(Hero hero, SkillObject skill)
        {
            var heroD = hero.HeroDeveloper;
            int points = heroD.GetFocus(skill) - RewardedPointsFromPerks(hero, skill);
            heroD.RemoveFocus(skill, points);
            heroD.UnspentFocusPoints += points;
            return "Success.";
        }

        public static string RespecAllFocusPoints(Hero hero)
        {
            foreach (var skill in Skills.All)
            {
                Respec.RespecFocusPoint(hero, skill);
            }
            return "Success.";
        }

        public static string RespecHero(Hero hero)
        {
            RespecAllAttributes(hero);
            RespecAllFocusPoints(hero);
            return "Success.";
        }

        // awkward, won't work for mods which change the bonus perks
        public static int RewardedPointsFromPerks(Hero hero, CharacterAttribute attribute)
        {
            int points = 0;
            if (attribute == DefaultCharacterAttributes.Vigor)
            {
                if (hero.GetPerkValue(DefaultPerks.Crafting.VigorousSmith)) points++;
                if (hero.GetPerkValue(DefaultPerks.Athletics.Strong)) points++;
            }
            else if (attribute == DefaultCharacterAttributes.Control)
            {
                if (hero.GetPerkValue(DefaultPerks.Crafting.StrongSmith)) points++;
                if (hero.GetPerkValue(DefaultPerks.Athletics.Steady)) points++;
            }
            else if (attribute == DefaultCharacterAttributes.Endurance)
            {
                if (hero.GetPerkValue(DefaultPerks.Crafting.EnduringSmith)) points++;
                if (hero.GetPerkValue(DefaultPerks.Athletics.HealthyCitizens)) points++;
            }
            return points;
        }

        public static int RewardedPointsFromPerks(Hero hero, SkillObject skill)
        {
            int points = 0;
            if (skill == DefaultSkills.OneHanded)
            {
                if (hero.GetPerkValue(DefaultPerks.Crafting.WeaponMasterSmith)) points++;
            }
            else if (skill == DefaultSkills.TwoHanded)
            {
                if (hero.GetPerkValue(DefaultPerks.Crafting.WeaponMasterSmith)) points++;
            }
            else if (skill == DefaultSkills.Throwing)
            {
                if (hero.GetPerkValue(DefaultPerks.Athletics.StrongArms)) points++;
            }
            return points;
        }

#if DEBUG
        public static string ShowLearntPerksForSkill(Hero hero, SkillObject skill)
        {
            var perksForSkill = PerkObject.All.Where(perk => perk.Skill == skill);
            var learntPerksForSkill = perksForSkill.Where(perk => hero.HeroDeveloper.GetPerkValue(perk) == true);
            return String.Join(", ", learntPerksForSkill);
        }

        public static string RespecPerk(Hero hero, SkillObject skill)
        {
            // var cb = Campaign.Current.GetCampaignBehavior<PerkRespecCampaignBehavior>();
            // Sadly this method used for tournament master is private.
            // cb.ClearPerksForSkill(hero, skill);
            return "Success.";
        }

        public static string RespecAllPerks(Hero hero)
        {
            foreach (var skill in Skills.All)
            {
                Respec.RespecPerk(hero, skill);
            }
            return "Success.";
        }

        public static string ResetSkill(Hero hero, SkillObject skill)
        {
            RespecFocusPoint(hero, skill);
            hero.HeroDeveloper.SetInitialSkillLevel(skill, 0);
            // Suppose to reset perks but the API is not accessable.
            // You can have negatigve attribute or focus point after this...
            foreach (var perk in PerkObject.All) {
                if (perk.Skill == skill) {
                    ErasePerkPermanentBonuses(hero, perk);
                }
            }
            return "Success.";
        }

        public static string ResetAllSkills(Hero hero)
        {
            foreach (var skill in Skills.All)
            {
                Respec.ResetSkill(hero, skill);
            }
            return "Success.";
        }

        public static string SetSkillLevel(Hero hero, SkillObject skill, int level)
        {
            RespecFocusPoint(hero, skill);
            hero.HeroDeveloper.SetInitialSkillLevel(skill, level);
            return "Success.";
        }

        public static void ErasePerkPermanentBonuses(Hero hero, PerkObject perk)
		{
			if (!hero.GetPerkValue(perk))
			{
				return;
			}
            // perks that add attribute
			if (perk == DefaultPerks.Crafting.VigorousSmith)
			{
				hero.HeroDeveloper.RemoveAttribute(DefaultCharacterAttributes.Vigor, 1);
				return;
			}
			if (perk == DefaultPerks.Crafting.StrongSmith)
			{
				hero.HeroDeveloper.RemoveAttribute(DefaultCharacterAttributes.Control, 1);
				return;
			}
			if (perk == DefaultPerks.Crafting.EnduringSmith)
			{
				hero.HeroDeveloper.RemoveAttribute(DefaultCharacterAttributes.Endurance, 1);
				return;
			}
			if (perk == DefaultPerks.Athletics.HealthyCitizens)
			{
				hero.HeroDeveloper.RemoveAttribute(DefaultCharacterAttributes.Endurance, 1);
				return;
			}
			if (perk == DefaultPerks.Athletics.Steady)
			{
				hero.HeroDeveloper.RemoveAttribute(DefaultCharacterAttributes.Control, 1);
				return;
			}
			if (perk == DefaultPerks.Athletics.Strong)
			{
				hero.HeroDeveloper.RemoveAttribute(DefaultCharacterAttributes.Vigor, 1);
				return;
			}
            // perks that add focus point(s)
			if (perk == DefaultPerks.Crafting.WeaponMasterSmith)
			{
				hero.HeroDeveloper.RemoveFocus(DefaultSkills.OneHanded, 1);
				hero.HeroDeveloper.RemoveFocus(DefaultSkills.TwoHanded, 1);
				return;
			}
			if (perk == DefaultPerks.Athletics.StrongArms)
			{
				hero.HeroDeveloper.RemoveFocus(DefaultSkills.Throwing, 1);
			}
		}
#endif
    }
}