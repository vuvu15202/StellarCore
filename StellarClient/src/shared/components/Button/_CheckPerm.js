import { ability } from 'shared/services';
export const CheckPerm=(perm)=>{
    if(Array.isArray(perm)){
        let res=false;
        perm.forEach(x=>{
            res=res||ability.can(x) || ability.can('admin');
        });
        return res;
    }
    return ability.can(perm);
};