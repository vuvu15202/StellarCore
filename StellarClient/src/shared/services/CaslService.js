import {Ability, AbilityBuilder} from '@casl/ability';
import {createContext} from 'react';
import {createContextualCan} from '@casl/react';
import {useUserProfileStore} from "../stores/useUserProfileStore.js";

const ability = new Ability();

useUserProfileStore.subscribe((state) => {
    ability.update(defineRulesFor(state));
});

const defineRulesFor = (oauth) => {
    const permissions = oauth.permissions || [];
    const {can, rules} = new AbilityBuilder();

    if (permissions) {
        permissions.forEach((p) => {
            can(p);
        });
    }

    return rules;
};

const AbilityContext = createContext();
const Can = createContextualCan(AbilityContext.Consumer);
export {AbilityContext, ability, Can};