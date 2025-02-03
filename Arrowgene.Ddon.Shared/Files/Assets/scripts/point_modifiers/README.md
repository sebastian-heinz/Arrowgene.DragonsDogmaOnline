# Point Modifiers

The scripts in this module define different types of point modifiers (such as exp, jp and pp).

A point modifier can have 2 types
- Base Modifier
- Bonus Modifier

Base modifiers are applied to the base point amount. Bonus modifiers are applied to the new base amount after all modifiers have been applied.

Each modifier also has 3 actions when calculating the final percentage
- Additive
- Multiplicative

The algorithm which will calculate the final modifier does as follows

1. Add together all `Additive` multipliers.
2. Apply all the `Multiplicative` multipliers.

## Guidelines

All point modifiers should be able to have their settings be updated by a hot load event while the server is running.

### Script naming

- Scripts which define modifications to the base amount should be `base_<point_type>_<name>.csx`
    - example `base_exp_globalmodifiers.csx`
    - example `base_exp_ptlvdiff.csx`
- Scripts which define a bonus should be named as `bonus_<point_type>_<name>.csx`.
    - example `bonus_exp_equipment.csx`.
    - example `bonus_exp_gpcourse.csx`.
    