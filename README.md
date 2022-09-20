## What does it do
This mod is a continuation of Respec v2. It allows you to respec character's attribute and focus points, or reset skill xp in the console. It can not respec perks at the moment.This is my first mod, so good luck with that ; )

## Installation
1. Download and extract the zip file
2. Copy MBRespec folder to Modules folder. For example: C:/Steam/steamapps/common/Mount & Blade II Bannerlord/Modules/MBRespec 
3. Unblock dll if it is needed: Right click to dll then select Properties
4. Enable this mod in Mods tab in Launcher

## Usage (Before making any changes, backup your save first!)
Open the console by pressing ALT and ~ (tilde)
### Commands (Before making any changes, backup your save first!)
respec.attribute <attribute name> [hero name]  
  Reset a attribute to zero, return those points as free.  
respec.focus_point <skill name> [hero name]  
  Reset focus point of a skill to zero, return those points as free.  
respec.all_attributes [hero name]  
respec.all_focus_points [hero name]  
respec.hero [hero name]  
  Reset all attributes and focus points of a hero.  

### ARGUMENTS
<attribute name> - Use the names in the ouput of respec.show_all_attributes to refer attributes, case insensetive.  
<skill name> - Use the names in the ouput of respec.show_all_skills to refer skills, case insensetive.  
[hero name] - Use the names in the output of respec.heroes to refer heroes, specify name with underscores or spaces. If omitted, commands applies to the player by default.  Note: Seems like non-english characters in Console can not display correctlly. If you are using a non-english language, use \"localization.change_language english\" then repec.You can also use the in-game option to change language, after you make changes, use the in-game option to change it back.  

### EXAMPLES
respec.all_attribute  
respec.all_focus_points joseph  
respec.attribute vigor Fetheir the ill-Starred  
respec.focus_point smithing  
respec.focus_point One_handed halgard  
==shorthand commands==  
You can use initial letters of a hero/attribute/skill name ONLY if there is no ambiguity, see examples below.  
respec.focus_point leardership fetheir  
respec.focus_point leader feth  
respec.focus_point l f  

Other commands not in a respec sense, this may helps some special need.  
respec.skill <skill name> [hero name]  
Reset skill level, xp and focus points to zero, perks will be reset after the save is saved and reloaded.  
visit the tournament master beforehand if this quirky behavior is unwated.  
You can use respec.show_learnt_perks_for_skill to check whether the perks match the skill level.  
respec.all_skills [hero name]"  