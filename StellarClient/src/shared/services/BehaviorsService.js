import { Subject } from 'rxjs';
import { createContext } from 'react';
class BehaviorsService {
    constructor() {
        this.onShowAlert = new Subject();
        this.onHideAlert = new Subject();
        this.onShowConfirm = new Subject();
        this.onHideConfirm = new Subject();
        this.onShowSuccess = new Subject();
        this.onHideSuccess = new Subject();
    }
    alert(msg, opt) {
        opt = Object.assign({}, opt, { msg });

        return new Promise((resolve) => {
            const subscriptions = this.onHideAlert.subscribe(res => {
                subscriptions.unsubscribe();
                resolve(res);
            });
            this.onShowAlert.next(opt);
        });
    }
    confirm(msg, opt) {
        opt = Object.assign({}, opt, { msg });
        return new Promise((resolve) => {
            const subscriptions = this.onHideConfirm.subscribe(res => {
                subscriptions.unsubscribe();
                resolve(res);
            });
            this.onShowConfirm.next(opt);
        });
    }

    success(msg, opt) {
        opt = Object.assign({}, opt, { msg });

        return new Promise((resolve) => {
            const subscriptions = this.onHideSuccess.subscribe(res => {
                subscriptions.unsubscribe();
                resolve(res);
            });
            this.onShowSuccess.next(opt);
        });
    }
}
const beh = new BehaviorsService();
const BehaviorsContext = createContext(beh);

export { beh, BehaviorsContext, BehaviorsService };
