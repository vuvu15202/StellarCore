import { Subject } from 'rxjs';
class AgService {
    constructor() {
        this.sendToRender = new Subject();
        this.sendToEditor = new Subject();
    }
}
const agService = new AgService();
export { agService, AgService };